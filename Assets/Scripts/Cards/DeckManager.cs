using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.Cards
{
    /// <summary>
    /// 덱, 손패, 버리기 더미, 소진 더미를 관리하는 매니저
    /// </summary>
    public class DeckManager : MonoBehaviour
    {
        [Header("카드 더미")]
        [SerializeField] private List<Card> drawPile = new List<Card>();      // 뽑기 더미
        [SerializeField] private List<Card> hand = new List<Card>();          // 손패
        [SerializeField] private List<Card> discardPile = new List<Card>();   // 버리기 더미
        [SerializeField] private List<Card> exhaustPile = new List<Card>();   // 소진 더미

        [Header("설정")]
        [SerializeField] private int maxHandSize = Constants.MAX_HAND_SIZE;

        // Properties
        public List<Card> DrawPile => drawPile;
        public List<Card> Hand => hand;
        public List<Card> DiscardPile => discardPile;
        public List<Card> ExhaustPile => exhaustPile;

        // Events
        public System.Action<Card> OnCardDrawn;
        public System.Action<Card> OnCardPlayed;
        public System.Action<Card> OnCardDiscarded;
        public System.Action<Card> OnCardExhausted;
        public System.Action OnDeckShuffled;

        /// <summary>
        /// 덱을 초기화합니다
        /// </summary>
        public void InitializeDeck(List<CardData> cardDataList)
        {
            // 모든 더미 초기화
            drawPile.Clear();
            hand.Clear();
            discardPile.Clear();
            exhaustPile.Clear();

            // CardData로부터 Card 생성
            foreach (var cardData in cardDataList)
            {
                Card card = new Card(cardData);
                drawPile.Add(card);
            }

            // 덱 셔플
            Shuffle(drawPile);

            Debug.Log($"덱 초기화 완료: {drawPile.Count}장");
        }

        /// <summary>
        /// 카드 ID 리스트로 덱을 초기화합니다
        /// </summary>
        public void InitializeDeck(List<string> cardIds)
        {
            var cardDataList = new List<CardData>();

            foreach (var cardId in cardIds)
            {
                CardData data = DataManager.Instance.GetCardData(cardId);
                if (data != null)
                {
                    cardDataList.Add(data);
                }
                else
                {
                    Debug.LogWarning($"카드를 찾을 수 없습니다: {cardId}");
                }
            }

            InitializeDeck(cardDataList);
        }

        /// <summary>
        /// 기본 시작 덱을 생성합니다 (테스트용)
        /// </summary>
        public void InitializeStarterDeck()
        {
            var starterDeck = new List<string>
            {
                "card_strike",
                "card_strike",
                "card_strike",
                "card_strike",
                "card_iron_guard",
                "card_iron_guard",
                "card_iron_guard",
                "card_iron_guard",
                "card_qi_circulation",
                "card_clear_mind"
            };

            InitializeDeck(starterDeck);
        }

        /// <summary>
        /// 카드 1장을 뽑습니다
        /// </summary>
        public Card DrawCard()
        {
            // 손패가 가득 찼는지 확인
            if (hand.Count >= maxHandSize)
            {
                Debug.LogWarning($"손패가 가득 찼습니다 ({hand.Count}/{maxHandSize})");
                return null;
            }

            // 뽑기 더미가 비었으면 버리기 더미 재셔플
            if (drawPile.Count == 0)
            {
                ReshuffleDiscardPile();

                // 재셔플 후에도 카드가 없으면
                if (drawPile.Count == 0)
                {
                    Debug.LogWarning("뽑을 카드가 없습니다");
                    return null;
                }
            }

            // 카드 뽑기
            Card card = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(card);

            Debug.Log($"카드 드로우: {card.Name} (손패: {hand.Count}/{maxHandSize}, 덱: {drawPile.Count})");
            OnCardDrawn?.Invoke(card);

            return card;
        }

        /// <summary>
        /// 여러 장의 카드를 뽑습니다
        /// </summary>
        public List<Card> DrawCards(int count)
        {
            var drawnCards = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                Card card = DrawCard();
                if (card != null)
                {
                    drawnCards.Add(card);
                }
                else
                {
                    // 더 이상 뽑을 수 없으면 중단
                    break;
                }
            }

            return drawnCards;
        }

        /// <summary>
        /// 카드를 사용합니다
        /// </summary>
        public bool PlayCard(Card card)
        {
            if (!hand.Contains(card))
            {
                Debug.LogWarning($"손패에 없는 카드를 사용하려고 했습니다: {card.Name}");
                return false;
            }

            // 손패에서 제거
            hand.Remove(card);

            Debug.Log($"카드 사용: {card}");
            OnCardPlayed?.Invoke(card);

            // 소진 여부에 따라 처리
            if (card.Exhaust)
            {
                exhaustPile.Add(card);
                OnCardExhausted?.Invoke(card);
                Debug.Log($"카드 소진: {card.Name}");
            }
            else
            {
                discardPile.Add(card);
                OnCardDiscarded?.Invoke(card);
            }

            return true;
        }

        /// <summary>
        /// 손패의 모든 카드를 버립니다
        /// </summary>
        public void DiscardHand()
        {
            int count = hand.Count;
            if (count == 0) return;

            foreach (var card in hand.ToArray())
            {
                discardPile.Add(card);
                OnCardDiscarded?.Invoke(card);
            }

            hand.Clear();
            Debug.Log($"손패 버리기: {count}장");
        }

        /// <summary>
        /// 특정 카드를 버립니다
        /// </summary>
        public void DiscardCard(Card card)
        {
            if (hand.Contains(card))
            {
                hand.Remove(card);
                discardPile.Add(card);
                OnCardDiscarded?.Invoke(card);
                Debug.Log($"카드 버리기: {card.Name}");
            }
        }

        /// <summary>
        /// 버리기 더미를 뽑기 더미로 재셔플합니다
        /// </summary>
        private void ReshuffleDiscardPile()
        {
            if (discardPile.Count == 0)
            {
                Debug.Log("버리기 더미가 비어있어 재셔플할 수 없습니다");
                return;
            }

            Debug.Log($"버리기 더미 재셔플: {discardPile.Count}장");

            // 버리기 더미를 뽑기 더미로 이동
            drawPile.AddRange(discardPile);
            discardPile.Clear();

            // 셔플
            Shuffle(drawPile);
            OnDeckShuffled?.Invoke();
        }

        /// <summary>
        /// Fisher-Yates 알고리즘으로 리스트를 셔플합니다
        /// </summary>
        private void Shuffle(List<Card> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Card temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            Debug.Log($"덱 셔플 완료: {list.Count}장");
        }

        /// <summary>
        /// 덱에 카드를 추가합니다 (전투 중 카드 획득)
        /// </summary>
        public void AddCardToDrawPile(Card card)
        {
            drawPile.Add(card);
            Debug.Log($"뽑기 더미에 카드 추가: {card.Name}");
        }

        /// <summary>
        /// 버리기 더미에 카드를 추가합니다
        /// </summary>
        public void AddCardToDiscardPile(Card card)
        {
            discardPile.Add(card);
            Debug.Log($"버리기 더미에 카드 추가: {card.Name}");
        }

        /// <summary>
        /// 전투 종료 시 모든 더미를 정리합니다
        /// </summary>
        public void ResetForNewCombat()
        {
            // 모든 카드를 뽑기 더미로 이동 (소진된 카드 제외)
            drawPile.AddRange(hand);
            drawPile.AddRange(discardPile);

            hand.Clear();
            discardPile.Clear();
            // exhaustPile은 유지 (전투 종료 시 소진된 카드는 영구적)

            // 덱 셔플
            Shuffle(drawPile);

            Debug.Log($"전투 종료: 덱 리셋 완료 ({drawPile.Count}장)");
        }

        /// <summary>
        /// 디버그 정보를 출력합니다
        /// </summary>
        public void PrintDebugInfo()
        {
            Debug.Log($"=== 덱 상태 ===");
            Debug.Log($"뽑기 더미: {drawPile.Count}장");
            Debug.Log($"손패: {hand.Count}장 - {string.Join(", ", hand.Select(c => c.Name))}");
            Debug.Log($"버리기 더미: {discardPile.Count}장");
            Debug.Log($"소진 더미: {exhaustPile.Count}장");
        }
    }
}
