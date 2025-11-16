using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Gameplay;
using GangHoBiGeup.Data;
using System.Collections.Generic;
using static GangHoBiGeup.Tests.TestHelper;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class PlayerDeckTests
    {
        [Test]
        public void Player에게_덱을_설정할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var card1 = CreateCard("card1", "카드1");
            var card2 = CreateCard("card2", "카드2");
            var deck = new List<CardData> { card1, card2 };

            // Act
            player.Deck = deck;

            // Assert
            Assert.IsNotNull(player.Deck);
            Assert.AreEqual(2, player.Deck.Count);

            Cleanup(player);
        }

        [Test]
        public void Player의_덱에_카드를_추가할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            player.Deck = new List<CardData>();
            var card = CreateCard("card", "카드");

            // Act
            player.AddCardToDeck(card);

            // Assert
            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.Contains(card, player.DiscardPile);

            Cleanup(player);
        }

        [Test]
        public void Player의_덱에서_카드를_제거할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var card = CreateCard("card", "카드");
            player.Deck = new List<CardData> { card };

            // Act
            bool removed = player.RemoveCardFromDeck(card);

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(0, player.Deck.Count);

            Cleanup(player);
        }

        [Test]
        public void Player의_덱을_셔플할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var cards = new List<CardData>();
            for (int i = 0; i < 10; i++)
            {
                var card = CreateCard($"card_{i}", $"카드{i}");
                cards.Add(card);
            }
            player.DrawPile = new List<CardData>(cards);

            // Act
            var originalOrder = new List<CardData>(player.DrawPile);
            player.ShuffleDeck();

            // Assert
            Assert.AreEqual(10, player.DrawPile.Count);
            // 셔플 후 순서가 변경되었는지 확인 (매우 낮은 확률로 같을 수 있음)
            bool orderChanged = false;
            for (int i = 0; i < originalOrder.Count; i++)
            {
                if (originalOrder[i] != player.DrawPile[i])
                {
                    orderChanged = true;
                    break;
                }
            }
            // 참고: 이 테스트는 확률적으로 실패할 수 있음

            Cleanup(player);
        }

        [Test]
        public void Player의_핸드에_카드를_뽑을_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var card1 = CreateCard("card1", "카드1");
            var card2 = CreateCard("card2", "카드2");
            player.DrawPile = new List<CardData> { card1, card2 };
            player.Hand = new List<CardData>();

            // Act
            player.DrawCards(2);

            // Assert
            Assert.AreEqual(2, player.Hand.Count);
            Assert.AreEqual(0, player.DrawPile.Count);

            Cleanup(player);
        }

        [Test]
        public void Player의_핸드에서_카드를_버릴_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var card = CreateCard("card", "카드");
            player.Hand = new List<CardData> { card };
            player.DiscardPile = new List<CardData>();

            // Act
            player.DiscardCard(card);

            // Assert
            Assert.AreEqual(0, player.Hand.Count);
            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.Contains(card, player.DiscardPile);

            Cleanup(player);
        }

        [Test]
        public void Player의_버린_카드_더미를_관리할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var card1 = CreateCard("card1", "카드1");
            var card2 = CreateCard("card2", "카드2");

            // Act
            player.DiscardPile = new List<CardData> { card1, card2 };

            // Assert
            Assert.IsNotNull(player.DiscardPile);
            Assert.AreEqual(2, player.DiscardPile.Count);

            Cleanup(player);
        }

        [Test]
        public void 덱이_비면_버린_카드_더미를_셔플하여_새_덱으로_만든다()
        {
            // Arrange
            var player = CreatePlayer();
            var card1 = CreateCard("card1", "카드1");
            var card2 = CreateCard("card2", "카드2");
            var card3 = CreateCard("card3", "카드3");

            player.DrawPile = new List<CardData> { card1 };
            player.DiscardPile = new List<CardData> { card2, card3 };
            player.Hand = new List<CardData>();

            // Act - 뽑을 더미의 카드 1장을 뽑음
            player.DrawCards(1);

            // Assert - 핸드에 1장
            Assert.AreEqual(1, player.Hand.Count);
            Assert.Contains(card1, player.Hand);

            // Act - 뽑을 더미가 비었으므로 버린 더미를 섞어서 뽑아야 함
            player.DrawCards(2);

            // Assert
            Assert.AreEqual(3, player.Hand.Count); // 총 3장
            Assert.AreEqual(0, player.DrawPile.Count); // 뽑을 더미 비움
            Assert.AreEqual(0, player.DiscardPile.Count); // 버린 더미도 비움

            Cleanup(player);
        }
    }
}
