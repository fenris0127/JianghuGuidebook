using System.Collections.Generic;

namespace JianghuGuidebook.Save
{
    /// <summary>
    /// 현재 런의 진행 상태를 저장하는 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class RunData
    {
        // 기본 정보
        public string runId;                    // 런 고유 ID
        public long startTimestamp;             // 시작 시간 (Unix timestamp)
        public long lastSaveTimestamp;          // 마지막 저장 시간
        public int seed;                        // 맵 생성 시드

        // 플레이어 상태
        public int currentHealth;
        public int maxHealth;
        public int currentGold;

        // 맵 진행
        public int currentLayer;                // 현재 레이어
        public string currentNodeId;            // 현재 노드 ID
        public List<string> visitedNodeIds;     // 방문한 노드 ID 리스트
        public int regionsCompleted;            // 완료한 지역 수

        // 덱
        public List<string> deckCardIds;        // 덱의 카드 ID 리스트

        // 유물
        public List<string> relicIds;           // 보유 유물 ID 리스트

        // 통계
        public int enemiesKilled;
        public int bossesDefeated;
        public int combatsWon;
        public int cardsCollected;
        public int goldEarned;

        // 기타
        public int turnsPlayed;
        public int damageDealt;
        public int damageTaken;

        public RunData()
        {
            runId = System.Guid.NewGuid().ToString();
            startTimestamp = GetCurrentTimestamp();
            lastSaveTimestamp = startTimestamp;
            seed = UnityEngine.Random.Range(0, int.MaxValue);

            currentHealth = 70;
            maxHealth = 70;
            currentGold = 0;

            currentLayer = 0;
            currentNodeId = "";
            visitedNodeIds = new List<string>();
            regionsCompleted = 0;

            deckCardIds = new List<string>();
            relicIds = new List<string>();

            enemiesKilled = 0;
            bossesDefeated = 0;
            combatsWon = 0;
            cardsCollected = 0;
            goldEarned = 0;

            turnsPlayed = 0;
            damageDealt = 0;
            damageTaken = 0;
        }

        /// <summary>
        /// 현재 Unix 타임스탬프를 반환합니다
        /// </summary>
        private long GetCurrentTimestamp()
        {
            return System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 마지막 저장 시간을 업데이트합니다
        /// </summary>
        public void UpdateSaveTimestamp()
        {
            lastSaveTimestamp = GetCurrentTimestamp();
        }

        /// <summary>
        /// 플레이 시간을 초 단위로 반환합니다
        /// </summary>
        public long GetPlayTimeSeconds()
        {
            return lastSaveTimestamp - startTimestamp;
        }

        /// <summary>
        /// 플레이 시간을 포맷된 문자열로 반환합니다
        /// </summary>
        public string GetFormattedPlayTime()
        {
            long seconds = GetPlayTimeSeconds();
            int hours = (int)(seconds / 3600);
            int minutes = (int)((seconds % 3600) / 60);
            int secs = (int)(seconds % 60);

            if (hours > 0)
                return $"{hours}시간 {minutes}분";
            else if (minutes > 0)
                return $"{minutes}분 {secs}초";
            else
                return $"{secs}초";
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(runId))
            {
                UnityEngine.Debug.LogError("RunData: runId가 비어있습니다");
                return false;
            }

            if (currentHealth < 0 || maxHealth <= 0)
            {
                UnityEngine.Debug.LogError($"RunData: 유효하지 않은 체력 값 (current: {currentHealth}, max: {maxHealth})");
                return false;
            }

            if (currentHealth > maxHealth)
            {
                UnityEngine.Debug.LogWarning($"RunData: 현재 체력이 최대 체력보다 큽니다 (current: {currentHealth}, max: {maxHealth})");
                currentHealth = maxHealth;
            }

            if (currentGold < 0)
            {
                UnityEngine.Debug.LogError($"RunData: 골드가 음수입니다 ({currentGold})");
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"RunData: Layer {currentLayer}, HP {currentHealth}/{maxHealth}, Gold {currentGold}, Cards {deckCardIds.Count}, Relics {relicIds.Count}";
        }
    }
}
