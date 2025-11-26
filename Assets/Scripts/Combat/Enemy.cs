using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Data;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.Combat
{
    /// <summary>
    /// 적 캐릭터 클래스
    /// 체력, 의도 및 상태 효과를 관리합니다
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("적 데이터")]
        [SerializeField] private EnemyData enemyData;

        [Header("체력")]
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;

        [Header("의도")]
        [SerializeField] private EnemyAction currentIntent;

        [Header("방어도")]
        [SerializeField] private int block;

        [Header("상태 효과")]
        [SerializeField] private List<StatusEffect> statusEffects = new List<StatusEffect>();

        // Properties
        public EnemyData EnemyData => enemyData;
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public EnemyAction CurrentIntent => currentIntent;
        public int Block => block;
        public List<StatusEffect> StatusEffects => statusEffects;

        // Events
        public System.Action<int, int> OnHealthChanged;  // (current, max)
        public System.Action<int> OnBlockChanged;
        public System.Action<EnemyAction> OnIntentChanged;
        public System.Action OnDeath;

        /// <summary>
        /// 적을 초기화합니다
        /// </summary>
        public void Initialize(EnemyData data)
        {
            enemyData = data;
            enemyData = data;
            
            // 난이도 적용
            float hpMult = 1.0f;
            if (DifficultyManager.Instance != null)
            {
                hpMult = DifficultyManager.Instance.GetCurrentModifier().enemyHealthMultiplier;
            }

            maxHealth = Mathf.RoundToInt(data.maxHealth * hpMult);
            currentHealth = maxHealth;
            block = 0;
            statusEffects.Clear();

            // 첫 의도 결정
            DetermineIntent();

            Debug.Log($"적 생성: {data.name} (HP: {currentHealth}/{maxHealth})");

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnBlockChanged?.Invoke(block);
            OnIntentChanged?.Invoke(currentIntent);
        }

        /// <summary>
        /// ID로 적을 초기화합니다
        /// </summary>
        public void Initialize(string enemyId)
        {
            EnemyData data = DataManager.Instance.GetEnemyData(enemyId);
            if (data != null)
            {
                Initialize(data);
            }
            else
            {
                Debug.LogError($"적 데이터를 찾을 수 없습니다: {enemyId}");
            }
        }

        /// <summary>
        /// 피해를 받습니다
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;

            // 방어도로 피해 감소
            if (block > 0)
            {
                if (block >= amount)
                {
                    // 방어도가 피해를 완전히 흡수
                    block -= amount;
                    Debug.Log($"{enemyData.name}: {amount} 피해를 방어도로 막음 (남은 방어도: {block})");
                    OnBlockChanged?.Invoke(block);
                    return;
                }
                else
                {
                    // 방어도가 일부 피해 흡수
                    amount -= block;
                    Debug.Log($"{enemyData.name}: 방어도 {block}로 {block} 피해 막음, {amount} 피해 받음");
                    block = 0;
                    OnBlockChanged?.Invoke(block);
                }
            }

            // 체력 감소
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;

            Debug.Log($"{enemyData.name}: {amount} 피해 받음 (현재 체력: {currentHealth}/{maxHealth})");
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
            Debug.Log($"{enemyData.name}: 방어도 {amount} 획득 (현재 방어도: {block})");
            OnBlockChanged?.Invoke(block);
        }

        /// <summary>
        /// 방어도를 초기화합니다
        /// </summary>
        public void ResetBlock()
        {
            if (block > 0)
            {
                Debug.Log($"{enemyData.name}: 방어도 {block} -> 0으로 초기화");
                block = 0;
                OnBlockChanged?.Invoke(block);
            }
        }

        /// <summary>
        /// 다음 의도를 결정합니다
        /// </summary>
        public void DetermineIntent()
        {
            if (enemyData == null)
            {
                Debug.LogError("적 데이터가 없습니다");
                return;
            }

            currentIntent = enemyData.GetRandomAction();
            Debug.Log($"{enemyData.name}: 다음 의도 - {currentIntent}");
            OnIntentChanged?.Invoke(currentIntent);
        }

        /// <summary>
        /// 현재 의도를 실행합니다
        /// </summary>
        public void ExecuteIntent(Player player)
        {
            if (currentIntent == null)
            {
                Debug.LogError($"{enemyData.name}: 실행할 의도가 없습니다");
                return;
            }

            Debug.Log($"{enemyData.name}: 의도 실행 - {currentIntent}");

            switch (currentIntent.type)
            {
                case EnemyActionType.Attack:
                    // 플레이어 공격
                    int damage = currentIntent.value;
                    if (DifficultyManager.Instance != null)
                    {
                        damage = Mathf.RoundToInt(damage * DifficultyManager.Instance.GetCurrentModifier().enemyDamageMultiplier);
                    }
                    player.TakeDamage(damage);
                    break;

                case EnemyActionType.Defend:
                    // 방어도 획득
                    GainBlock(currentIntent.value);
                    break;

                case EnemyActionType.Buff:
                    // TODO: 버프 적용
                    Debug.Log($"{enemyData.name}: 버프 사용 (값: {currentIntent.value})");
                    break;

                case EnemyActionType.Debuff:
                    // TODO: 디버프 적용
                    Debug.Log($"{enemyData.name}: 디버프 사용 (값: {currentIntent.value})");
                    break;

                case EnemyActionType.Special:
                    // TODO: 특수 행동
                    Debug.Log($"{enemyData.name}: 특수 행동 (값: {currentIntent.value})");
                    break;
            }

            // 다음 의도 결정
            DetermineIntent();
        }

        /// <summary>
        /// 상태 효과를 추가합니다
        /// </summary>
        public void AddStatusEffect(StatusEffect effect)
        {
            statusEffects.Add(effect);
            effect.Apply(this);
            Debug.Log($"{enemyData.name}: 상태 효과 추가 - {effect.GetType().Name}");
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
                Debug.Log($"{enemyData.name}: 상태 효과 제거 - {effect.GetType().Name}");
            }
        }

        /// <summary>
        /// 사망 처리
        /// </summary>
        private void Die()
        {
            Debug.Log($"=== {enemyData.name} 사망 ===");
            OnDeath?.Invoke();

            // TODO: CombatManager에 적 사망 알림
        }

        /// <summary>
        /// 턴 시작 시 처리
        /// </summary>
        public void OnTurnStart()
        {
            // 적은 턴 시작 시 방어도를 유지 (게임에 따라 다를 수 있음)
            // ResetBlock();

            // 상태 효과 처리 (향후 구현)
        }

        /// <summary>
        /// 턴 종료 시 처리
        /// </summary>
        public void OnTurnEnd()
        {
            // 상태 효과 처리 (향후 구현)
        }

        /// <summary>
        /// 적이 살아있는지 확인합니다
        /// </summary>
        public bool IsAlive()
        {
            return currentHealth > 0;
        }
    }
}
