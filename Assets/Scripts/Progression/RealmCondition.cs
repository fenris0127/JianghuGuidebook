namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 경지 돌파 조건 타입
    /// </summary>
    public enum RealmConditionType
    {
        None,
        UseEnergyCard,          // 내공 카드 사용 횟수
        SpendEnergyInBattle,    // 한 전투에서 소모한 내공 양
        TotalEnergySpent,       // 누적 소모 내공 양
        UseEnergySecret         // 내공 비기 사용 횟수
    }

    /// <summary>
    /// 경지 돌파 조건 클래스
    /// </summary>
    [System.Serializable]
    public class RealmCondition
    {
        public RealmConditionType type;
        public int requiredValue;
        public int currentValue;

        public RealmCondition(RealmConditionType type, int required)
        {
            this.type = type;
            this.requiredValue = required;
            this.currentValue = 0;
        }

        public bool IsMet()
        {
            return currentValue >= requiredValue;
        }

        public void AddProgress(int amount)
        {
            currentValue += amount;
        }

        public void ResetProgress()
        {
            currentValue = 0;
        }
    }
}
