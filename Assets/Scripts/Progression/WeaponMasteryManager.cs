using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Save;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 무기술 경지 시스템을 관리하는 매니저
    /// </summary>
    public class WeaponMasteryManager : MonoBehaviour
    {
        private static WeaponMasteryManager _instance;

        public static WeaponMasteryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<WeaponMasteryManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("WeaponMasteryManager");
                        _instance = go.AddComponent<WeaponMasteryManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("데이터베이스")]
        [SerializeField] private string databasePath = "WeaponMasteryDatabase";
        private WeaponMasteryDatabase database;

        [Header("진행 상황")]
        private Dictionary<WeaponType, WeaponMasteryProgress> progressDict = new Dictionary<WeaponType, WeaponMasteryProgress>();

        // 이벤트
        public System.Action<WeaponType, MasteryTier> OnMasteryAdvanced;
        public System.Action<WeaponType, MasteryTier, MasteryReward> OnRewardApplied;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeProgress();
        }

        private void Start()
        {
            LoadDatabase();
        }

        /// <summary>
        /// 진행 상황 초기화 (6개 무기술)
        /// </summary>
        private void InitializeProgress()
        {
            progressDict.Clear();

            foreach (WeaponType weaponType in System.Enum.GetValues(typeof(WeaponType)))
            {
                if (weaponType == WeaponType.None) continue;

                progressDict[weaponType] = new WeaponMasteryProgress(weaponType);
            }

            Debug.Log("[WeaponMasteryManager] 6개 무기술 경지 진행 상황 초기화 완료");
        }

        /// <summary>
        /// 데이터베이스 로드
        /// </summary>
        private void LoadDatabase()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(databasePath);

            if (jsonFile == null)
            {
                Debug.LogWarning($"[WeaponMasteryManager] 데이터베이스 파일을 찾을 수 없습니다: {databasePath}");
                return;
            }

            try
            {
                database = JsonUtility.FromJson<WeaponMasteryDatabase>(jsonFile.text);

                if (database != null && database.Validate())
                {
                    Debug.Log($"[WeaponMasteryManager] 데이터베이스 로드 성공: {database.masteries.Count}개 경지 데이터");
                }
                else
                {
                    Debug.LogError("[WeaponMasteryManager] 데이터베이스 검증 실패");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[WeaponMasteryManager] 데이터베이스 로드 실패: {e.Message}");
            }
        }

        /// <summary>
        /// 특정 무기술의 진행 상황 가져오기
        /// </summary>
        public WeaponMasteryProgress GetProgress(WeaponType weaponType)
        {
            if (progressDict.ContainsKey(weaponType))
            {
                return progressDict[weaponType];
            }

            Debug.LogWarning($"[WeaponMasteryManager] {weaponType}의 진행 상황을 찾을 수 없습니다");
            return null;
        }

        /// <summary>
        /// 모든 무기술의 진행 상황 가져오기
        /// </summary>
        public Dictionary<WeaponType, WeaponMasteryProgress> GetAllProgress()
        {
            return progressDict;
        }

        /// <summary>
        /// 카드 사용 시 호출 (전투 중)
        /// </summary>
        public void OnCardPlayed(string cardId, WeaponType cardWeaponType, int damageDealt, bool isSecretSkill)
        {
            // 모든 무기술에 대해 추적 (순수 빌드 체크를 위해)
            foreach (var progress in progressDict.Values)
            {
                progress.OnCardUsed(cardId, cardWeaponType, damageDealt, isSecretSkill);
            }

            Debug.Log($"[WeaponMasteryManager] 카드 사용 기록: {cardId} ({cardWeaponType}), 피해: {damageDealt}");
        }

        /// <summary>
        /// 전투 시작 시 호출
        /// </summary>
        public void OnCombatStart()
        {
            foreach (var progress in progressDict.Values)
            {
                progress.OnCombatStart();
            }

            Debug.Log("[WeaponMasteryManager] 전투 시작 - 무기술 추적 초기화");
        }

        /// <summary>
        /// 전투 종료 시 호출
        /// </summary>
        public void OnCombatEnd(bool victory)
        {
            foreach (var progress in progressDict.Values)
            {
                progress.OnCombatEnd(victory);
            }

            // 경지 상승 가능 여부 확인
            if (victory)
            {
                CheckForMasteryAdvancement();
            }

            Debug.Log($"[WeaponMasteryManager] 전투 종료 (승리: {victory})");
        }

        /// <summary>
        /// 덱 변경 시 호출 (카드 수 업데이트)
        /// </summary>
        public void OnDeckChanged(List<string> deckCardIds)
        {
            foreach (var progress in progressDict.Values)
            {
                progress.UpdateCardCount(deckCardIds);
            }

            // 카드 수 조건 체크
            CheckForMasteryAdvancement();

            Debug.Log("[WeaponMasteryManager] 덱 변경 - 카드 수 업데이트");
        }

        /// <summary>
        /// 경지 상승 가능 여부 확인 (모든 무기술)
        /// </summary>
        private void CheckForMasteryAdvancement()
        {
            if (database == null)
            {
                return;
            }

            foreach (var kvp in progressDict)
            {
                WeaponType weaponType = kvp.Key;
                WeaponMasteryProgress progress = kvp.Value;

                // 이미 최고 경지라면 스킵
                if (progress.currentTier >= MasteryTier.Master)
                {
                    continue;
                }

                // 다음 단계 경지 데이터 가져오기
                MasteryTier nextTier = progress.currentTier + 1;
                WeaponMasteryData nextMasteryData = database.GetMasteryData(weaponType, nextTier);

                if (nextMasteryData == null)
                {
                    continue;
                }

                // 조건 충족 확인
                if (nextMasteryData.condition.IsMet(progress))
                {
                    AdvanceMastery(weaponType, nextTier, nextMasteryData);
                }
            }
        }

        /// <summary>
        /// 경지 상승 처리
        /// </summary>
        private void AdvanceMastery(WeaponType weaponType, MasteryTier newTier, WeaponMasteryData masteryData)
        {
            WeaponMasteryProgress progress = progressDict[weaponType];

            Debug.Log($"=== [WeaponMasteryManager] {weaponType} 경지 돌파! ===");
            Debug.Log($"  {progress.currentTier} → {newTier} ({masteryData.displayName})");

            // 경지 상승
            progress.AdvanceTier();

            // 보상 적용
            if (masteryData.reward != null)
            {
                masteryData.reward.Apply(weaponType);
                OnRewardApplied?.Invoke(weaponType, newTier, masteryData.reward);
            }

            // 이벤트 발생
            OnMasteryAdvanced?.Invoke(weaponType, newTier);
        }

        /// <summary>
        /// 특정 무기술의 현재 경지 가져오기
        /// </summary>
        public MasteryTier GetCurrentTier(WeaponType weaponType)
        {
            if (progressDict.ContainsKey(weaponType))
            {
                return progressDict[weaponType].currentTier;
            }

            return MasteryTier.Beginner;
        }

        /// <summary>
        /// 특정 무기술의 피해 보너스 계산
        /// </summary>
        public float GetDamageBonus(WeaponType weaponType)
        {
            if (database == null || !progressDict.ContainsKey(weaponType))
            {
                return 0f;
            }

            WeaponMasteryProgress progress = progressDict[weaponType];
            float totalBonus = 0f;

            // 현재 경지까지의 모든 보너스 합산
            for (MasteryTier tier = MasteryTier.Minor; tier <= progress.currentTier; tier++)
            {
                WeaponMasteryData masteryData = database.GetMasteryData(weaponType, tier);
                if (masteryData != null && masteryData.reward != null)
                {
                    totalBonus += masteryData.reward.damageBonus;
                }
            }

            return totalBonus;
        }

        /// <summary>
        /// 특정 무기술의 비용 감소 계산
        /// </summary>
        public int GetCostReduction(WeaponType weaponType)
        {
            if (database == null || !progressDict.ContainsKey(weaponType))
            {
                return 0;
            }

            WeaponMasteryProgress progress = progressDict[weaponType];
            int totalReduction = 0;

            // 현재 경지까지의 모든 비용 감소 합산
            for (MasteryTier tier = MasteryTier.Minor; tier <= progress.currentTier; tier++)
            {
                WeaponMasteryData masteryData = database.GetMasteryData(weaponType, tier);
                if (masteryData != null && masteryData.reward != null)
                {
                    totalReduction += masteryData.reward.costReduction;
                }
            }

            return totalReduction;
        }

        /// <summary>
        /// 다음 경지 조건 텍스트 가져오기
        /// </summary>
        public string GetNextTierConditionText(WeaponType weaponType)
        {
            if (database == null || !progressDict.ContainsKey(weaponType))
            {
                return "데이터 없음";
            }

            WeaponMasteryProgress progress = progressDict[weaponType];

            // 이미 최고 경지라면
            if (progress.currentTier >= MasteryTier.Master)
            {
                return "최고 경지 도달";
            }

            // 다음 단계 경지 데이터 가져오기
            MasteryTier nextTier = progress.currentTier + 1;
            WeaponMasteryData nextMasteryData = database.GetMasteryData(weaponType, nextTier);

            if (nextMasteryData == null || nextMasteryData.condition == null)
            {
                return "조건 없음";
            }

            string conditionText = nextMasteryData.condition.GetDescription();
            string progressText = nextMasteryData.condition.GetProgressText(progress);

            return $"{conditionText} ({progressText})";
        }

        /// <summary>
        /// 세이브 데이터로 진행 상황 저장
        /// </summary>
        public void SaveToRunData(RunData runData)
        {
            if (runData == null)
            {
                Debug.LogWarning("[WeaponMasteryManager] RunData가 null입니다");
                return;
            }

            // TODO: RunData에 무기술 경지 필드 추가 필요
            // runData.weaponMasteryProgress = new Dictionary<string, WeaponMasteryProgressData>();

            Debug.Log("[WeaponMasteryManager] 무기술 경지 진행 상황 저장 (TODO: RunData 필드 추가 필요)");
        }

        /// <summary>
        /// 세이브 데이터에서 진행 상황 복원
        /// </summary>
        public void LoadFromRunData(RunData runData)
        {
            if (runData == null)
            {
                Debug.LogWarning("[WeaponMasteryManager] RunData가 null입니다");
                return;
            }

            // TODO: RunData에서 무기술 경지 복원

            Debug.Log("[WeaponMasteryManager] 무기술 경지 진행 상황 복원 (TODO: RunData 필드 추가 필요)");
        }

        /// <summary>
        /// 진행 상황 리셋 (새 게임 시작 시)
        /// </summary>
        public void ResetProgress()
        {
            InitializeProgress();
            Debug.Log("[WeaponMasteryManager] 무기술 경지 진행 상황 리셋");
        }

        /// <summary>
        /// 디버그 정보 출력
        /// </summary>
        [ContextMenu("Print Mastery Progress")]
        public void PrintMasteryProgress()
        {
            Debug.Log("=== 무기술 경지 진행 상황 ===");

            foreach (var kvp in progressDict)
            {
                WeaponType weaponType = kvp.Key;
                WeaponMasteryProgress progress = kvp.Value;

                Debug.Log($"{weaponType}:");
                Debug.Log($"  현재 경지: {TierDisplayNames.GetDisplayName(progress.currentTier, weaponType)}");
                Debug.Log($"  카드 보유: {progress.cardsOwned}장");
                Debug.Log($"  누적 피해: {progress.totalDamageDealt}");
                Debug.Log($"  순수 빌드 승리: {progress.pureBuildVictories}회");
                Debug.Log($"  비기 사용: {progress.secretSkillsUsed.Count}종");
                Debug.Log($"  피해 보너스: +{GetDamageBonus(weaponType)}%");
                Debug.Log($"  비용 감소: -{GetCostReduction(weaponType)}");
                Debug.Log("");
            }
        }

        /// <summary>
        /// 레거시 호환을 위한 XP 획득 메서드
        /// </summary>
        public void AddMasteryXP(WeaponType type, int amount)
        {
            // 레거시 XP 시스템 대신 카드 사용 시스템을 사용
            Debug.LogWarning($"[WeaponMasteryManager] AddMasteryXP는 구형 메서드입니다. OnCardPlayed를 사용하세요.");
        }

        /// <summary>
        /// 레거시 호환을 위한 경지 이름 반환
        /// </summary>
        public string GetMasteryName(WeaponType type)
        {
            if (!progressDict.ContainsKey(type))
            {
                return "입문";
            }

            WeaponMasteryProgress progress = progressDict[type];
            return TierDisplayNames.GetDisplayName(progress.currentTier, type);
        }
    }
}
