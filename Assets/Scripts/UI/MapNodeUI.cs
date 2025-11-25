using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JianghuGuidebook.Map;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 맵 노드의 시각적 표현을 담당하는 UI 컴포넌트
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class MapNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("참조")]
        [SerializeField] private Image nodeImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;

        [Header("색상 설정")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color visitedColor = Color.gray;
        [SerializeField] private Color currentColor = Color.yellow;
        [SerializeField] private Color accessibleColor = Color.green;
        [SerializeField] private Color inaccessibleColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        [SerializeField] private Color bossColor = new Color(1f, 0.2f, 0.2f);  // 빨간색 (보스 노드용)

        [Header("하이라이트 설정")]
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float normalScale = 1.0f;

        // 연결된 노드 데이터
        private MapNode nodeData;
        private Vector3 originalScale;

        private void Awake()
        {
            // 컴포넌트 자동 할당
            if (nodeImage == null)
                nodeImage = GetComponent<Image>();

            if (button == null)
                button = GetComponent<Button>();

            originalScale = transform.localScale;

            // 버튼 클릭 이벤트 연결
            button.onClick.AddListener(OnNodeClicked);
        }

        /// <summary>
        /// 노드 데이터를 설정하고 시각적으로 업데이트합니다
        /// </summary>
        public void SetNodeData(MapNode node)
        {
            nodeData = node;
            UpdateVisuals();
        }

        /// <summary>
        /// 노드의 시각적 상태를 업데이트합니다
        /// </summary>
        public void UpdateVisuals()
        {
            if (nodeData == null)
            {
                Debug.LogWarning("노드 데이터가 설정되지 않았습니다");
                return;
            }

            // 상태에 따른 색상 결정
            Color targetColor = normalColor;

            if (nodeData.isCurrentNode)
            {
                // 현재 노드 - 노란색 하이라이트
                targetColor = currentColor;
                button.interactable = false;
            }
            else if (nodeData.isVisited)
            {
                // 방문한 노드 - 회색 처리
                targetColor = visitedColor;
                button.interactable = false;
            }
            else if (nodeData.isAccessible)
            {
                // 보스 노드는 빨간색으로 특별 표시
                if (nodeData.nodeType == NodeType.Boss)
                {
                    targetColor = bossColor;
                }
                else
                {
                    // 일반 접근 가능한 노드 - 초록색 하이라이트
                    targetColor = accessibleColor;
                }
                button.interactable = true;
            }
            else
            {
                // 접근 불가능한 노드 - 기본 색상 또는 보스는 어두운 빨간색
                if (nodeData.nodeType == NodeType.Boss)
                {
                    targetColor = new Color(bossColor.r * 0.5f, bossColor.g * 0.5f, bossColor.b * 0.5f, 0.5f);
                }
                else
                {
                    targetColor = inaccessibleColor;
                }
                button.interactable = false;
            }

            // 색상 적용
            nodeImage.color = targetColor;

            // 아이콘 이미지가 있다면 동일한 색상 적용
            if (iconImage != null)
            {
                iconImage.color = targetColor;
            }
        }

        /// <summary>
        /// 노드 클릭 시 호출됩니다
        /// </summary>
        private void OnNodeClicked()
        {
            if (nodeData == null || !nodeData.isAccessible)
                return;

            // 보스 노드 클릭 시 특별 메시지
            if (nodeData.nodeType == NodeType.Boss)
            {
                Debug.Log($"[보스 노드] 클릭: {nodeData} - 자동으로 보스 전투가 시작됩니다!");
            }
            else
            {
                Debug.Log($"노드 클릭: {nodeData}");
            }

            // MapManager를 통해 노드 진입
            // 보스 노드의 경우 MoveToNode에서 자동으로 씬이 로드됩니다
            MapManager.Instance.EnterNode(nodeData);
        }

        /// <summary>
        /// 마우스 호버 시 확대 효과
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (nodeData != null && nodeData.isAccessible && !nodeData.isCurrentNode)
            {
                transform.localScale = originalScale * hoverScale;
            }
        }

        /// <summary>
        /// 마우스 나갈 때 원래 크기로
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = originalScale;
        }

        /// <summary>
        /// 노드 타입에 따른 아이콘 설정 (선택적)
        /// </summary>
        public void SetNodeIcon(Sprite iconSprite)
        {
            if (iconImage != null && iconSprite != null)
            {
                iconImage.sprite = iconSprite;
                iconImage.enabled = true;
            }
        }

        /// <summary>
        /// 강제로 시각적 상태를 갱신합니다 (외부에서 호출 가능)
        /// </summary>
        public void RefreshVisuals()
        {
            UpdateVisuals();
        }

        private void OnDestroy()
        {
            // 이벤트 리스너 정리
            if (button != null)
            {
                button.onClick.RemoveListener(OnNodeClicked);
            }
        }
    }
}
