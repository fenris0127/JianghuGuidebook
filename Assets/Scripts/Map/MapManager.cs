using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 맵 상태 및 진행을 관리하는 매니저
    /// </summary>
    public class MapManager : MonoBehaviour
    {
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

        // Properties
        public List<MapNode> AllNodes => allNodes;
        public MapNode CurrentNode => currentNode;
        public int CurrentSeed => currentSeed;

        // Events
        public System.Action<MapNode> OnNodeEntered;
        public System.Action<MapNode> OnNodeCompleted;
        public System.Action<List<MapNode>> OnMapGenerated;

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

        /// <summary>
        /// 새 맵을 생성합니다
        /// </summary>
        public void GenerateNewMap(int seed = -1)
        {
            Debug.Log("=== MapManager: 새 맵 생성 ===");

            MapGenerator generator = new MapGenerator();
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

            return true;
        }

        /// <summary>
        /// 현재 노드를 완료 처리합니다
        /// </summary>
        public void CompleteCurrentNode()
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
                // TODO: 지역 클리어 처리
            }
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
    }
}
