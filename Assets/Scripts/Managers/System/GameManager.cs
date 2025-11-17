using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GangHoBiGeup.Data;
using GangHoBiGeup.Managers;

// 게임의 전체적인 상태를 정의합니다.
public enum GameState { MainMenu, MapView, Battle, Reward, Shop, Event, RestSite, GameOver, Victory }

// 게임의 전체적인 흐름과 상태를 관리하는 최고 관리자 클래스입니다.
// 보상 생성 로직은 RewardManager로 분리되었습니다.
// UI 상태 관리는 Dictionary 기반으로 선언적으로 처리합니다.
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

    // 성능 최적화: 자주 사용하는 컴포넌트 캐싱
    private Player cachedPlayer;
    private BattleManager cachedBattleManager;

    // UI 상태 매핑
    private Dictionary<GameState, GameObject> uiPanels;

    private Player Player => cachedPlayer != null ? cachedPlayer : (cachedPlayer = FindObjectOfType<Player>(true));
    private BattleManager BattleManager => cachedBattleManager != null ? cachedBattleManager : (cachedBattleManager = FindObjectOfType<BattleManager>(true));

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

        InitializeUIPanels();
    }

    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    private void InitializeUIPanels()
    {
        uiPanels = new Dictionary<GameState, GameObject>
        {
            { GameState.MainMenu, mainMenuUI },
            { GameState.MapView, mapUI },
            { GameState.Battle, battleUI },
            { GameState.Shop, shopUI },
            { GameState.Event, eventUI },
            { GameState.RestSite, restSiteUI },
            { GameState.Victory, victoryUI },
            { GameState.GameOver, gameOverUI }
        };
    }

    public void StartNewGame(FactionData faction)
    {
        SaveLoadManager.Instance.DeleteSaveFile();
        SelectedFaction = faction;
        PlayerDeck = new List<CardData>(faction.startingDeck);
        currentFloor = 1;

        // 캐싱된 Player 사용
        MetaProgress progress = MetaManager.Instance.Progress;

        // RewardManager를 통해 보너스 계산
        int bonusHealth = RewardManager.Instance.CalculateBonusHealth(progress.bonusHealthLevel);
        Player.Setup(new List<CardData>(faction.startingDeck), bonusHealth);

        // RewardManager를 통해 시작 골드 계산
        int startingGold = RewardManager.Instance.CalculateStartingGold(progress.startingGoldLevel);
        if (startingGold > 0)
            Player.GainGold(startingGold);

        MapManager.Instance.GenerateMap(currentFloor);
        ChangeState(GameState.MapView);
    }

    #region Battle Management
    public void StartBattle(EncounterData encounter)
    {
        ChangeState(GameState.Battle);
        BattleManager.StartBattle(encounter);
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
            // RewardManager를 통해 전투 보상 골드 생성
            int goldReward = RewardManager.Instance.GenerateCombatRewardGold();
            Player.GainGold(goldReward);

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
            // RewardManager를 통해 깨달음 포인트 계산
            int pointsEarned = RewardManager.Instance.CalculateEnlightenmentPoints(currentFloor);
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

        // RewardManager를 통해 카드 보상 생성
        List<CardData> rewardChoices = RewardManager.Instance.GenerateCardRewards();
        foreach (var card in rewardChoices)
        {
            GameObject slotObj = Instantiate(cardRewardSlotPrefab, cardRewardContainer);
            slotObj.GetComponent<CardUI>().Setup(card);
            slotObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectCardReward(card));
        }
    }

    public void SelectCardReward(CardData chosenCard)
    {
        Player.AddCardToDeck(chosenCard);
        ChangeState(GameState.MapView);
    }

    public void ShowRelicRewardScreen()
    {
        ChangeState(GameState.Reward);
        relicRewardUI.SetActive(true);

        foreach (Transform child in relicRewardContainer) Destroy(child.gameObject);

        // RewardManager를 통해 유물 보상 생성
        List<RelicData> relicChoices = RewardManager.Instance.GenerateRelicRewards();

        foreach (var relic in relicChoices)
        {
            GameObject slotObj = Instantiate(relicRewardSlotPrefab, relicRewardContainer);
            slotObj.GetComponent<UnityEngine.UI.Image>().sprite = relic.icon;
            slotObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectRelicReward(relic));
        }
    }
    
    public void SelectRelicReward(RelicData chosenRelic)
    {
        Player.AddRelic(chosenRelic);
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
        // RewardManager를 통해 승리 포인트 계산
        int pointsEarned = RewardManager.Instance.CalculateVictoryEnlightenmentPoints(currentFloor);
        MetaManager.Instance.AddEnlightenmentPoints(pointsEarned);
        MetaManager.Instance.SaveProgress();
        SaveLoadManager.Instance.DeleteSaveFile();
    }

    public void SaveCurrentRun()
    {
        if (Player == null) return;

        RunData data = new RunData
        {
            currentFloor = this.currentFloor,

            playerMaxHealth = Player.maxHealth,
            playerCurrentHealth = Player.currentHealth,
            playerGold = Player.gold,

            relicIDs = Player.relics.Select(r => r.assetID).ToList(),

            drawPileIDs = Player.drawPile.Select(c => c.assetID).ToList(),
            discardPileIDs = Player.discardPile.Select(c => c.assetID).ToList(),
            handIDs = Player.hand.Select(c => c.assetID).ToList(),
            exhaustPileIDs = Player.exhaustPile.Select(c => c.assetID).ToList(),

            currentRealm = Player.currentRealm,
            currentXp = Player.currentXp,
            xpToNextRealm = Player.xpToNextRealm,
            currentSwordRealm = Player.currentSwordRealm
        };

        SaveLoadManager.Instance.SaveRun(data);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        // 모든 UI 패널 비활성화
        foreach (var panel in uiPanels.Values)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        // 현재 상태에 맞는 UI 패널 활성화
        if (uiPanels.TryGetValue(newState, out GameObject activePanel) && activePanel != null)
        {
            activePanel.SetActive(true);
        }

        // Reward 상태는 특별 처리 (cardRewardUI 또는 relicRewardUI)
        // 이미 ShowCardRewardScreen()이나 ShowRelicRewardScreen()에서 처리됨

        // 음악 재생
        PlayMusicForState(newState);
    }

    private void PlayMusicForState(GameState state)
    {
        if (AudioManager.Instance == null) return;

        switch (state)
        {
            case GameState.MainMenu:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuTheme);
                break;
            case GameState.MapView:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.mapTheme);
                break;
            case GameState.Battle:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.battleTheme);
                break;
            case GameState.Victory:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.victoryTheme);
                break;
        }
    }
}