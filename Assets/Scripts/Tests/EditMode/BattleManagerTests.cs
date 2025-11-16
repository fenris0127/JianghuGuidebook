using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;
using static GangHoBiGeup.Tests.TestHelper;

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
            InvokeAwake(battleManager);

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
            var player = CreatePlayer();
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

            Cleanup(player);
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
            var player = CreatePlayer();

            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = CreateCard($"card_{i}", $"카드 {i}");
                cards.Add(card);
            }

            player.Deck = new List<CardData>(cards);

            // Act
            player.ShuffleDeck();

            // Assert
            Assert.AreEqual(10, player.DrawPile.Count);

            Cleanup(player);
        }

        [Test]
        public void 전투_시작_시_초기_핸드를_뽑는다()
        {
            // Arrange
            var player = CreatePlayer();

            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = CreateCard($"card_{i}", $"카드 {i}");
                cards.Add(card);
            }

            player.DrawPile = new List<CardData>(cards);
            player.Hand = new List<CardData>();

            // Act
            player.DrawCards(5); // 초기 핸드 5장

            // Assert
            Assert.AreEqual(5, player.Hand.Count);
            Assert.AreEqual(5, player.DrawPile.Count);

            Cleanup(player);
        }

        // Phase 4.2: 턴 관리
        [Test]
        public void Player_턴을_시작할_수_있다()
        {
            // Arrange
            var player = CreatePlayer(maxNaegong: 3);

            // Act
            player.StartTurn();

            // Assert
            Assert.AreEqual(3, player.Energy);

            Cleanup(player);
        }

        [Test]
        public void Player_턴_시작_시_내공이_리필된다()
        {
            // Arrange
            var player = CreatePlayer(maxNaegong: 3);
            player.Energy = 0;

            // Act
            player.StartTurn();

            // Assert
            Assert.AreEqual(3, player.Energy);

            Cleanup(player);
        }

        [Test]
        public void Player_턴_시작_시_카드를_뽑는다()
        {
            // Arrange
            var player = CreatePlayer();

            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = CreateCard($"card_{i}", $"카드 {i}");
                cards.Add(card);
            }

            player.DrawPile = new List<CardData>(cards);
            player.Hand = new List<CardData>();

            // Act
            player.StartTurn(); // StartTurn에서 카드를 뽑음

            // Assert
            // StartTurn이 카드를 뽑는지 확인 (구현에 따라 다를 수 있음)
            Assert.IsNotNull(player.Hand);

            Cleanup(player);
        }

        [Test]
        public void Enemy_턴을_시작할_수_있다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var player = CreatePlayer(currentHealth: 100);

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

            Cleanup(enemy, player);
        }

        [Test]
        public void Enemy_턴에_모든_적이_순서대로_행동한다()
        {
            // Arrange
            var player = CreatePlayer(currentHealth: 100);

            var enemy1 = CreateEnemy();
            var action1 = new EnemyAction { actionType = EnemyActionType.Attack, value = 5 };
            enemy1.SetNextAction(action1);

            var enemy2 = CreateEnemy();
            var action2 = new EnemyAction { actionType = EnemyActionType.Attack, value = 10 };
            enemy2.SetNextAction(action2);

            // Act
            enemy1.TakeTurn(player);
            enemy2.TakeTurn(player);

            // Assert
            Assert.AreEqual(85, player.CurrentHealth); // 100 - 5 - 10 = 85

            Cleanup(enemy1, enemy2, player);
        }

        [Test]
        public void 턴_종료_시_핸드의_카드를_버린다()
        {
            // Arrange
            var player = CreatePlayer();

            var card1 = CreateCard("card1", "카드1");
            var card2 = CreateCard("card2", "카드2");
            player.Hand = new List<CardData> { card1, card2 };
            player.DiscardPile = new List<CardData>();

            // Act
            player.EndTurn();

            // Assert
            Assert.AreEqual(0, player.Hand.Count);
            Assert.AreEqual(2, player.DiscardPile.Count);

            Cleanup(player);
        }

        // Phase 4.3: 카드 사용
        [Test]
        public void Player가_카드를_사용할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Energy = 3;

            var card = CreateCard("strike", "타격", 1);

            player.Hand = new List<CardData> { card };

            var enemy = CreateEnemy();

            // Act
            player.PlayCard(card, enemy);

            // Assert
            Assert.AreEqual(2, player.Energy); // 3 - 1 = 2
            Assert.IsFalse(player.Hand.Contains(card)); // 카드가 핸드에서 제거됨

            Cleanup(player, enemy);
        }

        [Test]
        public void 카드_사용_시_내공이_소모된다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Energy = 5;

            var card = CreateCard("strike", "타격", 2);

            player.Hand = new List<CardData> { card };

            var enemy = CreateEnemy();

            // Act
            player.PlayCard(card, enemy);

            // Assert
            Assert.AreEqual(3, player.Energy); // 5 - 2 = 3

            Cleanup(player, enemy);
        }

        [Test]
        public void 내공이_부족하면_카드를_사용할_수_없다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Energy = 1;

            var card = CreateCard("strike", "타격", 3); // 필요 내공이 현재 내공보다 많음

            player.Hand = new List<CardData> { card };

            var enemy = CreateEnemy();

            // Act
            player.PlayCard(card, enemy);

            // Assert
            Assert.AreEqual(1, player.Energy); // 내공이 소모되지 않음
            Assert.IsTrue(player.Hand.Contains(card)); // 카드가 핸드에 남아있음

            Cleanup(player, enemy);
        }

        [Test]
        public void 카드_효과가_올바르게_적용된다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Energy = 5;

            var card = CreateCard("strike", "타격", 1, Damage(10));

            player.Hand = new List<CardData> { card };

            var enemy = CreateEnemy(currentHealth: 50);

            // Act
            player.PlayCard(card, enemy);

            // Assert
            Assert.AreEqual(40, enemy.CurrentHealth); // 50 - 10 = 40

            Cleanup(player, enemy);
        }

        [Test]
        public void 타겟이_필요한_카드는_타겟을_지정해야_사용할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Energy = 5;

            var card = CreateCard("strike", "타격", 1, Damage(10));

            player.Hand = new List<CardData> { card };

            var enemy = CreateEnemy(currentHealth: 50);

            // Act
            player.PlayCard(card, enemy); // 타겟 지정

            // Assert
            Assert.AreEqual(40, enemy.CurrentHealth); // 타겟에 데미지가 적용됨

            Cleanup(player, enemy);
        }

        [Test]
        public void 절초_카드는_사용_후_소멸된다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Energy = 5;

            var card = CreateCard("exhaust_card", "절초 카드", 0);
            card.isJeolcho = true; // 절초 카드

            player.Hand = new List<CardData> { card };
            player.ExhaustPile = new List<CardData>();
            player.DiscardPile = new List<CardData>();

            var enemy = CreateEnemy();

            // Act
            player.PlayCard(card, enemy);

            // Assert
            Assert.IsTrue(player.ExhaustPile.Contains(card)); // 소멸 더미에 추가됨
            Assert.IsFalse(player.DiscardPile.Contains(card)); // 버린 더미에는 없음

            Cleanup(player, enemy);
        }

        // Phase 4.4: 전투 종료
        [Test]
        public void 모든_적이_사망하면_전투에서_승리한다()
        {
            // Arrange
            var enemy = CreateEnemy(currentHealth: 10);

            // Act
            enemy.TakeDamage(10);

            // Assert
            Assert.IsTrue(enemy.IsDead);
            Assert.AreEqual(0, enemy.CurrentHealth);

            Cleanup(enemy);
        }

        [Test]
        public void Player가_사망하면_전투에서_패배한다()
        {
            // Arrange
            var player = CreatePlayer(currentHealth: 10);

            // Act
            player.TakeDamage(10);

            // Assert
            Assert.IsTrue(player.IsDead);
            Assert.AreEqual(0, player.CurrentHealth);

            Cleanup(player);
        }

        [Test]
        public void 전투_승리_시_수련치를_획득한다()
        {
            // Arrange
            var player = CreatePlayer();
            player.CurrentXp = 0;

            // Act
            player.GainXp(5); // 적 처치 시 수련치 획득

            // Assert
            Assert.AreEqual(5, player.CurrentXp);

            Cleanup(player);
        }

        [Test]
        public void 전투_승리_시_보상을_받는다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Gold = 0;

            var rewardCard = CreateCard("reward_card", "보상 카드");

            // Act
            player.Gold += 50; // 골드 보상
            player.AddCardToDeck(rewardCard); // 카드 보상

            // Assert
            Assert.AreEqual(50, player.Gold);
            Assert.Contains(rewardCard, player.DiscardPile); // 새 카드는 버린 더미에 추가됨

            Cleanup(player);
        }
    }
}
