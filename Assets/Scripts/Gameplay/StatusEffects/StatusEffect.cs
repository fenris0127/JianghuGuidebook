namespace GangHoBiGeup.Gameplay
{
    // 캐릭터에게 적용된 상태 이상 효과의 인스턴스를 나타냅니다.
    public class StatusEffect
    {
        public StatusEffectData Data { get; }
        public int Value { get; set; }
        public int Duration { get; set; } // TDD: 지속 시간

        public StatusEffect(StatusEffectData data, int value)
        {
            Data = data;
            Value = value;
            Duration = -1; // 기본적으로 무한 지속
        }
        
        // TDD 테스트용 생성자
        public StatusEffect(StatusEffectType type, int value, int duration)
        {
            // 임시 StatusEffectData 생성
            Data = UnityEngine.ScriptableObject.CreateInstance<StatusEffectData>();
            Data.type = type;
            Value = value;
            Duration = duration;
        }
    }
}
