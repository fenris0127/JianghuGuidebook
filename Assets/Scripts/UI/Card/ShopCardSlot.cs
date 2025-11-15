// Folder: Scripts/UI/ShopCardSlot.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 상점에서 판매되는 카드 슬롯 하나의 UI와 상호작용을 제어합니다.
/// </summary>
public class ShopCardSlot : MonoBehaviour
{
    [SerializeField] private CardUI cardUI;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject soldOutOverlay; // '판매 완료' 위에 덮어씌울 반투명 이미지

    private CardData cardData;
    private ShopManager shopManager;

    void Awake()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public void Setup(CardData data, ShopManager manager)
    {
        this.cardData = data;
        this.shopManager = manager;

        cardUI.Setup(data);
        costText.text = data.goldCost.ToString();
        soldOutOverlay.SetActive(false);
        buyButton.interactable = true;
    }

    private void OnBuyButtonClicked() => shopManager.TryBuyCard(this);

    public CardData GetCardData() => cardData;

    public void SetPurchased()
    {
        soldOutOverlay.SetActive(true);
        buyButton.interactable = false;
    }
}