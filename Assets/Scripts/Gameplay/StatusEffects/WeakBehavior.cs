using UnityEngine;
public class WeakBehavior : StatusEffectBehavior
{
    public override int OnDamageDealt(int damage) => Mathf.RoundToInt(damage * 0.75f);
    
    public override void OnTurnEnd() => Effect.Value--;
}