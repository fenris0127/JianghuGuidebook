using UnityEngine;
using UnityEngine.SceneManagement;
using JianghuGuidebook.Save;
using JianghuGuidebook.Map;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 게임 전체 흐름을 조율하는 매니저
    /// 지역 전환, 보스 격파, 엔딩 등을 관리합니다
    /// </summary>
    public class GameFlowManager : MonoBehaviour
    {
        private static GameFlowManager _instance;

        public static GameFlowManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameFlowManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameFlowManager");
                        _instance = go.AddComponent<GameFlowManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("씬 설정")]
        [SerializeField] private string mainMenuSceneName = "MainMenuScene";
        [SerializeField] private string mapSceneName = "MapScene";
        [SerializeField] private string combatSceneName = "CombatScene";

        [Header("전환 설정")]
        [SerializeField] private float regionTransitionDelay = 2.0f;

        // 게임 상태
        private bool isGameActive = false;
        private bool isBossFight = false;

        // Properties
        public bool IsGameActive => isGameActive;
        public bool IsBossFight => isBossFight;

        // Events
        public System.Action OnGameStart;
        public System.Action OnGameEnd;
        public System.Action OnBossDefeated;
        public System.Action OnGameCleared;

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
            Debug.Log("[GameFlowManager] 초기화");

            // GameManager 이벤트 구독
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted += OnRunStarted;
                GameManager.Instance.OnRunCompleted += OnRunEnded;
                GameManager.Instance.OnRunAbandoned += OnRunEnded;
            }

            // RegionManager 이벤트 구독
            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.OnRegionComplete += OnRegionCompleted;
                RegionManager.Instance.OnRegionTransition += OnRegionTransition;
            }
        }

        /// <summary>
        /// 런이 시작되었을 때
        /// </summary>
        private void OnRunStarted()
        {
            Debug.Log("[GameFlowManager] 런 시작");
            isGameActive = true;
            OnGameStart?.Invoke();

            // 첫 지역 시작
            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.StartFirstRegion();
            }
        }

        /// <summary>
        /// 런이 종료되었을 때
        /// </summary>
        private void OnRunEnded()
        {
            Debug.Log("[GameFlowManager] 런 종료");
            isGameActive = false;
            OnGameEnd?.Invoke();
        }

        /// <summary>
        /// 지역이 완료되었을 때
        /// </summary>
        private void OnRegionCompleted(Region completedRegion)
        {
            Debug.Log($"[GameFlowManager] 지역 완료: {completedRegion.name}");

            // RunData 업데이트
            if (SaveManager.Instance?.CurrentSaveData?.currentRun != null)
            {
                SaveManager.Instance.CurrentSaveData.currentRun.regionsCompleted++;
                Debug.Log($"[GameFlowManager] 완료한 지역 수: {SaveManager.Instance.CurrentSaveData.currentRun.regionsCompleted}");
            }

            // 마지막 지역이었다면 게임 클리어
            if (RegionManager.Instance != null && RegionManager.Instance.IsLastRegion)
            {
                HandleGameClear();
            }
        }

        /// <summary>
        /// 지역 전환 시
        /// </summary>
        private void OnRegionTransition(Region previousRegion, Region nextRegion)
        {
            Debug.Log($"[GameFlowManager] 지역 전환: {previousRegion.name} → {nextRegion.name}");

            // RunData의 현재 지역 업데이트
            if (SaveManager.Instance?.CurrentSaveData?.currentRun != null)
            {
                SaveManager.Instance.CurrentSaveData.currentRun.currentRegionId = nextRegion.id;
            }

            // 지역 전환 UI 표시 (있다면)
            // TODO: RegionTransitionUI 호출
        }

        /// <summary>
        /// 보스 전투 시작
        /// </summary>
        public void StartBossFight()
        {
            Debug.Log("[GameFlowManager] 보스 전투 시작");
            isBossFight = true;
        }

        /// <summary>
        /// 보스 격파 처리
        /// </summary>
        public void HandleBossDefeated()
        {
            Debug.Log("[GameFlowManager] 보스 격파!");
            isBossFight = false;
            OnBossDefeated?.Invoke();

            // 현재 지역 완료 처리
            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.CompleteCurrentRegion();
            }
        }

        /// <summary>
        /// 일반 전투 승리 처리
        /// </summary>
        public void HandleCombatVictory()
        {
            Debug.Log("[GameFlowManager] 전투 승리");

            // 맵으로 복귀
            if (!string.IsNullOrEmpty(mapSceneName))
            {
                SceneManager.LoadScene(mapSceneName);
            }
        }

        /// <summary>
        /// 전투 패배 처리
        /// </summary>
        public void HandleCombatDefeat()
        {
            Debug.Log("[GameFlowManager] 전투 패배 - 런 종료");

            // 게임 매니저를 통해 런 완료 처리 (패배)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRun(false);
            }

            // 패배 화면 표시는 VictoryDefeatUI에서 처리
        }

        /// <summary>
        /// 게임 클리어 처리 (최종 보스 격파)
        /// </summary>
        private void HandleGameClear()
        {
            Debug.Log("[GameFlowManager] === 게임 클리어! ===");
            OnGameCleared?.Invoke();

            // 게임 매니저를 통해 런 완료 처리 (승리)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRun(true);
            }

            // 엔딩 화면은 VictoryDefeatUI에서 처리
        }

        /// <summary>
        /// 메인 메뉴로 복귀
        /// </summary>
        public void ReturnToMainMenu()
        {
            Debug.Log("[GameFlowManager] 메인 메뉴로 복귀");

            // 게임 상태 초기화
            isGameActive = false;
            isBossFight = false;

            // RegionManager 리셋
            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.ResetRegionManager();
            }

            // 메인 메뉴 씬 로드
            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
            else
            {
                Debug.LogError("메인 메뉴 씬 이름이 설정되지 않았습니다");
            }
        }

        /// <summary>
        /// 현재 런 포기
        /// </summary>
        public void AbandonCurrentRun()
        {
            Debug.Log("[GameFlowManager] 런 포기");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AbandonRun();
            }

            // 메인 메뉴로 복귀는 GameManager에서 처리
        }

        /// <summary>
        /// 현재 게임 상태 정보 출력
        /// </summary>
        public void PrintGameState()
        {
            Debug.Log("=== 게임 상태 정보 ===");
            Debug.Log($"게임 활성화: {isGameActive}");
            Debug.Log($"보스 전투: {isBossFight}");
            Debug.Log($"현재 씬: {SceneManager.GetActiveScene().name}");

            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.PrintCurrentRegionInfo();
            }

            if (SaveManager.Instance?.CurrentSaveData?.currentRun != null)
            {
                Debug.Log($"현재 런: {SaveManager.Instance.CurrentSaveData.currentRun}");
            }
            else
            {
                Debug.Log("활성 런 없음");
            }
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted -= OnRunStarted;
                GameManager.Instance.OnRunCompleted -= OnRunEnded;
                GameManager.Instance.OnRunAbandoned -= OnRunEnded;
            }

            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.OnRegionComplete -= OnRegionCompleted;
                RegionManager.Instance.OnRegionTransition -= OnRegionTransition;
            }
        }
    }
}
