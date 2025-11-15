using UnityEngine;

/// <summary>
/// 깨달음 포인트 등 게임 세션을 넘어 영구적으로 저장되는 데이터를 관리합니다.
/// </summary>
public class MetaManager : MonoBehaviour
{
    public static MetaManager Instance;
    public MetaProgress Progress { get; private set; }
    private const string SAVE_KEY = "MetaProgressSave";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
            Progress = JsonUtility.FromJson<MetaProgress>(PlayerPrefs.GetString(SAVE_KEY));
        else
            Progress = new MetaProgress();
    }

    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(Progress);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }
    
    public void AddEnlightenmentPoints(int amount) =>  Progress.enlightenmentPoints += amount;
    
    public int GetUpgradeCost(string upgradeId)
    {
        switch (upgradeId)
        {
            case "BonusHealth":
                return 10 + (Progress.bonusHealthLevel * 5);
            case "StartingGold": // --- 새로운 강화 비용 계산 추가 ---
                return 20 + (Progress.startingGoldLevel * 10);
            default:
                return 9999;
        }
    }
    
    public bool TryPurchaseUpgrade(string upgradeId, int cost)
    {
        if (Progress.enlightenmentPoints < cost) return false;

        Progress.enlightenmentPoints -= cost;
        switch (upgradeId)
        {
            case "BonusHealth":
                Progress.bonusHealthLevel++;
                break;
            case "StartingGold": // --- 새로운 강화 구매 로직 추가 ---
                Progress.startingGoldLevel++;
                break;
        }
        
        SaveProgress();
        return true;
    }
}