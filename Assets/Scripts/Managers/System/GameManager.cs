using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 게임의 전체적인 상태를 정의합니다.
public enum GameState { MainMenu, MapView, Battle, Reward, Shop, Event, RestSite, GameOver, Victory }

// 게임의 전체적인 흐름과 상태를 관리하는 최고 관리자 클래스입니다.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject cardRewardUI;
    [SerializeField] private GameObject relicRewardUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject eventUI;
    [SerializeField] private GameObject restSiteUI;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject gameOverUI;
    
    [Header("Reward UI Components")]
    [SerializeField] private GameObject cardRewardSlotPrefab;
    [SerializeField] private Transform cardRewardContainer;
    [SerializeField] private GameObject relicRewardSlotPrefab;
    [SerializeField] private Transform relicRewardContainer;

    public GameState CurrentState { get; private set; }
    public FactionData SelectedFaction { get; private set; }
    public List<CardData> PlayerDeck { get; private set; }
    public int currentFloor { get; private set; } = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void StartNewGame(FactionData faction)
    {
        SaveLoadManager.Instance.DeleteSaveFile();
        SelectedFaction = faction;
        PlayerDeck = new List<CardData>(faction.startingDeck);
        currentFloor = 1;
        
        Player player = FindObjectOfType<Player>(true);
        MetaProgress progress = MetaManager.Instance.Progress;
        
        int bonusHealth = progress.bonusHealthLevel * 5;
        player.Setup(new List<CardData>(faction.startingDeck), bonusHealth);

        // --- 새로운 강화 적용 로직 ---
        int startingGold = progress.startingGoldLevel * 20; // 1레벨당 20골드
        if (startingGold > 0)
            player.GainGold(startingGold);

        MapManager.Instance.GenerateMap(currentFloor);
        ChangeState(GameState.MapView);
    }

    #region Battle Management
    public void StartBattle(EncounterData encounter)
    {
        ChangeState(GameState.Battle);
        FindObjectOfType<BattleManager>(true).StartBattle(encounter);
    }
    
    public void StartAscensionBattle(Realm currentRealm)
    {
        EncounterData ascensionEncounter = ResourceManager.Instance.GetEncounterData($"Ascension_{currentRealm}");
        if (ascensionEncounter != null)
            StartBattle(ascensionEncounter);
    }

    public void EndBattle(bool playerWon, bool wasBossFight)
    {
        if (playerWon)
        {
            FindObjectOfType<Player>().GainGold(Random.Range(10, 21));

            if (wasBossFight && currentFloor == MapManager.FINAL_FLOOR)
                ShowVictoryScreen();
            else if (wasBossFight)
                ClearFloor();
            else
                ShowCardRewardScreen();
        }
        else
        {
            ChangeState(GameState.GameOver);
            int pointsEarned = currentFloor * 10;
            MetaManager.Instance.AddEnlightenmentPoints(pointsEarned);
            MetaManager.Instance.SaveProgress();
        }
    }
    #endregion

    #region Reward Management
    public void ShowCardRewardScreen()
    {
        ChangeState(GameState.Reward);
        cardRewardUI.SetActive(true);

        foreach (Transform child in cardRewardContainer) Destroy(child.gameObject);

        List<CardData> rewardChoices = GenerateCardChoices(3);
        foreach (var card in rewardChoices)
        {
            GameObject slotObj = Instantiate(cardRewardSlotPrefab, cardRewardContainer);
            slotObj.GetComponent<CardUI>().Setup(card);
            slotObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectCardReward(card));
        }
    }

    private List<CardData> GenerateCardChoices(int count)
    {
        List<CardData> choices = new List<CardData>();
        List<CardData> availableCards = new List<CardData>(ResourceManager.Instance.GetAllCards());
        
        for (int i = 0; i < count; i++)
        {
            if (availableCards.Count == 0) break;
            CardRarity rarity = GetRandomRarity();
            List<CardData> candidates = availableCards.Where(c => c.rarity == rarity && !c.isUpgraded).ToList();
            if (candidates.Count == 0) candidates = availableCards.Where(c => c.rarity == CardRarity.Common && !c.isUpgraded).ToList();
            if (candidates.Count == 0) continue;

            CardData choice = candidates[Random.Range(0, candidates.Count)];
            choices.Add(choice);
            availableCards.Remove(choice);
        }
        return choices;
    }

    private CardRarity GetRandomRarity()
    {
        float roll = Random.value;
        if (roll <= 0.02f) return CardRarity.Legendary;
        if (roll <= 0.10f) return CardRarity.Epic;
        if (roll <= 0.30f) return CardRarity.Rare;
        if (roll <= 0.65f) return CardRarity.Uncommon;
        return CardRarity.Common;
    }

    public void SelectCardReward(CardData chosenCard)
    {
        FindObjectOfType<Player>().AddCardToDeck(chosenCard);
        ChangeState(GameState.MapView);
    }

    public void ShowRelicRewardScreen()
    {
        ChangeState(GameState.Reward);
        relicRewardUI.SetActive(true);

        foreach (Transform child in relicRewardContainer) Destroy(child.gameObject);

        List<RelicData> allRelics = ResourceManager.Instance.GetAllRelics();
        var relicChoices = allRelics.OrderBy(r => Random.value).Take(3).ToList();
        
        foreach (var relic in relicChoices)
        {
            GameObject slotObj = Instantiate(relicRewardSlotPrefab, relicRewardContainer);
            slotObj.GetComponent<UnityEngine.UI.Image>().sprite = relic.icon;
            slotObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectRelicReward(relic));
        }
    }
    
    public void SelectRelicReward(RelicData chosenRelic)
    {
        FindObjectOfType<Player>().AddRelic(chosenRelic);
        relicRewardUI.SetActive(false);
        MapManager.Instance.GenerateMap(currentFloor);
        ChangeState(GameState.MapView);
    }

    #endregion
    
    public void ClearFloor()
    {
        currentFloor++;
        ShowRelicRewardScreen();
    }
    
    private void ShowVictoryScreen()
    {
        ChangeState(GameState.Victory);
        int pointsEarned = 100 + (currentFloor * 10);
        MetaManager.Instance.AddEnlightenmentPoints(pointsEarned);
        MetaManager.Instance.SaveProgress();
        SaveLoadManager.Instance.DeleteSaveFile();
    }

    public void SaveCurrentRun()
{
    Player player = FindObjectOfType<Player>();
    if (player == null) return;

    RunData data = new RunData
    {
        currentFloor = this.currentFloor,

        playerMaxHealth = player.maxHealth,
        playerCurrentHealth = player.currentHealth,
        playerGold = player.gold,
        
        relicIDs = player.relics.Select(r => r.assetID).ToList(),
        
        drawPileIDs = player.drawPile.Select(c => c.assetID).ToList(),
        discardPileIDs = player.discardPile.Select(c => c.assetID).ToList(),
        handIDs = player.hand.Select(c => c.assetID).ToList(),
        exhaustPileIDs = player.exhaustPile.Select(c => c.assetID).ToList(),

        currentRealm = player.currentRealm,
        currentXp = player.currentXp,
        xpToNextRealm = player.xpToNextRealm,
        currentSwordRealm = player.currentSwordRealm
    };

    SaveLoadManager.Instance.SaveRun(data);
}

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        mainMenuUI?.SetActive(CurrentState == GameState.MainMenu);
        mapUI?.SetActive(CurrentState == GameState.MapView);
        battleUI?.SetActive(CurrentState == GameState.Battle);
        cardRewardUI?.SetActive(CurrentState == GameState.Reward && !relicRewardUI.activeSelf);
        relicRewardUI?.SetActive(CurrentState == GameState.Reward && relicRewardUI.activeSelf);
        shopUI?.SetActive(CurrentState == GameState.Shop);
        eventUI?.SetActive(CurrentState == GameState.Event);
        restSiteUI?.SetActive(CurrentState == GameState.RestSite);
        victoryUI?.SetActive(CurrentState == GameState.Victory);
        gameOverUI?.SetActive(CurrentState == GameState.GameOver);

        if (AudioManager.Instance != null)
        {
            switch (newState)
            {
                case GameState.MainMenu: AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuTheme); break;
                case GameState.MapView: AudioManager.Instance.PlayMusic(AudioManager.Instance.mapTheme); break;
                case GameState.Battle: AudioManager.Instance.PlayMusic(AudioManager.Instance.battleTheme); break;
                case GameState.Victory: AudioManager.Instance.PlayMusic(AudioManager.Instance.victoryTheme); break;
            }
        }
    }
}