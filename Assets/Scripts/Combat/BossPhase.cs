namespace JianghuGuidebook.Combat
{
    /// <summary>
    /// 보스 페이즈 열거형
    /// </summary>
    public enum BossPhaseType
    {
        Phase1,     // 페이즈 1 (100%-50%)
        Phase2,     // 페이즈 2 (50%-0%)
        Phase3      // 페이즈 3 (특수 보스용, 옵션)
    }

    /// <summary>
    /// 보스 페이즈 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class BossPhase
    {
        public BossPhaseType phaseType;     // 페이즈 타입
        public float healthThreshold;       // 페이즈 전환 체력 임계값 (%)
        public string phaseName;            // 페이즈 이름
        public string phaseDescription;     // 페이즈 설명

        public BossPhase(BossPhaseType type, float threshold, string name, string description = "")
        {
            this.phaseType = type;
            this.healthThreshold = threshold;
            this.phaseName = name;
            this.phaseDescription = description;
        }

        /// <summary>
        /// 현재 체력이 이 페이즈 임계값 이하인지 확인
        /// </summary>
        public bool IsPhaseTriggered(int currentHealth, int maxHealth)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            return healthPercent <= healthThreshold;
        }

        public override string ToString()
        {
            return $"{phaseName} (체력 {healthThreshold * 100}% 이하)";
        }
    }
}
