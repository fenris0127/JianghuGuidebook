using System.Collections.Generic;

namespace JianghuGuidebook.Save
{
    /// <summary>
    /// 메타 진행 세이브 데이터
    /// 무공 정수와 영구 업그레이드 정보를 저장합니다
    /// </summary>
    [System.Serializable]
    public class MetaSaveData
    {
        // 무공 정수
        public int totalEssence;
        public int currentEssence;

        // 업그레이드 해금 상태
        public List<UpgradeUnlockData> unlockedUpgrades;

        // Phase 3 추가 해금 데이터
        public List<string> unlockedFactions;
        public List<int> unlockedDifficulties;
        public List<string> unlockedAchievements;
        public List<string> unlockedCodexEntries;

        // 통계
        public int totalRunsCompleted;
        public int totalVictories;
        public int totalDeaths;
        public int totalEnemiesKilled;
        public int totalBossesDefeated;
        public float totalPlayTime; // 총 플레이 시간 (초 단위)

        public MetaSaveData()
        {
            totalEssence = 0;
            currentEssence = 0;
            unlockedUpgrades = new List<UpgradeUnlockData>();
            
            unlockedFactions = new List<string>();
            unlockedDifficulties = new List<int>();
            unlockedAchievements = new List<string>();
            unlockedCodexEntries = new List<string>();

            totalRunsCompleted = 0;
            totalVictories = 0;
            totalDeaths = 0;
            totalEnemiesKilled = 0;
            totalEnemiesKilled = 0;
            totalBossesDefeated = 0;
            totalPlayTime = 0f;
        }

        /// <summary>
        /// 업그레이드 해금 정보 추가
        /// </summary>
        public void AddUpgradeUnlock(string upgradeId, int timesPurchased)
        {
            var existing = unlockedUpgrades.Find(u => u.upgradeId == upgradeId);
            if (existing != null)
            {
                existing.timesPurchased = timesPurchased;
            }
            else
            {
                unlockedUpgrades.Add(new UpgradeUnlockData
                {
                    upgradeId = upgradeId,
                    timesPurchased = timesPurchased
                });
            }
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (currentEssence < 0 || totalEssence < 0)
            {
                UnityEngine.Debug.LogError("MetaSaveData: 무공 정수가 음수입니다");
                return false;
            }

            if (currentEssence > totalEssence)
            {
                UnityEngine.Debug.LogError("MetaSaveData: 현재 정수가 누적 정수보다 많습니다");
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"MetaSaveData: 정수 {currentEssence}/{totalEssence}, 업그레이드 {unlockedUpgrades.Count}개, 런 {totalRunsCompleted}회";
        }
    }

    /// <summary>
    /// 업그레이드 해금 데이터
    /// </summary>
    [System.Serializable]
    public class UpgradeUnlockData
    {
        public string upgradeId;
        public int timesPurchased;

        public UpgradeUnlockData()
        {
            upgradeId = "";
            timesPurchased = 0;
        }
    }
}
