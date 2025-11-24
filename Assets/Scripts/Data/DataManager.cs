using UnityEngine;
using System.Collections.Generic;

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

        // 로드된 데이터
        private Dictionary<string, CardData> cardDictionary;
        private Dictionary<string, EnemyData> enemyDictionary;

        // 데이터베이스
        private CardDatabase cardDatabase;
        private EnemyDatabase enemyDatabase;

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

            IsDataLoaded = cardLoadSuccess && enemyLoadSuccess;

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
    }
}
