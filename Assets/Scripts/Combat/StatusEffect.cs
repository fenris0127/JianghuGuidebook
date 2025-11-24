using UnityEngine;

namespace JianghuGuidebook.Combat
{
    /// <summary>
    /// 상태 효과의 기본 클래스
    /// 모든 버프와 디버프는 이 클래스를 상속받습니다
    /// </summary>
    public abstract class StatusEffect
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Stacks { get; protected set; }
        public int Duration { get; protected set; }

        protected StatusEffect(string name, string description, int stacks = 1, int duration = -1)
        {
            Name = name;
            Description = description;
            Stacks = stacks;
            Duration = duration;
        }

        /// <summary>
        /// 상태 효과가 적용될 때 호출됩니다
        /// </summary>
        public virtual void Apply(Player player)
        {
            Debug.Log($"플레이어에게 상태 효과 적용: {Name} (스택: {Stacks})");
        }

        /// <summary>
        /// 상태 효과가 적용될 때 호출됩니다 (적용 대상: 적)
        /// </summary>
        public virtual void Apply(Enemy enemy)
        {
            Debug.Log($"적에게 상태 효과 적용: {Name} (스택: {Stacks})");
        }

        /// <summary>
        /// 상태 효과가 제거될 때 호출됩니다
        /// </summary>
        public virtual void Remove(Player player)
        {
            Debug.Log($"플레이어의 상태 효과 제거: {Name}");
        }

        /// <summary>
        /// 상태 효과가 제거될 때 호출됩니다 (제거 대상: 적)
        /// </summary>
        public virtual void Remove(Enemy enemy)
        {
            Debug.Log($"적의 상태 효과 제거: {Name}");
        }

        /// <summary>
        /// 턴 시작 시 호출됩니다
        /// </summary>
        public virtual void OnTurnStart(Player player)
        {
            // 지속 시간 감소
            if (Duration > 0)
            {
                Duration--;
            }
        }

        /// <summary>
        /// 턴 시작 시 호출됩니다 (대상: 적)
        /// </summary>
        public virtual void OnTurnStart(Enemy enemy)
        {
            // 지속 시간 감소
            if (Duration > 0)
            {
                Duration--;
            }
        }

        /// <summary>
        /// 턴 종료 시 호출됩니다
        /// </summary>
        public virtual void OnTurnEnd(Player player)
        {
        }

        /// <summary>
        /// 턴 종료 시 호출됩니다 (대상: 적)
        /// </summary>
        public virtual void OnTurnEnd(Enemy enemy)
        {
        }

        /// <summary>
        /// 상태 효과의 스택을 추가합니다
        /// </summary>
        public void AddStacks(int amount)
        {
            Stacks += amount;
            Debug.Log($"{Name} 스택 증가: {amount} (현재: {Stacks})");
        }

        /// <summary>
        /// 상태 효과가 만료되었는지 확인합니다
        /// </summary>
        public bool IsExpired()
        {
            return Duration == 0;
        }
    }
}
