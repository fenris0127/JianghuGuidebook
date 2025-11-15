using UnityEngine;

// '중독' 효과의 구체적인 동작을 구현한 클래스입니다.
public class PoisonBehavior : StatusEffectBehavior
{
    public override void OnTurnStart()
    {
        if (Effect.Value > 0)
        {
            owner.TakeDamage(Effect.Value);
            Effect.Value--;
        }
    }
}