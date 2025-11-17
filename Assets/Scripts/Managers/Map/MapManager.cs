using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GangHoBiGeup.Data;
using GangHoBiGeup.Core;

// 맵의 생성과 노드 간의 상호작용을 관리하는 클래스입니다.
// MapConfig를 통해 설정값을 가져옵니다.
// 모든 노드 프리팹은 Resources 폴더에서 자동으로 로드됩니다.
public class MapManager : Singleton<MapManager>
{
    public static int FINAL_FLOOR => Instance?.mapConfig?.finalFloor ?? 3;

    [Header("맵 생성 설정")]
    [SerializeField] private MapConfig mapConfig;
    [SerializeField] private Transform mapContainer;

    // 자동 로드되는 프리팹들
    private List<GameObject> nodePrefabs;
    private GameObject bossNodePrefab;
    private GameObject finalBossNodePrefab;
    private GameObject pathPrefab;

    private List<List<MapNode>> mapLayers = new List<List<MapNode>>();

    protected override void OnAwake()
    {
        // MapConfig가 없으면 Resources에서 로드 시도
        if (mapConfig == null)
        {
            mapConfig = Resources.Load<MapConfig>("Config/MapConfig");
            if (mapConfig == null)
            {
                Debug.LogWarning("MapConfig를 찾을 수 없습니다. 기본값을 사용합니다.");
            }
        }

        LoadPrefabs();
    }

    /// <summary>
    /// Resources 폴더에서 모든 맵 관련 프리팹을 자동으로 로드합니다.
    /// </summary>
    private void LoadPrefabs()
    {
        // 일반 노드 프리팹들 로드 (Resources/Prefabs/MapNodes/)
        GameObject[] loadedNodes = Resources.LoadAll<GameObject>("Prefabs/MapNodes");
        nodePrefabs = new List<GameObject>(loadedNodes);

        // 개별 프리팹 로드
        bossNodePrefab = Resources.Load<GameObject>("Prefabs/MapNodes/BossNode");
        finalBossNodePrefab = Resources.Load<GameObject>("Prefabs/MapNodes/FinalBossNode");
        pathPrefab = Resources.Load<GameObject>("Prefabs/MapNodes/Path");

        // 프리팹이 없을 경우 경고
        if (nodePrefabs.Count == 0)
        {
            Debug.LogWarning("MapNodes 프리팹을 찾을 수 없습니다. Resources/Prefabs/MapNodes/ 폴더를 확인하세요.");
        }
        if (bossNodePrefab == null)
        {
            Debug.LogWarning("BossNode 프리팹을 찾을 수 없습니다. Resources/Prefabs/MapNodes/BossNode.prefab을 확인하세요.");
        }
        if (finalBossNodePrefab == null)
        {
            Debug.LogWarning("FinalBossNode 프리팹을 찾을 수 없습니다. Resources/Prefabs/MapNodes/FinalBossNode.prefab을 확인하세요.");
        }
        if (pathPrefab == null)
        {
            Debug.LogWarning("Path 프리팹을 찾을 수 없습니다. Resources/Prefabs/MapNodes/Path.prefab을 확인하세요.");
        }
    }

