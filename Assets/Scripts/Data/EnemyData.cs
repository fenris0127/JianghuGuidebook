using System;
using UnityEngine;

namespace JianghuGuidebook.Data
{
    /// <summary>
    /// 적 행동 타입 열거형
    /// </summary>
    public enum EnemyActionType
    {
        Attack,     // 공격
        Defend,     // 방어
        Buff,       // 버프
        Debuff,     // 디버프
        Special     // 특수 행동
    }

    /// <summary>
    /// 적의 행동 패턴을 정의하는 클래스
    /// </summary>
    [Serializable]
    public class EnemyAction
    {
        public EnemyActionType type;    // 행동 타입
        public int value;               // 행동 값 (피해량, 방어도 등)
        public int weight;              // 가중치 (행동 선택 확률)

        public override string ToString()
        {
            return $"{type} (값: {value}, 가중치: {weight})";
        }
    }

    /// <summary>
    /// 적 데이터 클래스
    /// JSON에서 로드될 적 정보를 저장합니다
    /// </summary>
    [Serializable]
    public class EnemyData
    {
        public string id;               // 고유 ID (예: "enemy_bandit")
        public string name;             // 적 이름 (예: "산적")
        public int maxHealth;           // 최대 체력
        public EnemyAction[] actions;   // 행동 패턴 목록

        /// <summary>
        /// 적 데이터가 유효한지 검증합니다
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("EnemyData: ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"EnemyData: 적 {id}의 이름이 비어있습니다");
                return false;
            }

            if (maxHealth <= 0)
            {
                Debug.LogError($"EnemyData: 적 {id}의 체력이 0 이하입니다");
                return false;
            }

            if (actions == null || actions.Length == 0)
            {
                Debug.LogError($"EnemyData: 적 {id}의 행동 패턴이 없습니다");
                return false;
            }

            // 행동 패턴 유효성 검사
            foreach (var action in actions)
            {
                if (action.weight <= 0)
                {
                    Debug.LogError($"EnemyData: 적 {id}의 행동 가중치가 0 이하입니다");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 가중치를 기반으로 랜덤 행동을 선택합니다
        /// </summary>
        public EnemyAction GetRandomAction()
        {
            if (actions == null || actions.Length == 0)
            {
                Debug.LogError($"적 {name}의 행동 패턴이 없습니다");
                return null;
            }

            // 총 가중치 계산
            int totalWeight = 0;
            foreach (var action in actions)
            {
                totalWeight += action.weight;
            }

            // 랜덤 값 생성
            int randomValue = UnityEngine.Random.Range(0, totalWeight);

            // 가중치 기반 선택
            int currentWeight = 0;
            foreach (var action in actions)
            {
                currentWeight += action.weight;
                if (randomValue < currentWeight)
                {
                    return action;
                }
            }

            // 폴백: 첫 번째 행동 반환
            return actions[0];
        }

        public override string ToString()
        {
            return $"[{id}] {name} (HP: {maxHealth}) - {actions.Length}개 행동 패턴";
        }
    }

    /// <summary>
    /// 적 데이터베이스 래퍼 클래스
    /// JSON 역직렬화를 위한 컨테이너
    /// </summary>
    [Serializable]
    public class EnemyDatabase
    {
        public EnemyData[] enemies;
    }
}
