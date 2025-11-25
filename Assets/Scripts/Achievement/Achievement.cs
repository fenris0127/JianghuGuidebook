using System;

namespace JianghuGuidebook.Achievement
{
    public enum AchievementType
    {
        Combat,     // 전투 관련
        Collection, // 수집 관련
        Realm,      // 경지 관련
        Special     // 특수/기타
    }

    public enum AchievementRewardType
    {
        None,
        Essence,    // 무공 정수
        UnlockRelic,// 유물 해금
        UnlockCard  // 카드 해금
    }

    [Serializable]
    public class Achievement
    {
        public string id;               // 고유 ID
        public string name;             // 업적 이름
        public string description;      // 업적 설명
        public AchievementType type;    // 업적 타입
        public bool isHidden;           // 숨겨진 업적 여부
        
        // 상태 (저장됨)
        public bool isUnlocked;         // 해금 여부
        public string unlockedDate;     // 해금 날짜 (yyyy-MM-dd HH:mm:ss)
        public int currentProgress;     // 현재 진행도
        public int currentProgress;     // 현재 진행도
        public int targetProgress;      // 목표 진행도 (0이면 단순 해금)
        
        // 보상
        public AchievementRewardType rewardType;
        public string rewardValue;

        public Achievement(string id, string name, string description, AchievementType type, bool isHidden = false)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.type = type;
            this.isHidden = isHidden;
            this.isUnlocked = false;
            this.unlockedDate = "";
        }

        public void Unlock()
        {
            if (!isUnlocked)
            {
                isUnlocked = true;
                unlockedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }

    [Serializable]
    public class AchievementDatabase
    {
        public Achievement[] achievements;
    }
}
