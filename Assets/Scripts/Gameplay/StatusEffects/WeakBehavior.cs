using UnityEngine;

public class WeakBehavior : StatusEffectBehavior
{
    public override int OnDamageDealt(int damage)
    {
        float multiplier = GangHoBiGeup.Managers.ConfigManager.Instance?.GetWeakMultiplier() ?? 0.75f;
        return Mathf.RoundToInt(damage * multiplier);
    }

    public override void OnTurnEnd() => Effect.Value--;
}