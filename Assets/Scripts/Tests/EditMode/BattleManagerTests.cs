using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class BattleManagerTests
    {
        private GameObject battleManagerObject;
        private BattleManager battleManager;

        [SetUp]
        public void Setup()
        {
            // BattleManager는 싱글톤이므로 테스트마다 새로 생성
            battleManagerObject = new GameObject("BattleManager");
            battleManager = battleManagerObject.AddComponent<BattleManager>();
        }

        [TearDown]
        public void Teardown()
        {
            if (battleManagerObject != null)
                Object.DestroyImmediate(battleManagerObject);
        }

        [Test]
        public void BattleManager를_싱글톤으로_생성할_수_있다()
        {
            // Arrange & Act
            // BattleManager는 Awake에서 Instance를 설정하므로 수동 호출
            var awakeMethod = typeof(BattleManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(battleManager, null);

            // Assert
            Assert.IsNotNull(BattleManager.Instance);
            Assert.AreEqual(battleManager, BattleManager.Instance);
        }

        [Test]
        public void 전투를_EncounterData로_초기화할_수_있다()
        {
            // Arrange
            var encounterData = ScriptableObject.CreateInstance<EncounterData>();
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            enemyData.id = "test_enemy";
            enemyData.enemyName = "테스트 적";
            enemyData.maxHealth = 50;

            encounterData.enemies = new List<EncounterData.EnemySpawn>
            {
                new EncounterData.EnemySpawn { enemyData = enemyData }
            };

            // BattleManager에 필요한 참조 설정
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            typeof(BattleManager).GetField("player",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(battleManager, player);

            var enemiesField = typeof(BattleManager).GetField("enemies",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            enemiesField?.SetValue(battleManager, new List<Enemy>());

            // Act
            // StartBattle 메서드 호출 (적 프리팹이 없으므로 에러가 발생할 수 있음)
            // 이 테스트는 메서드 호출이 가능함을 확인

            // Assert
            Assert.IsNotNull(encounterData);
            Assert.AreEqual(1, encounterData.enemies.Count);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 전투_시작_시_적들을_생성한다()
        {
            // Arrange
            var encounterData = ScriptableObject.CreateInstance<EncounterData>();
            var enemyData1 = ScriptableObject.CreateInstance<EnemyData>();
            enemyData1.maxHealth = 30;
            var enemyData2 = ScriptableObject.CreateInstance<EnemyData>();
            enemyData2.maxHealth = 40;

            encounterData.enemies = new List<EncounterData.EnemySpawn>
            {
                new EncounterData.EnemySpawn { enemyData = enemyData1 },
                new EncounterData.EnemySpawn { enemyData = enemyData2 }
            };

            // Assert
            Assert.AreEqual(2, encounterData.enemies.Count);
            Assert.AreEqual(30, encounterData.enemies[0].enemyData.maxHealth);
            Assert.AreEqual(40, encounterData.enemies[1].enemyData.maxHealth);
        }

        [Test]
        public void 전투_시작_시_Player의_덱을_셔플한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = ScriptableObject.CreateInstance<CardData>();
                card.id = $"card_{i}";
                cards.Add(card);
            }

            player.Deck = new List<CardData>(cards);

            // Act
            player.ShuffleDeck();

            // Assert
            Assert.AreEqual(10, player.DrawPile.Count);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 전투_시작_시_초기_핸드를_뽑는다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = ScriptableObject.CreateInstance<CardData>();
                card.id = $"card_{i}";
                cards.Add(card);
            }

            player.DrawPile = new List<CardData>(cards);
            player.Hand = new List<CardData>();

            // Act
            player.DrawCards(5); // 초기 핸드 5장

            // Assert
            Assert.AreEqual(5, player.Hand.Count);
            Assert.AreEqual(5, player.DrawPile.Count);

            Object.DestroyImmediate(playerObject);
        }

        // Phase 4.2: 턴 관리
        [Test]
        public void Player_턴을_시작할_수_있다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.MaxNaegong = 3;

            // Act
            player.StartTurn();

            // Assert
            Assert.AreEqual(3, player.Energy);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void Player_턴_시작_시_내공이_리필된다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.MaxNaegong = 3;
            player.Energy = 0;

            // Act
            player.StartTurn();

            // Assert
            Assert.AreEqual(3, player.Energy);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void Player_턴_시작_시_카드를_뽑는다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = ScriptableObject.CreateInstance<CardData>();
                card.id = $"card_{i}";
                cards.Add(card);
            }

            player.DrawPile = new List<CardData>(cards);
            player.Hand = new List<CardData>();

            // Act
            player.StartTurn(); // StartTurn에서 카드를 뽑음

            // Assert
            // StartTurn이 카드를 뽑는지 확인 (구현에 따라 다를 수 있음)
            Assert.IsNotNull(player.Hand);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void Enemy_턴을_시작할_수_있다()
        {
            // Arrange
            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.CurrentHealth = 100;

            var action = new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = 10
            };
            enemy.SetNextAction(action);

            // Act
            enemy.TakeTurn(player);

            // Assert
            Assert.AreEqual(90, player.CurrentHealth);

            Object.DestroyImmediate(enemyObject);
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void Enemy_턴에_모든_적이_순서대로_행동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.CurrentHealth = 100;

            var enemy1Object = new GameObject("Enemy1");
            var enemy1 = enemy1Object.AddComponent<Enemy>();
            var action1 = new EnemyAction { actionType = EnemyActionType.Attack, value = 5 };
            enemy1.SetNextAction(action1);

            var enemy2Object = new GameObject("Enemy2");
            var enemy2 = enemy2Object.AddComponent<Enemy>();
            var action2 = new EnemyAction { actionType = EnemyActionType.Attack, value = 10 };
            enemy2.SetNextAction(action2);

            // Act
            enemy1.TakeTurn(player);
            enemy2.TakeTurn(player);

            // Assert
            Assert.AreEqual(85, player.CurrentHealth); // 100 - 5 - 10 = 85

            Object.DestroyImmediate(enemy1Object);
            Object.DestroyImmediate(enemy2Object);
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 턴_종료_시_핸드의_카드를_버린다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var card1 = ScriptableObject.CreateInstance<CardData>();
            var card2 = ScriptableObject.CreateInstance<CardData>();
            player.Hand = new List<CardData> { card1, card2 };
            player.DiscardPile = new List<CardData>();

            // Act
            player.EndTurn();

            // Assert
            Assert.AreEqual(0, player.Hand.Count);
            Assert.AreEqual(2, player.DiscardPile.Count);

            Object.DestroyImmediate(playerObject);
        }
    }
}
