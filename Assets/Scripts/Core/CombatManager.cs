using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Combat;

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

        [Header("전투 참여자")]
        [SerializeField] private Player player;
        [SerializeField] private List<Enemy> enemies = new List<Enemy>();

        public CombatState CurrentState => currentState;
        public Player Player => player;
        public List<Enemy> Enemies => enemies;

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

            // 플레이어 찾기 또는 생성
            if (player == null)
            {
                player = FindObjectOfType<Player>();
                if (player == null)
                {
                    GameObject playerObj = new GameObject("Player");
                    player = playerObj.AddComponent<Player>();
                }
            }
            player.Initialize();

            // 테스트용 적 생성 (씬에 적이 없으면)
            if (enemies.Count == 0)
            {
                Enemy[] foundEnemies = FindObjectsOfType<Enemy>();
                if (foundEnemies.Length == 0)
                {
                    // 테스트용: 산적 1마리 생성
                    GameObject enemyObj = new GameObject("Enemy_Bandit");
                    Enemy enemy = enemyObj.AddComponent<Enemy>();
                    enemy.Initialize("enemy_bandit");
                    enemies.Add(enemy);

                    // 적 사망 이벤트 구독
                    enemy.OnDeath += () => OnEnemyDeath(enemy);
                }
                else
                {
                    enemies.AddRange(foundEnemies);
                    foreach (var enemy in enemies)
                    {
                        enemy.OnDeath += () => OnEnemyDeath(enemy);
                    }
                }
            }

            // TODO: 덱 초기화
            // TODO: UI 초기화
        }

        /// <summary>
        /// 전투 중에 적을 소환합니다
        /// </summary>
        public void SpawnEnemy(string enemyId)
        {
            GameObject enemyObj = new GameObject($"Enemy_{enemyId}");
            Enemy enemy = enemyObj.AddComponent<Enemy>();
            enemy.Initialize(enemyId);
            enemies.Add(enemy);

            // 적 사망 이벤트 구독
            enemy.OnDeath += () => OnEnemyDeath(enemy);

            Debug.Log($"적 소환: {enemyId}");
        }

        /// <summary>
        /// 카드를 사용합니다
        /// </summary>
        public void PlayCard(Cards.Card card, Enemy target)
        {
            if (card == null) return;

            Debug.Log($"카드 사용: {card.Name}");

            // 무기술 경지 경험치 획득
            if (Progression.WeaponMasteryManager.Instance != null)
            {
                Progression.WeaponMasteryManager.Instance.AddMasteryXP(card.Data.weaponType, 1);
            }

            // 내공 경지 진행도 업데이트 (내공 소모 시)
            if (card.Cost > 0 && Progression.RealmManager.Instance != null)
            {
                Progression.RealmManager.Instance.UpdateProgress(Progression.RealmConditionType.UseEnergyCard, 1);
            }

            // TODO: 실제 카드 효과 적용 (CardEffectManager 등을 통해)
        }

        /// <summary>
        /// 플레이어 턴을 시작합니다
        /// </summary>

        /// <summary>
        /// 플레이어 턴을 시작합니다
        /// </summary>
        public void StartPlayerTurn()
        {
            ChangeState(CombatState.PlayerTurn);
            Debug.Log("--- 플레이어 턴 시작 ---");

            // 플레이어 턴 시작 처리
            if (player != null)
            {
                player.OnTurnStart();
            }

            // TODO: 카드 드로우 (DeckManager 구현 후)
        }

        /// <summary>
        /// 플레이어 턴을 종료합니다
        /// </summary>
        public void EndPlayerTurn()
        {
            Debug.Log("--- 플레이어 턴 종료 ---");

            // 플레이어 턴 종료 처리
            if (player != null)
            {
                player.OnTurnEnd();
            }

            // TODO: 손패 버리기 (DeckManager 구현 후)

            StartEnemyTurn();
        }

        /// <summary>
        /// 적 턴을 시작합니다
        /// </summary>
        public void StartEnemyTurn()
        {
            ChangeState(CombatState.EnemyTurn);
            Debug.Log("--- 적 턴 시작 ---");

            // 적 턴 시작 처리
            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive())
                {
                    enemy.OnTurnStart();
                }
            }

            // 적 행동 실행
            StartCoroutine(ExecuteEnemyActions());
        }

        /// <summary>
        /// 적들의 행동을 순서대로 실행합니다
        /// </summary>
        private IEnumerator ExecuteEnemyActions()
        {
            foreach (var enemy in enemies.ToArray())
            {
                if (enemy != null && enemy.IsAlive())
                {
                    // 적 의도 실행
                    enemy.ExecuteIntent(player);

                    // 행동 사이에 약간의 딜레이
                    yield return new WaitForSeconds(0.5f);
                }
            }

            // 적 턴 종료 처리
            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive())
                {
                    enemy.OnTurnEnd();
                }
            }

            yield return new WaitForSeconds(0.5f);

            // 전투 종료 조건 체크
            CheckBattleEnd();
        }

        /// <summary>
        /// 전투 종료 조건을 체크합니다
        /// </summary>
        private void CheckBattleEnd()
        {
            // 패배 조건 체크 (플레이어 체력 <= 0)
            if (player != null && player.CurrentHealth <= 0)
            {
                Defeat();
                return;
            }

            // 승리 조건 체크 (모든 적 격파)
            bool allEnemiesDead = true;
            foreach (var enemy in enemies)
            {
                if (enemy != null && enemy.IsAlive())
                {
                    allEnemiesDead = false;
                    break;
                }
            }

            if (allEnemiesDead && enemies.Count > 0)
            {
                Victory();
                return;
            }

            // 전투가 끝나지 않았다면 플레이어 턴으로
            StartPlayerTurn();
        }

        /// <summary>
        /// 적이 사망했을 때 호출됩니다
        /// </summary>
        private void OnEnemyDeath(Enemy enemy)
        {
            Debug.Log($"적 사망 처리: {enemy.EnemyData?.name}");
            // 전투 중이면 승리 조건 체크
            if (currentState == CombatState.PlayerTurn || currentState == CombatState.EnemyTurn)
            {
                // 다음 프레임에 체크 (현재 실행 중인 로직 완료 후)
                StartCoroutine(CheckBattleEndNextFrame());
            }
        }

        private IEnumerator CheckBattleEndNextFrame()
        {
            yield return null;
            CheckBattleEnd();
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
