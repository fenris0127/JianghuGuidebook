// 피해를 받거나 상태 이상에 걸릴 수 있는 모든 게임 객체가 구현해야 하는 인터페이스입니다.
public interface IDamageable
{
    // 대상에게 피해를 입힙니다.
    void TakeDamage(int damage);

    // 대상에게 상태 이상 효과를 적용합니다.
    void ApplyStatusEffect(StatusEffect effect);
    int GetStatusEffectValue(StatusEffectType type);
}