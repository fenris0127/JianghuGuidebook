using UnityEngine;
using JianghuGuidebook.Data;
using JianghuGuidebook.Relics;

namespace JianghuGuidebook.Shop
{
    /// <summary>
    /// 상점 아이템 타입
    /// </summary>
    public enum ShopItemType
    {
        Card,           // 카드
        Relic,          // 유물
        CardRemoval,    // 카드 제거 서비스
        CardUpgrade,    // 카드 업그레이드 서비스
        HealthPotion    // 체력 회복 물약
    }

    /// <summary>
    /// 상점에서 판매되는 아이템 클래스
    /// </summary>
    [System.Serializable]
    public class ShopItem
    {
        public string id;
        public ShopItemType itemType;
        public int price;
        public bool isPurchased = false;

        // 아이템별 데이터
        public CardData cardData;       // 카드 아이템일 경우
        public Relic relic;             // 유물 아이템일 경우
        public int healAmount;          // 물약일 경우 회복량

        public ShopItem(ShopItemType type, int price)
        {
            this.id = System.Guid.NewGuid().ToString();
            this.itemType = type;
            this.price = price;
        }

        /// <summary>
        /// 카드 상품 생성
        /// </summary>
        public static ShopItem CreateCardItem(CardData card, int price)
        {
            ShopItem item = new ShopItem(ShopItemType.Card, price);
            item.cardData = card;
            return item;
        }

        /// <summary>
        /// 유물 상품 생성
        /// </summary>
        public static ShopItem CreateRelicItem(Relic relic, int price)
        {
            ShopItem item = new ShopItem(ShopItemType.Relic, price);
            item.relic = relic;
            return item;
        }

        /// <summary>
        /// 카드 제거 서비스 생성
        /// </summary>
        public static ShopItem CreateCardRemovalService(int price)
        {
            return new ShopItem(ShopItemType.CardRemoval, price);
        }

        /// <summary>
        /// 카드 업그레이드 서비스 생성
        /// </summary>
        public static ShopItem CreateCardUpgradeService(int price)
        {
            return new ShopItem(ShopItemType.CardUpgrade, price);
        }

        /// <summary>
        /// 체력 회복 물약 생성
        /// </summary>
        public static ShopItem CreateHealthPotion(int healAmount, int price)
        {
            ShopItem item = new ShopItem(ShopItemType.HealthPotion, price);
            item.healAmount = healAmount;
            return item;
        }

        /// <summary>
        /// 아이템 이름 가져오기
        /// </summary>
        public string GetName()
        {
            switch (itemType)
            {
                case ShopItemType.Card:
                    return cardData?.name ?? "카드";
                case ShopItemType.Relic:
                    return relic?.name ?? "유물";
                case ShopItemType.CardRemoval:
                    return "카드 제거";
                case ShopItemType.CardUpgrade:
                    return "카드 업그레이드";
                case ShopItemType.HealthPotion:
                    return $"회복 물약 ({healAmount} HP)";
                default:
                    return "상품";
            }
        }

        /// <summary>
        /// 아이템 설명 가져오기
        /// </summary>
        public string GetDescription()
        {
            switch (itemType)
            {
                case ShopItemType.Card:
                    return cardData?.description ?? "";
                case ShopItemType.Relic:
                    return relic?.description ?? "";
                case ShopItemType.CardRemoval:
                    return "덱에서 카드 1장을 영구적으로 제거합니다.";
                case ShopItemType.CardUpgrade:
                    return "카드 1장을 업그레이드합니다.";
                case ShopItemType.HealthPotion:
                    return $"최대 체력의 {healAmount}%를 즉시 회복합니다.";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 구매 처리
        /// </summary>
        public void Purchase()
        {
            isPurchased = true;
            Debug.Log($"상품 구매: {GetName()} ({price} 골드)");
        }

        public override string ToString()
        {
            return $"{GetName()} - {price}골드 {(isPurchased ? "(구매됨)" : "")}";
        }
    }
}
