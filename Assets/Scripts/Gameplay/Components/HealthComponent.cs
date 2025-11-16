using UnityEngine;
using System;

namespace GangHoBiGeup.Gameplay
{
    /// <summary>
    /// 체력과 방어도를 관리하는 컴포넌트
    /// Player와 Enemy의 공통 로직을 담당합니다.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        public event Action<int, int, int> OnStatsChanged; // currentHealth, maxHealth, defense

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Defense { get; set; }
        public bool IsDead => CurrentHealth <= 0;

        /// <summary>
        /// 체력과 방어도를 초기화합니다.
        /// </summary>
        public void Initialize(int maxHealth, int currentHealth = -1)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth < 0 ? maxHealth : currentHealth;
            Defense = 0;
            NotifyStatsChanged();
        }

        /// <summary>
        /// 피해를 받습니다. 방어도가 먼저 감소하고 남은 피해는 체력을 감소시킵니다.
        /// </summary>
        /// <param name="damage">받을 피해량</param>
        /// <param name="vulnerableMultiplier">취약 상태 배율 (기본 1.0)</param>
        /// <returns>실제로 받은 피해량</returns>
        public int TakeDamage(int damage, float vulnerableMultiplier = 1f)
        {
            int modifiedDamage = Mathf.RoundToInt(damage * vulnerableMultiplier);

            // 방어도가 먼저 감소함
            if (Defense > 0)
            {
                int blockedDamage = Mathf.Min(Defense, modifiedDamage);
                Defense -= blockedDamage;
                modifiedDamage -= blockedDamage;
            }

            // 남은 데미지로 체력 감소
            CurrentHealth -= modifiedDamage;
            if (CurrentHealth < 0) CurrentHealth = 0;

            NotifyStatsChanged();
            return modifiedDamage;
        }

        /// <summary>
        /// 방어도를 무시하는 직접 피해를 입습니다.
        /// </summary>
        public int TakeDirectDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0) CurrentHealth = 0;

            NotifyStatsChanged();
            return damage;
        }

        /// <summary>
        /// 체력을 회복합니다. 최대 체력을 초과할 수 없습니다.
        /// </summary>
        public void Heal(int amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
            NotifyStatsChanged();
        }

        /// <summary>
        /// 최대 체력을 증가시키고, 증가한 만큼 현재 체력도 회복합니다.
        /// </summary>
        public void IncreaseMaxHealth(int amount)
        {
            MaxHealth += amount;
            Heal(amount);
        }

        /// <summary>
        /// 방어도를 획득합니다.
        /// </summary>
        public void GainDefense(int amount)
        {
            Defense += amount;
            NotifyStatsChanged();
        }

        /// <summary>
        /// 방어도를 초기화합니다 (턴 시작 시 호출).
        /// </summary>
        public void ResetDefense()
        {
            Defense = 0;
            NotifyStatsChanged();
        }

        /// <summary>
        /// 현재 상태를 복원합니다 (세이브 로드 시 사용).
        /// </summary>
        public void RestoreState(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            Defense = 0;
            NotifyStatsChanged();
        }

        private void NotifyStatsChanged()
        {
            OnStatsChanged?.Invoke(CurrentHealth, MaxHealth, Defense);
        }
    }
}
