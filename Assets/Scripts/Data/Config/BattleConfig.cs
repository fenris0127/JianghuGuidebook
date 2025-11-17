using UnityEngine;

namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 전투 시스템 설정을 관리하는 ScriptableObject
    /// 타이밍과 애니메이션 대기 시간 등을 설정합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "BattleConfig", menuName = "GangHoBiGeup/Config/Battle Config")]
    public class BattleConfig : ScriptableObject
    {
        [Header("=== 전투 타이밍 설정 ===")]
        [Tooltip("전투 시작 시 대기 시간 (초)")]
        public float battleStartDelay = 0.5f;

        [Tooltip("적 턴 시작 전 대기 시간 (초)")]
        public float enemyTurnStartDelay = 0.5f;

        [Tooltip("적 턴 종료 후 대기 시간 (초)")]
        public float enemyTurnEndDelay = 1f;
    }
}
