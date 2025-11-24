using UnityEngine;

namespace JianghuGuidebook.Meta
{
    /// <summary>
    /// 무공 정수 시스템
    /// 게임 클리어/사망 시 획득하는 메타 화폐
    /// </summary>
    public class MugongEssence : MonoBehaviour
    {
        private static MugongEssence _instance;

        public static MugongEssence Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MugongEssence>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("MugongEssence");
                        _instance = go.AddComponent<MugongEssence>();
                    }
                }
                return _instance;
            }
        }

        [Header("무공 정수")]
        [SerializeField] private int totalEssence = 0;          // 누적 총 정수
        [SerializeField] private int currentEssence = 0;        // 현재 보유 정수 (소비 가능)

        // Properties
        public int TotalEssence => totalEssence;
        public int CurrentEssence => currentEssence;

        // Events
        public System.Action<int> OnEssenceGained;
        public System.Action<int> OnEssenceSpent;
        public System.Action<int, int> OnEssenceChanged; // (current, total)

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
        /// 무공 정수를 획득합니다
        /// </summary>
        public void GainEssence(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning($"무효한 정수 획득량: {amount}");
                return;
            }

            currentEssence += amount;
            totalEssence += amount;

            Debug.Log($"무공 정수 획득: +{amount} (현재: {currentEssence}, 누적: {totalEssence})");

            OnEssenceGained?.Invoke(amount);
            OnEssenceChanged?.Invoke(currentEssence, totalEssence);
        }

        /// <summary>
        /// 무공 정수를 소비합니다
        /// </summary>
        public bool SpendEssence(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning($"무효한 정수 소비량: {amount}");
                return false;
            }

            if (currentEssence < amount)
            {
                Debug.LogWarning($"무공 정수 부족: 필요 {amount}, 현재 {currentEssence}");
                return false;
            }

            currentEssence -= amount;

            Debug.Log($"무공 정수 소비: -{amount} (남은 정수: {currentEssence})");

            OnEssenceSpent?.Invoke(amount);
            OnEssenceChanged?.Invoke(currentEssence, totalEssence);

            return true;
        }

        /// <summary>
        /// 충분한 정수를 보유하고 있는지 확인합니다
        /// </summary>
        public bool HasEnoughEssence(int amount)
        {
            return currentEssence >= amount;
        }

        /// <summary>
        /// 런 결과에 따라 무공 정수를 계산하고 획득합니다
        /// </summary>
        public int CalculateAndGainEssence(RunResult result)
        {
            int essenceGained = CalculateEssenceFromRun(result);
            GainEssence(essenceGained);
            return essenceGained;
        }

        /// <summary>
        /// 런 결과로부터 획득할 무공 정수를 계산합니다
        /// </summary>
        private int CalculateEssenceFromRun(RunResult result)
        {
            int essence = 0;

            // 1. 도달한 지역 × 10
            essence += result.regionsReached * 10;

            // 2. 처치한 적 수 × 2
            essence += result.enemiesKilled * 2;

            // 3. 수집한 카드 수 × 1
            essence += result.cardsCollected * 1;

            // 4. 획득한 유물 수 × 5
            essence += result.relicsCollected * 5;

            // 5. 보스 격파 × 20
            essence += result.bossesDefeated * 20;

            // 6. 클리어 보너스
            if (result.isVictory)
            {
                essence += 50; // 클리어 보너스
            }
            else
            {
                // 사망 시에도 최소 정수 보장
                essence = Mathf.Max(essence, 5);
            }

            Debug.Log($"=== 무공 정수 계산 ===");
            Debug.Log($"지역 도달: {result.regionsReached} × 10 = {result.regionsReached * 10}");
            Debug.Log($"적 처치: {result.enemiesKilled} × 2 = {result.enemiesKilled * 2}");
            Debug.Log($"카드 수집: {result.cardsCollected} × 1 = {result.cardsCollected * 1}");
            Debug.Log($"유물 획득: {result.relicsCollected} × 5 = {result.relicsCollected * 5}");
            Debug.Log($"보스 격파: {result.bossesDefeated} × 20 = {result.bossesDefeated * 20}");
            Debug.Log($"총 획득 정수: {essence}");

            return essence;
        }

        /// <summary>
        /// 정수를 직접 설정합니다 (세이브/로드용)
        /// </summary>
        public void SetEssence(int current, int total)
        {
            currentEssence = current;
            totalEssence = total;

            Debug.Log($"무공 정수 설정: 현재 {currentEssence}, 누적 {totalEssence}");

            OnEssenceChanged?.Invoke(currentEssence, totalEssence);
        }

        /// <summary>
        /// 정수를 초기화합니다
        /// </summary>
        public void ResetEssence()
        {
            currentEssence = 0;
            totalEssence = 0;

            Debug.Log("무공 정수 초기화");

            OnEssenceChanged?.Invoke(currentEssence, totalEssence);
        }
    }

    /// <summary>
    /// 런 결과 데이터
    /// </summary>
    [System.Serializable]
    public class RunResult
    {
        public bool isVictory;          // 승리 여부
        public int regionsReached;      // 도달한 지역 수
        public int enemiesKilled;       // 처치한 적 수
        public int cardsCollected;      // 수집한 카드 수
        public int relicsCollected;     // 획득한 유물 수
        public int bossesDefeated;      // 격파한 보스 수

        public RunResult()
        {
            isVictory = false;
            regionsReached = 0;
            enemiesKilled = 0;
            cardsCollected = 0;
            relicsCollected = 0;
            bossesDefeated = 0;
        }

        public override string ToString()
        {
            return $"RunResult: {(isVictory ? "승리" : "패배")}, 지역 {regionsReached}, 적 {enemiesKilled}, 카드 {cardsCollected}, 유물 {relicsCollected}, 보스 {bossesDefeated}";
        }
    }
}
