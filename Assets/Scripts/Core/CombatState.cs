namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 전투 시스템의 상태를 정의하는 열거형
    /// </summary>
    public enum CombatState
    {
        /// <summary>전투 시작 전</summary>
        None,

        /// <summary>전투 초기화 중</summary>
        Initializing,

        /// <summary>플레이어 턴</summary>
        PlayerTurn,

        /// <summary>적 턴</summary>
        EnemyTurn,

        /// <summary>전투 승리</summary>
        Victory,

        /// <summary>전투 패배</summary>
        Defeat
    }
}
