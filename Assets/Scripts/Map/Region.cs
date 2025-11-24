using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 지역 타입
    /// </summary>
    public enum RegionType
    {
        Gangnam,    // 강남 (1지역)
        Jungwon,    // 중원 (2지역)
        Seoyu,      // 서역 (3지역)
        Bukhae,     // 북해 (4지역)
        Cheonha     // 천하 (5지역)
    }

    /// <summary>
    /// 지역 데이터 클래스
    /// 각 지역의 정보와 설정을 담습니다
    /// </summary>
    [System.Serializable]
    public class Region
    {
        public string id;                   // 지역 고유 ID (예: "region_gangnam")
        public string name;                 // 지역 이름 (예: "강남")
        public RegionType regionType;       // 지역 타입
        public string description;          // 지역 설명
        public string theme;                // 지역 테마 (예: "강호의 낭만")

        [Header("맵 설정")]
        public int layerCount;              // 레이어 수 (12-15)
        public int minNodesPerLayer;        // 레이어당 최소 노드 수
        public int maxNodesPerLayer;        // 레이어당 최대 노드 수

        [Header("보스")]
        public string bossId;               // 보스 ID

        [Header("난이도 스케일링")]
        public float difficultyMultiplier;  // 난이도 배율 (1.0 = 기본)
        public int enemyHealthBonus;        // 적 체력 보너스
        public int enemyDamageBonus;        // 적 공격력 보너스

        [Header("이벤트")]
        public List<string> eventPool;      // 이 지역에서 발생 가능한 이벤트 ID 리스트

        [Header("적 풀")]
        public List<string> enemyPool;      // 이 지역에서 등장하는 적 ID 리스트
        public List<string> eliteEnemyPool; // 이 지역의 정예 적 ID 리스트

        public Region()
        {
            eventPool = new List<string>();
            enemyPool = new List<string>();
            eliteEnemyPool = new List<string>();
        }

        /// <summary>
        /// 지역 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("Region: ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"Region {id}: 이름이 비어있습니다");
                return false;
            }

            if (layerCount < 10 || layerCount > 20)
            {
                Debug.LogError($"Region {id}: layerCount는 10-20 사이여야 합니다 (현재: {layerCount})");
                return false;
            }

            if (string.IsNullOrEmpty(bossId))
            {
                Debug.LogError($"Region {id}: bossId가 비어있습니다");
                return false;
            }

            if (enemyPool.Count == 0)
            {
                Debug.LogError($"Region {id}: enemyPool이 비어있습니다");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 현재 지역의 난이도를 계산합니다
        /// </summary>
        public float GetEffectiveDifficulty(float baseDifficulty)
        {
            return baseDifficulty * difficultyMultiplier;
        }

        public override string ToString()
        {
            return $"Region: {name} ({id}), Type: {regionType}, Layers: {layerCount}, Boss: {bossId}";
        }
    }
}
