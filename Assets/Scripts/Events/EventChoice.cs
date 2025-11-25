using System.Collections.Generic;

namespace JianghuGuidebook.Events
{
    /// <summary>
    /// 이벤트 선택지 결과 타입
    /// </summary>
    public enum OutcomeType
    {
        GainGold,           // 골드 획득
        LoseGold,           // 골드 손실
        GainCard,           // 카드 획득
        RemoveCard,         // 카드 제거
        GainRelic,          // 유물 획득
        Heal,               // 체력 회복
        TakeDamage,         // 체력 손실
        UpgradeCard,        // 카드 업그레이드
        StartCombat,        // 전투 발생
        GainMaxHealth,      // 최대 체력 증가
        ChainEvent          // 연쇄 이벤트 (다음 이벤트 트리거)
    }

    /// <summary>
    /// 이벤트 선택지 결과
    /// </summary>
    [System.Serializable]
    public class EventOutcome
    {
        public OutcomeType type;
        public int value;           // 숫자 값 (골드, 체력 등)
        public string stringValue;  // 문자열 값 (카드 ID, 유물 ID, 적 ID 등)

        public EventOutcome(OutcomeType type, int value = 0, string stringValue = "")
        {
            this.type = type;
            this.value = value;
            this.stringValue = stringValue;
        }

        public override string ToString()
        {
            switch (type)
            {
                case OutcomeType.GainGold:
                    return $"골드 +{value}";
                case OutcomeType.LoseGold:
                    return $"골드 -{value}";
                case OutcomeType.GainCard:
                    return $"카드 획득: {stringValue}";
                case OutcomeType.RemoveCard:
                    return "카드 1장 제거";
                case OutcomeType.GainRelic:
                    return $"유물 획득: {stringValue}";
                case OutcomeType.Heal:
                    return $"체력 +{value}";
                case OutcomeType.TakeDamage:
                    return $"체력 -{value}";
                case OutcomeType.UpgradeCard:
                    return "카드 업그레이드";
                case OutcomeType.StartCombat:
                    return $"전투 발생: {stringValue}";
                case OutcomeType.GainMaxHealth:
                    return $"최대 체력 +{value}";
                case OutcomeType.ChainEvent:
                    return $"후속 이벤트 발생: {stringValue}";
                default:
                    return "";
            }
        }
    }

    /// <summary>
    /// 이벤트 선택지 요구사항 타입
    /// </summary>
    public enum RequirementType
    {
        MinHealth,          // 최소 체력 (%)
        MinGold,            // 최소 골드
        HasRelic,           // 특정 유물 보유
        MaxHealth,          // 최대 체력 이상
        Random,             // 확률 기반 (성공률)
        HasWeaponMastery,   // 무기술 경지 조건 (stringValue: WeaponType, value: MasteryTier)
        HasInnerRealm,      // 내공 경지 조건 (value: InnerEnergyRealm)
        MinCardsInDeck,     // 최소 덱 카드 수
        HasCardInDeck,      // 특정 카드 보유 (stringValue: CardId)
        MinDeckSize,        // 덱 크기 최소
        MaxDeckSize         // 덱 크기 최대
    }

    /// <summary>
    /// 이벤트 선택지 요구사항
    /// </summary>
    [System.Serializable]
    public class EventRequirement
    {
        public RequirementType type;
        public int value;           // 숫자 값
        public string stringValue;  // 문자열 값 (유물 ID 등)

        public EventRequirement(RequirementType type, int value = 0, string stringValue = "")
        {
            this.type = type;
            this.value = value;
            this.stringValue = stringValue;
        }

        public override string ToString()
        {
            switch (type)
            {
                case RequirementType.MinHealth:
                    return $"체력 {value}% 이상 필요";
                case RequirementType.MinGold:
                    return $"골드 {value} 필요";
                case RequirementType.HasRelic:
                    return $"유물 '{stringValue}' 필요";
                case RequirementType.MaxHealth:
                    return "최대 체력 필요";
                case RequirementType.Random:
                    return $"성공률 {value}%";
                case RequirementType.HasWeaponMastery:
                    return $"{stringValue} 경지 {value} 이상 필요";
                case RequirementType.HasInnerRealm:
                    return $"내공 경지 {value} 이상 필요";
                case RequirementType.MinCardsInDeck:
                    return $"덱에 카드 {value}장 이상 필요";
                case RequirementType.HasCardInDeck:
                    return $"카드 '{stringValue}' 보유 필요";
                case RequirementType.MinDeckSize:
                    return $"덱 크기 {value} 이상 필요";
                case RequirementType.MaxDeckSize:
                    return $"덱 크기 {value} 이하 필요";
                default:
                    return "";
            }
        }
    }

    /// <summary>
    /// 이벤트 선택지 클래스
    /// </summary>
    [System.Serializable]
    public class EventChoice
    {
        public string text;                             // 선택지 텍스트
        public List<EventRequirement> requirements;     // 요구사항 (옵션)
        public List<EventOutcome> outcomes;             // 결과 리스트
        public string resultText;                       // 결과 텍스트 (선택 후 표시)

        public EventChoice()
        {
            requirements = new List<EventRequirement>();
            outcomes = new List<EventOutcome>();
        }

        public EventChoice(string text, string resultText)
        {
            this.text = text;
            this.resultText = resultText;
            requirements = new List<EventRequirement>();
            outcomes = new List<EventOutcome>();
        }

        /// <summary>
        /// 요구사항 추가
        /// </summary>
        public void AddRequirement(RequirementType type, int value = 0, string stringValue = "")
        {
            requirements.Add(new EventRequirement(type, value, stringValue));
        }

        /// <summary>
        /// 결과 추가
        /// </summary>
        public void AddOutcome(OutcomeType type, int value = 0, string stringValue = "")
        {
            outcomes.Add(new EventOutcome(type, value, stringValue));
        }

        public override string ToString()
        {
            string req = requirements.Count > 0 ? $" ({string.Join(", ", requirements)})" : "";
            return $"{text}{req}";
        }
    }
}
