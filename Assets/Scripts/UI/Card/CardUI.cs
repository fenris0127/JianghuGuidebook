using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// 카드 한 장의 시각적 표현과 상호작용을 담당하는 클래스입니다.
[RequireComponent(typeof(CanvasGroup))]
public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ITooltipProvider
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    private CardData cardData;

    // CardData를 받아와 UI 요소들을 설정합니다.
    public void Setup(CardData data)
    {
        this.cardData = data;
        
        nameText.text = data.cardName;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
    }

    /// 카드가 클릭되었을 때 BattleInteractionManager에 알립니다.
    public void OnPointerClick(PointerEventData eventData) => BattleInteractionManager.Instance?.SelectCard(this.cardData);

    #region Tooltip Provider
    // 마우스가 카드 위에 올라왔을 때 툴팁을 표시합니다.
    public void OnPointerEnter(PointerEventData eventData) => TooltipSystem.Instance.Show(GetTooltipContent(), GetTooltipTitle());

    // 마우스가 카드에서 벗어났을 때 툴팁을 숨깁니다.
    public void OnPointerExit(PointerEventData eventData) => TooltipSystem.Instance.Hide();

    public string GetTooltipTitle() => cardData.cardName;

    public string GetTooltipContent()
    {
        string content = cardData.description;
        if (cardData.isJeolcho)
            content += "\n\n<color=yellow>절초:</color> 이 카드는 이번 전투에서 한 번만 사용할 수 있습니다.";

        return content;
    }
    #endregion
}