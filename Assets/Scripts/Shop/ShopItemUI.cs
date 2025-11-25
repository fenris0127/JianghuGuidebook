using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Economy;

namespace JianghuGuidebook.Shop
{
    /// <summary>
    /// 상점의 개별 아이템 UI를 관리합니다.
    /// </summary>
    public class ShopItemUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private GameObject soldOutOverlay;
        [SerializeField] private Image typeIcon; // 카드, 유물 등 타입 아이콘

        [Header("Colors")]
        [SerializeField] private Color affordableColor = Color.white;
        [SerializeField] private Color expensiveColor = Color.red;

        private ShopItem shopItem;
        private ShopUI shopUI;

        /// <summary>
        /// 아이템 UI를 초기화합니다.
        /// </summary>
        public void Initialize(ShopItem item, ShopUI ui)
        {
            this.shopItem = item;
            this.shopUI = ui;

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (shopItem == null) return;

            // 텍스트 설정
            nameText.text = shopItem.GetName();
            descriptionText.text = shopItem.GetDescription();
            priceText.text = $"{shopItem.price} G";

            // 아이콘 설정 (TODO: 실제 아이콘 리소스 연결 필요)
            // if (shopItem.itemType == ShopItemType.Card) itemIcon.sprite = ...
            
            // 판매 완료 상태
            if (shopItem.isPurchased)
            {
                if (soldOutOverlay != null) soldOutOverlay.SetActive(true);
                purchaseButton.interactable = false;
            }
            else
            {
                if (soldOutOverlay != null) soldOutOverlay.SetActive(false);
                
                // 골드 부족 체크
                bool canAfford = GoldManager.Instance.HasEnoughGold(shopItem.price);
                priceText.color = canAfford ? affordableColor : expensiveColor;
                purchaseButton.interactable = canAfford;
            }
        }

        /// <summary>
        /// 구매 버튼 클릭 시 호출됩니다.
        /// </summary>
        public void OnPurchaseClick()
        {
            if (shopItem == null || shopItem.isPurchased) return;

            shopUI.TryPurchaseItem(shopItem);
        }
    }
}
