using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JianghuGuidebook.Achievement;

namespace JianghuGuidebook.UI
{
    public class AchievementUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Transform contentContainer;
        [SerializeField] private GameObject achievementSlotPrefab;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Text progressText;

        [Header("Filter Buttons")]
        [SerializeField] private Button allButton;
        [SerializeField] private Button combatButton;
        [SerializeField] private Button collectionButton;
        [SerializeField] private Button realmButton;
        [SerializeField] private Button specialButton;

        private AchievementType? currentFilter = null;

        private List<AchievementSlotUI> slots = new List<AchievementSlotUI>();

        private void Start()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }

            if (allButton != null) allButton.onClick.AddListener(() => SetFilter(null));
            if (combatButton != null) combatButton.onClick.AddListener(() => SetFilter(AchievementType.Combat));
            if (collectionButton != null) collectionButton.onClick.AddListener(() => SetFilter(AchievementType.Collection));
            if (realmButton != null) realmButton.onClick.AddListener(() => SetFilter(AchievementType.Realm));
            if (specialButton != null) specialButton.onClick.AddListener(() => SetFilter(AchievementType.Special));
        }

        public void Open()
        {
            gameObject.SetActive(true);
            SetFilter(null); // Default to All
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void SetFilter(AchievementType? type)
        {
            currentFilter = type;
            Refresh();
        }

        public void Refresh()
        {
            // Clear existing
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }
            slots.Clear();

            if (AchievementManager.Instance == null) return;

            List<Achievement.Achievement> achievements = AchievementManager.Instance.GetAllAchievements();
            int unlockedCount = 0;

            foreach (var ach in achievements)
            {
                if (currentFilter.HasValue && ach.type != currentFilter.Value) continue;

                GameObject obj = Instantiate(achievementSlotPrefab, contentContainer);
                AchievementSlotUI slot = obj.GetComponent<AchievementSlotUI>();
                if (slot != null)
                {
                    slot.Setup(ach);
                    slots.Add(slot);
                }

                if (ach.isUnlocked) unlockedCount++;
            }

            if (progressText != null)
            {
                progressText.text = $"달성률: {unlockedCount} / {achievements.Count} ({(float)unlockedCount / achievements.Count * 100:F1}%)";
            }
        }
    }
}
