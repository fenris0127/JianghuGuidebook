using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Data;
using JianghuGuidebook.Rewards;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 보상 화면 UI를 관리합니다.
    /// </summary>
    public class RewardUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform rewardsContainer;
        [SerializeField] private GameObject rewardItemPrefab; // 버튼 형태의 프리팹
        [SerializeField] private Button proceedButton;

        [Header("Card Selection")]
        [SerializeField] private GameObject cardSelectionPanel;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject cardPrefab; // CardUI 프리팹
        [SerializeField] private TextMeshProUGUI cardSelectionTitle;
        [SerializeField] private Button skipCardButton;

        [Header("Card Tooltip")]
        [SerializeField] private GameObject cardTooltipPanel;
        [SerializeField] private RectTransform tooltipRect;
        [SerializeField] private TextMeshProUGUI tooltipNameText;
        [SerializeField] private TextMeshProUGUI tooltipCostText;
        [SerializeField] private TextMeshProUGUI tooltipTypeText;
        [SerializeField] private TextMeshProUGUI tooltipRarityText;
        [SerializeField] private TextMeshProUGUI tooltipDescriptionText;
        [SerializeField] private Image tooltipBackground;
        [SerializeField] private Image tooltipBorder;
        [SerializeField] private CanvasGroup tooltipCanvasGroup;

        [Header("Tooltip Settings")]
        [SerializeField] private Vector2 tooltipOffset = new Vector2(10f, -10f);
        [SerializeField] private float tooltipFadeSpeed = 8f;
        [SerializeField] private float edgePadding = 20f;

        [Header("Skip Confirmation Dialog")]
        [SerializeField] private GameObject skipConfirmDialog;
        [SerializeField] private TextMeshProUGUI skipConfirmMessageText;
        [SerializeField] private Button skipConfirmYesButton;
        [SerializeField] private Button skipConfirmNoButton;

        private List<RewardItem> currentRewards;
        private RewardItem currentCardRewardItem; // 현재 선택 중인 카드 보상 항목
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                canvas = FindObjectOfType<Canvas>();

            // 툴팁 초기화
            if (tooltipCanvasGroup == null && cardTooltipPanel != null)
            {
                tooltipCanvasGroup = cardTooltipPanel.GetComponent<CanvasGroup>();
                if (tooltipCanvasGroup == null)
                    tooltipCanvasGroup = cardTooltipPanel.AddComponent<CanvasGroup>();
            }

            if (tooltipRect == null && cardTooltipPanel != null)
            {
                tooltipRect = cardTooltipPanel.GetComponent<RectTransform>();
            }
        }

        private void Start()
        {
            if (proceedButton != null)
                proceedButton.onClick.AddListener(OnProceedClicked);

            if (skipCardButton != null)
                skipCardButton.onClick.AddListener(OnSkipCardClicked);

            // Skip 확인 다이얼로그 버튼
            if (skipConfirmYesButton != null)
                skipConfirmYesButton.onClick.AddListener(OnSkipConfirmYes);

            if (skipConfirmNoButton != null)
                skipConfirmNoButton.onClick.AddListener(OnSkipConfirmNo);

            // 초기에는 숨김
            if (panel != null) panel.SetActive(false);
            if (cardSelectionPanel != null) cardSelectionPanel.SetActive(false);

            // 툴팁 숨김
            if (cardTooltipPanel != null)
            {
                cardTooltipPanel.SetActive(false);
                if (tooltipCanvasGroup != null)
                    tooltipCanvasGroup.alpha = 0f;
            }

            // Skip 확인 다이얼로그 숨김
            if (skipConfirmDialog != null)
            {
                skipConfirmDialog.SetActive(false);
            }
        }

        private void Update()
        {
            // 툴팁 페이드 애니메이션
            if (cardTooltipPanel != null && tooltipCanvasGroup != null)
            {
                float targetAlpha = cardTooltipPanel.activeSelf ? 1f : 0f;
                tooltipCanvasGroup.alpha = Mathf.Lerp(tooltipCanvasGroup.alpha, targetAlpha, Time.deltaTime * tooltipFadeSpeed);
            }
        }

        /// <summary>
        /// 보상 목록을 표시합니다.
        /// </summary>
        public void ShowRewards(List<RewardItem> rewards)
        {
            currentRewards = rewards;
            panel.SetActive(true);
            cardSelectionPanel.SetActive(false);

            // 기존 항목 제거
            foreach (Transform child in rewardsContainer)
            {
                Destroy(child.gameObject);
            }

            // 보상 항목 생성
            foreach (var reward in rewards)
            {
                GameObject obj = Instantiate(rewardItemPrefab, rewardsContainer);
                Button btn = obj.GetComponent<Button>();
                TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();

                if (text != null)
                {
                    text.text = reward.ToString();
                }

                if (btn != null)
                {
                    btn.onClick.AddListener(() => OnRewardClicked(reward, obj));
                }
            }
        }

        private void OnRewardClicked(RewardItem reward, GameObject buttonObj)
        {
            if (reward.type == RewardType.Card)
            {
                // 카드 선택 화면 표시
                currentCardRewardItem = reward;
                ShowCardSelection(RewardManager.Instance.CurrentCardChoices);
                
                // 카드 보상은 선택 후 버튼 제거 (여기서는 일단 유지하고 선택 완료 시 제거)
            }
            else
            {
                // 즉시 획득 (골드, 유물)
                RewardManager.Instance.ClaimReward(reward);
                
                // 버튼 제거 또는 비활성화
                Destroy(buttonObj);
                
                // 리스트에서 제거 (옵션)
                currentRewards.Remove(reward);
            }
        }

        private void ShowCardSelection(List<CardData> cards)
        {
            cardSelectionPanel.SetActive(true);
            panel.SetActive(false); // 보상 목록 잠시 숨김

            foreach (Transform child in cardContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var cardData in cards)
            {
                GameObject obj = Instantiate(cardPrefab, cardContainer);

                // CardUI 초기화 (CardUI 스크립트가 있다고 가정)
                var cardUI = obj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    // CardUI가 있으면 초기화는 하지만, 드래그 기능은 비활성화
                    // (보상 화면에서는 클릭만 필요)
                }

                // RewardCardItem 컴포넌트 추가 (호버 이벤트 처리)
                var rewardCardItem = obj.GetComponent<RewardCardItem>();
                if (rewardCardItem == null)
                {
                    rewardCardItem = obj.AddComponent<RewardCardItem>();
                }
                rewardCardItem.Setup(cardData, this);

                Button btn = obj.GetComponent<Button>();
                if (btn == null) btn = obj.AddComponent<Button>();

                btn.onClick.AddListener(() => OnCardSelected(cardData));
            }
        }

        private void OnCardSelected(CardData cardData)
        {
            RewardManager.Instance.SelectCard(cardData);
            
            // 카드 보상 획득 처리
            if (currentCardRewardItem != null)
            {
                RewardManager.Instance.ClaimReward(currentCardRewardItem);
                currentRewards.Remove(currentCardRewardItem);
                currentCardRewardItem = null;
            }

            CloseCardSelection();
        }

        private void OnSkipCardClicked()
        {
            // 건너뛰기 확인 다이얼로그 표시
            ShowSkipConfirmDialog();
        }

        /// <summary>
        /// 건너뛰기 확인 다이얼로그를 표시합니다
        /// </summary>
        private void ShowSkipConfirmDialog()
        {
            if (skipConfirmDialog == null)
            {
                Debug.LogWarning("Skip 확인 다이얼로그가 설정되지 않았습니다. 바로 건너뜁니다.");
                ConfirmSkipCard();
                return;
            }

            // 다이얼로그 메시지 설정
            if (skipConfirmMessageText != null)
            {
                skipConfirmMessageText.text = "카드 보상을 건너뛰시겠습니까?\n선택하지 않으면 카드를 획득할 수 없습니다.";
            }

            // 다이얼로그 표시
            skipConfirmDialog.SetActive(true);

            Debug.Log("[RewardUI] 건너뛰기 확인 다이얼로그 표시");
        }

        /// <summary>
        /// 건너뛰기 확인 (예)
        /// </summary>
        private void OnSkipConfirmYes()
        {
            // 다이얼로그 숨김
            if (skipConfirmDialog != null)
            {
                skipConfirmDialog.SetActive(false);
            }

            // 실제로 건너뛰기 실행
            ConfirmSkipCard();

            Debug.Log("[RewardUI] 카드 보상 건너뛰기 확정");
        }

        /// <summary>
        /// 건너뛰기 취소 (아니오)
        /// </summary>
        private void OnSkipConfirmNo()
        {
            // 다이얼로그만 숨김
            if (skipConfirmDialog != null)
            {
                skipConfirmDialog.SetActive(false);
            }

            Debug.Log("[RewardUI] 건너뛰기 취소");
        }

        /// <summary>
        /// 카드 건너뛰기를 확정합니다
        /// </summary>
        private void ConfirmSkipCard()
        {
            // 카드 보상 포기
            if (currentCardRewardItem != null)
            {
                currentRewards.Remove(currentCardRewardItem);
                currentCardRewardItem = null;
            }

            CloseCardSelection();
        }

        private void CloseCardSelection()
        {
            cardSelectionPanel.SetActive(false);
            
            // 보상 목록 다시 표시 (남은 보상이 있다면)
            if (currentRewards.Count > 0)
            {
                ShowRewards(currentRewards); // 다시 그리기 (제거된 항목 반영)
            }
            else
            {
                // 모든 보상 획득함
                OnProceedClicked();
            }
        }

        private void OnProceedClicked()
        {
            RewardManager.Instance.CompleteRewards();

            // 맵으로 복귀
            JianghuGuidebook.Map.MapManager.Instance.ReturnToMap();
        }

        /// <summary>
        /// 카드 툴팁을 표시합니다
        /// </summary>
        public void ShowCardTooltip(CardData cardData, Vector2 screenPosition)
        {
            if (cardTooltipPanel == null || cardData == null)
                return;

            // 툴팁 내용 설정
            UpdateTooltipContent(cardData);

            // 툴팁 위치 설정
            SetTooltipPosition(screenPosition);

            // 툴팁 표시
            cardTooltipPanel.SetActive(true);
        }

        /// <summary>
        /// 카드 툴팁을 숨깁니다
        /// </summary>
        public void HideCardTooltip()
        {
            if (cardTooltipPanel != null)
            {
                cardTooltipPanel.SetActive(false);
            }
        }

        /// <summary>
        /// 툴팁 내용을 업데이트합니다
        /// </summary>
        private void UpdateTooltipContent(CardData cardData)
        {
            // 카드 이름
            if (tooltipNameText != null)
            {
                tooltipNameText.text = cardData.name;
                tooltipNameText.color = GetRarityColor(cardData.rarity);
            }

            // 비용
            if (tooltipCostText != null)
            {
                tooltipCostText.text = $"비용: {cardData.cost}";
            }

            // 타입
            if (tooltipTypeText != null)
            {
                tooltipTypeText.text = $"종류: {GetCardTypeText(cardData.type)}";
                tooltipTypeText.color = GetCardTypeColor(cardData.type);
            }

            // 희귀도
            if (tooltipRarityText != null)
            {
                tooltipRarityText.text = GetRarityText(cardData.rarity);
                tooltipRarityText.color = GetRarityColor(cardData.rarity);
            }

            // 설명
            if (tooltipDescriptionText != null)
            {
                tooltipDescriptionText.text = cardData.description;
            }

            // 배경 색상 (희귀도 기반)
            if (tooltipBackground != null)
            {
                Color bgColor = GetRarityColor(cardData.rarity);
                bgColor.a = 0.15f;
                tooltipBackground.color = bgColor;
            }

            // 테두리 색상 (카드 타입 기반)
            if (tooltipBorder != null)
            {
                tooltipBorder.color = GetCardTypeColor(cardData.type);
            }
        }

        /// <summary>
        /// 툴팁 위치를 설정합니다 (화면 경계 체크 포함)
        /// </summary>
        private void SetTooltipPosition(Vector2 screenPosition)
        {
            if (tooltipRect == null || canvas == null)
                return;

            // 스크린 좌표를 캔버스 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out Vector2 localPosition
            );

            // 오프셋 적용
            localPosition += tooltipOffset;

            // 캔버스 크기
            RectTransform canvasRect = canvas.transform as RectTransform;
            Vector2 canvasSize = canvasRect.sizeDelta;

            // 툴팁 크기
            Vector2 tooltipSize = tooltipRect.sizeDelta;

            // 화면 경계 체크 (우측)
            if (localPosition.x + tooltipSize.x / 2 > canvasSize.x / 2 - edgePadding)
            {
                localPosition.x = canvasSize.x / 2 - tooltipSize.x / 2 - edgePadding;
            }

            // 화면 경계 체크 (좌측)
            if (localPosition.x - tooltipSize.x / 2 < -canvasSize.x / 2 + edgePadding)
            {
                localPosition.x = -canvasSize.x / 2 + tooltipSize.x / 2 + edgePadding;
            }

            // 화면 경계 체크 (상단)
            if (localPosition.y + tooltipSize.y / 2 > canvasSize.y / 2 - edgePadding)
            {
                localPosition.y = canvasSize.y / 2 - tooltipSize.y / 2 - edgePadding;
            }

            // 화면 경계 체크 (하단)
            if (localPosition.y - tooltipSize.y / 2 < -canvasSize.y / 2 + edgePadding)
            {
                localPosition.y = -canvasSize.y / 2 + tooltipSize.y / 2 + edgePadding;
            }

            tooltipRect.localPosition = localPosition;
        }

        /// <summary>
        /// 카드 타입 텍스트 반환
        /// </summary>
        private string GetCardTypeText(CardType type)
        {
            switch (type)
            {
                case CardType.Attack:
                    return "공격";
                case CardType.Defense:
                    return "방어";
                case CardType.Skill:
                    return "기술";
                case CardType.Secret:
                    return "비전";
                default:
                    return "알 수 없음";
            }
        }

        /// <summary>
        /// 카드 타입 색상 반환
        /// </summary>
        private Color GetCardTypeColor(CardType type)
        {
            switch (type)
            {
                case CardType.Attack:
                    return new Color(0.9f, 0.3f, 0.3f); // 빨강
                case CardType.Defense:
                    return new Color(0.3f, 0.6f, 0.9f); // 파랑
                case CardType.Skill:
                    return new Color(0.4f, 0.8f, 0.4f); // 초록
                case CardType.Secret:
                    return new Color(0.8f, 0.4f, 0.9f); // 보라
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// 희귀도 텍스트 반환
        /// </summary>
        private string GetRarityText(CardRarity rarity)
        {
            switch (rarity)
            {
                case CardRarity.Common:
                    return "【평범】";
                case CardRarity.Uncommon:
                    return "【고급】";
                case CardRarity.Rare:
                    return "【진귀】";
                case CardRarity.Epic:
                    return "【영웅】";
                case CardRarity.Legendary:
                    return "【전설】";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 희귀도 색상 반환
        /// </summary>
        private Color GetRarityColor(CardRarity rarity)
        {
            switch (rarity)
            {
                case CardRarity.Common:
                    return new Color(0.7f, 0.7f, 0.7f); // 회색
                case CardRarity.Uncommon:
                    return new Color(0.3f, 0.9f, 0.3f); // 초록
                case CardRarity.Rare:
                    return new Color(0.3f, 0.6f, 1f); // 파랑
                case CardRarity.Epic:
                    return new Color(0.8f, 0.3f, 1f); // 보라
                case CardRarity.Legendary:
                    return new Color(1f, 0.8f, 0.2f); // 금색
                default:
                    return Color.white;
            }
        }
    }

    /// <summary>
    /// 보상 카드 아이템 - 호버 이벤트 처리
    /// </summary>
    public class RewardCardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CardData cardData;
        private RewardUI rewardUI;

        /// <summary>
        /// 카드 데이터와 RewardUI 참조 설정
        /// </summary>
        public void Setup(CardData cardData, RewardUI rewardUI)
        {
            this.cardData = cardData;
            this.rewardUI = rewardUI;
        }

        /// <summary>
        /// 마우스 호버 시 툴팁 표시
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (rewardUI != null && cardData != null)
            {
                rewardUI.ShowCardTooltip(cardData, eventData.position);
            }
        }

        /// <summary>
        /// 마우스 나갈 때 툴팁 숨김
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (rewardUI != null)
            {
                rewardUI.HideCardTooltip();
            }
        }
    }
}
