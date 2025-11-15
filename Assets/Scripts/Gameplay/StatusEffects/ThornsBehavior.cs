public class ThornsBehavior : StatusEffectBehavior
{
    // 피격 시 피해 반사 로직은 Player/Enemy의 TakeDamage에서 처리
    public override bool IsFinished() { return false; } // 가시도 사라지지 않음
}