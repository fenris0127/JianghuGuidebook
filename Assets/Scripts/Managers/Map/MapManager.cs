using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 맵의 생성과 노드 간의 상호작용을 관리하는 클래스입니다.
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public const int FINAL_FLOOR = 3;

    [Header("맵 생성 설정")]
    [SerializeField] private List<GameObject> nodePrefabs;
    [SerializeField] private GameObject bossNodePrefab;
    [SerializeField] private GameObject finalBossNodePrefab;
    [SerializeField] private LineRenderer pathPrefab;
    [SerializeField] private Transform mapContainer;

    [Header("맵 구조")]
    [SerializeField] private int mapLength = 10;
    [SerializeField] private int nodesPerLayer = 4;
    [SerializeField] private float layerSpacing = 3.5f;
    [SerializeField] private float nodeVerticalSpacing = 2.0f;
    [SerializeField] private int pathDensity = 2;

    private List<List<MapNode>> mapLayers = new List<List<MapNode>>();

    void Awake() 
    {
        Instance = this; 
    }

    public void GenerateMap(int floor)
    {
        foreach (Transform child in mapContainer) Destroy(child.gameObject);
        mapLayers.Clear();

        if (floor >= FINAL_FLOOR)
        {
            GameObject nodeObj = Instantiate(finalBossNodePrefab, Vector2.zero, Quaternion.identity, mapContainer);
            MapNode node = nodeObj.GetComponent<MapNode>();
            node.encounterData = ResourceManager.Instance.GetEncounterData("FinalBossEncounter");
            node.Setup(0);
            node.SetClickable(true);
            return;
        }

        // 노드 생성
        for (int i = 0; i < mapLength; i++)
        {
            List<MapNode> layerNodes = new List<MapNode>();
            int nodeCount = Random.Range(2, nodesPerLayer + 1);
            List<float> yPositions = Enumerable.Range(0, nodeCount).Select(n => (n - (nodeCount - 1) / 2f) * nodeVerticalSpacing).ToList();
            yPositions.Shuffle();

            for (int j = 0; j < nodeCount; j++)
            {
                GameObject prefab = (i == mapLength - 1) ? bossNodePrefab : nodePrefabs[Random.Range(0, nodePrefabs.Count)];
                GameObject nodeObj = Instantiate(prefab, new Vector3(i * layerSpacing, yPositions[j], 0), Quaternion.identity, mapContainer);
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
                int pathsToMake = Random.Range(1, pathDensity + 1);
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
        float roll = Random.value; // 0.0 ~ 1.0
        // 카드와 유사하게 확률 설정 (에픽/레전드리는 특수 조건이므로 일반 맵에서는 제외)
        if (roll <= 0.15f) return EventRarity.Rare;      // 15%
        if (roll <= 0.50f) return EventRarity.Uncommon;  // 35%
        return EventRarity.Common;                     // 50%
    }
}