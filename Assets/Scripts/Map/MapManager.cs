using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Save;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 맵 상태 및 진행을 관리하는 매니저
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        // 씬 이름 상수
        private const string MAP_SCENE_NAME = "MapScene";

        private static MapManager _instance;

        public static MapManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MapManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("MapManager");
                        _instance = go.AddComponent<MapManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("맵 데이터")]
        [SerializeField] private List<MapNode> allNodes = new List<MapNode>();
        [SerializeField] private MapNode currentNode;
        [SerializeField] private int currentSeed;

        [Header("자동 저장 설정")]
        [SerializeField] private bool enableAutoSave = true;
        [SerializeField] private int autoSaveSlot = 0;

        [Header("첫 노드 자동 시작")]
        [SerializeField] private bool autoStartFirstNode = true;
        [SerializeField] private float autoStartDelay = 0.5f; // 씬 로드 후 대기 시간

        // Properties
        public List<MapNode> AllNodes => allNodes;
        public MapNode CurrentNode => currentNode;
        public int CurrentSeed => currentSeed;

        // Events
        public System.Action<MapNode> OnNodeEntered;
        public System.Action<MapNode> OnNodeCompleted;
        public System.Action<List<MapNode>> OnMapGenerated;
        public System.Action<MapNode> OnBossNodeReached;  // 보스 노드 도달 시

        private bool hasAutoStarted = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // 맵 씬에서만 동작
            if (SceneManager.GetActiveScene().name == MAP_SCENE_NAME)
            {
                CheckAndAutoStartFirstNode();
            }
        }

        /// <summary>
        /// 첫 노드 자동 시작 확인 및 실행
        /// </summary>
        private void CheckAndAutoStartFirstNode()
        {
            if (!autoStartFirstNode || hasAutoStarted)
            {
                return;
            }

            // 현재 노드가 시작 노드이고 아직 방문하지 않았다면 자동 진입
            if (currentNode != null &&
                currentNode.nodeType == NodeType.Start &&
                !currentNode.isVisited)
            {
                Debug.Log($"[MapManager] 첫 노드 자동 시작: {currentNode.nodeType}");
                hasAutoStarted = true;

                // 약간의 딜레이 후 진입 (UI 준비 시간)
                Invoke(nameof(AutoEnterFirstNode), autoStartDelay);
            }
        }

        /// <summary>
        /// 첫 노드에 자동으로 진입합니다
        /// </summary>
        private void AutoEnterFirstNode()
        {
            if (currentNode != null && !currentNode.isVisited)
            {
                // 시작 노드는 즉시 완료 처리하고 다음 레이어로 이동
                CompleteNode(currentNode);
                Debug.Log("[MapManager] 첫 노드(Start) 자동 완료");
            }
        }

        /// <summary>
        /// 새 맵을 생성합니다
        /// </summary>
        public void GenerateNewMap(int seed = -1)
        {
            Debug.Log("=== MapManager: 새 맵 생성 ===");

            MapGenerator generator = new MapGenerator();
            // 현재 지역 정보로 맵 설정
            Region currentRegion = RegionManager.Instance.CurrentRegion;
            if (currentRegion != null)
            {
                Debug.Log($"지역 설정 적용: {currentRegion.name} (Layers: {currentRegion.layerCount})");
                generator.ConfigureMap(
                    currentRegion.layerCount,
                    currentRegion.minNodesPerLayer,
                    currentRegion.maxNodesPerLayer
                );
            }
            else
            {
                Debug.LogWarning("현재 지역 정보가 없습니다. 기본 설정 사용.");
            }

            allNodes = generator.GenerateMap(seed);
            currentSeed = seed;

            // 시작 노드를 현재 노드로 설정
            currentNode = allNodes.FirstOrDefault(n => n.nodeType == NodeType.Start);
            if (currentNode != null)
            {
                currentNode.isCurrentNode = true;
                UpdateAccessibleNodes();
            }

            OnMapGenerated?.Invoke(allNodes);

            Debug.Log($"맵 생성 완료: {allNodes.Count}개 노드");
        }

        /// <summary>
        /// 노드로 이동합니다
        /// </summary>
        public bool MoveToNode(MapNode targetNode)
        {
            if (targetNode == null)
            {
                Debug.LogError("대상 노드가 null입니다");
                return false;
            }

            if (!targetNode.isAccessible)
            {
                Debug.LogWarning($"노드 {targetNode.id}는 접근할 수 없습니다");
                return false;
            }

            // 현재 노드 업데이트
            if (currentNode != null)
            {
                currentNode.isCurrentNode = false;
            }

            currentNode = targetNode;
            currentNode.Visit();

            Debug.Log($"노드 이동: {currentNode}");

            // 접근 가능한 노드 업데이트
            UpdateAccessibleNodes();

            // 이벤트 발생
            OnNodeEntered?.Invoke(currentNode);

            // 보스 노드 도달 시 강제 진입
            if (currentNode.nodeType == NodeType.Boss)
            {
                Debug.Log("=== 보스 노드 도달! 자동으로 전투를 시작합니다 ===");
                OnBossNodeReached?.Invoke(currentNode);

                // 보스 전투 씬 자동 로드
                LoadNodeScene(currentNode);
            }

            // 노드 이동 후 자동 저장
            TriggerAutoSave();

            return true;
        }

        /// <summary>
        /// 현재 노드를 완료 처리합니다
        /// </summary>
        /// <param name="returnToMap">완료 후 맵으로 자동 복귀할지 여부 (기본값: false)</param>
        public void CompleteCurrentNode(bool returnToMap = false)
        {
            if (currentNode == null)
            {
                Debug.LogWarning("완료할 현재 노드가 없습니다");
                return;
            }

            Debug.Log($"노드 완료: {currentNode}");
            OnNodeCompleted?.Invoke(currentNode);

            // 보스 노드 완료 시 지역 클리어
            if (currentNode.nodeType == NodeType.Boss)
            {
                Debug.Log("=== 지역 클리어! ===");
                RegionManager.Instance.CompleteCurrentRegion();
            }

            // 노드 완료 후 자동 저장
            TriggerAutoSave();

            // 맵으로 복귀
            if (returnToMap)
            {
                ReturnToMap();
            }
        }

        /// <summary>
        /// 노드에 진입하고 해당 씬을 로드합니다
        /// </summary>
        public void EnterNode(MapNode targetNode)
        {
            if (MoveToNode(targetNode))
            {
                // 보스 노드는 MoveToNode에서 자동으로 씬을 로드하므로 제외
                if (targetNode.nodeType != NodeType.Boss)
                {
                    LoadNodeScene(targetNode);
                }
            }
        }

        /// <summary>
        /// 노드 타입에 따라 해당 씬을 로드합니다
        /// </summary>
        public void LoadNodeScene(MapNode node)
        {
            if (node == null)
            {
                Debug.LogError("씬을 로드할 노드가 null입니다");
                return;
            }

            string sceneName = GetSceneNameForNodeType(node.nodeType);

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning($"노드 타입 {node.nodeType}에 대한 씬이 정의되지 않았습니다");
                return;
            }

            Debug.Log($"씬 로드: {sceneName} (노드 타입: {node.nodeType})");
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 노드 타입에 따른 씬 이름을 반환합니다
        /// </summary>
        private string GetSceneNameForNodeType(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Combat:
                case NodeType.EliteCombat:
                    return "CombatScene";

                case NodeType.Shop:
                    return "ShopScene";

                case NodeType.Rest:
                    return "RestScene";

                case NodeType.Event:
                    return "EventScene";

                case NodeType.Boss:
                    return "BossCombatScene";

                case NodeType.Treasure:
                    return "TreasureScene";

                case NodeType.Start:
                    // 시작 노드는 씬 전환이 필요 없음
                    return null;

                default:
                    Debug.LogWarning($"알 수 없는 노드 타입: {nodeType}");
                    return null;
            }
        }

        /// <summary>
        /// 맵 씬으로 복귀합니다
        /// </summary>
        public void ReturnToMap()
        {
            Debug.Log("맵으로 복귀합니다");
            SceneManager.LoadScene(MAP_SCENE_NAME);
        }

        /// <summary>
        /// 접근 가능한 노드를 업데이트합니다
        /// </summary>
        private void UpdateAccessibleNodes()
        {
            // 모든 노드를 접근 불가능으로 초기화
            foreach (var node in allNodes)
            {
                node.isAccessible = false;
            }

            // 현재 노드에서 연결된 다음 노드들을 접근 가능하게 설정
            if (currentNode != null)
            {
                foreach (int nextNodeId in currentNode.connectedNodeIds)
                {
                    MapNode nextNode = allNodes.FirstOrDefault(n => n.id == nextNodeId);
                    if (nextNode != null && !nextNode.isVisited)
                    {
                        nextNode.isAccessible = true;
                    }
                }
            }

            int accessibleCount = allNodes.Count(n => n.isAccessible);
            Debug.Log($"접근 가능한 노드: {accessibleCount}개");
        }

        /// <summary>
        /// 특정 레이어의 노드들을 가져옵니다
        /// </summary>
        public List<MapNode> GetNodesAtLayer(int layer)
        {
            return allNodes.Where(n => n.layer == layer).ToList();
        }

        /// <summary>
        /// ID로 노드를 찾습니다
        /// </summary>
        public MapNode GetNodeById(int id)
        {
            return allNodes.FirstOrDefault(n => n.id == id);
        }

        /// <summary>
        /// 특정 타입의 노드들을 가져옵니다
        /// </summary>
        public List<MapNode> GetNodesByType(NodeType type)
        {
            return allNodes.Where(n => n.nodeType == type).ToList();
        }

        /// <summary>
        /// 맵 진행률을 계산합니다
        /// </summary>
        public float GetProgress()
        {
            if (allNodes.Count == 0) return 0f;

            int visitedCount = allNodes.Count(n => n.isVisited);
            return (float)visitedCount / allNodes.Count;
        }

        /// <summary>
        /// 맵을 리셋합니다
        /// </summary>
        public void ResetMap()
        {
            allNodes.Clear();
            currentNode = null;
            currentSeed = 0;
            Debug.Log("맵 리셋 완료");
        }

        // ========== 자동 저장 관련 메서드 ==========

        /// <summary>
        /// 현재 맵 상태를 RunData에 업데이트합니다
        /// </summary>
        public void UpdateRunDataMapState()
        {
            if (SaveManager.Instance == null || SaveManager.Instance.CurrentSaveData == null)
            {
                Debug.LogWarning("SaveManager 또는 SaveData가 없어 맵 상태를 저장할 수 없습니다");
                return;
            }

            RunData runData = SaveManager.Instance.CurrentSaveData.currentRun;

            if (runData == null)
            {
                Debug.LogWarning("현재 진행 중인 런이 없습니다");
                return;
            }

            // 맵 진행 상태 업데이트
            runData.seed = currentSeed;

            if (currentNode != null)
            {
                runData.currentLayer = currentNode.layer;
                runData.currentNodeId = currentNode.id.ToString();
            }

            // 방문한 노드 ID 리스트 업데이트
            runData.visitedNodeIds.Clear();
            foreach (var node in allNodes)
            {
                if (node.isVisited)
                {
                    runData.visitedNodeIds.Add(node.id.ToString());
                }
            }

            Debug.Log($"맵 상태 업데이트: Layer {runData.currentLayer}, 방문한 노드: {runData.visitedNodeIds.Count}개");
        }

        /// <summary>
        /// 자동 저장을 실행합니다
        /// </summary>
        private void TriggerAutoSave()
        {
            if (!enableAutoSave)
            {
                Debug.Log("자동 저장이 비활성화되어 있습니다");
                return;
            }

            if (SaveManager.Instance == null)
            {
                Debug.LogWarning("SaveManager가 없어 자동 저장을 할 수 없습니다");
                return;
            }

            // 맵 상태를 RunData에 업데이트
            UpdateRunDataMapState();

            // 자동 저장 실행
            SaveManager.Instance.AutoSave(autoSaveSlot);
        }

        /// <summary>
        /// RunData로부터 맵 상태를 복원합니다
        /// </summary>
        public void RestoreMapStateFromRunData(RunData runData)
        {
            if (runData == null)
            {
                Debug.LogWarning("복원할 RunData가 없습니다");
                return;
            }

            // 맵 생성 (저장된 시드 사용)
            GenerateNewMap(runData.seed);

            // 방문한 노드 복원
            if (runData.visitedNodeIds != null)
            {
                foreach (string nodeIdStr in runData.visitedNodeIds)
                {
                    if (int.TryParse(nodeIdStr, out int nodeId))
                    {
                        MapNode node = GetNodeById(nodeId);
                        if (node != null)
                        {
                            node.isVisited = true;
                        }
                    }
                }
            }

            // 현재 노드 복원
            if (!string.IsNullOrEmpty(runData.currentNodeId))
            {
                if (int.TryParse(runData.currentNodeId, out int currentNodeId))
                {
                    MapNode node = GetNodeById(currentNodeId);
                    if (node != null)
                    {
                        currentNode = node;
                        currentNode.isCurrentNode = true;
                        UpdateAccessibleNodes();
                    }
                }
            }

            Debug.Log($"맵 상태 복원 완료: Seed {runData.seed}, Layer {runData.currentLayer}");
        }
    }
}
