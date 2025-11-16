using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class RelicSystemTests
    {
        // Phase 8.1: 유물 획득과 효과
        [Test]
        public void Player가_유물을_획득할_수_있다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var relicData = ScriptableObject.CreateInstance<RelicData>();
            relicData.relicName = "금강불괴";
            relicData.trigger = RelicTrigger.OnCombatStart;
            relicData.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.Block, value = 3 }
            };

            // Act
            player.AddRelic(relicData);

            // Assert
            Assert.AreEqual(1, player.relics.Count);
            Assert.AreEqual(relicData, player.relics[0]);
            Assert.AreEqual("금강불괴", player.relics[0].relicName);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 전투_시작_시_발동하는_유물이_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            // 전투 시작 시 방어도를 부여하는 유물
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            relicData.relicName = "금강불괴";
            relicData.trigger = RelicTrigger.OnCombatStart;
            relicData.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.Block, value = 5 }
            };

            player.AddRelic(relicData);

            // Act
            player.ApplyCombatStartRelicEffects();

            // Assert
            Assert.AreEqual(5, player.Block, "전투 시작 시 유물 효과로 방어도 5를 얻어야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 턴_시작_시_발동하는_유물이_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;
            player.MaxNaegong = 3;

            // 턴 시작 시 카드를 추가로 뽑는 유물
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            relicData.relicName = "천리안";
            relicData.trigger = RelicTrigger.OnTurnStart;
            relicData.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.DrawCard, value = 1 }
            };

            player.AddRelic(relicData);

            // 테스트를 위한 덱 설정
            var card1 = ScriptableObject.CreateInstance<CardData>();
            card1.cardName = "타격";
            var card2 = ScriptableObject.CreateInstance<CardData>();
            card2.cardName = "방어";
            var card3 = ScriptableObject.CreateInstance<CardData>();
            card3.cardName = "참격";
            var card4 = ScriptableObject.CreateInstance<CardData>();
            card4.cardName = "베기";
            var card5 = ScriptableObject.CreateInstance<CardData>();
            card5.cardName = "찌르기";
            var card6 = ScriptableObject.CreateInstance<CardData>();
            card6.cardName = "회전베기";

            var deck = new List<CardData> { card1, card2, card3, card4, card5, card6 };
            player.Setup(deck);

            // Act
            player.StartTurn();

            // Assert
            // 기본 5장 + 유물 효과 1장 = 6장
            Assert.AreEqual(6, player.hand.Count, "턴 시작 시 유물 효과로 카드를 1장 더 뽑아야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 카드_사용_시_발동하는_유물이_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.CurrentHealth = 100;

            // 카드 사용 시 골드를 획득하는 유물
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            relicData.relicName = "황금 부적";
            relicData.trigger = RelicTrigger.OnCardPlayed;
            relicData.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.GainGold, value = 1 }
            };

            player.AddRelic(relicData);

            var card = ScriptableObject.CreateInstance<CardData>();
            card.cardName = "타격";
            card.baseCost = 0; // 비용 0으로 설정
            card.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.Damage, value = 5 }
            };

            player.hand.Add(card);
            int initialGold = player.gold;

            // Act
            player.PlayCard(card, enemy);

            // Assert
            int goldGained = GetRelicGoldEffect(relicData);
            Assert.AreEqual(initialGold + goldGained, player.gold, "카드 사용 시 유물 효과로 골드를 얻어야 합니다");

            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 여러_유물의_효과가_동시에_적용된다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            // 전투 시작 시 방어도를 부여하는 유물 1
            var relic1 = ScriptableObject.CreateInstance<RelicData>();
            relic1.relicName = "금강불괴";
            relic1.trigger = RelicTrigger.OnCombatStart;
            relic1.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.Block, value = 3 }
            };

            // 전투 시작 시 방어도를 부여하는 유물 2
            var relic2 = ScriptableObject.CreateInstance<RelicData>();
            relic2.relicName = "철갑옷";
            relic2.trigger = RelicTrigger.OnCombatStart;
            relic2.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.Block, value = 2 }
            };

            // 전투 시작 시 내공을 부여하는 유물
            var relic3 = ScriptableObject.CreateInstance<RelicData>();
            relic3.relicName = "내공 구슬";
            relic3.trigger = RelicTrigger.OnCombatStart;
            relic3.effects = new List<GameEffect>
            {
                new GameEffect { type = GameEffectType.Energy, value = 1 }
            };

            player.AddRelic(relic1);
            player.AddRelic(relic2);
            player.AddRelic(relic3);

            // Act
            player.ApplyCombatStartRelicEffects();

            // Assert
            Assert.AreEqual(3, player.relics.Count, "3개의 유물을 소유해야 합니다");
            Assert.AreEqual(5, player.Block, "유물 2개의 방어도 효과가 합쳐져서 5가 되어야 합니다");
            Assert.AreEqual(1, player.Energy, "유물 효과로 내공 1을 얻어야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        // Helper method
        private int GetRelicGoldEffect(RelicData relic)
        {
            int totalGold = 0;
            if (relic.trigger == RelicTrigger.OnCardPlayed)
            {
                foreach (var effect in relic.effects)
                {
                    if (effect.type == GameEffectType.GainGold)
                    {
                        totalGold += effect.value;
                    }
                }
            }
            return totalGold;
        }
    }
}
