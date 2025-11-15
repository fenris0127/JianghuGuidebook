using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 모든 ScriptableObject 데이터 애셋을 미리 불러와 관리하고, 다른 스크립트가 쉽게 찾을 수 있도록 제공합니다.
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField] private ContentDatabase db;

    private Dictionary<string, CardData> cardDatabase;
    private Dictionary<string, RelicData> relicDatabase;
    private Dictionary<string, EncounterData> encounterDatabase;
    private Dictionary<string, EventData> eventDatabase;
    private Dictionary<string, StatusEffectData> statusEffectDatabase;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDatabases();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void BuildDatabases()
    {
        cardDatabase = db.allCards.Where(c => !string.IsNullOrEmpty(c.assetID)).ToDictionary(card => card.assetID, card => card);
        relicDatabase = db.allRelics.Where(r => !string.IsNullOrEmpty(r.assetID)).ToDictionary(relic => relic.assetID, relic => relic);
        encounterDatabase = db.allEncounters.ToDictionary(enc => enc.encounterName, enc => enc);
        eventDatabase = db.allEvents.ToDictionary(e => e.name, e => e);
        statusEffectDatabase = db.allStatusEffects.ToDictionary(se => se.assetID, se => se);
    }

    public List<CardData> GetAllCards() => db.allCards;
    public List<RelicData> GetAllRelics() => db.allRelics;
    public List<EventData> GetAllEvents() => db.allEvents;
    public List<EncounterData> GetAllEncounters() => db.allEncounters;
    public List<StatusEffectData> GetAllStatusEffects() => db.allStatusEffects;

    public CardData GetCardData(string id) { cardDatabase.TryGetValue(id, out CardData data); return data; }
    public RelicData GetRelicData(string id) { relicDatabase.TryGetValue(id, out RelicData data); return data; }
    public EncounterData GetEncounterData(string name) { encounterDatabase.TryGetValue(name, out EncounterData data); return data; }
    public EventData GetEventData(string name) { eventDatabase.TryGetValue(name, out EventData data); return data; }
    public StatusEffectData GetStatusEffectData(string id) { statusEffectDatabase.TryGetValue(id, out StatusEffectData data); return data;}
    
    
}