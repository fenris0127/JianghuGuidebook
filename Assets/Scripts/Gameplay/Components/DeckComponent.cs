using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Gameplay
{
    /// <summary>
    /// 플레이어의 카드 덱을 관리하는 컴포넌트
    /// 뽑을 더미, 핸드, 버린 더미, 소진 더미를 관리합니다.
    /// </summary>
    public class DeckComponent : MonoBehaviour
    {
        public event Action<List<CardData>> OnHandChanged;
        public event Action<int, int, int> OnPilesChanged; // drawPile, discardPile, exhaustPile

        public List<CardData> DrawPile { get; private set; } = new List<CardData>();
        public List<CardData> Hand { get; private set; } = new List<CardData>();
        public List<CardData> DiscardPile { get; private set; } = new List<CardData>();
        public List<CardData> ExhaustPile { get; private set; } = new List<CardData>();

        /// <summary>
        /// 덱을 초기화합니다.
        /// </summary>
        public void Initialize(List<CardData> startingDeck)
        {
            DrawPile = new List<CardData>(startingDeck);
            Hand.Clear();
            DiscardPile.Clear();
            ExhaustPile.Clear();

            DrawPile.Shuffle();

            OnHandChanged?.Invoke(new List<CardData>(Hand));
            OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
        }

        /// <summary>
        /// 카드를 뽑습니다. 뽑을 더미가 비어있으면 버린 더미를 섞어서 뽑을 더미로 만듭니다.
        /// </summary>
        public void DrawCards(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (DrawPile.Count == 0)
                {
                    if (DiscardPile.Count == 0) return;
                    DrawPile.AddRange(DiscardPile);
                    DiscardPile.Clear();
                    DrawPile.Shuffle();
                }

                CardData newCard = DrawPile[0];
                DrawPile.RemoveAt(0);
                Hand.Add(newCard);
            }

            OnHandChanged?.Invoke(new List<CardData>(Hand));
            OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
        }

        /// <summary>
        /// 뽑을 더미를 섞습니다.
        /// </summary>
        public void ShuffleDeck()
        {
            DrawPile.Shuffle();
        }

        /// <summary>
        /// 카드를 핸드에서 버린 더미로 이동합니다.
        /// </summary>
        public void DiscardCard(CardData card)
        {
            if (Hand.Remove(card))
            {
                DiscardPile.Add(card);
                OnHandChanged?.Invoke(new List<CardData>(Hand));
                OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
            }
        }

        /// <summary>
        /// 카드를 핸드에서 소진 더미로 이동합니다.
        /// </summary>
        public void ExhaustCard(CardData card)
        {
            if (Hand.Remove(card))
            {
                ExhaustPile.Add(card);
                OnHandChanged?.Invoke(new List<CardData>(Hand));
                OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
            }
        }

        /// <summary>
        /// 핸드의 모든 카드를 버린 더미로 이동합니다 (턴 종료 시).
        /// </summary>
        public void DiscardHand()
        {
            while (Hand.Count > 0)
            {
                var card = Hand[0];
                Hand.RemoveAt(0);
                DiscardPile.Add(card);
            }

            OnHandChanged?.Invoke(new List<CardData>(Hand));
            OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
        }

        /// <summary>
        /// 덱에 새로운 카드를 추가합니다 (버린 더미에 추가).
        /// </summary>
        public void AddCardToDeck(CardData newCard)
        {
            DiscardPile.Add(newCard);
            OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
        }

        /// <summary>
        /// 덱에서 카드를 제거합니다.
        /// </summary>
        public bool RemoveCardFromDeck(CardData cardToRemove)
        {
            bool removed = Hand.Remove(cardToRemove) ||
                          DiscardPile.Remove(cardToRemove) ||
                          DrawPile.Remove(cardToRemove);

            if (removed)
            {
                OnHandChanged?.Invoke(new List<CardData>(Hand));
                OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
            }

            return removed;
        }

        /// <summary>
        /// 카드를 강화합니다.
        /// </summary>
        public bool UpgradeCard(CardData cardToUpgrade)
        {
            if (cardToUpgrade.upgradedVersion == null || cardToUpgrade.isUpgraded)
                return false;

            CardData upgradedCard = cardToUpgrade.upgradedVersion;

            bool success = ReplaceCardInList(DrawPile, cardToUpgrade, upgradedCard) ||
                          ReplaceCardInList(DiscardPile, cardToUpgrade, upgradedCard) ||
                          ReplaceCardInList(Hand, cardToUpgrade, upgradedCard);

            if (success)
            {
                OnHandChanged?.Invoke(new List<CardData>(Hand));
                OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
            }

            return success;
        }

        /// <summary>
        /// 랜덤 카드를 강화합니다.
        /// </summary>
        public void UpgradeRandomCard()
        {
            var upgradableCards = GetAllCardsInDeck().FindAll(c => !c.isUpgraded && c.upgradedVersion != null);
            if (upgradableCards.Count > 0)
            {
                CardData cardToUpgrade = upgradableCards[UnityEngine.Random.Range(0, upgradableCards.Count)];
                UpgradeCard(cardToUpgrade);
            }
        }

        /// <summary>
        /// 랜덤으로 여러 장의 카드를 제거합니다.
        /// </summary>
        public void RemoveRandomCards(int count)
        {
            var removableCards = GetAllCardsInDeck().Where(c => c.rarity != CardRarity.Common).ToList();

            if (removableCards.Count < count)
                removableCards = GetAllCardsInDeck();

            removableCards.Shuffle();

            for (int i = 0; i < count; i++)
            {
                if (removableCards.Count > 0)
                {
                    CardData cardToRemove = removableCards[0];
                    RemoveCardFromDeck(cardToRemove);
                    removableCards.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// 모든 기본 카드를 제거합니다.
        /// </summary>
        public void RemoveAllBasicCards()
        {
            var basicCards = GetAllCardsInDeck()
                .Where(c => c.assetID == "card_basic_strike" || c.assetID == "card_basic_defend")
                .ToList();

            foreach (var card in basicCards)
            {
                while (RemoveCardFromDeck(card)) { }
            }
        }

        /// <summary>
        /// 모든 저주 카드를 제거합니다.
        /// </summary>
        public void RemoveAllCurseCards()
        {
            var curseCards = GetAllCardsInDeck().Where(c => c.isCurse).ToList();
            foreach (var card in curseCards)
            {
                while (RemoveCardFromDeck(card)) { }
            }
        }

        /// <summary>
        /// 덱의 모든 카드를 반환합니다 (뽑을 더미 + 버린 더미 + 핸드).
        /// </summary>
        public List<CardData> GetAllCardsInDeck()
        {
            return DrawPile.Concat(DiscardPile).Concat(Hand).ToList();
        }

        /// <summary>
        /// 상태를 복원합니다 (세이브 로드 시).
        /// </summary>
        public void RestoreState(List<CardData> drawPile, List<CardData> discardPile,
                                List<CardData> hand, List<CardData> exhaustPile)
        {
            DrawPile = new List<CardData>(drawPile);
            DiscardPile = new List<CardData>(discardPile);
            Hand = new List<CardData>(hand);
            ExhaustPile = new List<CardData>(exhaustPile);

            OnHandChanged?.Invoke(new List<CardData>(Hand));
            OnPilesChanged?.Invoke(DrawPile.Count, DiscardPile.Count, ExhaustPile.Count);
        }

        private bool ReplaceCardInList(List<CardData> list, CardData oldCard, CardData newCard)
        {
            int index = list.IndexOf(oldCard);
            if (index != -1)
            {
                list[index] = newCard;
                return true;
            }
            return false;
        }
    }
}
