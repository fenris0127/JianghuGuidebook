using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Economy;

namespace JianghuGuidebook.Shop
{
    /// <summary>
    /// 상점 화면의 전체 UI를 관리합니다.
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform itemsContainer; // 아이템들이 배치될 부모 객체
        [SerializeField] private GameObject itemPrefab; // ShopItemUI 프리팹
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Button leaveButton;
        [SerializeField] private CardRemovalUI cardRemovalUI;

        [Header("Message UI")]
        [SerializeField] private TextMeshProUGUI messageText; // 구매 성공/실패 메시지

        private List<ShopItemUI> itemUIs = new List<ShopItemUI>();

        private void Start()
        {
            // 이벤트 구독
            ShopManager.Instance.OnShopGenerated += GenerateShopUI;
            ShopManager.Instance.OnItemPurchased += OnItemPurchased;
            
            // 골드 변경 이벤트 구독 (GoldManager에 이벤트가 있다고 가정)
            // GoldManager.Instance.OnGoldChanged += UpdateGoldUI;

            if (leaveButton != null)
                leaveButton.onClick.AddListener(OnLeaveButtonClicked);

            if (cardRemovalUI != null)
                cardRemovalUI.Initialize(this);

            // 이미 상점이 생성되어 있다면 UI 생성
            if (ShopManager.Instance.CurrentShopItems.Count > 0)
            {
                GenerateShopUI(ShopManager.Instance.CurrentShopItems);
            }
            else
            {
                // 상점이 생성되지 않았다면 생성 요청 (테스트용)
                ShopManager.Instance.GenerateShop();
            }

            UpdateGoldUI();
        }

        private void OnDestroy()
        {
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.OnShopGenerated -= GenerateShopUI;
                ShopManager.Instance.OnItemPurchased -= OnItemPurchased;
            }
        }

        /// <summary>
        /// 상점 UI를 생성합니다.
        /// </summary>
        public void GenerateShopUI(List<ShopItem> items)
        {
            ClearShop();

            foreach (var item in items)
            {
                GameObject obj = Instantiate(itemPrefab, itemsContainer);
                ShopItemUI ui = obj.GetComponent<ShopItemUI>();
                ui.Initialize(item, this);
                itemUIs.Add(ui);
            }
        }

        private void ClearShop()
        {
            foreach (Transform child in itemsContainer)
            {
                Destroy(child.gameObject);
            }
            itemUIs.Clear();
        }

        /// <summary>
        /// 아이템 구매를 시도합니다.
        /// </summary>
        public void TryPurchaseItem(ShopItem item)
        {
            bool success = ShopManager.Instance.PurchaseItem(item);
            
            if (success)
            {
                ShowMessage($"구매 완료: {item.GetName()}");
                UpdateGoldUI();
                
                // 특수 서비스 처리
                if (item.itemType == ShopItemType.CardRemoval)
                {
                    if (cardRemovalUI != null) cardRemovalUI.Open();
                }
            }
            else
            {
                ShowMessage("구매 실패 (골드 부족)");
            }
        }

        private void OnItemPurchased(ShopItem item)
        {
            // 모든 아이템 UI 갱신 (골드 변경으로 인한 구매 가능 여부 변경 등)
            foreach (var ui in itemUIs)
            {
                ui.UpdateUI();
            }
        }

        private void UpdateGoldUI()
        {
            if (goldText != null)
            {
                goldText.text = $"{GoldManager.Instance.CurrentGold} G";
            }
        }

        private void ShowMessage(string msg)
        {
            if (messageText != null)
            {
                messageText.text = msg;
                // TODO: 페이드 아웃 애니메이션
                Invoke("ClearMessage", 2f);
            }
        }

        private void ClearMessage()
        {
            if (messageText != null) messageText.text = "";
        }

        private void OnLeaveButtonClicked()
        {
            ShopManager.Instance.ExitShop();
        }

        /// <summary>
        /// 카드 제거 완료 시 호출
        /// </summary>
        public void OnCardRemoved()
        {
            ShowMessage("카드가 제거되었습니다.");
        }
    }
}
