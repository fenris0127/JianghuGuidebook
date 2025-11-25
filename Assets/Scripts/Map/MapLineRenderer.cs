using UnityEngine;
using UnityEngine.UI;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 맵 노드 사이의 연결선을 그리는 유틸리티입니다.
    /// UI Canvas 상에서 RectTransform을 사용하여 선을 그립니다.
    /// </summary>
    public class MapLineRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite lineSprite;
        [SerializeField] private float lineWidth = 5f;
        [SerializeField] private Color lineColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        /// <summary>
        /// 두 지점 사이에 선을 생성합니다.
        /// </summary>
        /// <param name="startPos">시작 위치 (Local Position)</param>
        /// <param name="endPos">끝 위치 (Local Position)</param>
        /// <param name="parent">부모 Transform</param>
        public void CreateLine(Vector2 startPos, Vector2 endPos, Transform parent)
        {
            GameObject lineObj = new GameObject("Line");
            lineObj.transform.SetParent(parent, false);
            
            // Image 컴포넌트 추가
            Image image = lineObj.AddComponent<Image>();
            image.sprite = lineSprite;
            image.color = lineColor;
            image.raycastTarget = false; // 마우스 이벤트 차단하지 않도록

            // RectTransform 설정
            RectTransform rect = lineObj.GetComponent<RectTransform>();
            
            // 위치 및 회전 계산
            Vector2 direction = (endPos - startPos).normalized;
            float distance = Vector2.Distance(startPos, endPos);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 중심점 설정
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0, 0.5f); // 피벗을 왼쪽 중앙으로 설정하여 시작점에서 뻗어나가게 함
            
            rect.sizeDelta = new Vector2(distance, lineWidth);
            rect.anchoredPosition = startPos;
            rect.localRotation = Quaternion.Euler(0, 0, angle);
            
            // 계층 구조에서 뒤로 보내기 (노드보다 뒤에 그려지도록)
            lineObj.transform.SetAsFirstSibling();
        }
    }
}
