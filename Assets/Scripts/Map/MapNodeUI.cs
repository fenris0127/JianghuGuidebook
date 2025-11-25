using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 개별 맵 노드의 UI 표시 및 상호작용을 담당합니다.
    /// </summary>
    public class MapNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("UI Components")]
        [SerializeField] private Image nodeIcon;
        [SerializeField] private Image nodeBorder;
        [SerializeField] private Button button;
        [SerializeField] private GameObject visitedMark; // 방문 완료 표시 (체크표시 등)
        [SerializeField] private GameObject currentNodeMark; // 현재 위치 표시 (플레이어 아이콘 등)

        [Header("Colors")]
        [SerializeField] private Color accessibleColor = Color.white;
        [SerializeField] private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        [SerializeField] private Color visitedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        [SerializeField] private Color bossColor = new Color(1f, 0.3f, 0.3f, 1f);

        [Header("Icons")]
        [SerializeField] private Sprite combatIcon;
        [SerializeField] private Sprite eliteIcon;
        [SerializeField] private Sprite restIcon;
        [SerializeField] private Sprite shopIcon;
        [SerializeField] private Sprite eventIcon;
        [SerializeField] private Sprite treasureIcon;
        [SerializeField] private Sprite bossIcon;
        [SerializeField] private Sprite startIcon;

        private MapNode mapNode;
        private MapUI mapUI;

        public MapNode Node => mapNode;

        /// <summary>
        /// 노드 UI를 초기화합니다.
        /// </summary>
        public void Initialize(MapNode node, MapUI ui)
        {
            this.mapNode = node;
            this.mapUI = ui;

            // 아이콘 설정
            UpdateIcon();

            // 초기 상태 업데이트
            UpdateState();
        }

        private void UpdateIcon()
        {
            if (nodeIcon == null) return;

            switch (mapNode.nodeType)
            {
                case NodeType.Combat: nodeIcon.sprite = combatIcon; break;
                case NodeType.EliteCombat: nodeIcon.sprite = eliteIcon; break;
                case NodeType.Rest: nodeIcon.sprite = restIcon; break;
                case NodeType.Shop: nodeIcon.sprite = shopIcon; break;
                case NodeType.Event: nodeIcon.sprite = eventIcon; break;
                case NodeType.Treasure: nodeIcon.sprite = treasureIcon; break;
                case NodeType.Boss: nodeIcon.sprite = bossIcon; break;
                case NodeType.Start: nodeIcon.sprite = startIcon; break;
            }

            // 보스 노드는 특별한 색상
            if (mapNode.nodeType == NodeType.Boss)
            {
                nodeIcon.color = bossColor;
            }
        }

        /// <summary>
        /// 노드의 시각적 상태를 업데이트합니다.
        /// </summary>
        public void UpdateState()
        {
            if (mapNode == null) return;

            // 방문 여부 표시
            if (visitedMark != null)
                visitedMark.SetActive(mapNode.isVisited);

            // 현재 위치 표시
            if (currentNodeMark != null)
                currentNodeMark.SetActive(mapNode.isCurrentNode);

            // 버튼 활성화 상태 및 색상
            if (mapNode.isVisited)
            {
                // 이미 방문한 노드
                nodeIcon.color = visitedColor;
                button.interactable = false;
            }
            else if (mapNode.isAccessible)
            {
                // 갈 수 있는 노드
                nodeIcon.color = accessibleColor;
                button.interactable = true;
                
                // 보스 노드는 항상 보스 색상 유지하되 밝게
                if (mapNode.nodeType == NodeType.Boss)
                    nodeIcon.color = bossColor;
            }
            else
            {
                // 갈 수 없는 노드
                nodeIcon.color = lockedColor;
                button.interactable = false;
            }

            // 현재 노드라면 강조
            if (mapNode.isCurrentNode)
            {
                nodeIcon.color = Color.white;
                // 추가 강조 효과가 필요하다면 여기에 구현
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (mapNode.isAccessible && !mapNode.isVisited)
            {
                MapManager.Instance.MoveToNode(mapNode);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 툴팁 표시 요청
            string title = GetNodeTitle(mapNode.nodeType);
            string desc = GetNodeDescription(mapNode.nodeType);
            
            // TODO: MapUI를 통해 툴팁 표시
            // mapUI.ShowTooltip(title, desc, transform.position);
            
            // 호버 효과 (크기 증가 등)
            transform.localScale = Vector3.one * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 툴팁 숨기기
            // mapUI.HideTooltip();

            // 호버 효과 해제
            transform.localScale = Vector3.one;
        }

        private string GetNodeTitle(NodeType type)
        {
            switch (type)
            {
                case NodeType.Combat: return "전투";
                case NodeType.EliteCombat: return "정예 전투";
                case NodeType.Rest: return "휴식";
                case NodeType.Shop: return "상점";
                case NodeType.Event: return "이벤트";
                case NodeType.Treasure: return "보물";
                case NodeType.Boss: return "보스";
                case NodeType.Start: return "시작";
                default: return "알 수 없음";
            }
        }

        private string GetNodeDescription(NodeType type)
        {
            switch (type)
            {
                case NodeType.Combat: return "일반 적과 전투합니다.";
                case NodeType.EliteCombat: return "강력한 적과 전투하고 좋은 보상을 얻습니다.";
                case NodeType.Rest: return "체력을 회복하거나 카드를 강화합니다.";
                case NodeType.Shop: return "골드로 카드나 유물을 구매합니다.";
                case NodeType.Event: return "무작위 사건이 발생합니다.";
                case NodeType.Treasure: return "유물이나 골드를 획득합니다.";
                case NodeType.Boss: return "이 지역의 최종 보스와 대결합니다.";
                case NodeType.Start: return "여정의 시작점입니다.";
                default: return "";
            }
        }
    }
}
