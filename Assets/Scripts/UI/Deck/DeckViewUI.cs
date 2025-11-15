using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

// 카드 강화나 제거 시, 플레이어의 전체 덱 목록을 보여주는 범용 UI 클래스입니다.
public class DeckViewUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject selectableCardPrefab;
    [SerializeField] private Button closeButton;

    private Action<CardData> onCardSelectedCallback;
    private List<GameObject> cardObjects = new List<GameObject>();

    void Awake()
    {
        closeButton?.onClick.AddListener(Hide);
        panel.SetActive(false);
    }

    // 지정된 카드 목록과 타이틀, 그리고 카드 선택 시 호출될 콜백 함수를 받아 덱 뷰를 엽니다.
    public void Show(List<CardData> cards, string title, Action<CardData> callback)
    {
        panel.SetActive(true);
        titleText.text = title;
        onCardSelectedCallback = callback;

        foreach (var obj in cardObjects) Destroy(obj);
        cardObjects.Clear();

        foreach (var cardData in cards)
        {
            GameObject newCardObj = Instantiate(selectableCardPrefab, cardContainer);
            SelectableCardUI selectableCard = newCardObj.GetComponent<SelectableCardUI>();
            selectableCard.Setup(cardData);
            selectableCard.OnCardSelected += HandleCardSelection;
            cardObjects.Add(newCardObj);
        }
    }

    private void HandleCardSelection(SelectableCardUI selectedCard) => onCardSelectedCallback?.Invoke(selectedCard.GetCardData());

    public void Hide() => panel.SetActive(false);
}