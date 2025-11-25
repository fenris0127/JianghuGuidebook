using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JianghuGuidebook.Cards;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Codex
{
    public class CodexManager : MonoBehaviour
    {
        private static CodexManager _instance;

        public static CodexManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CodexManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CodexManager");
                        _instance = go.AddComponent<CodexManager>();
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, CodexEntry> codexDict = new Dictionary<string, CodexEntry>();
        private string saveFilePath;

        // Events
        public System.Action<CodexEntry> OnCodexUnlocked;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            saveFilePath = Path.Combine(Application.persistentDataPath, "codex.json");
            
            InitializeCodex();
            LoadProgress();
        }

        private void InitializeCodex()
        {
            // 1. Load Cards
            TextAsset cardJson = Resources.Load<TextAsset>("CardDatabase");
            if (cardJson != null)
            {
                CardDatabase db = JsonUtility.FromJson<CardDatabase>(cardJson.text);
                if (db != null && db.cards != null)
                {
                    foreach (var card in db.cards)
                    {
                        if (!codexDict.ContainsKey(card.id))
                        {
                            CodexEntry entry = new CodexEntry(card.id, CodexCategory.Card);
                            entry.name = card.name;
                            entry.description = card.description;
                            // entry.iconPath = ... 
                            codexDict.Add(card.id, entry);
                        }
                    }
                }
            }

            // 2. Load Relics
            TextAsset relicJson = Resources.Load<TextAsset>("RelicDatabase");
            if (relicJson != null)
            {
                RelicDatabase db = JsonUtility.FromJson<RelicDatabase>(relicJson.text);
                if (db != null && db.relics != null)
                {
                    foreach (var relic in db.relics)
                    {
                        if (!codexDict.ContainsKey(relic.id))
                        {
                            CodexEntry entry = new CodexEntry(relic.id, CodexCategory.Relic);
                            entry.name = relic.name;
                            entry.description = relic.description;
                            codexDict.Add(relic.id, entry);
                        }
                    }
                }
            }

            // 3. Load Enemies
            TextAsset enemyJson = Resources.Load<TextAsset>("EnemyDatabase");
            if (enemyJson != null)
            {
                EnemyDatabase db = JsonUtility.FromJson<EnemyDatabase>(enemyJson.text);
                if (db != null && db.enemies != null)
                {
                    foreach (var enemy in db.enemies)
                    {
                        if (!codexDict.ContainsKey(enemy.id))
                        {
                            CodexEntry entry = new CodexEntry(enemy.id, CodexCategory.Enemy);
                            entry.name = enemy.name;
                            entry.description = enemy.description;
                            codexDict.Add(enemy.id, entry);
                        }
                    }
                }
            }
            
            Debug.Log($"[CodexManager] Initialized with {codexDict.Count} entries.");
        }

        private void LoadProgress()
        {
            if (File.Exists(saveFilePath))
            {
                try
                {
                    string json = File.ReadAllText(saveFilePath);
                    CodexSaveData saveData = JsonUtility.FromJson<CodexSaveData>(json);
                    
                    if (saveData != null && saveData.discoveredEntries != null)
                    {
                        foreach (var savedInfo in saveData.discoveredEntries)
                        {
                            if (codexDict.ContainsKey(savedInfo.id))
                            {
                                codexDict[savedInfo.id].isDiscovered = true;
                                codexDict[savedInfo.id].firstDiscoveredDate = savedInfo.date;
                            }
                        }
                        Debug.Log($"[CodexManager] Loaded {saveData.discoveredEntries.Count} discovered entries.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[CodexManager] Failed to load codex progress: {e.Message}");
                }
            }
        }

        public void SaveProgress()
        {
            CodexSaveData saveData = new CodexSaveData();
            saveData.discoveredEntries = codexDict.Values
                .Where(e => e.isDiscovered)
                .Select(e => new CodexSaveInfo { id = e.id, date = e.firstDiscoveredDate })
                .ToList();

            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(saveFilePath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CodexManager] Failed to save codex progress: {e.Message}");
            }
        }

        public void Unlock(string id)
        {
            if (codexDict.ContainsKey(id))
            {
                CodexEntry entry = codexDict[id];
                if (!entry.isDiscovered)
                {
                    entry.Unlock();
                    Debug.Log($"[CodexManager] Discovered: {entry.name}");
                    OnCodexUnlocked?.Invoke(entry);
                    SaveProgress();
                    
                    // Check Collection Achievements
                    // AchievementManager.Instance?.CheckCollectionAchievements();
                }
            }
        }

        public List<CodexEntry> GetEntries(CodexCategory category)
        {
            return codexDict.Values.Where(e => e.category == category).ToList();
        }

        public (int current, int total) GetProgress(CodexCategory category)
        {
            var entries = GetEntries(category);
            return (entries.Count(e => e.isDiscovered), entries.Count);
        }
    }
}
