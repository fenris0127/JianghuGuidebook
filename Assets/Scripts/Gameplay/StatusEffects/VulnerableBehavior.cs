using UnityEngine;

/// <summary>
/// '취약' 효과의 구체적인 동작을 구현한 클래스입니다.
/// </summary>
public class VulnerableBehavior : StatusEffectBehavior
{
    public override int OnDamageTaken(int damage)
    {
        float multiplier = GangHoBiGeup.Managers.ConfigManager.Instance?.GetVulnerableMultiplier() ?? 1.5f;
        return Mathf.RoundToInt(damage * multiplier);
    }

    public override void OnTurnEnd() => Effect.Value--;
}