namespace JianghuGuidebook.Rest
{
    /// <summary>
    /// 휴식 선택지 타입
    /// </summary>
    public enum RestChoiceType
    {
        Sleep,          // 수면 (체력 회복)
        Training,       // 수련 (카드 업그레이드)
        Meditation      // 타파심마 (카드 2장 업그레이드, 체력 손실)
    }

    /// <summary>
    /// 휴식 선택지 클래스
    /// </summary>
    public class RestChoice
    {
        public RestChoiceType choiceType;
        public string name;
        public string description;
        public bool isAvailable;
        public string unavailableReason;

        public RestChoice(RestChoiceType type, string name, string description)
        {
            this.choiceType = type;
            this.name = name;
            this.description = description;
            this.isAvailable = true;
            this.unavailableReason = "";
        }

        /// <summary>
        /// 선택지를 사용 불가능하게 설정합니다
        /// </summary>
        public void SetUnavailable(string reason)
        {
            isAvailable = false;
            unavailableReason = reason;
        }

        public override string ToString()
        {
            if (isAvailable)
            {
                return $"{name}: {description}";
            }
            else
            {
                return $"{name} (불가능): {unavailableReason}";
            }
        }
    }
}
