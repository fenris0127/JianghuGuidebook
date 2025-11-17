using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GangHoBiGeup.Data;

// 맵의 생성과 노드 간의 상호작용을 관리하는 클래스입니다.
// MapConfig를 통해 설정값을 가져옵니다.
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public static int FINAL_FLOOR => Instance?.mapConfig?.finalFloor ?? 3;

    [Header("맵 생성 설정")]
    [SerializeField] private MapConfig mapConfig;
    [SerializeField] private List<GameObject> nodePrefabs;
    [SerializeField] private GameObject bossNodePrefab;
    [SerializeField] private GameObject finalBossNodePrefab;
    [SerializeField] private LineRenderer pathPrefab;
    [SerializeField] private Transform mapContainer;

    private List<List<MapNode>> mapLayers = new List<List<MapNode>>();

    void Awake()
    {
        Instance = this;

        // MapConfig가 없으면 Resources에서 로드 시도
        if (mapConfig == null)
        {
            mapConfig = Resources.Load<MapConfig>("Config/MapConfig");
            if (mapConfig == null)
            {
                Debug.LogWarning("MapConfig를 찾을 수 없습니다. 기본값을 사용합니다.");
            }
        }
    }

    public void GenerateMap(int floor)
    {
        foreach (Transform child in mapContainer) Destroy(child.gameObject);
        mapLayers.Clear();

        int finalFloorNum = mapConfig?.finalFloor ?? 3;
        if (floor >= finalFloorNum)
        {
            GameObject nodeObj = Instantiate(finalBossNodePrefab, Vector2.zero, Quaternion.identity, mapContainer);
            MapNode node = nodeObj.GetComponent<MapNode>();
            node.encounterData = ResourceManager.Instance.GetEncounterData("FinalBossEncounter");
            node.Setup(0);
            node.SetClickable(true);
            return;
        }

        int length = mapConfig?.mapLength ?? 10;
        int minNodes = mapConfig?.minNodesPerLayer ?? 2;
        int maxNodes = mapConfig?.maxNodesPerLayer ?? 4;
        float spacing = mapConfig?.layerSpacing ?? 3.5f;
        float verticalSpacing = mapConfig?.nodeVerticalSpacing ?? 2.0f;
        int maxPaths = mapConfig?.maxPathDensity ?? 2;

        // 노드 생성
        for (int i = 0; i < length; i++)
        {
            List<MapNode> layerNodes = new List<MapNode>();
            int nodeCount = Random.Range(minNodes, maxNodes + 1);
            List<float> yPositions = Enumerable.Range(0, nodeCount).Select(n => (n - (nodeCount - 1) / 2f) * verticalSpacing).ToList();
            yPositions.Shuffle();

            for (int j = 0; j < nodeCount; j++)
            {
                GameObject prefab = (i == length - 1) ? bossNodePrefab : nodePrefabs[Random.Range(0, nodePrefabs.Count)];
                GameObject nodeObj = Instantiate(prefab, new Vector3(i * spacing, yPositions[j], 0), Quaternion.identity, mapContainer);
                MapNode node = nodeObj.GetComponent<MapNode>();
                node.Setup(i);
                layerNodes.Add(node);

                if (node.nodeType == NodeType.Combat)
                    node.encounterData = ResourceManager.Instance.GetAllEncounters().Where(e => !e.isBossEncounter).ToList().OrderBy(e => Random.value).First();
                else if (node.nodeType == NodeType.Event)
                    node.eventData = ResourceManager.Instance.GetAllEvents().OrderBy(e => Random.value).First();
            }

            mapLayers.Add(layerNodes);
        }

        // 경로 생성
        for (int i = 0; i < mapLayers.Count - 1; i++)
        {
            foreach (MapNode fromNode in mapLayers[i])
            {
                int pathsToMake = Random.Range(1, maxPaths + 1);
                var potentialTargets = mapLayers[i + 1].OrderBy(n => Vector3.Distance(fromNode.transform.position, n.transform.position)).Take(pathsToMake);
                foreach (MapNode toNode in potentialTargets)
                {
                    fromNode.connectedNodes.Add(toNode);
                    DrawPath(fromNode.transform.position, toNode.transform.position);
                }
            }
        }

        // 첫 층 활성화
        foreach (var node in mapLayers[0])
            node.SetClickable(true);
    }

    private void DrawPath(Vector3 from, Vector3 to)
    {
        LineRenderer line = Instantiate(pathPrefab, mapContainer);
        line.SetPositions(new Vector3[] { from, to });
    }

    public void OnNodeSelected(MapNode selectedNode)
    {
        // 현재 층의 모든 노드 비활성화
        foreach (var node in mapLayers[selectedNode.layer])
            node.SetClickable(false);

        // 연결된 다음 층 노드들만 활성화
        foreach (var connectedNode in selectedNode.connectedNodes)
            connectedNode.SetClickable(true);
    }

    private void SpawnNode(GameObject prefab, Vector2 position)
    {
        GameObject nodeObj = Instantiate(prefab, position, Quaternion.identity, mapContainer);
        MapNode mapNode = nodeObj.GetComponent<MapNode>();
        if (mapNode != null && mapNode.nodeType == NodeType.Event)
        {
            // 1. 모든 기연 이벤트 목록을 가져옵니다.
            List<EventData> allEvents = ResourceManager.Instance.GetAllEvents();

            // 2. 확률에 따라 희귀도를 결정합니다.
            EventRarity selectedRarity = GetRandomEventRarity();

            // 3. 해당 희귀도를 가진 이벤트들 중에서 무작위로 하나를 선택합니다.
            var candidates = allEvents.Where(e => e.rarity == selectedRarity).ToList();
            if (candidates.Count > 0)
            {
                mapNode.eventData = candidates[Random.Range(0, candidates.Count)];
            }
            else // 만약 해당 희귀도 이벤트가 없다면, 일반(Common) 이벤트 중에서 선택
            {
                var commonEvents = allEvents.Where(e => e.rarity == EventRarity.Common).ToList();
                if (commonEvents.Count > 0)
                    mapNode.eventData = commonEvents[Random.Range(0, commonEvents.Count)];
            }
        }
    }

    private EventRarity GetRandomEventRarity()
    {
        if (mapConfig != null)
            return mapConfig.GetRandomEventRarity();

        // Fallback: MapConfig가 없을 때 기본값
        float roll = Random.value;
        if (roll <= 0.15f) return EventRarity.Rare;
        if (roll <= 0.50f) return EventRarity.Uncommon;
        return EventRarity.Common;
    }
}