using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        // Phase 14.1: 전체 게임 플로우
        [Test]
        public void 메인_메뉴에서_게임을_시작할_수_있다()
        {
            // Arrange
            var gameManagerObject = new GameObject("GameManager");
            var gameManager = gameManagerObject.AddComponent<GameManager>();

            // GameManager의 Awake 호출
            var awakeMethod = typeof(GameManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(gameManager, null);

            // Act
            var startMethod = typeof(GameManager).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            startMethod?.Invoke(gameManager, null);

            // Assert
            Assert.AreEqual(GameState.MainMenu, gameManager.CurrentState, "게임 시작 시 메인 메뉴 상태여야 합니다");
            Assert.IsNotNull(GameManager.Instance, "GameManager 싱글톤이 초기화되어야 합니다");

            Object.DestroyImmediate(gameManagerObject);
        }

        [Test]
        public void 문파를_선택하고_게임을_시작할_수_있다()
        {
            // Arrange
            var gameManagerObject = new GameObject("GameManager");
            var gameManager = gameManagerObject.AddComponent<GameManager>();

            var awakeMethod = typeof(GameManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(gameManager, null);

            var mapManagerObject = new GameObject("MapManager");
            var mapManager = mapManagerObject.AddComponent<MapManager>();
            var mapManagerAwake = typeof(MapManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mapManagerAwake?.Invoke(mapManager, null);

            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();
            var metaManagerAwake = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            metaManagerAwake?.Invoke(metaManager, null);

            var saveLoadManagerObject = new GameObject("SaveLoadManager");
            var saveLoadManager = saveLoadManagerObject.AddComponent<SaveLoadManager>();
            var saveLoadAwake = typeof(SaveLoadManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveLoadAwake?.Invoke(saveLoadManager, null);

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            // 문파 데이터 생성
            var hwasanFaction = ScriptableObject.CreateInstance<FactionData>();
            hwasanFaction.factionName = "화산파";
            hwasanFaction.startingDeck = new List<CardData>();

            for (int i = 0; i < 10; i++)
            {
                var card = ScriptableObject.CreateInstance<CardData>();
                card.cardName = $"시작 카드 {i}";
                hwasanFaction.startingDeck.Add(card);
            }

            // Act - 문파를 선택하고 게임 시작
            gameManager.StartNewGame(hwasanFaction);

            // Assert
            Assert.AreEqual(hwasanFaction, gameManager.SelectedFaction, "선택한 문파가 저장되어야 합니다");
            Assert.AreEqual(GameState.MapView, gameManager.CurrentState, "게임 시작 후 맵 뷰 상태여야 합니다");
            Assert.AreEqual(1, gameManager.currentFloor, "1층에서 게임이 시작되어야 합니다");
            Assert.IsNotNull(gameManager.PlayerDeck, "플레이어 덱이 초기화되어야 합니다");

            Object.DestroyImmediate(gameManagerObject);
            Object.DestroyImmediate(mapManagerObject);
            Object.DestroyImmediate(metaManagerObject);
            Object.DestroyImmediate(saveLoadManagerObject);
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 맵을_탐험하며_전투를_진행할_수_있다()
        {
            // Arrange
            var gameManagerObject = new GameObject("GameManager");
            var gameManager = gameManagerObject.AddComponent<GameManager>();

            var awakeMethod = typeof(GameManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(gameManager, null);

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;
            player.MaxNaegong = 3;

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 50;
            enemy.currentHealth = 50;

            // 전투 노드 데이터 생성
            var encounterData = ScriptableObject.CreateInstance<EncounterData>();
            encounterData.enemies = new List<EncounterData.EnemySpawn>
            {
                new EncounterData.EnemySpawn()
            };

            // Act - 전투 시작
            gameManager.StartBattle(encounterData);

            // Assert
            Assert.AreEqual(GameState.Battle, gameManager.CurrentState, "전투가 시작되면 Battle 상태여야 합니다");

            Object.DestroyImmediate(gameManagerObject);
            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 3층_보스를_격파하면_게임에서_승리한다()
        {
            // Arrange
            var gameManagerObject = new GameObject("GameManager");
            var gameManager = gameManagerObject.AddComponent<GameManager>();

            var awakeMethod = typeof(GameManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(gameManager, null);

            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();
            var metaManagerAwake = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            metaManagerAwake?.Invoke(metaManager, null);

            var saveLoadManagerObject = new GameObject("SaveLoadManager");
            var saveLoadManager = saveLoadManagerObject.AddComponent<SaveLoadManager>();
            var saveLoadAwake = typeof(SaveLoadManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveLoadAwake?.Invoke(saveLoadManager, null);

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            // 3층으로 설정
            var currentFloorField = typeof(GameManager).GetField("currentFloor",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            currentFloorField?.SetValue(gameManager, MapManager.FINAL_FLOOR); // 3층

            int enlightenmentBefore = metaManager.Progress.enlightenmentPoints;

            // Act - 보스 전투 승리
            gameManager.EndBattle(playerWon: true, wasBossFight: true);

            // Assert
            Assert.AreEqual(GameState.Victory, gameManager.CurrentState, "보스 격파 시 Victory 상태여야 합니다");
            Assert.Greater(metaManager.Progress.enlightenmentPoints, enlightenmentBefore,
                "승리 시 깨달음 포인트를 획득해야 합니다");

            Object.DestroyImmediate(gameManagerObject);
            Object.DestroyImmediate(metaManagerObject);
            Object.DestroyImmediate(saveLoadManagerObject);
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 사망_시_게임_오버가_된다()
        {
            // Arrange
            var gameManagerObject = new GameObject("GameManager");
            var gameManager = gameManagerObject.AddComponent<GameManager>();

            var awakeMethod = typeof(GameManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(gameManager, null);

            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();
            var metaManagerAwake = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            metaManagerAwake?.Invoke(metaManager, null);

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 0; // 사망

            var currentFloorField = typeof(GameManager).GetField("currentFloor",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            currentFloorField?.SetValue(gameManager, 2); // 2층에서 사망

            int enlightenmentBefore = metaManager.Progress.enlightenmentPoints;

            // Act - 전투 패배
            gameManager.EndBattle(playerWon: false, wasBossFight: false);

            // Assert
            Assert.AreEqual(GameState.GameOver, gameManager.CurrentState, "사망 시 GameOver 상태여야 합니다");
            Assert.Greater(metaManager.Progress.enlightenmentPoints, enlightenmentBefore,
                "패배 시에도 깨달음 포인트를 획득해야 합니다");

            Object.DestroyImmediate(gameManagerObject);
            Object.DestroyImmediate(metaManagerObject);
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 게임_종료_후_메타_진행이_저장된다()
        {
            // Arrange
            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();

            var awakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(metaManager, null);

            // Act - 깨달음 포인트 추가 및 저장
            int initialPoints = metaManager.Progress.enlightenmentPoints;
            metaManager.AddEnlightenmentPoints(100);
            metaManager.SaveProgress();

            // 새로운 MetaManager 생성하여 로드 테스트
            var newMetaManagerObject = new GameObject("NewMetaManager");
            var newMetaManager = newMetaManagerObject.AddComponent<MetaManager>();

            var newAwakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            newAwakeMethod?.Invoke(newMetaManager, null);

            // Assert
            Assert.AreEqual(initialPoints + 100, newMetaManager.Progress.enlightenmentPoints,
                "저장된 깨달음 포인트가 로드되어야 합니다");

            Object.DestroyImmediate(metaManagerObject);
            Object.DestroyImmediate(newMetaManagerObject);
        }

        [Test]
        public void 완전한_게임_플레이_시나리오를_실행할_수_있다()
        {
            // Arrange - 모든 주요 매니저 초기화
            var gameManagerObject = new GameObject("GameManager");
            var gameManager = gameManagerObject.AddComponent<GameManager>();

            var awakeMethod = typeof(GameManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(gameManager, null);

            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();
            var metaManagerAwake = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            metaManagerAwake?.Invoke(metaManager, null);

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            // 문파 선택
            var faction = ScriptableObject.CreateInstance<FactionData>();
            faction.factionName = "화산파";
            faction.startingDeck = new List<CardData>();

            for (int i = 0; i < 10; i++)
            {
                var card = ScriptableObject.CreateInstance<CardData>();
                card.cardName = $"카드 {i}";
                faction.startingDeck.Add(card);
            }

            // Act - 게임 플레이 시뮬레이션
            // 1. 게임 시작
            Assert.AreEqual(GameState.MainMenu, gameManager.CurrentState);

            // 2. 문파 선택 및 게임 시작 시뮬레이션
            gameManager.SelectedFaction = faction;
            gameManager.PlayerDeck = new List<CardData>(faction.startingDeck);

            // 3. 전투 승리 시뮬레이션
            player.maxHealth = 100;
            player.currentHealth = 80;

            var currentFloorField = typeof(GameManager).GetField("currentFloor",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            currentFloorField?.SetValue(gameManager, 1);

            gameManager.EndBattle(playerWon: true, wasBossFight: false);

            // 4. 카드 보상 화면 진입 확인
            Assert.AreEqual(GameState.Reward, gameManager.CurrentState, "전투 승리 후 보상 화면이 표시되어야 합니다");

            // Assert - 전체 흐름 검증
            Assert.IsNotNull(gameManager.SelectedFaction, "문파가 선택되어 있어야 합니다");
            Assert.IsNotNull(gameManager.PlayerDeck, "플레이어 덱이 존재해야 합니다");
            Assert.Greater(player.currentHealth, 0, "플레이어가 살아있어야 합니다");

            Object.DestroyImmediate(gameManagerObject);
            Object.DestroyImmediate(metaManagerObject);
            Object.DestroyImmediate(playerObject);
        }
    }
}
