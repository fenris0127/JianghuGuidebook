namespace GangHoBiGeup.Gameplay
{
    public class StrengthBehavior : StatusEffectBehavior
    {
        // OnDamageDealt는 BattleManager에서 처리하므로 여기서는 수치만 관리
        public override bool IsFinished() { return false; } // 힘은 0이 되어도 사라지지 않음
    }
}
