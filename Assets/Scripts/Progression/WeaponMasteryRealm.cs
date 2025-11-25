using UnityEngine;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 무기술 경지 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class WeaponMasteryRealm
    {
        public WeaponType weaponType;       // 무기 타입
        public int stage;                   // 경지 단계 (1~5)
        public string name;                 // 경지 이름 (예: "검기상인")
        public int damageBonus;             // 피해량 보너스
        public string effectDescription;    // 효과 설명
        public int requiredXP;              // 다음 단계 필요 경험치

        public WeaponMasteryRealm(WeaponType type, int stage, string name, int damage, string desc, int xp)
        {
            this.weaponType = type;
            this.stage = stage;
            this.name = name;
            this.damageBonus = damage;
            this.effectDescription = desc;
            this.requiredXP = xp;
        }
    }
}
