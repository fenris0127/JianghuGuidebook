using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Economy;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.Rewards
{
    /// <summary>
    /// 전투 보상 타입
    /// </summary>
    public enum RewardType
    {
        Card,       // 카드 선택
        Relic,      // 유물 획득
        Gold        // 골드 획득
    }

    /// <summary>
    /// 전투 타입
    /// </summary>
    public enum CombatType
    {
        Normal,     // 일반 전투
        Elite,      // 정예 전투
        Boss        // 보스 전투
    }

    /// <summary>
    /// 보상 아이템 클래스
    /// </summary>
    [System.Serializable]
    public class RewardItem
    {
        public RewardType type;
        public CardData cardData;       // 카드 보상일 경우
        public Relic relic;             // 유물 보상일 경우
        public int goldAmount;          // 골드 보상일 경우

        public RewardItem(RewardType type)
        {
            this.type = type;
        }

        public static RewardItem CreateCardReward(CardData card)
        {
            var reward = new RewardItem(RewardType.Card);
            reward.cardData = card;
            return reward;
        }

        public static RewardItem CreateRelicReward(Relic relic)
        {
            var reward = new RewardItem(RewardType.Relic);
            reward.relic = relic;
            return reward;
        }

        public static RewardItem CreateGoldReward(int amount)
        {
            var reward = new RewardItem(RewardType.Gold);
            reward.goldAmount = amount;
            return reward;
        }

        public override string ToString()
        {
            switch (type)
            {
                case RewardType.Card:
                    return $"카드: {cardData?.name ?? "없음"}";
                case RewardType.Relic:
                    return $"유물: {relic?.name ?? "없음"}";
                case RewardType.Gold:
                    return $"골드: {goldAmount}";
                default:
                    return "알 수 없는 보상";
            }
        }
    }

    /// <summary>
    /// 보상 시스템을 관리하는 매니저
    /// </summary>
    public class RewardManager : MonoBehaviour
    {
        private static RewardManager _instance;

        public static RewardManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RewardManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("RewardManager");
                        _instance = go.AddComponent<RewardManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("카드 보상 설정")]
        [SerializeField] private int normalCombatCardChoices = 3;       // 일반 전투: 3장 선택지
        [SerializeField] private int eliteCombatCardChoices = 3;        // 정예 전투: 3장 선택지
        [SerializeField] private int bossCombatCardChoices = 3;         // 보스 전투: 3장 선택지

        [Header("유물 보상 확률")]
        [SerializeField] private float eliteRelicChance = 0.3f;         // 정예 전투 유물 확률 30%
        [SerializeField] private float bossRelicChance = 1.0f;          // 보스 전투 유물 확률 100%

        [Header("골드 보상 범위")]
        [SerializeField] private int normalGoldMin = 40;
        [SerializeField] private int normalGoldMax = 60;
        [SerializeField] private int eliteGoldMin = 60;
        [SerializeField] private int eliteGoldMax = 90;
        [SerializeField] private int bossGoldMin = 200;
        [SerializeField] private int bossGoldMax = 300;

        private List<RewardItem> currentRewards = new List<RewardItem>();
        private List<CardData> currentCardChoices = new List<CardData>();

        // Properties
        public List<RewardItem> CurrentRewards => currentRewards;
        public List<CardData> CurrentCardChoices => currentCardChoices;

        // Events
        public System.Action<List<RewardItem>> OnRewardsGenerated;
        public System.Action<RewardItem> OnRewardClaimed;
        public System.Action OnRewardsCompleted;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        /// <summary>
        /// 전투 보상을 생성합니다
        /// </summary>
        public List<RewardItem> GenerateCombatRewards(CombatType combatType)
        {
            currentRewards.Clear();
            currentCardChoices.Clear();

            Debug.Log($"=== 보상 생성: {combatType} ===");

            // 1. 골드 보상
            int goldAmount = GenerateGoldReward(combatType);
            currentRewards.Add(RewardItem.CreateGoldReward(goldAmount));

            // 2. 카드 선택지 생성
            int cardChoiceCount = GetCardChoiceCount(combatType);
            currentCardChoices = GenerateCardChoices(cardChoiceCount, combatType);

            // 카드 보상은 선택 가능한 형태로 추가 (실제 카드는 선택 후 추가)
            currentRewards.Add(new RewardItem(RewardType.Card));

            // 3. 유물 보상 (확률 기반)
            if (ShouldGrantRelic(combatType))
            {
                Relic relic = GenerateRelicReward(combatType);
                if (relic != null)
                {
                    currentRewards.Add(RewardItem.CreateRelicReward(relic));
                }
            }

            Debug.Log($"보상 생성 완료: {currentRewards.Count}개");
            foreach (var reward in currentRewards)
            {
                Debug.Log($"  - {reward}");
            }

            OnRewardsGenerated?.Invoke(currentRewards);

            return currentRewards;
        }

        /// <summary>
        /// 골드 보상을 생성합니다
        /// </summary>
        private int GenerateGoldReward(CombatType combatType)
        {
            int min, max;

            switch (combatType)
            {
                case CombatType.Normal:
                    min = normalGoldMin;
                    max = normalGoldMax;
                    break;
                case CombatType.Elite:
                    min = eliteGoldMin;
                    max = eliteGoldMax;
                    break;
                case CombatType.Boss:
                    min = bossGoldMin;
                    max = bossGoldMax;
                    break;
                default:
                    min = normalGoldMin;
                    max = normalGoldMax;
                    break;
            }

            int goldAmount = Random.Range(min, max + 1);

            // 메타 업그레이드 적용 (골드 보상 증가)
            // TODO: MetaProgressionManager에서 골드 보상 증가 업그레이드 적용

            Debug.Log($"골드 보상: {goldAmount}");
            return goldAmount;
        }

        /// <summary>
        /// 카드 선택지를 생성합니다
        /// </summary>
        private List<CardData> GenerateCardChoices(int count, CombatType combatType)
        {
            List<CardData> choices = new List<CardData>();

            for (int i = 0; i < count; i++)
            {
                CardData card = GenerateRandomCard(combatType);
                if (card != null)
                {
                    choices.Add(card);
                }
            }

            Debug.Log($"카드 선택지 생성: {choices.Count}장");
            return choices;
        }

        /// <summary>
        /// 랜덤 카드를 생성합니다 (전투 타입에 따라 등급 가중치 조정)
        /// </summary>
        private CardData GenerateRandomCard(CombatType combatType)
        {
            CardData[] allCards = DataManager.Instance.GetAllCards();
            if (allCards.Length == 0)
            {
                Debug.LogError("카드 데이터가 없습니다");
                return null;
            }

            // 전투 타입에 따른 등급 가중치 설정
            CardRarity rarity = DetermineCardRarity(combatType);

            // 해당 등급의 카드 필터링
            CardData[] filteredCards = DataManager.Instance.GetCardsByRarity(rarity);

            if (filteredCards.Length == 0)
            {
                // 해당 등급이 없으면 전체에서 랜덤
                return allCards[Random.Range(0, allCards.Length)];
            }

            return filteredCards[Random.Range(0, filteredCards.Length)];
        }

        /// <summary>
        /// 전투 타입에 따라 카드 등급을 결정합니다
        /// </summary>
        private CardRarity DetermineCardRarity(CombatType combatType)
        {
            int roll = Random.Range(0, 100);

            switch (combatType)
            {
                case CombatType.Normal:
                    // Normal: Common 70%, Uncommon 25%, Rare 5%
                    if (roll < 70)
                        return CardRarity.Common;
                    else if (roll < 95)
                        return CardRarity.Uncommon;
                    else
                        return CardRarity.Rare;

                case CombatType.Elite:
                    // Elite: Common 40%, Uncommon 45%, Rare 15%
                    if (roll < 40)
                        return CardRarity.Common;
                    else if (roll < 85)
                        return CardRarity.Uncommon;
                    else
                        return CardRarity.Rare;

                case CombatType.Boss:
                    // Boss: Uncommon 50%, Rare 40%, Epic 10%
                    if (roll < 50)
                        return CardRarity.Uncommon;
                    else if (roll < 90)
                        return CardRarity.Rare;
                    else
                        return CardRarity.Epic;

                default:
                    return CardRarity.Common;
            }
        }

        /// <summary>
        /// 유물 보상을 지급할지 결정합니다
        /// </summary>
        private bool ShouldGrantRelic(CombatType combatType)
        {
            switch (combatType)
            {
                case CombatType.Normal:
                    return false; // 일반 전투는 유물 없음

                case CombatType.Elite:
                    return Random.value < eliteRelicChance;

                case CombatType.Boss:
                    return Random.value < bossRelicChance;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 유물 보상을 생성합니다
        /// </summary>
        private Relic GenerateRelicReward(CombatType combatType)
        {
            // TODO: DataManager에서 유물 데이터 로드 후 사용
            // 지금은 임시로 생성

            RelicRarity rarity = DetermineRelicRarity(combatType);

            Relic relic = new Relic(
                $"relic_{Random.Range(1000, 9999)}",
                "보상 유물",
                "전투 보상으로 획득한 유물입니다.",
                rarity,
                RelicEffectType.Passive
            );

            Debug.Log($"유물 보상 생성: {relic}");
            return relic;
        }

        /// <summary>
        /// 전투 타입에 따라 유물 등급을 결정합니다
        /// </summary>
        private RelicRarity DetermineRelicRarity(CombatType combatType)
        {
            int roll = Random.Range(0, 100);

            switch (combatType)
            {
                case CombatType.Elite:
                    // Elite: Common 60%, Uncommon 35%, Rare 5%
                    if (roll < 60)
                        return RelicRarity.Common;
                    else if (roll < 95)
                        return RelicRarity.Uncommon;
                    else
                        return RelicRarity.Rare;

                case CombatType.Boss:
                    // Boss: Uncommon 30%, Rare 50%, Legendary 20%
                    if (roll < 30)
                        return RelicRarity.Uncommon;
                    else if (roll < 80)
                        return RelicRarity.Rare;
                    else
                        return RelicRarity.Legendary;

                default:
                    return RelicRarity.Common;
            }
        }

        /// <summary>
        /// 카드 선택지 개수를 반환합니다
        /// </summary>
        private int GetCardChoiceCount(CombatType combatType)
        {
            switch (combatType)
            {
                case CombatType.Normal:
                    return normalCombatCardChoices;
                case CombatType.Elite:
                    return eliteCombatCardChoices;
                case CombatType.Boss:
                    return bossCombatCardChoices;
                default:
                    return 3;
            }
        }

        /// <summary>
        /// 보상을 획득합니다
        /// </summary>
        public void ClaimReward(RewardItem reward)
        {
            if (reward == null)
            {
                Debug.LogError("획득할 보상이 null입니다");
                return;
            }

            Debug.Log($"보상 획득: {reward}");

            switch (reward.type)
            {
                case RewardType.Card:
                    // 카드는 별도로 선택 후 처리
                    Debug.Log("카드 선택 UI 표시 필요");
                    break;

                case RewardType.Relic:
                    RelicManager.Instance.AddRelic(reward.relic);
                    break;

                case RewardType.Gold:
                    GoldManager.Instance.GainGold(reward.goldAmount);
                    break;
            }

            OnRewardClaimed?.Invoke(reward);
        }

        /// <summary>
        /// 카드를 선택합니다
        /// </summary>
        public void SelectCard(CardData card)
        {
            if (card == null)
            {
                Debug.LogWarning("카드 선택 건너뛰기");
                return;
            }

            Debug.Log($"카드 선택: {card.name}");

            // TODO: DeckManager에 카드 추가
            // DeckManager.Instance.AddCardToDeck(card);
        }

        /// <summary>
        /// 모든 보상을 자동으로 획득합니다 (골드, 유물)
        /// </summary>
        public void ClaimAllAutoRewards()
        {
            foreach (var reward in currentRewards)
            {
                if (reward.type != RewardType.Card) // 카드는 제외 (선택 필요)
                {
                    ClaimReward(reward);
                }
            }
        }

        /// <summary>
        /// 보상을 완료합니다
        /// </summary>
        public void CompleteRewards()
        {
            Debug.Log("모든 보상 획득 완료");
            currentRewards.Clear();
            currentCardChoices.Clear();

            OnRewardsCompleted?.Invoke();
        }

        /// <summary>
        /// 보상 시스템을 리셋합니다
        /// </summary>
        public void ResetRewards()
        {
            currentRewards.Clear();
            currentCardChoices.Clear();
            Debug.Log("보상 시스템 리셋 완료");
        }
    }
}
