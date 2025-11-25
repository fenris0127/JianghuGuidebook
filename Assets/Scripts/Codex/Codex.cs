using System;
using System.Collections.Generic;

namespace JianghuGuidebook.Codex
{
    public enum CodexCategory
    {
        Card,
        Relic,
        Enemy,
        Event,
        Boss
    }

    [Serializable]
    public class CodexEntry
    {
        public string id;
        public CodexCategory category;
        public bool isDiscovered;
        public string firstDiscoveredDate;
        
        // Runtime data (not saved)
        public string name;
        public string description;
        public string iconPath; // or Sprite reference
        
        public CodexEntry(string id, CodexCategory category)
        {
            this.id = id;
            this.category = category;
            this.isDiscovered = false;
            this.firstDiscoveredDate = "";
        }

        public void Unlock()
        {
            if (!isDiscovered)
            {
                isDiscovered = true;
                firstDiscoveredDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }

    [Serializable]
    public class CodexSaveData
    {
        public List<CodexSaveInfo> discoveredEntries;
    }

    [Serializable]
    public class CodexSaveInfo
    {
        public string id;
        public string date;
    }
}
