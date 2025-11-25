using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JianghuGuidebook.Faction;
using UnityEngine.SceneManagement;

namespace JianghuGuidebook.UI
{
    public class FactionSelectionUI : MonoBehaviour
    {
        [Header("List")]
        [SerializeField] private Transform factionListContainer;
        [SerializeField] private GameObject factionSlotPrefab;

        [Header("Details")]
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text passiveNameText;
        [SerializeField] private Text passiveDescText;
        [SerializeField] private Text relicNameText;
        [SerializeField] private Text deckCountText;

        [Header("Controls")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button backButton;

        private List<FactionSlotUI> slots = new List<FactionSlotUI>();
        private FactionData selectedFaction;

        // Event to notify MainMenuUI
        public System.Action<FactionData> OnFactionConfirmed;

        private void Start()
        {
            // Initialize is called by MainMenuUI
        }

        public void Initialize()
        {
            // Clear existing slots
            foreach (Transform child in factionListContainer)
            {
                Destroy(child.gameObject);
            }
            slots.Clear();

            if (FactionManager.Instance == null)
            {
                Debug.LogError("FactionManager instance not found!");
                return;
            }

            List<FactionData> factions = FactionManager.Instance.GetAllFactions();

            foreach (var faction in factions)
            {
                GameObject obj = Instantiate(factionSlotPrefab, factionListContainer);
                FactionSlotUI slot = obj.GetComponent<FactionSlotUI>();
                if (slot != null)
                {
                    slot.Setup(faction, OnFactionSelected);
                    slots.Add(slot);
                }
            }

            // Select first faction by default if available
            if (factions.Count > 0)
            {
                OnFactionSelected(factions[0]);
            }

            if (startButton != null)
            {
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(OnStartButtonClicked);
            }

            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(OnBackButtonClicked);
            }
        }

        private void OnFactionSelected(FactionData faction)
        {
            selectedFaction = faction;

            // Update UI
            if (nameText != null) nameText.text = faction.name;
            if (descriptionText != null) descriptionText.text = faction.description;
            
            if (passiveNameText != null) passiveNameText.text = "패시브 효과"; // Or specific name if added
            if (passiveDescText != null) passiveDescText.text = faction.passive.description;

            if (relicNameText != null) relicNameText.text = $"시작 유물: {faction.startingRelic}"; // Should resolve name from ID
            if (deckCountText != null) deckCountText.text = $"시작 덱: {faction.startingDeck.Count}장";

            // Update highlights
            foreach (var slot in slots)
            {
                // Assuming simple object comparison works for now, or compare IDs
                // FactionData doesn't override Equals, so reference comparison
                // But data comes from same list, so it should be fine.
                // Better to compare IDs if possible.
                slot.SetSelected(slot == null ? false : (faction.id == selectedFaction.id)); // Need to access ID inside slot or pass it back? 
                // Actually slot doesn't expose data. Let's fix logic.
                // Re-setup slot to expose ID or just rely on the callback context.
                // For now, let's just iterate and reset all, then highlight the one that matches ID.
            }
            
            // Correct highlight logic:
            // We need to know which slot holds this faction.
            // Since we don't have easy access back from data to slot without searching:
            int index = FactionManager.Instance.GetAllFactions().FindIndex(f => f.id == faction.id);
            if (index >= 0 && index < slots.Count)
            {
                for(int i=0; i<slots.Count; i++)
                {
                    slots[i].SetSelected(i == index);
                }
            }
        }

        private void OnStartButtonClicked()
        {
            if (selectedFaction == null) return;

            if (FactionManager.Instance.SelectFaction(selectedFaction.id))
            {
                // Notify MainMenuUI to proceed
                OnFactionConfirmed?.Invoke(selectedFaction);
                gameObject.SetActive(false);
            }
        }

        private void OnBackButtonClicked()
        {
            // Return to Main Menu
            gameObject.SetActive(false);
        }
    }
}
