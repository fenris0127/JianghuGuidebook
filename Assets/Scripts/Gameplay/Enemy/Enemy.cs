using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// 적 캐릭터의 데이터를 관리하고 행동 로직을 수행하는 클래스입니다.
public class Enemy : MonoBehaviour, IDamageable
{
    // 이벤트: 자신의 상태(체력, 다음 행동)가 변경되었음을 UI에 알림
    public event Action<int, int, EnemyAction> OnStateChanged;
    public event Action<List<StatusEffect>> OnStatusEffectsChanged;
    
    // 상태 변수
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public int defense { get; private set; }
    
    // 상태 이상 목록
    private List<StatusEffectBehavior> activeStatusEffects = new List<StatusEffectBehavior>();
    
    // 전투 정보
    public EnemyData enemyData { get; private set; }
    private EncounterData encounterData;
    private List<EnemyAction> actionPattern;
    private int turnIndex = 0;
    private int currentPhase = 1;
    private EnemyAction nextAction;

    // BattleManager가 호출하여 적을 초기화합니다.
    public void Setup(EnemyData data, EncounterData encounter)
    {
        this.enemyData = data;
        this.encounterData = encounter;
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        actionPattern = new List<EnemyAction>(data.actionPattern);
        turnIndex = 0;
        currentPhase = 1;
        defense = 0;
        activeStatusEffects.Clear();
        
        // 첫 행동 계획 및 UI 업데이트 요청
        PlanNextAction();
        OnStatusEffectsChanged?.Invoke(activeStatusEffects.Select(b => b.Effect).ToList());
    }

    // 피해를 받는 로직을 처리합니다. IDamageable 인터페이스 구현.
    public void TakeDamage(int damage)
    {
        int finalDamage = damage;

        // 새로운 상태 이상 시스템을 통해 받는 피해량 계산
        foreach (var behavior in activeStatusEffects)
            finalDamage = behavior.OnDamageTaken(finalDamage);

        finalDamage = Mathf.Max(0, finalDamage - defense);
        
        currentHealth -= finalDamage;
        
        if (finalDamage > 0)
            FeedbackManager.Instance.ShowDamageNumber(finalDamage, transform.position);

        if (encounterData.hasMultiplePhases && currentPhase == 1 && 
            (float)currentHealth / maxHealth <= encounterData.phaseTransitionHealthThreshold &&
            encounterData.actionPatternPhase2 != null && encounterData.actionPatternPhase2.Count > 0)
            SwitchToPhase2();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        OnStateChanged?.Invoke(currentHealth, maxHealth, nextAction);
    }

    // 방어도를 무시하는 직접적인 피해를 입습니다 (예: 중독).
    public void TakeDirectDamage(int damage)
    {
        currentHealth -= damage;
        if (damage > 0)
            FeedbackManager.Instance.ShowDamageNumber(damage, transform.position);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        OnStateChanged?.Invoke(currentHealth, maxHealth, nextAction);
    }

    // 사망 처리 로직.
    private void Die()
    {
        // 플레이어에게 수련치 보상 지급
        BattleManager.Instance.GetPlayer()?.GainXp(enemyData.xpReward);
        
        // BattleManager에게 사망했음을 알림 (전투 종료 체크용)
        BattleManager.Instance.OnEnemyDied(this);
        
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
            foreach (var behavior in activeStatusEffects)
                finalAttackValue = behavior.OnDamageDealt(finalAttackValue);
        }

        switch (nextAction.actionType)
        {
            case EnemyActionType.Attack:
                target.TakeDamage(nextAction.value + GetStatusEffectValue(StatusEffectType.Strength));
                break;
            case EnemyActionType.Defend:
                defense += nextAction.value;
                break;
            case EnemyActionType.Buff:
                ApplyStatusEffect(new StatusEffect(nextAction.statusEffectData, nextAction.value));
                break;
            case EnemyActionType.Debuff:
                // 올바른 생성자 문법 사용
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
        var existingEffect = activeStatusEffects.Find(e => e.Effect.Data.type == effectToApply.Data.type);
        if (existingEffect != null)
            existingEffect.Effect.Value += effectToApply.Value;
        else
        {
            StatusEffectBehavior newBehavior = StatusEffectFactory.Create(effectToApply);
            newBehavior.Setup(effectToApply, this);
            activeStatusEffects.Add(newBehavior);
            newBehavior.OnApplied();
        }

        OnStatusEffectsChanged?.Invoke(activeStatusEffects.Select(b => b.Effect).ToList());
    }
    
    public int GetStatusEffectValue(StatusEffectType type)
    {
        var behavior = activeStatusEffects.Find(b => b.Effect.Data.type == type);
        return behavior?.Effect.Value ?? 0;
    }

    /// 턴 종료 시 지속 시간이 있는 상태 이상들을 처리합니다.
    public void ProcessStatusEffectsOnTurnEnd()
    {
        bool changed = false;
        for(int i = activeStatusEffects.Count - 1; i >= 0; i--)
        {
            activeStatusEffects[i].OnTurnEnd();
            if(activeStatusEffects[i].IsFinished())
            {
                activeStatusEffects.RemoveAt(i);
                changed = true;
            }
        }
        
        if (changed)
            OnStatusEffectsChanged?.Invoke(activeStatusEffects.Select(b => b.Effect).ToList());
    }

    // 다음 행동을 반환합니다 (UI 표시용).
    public EnemyAction GetNextAction() => nextAction;
    
    // 행동 패턴에 따라 다음 행동을 계획합니다.
    private void PlanNextAction()
    {
        if (actionPattern == null || actionPattern.Count == 0) return;
        nextAction = actionPattern[turnIndex % actionPattern.Count];
        OnStateChanged?.Invoke(currentHealth, maxHealth, nextAction);
    }

    private void SwitchToPhase2()
    {
        currentPhase = 2;
        actionPattern = new List<EnemyAction>(encounterData.actionPatternPhase2);
        turnIndex = 0; // 2페이즈 패턴의 처음부터 시작
        
        // 페이즈 전환 시 모든 디버프 제거 및 강력한 버프 (예시)
        activeStatusEffects.RemoveAll(b => !b.Effect.Data.isBuff);
        ApplyStatusEffect(new StatusEffect(ResourceManager.Instance.GetStatusEffectData("Strength"), 3));

        Debug.LogWarning($"<color=red>{enemyData.enemyName}이(가) 분노합니다! (2페이즈 돌입)</color>");
        
        // 변경된 상태 이상 목록을 UI에 즉시 반영
        OnStatusEffectsChanged?.Invoke(activeStatusEffects.Select(b => b.Effect).ToList());
    }
}