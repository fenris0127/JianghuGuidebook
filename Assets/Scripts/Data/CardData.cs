using System;
using UnityEngine;

namespace JianghuGuidebook.Data
{
    /// <summary>
    /// 카드 타입 열거형
    /// </summary>
    public enum CardType
    {
        Attack,     // 공격 (초식)
        Defense,    // 방어 (신법)
        Skill,      // 스킬 (기공)
        Secret      // 비기
    }

    /// <summary>
    /// 카드 희귀도 열거형
    /// </summary>
    public enum CardRarity
    {
        Common,     // 일반
        Uncommon,   // 고급
        Rare,       // 희귀
        Epic,       // 영웅
        Legendary   // 전설
    }

    /// <summary>
    /// 카드 데이터 클래스
    /// JSON에서 로드될 카드 정보를 저장합니다
    /// </summary>
    [Serializable]
    public class CardData
    {
        public string id;               // 고유 ID (예: "card_strike_01")
        public string name;             // 카드 이름 (예: "일검")
        public int cost;                // 내공 비용
        public CardType type;           // 카드 타입
        public CardRarity rarity;       // 희귀도
        public int baseDamage;          // 기본 피해량 (공격 카드용)
        public int baseBlock;           // 기본 방어도 (방어 카드용)
        public string description;      // 카드 설명

        // 추가 효과 (향후 확장용)
        public int drawCards;           // 드로우할 카드 수
        public bool exhaust;            // 소진 여부
        public int timesToPlay;         // 여러 번 적용 (예: 3번 공격)

        /// <summary>
        /// 카드 데이터가 유효한지 검증합니다
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("CardData: ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"CardData: 카드 {id}의 이름이 비어있습니다");
                return false;
            }

            if (cost < 0)
            {
                Debug.LogError($"CardData: 카드 {id}의 비용이 음수입니다");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 카드 정보를 문자열로 반환합니다
        /// </summary>
        public override string ToString()
        {
            return $"[{id}] {name} (비용: {cost}) - {description}";
        }
    }

    /// <summary>
    /// 카드 데이터베이스 래퍼 클래스
    /// JSON 역직렬화를 위한 컨테이너
    /// </summary>
    [Serializable]
    public class CardDatabase
    {
        public CardData[] cards;
    }
}
