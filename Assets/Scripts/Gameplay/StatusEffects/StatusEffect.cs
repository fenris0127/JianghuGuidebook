// 캐릭터에게 적용된 상태 이상 효과의 인스턴스를 나타냅니다.
public class StatusEffect
{
    public StatusEffectData Data { get; }
    public int Value { get; set; }

    public StatusEffect(StatusEffectData data, int value)
    {
        Data = data;
        Value = value;
    }
}