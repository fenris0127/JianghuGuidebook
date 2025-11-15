using UnityEngine;

// 휴식처 UI의 열고 닫기와 게임 상태 변경을 총괄하는 관리자입니다.
public class RestSiteManager : MonoBehaviour
{
    public static RestSiteManager Instance;

    [SerializeField] private RestSiteUI restSiteUI;

    void Awake()
    {
        Instance = this;
    }

    // MapNode에서 호출하여 휴식처를 엽니다.
    public void Open()
    {
        GameManager.Instance.ChangeState(GameState.RestSite);
        restSiteUI.Open();
    }

    // RestSiteUI에서 호출하여 휴식처를 닫습니다.
    public void Close()
    {
        GameManager.Instance.ChangeState(GameState.MapView);
    }
}