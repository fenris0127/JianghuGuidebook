using UnityEngine;
using UnityEngine.UI;
using JianghuGuidebook.Faction;

namespace JianghuGuidebook.UI
{
    public class FactionSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Text nameText;
        [SerializeField] private Image iconImage; // Optional
        [SerializeField] private Button selectButton;
        [SerializeField] private GameObject selectedHighlight;

        private FactionData factionData;
        private System.Action<FactionData> onSelectCallback;

        public void Setup(FactionData data, System.Action<FactionData> onSelect)
        {
            factionData = data;
            onSelectCallback = onSelect;

            if (nameText != null)
            {
                nameText.text = data.name;
            }

            // TODO: Set icon if available

            if (selectButton != null)
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.onClick.AddListener(OnClicked);
            }

            SetSelected(false);
        }

        private void OnClicked()
        {
            onSelectCallback?.Invoke(factionData);
        }

        public void SetSelected(bool isSelected)
        {
            if (selectedHighlight != null)
            {
                selectedHighlight.SetActive(isSelected);
            }
        }
    }
}
