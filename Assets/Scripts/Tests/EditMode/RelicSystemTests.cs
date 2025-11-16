using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;
using static GangHoBiGeup.Tests.TestHelper;

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
            var player = CreatePlayer();
            var relicData = CreateRelicData("금강불괴", RelicTrigger.OnCombatStart, Block(3));

            // Act
            player.AddRelic(relicData);

            // Assert
            Assert.AreEqual(1, player.relics.Count);
            Assert.AreEqual(relicData, player.relics[0]);
            Assert.AreEqual("금강불괴", player.relics[0].relicName);

            Cleanup(player);
        }

        [Test]
        public void 전투_시작_시_발동하는_유물이_작동한다()
        {
            // Arrange
            var player = CreatePlayer();
            var relicData = CreateRelicData("금강불괴", RelicTrigger.OnCombatStart, Block(5));
            player.AddRelic(relicData);

            // Act
            player.ApplyCombatStartRelicEffects();

            // Assert
            Assert.AreEqual(5, player.Block, "전투 시작 시 유물 효과로 방어도 5를 얻어야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 턴_시작_시_발동하는_유물이_작동한다()
        {
            // Arrange
            var player = CreatePlayer(maxNaegong: 3);
            var relicData = CreateRelicData("천리안", RelicTrigger.OnTurnStart,
                CreateEffect(GameEffectType.DrawCard, 1));
            player.AddRelic(relicData);

            // 테스트를 위한 덱 설정
            var deck = new List<CardData>
            {
                CreateCard("card1", "타격"),
                CreateCard("card2", "방어"),
                CreateCard("card3", "참격"),
                CreateCard("card4", "베기"),
                CreateCard("card5", "찌르기"),
                CreateCard("card6", "회전베기")
            };
            player.Setup(deck);

            // Act
            player.StartTurn();

            // Assert
            // 기본 5장 + 유물 효과 1장 = 6장
            Assert.AreEqual(6, player.hand.Count, "턴 시작 시 유물 효과로 카드를 1장 더 뽑아야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 카드_사용_시_발동하는_유물이_작동한다()
        {
            // Arrange
            var player = CreatePlayer();
            var enemy = CreateEnemy(100, 100);

            var relicData = CreateRelicData("황금 부적", RelicTrigger.OnCardPlayed,
                CreateEffect(GameEffectType.GainGold, 1));
            player.AddRelic(relicData);

            var card = CreateCard("strike", "타격", 0, Damage(5));
            player.hand.Add(card);
            int initialGold = player.gold;

            // Act
            player.PlayCard(card, enemy);

            // Assert
            int goldGained = GetRelicGoldEffect(relicData);
            Assert.AreEqual(initialGold + goldGained, player.gold, "카드 사용 시 유물 효과로 골드를 얻어야 합니다");

            Cleanup(player, enemy);
        }

        [Test]
        public void 여러_유물의_효과가_동시에_적용된다()
        {
            // Arrange
            var player = CreatePlayer();

            var relic1 = CreateRelicData("금강불괴", RelicTrigger.OnCombatStart, Block(3));
            var relic2 = CreateRelicData("철갑옷", RelicTrigger.OnCombatStart, Block(2));
            var relic3 = CreateRelicData("내공 구슬", RelicTrigger.OnCombatStart, Energy(1));

            player.AddRelic(relic1);
            player.AddRelic(relic2);
            player.AddRelic(relic3);

            // Act
            player.ApplyCombatStartRelicEffects();

            // Assert
            Assert.AreEqual(3, player.relics.Count, "3개의 유물을 소유해야 합니다");
            Assert.AreEqual(5, player.Block, "유물 2개의 방어도 효과가 합쳐져서 5가 되어야 합니다");
            Assert.AreEqual(1, player.Energy, "유물 효과로 내공 1을 얻어야 합니다");

            Cleanup(player);
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
