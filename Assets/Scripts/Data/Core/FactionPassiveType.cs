namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 문파의 패시브 능력 타입
    /// </summary>
    public enum FactionPassiveType
    {
        None,               // 패시브 없음
        ComboBonus,         // 연계 보너스 (화산파)
        HealthCost,         // 체력 소모 강화 (천마신교)
        PoisonSynergy,      // 중독 시너지 (사천당문)
        StrengthFocus,      // 힘 집중 (하북팽가)
        EnergyRegeneration, // 내공 재생
        DrawPower,          // 드로우 강화
        BlockBonus          // 방어도 보너스
    }
}