    public void GenerateMap(int floor)
    {
        foreach (Transform child in mapContainer) Destroy(child.gameObject);
        mapLayers.Clear();

        int finalFloorNum = mapConfig?.finalFloor ?? 3;
        if (floor >= finalFloorNum)
        {
            // 최종 보스 노드 생성
            GameObject nodeObj;
            if (finalBossNodePrefab != null)
            {
                nodeObj = Instantiate(finalBossNodePrefab, Vector2.zero, Quaternion.identity, mapContainer);
            }
            else
            {
                Debug.LogWarning("FinalBossNode 프리팹이 없어 기본 노드를 생성합니다.");
                nodeObj = CreateDefaultMapNode(Vector2.zero, true);
            }

            MapNode node = nodeObj.GetComponent<MapNode>();
            if (node != null)
            {
                node.encounterData = ResourceManager.Instance.GetEncounterData("FinalBossEncounter");
                node.Setup(0);
                node.SetClickable(true);
            }
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
                GameObject nodeObj = CreateMapNode(i, new Vector3(i * spacing, yPositions[j], 0));
                if (nodeObj == null)
                {
                    Debug.LogError($"노드 생성 실패: Layer {i}, Index {j}");
                    continue;
                }

                MapNode node = nodeObj.GetComponent<MapNode>();
                if (node == null)
                {
                    Debug.LogError($"MapNode 컴포넌트를 찾을 수 없습니다: {nodeObj.name}");
                    continue;
                }

                node.Setup(i);
                layerNodes.Add(node);

                // 노드 타입에 따라 데이터 할당
                if (node.nodeType == NodeType.Combat)
                {
                    var encounters = ResourceManager.Instance.GetAllEncounters().Where(e => !e.isBossEncounter).ToList();
                    if (encounters.Count > 0)
                        node.encounterData = encounters.OrderBy(e => Random.value).First();
                }
                else if (node.nodeType == NodeType.Event)
                {
                    var events = ResourceManager.Instance.GetAllEvents();
                    if (events.Count > 0)
                        node.eventData = events.OrderBy(e => Random.value).First();
                }
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

    /// <summary>
    /// 맵 노드를 생성합니다. 프리팹이 없으면 런타임에 동적 생성합니다.
    /// </summary>
    private GameObject CreateMapNode(int layerIndex, Vector3 position)
    {
        GameObject prefab = null;
        int totalLayers = mapConfig?.mapLength ?? 10;

        // 마지막 레이어는 보스 노드
        if (layerIndex == totalLayers - 1)
        {
            prefab = bossNodePrefab;
        }
        else if (nodePrefabs != null && nodePrefabs.Count > 0)
        {
            // 일반 노드 프리팹에서 랜덤 선택
            prefab = nodePrefabs[Random.Range(0, nodePrefabs.Count)];
        }

        // 프리팹이 있으면 인스턴스화
        if (prefab != null)
        {
            return Instantiate(prefab, position, Quaternion.identity, mapContainer);
        }

        // 프리팹이 없으면 런타임에 기본 노드 생성
        Debug.LogWarning($"노드 프리팹이 없어 기본 노드를 생성합니다. Layer: {layerIndex}");
        return CreateDefaultMapNode(position, layerIndex == totalLayers - 1);
    }

    /// <summary>
    /// 프리팹이 없을 때 기본 노드를 런타임에 생성합니다.
    /// </summary>
    private GameObject CreateDefaultMapNode(Vector3 position, bool isBoss)
    {
        GameObject nodeObj = new GameObject(isBoss ? "BossNode" : "MapNode");
        nodeObj.transform.SetParent(mapContainer);
        nodeObj.transform.position = position;

        // MapNode 컴포넌트 추가
        MapNode mapNode = nodeObj.AddComponent<MapNode>();

        // 기본 비주얼 추가 (Sprite Renderer)
        SpriteRenderer sprite = nodeObj.AddComponent<SpriteRenderer>();
        sprite.color = isBoss ? Color.red : Color.white;
        // TODO: 스프라이트가 있다면 할당

        // Collider 추가 (클릭 감지용)
        CircleCollider2D collider = nodeObj.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;

        return nodeObj;
    }

    private void DrawPath(Vector3 from, Vector3 to)
    {
        if (pathPrefab == null)
        {
            // Path 프리팹이 없으면 기본 LineRenderer 생성
            GameObject pathObj = new GameObject("Path");
            pathObj.transform.SetParent(mapContainer);
            LineRenderer line = pathObj.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.SetPositions(new Vector3[] { from, to });
        }
        else
        {
            GameObject pathObj = Instantiate(pathPrefab, mapContainer);
            LineRenderer line = pathObj.GetComponent<LineRenderer>();
            if (line != null)
            {
                line.SetPositions(new Vector3[] { from, to });
            }
        }
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