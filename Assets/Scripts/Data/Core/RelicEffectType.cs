namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 유물의 효과 타입
    /// </summary>
    public enum RelicEffectType
    {
        None,               // 효과 없음
        GainBlock,          // 방어도 획득
        GainEnergy,         // 내공 획득
        DrawCard,           // 카드 뽑기
        GainStrength,       // 힘 획득
        GainGold,           // 골드 획득
        HealHealth,         // 체력 회복
        DamageAllEnemies,   // 모든 적에게 피해
        ReduceCardCost,     // 카드 비용 감소
        ExtraCardReward     // 추가 카드 보상
    }
}
