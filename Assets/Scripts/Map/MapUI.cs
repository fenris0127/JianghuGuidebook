using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 맵 화면의 전체 UI를 관리합니다.
    /// 노드 생성, 스크롤, 상태 업데이트 등을 담당합니다.
    /// </summary>
    public class MapUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;
        [SerializeField] private MapLineRenderer lineRenderer;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject nodePrefab;
        
        [Header("Layout Settings")]
        [SerializeField] private float xSpacing = 200f; // 레이어 간 가로 간격
        [SerializeField] private float ySpacing = 150f; // 노드 간 세로 간격
        [SerializeField] private float paddingX = 100f; // 좌우 여백
        [SerializeField] private float paddingY = 100f; // 상하 여백
        [SerializeField] private float randomness = 30f; // 위치 랜덤성

        private List<MapNodeUI> nodeUIs = new List<MapNodeUI>();

        private void Start()
        {
            // 맵 매니저 이벤트 구독
            MapManager.Instance.OnMapGenerated += GenerateMapUI;
            MapManager.Instance.OnNodeEntered += OnNodeEntered;
            
            // 이미 맵이 생성되어 있다면 UI 생성
            if (MapManager.Instance.AllNodes.Count > 0)
            {
                GenerateMapUI(MapManager.Instance.AllNodes);
            }
        }

        private void OnDestroy()
        {
            if (MapManager.Instance != null)
            {
                MapManager.Instance.OnMapGenerated -= GenerateMapUI;
                MapManager.Instance.OnNodeEntered -= OnNodeEntered;
            }
        }

        /// <summary>
        /// 맵 데이터를 기반으로 UI를 생성합니다.
        /// </summary>
        public void GenerateMapUI(List<MapNode> nodes)
        {
            ClearMap();

            if (nodes == null || nodes.Count == 0) return;

            // 1. 노드 위치 계산 및 배치
            PlaceNodes(nodes);

            // 2. 연결선 그리기
            DrawConnections(nodes);

            // 3. 스크롤 뷰 크기 조정
            UpdateContentSize(nodes);

            // 4. 현재 노드로 스크롤 이동
            if (MapManager.Instance.CurrentNode != null)
            {
                ScrollToNode(MapManager.Instance.CurrentNode);
            }
            else
            {
                // 시작 노드로 이동
                var startNode = nodes.FirstOrDefault(n => n.nodeType == NodeType.Start);
                if (startNode != null) ScrollToNode(startNode);
            }
        }

        private void ClearMap()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            nodeUIs.Clear();
        }

        private void PlaceNodes(List<MapNode> nodes)
        {
            // 레이어별로 노드 그룹화
            var nodesByLayer = nodes.GroupBy(n => n.layer).OrderBy(g => g.Key);

            foreach (var layerGroup in nodesByLayer)
            {
                int layerIndex = layerGroup.Key;
                var layerNodes = layerGroup.ToList();
                int nodeCount = layerNodes.Count;

                for (int i = 0; i < nodeCount; i++)
                {
                    MapNode node = layerNodes[i];
                    
                    // 위치 계산
                    // X: 레이어 인덱스에 따라 증가
                    float xPos = paddingX + (layerIndex * xSpacing);
                    
                    // Y: 중앙을 기준으로 위아래로 배치
                    // 예: 3개일 때 -> -150, 0, 150
                    float yOffset = (nodeCount - 1) * ySpacing / 2f;
                    float yPos = (i * ySpacing) - yOffset;

                    // 랜덤성 추가 (시작/보스 노드 제외)
                    if (node.nodeType != NodeType.Start && node.nodeType != NodeType.Boss)
                    {
                        yPos += Random.Range(-randomness, randomness);
                    }

                    // 노드 데이터에 위치 저장 (나중에 선 그릴 때 사용)
                    node.position = new Vector2(xPos, yPos);

                    // UI 생성
                    CreateNodeUI(node, new Vector2(xPos, yPos));
                }
            }
        }

        private void CreateNodeUI(MapNode node, Vector2 position)
        {
            GameObject nodeObj = Instantiate(nodePrefab, content);
            nodeObj.name = $"Node_{node.id}_{node.nodeType}";
            
            RectTransform rect = nodeObj.GetComponent<RectTransform>();
            rect.anchoredPosition = position;

            MapNodeUI ui = nodeObj.GetComponent<MapNodeUI>();
            ui.Initialize(node, this);
            nodeUIs.Add(ui);
        }

        private void DrawConnections(List<MapNode> nodes)
        {
            if (lineRenderer == null) return;

            foreach (var node in nodes)
            {
                foreach (int nextNodeId in node.connectedNodeIds)
                {
                    MapNode nextNode = nodes.FirstOrDefault(n => n.id == nextNodeId);
                    if (nextNode != null)
                    {
                        lineRenderer.CreateLine(node.position, nextNode.position, content);
                    }
                }
            }
        }

        private void UpdateContentSize(List<MapNode> nodes)
        {
            if (nodes.Count == 0) return;

            int maxLayer = nodes.Max(n => n.layer);
            float width = paddingX * 2 + (maxLayer * xSpacing);
            
            // 높이는 넉넉하게 잡음 (가장 많은 노드가 있는 레이어 기준)
            int maxNodesInLayer = nodes.GroupBy(n => n.layer).Max(g => g.Count());
            float height = paddingY * 2 + (maxNodesInLayer * ySpacing);

            content.sizeDelta = new Vector2(width, height);
        }

        private void OnNodeEntered(MapNode node)
        {
            // 모든 노드 UI 상태 업데이트
            foreach (var ui in nodeUIs)
            {
                ui.UpdateState();
            }

            // 현재 노드로 스크롤
            ScrollToNode(node);
        }

        public void ScrollToNode(MapNode node)
        {
            // TODO: 부드러운 스크롤 구현
            // 현재는 간단하게 구현
            
            // 뷰포트 너비
            float viewportWidth = scrollRect.viewport.rect.width;
            
            // 노드의 X 위치
            float nodeX = node.position.x;
            
            // 노드가 화면 중앙에 오도록 컨텐츠 위치 조정
            // content.anchoredPosition.x는 음수여야 함 (오른쪽으로 이동하려면)
            float targetX = -nodeX + (viewportWidth / 2f);
            
            // 범위 제한
            float minX = -(content.sizeDelta.x - viewportWidth);
            float maxX = 0;
            
            targetX = Mathf.Clamp(targetX, minX, maxX);
            
            content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
        }
    }
}
