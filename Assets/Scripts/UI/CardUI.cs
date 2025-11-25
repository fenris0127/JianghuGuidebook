using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 개별 카드의 UI 표시 및 상호작용을 담당하는 컴포넌트
    /// </summary>
    public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
                          IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI References")]
        [SerializeField] private Image cardBackground;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image typeIcon;
        [SerializeField] private Image rarityBorder;

        [Header("Visual Settings")]
        [SerializeField] private float hoverScale = 1.2f;
        [SerializeField] private float hoverLiftHeight = 50f;
        [SerializeField] private float animationSpeed = 10f;

        [Header("Drag Settings")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private float dragAlpha = 0.7f;

        private Cards.Card cardData;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector3 originalPosition;
        private Vector3 originalScale;
        private int originalSiblingIndex;
        private bool isHovered = false;
        private bool isDragging = false;
        private bool isPlayable = true;

        public Cards.Card CardData => cardData;
        public bool IsPlayable => isPlayable;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            if (canvas == null)
            {
                canvas = GetComponentInParent<Canvas>();
            }

            originalScale = transform.localScale;
        }

        private void Update()
        {
            // 부드러운 호버 애니메이션
            if (isHovered && !isDragging)
            {
                Vector3 targetPosition = originalPosition + Vector3.up * hoverLiftHeight;
                Vector3 targetScale = originalScale * hoverScale;

                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * animationSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
            }
            else if (!isDragging)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * animationSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
            }
        }

        /// <summary>
        /// 카드 데이터를 기반으로 UI 초기화
        /// </summary>
        public void Initialize(Cards.Card card)
        {
            cardData = card;
            UpdateVisuals();
        }

        /// <summary>
        /// 카드 시각 요소 업데이트
        /// </summary>
        private void UpdateVisuals()
        {
            if (cardData == null) return;

            nameText.text = cardData.Name;
            costText.text = cardData.Cost.ToString();
            descriptionText.text = cardData.Description;

            // 카드 타입에 따른 배경색 설정
            SetCardTypeColor();

            // 희귀도에 따른 테두리 색상 설정
            SetRarityBorderColor();

            // 사용 가능 여부에 따른 투명도 설정
            UpdatePlayability();
        }

        /// <summary>
        /// 카드 타입에 따른 배경색 설정
        /// </summary>
        private void SetCardTypeColor()
        {
            switch (cardData.Type)
            {
                case Data.CardType.Attack:
                    cardBackground.color = new Color(0.8f, 0.2f, 0.2f); // 빨강
                    break;
                case Data.CardType.Defense:
                    cardBackground.color = new Color(0.2f, 0.6f, 0.8f); // 파랑
                    break;
                case Data.CardType.Skill:
                    cardBackground.color = new Color(0.3f, 0.7f, 0.3f); // 초록
                    break;
                case Data.CardType.Secret:
                    cardBackground.color = new Color(0.7f, 0.3f, 0.8f); // 보라
                    break;
            }
        }

        /// <summary>
        /// 희귀도에 따른 테두리 색상 설정
        /// </summary>
        private void SetRarityBorderColor()
        {
            if (rarityBorder == null) return;

            switch (cardData.Rarity)
            {
                case Data.CardRarity.Common:
                    rarityBorder.color = new Color(0.7f, 0.7f, 0.7f); // 회색
                    break;
                case Data.CardRarity.Uncommon:
                    rarityBorder.color = new Color(0.2f, 0.8f, 0.2f); // 초록
                    break;
                case Data.CardRarity.Rare:
                    rarityBorder.color = new Color(0.2f, 0.5f, 1f); // 파랑
                    break;
                case Data.CardRarity.Epic:
                    rarityBorder.color = new Color(0.7f, 0.2f, 0.9f); // 보라
                    break;
                case Data.CardRarity.Legendary:
                    rarityBorder.color = new Color(1f, 0.7f, 0.1f); // 금색
                    break;
            }
        }

        /// <summary>
        /// 카드 사용 가능 여부 업데이트
        /// </summary>
        public void UpdatePlayability()
        {
            var player = FindObjectOfType<Combat.Player>();
            if (player != null)
            {
                isPlayable = player.CurrentEnergy >= cardData.Cost;
            }

            // 사용 불가능한 카드는 어둡게 표시
            canvasGroup.alpha = isPlayable ? 1f : 0.5f;
        }

        /// <summary>
        /// 원래 위치 저장
        /// </summary>
        public void SetOriginalPosition(Vector3 position)
        {
            originalPosition = position;
        }

        #region Event Handlers

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isDragging)
            {
                isHovered = true;
                originalSiblingIndex = transform.GetSiblingIndex();
                transform.SetAsLastSibling(); // 다른 카드 위에 표시
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isDragging)
            {
                isHovered = false;
                transform.SetSiblingIndex(originalSiblingIndex);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isPlayable) return;

            isDragging = true;
            isHovered = false;
            originalPosition = transform.localPosition;
            canvasGroup.alpha = dragAlpha;
            canvasGroup.blocksRaycasts = false;

            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isPlayable) return;

            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isPlayable)
            {
                ResetPosition();
                return;
            }

            isDragging = false;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            // 드롭 위치 확인
            if (IsValidDropTarget(eventData))
            {
                PlayCard();
            }
            else
            {
                ResetPosition();
            }
        }

        #endregion

        /// <summary>
        /// 유효한 드롭 대상인지 확인
        /// </summary>
        private bool IsValidDropTarget(PointerEventData eventData)
        {
            // 화면 상단 절반에 드롭하면 카드 사용
            return eventData.position.y > Screen.height * 0.4f;
        }

        /// <summary>
        /// 카드 사용
        /// </summary>
        private void PlayCard()
        {
            var combatUI = FindObjectOfType<CombatUI>();
            if (combatUI != null)
            {
                combatUI.PlayCard(this);
            }
        }

        /// <summary>
        /// 원래 위치로 돌아가기
        /// </summary>
        private void ResetPosition()
        {
            transform.localPosition = originalPosition;
            transform.localScale = originalScale;
        }

        /// <summary>
        /// 카드 사용 애니메이션
        /// </summary>
        public void PlayAnimation(System.Action onComplete = null)
        {
            StartCoroutine(PlayAnimationCoroutine(onComplete));
        }

        private System.Collections.IEnumerator PlayAnimationCoroutine(System.Action onComplete)
        {
            // 빛나는 효과
            float duration = 0.3f;
            float elapsed = 0f;

            Vector3 targetScale = originalScale * 1.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                canvasGroup.alpha = 1f - t;

                yield return null;
            }

            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
