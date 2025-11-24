using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 맵의 개별 노드를 나타냅니다
    /// </summary>
    [System.Serializable]
    public class MapNode
    {
        // 노드 정보
        public int id;
        public NodeType nodeType;
        public int layer;           // 세로 레이어 (0 = 시작, N = 보스)
        public Vector2 position;    // UI 상의 위치

        // 연결 정보
        public List<int> connectedNodeIds = new List<int>();  // 다음 레이어 노드 ID 리스트

        // 상태
        public bool isVisited = false;
        public bool isAccessible = false;  // 현재 진입 가능한지
        public bool isCurrentNode = false;

        // 특수 데이터 (선택적)
        public string specialDataId;  // 특정 이벤트 ID, 보스 ID 등

        public MapNode(int id, NodeType type, int layer, Vector2 position)
        {
            this.id = id;
            this.nodeType = type;
            this.layer = layer;
            this.position = position;
        }

        /// <summary>
        /// 다음 노드와 연결합니다
        /// </summary>
        public void ConnectTo(MapNode nextNode)
        {
            if (!connectedNodeIds.Contains(nextNode.id))
            {
                connectedNodeIds.Add(nextNode.id);
            }
        }

        /// <summary>
        /// 특정 노드와 연결되어 있는지 확인합니다
        /// </summary>
        public bool IsConnectedTo(int nodeId)
        {
            return connectedNodeIds.Contains(nodeId);
        }

        /// <summary>
        /// 노드를 방문 처리합니다
        /// </summary>
        public void Visit()
        {
            isVisited = true;
            isCurrentNode = true;
            isAccessible = false;
        }

        /// <summary>
        /// 노드 정보를 문자열로 반환합니다
        /// </summary>
        public override string ToString()
        {
            return $"Node {id} - {nodeType} (Layer {layer}) - Connections: {connectedNodeIds.Count}";
        }
    }
}
