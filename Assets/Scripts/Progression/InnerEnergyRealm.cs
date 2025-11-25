using UnityEngine;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 내공 경지 단계
    /// </summary>
    public enum RealmStage
    {
        Hucheon,        // 후천 (기본)
        Seoncheon,      // 선천
        Hwagyeong,      // 화경
        Jongsa,         // 종사
        CheonInhapil    // 천인합일
    }

    /// <summary>
    /// 내공 경지 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class InnerEnergyRealm
    {
        public RealmStage stage;            // 경지 단계
        public string name;                 // 경지 이름 (예: "화경")
        public string description;          // 경지 설명
        public string conditionDescription; // 달성 조건 설명

        [Header("보너스 스탯")]
        public int maxEnergyBonus;          // 최대 내공 증가량
        public int drawBonus;               // 드로우 증가량
        public float damageMultiplier;      // 내공 카드 피해 배율 (1.0 = 100%)
        public float energyRefundChance;    // 내공 사용 시 반환 확률 (0.0 ~ 1.0)

        public InnerEnergyRealm(RealmStage stage, string name, string desc, string condDesc)
        {
            this.stage = stage;
            this.name = name;
            this.description = desc;
            this.conditionDescription = condDesc;
            this.maxEnergyBonus = 0;
            this.drawBonus = 0;
            this.damageMultiplier = 1.0f;
            this.energyRefundChance = 0.0f;
        }
    }
}
