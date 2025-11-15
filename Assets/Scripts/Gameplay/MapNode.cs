using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// 맵을 구성하는 단일 노드(지점)의 타입입니다.
public enum NodeType { Combat, EliteCombat, Event, Shop, RestSite, Boss }

// 맵을 구성하는 단일 노드의 동작을 정의합니다.
[RequireComponent(typeof(Button), typeof(Image))]
public class MapNode : MonoBehaviour
{
    public NodeType nodeType;
    public EncounterData encounterData; // 이 노드가 전투 노드일 경우의 데이터
    public EventData eventData; // 이 노드가 기연 노드일 경우의 데이터
    
    [HideInInspector] public List<MapNode> connectedNodes = new List<MapNode>();
    [HideInInspector] public int layer;

    [Header("시각적 처리")]
    [SerializeField] private Color clearedColor = Color.gray;
    [SerializeField] private Color clickableColor = Color.white;
    [SerializeField] private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);

    private Button button;
    private Image image;
    private bool isCleared = false;

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(OnNodeClicked);
    }

    // 플레이어가 이 노드를 클릭했을 때 호출됩니다.
    public void OnNodeClicked()
    {
        if (!button.interactable || isCleared) return;
        
        MapManager.Instance.OnNodeSelected(this);
        isCleared = true;
        UpdateVisuals();

        switch (nodeType)
        {
            case NodeType.Combat:
            case NodeType.EliteCombat:
            case NodeType.Boss:
                if (encounterData != null) GameManager.Instance.StartBattle(encounterData);
                break;
            case NodeType.Event:
                if (eventData != null) EventManager.Instance.ShowEvent(eventData);
                break;
            case NodeType.Shop:
                ShopManager.Instance.OpenShop();
                break;
            case NodeType.RestSite:
                RestSiteManager.Instance.Open();
                break;
        }
    }

    // MapManager가 호출하여 노드의 위치 정보를 설정합니다.
    public void Setup(int layer)
    {
        this.layer = layer;
    }
    
    // 노드의 클릭 가능 상태를 설정합니다.
    public void SetClickable(bool state)
    {
        button.interactable = state;
        UpdateVisuals();
    }

    // 현재 노드의 상태(클리어, 클릭 가능/불가능)에 따라 색상을 업데이트합니다.
    public void UpdateVisuals()
    {
        if (isCleared)
        {
            image.color = clearedColor;
        }
        else if (button.interactable)
        {
            image.color = clickableColor;
        }
        else
        {
            image.color = lockedColor;
        }
    }
}