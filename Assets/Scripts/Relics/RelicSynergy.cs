using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace JianghuGuidebook.Relics
{
    /// <summary>
    /// 시너지 효과 타입
    /// </summary>
    public enum SynergyEffectType
    {
        DamageBonus,        // 피해 증가
        HealthBonus,        // 최대 체력 증가
        EnergyBonus,        // 내공 증가
        BlockBonus,         // 방어도 증가
        GoldBonus,          // 골드 획득 증가
        ShopDiscount,       // 상점 할인
        CardReward,         // 카드 보상 증가
        HealBonus,          // 회복량 증가
        DrawCard,           // 드로우 증가
        StartingRelic       // 추가 시작 유물
    }

    /// <summary>
    /// 유물 조합 시너지 클래스
    /// </summary>
    [System.Serializable]
    public class RelicSynergy
    {
        public string synergyId;
        public string synergyName;
        public string description;
        public List<string> requiredRelicIds;   // 필요한 유물 ID 리스트
        public int minRelicsRequired;           // 최소 필요 유물 수
        public SynergyEffectType effectType;
        public int effectValue;                 // 효과 수치
        public bool isActive;                   // 현재 활성화 여부

        public RelicSynergy(string id, string name, string desc, List<string> relicIds, int minRequired, SynergyEffectType type, int value)
        {
            synergyId = id;
            synergyName = name;
            description = desc;
            requiredRelicIds = relicIds;
            minRelicsRequired = minRequired;
            effectType = type;
            effectValue = value;
            isActive = false;
        }

        /// <summary>
        /// 시너지 조건을 충족하는지 확인합니다
        /// </summary>
        public bool CheckCondition(List<Relic> ownedRelics)
        {
            int matchCount = 0;

            foreach (string relicId in requiredRelicIds)
            {
                if (ownedRelics.Any(r => r.id == relicId))
                {
                    matchCount++;
                }
            }

            return matchCount >= minRelicsRequired;
        }

        public override string ToString()
        {
            string status = isActive ? "[활성화]" : "[비활성화]";
            return $"{status} {synergyName}: {description}";
        }
    }

    /// <summary>
    /// 시너지 관리 클래스
    /// </summary>
    public class SynergyManager
    {
        private List<RelicSynergy> allSynergies;
        private List<RelicSynergy> activeSynergies;

        public List<RelicSynergy> ActiveSynergies => activeSynergies;

        public SynergyManager()
        {
            allSynergies = new List<RelicSynergy>();
            activeSynergies = new List<RelicSynergy>();
            InitializeSynergies();
        }

        /// <summary>
        /// 기본 시너지들을 초기화합니다
        /// </summary>
        private void InitializeSynergies()
        {
            // 검술 대가: 검 관련 유물 2개 이상
            allSynergies.Add(new RelicSynergy(
                "synergy_sword_master",
                "검술 대가",
                "검 관련 유물 2개 이상 보유 시 공격 피해 +20%",
                new List<string> { "relic_old_sword_sheath", "relic_iron_sword", "relic_dragon_scale_sword" },
                2,
                SynergyEffectType.DamageBonus,
                20
            ));

            // 불멸의 체질: 회복/체력 유물 2개 이상
            allSynergies.Add(new RelicSynergy(
                "synergy_immortal_body",
                "불멸의 체질",
                "회복 관련 유물 2개 이상 보유 시 최대 체력 +15",
                new List<string> { "relic_qixue_pill", "relic_century_ginseng", "relic_ice_jade_pill" },
                2,
                SynergyEffectType.HealthBonus,
                15
            ));

            // 재물의 신: 골드 유물 2개 이상
            allSynergies.Add(new RelicSynergy(
                "synergy_god_of_wealth",
                "재물의 신",
                "골드 관련 유물 2개 이상 보유 시 상점 가격 10% 할인",
                new List<string> { "relic_golden_tael", "relic_merchant_token", "relic_jade_pendant" },
                2,
                SynergyEffectType.ShopDiscount,
                10
            ));

            // 내공 수련: 내공 유물 2개 이상
            allSynergies.Add(new RelicSynergy(
                "synergy_inner_energy",
                "내공 대가",
                "내공 관련 유물 2개 이상 보유 시 턴 시작 시 내공 +1",
                new List<string> { "relic_meditation_beads", "relic_ouyang_manual", "relic_xiaoyao_secret" },
                2,
                SynergyEffectType.EnergyBonus,
                1
            ));

            // 철벽 방어: 방어 유물 2개 이상
            allSynergies.Add(new RelicSynergy(
                "synergy_iron_wall",
                "철벽 방어",
                "방어 관련 유물 2개 이상 보유 시 방어도 획득량 +3",
                new List<string> { "relic_iron_armor", "relic_turtle_shell", "relic_golden_bell" },
                2,
                SynergyEffectType.BlockBonus,
                3
            ));

            // 무림 고수: 전설 유물 2개 이상
            allSynergies.Add(new RelicSynergy(
                "synergy_martial_master",
                "무림 고수",
                "전설 유물 2개 이상 보유 시 모든 카드 드로우 +1",
                new List<string> { "relic_xiaoyao_secret", "relic_dragon_scale_sword", "relic_nine_yang_scripture" },
                2,
                SynergyEffectType.DrawCard,
                1
            ));

            // 수집가: 유물 10개 이상 보유
            allSynergies.Add(new RelicSynergy(
                "synergy_collector",
                "수집가",
                "유물 10개 이상 보유 시 전투 시작 시 골드 +25",
                new List<string>(),  // 특정 유물이 아닌 총 개수로 체크
                10,
                SynergyEffectType.GoldBonus,
                25
            ));

            Debug.Log($"시너지 초기화 완료: {allSynergies.Count}개");
        }

        /// <summary>
        /// 현재 보유 유물로 시너지를 체크하고 업데이트합니다
        /// </summary>
        public void UpdateSynergies(List<Relic> ownedRelics)
        {
            activeSynergies.Clear();

            foreach (var synergy in allSynergies)
            {
                // 특별 처리: 수집가 시너지 (총 유물 개수)
                if (synergy.synergyId == "synergy_collector")
                {
                    synergy.isActive = ownedRelics.Count >= synergy.minRelicsRequired;
                }
                else
                {
                    // 일반 시너지: 특정 유물 조합
                    synergy.isActive = synergy.CheckCondition(ownedRelics);
                }

                if (synergy.isActive)
                {
                    activeSynergies.Add(synergy);
                    Debug.Log($"시너지 활성화: {synergy}");
                }
            }

            Debug.Log($"활성화된 시너지: {activeSynergies.Count}개");
        }

        /// <summary>
        /// 특정 타입의 시너지 보너스 값을 합산하여 반환합니다
        /// </summary>
        public int GetSynergyBonus(SynergyEffectType type)
        {
            int totalBonus = 0;

            foreach (var synergy in activeSynergies)
            {
                if (synergy.effectType == type)
                {
                    totalBonus += synergy.effectValue;
                }
            }

            return totalBonus;
        }

        /// <summary>
        /// 특정 타입의 시너지가 활성화되어 있는지 확인합니다
        /// </summary>
        public bool HasActiveSynergy(SynergyEffectType type)
        {
            return activeSynergies.Any(s => s.effectType == type);
        }

        /// <summary>
        /// 모든 활성 시너지 정보를 출력합니다
        /// </summary>
        public void PrintActiveSynergies()
        {
            Debug.Log("=== 활성화된 시너지 ===");
            if (activeSynergies.Count == 0)
            {
                Debug.Log("활성화된 시너지 없음");
                return;
            }

            foreach (var synergy in activeSynergies)
            {
                Debug.Log(synergy);
            }
        }
    }
}
