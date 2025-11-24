using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Relics
{
    /// <summary>
    /// 유물 효과 인터페이스
    /// 모든 유물 효과는 이 인터페이스를 구현해야 합니다
    /// </summary>
    public interface IRelicEffect
    {
        void OnCombatStart(Player player);
        void OnTurnStart(Player player);
        void OnTurnEnd(Player player);
        void OnCardPlay(Card card, Player player, Enemy target);
        void OnAttack(int damage, Enemy target);
        void OnDefend(int block, Player player);
        void OnDamageReceived(int damage, Player player);
        void OnEnemyDeath(Enemy enemy);
        void OnDraw(Card card);
        void OnDiscard(Card card);
        void OnRest();
        void OnShop();
        void OnVictory();
    }

    /// <summary>
    /// 기본 유물 효과 클래스
    /// 필요한 메서드만 오버라이드하면 됩니다
    /// </summary>
    public class BaseRelicEffect : IRelicEffect
    {
        protected Relic relic;

        public BaseRelicEffect(Relic relic)
        {
            this.relic = relic;
        }

        public virtual void OnCombatStart(Player player) { }
        public virtual void OnTurnStart(Player player) { }
        public virtual void OnTurnEnd(Player player) { }
        public virtual void OnCardPlay(Card card, Player player, Enemy target) { }
        public virtual void OnAttack(int damage, Enemy target) { }
        public virtual void OnDefend(int block, Player player) { }
        public virtual void OnDamageReceived(int damage, Player player) { }
        public virtual void OnEnemyDeath(Enemy enemy) { }
        public virtual void OnDraw(Card card) { }
        public virtual void OnDiscard(Card card) { }
        public virtual void OnRest() { }
        public virtual void OnShop() { }
        public virtual void OnVictory() { }
    }
}
