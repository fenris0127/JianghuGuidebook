using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 절차적 맵 생성 시스템
    /// 레이어 기반으로 노드를 생성하고 연결합니다
    /// </summary>
    public class MapGenerator
    {
        // 맵 설정
        private int totalLayers = 15;
        private int minNodesPerLayer = 3;
        private int maxNodesPerLayer = 5;
        private int minConnectionsPerNode = 1;
        private int maxConnectionsPerNode = 3;

        // 노드 타입 분포 가중치
        private Dictionary<NodeType, int> nodeTypeWeights = new Dictionary<NodeType, int>()
        {
            { NodeType.Combat, 50 },
            { NodeType.EliteCombat, 10 },
            { NodeType.Shop, 10 },
            { NodeType.Rest, 10 },
            { NodeType.Event, 15 },
            { NodeType.Treasure, 5 }
        };

        private System.Random random;
        private int currentNodeId = 0;

        /// <summary>
        /// 맵을 생성합니다
        /// </summary>
        public List<MapNode> GenerateMap(int seed = -1)
        {
            // 시드 설정
            if (seed == -1)
            {
                seed = Random.Range(0, 100000);
            }
            random = new System.Random(seed);

            Debug.Log($"=== 맵 생성 시작 (시드: {seed}) ===");

            List<MapNode> allNodes = new List<MapNode>();
            List<MapNode> previousLayer = new List<MapNode>();

            // 레이어별로 노드 생성
            for (int layer = 0; layer <= totalLayers; layer++)
            {
                List<MapNode> currentLayer = GenerateLayer(layer, previousLayer);
                allNodes.AddRange(currentLayer);

                // 이전 레이어와 현재 레이어 연결
                if (previousLayer.Count > 0)
                {
                    ConnectLayers(previousLayer, currentLayer);
                }

                previousLayer = currentLayer;
            }

            // 경로 검증
            ValidateMap(allNodes);

            Debug.Log($"=== 맵 생성 완료: 총 {allNodes.Count}개 노드, {totalLayers + 1}개 레이어 ===");

            return allNodes;
        }

        /// <summary>
        /// 특정 레이어의 노드들을 생성합니다
        /// </summary>
        private List<MapNode> GenerateLayer(int layer, List<MapNode> previousLayer)
        {
            List<MapNode> layerNodes = new List<MapNode>();

            // 레이어별 노드 개수 결정
            int nodeCount;

            if (layer == 0)
            {
                // 시작 노드 1개
                nodeCount = 1;
            }
            else if (layer == totalLayers)
            {
                // 보스 노드 1개
                nodeCount = 1;
            }
            else if (layer <= 3)
            {
                // 초반: 점점 증가
                nodeCount = random.Next(2, 4);
            }
            else if (layer >= totalLayers - 2)
            {
                // 후반: 점점 감소
                nodeCount = random.Next(2, 4);
            }
            else
            {
                // 중반: 최대 폭
                nodeCount = random.Next(minNodesPerLayer, maxNodesPerLayer + 1);
            }

            // 노드 생성
            for (int i = 0; i < nodeCount; i++)
            {
                NodeType nodeType = DetermineNodeType(layer);
                Vector2 position = CalculateNodePosition(layer, i, nodeCount);

                MapNode node = new MapNode(currentNodeId++, nodeType, layer, position);
                layerNodes.Add(node);

                Debug.Log($"레이어 {layer} 노드 생성: {node}");
            }

            return layerNodes;
        }

        /// <summary>
        /// 노드 타입을 결정합니다
        /// </summary>
        private NodeType DetermineNodeType(int layer)
        {
            if (layer == 0)
            {
                return NodeType.Start;
            }

            if (layer == totalLayers)
            {
                return NodeType.Boss;
            }

            // 특정 레이어에 상점/휴식 보장
            if (layer == totalLayers / 3)
            {
                // 1/3 지점에 상점 보장
                return NodeType.Shop;
            }

            if (layer == (totalLayers * 2) / 3)
            {
                // 2/3 지점에 휴식 보장
                return NodeType.Rest;
            }

            // 가중치 기반 랜덤 선택
            int totalWeight = nodeTypeWeights.Values.Sum();
            int randomValue = random.Next(0, totalWeight);

            int currentWeight = 0;
            foreach (var kvp in nodeTypeWeights)
            {
                currentWeight += kvp.Value;
                if (randomValue < currentWeight)
                {
                    return kvp.Key;
                }
            }

            // 폴백
            return NodeType.Combat;
        }

        /// <summary>
        /// 노드의 UI 위치를 계산합니다
        /// </summary>
        private Vector2 CalculateNodePosition(int layer, int index, int totalInLayer)
        {
            float x = 0f;
            float y = layer * 200f;  // 레이어당 200 픽셀 간격

            // 가로 배치 (중앙 정렬)
            if (totalInLayer == 1)
            {
                x = 0f;
            }
            else
            {
                float spacing = 300f;  // 노드 간 가로 간격
                float totalWidth = (totalInLayer - 1) * spacing;
                x = -totalWidth / 2f + index * spacing;
            }

            // 약간의 랜덤 오프셋 추가
            x += random.Next(-30, 31);

            return new Vector2(x, y);
        }

        /// <summary>
        /// 두 레이어를 연결합니다
        /// </summary>
        private void ConnectLayers(List<MapNode> previousLayer, List<MapNode> currentLayer)
        {
            // 각 이전 레이어 노드에서 현재 레이어 노드로 연결
            foreach (var prevNode in previousLayer)
            {
                int connectionCount = random.Next(minConnectionsPerNode, maxConnectionsPerNode + 1);
                connectionCount = Mathf.Min(connectionCount, currentLayer.Count);

                // 연결 가능한 노드 리스트 (가까운 노드 우선)
                List<MapNode> availableNodes = currentLayer
                    .OrderBy(n => Mathf.Abs(n.position.x - prevNode.position.x))
                    .ToList();

                // 연결 생성
                for (int i = 0; i < connectionCount && i < availableNodes.Count; i++)
                {
                    prevNode.ConnectTo(availableNodes[i]);
                }
            }

            // 현재 레이어의 모든 노드가 최소 1개 연결되도록 보장
            foreach (var currentNode in currentLayer)
            {
                bool hasConnection = previousLayer.Any(n => n.IsConnectedTo(currentNode.id));
                if (!hasConnection && previousLayer.Count > 0)
                {
                    // 가장 가까운 이전 노드와 연결
                    var closestPrevNode = previousLayer
                        .OrderBy(n => Mathf.Abs(n.position.x - currentNode.position.x))
                        .First();
                    closestPrevNode.ConnectTo(currentNode);
                }
            }
        }

        /// <summary>
        /// 맵이 유효한지 검증합니다
        /// </summary>
        private void ValidateMap(List<MapNode> allNodes)
        {
            // 시작 노드 찾기
            MapNode startNode = allNodes.FirstOrDefault(n => n.nodeType == NodeType.Start);
            if (startNode == null)
            {
                Debug.LogError("시작 노드가 없습니다!");
                return;
            }

            // 보스 노드 찾기
            MapNode bossNode = allNodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
            if (bossNode == null)
            {
                Debug.LogError("보스 노드가 없습니다!");
                return;
            }

            // BFS로 시작→보스 경로 확인
            HashSet<int> visited = new HashSet<int>();
            Queue<MapNode> queue = new Queue<MapNode>();
            queue.Enqueue(startNode);
            visited.Add(startNode.id);

            bool pathToBossExists = false;

            while (queue.Count > 0)
            {
                MapNode current = queue.Dequeue();

                if (current.id == bossNode.id)
                {
                    pathToBossExists = true;
                    break;
                }

                foreach (int nextId in current.connectedNodeIds)
                {
                    if (!visited.Contains(nextId))
                    {
                        visited.Add(nextId);
                        MapNode nextNode = allNodes.FirstOrDefault(n => n.id == nextId);
                        if (nextNode != null)
                        {
                            queue.Enqueue(nextNode);
                        }
                    }
                }
            }

            if (pathToBossExists)
            {
                Debug.Log($"맵 검증 성공: 시작→보스 경로 존재 (방문 가능 노드: {visited.Count}/{allNodes.Count})");
            }
            else
            {
                Debug.LogError("맵 검증 실패: 시작→보스 경로가 없습니다!");
            }
        }

        /// <summary>
        /// 맵 설정을 변경합니다
        /// </summary>
        public void ConfigureMap(int layers, int minNodes, int maxNodes)
        {
            totalLayers = layers;
            minNodesPerLayer = minNodes;
            maxNodesPerLayer = maxNodes;
        }
    }
}
