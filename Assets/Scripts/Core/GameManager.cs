using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using JianghuGuidebook.Save;
using JianghuGuidebook.Meta;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Economy;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Data;
using JianghuGuidebook.Achievement;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 게임의 메인 매니저 - 싱글톤 패턴으로 구현
    /// 전체 게임 루프와 상태를 관리합니다
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("새 게임 설정")]
        [SerializeField] private int baseStartingHealth = 70;
        [SerializeField] private int baseStartingGold = 100;
        [SerializeField] private int baseMaxEnergy = 3;
        [SerializeField] private List<string> baseStartingDeck = new List<string>
        {
            "card_strike", "card_strike", "card_strike", "card_strike",
            "card_iron_guard", "card_iron_guard", "card_iron_guard", "card_iron_guard",
            "card_qi_circulation", "card_clear_mind"
        };

        [Header("씬 설정")]
        [SerializeField] private string mapSceneName = "MapScene";
        [SerializeField] private string mainMenuSceneName = "MainMenuScene";

        [Header("런 포기 설정")]
        [SerializeField] [Range(0f, 1f)] private float abandonEssencePercentage = 0.5f; // 50%

        // Events
        public System.Action OnRunStarted;
        public System.Action OnRunCompleted;
        public System.Action OnRunAbandoned;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("GameManager 초기화 완료");
        }

        /// <summary>
        /// 새로운 런을 시작합니다
        /// </summary>
        public void StartNewRun()
        {
            Debug.Log("=== 새로운 런 시작 ===");

            // 1. 새로운 RunData 생성
            RunData newRun = new RunData();
            
            // 난이도 설정
            if (DifficultyManager.Instance != null)
            {
                newRun.difficultyLevel = DifficultyManager.Instance.SelectedDifficulty;
                Debug.Log($"난이도 적용: {newRun.difficultyLevel}");
            }

            // 2. 선택한 분파 적용 (있으면)
            ApplyFactionToRun(newRun);

            // 3. 메타 업그레이드 적용
            ApplyMetaUpgradesToRun(newRun);

            // 4. SaveManager에 RunData 설정
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.CurrentSaveData.currentRun = newRun;
                Debug.Log($"[GameManager] RunData 설정 완료: {newRun}");
            }

            // 5. 게임 시스템 초기화
            InitializeGameSystems(newRun);

            // 6. 이벤트 발생
            OnRunStarted?.Invoke();

            // 7. 맵 씬 로드
            Debug.Log($"[GameManager] 맵 씬 로드: {mapSceneName}");
            SceneManager.LoadScene(mapSceneName);
        }

        /// <summary>
        /// 선택한 분파를 새 런에 적용합니다
        /// </summary>
        private void ApplyFactionToRun(RunData runData)
        {
            Debug.Log("=== 분파 데이터 적용 시작 ===");

            // PlayerPrefs에서 선택된 분파 ID 가져오기
            string selectedFactionId = PlayerPrefs.GetString("SelectedFactionId", "");

            if (string.IsNullOrEmpty(selectedFactionId))
            {
                Debug.LogWarning("선택된 분파가 없습니다. 기본 분파(화산파) 사용");
                selectedFactionId = "faction_huashan";
            }

            // DataManager에서 분파 데이터 로드
            if (DataManager.Instance == null)
            {
                Debug.LogError("DataManager가 없습니다. 기본값 사용");
                return;
            }

            FactionData faction = DataManager.Instance.GetFactionById(selectedFactionId);

            if (faction == null)
            {
                Debug.LogError($"분파 데이터를 찾을 수 없습니다: {selectedFactionId}. 기본값 사용");
                return;
            }

            Debug.Log($"[분파 적용] {faction.name} ({faction.specialty})");

            // 분파 데이터를 RunData에 적용
            runData.currentHealth = faction.startingHealth;
            runData.maxHealth = faction.startingHealth;
            runData.currentGold = faction.startingGold;
            runData.deckCardIds = new List<string>(faction.startingDeck);
            runData.relicIds = new List<string>(faction.startingRelics);

            // 최대 내공 저장
            PlayerPrefs.SetInt("StartingMaxEnergy", faction.startingMaxEnergy);

            Debug.Log($"[분파 적용] 체력: {runData.currentHealth}/{runData.maxHealth}, " +
                     $"골드: {runData.currentGold}, 내공: {faction.startingMaxEnergy}, " +
                     $"덱: {runData.deckCardIds.Count}장, 유물: {runData.relicIds.Count}개");
        }

        /// <summary>
        /// 메타 업그레이드를 새 런에 적용합니다
        /// </summary>
        private void ApplyMetaUpgradesToRun(RunData runData)
        {
            Debug.Log("=== 메타 업그레이드 적용 시작 ===");

            if (MetaProgressionManager.Instance == null)
            {
                Debug.LogWarning("MetaProgressionManager가 없습니다. 메타 업그레이드 스킵");
                return;
            }

            // 현재 RunData의 값을 기준으로 업그레이드 적용 (분파 데이터가 이미 적용된 상태)
            int startingHealth = runData.maxHealth;
            int startingGold = runData.currentGold;
            int maxEnergy = PlayerPrefs.GetInt("StartingMaxEnergy", baseMaxEnergy);
            List<string> startingDeck = new List<string>(runData.deckCardIds);
            List<string> startingRelics = new List<string>(runData.relicIds);

            // 업그레이드 적용
            var upgrades = MetaProgressionManager.Instance.GetUnlockedUpgrades();

            foreach (var upgrade in upgrades)
            {
                switch (upgrade.type)
                {
                    case UpgradeType.IncreaseMaxHealth:
                        int healthBonus = upgrade.value * upgrade.timesPurchased;
                        startingHealth += healthBonus;
                        Debug.Log($"[메타] 최대 체력 +{healthBonus} → {startingHealth}");
                        break;

                    case UpgradeType.IncreaseStartingGold:
                        int goldBonus = upgrade.value * upgrade.timesPurchased;
                        startingGold += goldBonus;
                        Debug.Log($"[메타] 시작 골드 +{goldBonus} → {startingGold}");
                        break;

                    case UpgradeType.UnlockStartingCard:
                        if (!string.IsNullOrEmpty(upgrade.stringValue))
                        {
                            // 업그레이드한 횟수만큼 카드 추가
                            for (int i = 0; i < upgrade.timesPurchased; i++)
                            {
                                startingDeck.Add(upgrade.stringValue);
                            }
                            Debug.Log($"[메타] 시작 카드 추가: {upgrade.stringValue} x{upgrade.timesPurchased}");
                        }
                        break;

                    case UpgradeType.IncreaseMaxEnergy:
                        maxEnergy += upgrade.value * upgrade.timesPurchased;
                        Debug.Log($"[메타] 최대 내공 +{upgrade.value * upgrade.timesPurchased} → {maxEnergy}");
                        break;

                    case UpgradeType.StartWithRelic:
                        if (!string.IsNullOrEmpty(upgrade.stringValue) && !startingRelics.Contains(upgrade.stringValue))
                        {
                            startingRelics.Add(upgrade.stringValue);
                            Debug.Log($"[메타] 시작 유물: {upgrade.stringValue}");
                        }
                        break;
                }
            }

            // RunData에 적용
            runData.currentHealth = startingHealth;
            runData.maxHealth = startingHealth;
            runData.currentGold = startingGold;
            runData.deckCardIds = new List<string>(startingDeck);
            runData.relicIds = new List<string>(startingRelics);

            Debug.Log($"[메타] 최종 적용:");
            Debug.Log($"  - 체력: {runData.currentHealth}/{runData.maxHealth}");
            Debug.Log($"  - 골드: {runData.currentGold}");
            Debug.Log($"  - 최대 내공: {maxEnergy}");
            Debug.Log($"  - 덱: {runData.deckCardIds.Count}장");
            Debug.Log($"  - 유물: {runData.relicIds.Count}개");

            // 최대 내공은 Player 생성 시 적용되므로 별도 저장 필요
            PlayerPrefs.SetInt("StartingMaxEnergy", maxEnergy);
        }

        /// <summary>
        /// 기본값을 적용합니다 (메타 업그레이드 없을 때)
        /// </summary>
        private void ApplyBaseValues(RunData runData)
        {
            runData.currentHealth = baseStartingHealth;
            runData.maxHealth = baseStartingHealth;
            runData.currentGold = baseStartingGold;
            runData.deckCardIds = new List<string>(baseStartingDeck);
            runData.relicIds = new List<string>();

            PlayerPrefs.SetInt("StartingMaxEnergy", baseMaxEnergy);

            Debug.Log("[GameManager] 기본값 적용 완료");
        }

        /// <summary>
        /// 게임 시스템을 초기화합니다
        /// </summary>
        private void InitializeGameSystems(RunData runData)
        {
            Debug.Log("=== 게임 시스템 초기화 ===");

            // 골드 매니저 초기화
            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.SetGold(runData.currentGold);
                Debug.Log($"[GameManager] 골드 설정: {runData.currentGold}");
            }

            // 유물 매니저 초기화
            if (RelicManager.Instance != null)
            {
                RelicManager.Instance.ResetRelics();

                // 시작 유물 추가
                foreach (string relicId in runData.relicIds)
                {
                    Relic relic = DataManager.Instance?.GetRelicById(relicId);
                    if (relic != null)
                    {
                        RelicManager.Instance.AddRelic(relic);
                        Debug.Log($"[GameManager] 시작 유물 추가: {relic.name}");
                    }
                }
            }

            // 무공 정수는 메타 데이터에서 이미 로드됨
            Debug.Log($"[GameManager] 무공 정수: {MugongEssence.Instance?.CurrentEssence ?? 0}");
        }

        /// <summary>
        /// 런을 완료합니다 (승리/패배)
        /// </summary>
        public void CompleteRun(bool victory)
        {
            Debug.Log($"=== 런 완료: {(victory ? "승리" : "패배")} ===");

            if (SaveManager.Instance?.CurrentSaveData?.currentRun == null)
            {
                Debug.LogWarning("완료할 런 데이터가 없습니다");
                return;
            }

            RunData completedRun = SaveManager.Instance.CurrentSaveData.currentRun;

            // 무공 정수 계산 및 지급
            int essenceEarned = CalculateEssenceReward(completedRun, victory);
            if (MugongEssence.Instance != null)
            {
                MugongEssence.Instance.GainEssence(essenceEarned);
                Debug.Log($"[GameManager] 무공 정수 획득: {essenceEarned}");
            }

            // 메타 통계 업데이트
            UpdateMetaStatistics(completedRun, victory);

            // 업적 체크
            CheckRunAchievements(completedRun, victory);

            // 메타 데이터 저장
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.UpdateMetaDataFromGameState();
                SaveManager.Instance.SaveMetaData();
            }

            // 현재 런 데이터 제거 (런 종료)
            SaveManager.Instance.CurrentSaveData.currentRun = null;

            OnRunCompleted?.Invoke();

            Debug.Log("[GameManager] 런 완료 처리 완료");
        }

        private void CheckRunAchievements(RunData runData, bool victory)
        {
            if (AchievementManager.Instance == null) return;

            // 1. 전투 관련
            if (victory)
            {
                AchievementManager.Instance.UnlockAchievement("ach_combat_first_blood"); // 첫 승리 (단순화: 승리하면 무조건 달성)
                
                if (runData.currentHealth == 1)
                {
                    AchievementManager.Instance.UnlockAchievement("ach_combat_survival");
                }
            }

            // 2. 수집 관련
            if (runData.relicIds.Count >= 40)
            {
                AchievementManager.Instance.UnlockAchievement("ach_col_relic_hunter");
            }

            if (runData.currentGold >= 1000)
            {
                AchievementManager.Instance.UnlockAchievement("ach_col_rich");
            }

            if (runData.deckCardIds.Count >= 30)
            {
                AchievementManager.Instance.UnlockAchievement("ach_col_deck_master");
            }

            // 3. 특수
            if (victory)
            {
                if (runData.deckCardIds.Count <= 20)
                {
                    AchievementManager.Instance.UnlockAchievement("ach_spec_minimalist");
                }

                // 4. 난이도 관련
                if (runData.difficultyLevel >= DifficultyLevel.Master)
                {
                    AchievementManager.Instance.UnlockAchievement("ach_diff_master");
                }
                if (runData.difficultyLevel >= DifficultyLevel.Grandmaster)
                {
                    AchievementManager.Instance.UnlockAchievement("ach_diff_grandmaster");
                }
                if (runData.difficultyLevel >= DifficultyLevel.Supreme)
                {
                    AchievementManager.Instance.UnlockAchievement("ach_diff_supreme");
                }

                // 다음 난이도 해금
                if (DifficultyManager.Instance != null)
                {
                    DifficultyLevel nextLevel = runData.difficultyLevel + 1;
                    if (nextLevel <= DifficultyLevel.Supreme)
                    {
                        DifficultyManager.Instance.UnlockDifficulty(nextLevel);
                    }
                }
            }
        }

        /// <summary>
        /// 현재 런의 예상 무공 정수를 반환합니다 (포기 시 미리보기용)
        /// </summary>
        public int GetEstimatedEssence(bool includeVictoryBonus = false)
        {
            if (SaveManager.Instance?.CurrentSaveData?.currentRun == null)
            {
                return 0;
            }

            RunData currentRun = SaveManager.Instance.CurrentSaveData.currentRun;
            return CalculateEssenceReward(currentRun, includeVictoryBonus);
        }

        /// <summary>
        /// 포기 시 획득 가능한 무공 정수를 반환합니다
        /// </summary>
        public int GetAbandonEssence()
        {
            int fullEssence = GetEstimatedEssence(false);
            return Mathf.FloorToInt(fullEssence * abandonEssencePercentage);
        }

        /// <summary>
        /// 무공 정수 보상을 계산합니다
        /// </summary>
        private int CalculateEssenceReward(RunData runData, bool victory)
        {
            int essence = 0;

            // 기본: 도달한 지역 × 10
            essence += runData.regionsCompleted * 10;

            // 처치한 적 수 × 2
            essence += runData.enemiesKilled * 2;

            // 수집한 카드 수 × 1
            essence += runData.cardsCollected * 1;

            // 획득한 유물 수 × 5
            essence += runData.relicIds.Count * 5;

            // 보스 격파 × 20
            essence += runData.bossesDefeated * 20;

            // 승리 시 보너스
            if (victory)
            {
                essence += 50;
            }

            Debug.Log($"[정수 계산] 지역: {runData.regionsCompleted * 10}, 적: {runData.enemiesKilled * 2}, " +
                     $"카드: {runData.cardsCollected}, 유물: {runData.relicIds.Count * 5}, " +
                     $"보스: {runData.bossesDefeated * 20}, 승리보너스: {(victory ? 50 : 0)} = 총 {essence}");

            return essence;
        }

        /// <summary>
        /// 메타 통계를 업데이트합니다
        /// </summary>
        private void UpdateMetaStatistics(RunData runData, bool victory)
        {
            if (SaveManager.Instance?.CurrentSaveData?.metaData == null)
            {
                Debug.LogWarning("메타 데이터가 없습니다");
                return;
            }

            MetaSaveData meta = SaveManager.Instance.CurrentSaveData.metaData;

            meta.totalRunsCompleted++;
            if (victory)
            {
                meta.totalVictories++;
            }
            else
            {
                meta.totalDeaths++;
            }

            meta.totalEnemiesKilled += runData.enemiesKilled;
            meta.totalEnemiesKilled += runData.enemiesKilled;
            meta.totalBossesDefeated += runData.bossesDefeated;
            meta.totalPlayTime += runData.playTime;

            Debug.Log($"[메타 통계] 완료: {meta.totalRunsCompleted}, 승리: {meta.totalVictories}, " +
                     $"사망: {meta.totalDeaths}, 적: {meta.totalEnemiesKilled}, 보스: {meta.totalBossesDefeated}, " +
                     $"시간: {meta.totalPlayTime:F1}초");
        }

        /// <summary>
        /// 런을 포기합니다 (무공 정수 50% 획득)
        /// </summary>
        public void AbandonRun()
        {
            Debug.Log("=== 런 포기 ===");

            if (SaveManager.Instance?.CurrentSaveData?.currentRun == null)
            {
                Debug.LogWarning("포기할 런 데이터가 없습니다");
                return;
            }

            RunData abandonedRun = SaveManager.Instance.CurrentSaveData.currentRun;

            // 무공 정수 계산 (50%)
            int fullEssence = CalculateEssenceReward(abandonedRun, false); // 승리 보너스 없음
            int essenceEarned = Mathf.FloorToInt(fullEssence * abandonEssencePercentage);

            if (MugongEssence.Instance != null)
            {
                MugongEssence.Instance.GainEssence(essenceEarned);
                Debug.Log($"[GameManager] 무공 정수 획득 (포기): {essenceEarned} (원래: {fullEssence}, {abandonEssencePercentage * 100}%)");
            }

            // 메타 통계 업데이트 (포기도 사망으로 기록)
            UpdateMetaStatistics(abandonedRun, false);

            // 메타 데이터 저장
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.UpdateMetaDataFromGameState();
                SaveManager.Instance.SaveMetaData();
            }

            // 현재 런 데이터 제거 (런 종료)
            SaveManager.Instance.CurrentSaveData.currentRun = null;

            // 세이브 파일에서도 런 제거 (선택적)
            int currentSlot = PlayerPrefs.GetInt("CurrentSaveSlot", 0);
            SaveManager.Instance.SaveGame(currentSlot);

            OnRunAbandoned?.Invoke();

            Debug.Log("[GameManager] 런 포기 처리 완료");

            // 메인 메뉴로 복귀
            ReturnToMainMenu();
        }

        /// <summary>
        /// 메인 메뉴로 돌아갑니다
        /// </summary>
        public void ReturnToMainMenu()
        {
            Debug.Log("메인 메뉴로 복귀");

            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
            else
            {
                Debug.LogError("메인 메뉴 씬 이름이 설정되지 않았습니다");
            }
        }

        public void QuitGame()
        {
            Debug.Log("게임 종료");
            Application.Quit();
        }
    }
}
