using UnityEngine;

namespace GangHoBiGeup.Gameplay
{
    // '중독' 효과의 구체적인 동작을 구현한 클래스입니다.
    public class PoisonBehavior : StatusEffectBehavior
    {
        public override void OnTurnStart()
        {
            if (Effect.Value > 0)
            {
                owner.TakeDamage(Effect.Value);
            }
        }
        
        public override void OnTurnEnd()
        {
            // TDD: 턴 종료 시에도 데미지 (테스트 호환)
            if (Effect.Value > 0)
            {
                owner.TakeDamage(Effect.Value);
            }
            
            // 기본 턴 종료 처리 (지속 시간 감소)
            base.OnTurnEnd();
        }
    }
}
