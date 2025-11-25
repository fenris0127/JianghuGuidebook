using System.Collections.Generic;

namespace JianghuGuidebook.Data
{
    /// <summary>
    /// 분파(문파) 데이터
    /// 시작 덱, 유물, 스탯을 정의합니다
    /// </summary>
    [System.Serializable]
    public class FactionData
    {
        public string id;                           // 분파 ID (예: "faction_huashan")
        public string name;                         // 분파 이름 (예: "화산파")
        public string description;                  // 분파 설명
        public string specialty;                    // 특화 분야 (예: "검술")

        // 시작 스탯
        public int startingHealth;                  // 시작 체력
        public int startingGold;                    // 시작 골드
        public int startingMaxEnergy;               // 시작 최대 내공

        // 시작 덱
        public List<string> startingDeck;           // 시작 카드 ID 리스트

        // 시작 유물
        public List<string> startingRelics;         // 시작 유물 ID 리스트

        // UI
        public string iconPath;                     // 분파 아이콘 경로
        public string backgroundPath;               // 분파 배경 이미지 경로

        public FactionData()
        {
            id = "";
            name = "";
            description = "";
            specialty = "";
            startingHealth = 70;
            startingGold = 100;
            startingMaxEnergy = 3;
            startingDeck = new List<string>();
            startingRelics = new List<string>();
            iconPath = "";
            backgroundPath = "";
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(id))
            {
                UnityEngine.Debug.LogError("FactionData: id가 비어 있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                UnityEngine.Debug.LogError($"FactionData ({id}): name이 비어 있습니다");
                return false;
            }

            if (startingHealth <= 0)
            {
                UnityEngine.Debug.LogError($"FactionData ({id}): startingHealth가 0 이하입니다");
                return false;
            }

            if (startingMaxEnergy <= 0)
            {
                UnityEngine.Debug.LogError($"FactionData ({id}): startingMaxEnergy가 0 이하입니다");
                return false;
            }

            if (startingDeck == null || startingDeck.Count == 0)
            {
                UnityEngine.Debug.LogError($"FactionData ({id}): 시작 덱이 비어 있습니다");
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{name} ({specialty}) - 체력: {startingHealth}, 골드: {startingGold}, 내공: {startingMaxEnergy}, 덱: {startingDeck.Count}장";
        }
    }

    /// <summary>
    /// 분파 데이터베이스
    /// </summary>
    [System.Serializable]
    public class FactionDatabase
    {
        public FactionData[] factions;

        public FactionDatabase()
        {
            factions = new FactionData[0];
        }
    }
}
