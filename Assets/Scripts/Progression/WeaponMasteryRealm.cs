using UnityEngine;
using System;
using System.Collections.Generic;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 무기술 경지 단계
    /// </summary>
    public enum MasteryTier
    {
        Beginner = 0,       // 입문 (시작)
        Minor = 1,          // 소성 (작은 성취)
        Major = 2,          // 대성 (큰 성취)
        Transcendent = 3,   // 화경 (승화의 경지)
        Master = 4          // 최고경지 (무기술별 고유 명칭)
    }

    /// <summary>
    /// 무기술별 최고경지 명칭
    /// </summary>
    public static class MasterTierNames
    {
        public static readonly Dictionary<WeaponType, string> Names = new Dictionary<WeaponType, string>
        {
            { WeaponType.Sword, "검심통명" },       // 劍心通明 - 검의 마음이 통달함
            { WeaponType.Saber, "도의" },           // 刀意 - 도의 뜻
            { WeaponType.Spear, "창신" },           // 槍神 - 창의 신
            { WeaponType.Palm, "장화경" },          // 掌化境 - 장법의 화경
            { WeaponType.Fist, "권제" },            // 拳帝 - 권법의 제왕
            { WeaponType.ExoticWeapon, "기문대가" } // 奇門大家 - 기문의 대가
        };

        public static string GetName(WeaponType weaponType)
        {
            return Names.ContainsKey(weaponType) ? Names[weaponType] : "최고경지";
        }
    }

    /// <summary>
    /// 경지 단계별 명칭
    /// </summary>
    public static class TierDisplayNames
    {
        public static string GetDisplayName(MasteryTier tier, WeaponType weaponType)
        {
            switch (tier)
            {
                case MasteryTier.Beginner:
                    return "입문";
                case MasteryTier.Minor:
                    return "소성";
                case MasteryTier.Major:
                    return "대성";
                case MasteryTier.Transcendent:
                    return "화경";
                case MasteryTier.Master:
                    return MasterTierNames.GetName(weaponType);
                default:
                    return "알 수 없음";
            }
        }
    }

    /// <summary>
    /// 무기술 경지 데이터
    /// </summary>
    [System.Serializable]
    public class WeaponMasteryData
    {
        public string id;
        public WeaponType weaponType;
        public MasteryTier tier;
        public string displayName;
        public string description;

        // 달성 조건
        public MasteryCondition condition;

        // 보상
        public MasteryReward reward;

        public bool Validate()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("WeaponMasteryData: id가 비어있습니다");
                return false;
            }

            if (condition == null)
            {
                Debug.LogError($"WeaponMasteryData {id}: 조건이 없습니다");
                return false;
            }

            if (reward == null)
            {
                Debug.LogError($"WeaponMasteryData {id}: 보상이 없습니다");
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 경지 달성 조건
    /// </summary>
    [System.Serializable]
    public class MasteryCondition
    {
        // 조건 타입
        public MasteryConditionType type;

        // 카드 보유 조건
        public int requiredCardCount;           // 해당 무기술 카드 N장 보유

        // 피해 누적 조건
        public int requiredDamageDealt;         // 해당 무기술 카드로 N 피해 누적

        // 순수 빌드 조건
        public bool requirePureBuild;           // 한 전투에서 해당 무기술 카드만 사용하여 승리

        // 비기 사용 조건
        public int requiredSecretSkillsUsed;    // 해당 무기술 비기 N종 사용

        /// <summary>
        /// 조건 충족 여부 확인
        /// </summary>
        public bool IsMet(WeaponMasteryProgress progress)
        {
            switch (type)
            {
                case MasteryConditionType.CardCount:
                    return progress.cardsOwned >= requiredCardCount;

                case MasteryConditionType.DamageDealt:
                    return progress.totalDamageDealt >= requiredDamageDealt;

                case MasteryConditionType.PureBuild:
                    return progress.pureBuildVictories > 0;

                case MasteryConditionType.SecretSkills:
                    return progress.secretSkillsUsed.Count >= requiredSecretSkillsUsed;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 조건 설명 텍스트 반환
        /// </summary>
        public string GetDescription()
        {
            switch (type)
            {
                case MasteryConditionType.CardCount:
                    return $"해당 무기술 카드 {requiredCardCount}장 보유";

                case MasteryConditionType.DamageDealt:
                    return $"해당 무기술 카드로 {requiredDamageDealt} 피해 누적";

                case MasteryConditionType.PureBuild:
                    return "한 전투에서 해당 무기술 카드만 사용하여 승리";

                case MasteryConditionType.SecretSkills:
                    return $"해당 무기술 비기 {requiredSecretSkillsUsed}종 사용";

                default:
                    return "알 수 없는 조건";
            }
        }

        /// <summary>
        /// 진행도 표시 텍스트 반환
        /// </summary>
        public string GetProgressText(WeaponMasteryProgress progress)
        {
            switch (type)
            {
                case MasteryConditionType.CardCount:
                    return $"{progress.cardsOwned} / {requiredCardCount}";

                case MasteryConditionType.DamageDealt:
                    return $"{progress.totalDamageDealt} / {requiredDamageDealt}";

                case MasteryConditionType.PureBuild:
                    return progress.pureBuildVictories > 0 ? "달성" : "미달성";

                case MasteryConditionType.SecretSkills:
                    return $"{progress.secretSkillsUsed.Count} / {requiredSecretSkillsUsed}";

                default:
                    return "?";
            }
        }
    }

    /// <summary>
    /// 조건 타입
    /// </summary>
    public enum MasteryConditionType
    {
        CardCount,      // 카드 보유 수
        DamageDealt,    // 피해 누적
        PureBuild,      // 순수 빌드 승리
        SecretSkills    // 비기 사용
    }

    /// <summary>
    /// 경지 돌파 보상
    /// </summary>
    [System.Serializable]
    public class MasteryReward
    {
        public string description;

        // 피해 증가 보너스 (%)
        public float damageBonus;

        // 비용 감소
        public int costReduction;

        // 추가 효과
        public string specialEffect;

        // 카드 해금
        public List<string> unlockedCardIds = new List<string>();

        public void Apply(WeaponType weaponType)
        {
            Debug.Log($"[WeaponMastery] {weaponType} 경지 보상 적용: {description}");

            // 해금된 카드가 있다면 처리
            if (unlockedCardIds != null && unlockedCardIds.Count > 0)
            {
                foreach (string cardId in unlockedCardIds)
                {
                    Debug.Log($"  - 카드 해금: {cardId}");
                    // TODO: 카드 풀에 추가하는 로직 (CardPoolManager 등에서 처리)
                }
            }
        }
    }

    /// <summary>
    /// 무기술 경지 진행 상황
    /// </summary>
    [System.Serializable]
    public class WeaponMasteryProgress
    {
        public WeaponType weaponType;
        public MasteryTier currentTier;

        // 진행도 추적
        public int cardsOwned;                          // 보유한 해당 무기술 카드 수
        public int totalDamageDealt;                    // 누적 피해
        public int pureBuildVictories;                  // 순수 빌드 승리 횟수
        public HashSet<string> secretSkillsUsed;        // 사용한 비기 ID

        // 전투별 추적 (순수 빌드 체크용)
        [NonSerialized] public bool usedOnlyThisWeaponInCombat;
        [NonSerialized] public int damageDealtThisCombat;

        public WeaponMasteryProgress(WeaponType type)
        {
            weaponType = type;
            currentTier = MasteryTier.Beginner;
            cardsOwned = 0;
            totalDamageDealt = 0;
            pureBuildVictories = 0;
            secretSkillsUsed = new HashSet<string>();
            usedOnlyThisWeaponInCombat = true;
            damageDealtThisCombat = 0;
        }

        /// <summary>
        /// 카드 사용 시 호출
        /// </summary>
        public void OnCardUsed(string cardId, WeaponType cardWeaponType, int damage, bool isSecretSkill)
        {
            // 다른 무기술 카드를 사용하면 순수 빌드 실패
            if (cardWeaponType != weaponType)
            {
                usedOnlyThisWeaponInCombat = false;
            }

            // 이 무기술 카드라면 피해 누적
            if (cardWeaponType == weaponType)
            {
                totalDamageDealt += damage;
                damageDealtThisCombat += damage;

                // 비기라면 기록
                if (isSecretSkill)
                {
                    secretSkillsUsed.Add(cardId);
                }
            }
        }

        /// <summary>
        /// 전투 종료 시 호출
        /// </summary>
        public void OnCombatEnd(bool victory)
        {
            if (victory && usedOnlyThisWeaponInCombat && damageDealtThisCombat > 0)
            {
                pureBuildVictories++;
                Debug.Log($"[WeaponMastery] {weaponType} 순수 빌드 승리! (총 {pureBuildVictories}회)");
            }

            // 리셋
            usedOnlyThisWeaponInCombat = true;
            damageDealtThisCombat = 0;
        }

        /// <summary>
        /// 전투 시작 시 호출
        /// </summary>
        public void OnCombatStart()
        {
            usedOnlyThisWeaponInCombat = true;
            damageDealtThisCombat = 0;
        }

        /// <summary>
        /// 덱 업데이트 시 카드 수 갱신
        /// </summary>
        public void UpdateCardCount(List<string> deckCardIds)
        {
            cardsOwned = 0;

            foreach (string cardId in deckCardIds)
            {
                // TODO: CardData에서 무기술 타입 확인하여 카운트
                // 지금은 간단히 접두사로 판단
                if (IsCardOfWeaponType(cardId, weaponType))
                {
                    cardsOwned++;
                }
            }

            Debug.Log($"[WeaponMastery] {weaponType} 카드 보유: {cardsOwned}장");
        }

        /// <summary>
        /// 카드가 특정 무기술에 속하는지 확인 (임시 구현)
        /// </summary>
        private bool IsCardOfWeaponType(string cardId, WeaponType type)
        {
            // TODO: DataManager를 통해 CardData를 가져와서 정확히 확인
            // 지금은 간단히 id 패턴으로 판단
            switch (type)
            {
                case WeaponType.Sword:
                    return cardId.Contains("sword") || cardId.Contains("검");
                case WeaponType.Saber:
                    return cardId.Contains("saber") || cardId.Contains("도");
                case WeaponType.Spear:
                    return cardId.Contains("spear") || cardId.Contains("창");
                case WeaponType.Palm:
                    return cardId.Contains("palm") || cardId.Contains("장");
                case WeaponType.Fist:
                    return cardId.Contains("fist") || cardId.Contains("권");
                case WeaponType.ExoticWeapon:
                    return cardId.Contains("exotic") || cardId.Contains("기문");
                default:
                    return false;
            }
        }

        /// <summary>
        /// 경지 상승
        /// </summary>
        public void AdvanceTier()
        {
            if (currentTier < MasteryTier.Master)
            {
                currentTier++;
                Debug.Log($"[WeaponMastery] {weaponType} 경지 상승: {currentTier}");
            }
        }
    }

    /// <summary>
    /// 무기술 경지 데이터베이스
    /// </summary>
    [System.Serializable]
    public class WeaponMasteryDatabase
    {
        public List<WeaponMasteryData> masteries = new List<WeaponMasteryData>();

        public bool Validate()
        {
            if (masteries == null || masteries.Count == 0)
            {
                Debug.LogError("WeaponMasteryDatabase: 경지 데이터가 없습니다");
                return false;
            }

            bool allValid = true;
            foreach (var mastery in masteries)
            {
                if (!mastery.Validate())
                {
                    allValid = false;
                }
            }

            return allValid;
        }

        /// <summary>
        /// 특정 무기술과 경지 단계에 해당하는 데이터 찾기
        /// </summary>
        public WeaponMasteryData GetMasteryData(WeaponType weaponType, MasteryTier tier)
        {
            return masteries.Find(m => m.weaponType == weaponType && m.tier == tier);
        }

        /// <summary>
        /// 특정 무기술의 모든 경지 데이터 가져오기
        /// </summary>
        public List<WeaponMasteryData> GetMasteriesForWeapon(WeaponType weaponType)
        {
            return masteries.FindAll(m => m.weaponType == weaponType);
        }
    }

    /// <summary>
    /// 무기술 경지 데이터 클래스 (레거시 호환용)
    /// </summary>
    [System.Serializable]
    public class WeaponMasteryRealm
    {
        public WeaponType weaponType;       // 무기 타입
        public int stage;                   // 경지 단계 (1~5)
        public string name;                 // 경지 이름
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
