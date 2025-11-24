using UnityEngine;
using System.Collections;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 전투 흐름을 제어하고 턴을 관리하는 매니저
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        private static CombatManager _instance;

        public static CombatManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CombatManager>();
                }
                return _instance;
            }
        }

        [Header("전투 상태")]
        [SerializeField] private CombatState currentState = CombatState.None;

        public CombatState CurrentState => currentState;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start()
        {
            // 테스트용: 게임 시작 시 자동으로 전투 시작
            StartCoroutine(StartCombatAfterDelay());
        }

        private IEnumerator StartCombatAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            StartCombat();
        }

        /// <summary>
        /// 전투를 시작합니다
        /// </summary>
        public void StartCombat()
        {
            Debug.Log("=== 전투 시작 ===");
            ChangeState(CombatState.Initializing);

            // 전투 초기화 로직
            InitializeCombat();

            // 플레이어 턴 시작
            StartPlayerTurn();
        }

        /// <summary>
        /// 전투를 초기화합니다
        /// </summary>
        private void InitializeCombat()
        {
            Debug.Log("전투 초기화 중...");
            // TODO: 플레이어 및 적 초기화
            // TODO: 덱 초기화
            // TODO: UI 초기화
        }

        /// <summary>
        /// 플레이어 턴을 시작합니다
        /// </summary>
        public void StartPlayerTurn()
        {
            ChangeState(CombatState.PlayerTurn);
            Debug.Log("--- 플레이어 턴 시작 ---");

            // TODO: 내공 리셋
            // TODO: 방어도 리셋
            // TODO: 카드 드로우
        }

        /// <summary>
        /// 플레이어 턴을 종료합니다
        /// </summary>
        public void EndPlayerTurn()
        {
            Debug.Log("--- 플레이어 턴 종료 ---");

            // TODO: 손패 버리기
            // TODO: 턴 종료 효과 발동

            StartEnemyTurn();
        }

        /// <summary>
        /// 적 턴을 시작합니다
        /// </summary>
        public void StartEnemyTurn()
        {
            ChangeState(CombatState.EnemyTurn);
            Debug.Log("--- 적 턴 시작 ---");

            // TODO: 적 행동 실행
            // TODO: 적 의도 업데이트

            // 테스트용: 적 턴 후 자동으로 플레이어 턴으로 전환
            StartCoroutine(EndEnemyTurnAfterDelay());
        }

        private IEnumerator EndEnemyTurnAfterDelay()
        {
            yield return new WaitForSeconds(1f);
            CheckBattleEnd();
        }

        /// <summary>
        /// 전투 종료 조건을 체크합니다
        /// </summary>
        private void CheckBattleEnd()
        {
            // TODO: 승리 조건 체크 (모든 적 격파)
            // TODO: 패배 조건 체크 (플레이어 체력 <= 0)

            // 전투가 끝나지 않았다면 플레이어 턴으로
            StartPlayerTurn();
        }

        /// <summary>
        /// 승리 처리
        /// </summary>
        public void Victory()
        {
            ChangeState(CombatState.Victory);
            Debug.Log("=== 승리! ===");

            // TODO: 승리 UI 표시
            // TODO: 보상 화면으로 전환
        }

        /// <summary>
        /// 패배 처리
        /// </summary>
        public void Defeat()
        {
            ChangeState(CombatState.Defeat);
            Debug.Log("=== 패배... ===");

            // TODO: 패배 UI 표시
            // TODO: 메인 메뉴로 돌아가기
        }

        /// <summary>
        /// 전투를 종료합니다
        /// </summary>
        public void EndCombat()
        {
            Debug.Log("=== 전투 종료 ===");
            ChangeState(CombatState.None);
        }

        /// <summary>
        /// 전투 상태를 변경합니다
        /// </summary>
        private void ChangeState(CombatState newState)
        {
            if (currentState == newState) return;

            CombatState oldState = currentState;
            currentState = newState;

            Debug.Log($"전투 상태 변경: {oldState} -> {newState}");
        }
    }
}
