using UnityEngine;

namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 맵 생성 및 노드 배치 설정을 관리하는 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "MapConfig", menuName = "GangHoBiGeup/Config/Map Config")]
    public class MapConfig : ScriptableObject
    {
        [Header("=== 맵 구조 설정 ===")]
        [Tooltip("각 층의 맵 길이 (노드 레이어 수)")]
        public int mapLength = 10;

        [Tooltip("각 레이어당 최대 노드 수")]
        public int maxNodesPerLayer = 4;

        [Tooltip("각 레이어당 최소 노드 수")]
        public int minNodesPerLayer = 2;

        [Tooltip("레이어 간 가로 간격")]
        public float layerSpacing = 3.5f;

        [Tooltip("노드 간 세로 간격")]
        public float nodeVerticalSpacing = 2.0f;

        [Tooltip("각 노드에서 연결할 최대 경로 수")]
        public int maxPathDensity = 2;

        [Tooltip("최종 보스 층 번호")]
        public int finalFloor = 3;

        [Header("=== 이벤트 희귀도 확률 ===")]
        [Tooltip("희귀 이벤트 확률 (0.0 ~ 1.0)")]
        [Range(0f, 1f)]
        public float rareEventChance = 0.15f; // 15%

        [Tooltip("고급 이벤트 확률 (0.0 ~ 1.0)")]
        [Range(0f, 1f)]
        public float uncommonEventChance = 0.50f; // 35% (0.15 ~ 0.50)

        // 나머지는 일반 이벤트 (Common) - 50%

        /// <summary>
        /// 이벤트 희귀도를 랜덤으로 결정합니다.
        /// </summary>
        public EventRarity GetRandomEventRarity()
        {
            float roll = Random.value;

            if (roll <= rareEventChance)
                return EventRarity.Rare;
            if (roll <= uncommonEventChance)
                return EventRarity.Uncommon;

            return EventRarity.Common;
        }
    }
}
