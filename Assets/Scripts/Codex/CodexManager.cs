using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JianghuGuidebook.Cards;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Data;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Events;

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
                            entry.acquisitionHint = "전투 보상, 상점, 또는 이벤트를 통해 획득 가능합니다.";
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
                            entry.acquisitionHint = "보물 상자, 엘리트 전투, 또는 이벤트를 통해 획득 가능합니다.";
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
                            entry.acquisitionHint = "강호를 여행하며 마주칠 수 있습니다.";
                            codexDict.Add(enemy.id, entry);
                        }
                    }
                }
            }

            // 4. Load Bosses
            TextAsset bossJson = Resources.Load<TextAsset>("BossDatabase");
            if (bossJson != null)
            {
                BossDatabase db = JsonUtility.FromJson<BossDatabase>(bossJson.text);
                if (db != null && db.bosses != null)
                {
                    foreach (var boss in db.bosses)
                    {
                        if (!codexDict.ContainsKey(boss.id))
                        {
                            CodexEntry entry = new CodexEntry(boss.id, CodexCategory.Boss);
                            entry.name = boss.enemyData.name;
                            entry.description = boss.enemyData.description;
                            entry.acquisitionHint = "각 지역의 마지막 관문에서 등장합니다.";
                            codexDict.Add(boss.id, entry);
                        }
                    }
                }
            }

            // 5. Load Events
            TextAsset eventJson = Resources.Load<TextAsset>("EventDatabase");
            if (eventJson != null)
            {
                EventDatabase db = JsonUtility.FromJson<EventDatabase>(eventJson.text);
                if (db != null && db.events != null)
                {
                    foreach (var evt in db.events)
                    {
                        if (!codexDict.ContainsKey(evt.id))
                        {
                            CodexEntry entry = new CodexEntry(evt.id, CodexCategory.Event);
                            entry.name = evt.title; // Use title as name
                            entry.description = evt.description;
                            entry.acquisitionHint = "여행 중 우연히 마주칠 수 있습니다.";
                            codexDict.Add(evt.id, entry);
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
