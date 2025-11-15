using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// 휴식처 노드에서 선택지(휴식, 강화, 폐관수련)를 제공하는 UI 클래스입니다.
public class RestSiteUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button restButton;
    [SerializeField] private Button smithButton;
    [SerializeField] private Button ascendButton;
    [SerializeField] private DeckViewUI deckViewUI;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>(true);
        
        restButton.onClick.AddListener(OnRest);
        smithButton.onClick.AddListener(OnSmith);
        ascendButton.onClick.AddListener(OnAscend);

        panel.SetActive(false);
    }
    
    public void Open()
    {
        panel.SetActive(true);
        ascendButton.gameObject.SetActive(player.isReadyToAscend);

        bool canSmith = player.GetAllCardsInDeck().Any(c => !c.isUpgraded && c.upgradedVersion != null);
        smithButton.interactable = canSmith;
    }

    private void OnRest()
    {
        player.Heal(Mathf.RoundToInt(player.maxHealth * 0.3f));
        Close();
    }

    private void OnSmith()
    {
        panel.SetActive(false);
        
        var upgradableCards = player.GetAllCardsInDeck().FindAll(c => !c.isUpgraded && c.upgradedVersion != null);
        deckViewUI.Show(upgradableCards, "강화할 카드를 선택하세요", card => {
            player.UpgradeCard(card);
            deckViewUI.Hide();
            Close();
        });
    }

    private void OnAscend()
    {
        GameManager.Instance.StartAscensionBattle(player.currentRealm);
        Close();
    }
    
    private void Close()
    {
        panel.SetActive(false);
        RestSiteManager.Instance.Close();
    }
}