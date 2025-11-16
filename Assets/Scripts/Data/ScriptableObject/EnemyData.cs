using System.Collections.Generic;
using UnityEngine;

namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 적 데이터를 저장하는 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "New Enemy", menuName = "GangHoBiGeup/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("기본 속성")]
        public string id;
        public string enemyName;
        public int maxHealth;
        
        [Header("보상")]
        public int xpReward = 5;
        
        [Header("행동 패턴")]
        public List<ActionPattern> actionPatterns = new List<ActionPattern>();
        
        // 기존 코드 호환성을 위한 속성
        public List<GangHoBiGeup.Gameplay.EnemyAction> actionPattern 
        { 
            get 
            {
                // ActionPattern을 EnemyAction으로 변환
                var result = new List<GangHoBiGeup.Gameplay.EnemyAction>();
                // 변환 로직은 나중에 구현
                return result;
            }
        }
    }
}
