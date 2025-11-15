using UnityEngine;
using System.Collections.Generic; // List 사용을 위해
using System.Linq;

// GameEffect 데이터를 해석하고 실행하는 정적 클래스입니다.
public static class EffectProcessor
{
    public static void Process(GameEffect effect, IDamageable caster, IDamageable primaryTarget)
    {
        List<IDamageable> finalTargets = new List<IDamageable>();

        switch (effect.target)
        {
            case EffectTarget.Self:
                if (caster != null) finalTargets.Add(caster);
                break;
            case EffectTarget.SingleEnemy: // '주 대상' 하나만 지정
                if (primaryTarget != null) finalTargets.Add(primaryTarget);
                break;
            case EffectTarget.AllEnemies: // 씬에 있는 모든 적을 대상으로 지정 BattleManager를 통해 현재 살아있는 모든 적 목록을 가져옵니다.
                finalTargets.AddRange(BattleManager.Instance.GetEnemies());
                break;
            case EffectTarget.RandomEnemy: // 살아있는 적 중 하나를 무작위로 지정
                var livingEnemies = BattleManager.Instance.GetEnemies();
                if (livingEnemies.Count > 0)
                    finalTargets.Add(livingEnemies[Random.Range(0, livingEnemies.Count)]);
                break;
            case EffectTarget.Player: // 플레이어를 대상으로 직접 지정
                finalTargets.Add(BattleManager.Instance.GetPlayer());
                break;
        }

        foreach (IDamageable target in finalTargets)
            ApplyEffect(effect, caster, target);
    }

    private static void ApplyEffect(GameEffect effect, IDamageable caster, IDamageable target)
    {
        Player player = BattleManager.Instance.GetPlayer(); // caster가 null일 경우를 대비

        switch (effect.type)
        {
            // --- 전투 효과 ---
            case GameEffectType.Damage:
                int baseDamage = effect.value;
                int finalDamage = baseDamage;

                // 시전자가 있다면, 시전자의 버프/디버프를 계산
                if (caster != null)
                {
                    // 1. '힘' 수치만큼 기본 피해량 증가
                    finalDamage += caster.GetStatusEffectValue(StatusEffectType.Strength);

                    // 2. '약화' 상태라면 피해량 25% 감소
                    if (caster.GetStatusEffectValue(StatusEffectType.Weak) > 0)
                        finalDamage = Mathf.RoundToInt(finalDamage * 0.75f);
                }
                
                target.TakeDamage(finalDamage);
                break;
            case GameEffectType.ApplyStatus:
                if (effect.statusEffectData != null)
                    target.ApplyStatusEffect(new StatusEffect(effect.statusEffectData, effect.value));
                break;
            case GameEffectType.Block:
                if (target is Player p_block) p_block.GainDefense(effect.value);
                break;
            case GameEffectType.DrawCard:
                if (target is Player p_draw) p_draw.DrawCards(effect.value);
                break;
            case GameEffectType.Heal:
                if (target is Player p_heal) p_heal.Heal(effect.value);
                break;
            case GameEffectType.GainGold:
                if (target is Player p_gold) p_gold.GainGold(effect.value);
                break;
            case GameEffectType.UpgradeRandomCard:
                if (target is Player p_upgrade) p_upgrade.UpgradeRandomCard();
                break;
            
            // ... 다른 모든 GameEffectType에 대한 처리를 여기에 추가 ...
        }
    }
}