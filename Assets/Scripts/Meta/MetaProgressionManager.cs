using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.Meta
{
    /// <summary>
    /// 메타 진행 시스템을 관리하는 매니저
    /// 영구 업그레이드 구매 및 적용을 담당합니다
    /// </summary>
    public class MetaProgressionManager : MonoBehaviour
    {
        private static MetaProgressionManager _instance;

        public static MetaProgressionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MetaProgressionManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("MetaProgressionManager");
                        _instance = go.AddComponent<MetaProgressionManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("업그레이드 목록")]
        [SerializeField] private List<PermanentUpgrade> allUpgrades = new List<PermanentUpgrade>();
        [SerializeField] private List<PermanentUpgrade> unlockedUpgrades = new List<PermanentUpgrade>();

        // Properties
        public List<PermanentUpgrade> AllUpgrades => allUpgrades;
        public List<PermanentUpgrade> UnlockedUpgrades => unlockedUpgrades;

        // Events
        public System.Action<PermanentUpgrade> OnUpgradePurchased;
        public System.Action OnUpgradesLoaded;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadUpgrades();
        }

        /// <summary>
        /// 업그레이드 데이터를 로드합니다
        /// </summary>
        public void LoadUpgrades()
        {
            Debug.Log("=== 영구 업그레이드 로딩 시작 ===");

            // DataManager에서 업그레이드 데이터 로드
            PermanentUpgrade[] upgrades = DataManager.Instance.GetAllUpgrades();

            if (upgrades == null || upgrades.Length == 0)
            {
                Debug.LogError("업그레이드 데이터가 없습니다");
                return;
            }

            allUpgrades.Clear();
            allUpgrades.AddRange(upgrades);

            Debug.Log($"영구 업그레이드 로드 완료: {allUpgrades.Count}개");

            OnUpgradesLoaded?.Invoke();
        }

        /// <summary>
        /// 업그레이드를 구매합니다
        /// </summary>
        public bool PurchaseUpgrade(PermanentUpgrade upgrade)
        {
            if (upgrade == null)
            {
                Debug.LogError("구매할 업그레이드가 null입니다");
                return false;
            }

            // 이미 최대 횟수 구매했는지 확인
            if (!upgrade.CanPurchaseMore())
            {
                Debug.LogWarning($"{upgrade.name}은 이미 최대 횟수만큼 구매되었습니다");
                return false;
            }

            // 무공 정수 확인
            int cost = upgrade.GetCurrentCost();
            if (!MugongEssence.Instance.HasEnoughEssence(cost))
            {
                Debug.LogWarning($"무공 정수 부족: 필요 {cost}, 현재 {MugongEssence.Instance.CurrentEssence}");
                return false;
            }

            // 정수 소비
            if (!MugongEssence.Instance.SpendEssence(cost))
            {
                return false;
            }

            // 업그레이드 해금
            upgrade.Unlock();

            // 해금된 업그레이드 목록에 추가 (중복 방지)
            if (!unlockedUpgrades.Contains(upgrade))
            {
                unlockedUpgrades.Add(upgrade);
            }

            Debug.Log($"업그레이드 구매 완료: {upgrade}");

            OnUpgradePurchased?.Invoke(upgrade);

            return true;
        }

        /// <summary>
        /// ID로 업그레이드를 찾습니다
        /// </summary>
        public PermanentUpgrade GetUpgradeById(string upgradeId)
        {
            return allUpgrades.FirstOrDefault(u => u.id == upgradeId);
        }

        /// <summary>
        /// 특정 타입의 업그레이드를 모두 찾습니다
        /// </summary>
        public List<PermanentUpgrade> GetUpgradesByType(UpgradeType type)
        {
            return allUpgrades.Where(u => u.type == type).ToList();
        }

        /// <summary>
        /// 해금된 업그레이드 중 특정 타입만 찾습니다
        /// </summary>
        public List<PermanentUpgrade> GetUnlockedUpgradesByType(UpgradeType type)
        {
            return unlockedUpgrades.Where(u => u.type == type).ToList();
        }

        /// <summary>
        /// 모든 해금된 업그레이드 목록을 반환합니다
        /// </summary>
        public List<PermanentUpgrade> GetUnlockedUpgrades()
        {
            return new List<PermanentUpgrade>(unlockedUpgrades);
        }

        /// <summary>
        /// 게임 시작 시 해금된 업그레이드를 플레이어에 적용합니다
        /// </summary>
        public void ApplyUpgradesToPlayer(Player player)
        {
            Debug.Log($"=== 플레이어에 업그레이드 적용: {unlockedUpgrades.Count}개 ===");

            foreach (var upgrade in unlockedUpgrades)
            {
                ApplyUpgrade(upgrade, player);
            }
        }

        /// <summary>
        /// 개별 업그레이드를 플레이어에 적용합니다
        /// </summary>
        private void ApplyUpgrade(PermanentUpgrade upgrade, Player player)
        {
            switch (upgrade.type)
            {
                case UpgradeType.IncreaseMaxHealth:
                    // 최대 체력 증가 (구매 횟수만큼)
                    int healthIncrease = upgrade.value * upgrade.timesPurchased;
                    player.IncreaseMaxHealth(healthIncrease);
                    Debug.Log($"업그레이드 적용: 최대 체력 +{healthIncrease}");
                    break;

                case UpgradeType.IncreaseStartingGold:
                    // 시작 골드 증가
                    if (Economy.GoldManager.Instance != null)
                    {
                        int goldAmount = upgrade.value * upgrade.timesPurchased;
                        Economy.GoldManager.Instance.AddGold(goldAmount);
                        Debug.Log($"업그레이드 적용: 시작 골드 +{goldAmount}");
                    }
                    break;

                case UpgradeType.UnlockStartingCard:
                    // 시작 카드 해금 (DeckManager에 추가)
                    if (DeckManager.Instance != null && !string.IsNullOrEmpty(upgrade.stringValue))
                    {
                        // DataManager에서 카드 데이터 가져오기
                        var cardData = DataManager.Instance.GetCardById(upgrade.stringValue);
                        if (cardData != null)
                        {
                            DeckManager.Instance.AddCardToDeck(cardData);
                            Debug.Log($"업그레이드 적용: 시작 카드 추가 ({cardData.name})");
                        }
                    }
                    break;

                case UpgradeType.IncreaseMaxEnergy:
                    // 최대 내공 증가
                    player.IncreaseMaxEnergy(upgrade.value * upgrade.timesPurchased);
                    Debug.Log($"업그레이드 적용: 최대 내공 +{upgrade.value * upgrade.timesPurchased}");
                    break;

                case UpgradeType.StartWithRelic:
                    // 시작 유물 (RelicManager에서 처리)
                    if (Relics.RelicManager.Instance != null && !string.IsNullOrEmpty(upgrade.stringValue))
                    {
                        Relics.RelicManager.Instance.AddRelic(upgrade.stringValue);
                        Debug.Log($"업그레이드 적용: 시작 유물 ({upgrade.stringValue})");
                    }
                    break;

                case UpgradeType.IncreaseCardRewards:
                    // 카드 보상 개수 증가 (RewardManager에서 처리)
                    // RewardManager는 런타임에 값을 참조하도록 수정 필요
                    Debug.Log($"업그레이드 적용: 카드 보상 +{upgrade.value} (RewardManager에서 참조)");
                    break;

                case UpgradeType.IncreaseGoldRewards:
                    // 골드 보상 증가 (GoldManager에서 처리)
                    Debug.Log($"업그레이드 적용: 골드 보상 +{upgrade.value}% (GoldManager에서 참조)");
                    break;

                case UpgradeType.ReduceShopPrices:
                    // 상점 가격 할인 (ShopManager에서 처리)
                    Debug.Log($"업그레이드 적용: 상점 가격 -{upgrade.value}% (ShopManager에서 참조)");
                    break;
            }
        }

        /// <summary>
        /// 특정 타입의 업그레이드 효과 합계를 반환합니다
        /// </summary>
        public int GetTotalUpgradeValue(UpgradeType type)
        {
            int total = 0;
            foreach (var upgrade in unlockedUpgrades.Where(u => u.type == type))
            {
                total += upgrade.value * upgrade.timesPurchased;
            }
            return total;
        }

        /// <summary>
        /// 업그레이드 해금 상태를 설정합니다 (세이브/로드용)
        /// </summary>
        public void SetUpgradeUnlockState(string upgradeId, bool unlocked, int timesPurchased)
        {
            var upgrade = GetUpgradeById(upgradeId);
            if (upgrade != null)
            {
                upgrade.isUnlocked = unlocked;
                upgrade.timesPurchased = timesPurchased;

                if (unlocked && !unlockedUpgrades.Contains(upgrade))
                {
                    unlockedUpgrades.Add(upgrade);
                }
                else if (!unlocked && unlockedUpgrades.Contains(upgrade))
                {
                    unlockedUpgrades.Remove(upgrade);
                }
            }
        }

        /// <summary>
        /// 모든 업그레이드를 초기화합니다
        /// </summary>
        public void ResetAllUpgrades()
        {
            foreach (var upgrade in allUpgrades)
            {
                upgrade.isUnlocked = false;
                upgrade.timesPurchased = 0;
            }

            unlockedUpgrades.Clear();

            Debug.Log("모든 업그레이드 초기화 완료");
        }

        /// <summary>
        /// 현재 메타 진행 상황을 출력합니다
        /// </summary>
        public void PrintMetaProgress()
        {
            Debug.Log("=== 메타 진행 상황 ===");
            Debug.Log($"무공 정수: {MugongEssence.Instance.CurrentEssence} (누적: {MugongEssence.Instance.TotalEssence})");
            Debug.Log($"해금된 업그레이드: {unlockedUpgrades.Count}/{allUpgrades.Count}");

            foreach (var upgrade in unlockedUpgrades)
            {
                Debug.Log($"  - {upgrade}");
            }
        }
    }
}
