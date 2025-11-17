using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Gameplay
{
    // 적 캐릭터의 데이터를 관리하고 행동 로직을 수행하는 클래스입니다.
    // 컴포넌트 기반 아키텍처로 리팩토링되었습니다.
    public class Enemy : MonoBehaviour, IDamageable
    {
        #region Components
        private HealthComponent _health;
        private StatusEffectContainer _statusEffects;

        // Lazy initialization for components
        private HealthComponent health => _health ?? (_health = GetComponent<HealthComponent>() ?? gameObject.AddComponent<HealthComponent>());
        private StatusEffectContainer statusEffects => _statusEffects ?? (_statusEffects = GetComponent<StatusEffectContainer>() ?? gameObject.AddComponent<StatusEffectContainer>());
        #endregion

        #region TDD Test Properties
        // TDD 테스트를 위한 프로퍼티들
        public int CurrentHealth
        {
            get => health.CurrentHealth;
            set => health.CurrentHealth = value;
        }

        public int MaxHealth
        {
            get => health.MaxHealth;
            set => health.MaxHealth = value;
        }

        public int Block
        {
            get => health.Defense;
            set => health.Defense = value;
        }

        public bool IsDead => health.IsDead;

        // TDD 테스트용 간단한 초기화 메서드
        public void Initialize(EnemyData data)
        {
            this.enemyData = data;
            health.Initialize(data.maxHealth);
            statusEffects.Initialize(this);
        }

        // TDD: 다음 행동 설정
        public void SetNextAction(EnemyAction action)
        {
            nextAction = action;
        }

        // TDD: 패턴으로 초기화
        public void InitializeWithPattern(EnemyData data, List<EnemyAction> pattern)
        {
            Initialize(data);
            actionPattern = pattern;
            turnIndex = 0;
            if (pattern != null && pattern.Count > 0)
            {
                nextAction = pattern[0];
            }
        }

        // TDD: 턴 진행 (다음 행동으로 이동)
        public void AdvanceTurn()
        {
            turnIndex++;
            if (actionPattern != null && actionPattern.Count > 0)
            {
                nextAction = actionPattern[turnIndex % actionPattern.Count];
            }
        }

        // TDD: 현재 행동 수행
        public void PerformAction(Player target)
        {
            if (nextAction == null) return;

            switch (nextAction.actionType)
            {
                case EnemyActionType.Attack:
                    target.TakeDamage(nextAction.value);
                    break;

                case EnemyActionType.Defend:
                    health.GainDefense(nextAction.value);
                    break;

                case EnemyActionType.Buff:
                    if (nextAction.statusEffectData != null)
                    {
                        ApplyStatusEffect(new StatusEffect(nextAction.statusEffectData, nextAction.value));
                    }
                    break;

                case EnemyActionType.Debuff:
                    if (nextAction.statusEffectData != null)
                    {
                        target.ApplyStatusEffect(new StatusEffect(nextAction.statusEffectData, nextAction.value));
                    }
                    break;
            }
        }
        #endregion

        // 이벤트: 자신의 상태(체력, 다음 행동)가 변경되었음을 UI에 알림
        public event Action<int, int, EnemyAction> OnStateChanged;
        public event Action<List<StatusEffect>> OnStatusEffectsChanged;

        // 상태 변수
        public int maxHealth => health.MaxHealth;
        public int currentHealth => health.CurrentHealth;
        public int defense => health.Defense;

        // 전투 정보
        public EnemyData enemyData { get; private set; }
        private EncounterData encounterData;
        private List<EnemyAction> actionPattern;
        private int turnIndex = 0;
        private int currentPhase = 1;
        private EnemyAction nextAction;

        void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Force initialization
            var _ = health;
            var __ = statusEffects;

            // 이벤트 구독
            health.OnStatsChanged += (curHP, maxHP, def) =>
            {
                OnStateChanged?.Invoke(curHP, maxHP, nextAction);
            };

            statusEffects.OnStatusEffectsChanged += (effects) =>
            {
                OnStatusEffectsChanged?.Invoke(effects);
            };
        }

        // BattleManager가 호출하여 적을 초기화합니다.
        public void Setup(EnemyData data, EncounterData encounter)
        {
            this.enemyData = data;
            this.encounterData = encounter;

            health.Initialize(data.maxHealth);
            statusEffects.Initialize(this);

            actionPattern = new List<EnemyAction>(data.actionPattern);
            turnIndex = 0;
            currentPhase = 1;

            // 첫 행동 계획 및 UI 업데이트 요청
            PlanNextAction();
        }

        // 피해를 받는 로직을 처리합니다. IDamageable 인터페이스 구현.
        public void TakeDamage(int damage)
        {
            // 상태이상에 따른 피해량 계산
            int finalDamage = statusEffects.CalculateDamageTaken(damage);

            // 취약 상태 적용
            float multiplier = 1f;
            if (GetStatusEffectValue(StatusEffectType.Vulnerable) > 0)
                multiplier = GangHoBiGeup.Managers.ConfigManager.Instance?.GetVulnerableMultiplier() ?? 1.5f;

            int actualDamage = health.TakeDamage(finalDamage, multiplier);

            if (actualDamage > 0 && FeedbackManager.Instance != null)
                FeedbackManager.Instance.ShowDamageNumber(actualDamage, transform.position);

            // 다중 페이즈 체크
            if (encounterData != null && encounterData.hasMultiplePhases && currentPhase == 1 &&
                (float)health.CurrentHealth / health.MaxHealth <= encounterData.phaseTransitionHealthThreshold &&
                encounterData.actionPatternPhase2 != null && encounterData.actionPatternPhase2.Count > 0)
            {
                SwitchToPhase2();
            }

            if (health.IsDead)
            {
                Die();
            }
        }

        // 방어도를 무시하는 직접적인 피해를 입습니다 (예: 중독).
        public void TakeDirectDamage(int damage)
        {
            int actualDamage = health.TakeDirectDamage(damage);

            if (actualDamage > 0 && FeedbackManager.Instance != null)
                FeedbackManager.Instance.ShowDamageNumber(actualDamage, transform.position);

            if (health.IsDead)
            {
                Die();
            }
        }

        // 사망 처리 로직.
        private void Die()
        {
            // 플레이어에게 수련치 보상 지급
            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.GetPlayer()?.GainXp(enemyData?.xpReward ?? 0);
                // BattleManager에게 사망했음을 알림 (전투 종료 체크용)
                BattleManager.Instance.OnEnemyDied(this);
            }

            // 자기 자신을 파괴
            Destroy(gameObject);
        }

        // 자신의 턴에 행동을 수행합니다.
        public void TakeTurn(Player target)
        {
            if (nextAction == null) return;

            // 공격 시, 자신의 '힘'과 '약화' 상태를 고려하여 최종 피해량 계산
            int finalAttackValue = nextAction.value;
            if (nextAction.actionType == EnemyActionType.Attack)
            {
                finalAttackValue += GetStatusEffectValue(StatusEffectType.Strength);
                finalAttackValue = statusEffects.CalculateDamageDealt(finalAttackValue);
            }

            switch (nextAction.actionType)
            {
                case EnemyActionType.Attack:
                    target.TakeDamage(finalAttackValue);
                    break;
                case EnemyActionType.Defend:
                    health.GainDefense(nextAction.value);
                    break;
                case EnemyActionType.Buff:
                    ApplyStatusEffect(new StatusEffect(nextAction.statusEffectData, nextAction.value));
                    break;
                case EnemyActionType.Debuff:
                    target.ApplyStatusEffect(new StatusEffect(nextAction.statusEffectData, nextAction.value));
                    break;
            }

            // 다음 턴 행동 계획
            turnIndex++;
            PlanNextAction();
        }

        // 상태 이상을 적용합니다. IDamageable 인터페이스 구현.
        public void ApplyStatusEffect(StatusEffect effectToApply)
        {
            statusEffects.ApplyStatusEffect(effectToApply);
        }

        public int GetStatusEffectValue(StatusEffectType type)
        {
            return statusEffects.GetStatusEffectValue(type);
        }

        /// 턴 종료 시 지속 시간이 있는 상태 이상들을 처리합니다.
        public void ProcessStatusEffectsOnTurnEnd()
        {
            statusEffects.ProcessStatusEffectsOnTurnEnd();
        }

        // 다음 행동을 반환합니다 (UI 표시용).
        public EnemyAction GetNextAction() => nextAction;

        // 행동 패턴에 따라 다음 행동을 계획합니다.
        private void PlanNextAction()
        {
            if (actionPattern == null || actionPattern.Count == 0) return;
            nextAction = actionPattern[turnIndex % actionPattern.Count];
            OnStateChanged?.Invoke(health.CurrentHealth, health.MaxHealth, nextAction);
        }

        private void SwitchToPhase2()
        {
            currentPhase = 2;
            actionPattern = new List<EnemyAction>(encounterData.actionPatternPhase2);
            turnIndex = 0; // 2페이즈 패턴의 처음부터 시작

            // 페이즈 전환 시 모든 디버프 제거 및 강력한 버프 (예시)
            statusEffects.RemoveAllDebuffs();
            if (ResourceManager.Instance != null)
                ApplyStatusEffect(new StatusEffect(ResourceManager.Instance.GetStatusEffectData("Strength"), 3));

            Debug.LogWarning($"<color=red>{enemyData.enemyName}이(가) 분노합니다! (2페이즈 돌입)</color>");
        }
    }
}
