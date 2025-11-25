using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 손패의 카드들을 부드럽게 배치하고 애니메이션하는 관리자
    /// </summary>
    public class HandLayoutManager : MonoBehaviour
    {
        [Header("Layout Settings")]
        [SerializeField] private float cardSpacing = 120f;
        [SerializeField] private float maxHandWidth = 1200f;
        [SerializeField] private float curveHeight = 50f;
        [SerializeField] private AnimationCurve layoutCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Animation Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotationAmount = 5f;
        [SerializeField] private bool enableCurvedLayout = true;

        [Header("Card Prefab")]
        [SerializeField] private GameObject cardPrefab;

        private List<CardUI> cardsInHand = new List<CardUI>();
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            // 매 프레임마다 카드 위치 업데이트
            UpdateCardPositions();
        }

        /// <summary>
        /// 손패에 카드 추가
        /// </summary>
        public CardUI AddCard(Cards.Card cardData)
        {
            if (cardPrefab == null)
            {
                Debug.LogError("HandLayoutManager: Card prefab not assigned!");
                return null;
            }

            GameObject cardObject = Instantiate(cardPrefab, transform);
            CardUI cardUI = cardObject.GetComponent<CardUI>();

            if (cardUI != null)
            {
                cardUI.Initialize(cardData);
                cardsInHand.Add(cardUI);
                UpdateCardPositions();
            }

            return cardUI;
        }

        /// <summary>
        /// 손패에서 카드 제거
        /// </summary>
        public void RemoveCard(CardUI card)
        {
            if (cardsInHand.Contains(card))
            {
                cardsInHand.Remove(card);
                UpdateCardPositions();
            }
        }

        /// <summary>
        /// 모든 카드 제거
        /// </summary>
        public void ClearHand()
        {
            foreach (var card in cardsInHand)
            {
                if (card != null)
                {
                    Destroy(card.gameObject);
                }
            }

            cardsInHand.Clear();
        }

        /// <summary>
        /// 카드 위치 업데이트
        /// </summary>
        private void UpdateCardPositions()
        {
            int cardCount = cardsInHand.Count;
            if (cardCount == 0) return;

            // 카드 간격 계산
            float totalWidth = Mathf.Min((cardCount - 1) * cardSpacing, maxHandWidth);
            float actualSpacing = cardCount > 1 ? totalWidth / (cardCount - 1) : 0;

            for (int i = 0; i < cardCount; i++)
            {
                if (cardsInHand[i] == null) continue;

                // 카드의 목표 위치 계산
                Vector3 targetPosition = CalculateCardPosition(i, cardCount, actualSpacing);
                float targetRotation = CalculateCardRotation(i, cardCount);

                // 부드러운 이동
                RectTransform cardRect = cardsInHand[i].GetComponent<RectTransform>();
                cardRect.localPosition = Vector3.Lerp(cardRect.localPosition, targetPosition, Time.deltaTime * moveSpeed);
                cardRect.localRotation = Quaternion.Lerp(cardRect.localRotation,
                    Quaternion.Euler(0, 0, targetRotation), Time.deltaTime * moveSpeed);

                // CardUI에 원래 위치 저장
                cardsInHand[i].SetOriginalPosition(targetPosition);
            }
        }

        /// <summary>
        /// 개별 카드의 위치 계산
        /// </summary>
        private Vector3 CalculateCardPosition(int index, int totalCards, float spacing)
        {
            // 중앙을 기준으로 위치 계산
            float centerOffset = (totalCards - 1) * 0.5f;
            float xPosition = (index - centerOffset) * spacing;

            float yPosition = 0f;

            // 곡선 레이아웃이 활성화된 경우
            if (enableCurvedLayout)
            {
                float normalizedPosition = totalCards > 1 ? (float)index / (totalCards - 1) : 0.5f;
                float curveValue = layoutCurve.Evaluate(normalizedPosition);

                // 아래로 휘어진 곡선 생성
                yPosition = -curveHeight * (1f - Mathf.Abs(normalizedPosition * 2f - 1f));
            }

            return new Vector3(xPosition, yPosition, 0);
        }

        /// <summary>
        /// 개별 카드의 회전 각도 계산
        /// </summary>
        private float CalculateCardRotation(int index, int totalCards)
        {
            if (!enableCurvedLayout) return 0f;

            // 중앙을 기준으로 회전
            float centerOffset = (totalCards - 1) * 0.5f;
            float rotationFactor = (index - centerOffset) / Mathf.Max(totalCards - 1, 1);

            return rotationFactor * rotationAmount;
        }

        /// <summary>
        /// 손패의 모든 카드 가져오기
        /// </summary>
        public List<CardUI> GetCardsInHand()
        {
            return new List<CardUI>(cardsInHand);
        }

        /// <summary>
        /// 손패의 카드 수 가져오기
        /// </summary>
        public int GetCardCount()
        {
            return cardsInHand.Count;
        }

        /// <summary>
        /// 모든 카드의 사용 가능 여부 업데이트
        /// </summary>
        public void UpdateAllCardPlayability()
        {
            foreach (var card in cardsInHand)
            {
                if (card != null)
                {
                    card.UpdatePlayability();
                }
            }
        }

        /// <summary>
        /// 손패를 다시 정렬
        /// </summary>
        public void ReorganizeHand()
        {
            // null 참조 제거
            cardsInHand.RemoveAll(card => card == null);

            // 카드 타입별로 정렬 (선택사항)
            // cardsInHand.Sort((a, b) => a.CardData.Type.CompareTo(b.CardData.Type));

            UpdateCardPositions();
        }

        /// <summary>
        /// 카드 드로우 애니메이션
        /// </summary>
        public void PlayDrawAnimation(CardUI card)
        {
            if (card == null) return;

            StartCoroutine(DrawCardAnimation(card));
        }

        /// <summary>
        /// 카드 드로우 애니메이션 코루틴
        /// </summary>
        private System.Collections.IEnumerator DrawCardAnimation(CardUI card)
        {
            RectTransform cardRect = card.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = card.gameObject.AddComponent<CanvasGroup>();
            }

            // 덱 위치에서 시작 (화면 오른쪽 하단)
            Vector3 startPosition = new Vector3(500f, -300f, 0);
            cardRect.localPosition = startPosition;
            canvasGroup.alpha = 0f;

            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                canvasGroup.alpha = t;

                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 손패 흔들림 효과 (카드를 사용할 수 없을 때)
        /// </summary>
        public void ShakeHand()
        {
            StartCoroutine(ShakeHandCoroutine());
        }

        private System.Collections.IEnumerator ShakeHandCoroutine()
        {
            Vector3 originalPosition = transform.localPosition;
            float duration = 0.3f;
            float elapsed = 0f;
            float magnitude = 5f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = originalPosition + new Vector3(x, 0, 0);

                elapsed += Time.deltaTime;
                magnitude = Mathf.Lerp(5f, 0f, elapsed / duration);

                yield return null;
            }

            transform.localPosition = originalPosition;
        }
    }
}
