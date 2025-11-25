using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Events;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.Data
{
    /// <summary>
    /// 게임 데이터를 로드하고 관리하는 매니저
    /// JSON 파일에서 카드 및 적 데이터를 불러옵니다
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        private static DataManager _instance;

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DataManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("DataManager");
                        _instance = go.AddComponent<DataManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("데이터 파일 경로")]
        [SerializeField] private string cardDatabasePath = "CardDatabase";
        [SerializeField] private string enemyDatabasePath = "EnemyDatabase";
        [SerializeField] private string eventDatabasePath = "EventDatabase";
        [SerializeField] private string bossDatabasePath = "BossDatabase";
        [SerializeField] private string upgradeDatabasePath = "UpgradeDatabase";
        [SerializeField] private string factionDatabasePath = "FactionDatabase";

        // 로드된 데이터
        private Dictionary<string, CardData> cardDictionary;
        private Dictionary<string, EnemyData> enemyDictionary;
        private Dictionary<string, EventData> eventDictionary;
        private Dictionary<string, BossData> bossDictionary;
        private Dictionary<string, PermanentUpgrade> upgradeDictionary;
        private Dictionary<string, FactionData> factionDictionary;

        // 데이터베이스
        private CardDatabase cardDatabase;
        private EnemyDatabase enemyDatabase;
        private EventDatabase eventDatabase;
        private BossDatabase bossDatabase;
        private UpgradeDatabase upgradeDatabase;
        private FactionDatabase factionDatabase;

        // 로드 상태
        public bool IsDataLoaded { get; private set; } = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadAllData();
        }

        /// <summary>
        /// 모든 게임 데이터를 로드합니다
        /// </summary>
        public void LoadAllData()
        {
            Debug.Log("=== 게임 데이터 로딩 시작 ===");

            bool cardLoadSuccess = LoadCardData();
            bool enemyLoadSuccess = LoadEnemyData();
            bool eventLoadSuccess = LoadEventData();
            bool bossLoadSuccess = LoadBossData();
            bool upgradeLoadSuccess = LoadUpgradeData();
            bool factionLoadSuccess = LoadFactionData();

            IsDataLoaded = cardLoadSuccess && enemyLoadSuccess && eventLoadSuccess && bossLoadSuccess && upgradeLoadSuccess && factionLoadSuccess;

            if (IsDataLoaded)
            {
                Debug.Log("=== 게임 데이터 로딩 완료 ===");
            }
            else
            {
                Debug.LogError("=== 게임 데이터 로딩 실패 ===");
            }
        }

        /// <summary>
        /// 카드 데이터베이스를 로드합니다
        /// </summary>
        private bool LoadCardData()
        {
            try
            {
                // Resources 폴더에서 JSON 파일 로드
                TextAsset jsonFile = Resources.Load<TextAsset>(cardDatabasePath);

                if (jsonFile == null)
                {
                    Debug.LogError($"카드 데이터 파일을 찾을 수 없습니다: Resources/{cardDatabasePath}");
                    Debug.LogError("Assets/Resources/CardDatabase.json 파일이 존재하는지 확인하세요");
                    return false;
                }

                // JSON 역직렬화
                cardDatabase = JsonUtility.FromJson<CardDatabase>(jsonFile.text);

                if (cardDatabase == null || cardDatabase.cards == null)
                {
                    Debug.LogError("카드 데이터베이스 역직렬화 실패");
                    return false;
                }

                // Dictionary 생성
                cardDictionary = new Dictionary<string, CardData>();

                int validCount = 0;
                int invalidCount = 0;

                foreach (var card in cardDatabase.cards)
                {
                    if (card.IsValid())
                    {
                        if (!cardDictionary.ContainsKey(card.id))
                        {
                            cardDictionary.Add(card.id, card);
                            validCount++;
                        }
                        else
                        {
                            Debug.LogWarning($"중복된 카드 ID: {card.id}");
                            invalidCount++;
                        }
                    }
                    else
                    {
                        invalidCount++;
                    }
                }

                Debug.Log($"카드 데이터 로드 완료: {validCount}개 성공, {invalidCount}개 실패");
                return validCount > 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"카드 데이터 로드 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 적 데이터베이스를 로드합니다
        /// </summary>
        private bool LoadEnemyData()
        {
            try
            {
                // Resources 폴더에서 JSON 파일 로드
                TextAsset jsonFile = Resources.Load<TextAsset>(enemyDatabasePath);

                if (jsonFile == null)
                {
                    Debug.LogError($"적 데이터 파일을 찾을 수 없습니다: Resources/{enemyDatabasePath}");
                    Debug.LogError("Assets/Resources/EnemyDatabase.json 파일이 존재하는지 확인하세요");
                    return false;
                }

                // JSON 역직렬화
                enemyDatabase = JsonUtility.FromJson<EnemyDatabase>(jsonFile.text);

                if (enemyDatabase == null || enemyDatabase.enemies == null)
                {
                    Debug.LogError("적 데이터베이스 역직렬화 실패");
                    return false;
                }

                // Dictionary 생성
                enemyDictionary = new Dictionary<string, EnemyData>();

                int validCount = 0;
                int invalidCount = 0;

                foreach (var enemy in enemyDatabase.enemies)
                {
                    if (enemy.IsValid())
                    {
                        if (!enemyDictionary.ContainsKey(enemy.id))
                        {
                            enemyDictionary.Add(enemy.id, enemy);
                            validCount++;
                        }
                        else
                        {
                            Debug.LogWarning($"중복된 적 ID: {enemy.id}");
                            invalidCount++;
                        }
                    }
                    else
                    {
                        invalidCount++;
                    }
                }

                Debug.Log($"적 데이터 로드 완료: {validCount}개 성공, {invalidCount}개 실패");
                return validCount > 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"적 데이터 로드 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 이벤트 데이터베이스를 로드합니다
        /// </summary>
        private bool LoadEventData()
        {
            try
            {
                // Resources 폴더에서 JSON 파일 로드
                TextAsset jsonFile = Resources.Load<TextAsset>(eventDatabasePath);

                if (jsonFile == null)
                {
                    Debug.LogError($"이벤트 데이터 파일을 찾을 수 없습니다: Resources/{eventDatabasePath}");
                    Debug.LogError("Assets/Resources/EventDatabase.json 파일이 존재하는지 확인하세요");
                    return false;
                }

                // JSON 역직렬화
                eventDatabase = JsonUtility.FromJson<EventDatabase>(jsonFile.text);

                if (eventDatabase == null || eventDatabase.events == null)
                {
                    Debug.LogError("이벤트 데이터베이스 역직렬화 실패");
                    return false;
                }

                // Dictionary 생성
                eventDictionary = new Dictionary<string, EventData>();

                int validCount = 0;
                int invalidCount = 0;

                foreach (var eventData in eventDatabase.events)
                {
                    if (eventData.Validate())
                    {
                        if (!eventDictionary.ContainsKey(eventData.id))
                        {
                            eventDictionary.Add(eventData.id, eventData);
                            validCount++;
                        }
                        else
                        {
                            Debug.LogWarning($"중복된 이벤트 ID: {eventData.id}");
                            invalidCount++;
                        }
                    }
                    else
                    {
                        invalidCount++;
                    }
                }

                Debug.Log($"이벤트 데이터 로드 완료: {validCount}개 성공, {invalidCount}개 실패");
                return validCount > 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"이벤트 데이터 로드 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 보스 데이터베이스를 로드합니다
        /// </summary>
        private bool LoadBossData()
        {
            try
            {
                // Resources 폴더에서 JSON 파일 로드
                TextAsset jsonFile = Resources.Load<TextAsset>(bossDatabasePath);

                if (jsonFile == null)
                {
                    Debug.LogError($"보스 데이터 파일을 찾을 수 없습니다: Resources/{bossDatabasePath}");
                    Debug.LogError("Assets/Resources/BossDatabase.json 파일이 존재하는지 확인하세요");
                    return false;
                }

                // JSON 역직렬화
                bossDatabase = JsonUtility.FromJson<BossDatabase>(jsonFile.text);

                if (bossDatabase == null || bossDatabase.bosses == null)
                {
                    Debug.LogError("보스 데이터베이스 역직렬화 실패");
                    return false;
                }

                // Dictionary 생성
                bossDictionary = new Dictionary<string, BossData>();

                int validCount = 0;
                int invalidCount = 0;

                foreach (var bossData in bossDatabase.bosses)
                {
                    if (bossData.IsValid())
                    {
                        if (!bossDictionary.ContainsKey(bossData.id))
                        {
                            bossDictionary.Add(bossData.id, bossData);
                            validCount++;
                        }
                        else
                        {
                            Debug.LogWarning($"중복된 보스 ID: {bossData.id}");
                            invalidCount++;
                        }
                    }
                    else
                    {
                        invalidCount++;
                    }
                }

                Debug.Log($"보스 데이터 로드 완료: {validCount}개 성공, {invalidCount}개 실패");
                return validCount > 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"보스 데이터 로드 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 업그레이드 데이터베이스를 로드합니다
        /// </summary>
        private bool LoadUpgradeData()
        {
            try
            {
                // Resources 폴더에서 JSON 파일 로드
                TextAsset jsonFile = Resources.Load<TextAsset>(upgradeDatabasePath);

                if (jsonFile == null)
                {
                    Debug.LogError($"업그레이드 데이터 파일을 찾을 수 없습니다: Resources/{upgradeDatabasePath}");
                    Debug.LogError("Assets/Resources/UpgradeDatabase.json 파일이 존재하는지 확인하세요");
                    return false;
                }

                // JSON 역직렬화
                upgradeDatabase = JsonUtility.FromJson<UpgradeDatabase>(jsonFile.text);

                if (upgradeDatabase == null || upgradeDatabase.upgrades == null)
                {
                    Debug.LogError("업그레이드 데이터베이스 역직렬화 실패");
                    return false;
                }

                // Dictionary 생성
                upgradeDictionary = new Dictionary<string, PermanentUpgrade>();

                int validCount = 0;
                int invalidCount = 0;

                foreach (var upgrade in upgradeDatabase.upgrades)
                {
                    if (upgrade.Validate())
                    {
                        if (!upgradeDictionary.ContainsKey(upgrade.id))
                        {
                            upgradeDictionary.Add(upgrade.id, upgrade);
                            validCount++;
                        }
                        else
                        {
                            Debug.LogWarning($"중복된 업그레이드 ID: {upgrade.id}");
                            invalidCount++;
                        }
                    }
                    else
                    {
                        invalidCount++;
                    }
                }

                Debug.Log($"업그레이드 데이터 로드 완료: {validCount}개 성공, {invalidCount}개 실패");
                return validCount > 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"업그레이드 데이터 로드 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        // ===== Public API =====

        /// <summary>
        /// ID로 카드 데이터를 가져옵니다
        /// </summary>
        public CardData GetCardData(string cardId)
        {
            if (cardDictionary == null)
            {
                Debug.LogError("카드 데이터가 로드되지 않았습니다");
                return null;
            }

            if (cardDictionary.TryGetValue(cardId, out CardData card))
            {
                return card;
            }

            Debug.LogWarning($"카드를 찾을 수 없습니다: {cardId}");
            return null;
        }

        /// <summary>
        /// ID로 적 데이터를 가져옵니다
        /// </summary>
        public EnemyData GetEnemyData(string enemyId)
        {
            if (enemyDictionary == null)
            {
                Debug.LogError("적 데이터가 로드되지 않았습니다");
                return null;
            }

            if (enemyDictionary.TryGetValue(enemyId, out EnemyData enemy))
            {
                return enemy;
            }

            Debug.LogWarning($"적을 찾을 수 없습니다: {enemyId}");
            return null;
        }

        /// <summary>
        /// 모든 카드 데이터를 가져옵니다
        /// </summary>
        public CardData[] GetAllCards()
        {
            if (cardDatabase == null || cardDatabase.cards == null)
            {
                Debug.LogError("카드 데이터가 로드되지 않았습니다");
                return new CardData[0];
            }

            return cardDatabase.cards;
        }

        /// <summary>
        /// 모든 적 데이터를 가져옵니다
        /// </summary>
        public EnemyData[] GetAllEnemies()
        {
            if (enemyDatabase == null || enemyDatabase.enemies == null)
            {
                Debug.LogError("적 데이터가 로드되지 않았습니다");
                return new EnemyData[0];
            }

            return enemyDatabase.enemies;
        }

        /// <summary>
        /// 타입별로 카드를 필터링하여 가져옵니다
        /// </summary>
        public CardData[] GetCardsByType(CardType type)
        {
            var result = new List<CardData>();

            foreach (var card in cardDatabase.cards)
            {
                if (card.type == type)
                {
                    result.Add(card);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 희귀도별로 카드를 필터링하여 가져옵니다
        /// </summary>
        public CardData[] GetCardsByRarity(CardRarity rarity)
        {
            var result = new List<CardData>();

            foreach (var card in cardDatabase.cards)
            {
                if (card.rarity == rarity)
                {
                    result.Add(card);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// ID로 이벤트 데이터를 가져옵니다
        /// </summary>
        public EventData GetEventById(string eventId)
        {
            if (eventDictionary == null)
            {
                Debug.LogError("이벤트 데이터가 로드되지 않았습니다");
                return null;
            }

            if (eventDictionary.TryGetValue(eventId, out EventData eventData))
            {
                return eventData;
            }

            Debug.LogWarning($"이벤트를 찾을 수 없습니다: {eventId}");
            return null;
        }

        /// <summary>
        /// 모든 이벤트 데이터를 가져옵니다
        /// </summary>
        public EventData[] GetAllEvents()
        {
            if (eventDatabase == null || eventDatabase.events == null)
            {
                Debug.LogError("이벤트 데이터가 로드되지 않았습니다");
                return new EventData[0];
            }

            return eventDatabase.events.ToArray();
        }

        /// <summary>
        /// ID로 보스 데이터를 가져옵니다
        /// </summary>
        public BossData GetBossById(string bossId)
        {
            if (bossDictionary == null)
            {
                Debug.LogError("보스 데이터가 로드되지 않았습니다");
                return null;
            }

            if (bossDictionary.TryGetValue(bossId, out BossData bossData))
            {
                return bossData;
            }

            Debug.LogWarning($"보스를 찾을 수 없습니다: {bossId}");
            return null;
        }

        /// <summary>
        /// 모든 보스 데이터를 가져옵니다
        /// </summary>
        public BossData[] GetAllBosses()
        {
            if (bossDatabase == null || bossDatabase.bosses == null)
            {
                Debug.LogError("보스 데이터가 로드되지 않았습니다");
                return new BossData[0];
            }

            return bossDatabase.bosses.ToArray();
        }

        /// <summary>
        /// ID로 업그레이드 데이터를 가져옵니다
        /// </summary>
        public PermanentUpgrade GetUpgradeById(string upgradeId)
        {
            if (upgradeDictionary == null)
            {
                Debug.LogError("업그레이드 데이터가 로드되지 않았습니다");
                return null;
            }

            if (upgradeDictionary.TryGetValue(upgradeId, out PermanentUpgrade upgrade))
            {
                return upgrade;
            }

            Debug.LogWarning($"업그레이드를 찾을 수 없습니다: {upgradeId}");
            return null;
        }

        /// <summary>
        /// 모든 업그레이드 데이터를 가져옵니다
        /// </summary>
        public PermanentUpgrade[] GetAllUpgrades()
        {
            if (upgradeDatabase == null || upgradeDatabase.upgrades == null)
            {
                Debug.LogError("업그레이드 데이터가 로드되지 않았습니다");
                return new PermanentUpgrade[0];
            }

            return upgradeDatabase.upgrades.ToArray();
        }

        // ===== 분파 데이터 =====

        /// <summary>
        /// 분파 데이터베이스를 로드합니다
        /// </summary>
        private bool LoadFactionData()
        {
            Debug.Log("분파 데이터베이스 로딩 시작...");

            TextAsset jsonFile = Resources.Load<TextAsset>(factionDatabasePath);

            if (jsonFile == null)
            {
                Debug.LogError($"분파 데이터베이스 파일을 찾을 수 없습니다: {factionDatabasePath}");
                return false;
            }

            try
            {
                factionDatabase = JsonUtility.FromJson<FactionDatabase>(jsonFile.text);

                if (factionDatabase == null || factionDatabase.factions == null)
                {
                    Debug.LogError("분파 데이터베이스 역직렬화 실패");
                    return false;
                }

                // Dictionary 생성
                factionDictionary = new Dictionary<string, FactionData>();

                foreach (var faction in factionDatabase.factions)
                {
                    if (!faction.Validate())
                    {
                        Debug.LogWarning($"분파 데이터 검증 실패: {faction.id}");
                        continue;
                    }

                    factionDictionary[faction.id] = faction;
                }

                Debug.Log($"분파 데이터 로드 완료: {factionDictionary.Count}개");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"분파 데이터 로드 중 오류 발생: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// ID로 분파 데이터를 가져옵니다
        /// </summary>
        public FactionData GetFactionById(string factionId)
        {
            if (factionDictionary == null)
            {
                Debug.LogError("분파 데이터가 로드되지 않았습니다");
                return null;
            }

            if (factionDictionary.TryGetValue(factionId, out FactionData faction))
            {
                return faction;
            }

            Debug.LogWarning($"분파를 찾을 수 없습니다: {factionId}");
            return null;
        }

        /// <summary>
        /// 모든 분파 데이터를 가져옵니다
        /// </summary>
        public FactionData[] GetAllFactions()
        {
            if (factionDatabase == null || factionDatabase.factions == null)
            {
                Debug.LogError("분파 데이터가 로드되지 않았습니다");
                return new FactionData[0];
            }

            return factionDatabase.factions;
        }
    }
}
