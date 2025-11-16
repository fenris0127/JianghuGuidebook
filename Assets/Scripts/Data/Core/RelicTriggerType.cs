namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 유물이 발동하는 시점
    /// </summary>
    public enum RelicTriggerType
    {
        None,               // 발동 없음
        OnBattleStart,      // 전투 시작 시
        OnTurnStart,        // 턴 시작 시
        OnTurnEnd,          // 턴 종료 시
        OnCardPlayed,       // 카드 사용 시
        OnEnemyDeath,       // 적 처치 시
        OnDamageTaken,      // 피해를 받을 때
        OnDamageDealt,      // 피해를 줄 때
        OnVictory,          // 전투 승리 시
        Passive             // 상시 효과
    }
}
