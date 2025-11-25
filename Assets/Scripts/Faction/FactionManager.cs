using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Save;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.Faction
{
    /// <summary>
    /// 분파 시스템을 관리하는 매니저
    /// </summary>
    public class FactionManager : MonoBehaviour
    {
        private static FactionManager _instance;

        public static FactionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<FactionManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("FactionManager");
                        _instance = go.AddComponent<FactionManager>();
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, FactionData> factionDict;
        private FactionData currentFaction;

        // Events
        public System.Action<FactionData> OnFactionSelected;
        public System.Action<FactionPassiveType, int> OnPassiveTriggered;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadFactionDatabase();
        }

        /// <summary>
        /// 분파 데이터베이스를 로드합니다
        /// </summary>
        private void LoadFactionDatabase()
        {
            factionDict = new Dictionary<string, FactionData>();

            // 5개 분파 JSON 파일 로드
            string[] factionIds = { "hwasan", "shaolin", "beggars", "tang", "demonic" };

            foreach (string factionId in factionIds)
            {
                string path = $"Factions/{factionId}";
                TextAsset jsonFile = Resources.Load<TextAsset>(path);

                if (jsonFile == null)
                {
                    Debug.LogWarning($"[FactionManager] 분파 파일을 찾을 수 없습니다: {path}");
                    continue;
                }

                try
                {
                    FactionData faction = JsonUtility.FromJson<FactionData>(jsonFile.text);

                    if (faction != null && faction.IsValid())
                    {
                        factionDict[faction.id] = faction;
                        Debug.Log($"[FactionManager] 분파 로드 완료: {faction.name}");
                    }
                    else
                    {
                        Debug.LogError($"[FactionManager] 분파 데이터가 유효하지 않습니다: {factionId}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[FactionManager] 분파 로드 실패 ({factionId}): {e.Message}");
                }
            }

            Debug.Log($"[FactionManager] 총 {factionDict.Count}개 분파 로드 완료");
        }

        /// <summary>
        /// 분파를 선택합니다
        /// </summary>
        public bool SelectFaction(string factionId)
        {
            if (!factionDict.ContainsKey(factionId))
            {
                Debug.LogError($"[FactionManager] 존재하지 않는 분파입니다: {factionId}");
                return false;
            }

            currentFaction = factionDict[factionId];

            Debug.Log($"[FactionManager] 분파 선택: {currentFaction.name}");
            Debug.Log($"  - 패시브: {currentFaction.passive.description}");
            Debug.Log($"  - 시작 덱: {currentFaction.startingDeck.Count}장");
            Debug.Log($"  - 시작 유물: {currentFaction.startingRelic}");

            // SaveManager에 기록
            if (SaveManager.Instance != null && SaveManager.Instance.CurrentSaveData != null)
            {
                RunData runData = SaveManager.Instance.CurrentSaveData.currentRun;
                if (runData != null)
                {
                    // 분파 ID 저장 (RunData에 factionId 필드 추가 필요)
                    // runData.factionId = factionId;
                }
            }

            OnFactionSelected?.Invoke(currentFaction);

            return true;
        }

        /// <summary>
        /// 선택한 분파의 시작 덱을 적용합니다
        /// </summary>
        public List<string> GetStartingDeck()
        {
            if (currentFaction == null)
            {
                Debug.LogWarning("[FactionManager] 선택된 분파가 없습니다");
                return new List<string>();
            }

            // 시작 덱 복사 (원본 보호)
            List<string> deck = new List<string>(currentFaction.startingDeck);

            Debug.Log($"[FactionManager] {currentFaction.name} 시작 덱 반환: {deck.Count}장");

            return deck;
        }

        /// <summary>
        /// 선택한 분파의 시작 유물을 적용합니다
        /// </summary>
        public string GetStartingRelic()
        {
            if (currentFaction == null)
            {
                Debug.LogWarning("[FactionManager] 선택된 분파가 없습니다");
                return null;
            }

            Debug.Log($"[FactionManager] {currentFaction.name} 시작 유물 반환: {currentFaction.startingRelic}");

            return currentFaction.startingRelic;
        }

        /// <summary>
        /// 현재 분파의 패시브 능력을 가져옵니다
        /// </summary>
        public FactionPassive GetCurrentPassive()
        {
            return currentFaction?.passive;
        }

        /// <summary>
        /// 현재 선택된 분파를 가져옵니다
        /// </summary>
        public FactionData GetCurrentFaction()
        {
            return currentFaction;
        }

        /// <summary>
        /// 모든 분파 목록을 가져옵니다
        /// </summary>
        public List<FactionData> GetAllFactions()
        {
            return factionDict.Values.ToList();
        }

        /// <summary>
        /// ID로 분파 데이터를 가져옵니다
        /// </summary>
        public FactionData GetFactionById(string factionId)
        {
            if (factionDict.ContainsKey(factionId))
            {
                return factionDict[factionId];
            }

            Debug.LogWarning($"[FactionManager] 분파를 찾을 수 없습니다: {factionId}");
            return null;
        }

        // ========== 패시브 능력 처리 메서드 ==========

        /// <summary>
        /// 검술 카드 피해량 보너스 계산 (화산파 패시브)
        /// </summary>
        public int ApplySwordDamageBonus(int baseDamage)
        {
            if (currentFaction == null || currentFaction.passive.type != FactionPassiveType.SwordDamageBonus)
            {
                return baseDamage;
            }

            int bonusDamage = baseDamage * currentFaction.passive.value / 100;
            int totalDamage = baseDamage + bonusDamage;

            Debug.Log($"[FactionManager] 검술 피해 보너스: {baseDamage} -> {totalDamage} (+{currentFaction.passive.value}%)");
            OnPassiveTriggered?.Invoke(FactionPassiveType.SwordDamageBonus, bonusDamage);

            return totalDamage;
        }

        /// <summary>
        /// 권법 카드 사용 시 방어도 획득 (소림사 패시브)
        /// </summary>
        public void OnFistCardPlayed(Player player)
        {
            if (currentFaction == null || currentFaction.passive.type != FactionPassiveType.FistBlockGain)
            {
                return;
            }

            if (player != null)
            {
                player.GainBlock(currentFaction.passive.value);
                Debug.Log($"[FactionManager] 권법 방어도 획득: +{currentFaction.passive.value}");
                OnPassiveTriggered?.Invoke(FactionPassiveType.FistBlockGain, currentFaction.passive.value);
            }
        }

        /// <summary>
        /// 장법 카드 사용 시 골드 획득 (개방 패시브)
        /// </summary>
        public void OnPalmCardPlayed()
        {
            if (currentFaction == null || currentFaction.passive.type != FactionPassiveType.PalmGoldGain)
            {
                return;
            }

            // GoldManager 통합 필요
            Debug.Log($"[FactionManager] 장법 골드 획득: +{currentFaction.passive.value}");
            OnPassiveTriggered?.Invoke(FactionPassiveType.PalmGoldGain, currentFaction.passive.value);
        }

        /// <summary>
        /// 중독 효과 2배 적용 (당문 패시브)
        /// </summary>
        public int ApplyPoisonDoubleEffect(int basePoisonStacks)
        {
            if (currentFaction == null || currentFaction.passive.type != FactionPassiveType.PoisonDoubleEffect)
            {
                return basePoisonStacks;
            }

            int totalStacks = basePoisonStacks * 2;
            Debug.Log($"[FactionManager] 중독 효과 2배: {basePoisonStacks} -> {totalStacks}");
            OnPassiveTriggered?.Invoke(FactionPassiveType.PoisonDoubleEffect, basePoisonStacks);

            return totalStacks;
        }

        /// <summary>
        /// 체력 손실 시 내공 회복 (혈마신교 패시브)
        /// </summary>
        public void OnHealthLost(Player player, int healthLost)
        {
            if (currentFaction == null || currentFaction.passive.type != FactionPassiveType.HealthLossEnergyGain)
            {
                return;
            }

            if (player != null)
            {
                int energyGain = currentFaction.passive.value;
                player.GainEnergy(energyGain);
                Debug.Log($"[FactionManager] 체력 손실 시 내공 회복: +{energyGain}");
                OnPassiveTriggered?.Invoke(FactionPassiveType.HealthLossEnergyGain, energyGain);
            }
        }

        /// <summary>
        /// 분파 시스템을 초기화합니다
        /// </summary>
        public void ResetFaction()
        {
            currentFaction = null;
            Debug.Log("[FactionManager] 분파 시스템 초기화 완료");
        }
    }
}
