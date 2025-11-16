using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Gameplay
{
    /// <summary>
    /// 상태이상을 관리하는 컴포넌트
    /// Player와 Enemy의 공통 로직을 담당합니다.
    /// </summary>
    public class StatusEffectContainer : MonoBehaviour
    {
        public event Action<List<StatusEffect>> OnStatusEffectsChanged;

        private List<StatusEffectBehavior> statusEffects = new List<StatusEffectBehavior>();
        private IDamageable owner;

        /// <summary>
        /// 컨테이너를 초기화합니다.
        /// </summary>
        public void Initialize(IDamageable owner)
        {
            this.owner = owner;
            statusEffects.Clear();
            OnStatusEffectsChanged?.Invoke(new List<StatusEffect>());
        }

        /// <summary>
        /// 상태이상을 적용합니다.
        /// </summary>
        public void ApplyStatusEffect(StatusEffect effectToApply)
        {
            var existingEffect = statusEffects.Find(e =>
                e.Effect.Data != null && e.Effect.Data.type == effectToApply.Data.type);

            if (existingEffect != null)
            {
                existingEffect.Effect.Value += effectToApply.Value;
            }
            else
            {
                StatusEffectBehavior newBehavior = StatusEffectFactory.Create(effectToApply);
                newBehavior.Setup(effectToApply, owner);
                statusEffects.Add(newBehavior);
                newBehavior.OnApplied();
            }

            OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
        }

        /// <summary>
        /// 특정 타입의 상태이상 값을 반환합니다.
        /// </summary>
        public int GetStatusEffectValue(StatusEffectType type)
        {
            var behavior = statusEffects.Find(b => b.Effect.Data.type == type);
            return behavior?.Effect.Value ?? 0;
        }

        /// <summary>
        /// 턴 시작 시 상태이상을 처리합니다.
        /// </summary>
        public void ProcessStatusEffectsOnTurnStart()
        {
            bool changed = false;
            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                statusEffects[i].OnTurnStart();
                if (statusEffects[i].IsFinished())
                {
                    statusEffects.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
                OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
        }

        /// <summary>
        /// 턴 종료 시 상태이상을 처리합니다.
        /// </summary>
        public void ProcessStatusEffectsOnTurnEnd()
        {
            bool changed = false;
            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                statusEffects[i].OnTurnEnd();
                if (statusEffects[i].IsFinished())
                {
                    statusEffects.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
                OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
        }

        /// <summary>
        /// 받는 피해량을 상태이상에 따라 계산합니다.
        /// </summary>
        public int CalculateDamageTaken(int baseDamage)
        {
            int finalDamage = baseDamage;
            foreach (var behavior in statusEffects)
                finalDamage = behavior.OnDamageTaken(finalDamage);
            return finalDamage;
        }

        /// <summary>
        /// 주는 피해량을 상태이상에 따라 계산합니다.
        /// </summary>
        public int CalculateDamageDealt(int baseDamage)
        {
            int finalDamage = baseDamage;
            foreach (var behavior in statusEffects)
                finalDamage = behavior.OnDamageDealt(finalDamage);
            return finalDamage;
        }

        /// <summary>
        /// 디버프만 제거합니다 (페이즈 전환 시 사용).
        /// </summary>
        public void RemoveAllDebuffs()
        {
            bool changed = statusEffects.RemoveAll(b => !b.Effect.Data.isBuff) > 0;
            if (changed)
                OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
        }

        /// <summary>
        /// 모든 상태이상을 제거합니다.
        /// </summary>
        public void ClearAll()
        {
            statusEffects.Clear();
            OnStatusEffectsChanged?.Invoke(new List<StatusEffect>());
        }

        /// <summary>
        /// 현재 활성화된 모든 상태이상을 반환합니다.
        /// </summary>
        public List<StatusEffect> GetAllStatusEffects()
        {
            return statusEffects.Select(b => b.Effect).ToList();
        }
    }
}
