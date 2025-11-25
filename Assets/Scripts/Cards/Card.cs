using UnityEngine;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Cards
{
    /// <summary>
    /// 런타임에 사용되는 카드 클래스
    /// CardData로부터 생성되며 실제 게임플레이에 사용됩니다
    /// </summary>
    public class Card
    {
        // 원본 데이터 참조
        private CardData cardData;

        // 런타임 속성 (버프/디버프로 변경 가능)
        public string Id => cardData.id;
        public string Name => cardData.name;
        public int Cost { get; private set; }
        public CardType Type => cardData.type;
        public CardRarity Rarity => cardData.rarity;
        public int Damage { get; private set; }
        public int Block { get; private set; }
        public string Description => cardData.description;
        public int DrawCards => cardData.drawCards;
        public bool Exhaust => cardData.exhaust;
        public int TimesToPlay => cardData.timesToPlay;

        // 카드 상태
        public bool IsPlayable { get; set; }
        public bool IsExhausted { get; set; }

        // 원본 데이터 접근
        public CardData Data => cardData;

        /// <summary>
        /// CardData로부터 Card 인스턴스를 생성합니다
        /// </summary>
        public Card(CardData data)
        {
            if (data == null)
            {
                Debug.LogError("CardData가 null입니다");
                return;
            }

            cardData = data;

            // 초기 값 설정
            Cost = data.cost;
            Damage = data.baseDamage;
            Block = data.baseBlock;
            IsPlayable = true;
            IsExhausted = false;

            // 무기술 경지 보너스 적용
            if (Progression.WeaponMasteryManager.Instance != null)
            {
                int bonus = Progression.WeaponMasteryManager.Instance.GetDamageBonus(data.weaponType);
                if (bonus > 0)
                {
                    Damage += bonus;
                    // Debug.Log($"[{Name}] 무기술 보너스 적용: +{bonus}");
                }
            }
        }

        /// <summary>
        /// 카드 데이터를 복사하여 새 Card 인스턴스를 생성합니다
        /// </summary>
        public Card Clone()
        {
            return new Card(cardData);
        }

        /// <summary>
        /// 카드의 피해량을 수정합니다 (버프/디버프)
        /// </summary>
        public void ModifyDamage(int amount)
        {
            Damage += amount;
            if (Damage < 0) Damage = 0;
        }

        /// <summary>
        /// 카드의 방어도를 수정합니다 (버프/디버프)
        /// </summary>
        public void ModifyBlock(int amount)
        {
            Block += amount;
            if (Block < 0) Block = 0;
        }

        /// <summary>
        /// 카드의 비용을 수정합니다 (버프/디버프)
        /// </summary>
        public void ModifyCost(int amount)
        {
            Cost += amount;
            if (Cost < 0) Cost = 0;
        }

        /// <summary>
        /// 카드를 초기 값으로 리셋합니다
        /// </summary>
        public void Reset()
        {
            Cost = cardData.cost;
            Damage = cardData.baseDamage;
            Block = cardData.baseBlock;
            IsPlayable = true;
            IsExhausted = false;
        }

        /// <summary>
        /// 카드가 현재 사용 가능한지 확인합니다
        /// </summary>
        public bool CanPlay(int availableEnergy)
        {
            return IsPlayable && !IsExhausted && Cost <= availableEnergy;
        }

        /// <summary>
        /// 카드 정보를 문자열로 반환합니다
        /// </summary>
        public override string ToString()
        {
            string info = $"[{Name}] 비용: {Cost}";

            if (Type == CardType.Attack)
            {
                info += $", 피해: {Damage}";
                if (TimesToPlay > 1)
                {
                    info += $" x{TimesToPlay}";
                }
            }
            else if (Type == CardType.Defense)
            {
                info += $", 방어도: {Block}";
            }

            if (DrawCards > 0)
            {
                info += $", 드로우: {DrawCards}";
            }

            if (Exhaust)
            {
                info += " [소진]";
            }

            return info;
        }
    }
}
