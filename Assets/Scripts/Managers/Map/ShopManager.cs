using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

// 상점의 상품 생성 및 상호작용을 관리하는 클래스입니다.
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("상점 설정")]
    [SerializeField] private int numberOfCardsForSale = 3;
    [SerializeField] private int removeServiceCost = 50;

    [Header("UI 요소")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private DeckViewUI deckViewUI;
    [SerializeField] private Button removeServiceButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Transform cardSlotsContainer;
    [SerializeField] private GameObject shopCardSlotPrefab;

    private Player player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<Player>(true);
        shopPanel.SetActive(false);
        removeServiceButton.onClick.AddListener(OnRemoveServiceButtonClicked);
        leaveButton.onClick.AddListener(CloseShop);
    }

    public void OpenShop()
    {
        GameManager.Instance.ChangeState(GameState.Shop);
        GenerateCardStock();
    }

    private void GenerateCardStock()
    {
        foreach(Transform child in cardSlotsContainer) Destroy(child.gameObject);

        var cardsForSale = ResourceManager.Instance.GetAllCards()
            .Where(c => !c.isUpgraded && c.rarity <= CardRarity.Rare)
            .OrderBy(c => Random.value)
            .Take(numberOfCardsForSale)
            .ToList();

        foreach (var card in cardsForSale)
        {
            GameObject slotObj = Instantiate(shopCardSlotPrefab, cardSlotsContainer);
            slotObj.GetComponent<ShopCardSlot>().Setup(card, this);
        }
    }

    public void TryBuyCard(ShopCardSlot slot)
    {
        CardData card = slot.GetCardData();
        if (player.SpendGold(card.goldCost))
        {
            player.AddCardToDeck(card);
            slot.SetPurchased();
        }
    }
    
    public void OnRemoveServiceButtonClicked()
    {
        if (player.gold >= removeServiceCost)
        {
            deckViewUI.Show(player.GetAllCardsInDeck(), "제거할 카드를 선택하세요", card => {
                if (player.SpendGold(removeServiceCost))
                    player.RemoveCardFromDeck(card);
                deckViewUI.Hide();
            });
        }
    }

    public void CloseShop()
    {
        GameManager.Instance.ChangeState(GameState.MapView);
    }
}