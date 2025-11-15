using UnityEngine;
using System.Collections.Generic;

// 플레이어의 핸드(손패)를 화면에 표시하고 관리하는 클래스입니다.
public class HandUI : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handContainer;

    private Player player;
    private List<GameObject> cardObjects = new List<GameObject>();

    void Start()
    {
        player = FindObjectOfType<Player>(true);
        if (player != null)
            player.OnHandChanged += UpdateHandUI;
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnHandChanged -= UpdateHandUI;
    }

    private void UpdateHandUI(List<CardData> currentHand)
    {
        foreach (GameObject card in cardObjects)
            Destroy(card);

        cardObjects.Clear();

        foreach (CardData cardData in currentHand)
        {
            GameObject newCardObj = Instantiate(cardPrefab, handContainer);
            newCardObj.GetComponent<CardUI>().Setup(cardData);
            cardObjects.Add(newCardObj);
        }
    }
}