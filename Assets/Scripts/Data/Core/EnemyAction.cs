using System;
using UnityEngine;

namespace GangHoBiGeup.Gameplay
{
    // 적의 행동 타입을 정의합니다.
    public enum EnemyActionType 
    { 
        Attack, 
        Defend, 
        Buff, 
        Debuff 
    }

    // 적의 단일 행동(타입과 수치)을 정의하는 클래스입니다.
    [Serializable]
    public class EnemyAction
    {
        public EnemyActionType actionType;
        public int value;

        [Tooltip("행동 타입이 Buff 또는 Debuff일 경우, 적용할 상태 이상 데이터입니다.")]
        public StatusEffectData statusEffectData;
    }
}
