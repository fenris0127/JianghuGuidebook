using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 요소에 붙여서 간단한 텍스트 툴팁을 띄우는 범용 트리거입니다.
/// </summary>
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string title;
    [SerializeField] [TextArea] private string content;

    public void OnPointerEnter(PointerEventData eventData) => TooltipSystem.Instance.Show(content, title);

    public void OnPointerExit(PointerEventData eventData) => TooltipSystem.Instance.Hide();

    public void SetText(string newContent, string newTitle = "")
    {
        this.content = newContent;
        this.title = newTitle;
    }
}