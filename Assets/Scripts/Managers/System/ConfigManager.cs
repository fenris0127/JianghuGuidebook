using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Managers
{
    /// <summary>
    /// 모든 ScriptableObject Config를 중앙 집중식으로 관리하는 매니저
    /// Singleton 패턴으로 어디서나 접근 가능합니다.
    /// </summary>
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager Instance;

        [Header("게임 설정 파일")]
        [SerializeField] private GameBalanceConfig gameBalanceConfig;
        [SerializeField] private RealmConfig realmConfig;
        [SerializeField] private MapConfig mapConfig;
        [SerializeField] private BattleConfig battleConfig;

        public GameBalanceConfig GameBalance => gameBalanceConfig ?? LoadConfig(ref gameBalanceConfig, "Config/GameBalanceConfig");
        public RealmConfig Realm => realmConfig ?? LoadConfig(ref realmConfig, "Config/RealmConfig");
        public MapConfig Map => mapConfig ?? LoadConfig(ref mapConfig, "Config/MapConfig");
        public BattleConfig Battle => battleConfig ?? LoadConfig(ref battleConfig, "Config/BattleConfig");

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

        private T LoadConfig<T>(ref T config, string path) where T : ScriptableObject
        {
            config = Resources.Load<T>(path);
            if (config == null)
            {
                Debug.LogWarning($"{typeof(T).Name}을 {path}에서 찾을 수 없습니다. 기본값을 사용합니다.");
            }
            return config;
        }

        // 자주 사용되는 값들을 위한 편의 메서드
        public float GetVulnerableMultiplier() => GameBalance?.vulnerableDamageMultiplier ?? 1.5f;
        public float GetWeakMultiplier() => GameBalance?.weakDamageMultiplier ?? 0.75f;
    }
}
