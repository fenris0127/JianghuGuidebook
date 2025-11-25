using UnityEngine;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 테스트용 전투 씬을 자동으로 설정하는 스크립트
    /// </summary>
    public class CombatTestSceneSetup : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private string enemyIdToTest = "enemy_bandit";
        [SerializeField] private bool autoStart = true;
        [SerializeField] private bool enableDebugMode = true;

        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;

        [Header("Spawn Positions")]
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;

        private Player spawnedPlayer;
        private Enemy spawnedEnemy;
        private CombatManager combatManager;
        private DataManager dataManager;

        private void Start()
        {
            if (autoStart)
            {
                SetupTestCombat();
            }
        }

        /// <summary>
        /// 테스트 전투 설정
        /// </summary>
        [ContextMenu("Setup Test Combat")]
        public void SetupTestCombat()
        {
            Debug.Log($"[CombatTestSceneSetup] Setting up test combat with enemy: {enemyIdToTest}");

            // DataManager 초기화
            dataManager = FindObjectOfType<DataManager>();
            if (dataManager == null)
            {
                GameObject dataManagerObj = new GameObject("DataManager");
                dataManager = dataManagerObj.AddComponent<DataManager>();
            }

            // CombatManager 초기화
            combatManager = FindObjectOfType<CombatManager>();
            if (combatManager == null)
            {
                GameObject combatManagerObj = new GameObject("CombatManager");
                combatManager = combatManagerObj.AddComponent<CombatManager>();
            }

            // 기존 플레이어/적 제거
            CleanupExistingEntities();

            // 플레이어 생성
            SpawnPlayer();

            // 적 생성
            SpawnEnemy(enemyIdToTest);

            // 전투 시작
            if (combatManager != null)
            {
                Debug.Log("[CombatTestSceneSetup] Test combat setup complete!");
            }
        }

        /// <summary>
        /// 기존 엔티티 제거
        /// </summary>
        private void CleanupExistingEntities()
        {
            var existingPlayers = FindObjectsOfType<Player>();
            foreach (var player in existingPlayers)
            {
                DestroyImmediate(player.gameObject);
            }

            var existingEnemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in existingEnemies)
            {
                DestroyImmediate(enemy.gameObject);
            }
        }

        /// <summary>
        /// 플레이어 생성
        /// </summary>
        private void SpawnPlayer()
        {
            GameObject playerObj;

            if (playerPrefab != null)
            {
                playerObj = Instantiate(playerPrefab, GetPlayerSpawnPosition(), Quaternion.identity);
            }
            else
            {
                playerObj = new GameObject("Player");
                playerObj.transform.position = GetPlayerSpawnPosition();
            }

            spawnedPlayer = playerObj.GetComponent<Player>();
            if (spawnedPlayer == null)
            {
                spawnedPlayer = playerObj.AddComponent<Player>();
            }

            // 플레이어 초기화 (기본값)
            spawnedPlayer.Initialize(70, 3); // 70 HP, 3 에너지

            Debug.Log($"[CombatTestSceneSetup] Player spawned with {spawnedPlayer.MaxHealth} HP");
        }

        /// <summary>
        /// 적 생성
        /// </summary>
        private void SpawnEnemy(string enemyId)
        {
            // 적 데이터 로드
            EnemyData enemyData = dataManager.GetEnemyData(enemyId);

            if (enemyData == null)
            {
                Debug.LogError($"[CombatTestSceneSetup] Enemy data not found: {enemyId}");
                return;
            }

            GameObject enemyObj;

            if (enemyPrefab != null)
            {
                enemyObj = Instantiate(enemyPrefab, GetEnemySpawnPosition(), Quaternion.identity);
            }
            else
            {
                enemyObj = new GameObject($"Enemy_{enemyData.name}");
                enemyObj.transform.position = GetEnemySpawnPosition();
            }

            spawnedEnemy = enemyObj.GetComponent<Enemy>();
            if (spawnedEnemy == null)
            {
                spawnedEnemy = enemyObj.AddComponent<Enemy>();
            }

            // 적 초기화
            spawnedEnemy.Initialize(enemyData);

            Debug.Log($"[CombatTestSceneSetup] Enemy spawned: {enemyData.name} with {enemyData.maxHealth} HP");
        }

        /// <summary>
        /// 플레이어 스폰 위치
        /// </summary>
        private Vector3 GetPlayerSpawnPosition()
        {
            if (playerSpawnPoint != null)
            {
                return playerSpawnPoint.position;
            }
            return new Vector3(-3f, 0f, 0f);
        }

        /// <summary>
        /// 적 스폰 위치
        /// </summary>
        private Vector3 GetEnemySpawnPosition()
        {
            if (enemySpawnPoint != null)
            {
                return enemySpawnPoint.position;
            }
            return new Vector3(3f, 0f, 0f);
        }

        /// <summary>
        /// 특정 적으로 테스트 (에디터 메뉴)
        /// </summary>
        [ContextMenu("Test: Bandit")]
        public void TestBandit()
        {
            enemyIdToTest = "enemy_bandit";
            SetupTestCombat();
        }

        [ContextMenu("Test: Dark Cultivator")]
        public void TestDarkCultivator()
        {
            enemyIdToTest = "enemy_dark_cultivator";
            SetupTestCombat();
        }

        [ContextMenu("Test: Armored Guard")]
        public void TestArmoredGuard()
        {
            enemyIdToTest = "enemy_armored_guard";
            SetupTestCombat();
        }

        /// <summary>
        /// 현재 전투 상태 출력
        /// </summary>
        [ContextMenu("Print Combat Status")]
        public void PrintCombatStatus()
        {
            if (spawnedPlayer != null)
            {
                Debug.Log($"[Player] HP: {spawnedPlayer.CurrentHealth}/{spawnedPlayer.MaxHealth}, " +
                    $"Energy: {spawnedPlayer.CurrentEnergy}/{spawnedPlayer.MaxEnergy}, " +
                    $"Block: {spawnedPlayer.Block}");
            }

            if (spawnedEnemy != null)
            {
                Debug.Log($"[Enemy] {spawnedEnemy.EnemyName} HP: {spawnedEnemy.CurrentHealth}/{spawnedEnemy.MaxHealth}, " +
                    $"Block: {spawnedEnemy.Block}, " +
                    $"Intent: {spawnedEnemy.CurrentIntent?.Type} ({spawnedEnemy.CurrentIntent?.Value})");
            }
        }
    }
}
