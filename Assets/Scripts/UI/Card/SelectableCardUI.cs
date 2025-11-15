using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 클릭 가능한 카드 UI 슬롯의 동작을 제어하는 범용 클래스입니다.
/// </summary>
[RequireComponent(typeof(Button))]
public class SelectableCardUI : MonoBehaviour
{
    [SerializeField] private CardUI cardUI;
    private Button button;
    private CardData cardData;

    public event Action<SelectableCardUI> OnCardSelected;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    public void Setup(CardData data)
    {
        this.cardData = data;
        cardUI.Setup(data);
    }

    public CardData GetCardData() => cardData;

    public void SetInteractable(bool state) => button.interactable = state;

    private void HandleClick() => OnCardSelected?.Invoke(this);
}