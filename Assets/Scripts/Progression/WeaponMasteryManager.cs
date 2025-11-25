using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 무기술 경지 시스템을 관리하는 매니저
    /// </summary>
    public class WeaponMasteryManager : MonoBehaviour
    {
        private static WeaponMasteryManager _instance;

        public static WeaponMasteryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<WeaponMasteryManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("WeaponMasteryManager");
                        _instance = go.AddComponent<WeaponMasteryManager>();
                    }
                }
                return _instance;
            }
        }

        // 각 무기별 현재 단계 (Key: WeaponType, Value: Stage)
        private Dictionary<WeaponType, int> currentStages = new Dictionary<WeaponType, int>();

        // 각 무기별 현재 경험치 (Key: WeaponType, Value: XP)
        private Dictionary<WeaponType, int> currentXP = new Dictionary<WeaponType, int>();

        // 마스터리 데이터베이스 (Key: WeaponType + Stage, Value: Data)
        private Dictionary<string, WeaponMasteryRealm> masteryDatabase = new Dictionary<string, WeaponMasteryRealm>();

        // Events
        public System.Action<WeaponType, int> OnMasteryLevelUp; // (Type, NewStage)
        public System.Action<WeaponType, int, int> OnXPChanged; // (Type, CurrentXP, RequiredXP)

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeDatabase();
            InitializeProgress();
        }

        private void InitializeDatabase()
        {
            // 예시 데이터 (실제로는 JSON에서 로드하거나 더 방대하게 설정)
            // 검 (Sword)
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Sword, 1, "검기상인", 1, "검 공격력 +1", 10));
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Sword, 2, "검기종횡", 2, "검 공격력 +2", 30));
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Sword, 3, "이기어검", 3, "검 공격력 +3", 60));
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Sword, 4, "무형검", 5, "검 공격력 +5", 100));
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Sword, 5, "심검", 8, "검 공격력 +8", 0));

            // 도 (Blade)
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Blade, 1, "도법입문", 1, "도 공격력 +1", 10));
            AddMasteryData(new WeaponMasteryRealm(WeaponType.Blade, 2, "광도", 2, "도 공격력 +2", 30));
            // ... 나머지 무기들도 유사하게 추가
        }

        private void AddMasteryData(WeaponMasteryRealm data)
        {
            string key = $"{data.weaponType}_{data.stage}";
            if (!masteryDatabase.ContainsKey(key))
            {
                masteryDatabase.Add(key, data);
            }
        }

        private void InitializeProgress()
        {
            foreach (WeaponType type in System.Enum.GetValues(typeof(WeaponType)))
            {
                if (type == WeaponType.None) continue;
                currentStages[type] = 0; // 0단계부터 시작
                currentXP[type] = 0;
            }
        }

        /// <summary>
        /// 무기 숙련도 경험치 획득
        /// </summary>
        public void AddMasteryXP(WeaponType type, int amount)
        {
            if (type == WeaponType.None) return;

            if (!currentXP.ContainsKey(type)) currentXP[type] = 0;
            if (!currentStages.ContainsKey(type)) currentStages[type] = 0;

            int currentStage = currentStages[type];
            
            // 다음 단계 데이터 확인
            string nextKey = $"{type}_{currentStage + 1}";
            if (!masteryDatabase.ContainsKey(nextKey)) return; // 이미 최고 단계

            WeaponMasteryRealm nextData = masteryDatabase[nextKey];
            
            currentXP[type] += amount;
            Debug.Log($"[{type}] 숙련도 증가: +{amount} (현재: {currentXP[type]}/{nextData.requiredXP})");
            
            OnXPChanged?.Invoke(type, currentXP[type], nextData.requiredXP);

            // 레벨업 체크
            if (currentXP[type] >= nextData.requiredXP)
            {
                LevelUp(type);
            }
        }

        private void LevelUp(WeaponType type)
        {
            currentStages[type]++;
            currentXP[type] = 0; // XP 리셋 (또는 초과분 이월)

            int newStage = currentStages[type];
            Debug.Log($"!!! [{type}] 경지 상승 !!! {newStage}단계 도달");

            OnMasteryLevelUp?.Invoke(type, newStage);
        }

        /// <summary>
        /// 현재 무기 타입의 추가 피해량을 반환
        /// </summary>
        public int GetDamageBonus(WeaponType type)
        {
            if (type == WeaponType.None) return 0;
            if (!currentStages.ContainsKey(type)) return 0;

            int stage = currentStages[type];
            if (stage == 0) return 0;

            string key = $"{type}_{stage}";
            if (masteryDatabase.TryGetValue(key, out WeaponMasteryRealm data))
            {
                return data.damageBonus;
            }

            return 0;
        }

        /// <summary>
        /// 현재 무기 타입의 경지 이름을 반환
        /// </summary>
        public string GetMasteryName(WeaponType type)
        {
            if (type == WeaponType.None) return "-";
            if (!currentStages.ContainsKey(type)) return "입문 전";

            int stage = currentStages[type];
            if (stage == 0) return "입문 전";

            string key = $"{type}_{stage}";
            if (masteryDatabase.TryGetValue(key, out WeaponMasteryRealm data))
            {
                return data.name;
            }

            return "알 수 없음";
        }
    }
}
