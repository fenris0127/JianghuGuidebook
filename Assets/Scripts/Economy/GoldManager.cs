using UnityEngine;

namespace JianghuGuidebook.Economy
{
    /// <summary>
    /// 골드(재화)를 관리하는 매니저
    /// </summary>
    public class GoldManager : MonoBehaviour
    {
        private static GoldManager _instance;

        public static GoldManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GoldManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GoldManager");
                        _instance = go.AddComponent<GoldManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("골드")]
        [SerializeField] private int currentGold = 0;

        // Properties
        public int CurrentGold => currentGold;

        // Events
        public System.Action<int> OnGoldChanged;  // (current gold)
        public System.Action<int> OnGoldGained;   // (amount)
        public System.Action<int> OnGoldSpent;    // (amount)

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 골드를 획득합니다
        /// </summary>
        public void GainGold(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning("획득할 골드가 0 이하입니다");
                return;
            }

            currentGold += amount;
            Debug.Log($"골드 획득: +{amount} (현재: {currentGold})");

            OnGoldGained?.Invoke(amount);
            OnGoldChanged?.Invoke(currentGold);
        }

        /// <summary>
        /// 골드를 소비합니다
        /// </summary>
        public bool SpendGold(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning("소비할 골드가 0 이하입니다");
                return false;
            }

            if (currentGold < amount)
            {
                Debug.LogWarning($"골드 부족: 필요 {amount}, 현재 {currentGold}");
                return false;
            }

            currentGold -= amount;
            Debug.Log($"골드 소비: -{amount} (현재: {currentGold})");

            OnGoldSpent?.Invoke(amount);
            OnGoldChanged?.Invoke(currentGold);

            return true;
        }

        /// <summary>
        /// 골드가 충분한지 확인합니다
        /// </summary>
        public bool HasEnoughGold(int amount)
        {
            return currentGold >= amount;
        }

        /// <summary>
        /// 현재 골드를 설정합니다 (치트 또는 로드용)
        /// </summary>
        public void SetGold(int amount)
        {
            currentGold = Mathf.Max(0, amount);
            Debug.Log($"골드 설정: {currentGold}");
            OnGoldChanged?.Invoke(currentGold);
        }

        /// <summary>
        /// 골드를 초기화합니다
        /// </summary>
        public void ResetGold()
        {
            currentGold = 0;
            OnGoldChanged?.Invoke(currentGold);
            Debug.Log("골드 초기화 완료");
        }

        /// <summary>
        /// 시작 골드를 설정합니다
        /// </summary>
        public void InitializeGold(int startingGold = 100)
        {
            currentGold = startingGold;
            Debug.Log($"시작 골드: {currentGold}");
            OnGoldChanged?.Invoke(currentGold);
        }

        /// <summary>
        /// 전투 승리 보상 골드를 계산합니다
        /// </summary>
        public int CalculateCombatReward()
        {
            // 기본 40-60 골드 랜덤
            int baseReward = Random.Range(40, 61);

            // TODO: 유물 효과 적용 (골드 획득량 증가 등)
            // if (RelicManager.Instance.HasPassiveRelic("relic_lucky_coin"))
            // {
            //     baseReward = Mathf.RoundToInt(baseReward * 1.25f);
            // }

            return baseReward;
        }
    }
}
