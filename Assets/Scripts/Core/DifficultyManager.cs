using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.Core
{
    public enum DifficultyLevel
    {
        Intro,          // 입문 (기본)
        Master,         // 고수
        Grandmaster,    // 초고수
        Supreme         // 절정고수
    }

    [System.Serializable]
    public struct DifficultyModifier
    {
        public float enemyHealthMultiplier;
        public float enemyDamageMultiplier;
        public float goldRewardMultiplier;
        public float essenceRewardMultiplier;
        public string description;

        public DifficultyModifier(float hp, float dmg, float gold, float essence, string desc)
        {
            enemyHealthMultiplier = hp;
            enemyDamageMultiplier = dmg;
            goldRewardMultiplier = gold;
            essenceRewardMultiplier = essence;
            description = desc;
        }
    }

    public class DifficultyManager : MonoBehaviour
    {
        private static DifficultyManager _instance;
        public static DifficultyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DifficultyManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("DifficultyManager");
                        _instance = go.AddComponent<DifficultyManager>();
                    }
                }
                return _instance;
            }
        }

        private Dictionary<DifficultyLevel, DifficultyModifier> modifiers;
        private DifficultyLevel selectedDifficulty = DifficultyLevel.Intro;

        public DifficultyLevel SelectedDifficulty 
        { 
            get => selectedDifficulty; 
            set => selectedDifficulty = value; 
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeModifiers();
        }

        private void InitializeModifiers()
        {
            modifiers = new Dictionary<DifficultyLevel, DifficultyModifier>();

            // 11.3.1 입문 (기본)
            modifiers.Add(DifficultyLevel.Intro, new DifficultyModifier(1.0f, 1.0f, 1.0f, 1.0f, "기본 난이도입니다. 적절한 도전과 보상이 주어집니다."));

            // 11.3.2 고수 (150%)
            modifiers.Add(DifficultyLevel.Master, new DifficultyModifier(1.5f, 1.2f, 1.2f, 1.5f, "적들이 더 강해지지만, 보상도 증가합니다."));

            // 11.3.3 초고수 (200%)
            modifiers.Add(DifficultyLevel.Grandmaster, new DifficultyModifier(2.0f, 1.5f, 1.5f, 2.0f, "극한의 도전입니다. 적들이 매우 강력합니다."));

            // 11.3.4 절정고수 (250%)
            modifiers.Add(DifficultyLevel.Supreme, new DifficultyModifier(2.5f, 2.0f, 2.0f, 3.0f, "무림의 정점에 도전하십시오."));
        }

        public DifficultyModifier GetModifier(DifficultyLevel level)
        {
            if (modifiers.ContainsKey(level))
            {
                return modifiers[level];
            }
            return modifiers[DifficultyLevel.Intro];
        }

        public DifficultyModifier GetCurrentModifier()
        {
            return GetModifier(selectedDifficulty);
        }

        public bool IsDifficultyUnlocked(DifficultyLevel level)
        {
            if (level == DifficultyLevel.Intro) return true;

            // SaveManager를 통해 해금 여부 확인 (TODO: SaveManager에 관련 로직 추가 필요)
            // 임시로 Intro만 해금된 상태로 가정하거나, PlayerPrefs 등을 사용할 수 있음
            // 여기서는 간단히 PlayerPrefs를 사용하여 구현 예시
            int unlockedLevel = PlayerPrefs.GetInt("UnlockedDifficulty", (int)DifficultyLevel.Intro);
            return (int)level <= unlockedLevel;
        }

        public void UnlockDifficulty(DifficultyLevel level)
        {
            int currentUnlocked = PlayerPrefs.GetInt("UnlockedDifficulty", (int)DifficultyLevel.Intro);
            if ((int)level > currentUnlocked)
            {
                PlayerPrefs.SetInt("UnlockedDifficulty", (int)level);
                PlayerPrefs.Save();
                Debug.Log($"난이도 해금: {level}");
            }
        }
    }
}
