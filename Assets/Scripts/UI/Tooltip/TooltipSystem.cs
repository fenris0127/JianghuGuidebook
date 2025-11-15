using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 게임 내 모든 툴팁의 표시와 숨김을 관리하는 중앙 시스템입니다.
[RequireComponent(typeof(CanvasGroup))]
public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit; // 텍스트 너비 자동 줄바꿈 기준 길이

    private RectTransform panelRectTransform;

    void Awake()
    {
        Instance = this;
        panelRectTransform = tooltipPanel.GetComponent<RectTransform>();
        Hide();
    }

    void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            // 툴팁이 활성화되어 있을 때 마우스 위치를 따라다니도록 함
            Vector2 position = Input.mousePosition;
            
            // 화면 경계를 벗어나지 않도록 피벗(pivot)을 동적으로 조정
            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;
            panelRectTransform.pivot = new Vector2(pivotX, pivotY);

            transform.position = position;
        }
    }

    public void Show(string content, string title = "")
    {
        if (string.IsNullOrEmpty(title))
        {
            titleText.gameObject.SetActive(false);
        }
        else
        {
            titleText.gameObject.SetActive(true);
            titleText.text = title;
        }

        contentText.text = content;
        
        // 텍스트 길이에 따라 패널 너비를 자동으로 조절할지 결정
        int titleLength = titleText.text.Length;
        int contentLength = contentText.text.Length;
        layoutElement.enabled = (titleLength > characterWrapLimit || contentLength > characterWrapLimit);
        
        tooltipPanel.SetActive(true);
    }

    public void Hide() => tooltipPanel.SetActive(false);
}