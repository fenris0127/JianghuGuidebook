namespace GangHoBiGeup.Gameplay
{
    // 모든 상태 이상 효과의 동작을 정의하는 기반 클래스입니다. (스트래티지 패턴)
    public abstract class StatusEffectBehavior
    {
        public StatusEffect Effect { get; private set; }
        protected IDamageable owner;

        public void Setup(StatusEffect effect, IDamageable owner)
        {
            this.Effect = effect;
            this.owner = owner;
        }

        public virtual void OnApplied() { }
        
        public virtual void OnTurnStart() { }
        
        public virtual void OnTurnEnd() 
        { 
            // TDD: 턴 종료 시 지속 시간 감소
            if (Effect.Duration > 0)
            {
                Effect.Duration--;
            }
        }
        
        public virtual int OnDamageDealt(int damage) { return damage; }
        public virtual int OnDamageTaken(int damage) { return damage; }
        
        public virtual bool IsFinished() 
        { 
            // TDD: Duration이 0이 되거나 Value가 0 이하면 종료
            return Effect.Duration == 0 || Effect.Value <= 0; 
        }
    }

    // 팩토리 클래스는 StatusEffect 타입에 맞는 Behavior 인스턴스를 생성합니다.
    public static class StatusEffectFactory
    {
        public static StatusEffectBehavior Create(StatusEffect effect)
        {
            switch (effect.Data.type)
            {
                case StatusEffectType.Poison:       return new PoisonBehavior();
                case StatusEffectType.Vulnerable:   return new VulnerableBehavior();
                case StatusEffectType.Weak:         return new WeakBehavior();
                case StatusEffectType.Strength:     return new StrengthBehavior();
                case StatusEffectType.Thorns:       return new ThornsBehavior();
                default:                            return new DefaultBehavior(); // 아무것도 안하는 기본 비헤이비어
            }
        }
    }
}
