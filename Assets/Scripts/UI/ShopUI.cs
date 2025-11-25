using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Shop;
using JianghuGuidebook.Economy;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Map;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 상점 UI를 관리하는 컴포넌트
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private GameObject shopItemPrefab;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Button exitButton;

        [Header("구매 확인 다이얼로그")]
        [SerializeField] private GameObject confirmDialog;
        [SerializeField] private TextMeshProUGUI confirmItemNameText;
        [SerializeField] private TextMeshProUGUI confirmPriceText;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        [Header("상점 정보")]
        [SerializeField] private TextMeshProUGUI shopTitleText;
        [SerializeField] private TextMeshProUGUI shopDescriptionText;

        private List<ShopItemUI> shopItemUIs = new List<ShopItemUI>();
        private ShopItem pendingPurchaseItem;

        private void Start()
        {
            // 이벤트 구독
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.OnShopGenerated += OnShopGenerated;
                ShopManager.Instance.OnItemPurchased += OnItemPurchased;
            }

            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.OnGoldChanged += OnGoldChanged;
            }

            // 버튼 이벤트
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(OnExitButtonClicked);
            }

            if (confirmYesButton != null)
            {
                confirmYesButton.onClick.AddListener(OnConfirmYes);
            }

            if (confirmNoButton != null)
            {
                confirmNoButton.onClick.AddListener(OnConfirmNo);
            }

            // 확인 다이얼로그 숨김
            if (confirmDialog != null)
            {
                confirmDialog.SetActive(false);
            }

            // 상점 정보 설정
            if (shopTitleText != null)
            {
                shopTitleText.text = "강호 상점";
            }

            if (shopDescriptionText != null)
            {
                shopDescriptionText.text = "무림의 비전과 보물이 가득한 곳";
            }

            // 골드 표시 업데이트
            UpdateGoldDisplay();

            // 상점 생성
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.GenerateShop();
            }
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.OnShopGenerated -= OnShopGenerated;
                ShopManager.Instance.OnItemPurchased -= OnItemPurchased;
            }

            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.OnGoldChanged -= OnGoldChanged;
            }

            // 버튼 이벤트 해제
            if (exitButton != null)
            {
                exitButton.onClick.RemoveListener(OnExitButtonClicked);
            }

            if (confirmYesButton != null)
            {
                confirmYesButton.onClick.RemoveListener(OnConfirmYes);
            }

            if (confirmNoButton != null)
            {
                confirmNoButton.onClick.RemoveListener(OnConfirmNo);
            }
        }

        /// <summary>
        /// 상점이 생성되었을 때 호출
        /// </summary>
        private void OnShopGenerated(List<ShopItem> items)
        {
            RefreshShopDisplay(items);
        }

        /// <summary>
        /// 아이템이 구매되었을 때 호출
        /// </summary>
        private void OnItemPurchased(ShopItem item)
        {
            Debug.Log($"[ShopUI] 아이템 구매됨: {item.GetName()}");
            RefreshShopDisplay(ShopManager.Instance.CurrentShopItems);
        }

        /// <summary>
        /// 골드가 변경되었을 때 호출
        /// </summary>
        private void OnGoldChanged(int currentGold)
        {
            UpdateGoldDisplay();
        }

        /// <summary>
        /// 상점 상품을 표시합니다
        /// </summary>
        public void RefreshShopDisplay(List<ShopItem> items)
        {
            // 기존 아이템 제거
            foreach (var itemUI in shopItemUIs)
            {
                if (itemUI != null && itemUI.gameObject != null)
                {
                    Destroy(itemUI.gameObject);
                }
            }
            shopItemUIs.Clear();

            // 새 아이템 생성
            if (shopItemPrefab != null && itemContainer != null)
            {
                foreach (var item in items)
                {
                    GameObject itemObj = Instantiate(shopItemPrefab, itemContainer);
                    ShopItemUI itemUI = itemObj.GetComponent<ShopItemUI>();

                    if (itemUI != null)
                    {
                        itemUI.Setup(item, this);
                        shopItemUIs.Add(itemUI);
                    }
                }
            }

            Debug.Log($"[ShopUI] 상품 표시: {items.Count}개");
        }

        /// <summary>
        /// 골드 표시를 업데이트합니다
        /// </summary>
        private void UpdateGoldDisplay()
        {
            if (goldText != null && GoldManager.Instance != null)
            {
                goldText.text = $"골드: {GoldManager.Instance.CurrentGold}";
            }
        }

        /// <summary>
        /// 구매 확인 다이얼로그를 표시합니다
        /// </summary>
        public void ShowPurchaseConfirmation(ShopItem item)
        {
            if (confirmDialog == null || item == null)
                return;

            pendingPurchaseItem = item;

            // 다이얼로그 정보 설정
            if (confirmItemNameText != null)
            {
                confirmItemNameText.text = item.GetName();
            }

            if (confirmPriceText != null)
            {
                confirmPriceText.text = $"{item.price} 골드를 소비하여 구매하시겠습니까?";
            }

            // 다이얼로그 표시
            confirmDialog.SetActive(true);
        }

        /// <summary>
        /// 구매 확인 (예)
        /// </summary>
        private void OnConfirmYes()
        {
            if (pendingPurchaseItem != null && ShopManager.Instance != null)
            {
                bool success = ShopManager.Instance.PurchaseItem(pendingPurchaseItem);

                if (!success)
                {
                    // 구매 실패 (골드 부족 등)
                    Debug.Log("구매 실패!");
                    // TODO: 에러 메시지 표시
                }

                pendingPurchaseItem = null;
            }

            // 다이얼로그 숨김
            if (confirmDialog != null)
            {
                confirmDialog.SetActive(false);
            }
        }

        /// <summary>
        /// 구매 취소 (아니오)
        /// </summary>
        private void OnConfirmNo()
        {
            pendingPurchaseItem = null;

            if (confirmDialog != null)
            {
                confirmDialog.SetActive(false);
            }
        }

        /// <summary>
        /// 나가기 버튼 클릭
        /// </summary>
        private void OnExitButtonClicked()
        {
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.ExitShop();
            }

            // 맵으로 복귀
            if (MapManager.Instance != null)
            {
                MapManager.Instance.CompleteCurrentNode(returnToMap: true);
            }
        }
    }

    /// <summary>
    /// 개별 상점 아이템 UI
    /// </summary>
    public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI 참조")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image borderImage;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private GameObject soldOutOverlay;

        [Header("색상")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color cardColor = new Color(0.8f, 0.9f, 1f);
        [SerializeField] private Color relicColor = new Color(1f, 0.9f, 0.6f);
        [SerializeField] private Color serviceColor = new Color(0.9f, 1f, 0.9f);
        [SerializeField] private Color soldOutColor = Color.gray;

        private ShopItem item;
        private ShopUI shopUI;
        private bool isHovering = false;

        private void Awake()
        {
            if (purchaseButton != null)
            {
                purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
            }

            if (soldOutOverlay != null)
            {
                soldOutOverlay.SetActive(false);
            }
        }

        private void Update()
        {
            // 호버 애니메이션 (선택적)
            if (isHovering && !item.isPurchased)
            {
                // 약간의 확대 효과 등
            }
        }

        public void Setup(ShopItem item, ShopUI shopUI)
        {
            this.item = item;
            this.shopUI = shopUI;

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (item == null)
                return;

            // 이름
            if (itemNameText != null)
            {
                itemNameText.text = item.GetName();
            }

            // 설명
            if (itemDescriptionText != null)
            {
                itemDescriptionText.text = item.GetDescription();
            }

            // 가격
            if (priceText != null)
            {
                priceText.text = $"{item.price} 골드";
            }

            // 아이콘 설정
            SetItemIcon();

            // 배경 색상
            if (backgroundImage != null)
            {
                Color bgColor = GetItemTypeColor();
                bgColor.a = 0.2f;
                backgroundImage.color = bgColor;
            }

            // 테두리 색상
            if (borderImage != null)
            {
                borderImage.color = GetItemTypeColor();
            }

            // 구매 여부에 따라 UI 업데이트
            if (item.isPurchased)
            {
                if (soldOutOverlay != null)
                {
                    soldOutOverlay.SetActive(true);
                }

                if (purchaseButton != null)
                {
                    purchaseButton.interactable = false;
                }
            }
            else
            {
                // 골드 부족 체크
                bool canAfford = GoldManager.Instance != null &&
                                GoldManager.Instance.HasEnoughGold(item.price);

                if (purchaseButton != null)
                {
                    purchaseButton.interactable = canAfford;
                }
            }
        }

        private void SetItemIcon()
        {
            if (itemIcon == null)
                return;

            switch (item.itemType)
            {
                case ShopItemType.Card:
                    // TODO: 카드 아이콘 설정
                    break;

                case ShopItemType.Relic:
                    if (RelicIconManager.Instance != null && item.relic != null)
                    {
                        itemIcon.sprite = RelicIconManager.Instance.GetRelicIcon(item.relic);
                    }
                    break;

                case ShopItemType.CardRemoval:
                case ShopItemType.CardUpgrade:
                case ShopItemType.HealthPotion:
                    // TODO: 서비스/물약 아이콘 설정
                    break;
            }
        }

        private Color GetItemTypeColor()
        {
            switch (item.itemType)
            {
                case ShopItemType.Card:
                    return cardColor;
                case ShopItemType.Relic:
                    return relicColor;
                case ShopItemType.CardRemoval:
                case ShopItemType.CardUpgrade:
                case ShopItemType.HealthPotion:
                    return serviceColor;
                default:
                    return normalColor;
            }
        }

        private void OnPurchaseButtonClicked()
        {
            if (item != null && shopUI != null && !item.isPurchased)
            {
                shopUI.ShowPurchaseConfirmation(item);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;

            // 툴팁 표시 (선택적)
            if (item.itemType == ShopItemType.Relic && item.relic != null)
            {
                if (RelicTooltip.Instance != null)
                {
                    RelicTooltip.Instance.Show(item.relic, eventData.position);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;

            // 툴팁 숨김
            if (RelicTooltip.Instance != null)
            {
                RelicTooltip.Instance.Hide();
            }
        }

        private void OnDestroy()
        {
            if (purchaseButton != null)
            {
                purchaseButton.onClick.RemoveListener(OnPurchaseButtonClicked);
            }
        }
    }
}
