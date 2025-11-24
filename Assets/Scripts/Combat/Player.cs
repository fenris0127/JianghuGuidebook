using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.Combat
{
    /// <summary>
    /// 플레이어 캐릭터 클래스
    /// 체력, 내공, 방어도 및 상태 효과를 관리합니다
    /// </summary>
    public class Player : MonoBehaviour
    {
        [Header("체력")]
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;

        [Header("내공 (Energy)")]
        [SerializeField] private int currentEnergy;
        [SerializeField] private int maxEnergy;

        [Header("방어도")]
        [SerializeField] private int block;

        [Header("상태 효과")]
        [SerializeField] private List<StatusEffect> statusEffects = new List<StatusEffect>();

        // Properties
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public int CurrentEnergy => currentEnergy;
        public int MaxEnergy => maxEnergy;
        public int Block => block;
        public List<StatusEffect> StatusEffects => statusEffects;

        // Events
        public System.Action<int, int> OnHealthChanged;  // (current, max)
        public System.Action<int, int> OnEnergyChanged;  // (current, max)
        public System.Action<int> OnBlockChanged;
        public System.Action OnDeath;

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// 플레이어를 초기화합니다
        /// </summary>
        public void Initialize()
        {
            maxHealth = Constants.PLAYER_MAX_HEALTH;
            currentHealth = maxHealth;
            maxEnergy = Constants.MAX_ENERGY;
            currentEnergy = maxEnergy;
            block = 0;
            statusEffects.Clear();

            Debug.Log($"플레이어 초기화: HP {currentHealth}/{maxHealth}, 내공 {currentEnergy}/{maxEnergy}");

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            OnBlockChanged?.Invoke(block);
        }

        /// <summary>
        /// 피해를 받습니다
        /// 먼저 방어도로 피해를 감소시킨 후 체력을 감소시킵니다
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;

            int initialDamage = amount;

            // 방어도로 피해 감소
            if (block > 0)
            {
                if (block >= amount)
                {
                    // 방어도가 피해를 완전히 흡수
                    block -= amount;
                    Debug.Log($"플레이어: {amount} 피해를 방어도로 막음 (남은 방어도: {block})");
                    OnBlockChanged?.Invoke(block);
                    return;
                }
                else
                {
                    // 방어도가 일부 피해 흡수
                    amount -= block;
                    Debug.Log($"플레이어: 방어도 {block}로 {block} 피해 막음, {amount} 피해 받음");
                    block = 0;
                    OnBlockChanged?.Invoke(block);
                }
            }

            // 체력 감소
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;

            Debug.Log($"플레이어: {amount} 피해 받음 (현재 체력: {currentHealth}/{maxHealth})");
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            // 사망 체크
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 방어도를 획득합니다
        /// </summary>
        public void GainBlock(int amount)
        {
            if (amount <= 0) return;

            block += amount;
            Debug.Log($"플레이어: 방어도 {amount} 획득 (현재 방어도: {block})");
            OnBlockChanged?.Invoke(block);
        }

        /// <summary>
        /// 방어도를 초기화합니다 (턴 종료 시 호출)
        /// </summary>
        public void ResetBlock()
        {
            if (block > 0)
            {
                Debug.Log($"플레이어: 방어도 {block} -> 0으로 초기화");
                block = 0;
                OnBlockChanged?.Invoke(block);
            }
        }

        /// <summary>
        /// 체력을 회복합니다
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0) return;

            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            Debug.Log($"플레이어: {amount} 체력 회복 (현재 체력: {currentHealth}/{maxHealth})");
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        /// <summary>
        /// 내공을 소모합니다
        /// </summary>
        public bool SpendEnergy(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("음수의 내공을 소모할 수 없습니다");
                return false;
            }

            if (currentEnergy < amount)
            {
                Debug.LogWarning($"내공 부족: 필요 {amount}, 현재 {currentEnergy}");
                return false;
            }

            currentEnergy -= amount;
            Debug.Log($"플레이어: 내공 {amount} 소모 (현재 내공: {currentEnergy}/{maxEnergy})");
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            return true;
        }

        /// <summary>
        /// 내공을 획득합니다
        /// </summary>
        public void GainEnergy(int amount)
        {
            if (amount <= 0) return;

            currentEnergy += amount;
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }

            Debug.Log($"플레이어: 내공 {amount} 획득 (현재 내공: {currentEnergy}/{maxEnergy})");
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }

        /// <summary>
        /// 내공을 최대치로 리셋합니다 (턴 시작 시 호출)
        /// </summary>
        public void ResetEnergy()
        {
            currentEnergy = maxEnergy;
            Debug.Log($"플레이어: 내공 리셋 ({currentEnergy}/{maxEnergy})");
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }

        /// <summary>
        /// 상태 효과를 추가합니다
        /// </summary>
        public void AddStatusEffect(StatusEffect effect)
        {
            statusEffects.Add(effect);
            effect.Apply(this);
            Debug.Log($"플레이어: 상태 효과 추가 - {effect.GetType().Name}");
        }

        /// <summary>
        /// 상태 효과를 제거합니다
        /// </summary>
        public void RemoveStatusEffect(StatusEffect effect)
        {
            if (statusEffects.Contains(effect))
            {
                effect.Remove(this);
                statusEffects.Remove(effect);
                Debug.Log($"플레이어: 상태 효과 제거 - {effect.GetType().Name}");
            }
        }

        /// <summary>
        /// 사망 처리
        /// </summary>
        private void Die()
        {
            Debug.Log("=== 플레이어 사망 ===");
            OnDeath?.Invoke();

            // CombatManager에 패배 알림
            if (CombatManager.Instance != null)
            {
                CombatManager.Instance.Defeat();
            }
        }

        /// <summary>
        /// 턴 시작 시 처리
        /// </summary>
        public void OnTurnStart()
        {
            ResetEnergy();
            ResetBlock();

            // 상태 효과 처리 (향후 구현)
            // foreach (var effect in statusEffects)
            // {
            //     effect.OnTurnStart(this);
            // }
        }

        /// <summary>
        /// 턴 종료 시 처리
        /// </summary>
        public void OnTurnEnd()
        {
            // 상태 효과 처리 (향후 구현)
            // foreach (var effect in statusEffects.ToArray())
            // {
            //     effect.OnTurnEnd(this);
            // }
        }
    }
}
