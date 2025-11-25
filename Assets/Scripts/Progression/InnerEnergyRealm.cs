using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 내공 경지 단계
    /// </summary>
    public enum InnerEnergyRealmLevel
    {
        Hucheon,        // 후천 (시작)
        Seoncheon,      // 선천
        Hwagyeong,      // 화경
        Jongsa,         // 종사
        CheoninHapil    // 천인합일 (최고)
    }

    /// <summary>
    /// 내공 경지 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class InnerEnergyRealm
    {
        public InnerEnergyRealmLevel level;     // 경지 레벨
        public string name;                     // 경지 이름
        public string description;              // 경지 설명
        public RealmCondition condition;        // 돌파 조건
        public List<RealmReward> rewards;       // 돌파 시 보상

        public InnerEnergyRealm()
        {
            rewards = new List<RealmReward>();
        }

        public override string ToString()
        {
            return $"{name} ({level})";
        }
    }

    /// <summary>
    /// 경지 돌파 조건
    /// </summary>
    [System.Serializable]
    public class RealmCondition
    {
        public RealmConditionType type;
        public int requiredValue;       // 필요한 값
        public string additionalInfo;   // 추가 정보

        public RealmCondition(RealmConditionType type, int value)
        {
            this.type = type;
            this.requiredValue = value;
        }

        public override string ToString()
        {
            return $"{type}: {requiredValue} {additionalInfo}";
        }
    }

    /// <summary>
    /// 경지 조건 타입
    /// </summary>
    public enum RealmConditionType
    {
        InnerEnergyCardUsed,        // 내공 카드 사용 횟수
        InnerEnergySpentInCombat,   // 한 전투에서 내공 소모량
        TotalInnerEnergySpent,      // 누적 내공 소모량
        InnerEnergySkillsUsed       // 내공 비기 사용 종류
    }

    /// <summary>
    /// 경지 돌파 보상
    /// </summary>
    [System.Serializable]
    public class RealmReward
    {
        public RealmRewardType type;
        public int value;
        public string description;

        public RealmReward(RealmRewardType type, int value, string description)
        {
            this.type = type;
            this.value = value;
            this.description = description;
        }

        public override string ToString()
        {
            return $"{type}: {description}";
        }
    }

    /// <summary>
    /// 경지 보상 타입
    /// </summary>
    public enum RealmRewardType
    {
        MaxEnergyIncrease,          // 최대 내공 증가
        CardDrawIncrease,           // 카드 드로우 증가
        InnerEnergyDamageBonus,     // 내공 카드 피해 증가
        EnergyRefund                // 내공 환원
    }

    /// <summary>
    /// 내공 경지 진행 데이터
    /// </summary>
    [System.Serializable]
    public class InnerEnergyRealmProgress
    {
        public InnerEnergyRealmLevel currentLevel;

        // 통계
        public int innerEnergyCardsUsed;        // 내공 카드 사용 횟수
        public int maxEnergySpentInCombat;      // 한 전투 최대 내공 소모
        public int totalEnergySpent;            // 누적 내공 소모
        public HashSet<string> innerEnergySkillsUsed; // 사용한 내공 비기 ID

        // 경지 돌파 완료 여부
        public Dictionary<InnerEnergyRealmLevel, bool> realmBreakthroughs;

        public InnerEnergyRealmProgress()
        {
            currentLevel = InnerEnergyRealmLevel.Hucheon;
            innerEnergyCardsUsed = 0;
            maxEnergySpentInCombat = 0;
            totalEnergySpent = 0;
            innerEnergySkillsUsed = new HashSet<string>();
            realmBreakthroughs = new Dictionary<InnerEnergyRealmLevel, bool>();

            // 초기 경지는 이미 돌파한 상태
            realmBreakthroughs[InnerEnergyRealmLevel.Hucheon] = true;
        }

        /// <summary>
        /// 경지 돌파 여부 확인
        /// </summary>
        public bool HasBrokenThrough(InnerEnergyRealmLevel level)
        {
            return realmBreakthroughs.ContainsKey(level) && realmBreakthroughs[level];
        }

        /// <summary>
        /// 경지 돌파 처리
        /// </summary>
        public void BreakThrough(InnerEnergyRealmLevel level)
        {
            if (!HasBrokenThrough(level))
            {
                realmBreakthroughs[level] = true;
                currentLevel = level;
                Debug.Log($"=== 내공 경지 돌파: {level} ===");
            }
        }

        /// <summary>
        /// 다음 경지 레벨을 반환
        /// </summary>
        public InnerEnergyRealmLevel? GetNextLevel()
        {
            switch (currentLevel)
            {
                case InnerEnergyRealmLevel.Hucheon:
                    return InnerEnergyRealmLevel.Seoncheon;
                case InnerEnergyRealmLevel.Seoncheon:
                    return InnerEnergyRealmLevel.Hwagyeong;
                case InnerEnergyRealmLevel.Hwagyeong:
                    return InnerEnergyRealmLevel.Jongsa;
                case InnerEnergyRealmLevel.Jongsa:
                    return InnerEnergyRealmLevel.CheoninHapil;
                case InnerEnergyRealmLevel.CheoninHapil:
                    return null; // 최고 경지
                default:
                    return null;
            }
        }

        /// <summary>
        /// 최고 경지인지 확인
        /// </summary>
        public bool IsMaxLevel()
        {
            return currentLevel == InnerEnergyRealmLevel.CheoninHapil;
        }
    }
}
