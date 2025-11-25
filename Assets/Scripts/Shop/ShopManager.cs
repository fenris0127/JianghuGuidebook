using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Economy;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Shop
{
    /// <summary>
    /// 상점 시스템을 관리하는 매니저
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        private static ShopManager _instance;

        public static ShopManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ShopManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("ShopManager");
                        _instance = go.AddComponent<ShopManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("상점 상품")]
        [SerializeField] private List<ShopItem> currentShopItems = new List<ShopItem>();

        [Header("가격 설정")]
        [SerializeField] private int cardRemovalBasePrice = 50;
        [SerializeField] private int cardUpgradePrice = 50;
        [SerializeField] private int healthPotionPrice = 50;
        [SerializeField] private int cardRemovalPriceIncrease = 25; // 구매마다 증가

        private int timesCardRemoved = 0;

        // Properties
        public List<ShopItem> CurrentShopItems => currentShopItems;

        // Events
        public System.Action<List<ShopItem>> OnShopGenerated;
        public System.Action<ShopItem> OnItemPurchased;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        /// <summary>
        /// 상점 상품을 생성합니다
        /// </summary>
        public void GenerateShop()
        {
            currentShopItems.Clear();

            Debug.Log("=== 상점 상품 생성 ===");

            // 1. 카드 5-7장 추가
            int cardCount = Random.Range(5, 8);
            for (int i = 0; i < cardCount; i++)
            {
                CardData card = GetRandomCard();
                if (card != null)
                {
                    int price = CalculateCardPrice(card);
                    currentShopItems.Add(ShopItem.CreateCardItem(card, price));
                }
            }

            // 2. 유물 2-3개 추가
            int relicCount = Random.Range(2, 4);
            for (int i = 0; i < relicCount; i++)
            {
                Relic relic = GetRandomRelic();
                if (relic != null)
                {
                    int price = relic.GetPrice();
                    currentShopItems.Add(ShopItem.CreateRelicItem(relic, price));
                }
            }

            // 3. 카드 제거 서비스
            int removalPrice = cardRemovalBasePrice + (timesCardRemoved * cardRemovalPriceIncrease);
            currentShopItems.Add(ShopItem.CreateCardRemovalService(removalPrice));

            // 4. 카드 업그레이드 서비스
            currentShopItems.Add(ShopItem.CreateCardUpgradeService(cardUpgradePrice));

            // 5. 체력 회복 물약
            currentShopItems.Add(ShopItem.CreateHealthPotion(30, healthPotionPrice));

            Debug.Log($"상점 상품 생성 완료: 총 {currentShopItems.Count}개");

            OnShopGenerated?.Invoke(currentShopItems);
        }

        /// <summary>
        /// 상품을 구매합니다
        /// </summary>
        public bool PurchaseItem(ShopItem item)
        {
            if (item == null)
            {
                Debug.LogError("구매할 아이템이 null입니다");
                return false;
            }

            if (item.isPurchased)
            {
                Debug.LogWarning($"{item.GetName()}은 이미 구매된 상품입니다");
                return false;
            }

            // 골드 체크
            if (!GoldManager.Instance.HasEnoughGold(item.price))
            {
                Debug.LogWarning($"골드 부족: 필요 {item.price}, 현재 {GoldManager.Instance.CurrentGold}");
                return false;
            }

            // 골드 차감
            if (!GoldManager.Instance.SpendGold(item.price))
            {
                return false;
            }

            // 구매 처리
            item.Purchase();

            // 아이템 효과 적용
            ApplyItemEffect(item);

            OnItemPurchased?.Invoke(item);

            Debug.Log($"구매 완료: {item}");
            return true;
        }

        /// <summary>
        /// 아이템 효과를 적용합니다
        /// </summary>
        private void ApplyItemEffect(ShopItem item)
        {
            switch (item.itemType)
            {
                case ShopItemType.Card:
                    // TODO: 덱에 카드 추가
                    Debug.Log($"덱에 카드 추가: {item.cardData.name}");
                    break;

                case ShopItemType.Relic:
                    // 유물 추가
                    RelicManager.Instance.AddRelic(item.relic);
                    break;

                case ShopItemType.CardRemoval:
                    // TODO: 카드 제거 UI 표시
                    timesCardRemoved++;
                    Debug.Log("카드 제거 서비스 구매 (UI 표시 필요)");
                    break;

                case ShopItemType.CardUpgrade:
                    // TODO: 카드 업그레이드 UI 표시
                    Debug.Log("카드 업그레이드 서비스 구매 (UI 표시 필요)");
                    break;

                case ShopItemType.HealthPotion:
                    // 체력 회복
                    Player player = FindObjectOfType<Player>();
                    if (player != null)
                    {
                        int healAmount = Mathf.RoundToInt(player.MaxHealth * item.healAmount / 100f);
                        player.Heal(healAmount);
                        Debug.Log($"체력 회복: {healAmount} HP");
                    }
                    break;
            }
        }

        /// <summary>
        /// 랜덤 카드를 가져옵니다
        /// </summary>
        private CardData GetRandomCard()
        {
            CardData[] allCards = DataManager.Instance.GetAllCards();
            if (allCards.Length == 0)
            {
                Debug.LogError("카드 데이터가 없습니다");
                return null;
            }

            // 등급별 가중치 (Common: 60%, Uncommon: 30%, Rare: 10%)
            int roll = Random.Range(0, 100);
            CardRarity targetRarity;

            if (roll < 60)
                targetRarity = CardRarity.Common;
            else if (roll < 90)
                targetRarity = CardRarity.Uncommon;
            else
                targetRarity = CardRarity.Rare;

            // 해당 등급의 카드 필터링
            CardData[] filteredCards = DataManager.Instance.GetCardsByRarity(targetRarity);

            if (filteredCards.Length == 0)
            {
                // 해당 등급이 없으면 전체에서 랜덤
                return allCards[Random.Range(0, allCards.Length)];
            }

            return filteredCards[Random.Range(0, filteredCards.Length)];
        }

        /// <summary>
        /// 카드 가격을 계산합니다
        /// </summary>
        private int CalculateCardPrice(CardData card)
        {
            int basePrice = 50;

            switch (card.rarity)
            {
                case CardRarity.Common:
                    basePrice = 50;
                    break;
                case CardRarity.Uncommon:
                    basePrice = 75;
                    break;
                case CardRarity.Rare:
                    basePrice = 100;
                    break;
                case CardRarity.Epic:
                    basePrice = 150;
                    break;
                case CardRarity.Legendary:
                    basePrice = 200;
                    break;
            }

            return basePrice;
        }

        /// <summary>
        /// 랜덤 유물을 가져옵니다
        /// </summary>
        private Relic GetRandomRelic()
        {
            // TODO: DataManager에서 유물 데이터 로드
            // 지금은 임시로 생성

            // 등급별 가중치 (Common: 50%, Uncommon: 35%, Rare: 15%)
            int roll = Random.Range(0, 100);
            RelicRarity targetRarity;

            if (roll < 50)
                targetRarity = RelicRarity.Common;
            else if (roll < 85)
                targetRarity = RelicRarity.Uncommon;
            else
                targetRarity = RelicRarity.Rare;

            // TODO: 실제로는 RelicDatabase에서 로드
            Relic relic = new Relic(
                $"relic_{Random.Range(1000, 9999)}",
                "임시 유물",
                "상점 테스트용 임시 유물입니다.",
                targetRarity,
                RelicEffectType.Passive
            );

            return relic;
        }

        /// <summary>
        /// 상점을 나갑니다
        /// </summary>
        public void ReturnToMap()
        {
            Debug.Log("맵으로 복귀");
            
            // 맵 매니저를 통해 맵 씬으로 이동
            if (Map.MapManager.Instance != null)
            {
                Map.MapManager.Instance.ReturnToMap();
            }
            else
            {
                // 맵 매니저가 없으면 직접 씬 로드 (비상용)
                UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
            }
        }

        /// <summary>
        /// 상점을 리셋합니다
        /// </summary>
        public void ResetShop()
        {
            currentShopItems.Clear();
            timesCardRemoved = 0;
            Debug.Log("상점 리셋 완료");
        }
    }
}
