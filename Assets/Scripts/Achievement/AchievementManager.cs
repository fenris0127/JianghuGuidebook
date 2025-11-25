using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.Achievement
{
    public class AchievementManager : MonoBehaviour
    {
        private static AchievementManager _instance;

        public static AchievementManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AchievementManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("AchievementManager");
                        _instance = go.AddComponent<AchievementManager>();
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, Achievement> achievementDict = new Dictionary<string, Achievement>();
        private string saveFilePath;

        // Events
        public System.Action<Achievement> OnAchievementUnlocked;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            saveFilePath = Path.Combine(Application.persistentDataPath, "achievements.json");
            
            LoadDatabase();
            LoadProgress();
        }

        private void LoadDatabase()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("AchievementDatabase");
            if (jsonFile != null)
            {
                AchievementDatabase db = JsonUtility.FromJson<AchievementDatabase>(jsonFile.text);
                if (db != null && db.achievements != null)
                {
                    foreach (var achievement in db.achievements)
                    {
                        if (!achievementDict.ContainsKey(achievement.id))
                        {
                            achievementDict.Add(achievement.id, achievement);
                        }
                    }
                    Debug.Log($"[AchievementManager] {achievementDict.Count} achievements loaded from database.");
                }
            }
            else
            {
                Debug.LogWarning("[AchievementManager] AchievementDatabase.json not found in Resources.");
            }
        }

        private void LoadProgress()
        {
            if (File.Exists(saveFilePath))
            {
                try
                {
                    string json = File.ReadAllText(saveFilePath);
                    AchievementSaveData saveData = JsonUtility.FromJson<AchievementSaveData>(json);
                    
                    if (saveData != null && saveData.unlockedAchievements != null)
                    {
                        foreach (var savedAch in saveData.unlockedAchievements)
                        {
                            if (achievementDict.ContainsKey(savedAch.id))
                            {
                                achievementDict[savedAch.id].isUnlocked = true;
                                achievementDict[savedAch.id].unlockedDate = savedAch.unlockedDate;
                                achievementDict[savedAch.id].currentProgress = savedAch.currentProgress;
                            }
                        }
                        Debug.Log($"[AchievementManager] Loaded progress for {saveData.unlockedAchievements.Count} achievements.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[AchievementManager] Failed to load achievement progress: {e.Message}");
                }
            }
        }

        public void SaveProgress()
        {
            AchievementSaveData saveData = new AchievementSaveData();
            // Save all achievements that are unlocked OR have progress > 0
            saveData.unlockedAchievements = achievementDict.Values
                .Where(a => a.isUnlocked || a.currentProgress > 0)
                .Select(a => new AchievementSaveInfo { 
                    id = a.id, 
                    unlockedDate = a.unlockedDate,
                    currentProgress = a.currentProgress
                })
                .ToList();

            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(saveFilePath, json);
                Debug.Log("[AchievementManager] Achievement progress saved.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AchievementManager] Failed to save achievement progress: {e.Message}");
            }
        }

        public void UnlockAchievement(string id)
        {
            if (achievementDict.ContainsKey(id))
            {
                Achievement achievement = achievementDict[id];
                if (!achievement.isUnlocked)
                {
                    achievement.Unlock();
                    Debug.Log($"[AchievementManager] Unlocked: {achievement.name}");
                    
                    OnAchievementUnlocked?.Invoke(achievement);
                    
                    // Give Reward
                    GiveReward(achievement);

                    // Show UI notification (TODO)
                    
                    // Show UI notification (TODO)
                    // NotificationManager.Instance.ShowAchievement(achievement);

                    SaveProgress();
                }
            }
            else
            {
                Debug.LogWarning($"[AchievementManager] Achievement ID not found: {id}");
            }
        }

        private void GiveReward(Achievement achievement)
        {
            if (achievement.rewardType == AchievementRewardType.None) return;

            switch (achievement.rewardType)
            {
                case AchievementRewardType.Essence:
                    if (int.TryParse(achievement.rewardValue, out int amount))
                    {
                        MugongEssence.Instance?.GainEssence(amount);
                        Debug.Log($"[Achievement] Reward: {amount} Essence");
                    }
                    break;
                
                case AchievementRewardType.UnlockRelic:
                    // TODO: Implement Relic Unlock
                    Debug.Log($"[Achievement] Reward: Unlock Relic {achievement.rewardValue}");
                    break;

                case AchievementRewardType.UnlockCard:
                    // TODO: Implement Card Unlock
                    Debug.Log($"[Achievement] Reward: Unlock Card {achievement.rewardValue}");
                    break;
            }
        }

        public void IncrementProgress(string id, int amount)
        {
            if (achievementDict.ContainsKey(id))
            {
                Achievement achievement = achievementDict[id];
                if (!achievement.isUnlocked)
                {
                    achievement.currentProgress += amount;
                    if (achievement.targetProgress > 0 && achievement.currentProgress >= achievement.targetProgress)
                    {
                        UnlockAchievement(id);
                    }
                    else
                    {
                        SaveProgress(); // Save progress updates
                    }
                }
            }
        }

        public void SetProgress(string id, int value)
        {
            if (achievementDict.ContainsKey(id))
            {
                Achievement achievement = achievementDict[id];
                if (!achievement.isUnlocked)
                {
                    achievement.currentProgress = value;
                    if (achievement.targetProgress > 0 && achievement.currentProgress >= achievement.targetProgress)
                    {
                        UnlockAchievement(id);
                    }
                    else
                    {
                        SaveProgress();
                    }
                }
            }
        }

        public List<Achievement> GetAllAchievements()
        {
            return achievementDict.Values.ToList();
        }

        public Achievement GetAchievement(string id)
        {
            if (achievementDict.ContainsKey(id))
                return achievementDict[id];
            return null;
        }
        
        // Helper to reset all achievements (Debug)
        public void ResetAllAchievements()
        {
            foreach(var ach in achievementDict.Values)
            {
                ach.isUnlocked = false;
                ach.unlockedDate = "";
            }
            if(File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
            Debug.Log("[AchievementManager] All achievements reset.");
        }
    }

    [System.Serializable]
    public class AchievementSaveData
    {
        public List<AchievementSaveInfo> unlockedAchievements;
    }

    [System.Serializable]
    public class AchievementSaveInfo
    {
        public string id;
        public string unlockedDate;
        public int currentProgress;
    }
}
