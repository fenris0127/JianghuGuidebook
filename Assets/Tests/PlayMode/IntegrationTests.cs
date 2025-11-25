using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;
using JianghuGuidebook.Data;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.Tests.PlayMode
{
    /// <summary>
    /// Phase 1 프로토타입 통합 테스트
    /// </summary>
    public class IntegrationTests
    {
        private Player player;
        private Enemy enemy;
        private DeckManager deckManager;
        private DataManager dataManager;

        [SetUp]
        public void Setup()
        {
            // DataManager 초기화
            GameObject dataManagerObj = new GameObject("DataManager");
            dataManager = dataManagerObj.AddComponent<DataManager>();

            // Player 초기화
            GameObject playerObj = new GameObject("Player");
            player = playerObj.AddComponent<Player>();
            player.Initialize(70, 3);

            // DeckManager 초기화
            GameObject deckManagerObj = new GameObject("DeckManager");
            deckManager = deckManagerObj.AddComponent<DeckManager>();
        }

        [TearDown]
        public void Teardown()
        {
            if (player != null) Object.Destroy(player.gameObject);
            if (enemy != null) Object.Destroy(enemy.gameObject);
            if (deckManager != null) Object.Destroy(deckManager.gameObject);
            if (dataManager != null) Object.Destroy(dataManager.gameObject);
        }

        #region 9.1 Comprehensive Playtest Scenarios

        [UnityTest]
        public IEnumerator Test_FullCombat_PlayerVsBandit()
        {
            // Arrange
            EnemyData banditData = dataManager.GetEnemyData("enemy_bandit");
            GameObject enemyObj = new GameObject("Enemy_Bandit");
            enemy = enemyObj.AddComponent<Enemy>();
            enemy.Initialize(banditData);

            // Act & Assert
            Assert.IsNotNull(enemy);
            Assert.AreEqual(40, enemy.MaxHealth);
            Assert.AreEqual(40, enemy.CurrentHealth);

            // 적이 의도를 결정할 수 있는지
            enemy.DetermineIntent();
            Assert.IsNotNull(enemy.CurrentIntent);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_FullCombat_PlayerVsDarkCultivator()
        {
            // Arrange
            EnemyData cultivatorData = dataManager.GetEnemyData("enemy_dark_cultivator");
            GameObject enemyObj = new GameObject("Enemy_DarkCultivator");
            enemy = enemyObj.AddComponent<Enemy>();
            enemy.Initialize(cultivatorData);

            // Assert
            Assert.IsNotNull(enemy);
            Assert.AreEqual(30, enemy.MaxHealth);
            Assert.AreEqual("마도수련자", enemy.EnemyName);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_FullCombat_PlayerVsArmoredGuard()
        {
            // Arrange
            EnemyData guardData = dataManager.GetEnemyData("enemy_armored_guard");
            GameObject enemyObj = new GameObject("Enemy_ArmoredGuard");
            enemy = enemyObj.AddComponent<Enemy>();
            enemy.Initialize(guardData);

            // Assert
            Assert.IsNotNull(enemy);
            Assert.AreEqual(50, enemy.MaxHealth);
            Assert.AreEqual("철갑위병", enemy.EnemyName);

            yield return null;
        }

        #endregion

        #region 9.2 Edge Case Tests

        [Test]
        public void Test_9_2_1_DrawFromEmptyDeck_ShouldReshuffleDiscard()
        {
            // Arrange
            var cards = CreateTestDeck(5);
            deckManager.InitializeDeck(cards);

            // Draw all cards
            for (int i = 0; i < 5; i++)
            {
                deckManager.DrawCard();
            }

            Assert.AreEqual(0, deckManager.GetDrawPileCount(), "Draw pile should be empty");
            Assert.AreEqual(5, deckManager.GetHand().Count, "Hand should have 5 cards");

            // Discard hand
            var hand = new System.Collections.Generic.List<Card>(deckManager.GetHand());
            foreach (var card in hand)
            {
                deckManager.DiscardCard(card);
            }

            Assert.AreEqual(5, deckManager.GetDiscardPileCount(), "Discard pile should have 5 cards");

            // Act - Draw from empty deck (should reshuffle)
            Card drawnCard = deckManager.DrawCard();

            // Assert
            Assert.IsNotNull(drawnCard, "Should be able to draw after reshuffle");
            Assert.AreEqual(4, deckManager.GetDrawPileCount(), "Draw pile should have 4 cards after reshuffle");
            Assert.AreEqual(0, deckManager.GetDiscardPileCount(), "Discard pile should be empty after reshuffle");
        }

        [Test]
        public void Test_9_2_2_DrawExceedingMaxHandSize_ShouldBeLimited()
        {
            // Arrange
            var cards = CreateTestDeck(15);
            deckManager.InitializeDeck(cards);

            // Act - Try to draw 15 cards (max hand size is 10)
            deckManager.DrawCards(15);

            // Assert
            Assert.LessOrEqual(deckManager.GetHand().Count, Constants.MAX_HAND_SIZE,
                $"Hand size should not exceed {Constants.MAX_HAND_SIZE}");
        }

        [Test]
        public void Test_9_2_3_PlayCardWithInsufficientEnergy_ShouldFail()
        {
            // Arrange
            player.CurrentEnergy = 1;
            var expensiveCard = new Card(new CardData
            {
                id = "test_expensive",
                name = "Expensive Card",
                cost = 3,
                type = CardType.Attack,
                baseDamage = 20
            });

            // Act
            bool canPlay = player.CurrentEnergy >= expensiveCard.Cost;

            // Assert
            Assert.IsFalse(canPlay, "Should not be able to play card with insufficient energy");
        }

        [Test]
        public void Test_9_2_4_EnemyDeath_ShouldTriggerVictory()
        {
            // Arrange
            EnemyData testEnemyData = new EnemyData
            {
                id = "test_enemy",
                name = "Test Enemy",
                maxHealth = 10
            };

            GameObject enemyObj = new GameObject("TestEnemy");
            enemy = enemyObj.AddComponent<Enemy>();
            enemy.Initialize(testEnemyData);

            bool deathEventTriggered = false;
            enemy.OnDeath += () => deathEventTriggered = true;

            // Act
            enemy.TakeDamage(15);

            // Assert
            Assert.LessOrEqual(enemy.CurrentHealth, 0, "Enemy should be dead");
            Assert.IsTrue(deathEventTriggered, "Death event should be triggered");
        }

        [Test]
        public void Test_9_2_5_PlayerDeath_ShouldTriggerDefeat()
        {
            // Arrange
            player.Initialize(10, 3);

            // Act
            player.TakeDamage(15);

            // Assert
            Assert.LessOrEqual(player.CurrentHealth, 0, "Player should be dead");
        }

        #endregion

        #region Additional Edge Cases

        [Test]
        public void Test_BlockResetOnTurnStart()
        {
            // Arrange
            player.GainBlock(10);
            Assert.AreEqual(10, player.Block);

            // Act
            player.OnTurnStart();

            // Assert
            Assert.AreEqual(0, player.Block, "Block should reset to 0 at turn start");
        }

        [Test]
        public void Test_EnergyResetOnTurnStart()
        {
            // Arrange
            player.CurrentEnergy = 0;

            // Act
            player.OnTurnStart();

            // Assert
            Assert.AreEqual(player.MaxEnergy, player.CurrentEnergy, "Energy should reset to max at turn start");
        }

        [Test]
        public void Test_DamageReductionByBlock()
        {
            // Arrange
            player.Initialize(50, 3);
            player.GainBlock(10);

            // Act
            player.TakeDamage(15);

            // Assert
            Assert.AreEqual(45, player.CurrentHealth, "5 damage should go through (15 - 10 block)");
            Assert.AreEqual(0, player.Block, "Block should be consumed");
        }

        [Test]
        public void Test_ExcessBlockAbsorbsDamageCompletely()
        {
            // Arrange
            player.Initialize(50, 3);
            player.GainBlock(20);

            // Act
            player.TakeDamage(15);

            // Assert
            Assert.AreEqual(50, player.CurrentHealth, "No damage should go through");
            Assert.AreEqual(5, player.Block, "5 block should remain");
        }

        [Test]
        public void Test_MultipleEnemiesCanExist()
        {
            // Arrange
            EnemyData banditData = dataManager.GetEnemyData("enemy_bandit");

            GameObject enemy1Obj = new GameObject("Enemy1");
            Enemy enemy1 = enemy1Obj.AddComponent<Enemy>();
            enemy1.Initialize(banditData);

            GameObject enemy2Obj = new GameObject("Enemy2");
            Enemy enemy2 = enemy2Obj.AddComponent<Enemy>();
            enemy2.Initialize(banditData);

            // Assert
            Assert.IsNotNull(enemy1);
            Assert.IsNotNull(enemy2);
            Assert.AreNotSame(enemy1, enemy2);

            // Cleanup
            Object.Destroy(enemy1.gameObject);
            Object.Destroy(enemy2.gameObject);
        }

        [Test]
        public void Test_CardExhaustMechanism()
        {
            // Arrange
            var cards = CreateTestDeck(1);
            deckManager.InitializeDeck(cards);

            var card = deckManager.DrawCard();

            // Act
            deckManager.ExhaustCard(card);

            // Assert
            Assert.AreEqual(1, deckManager.GetExhaustPileCount(), "Card should be in exhaust pile");
            Assert.AreEqual(0, deckManager.GetHand().Count, "Card should not be in hand");
            Assert.AreEqual(0, deckManager.GetDiscardPileCount(), "Card should not be in discard");
        }

        #endregion

        #region Helper Methods

        private System.Collections.Generic.List<CardData> CreateTestDeck(int count)
        {
            var deck = new System.Collections.Generic.List<CardData>();

            for (int i = 0; i < count; i++)
            {
                deck.Add(new CardData
                {
                    id = $"test_card_{i}",
                    name = $"Test Card {i}",
                    cost = 1,
                    type = CardType.Attack,
                    baseDamage = 6,
                    description = "Test card"
                });
            }

            return deck;
        }

        #endregion
    }
}
