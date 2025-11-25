using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Codex;

namespace JianghuGuidebook.UI
{
    public class CodexSlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject lockOverlay;
        [SerializeField] private Button button;

        private CodexEntry entry;
        private System.Action<CodexEntry> onClickCallback;

        private void Start()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        public void Setup(CodexEntry entry, System.Action<CodexEntry> onClick)
        {
            this.entry = entry;
            this.onClickCallback = onClick;

            if (entry.isDiscovered)
            {
                if (nameText != null) nameText.text = entry.name;
                if (lockOverlay != null) lockOverlay.SetActive(false);
                if (iconImage != null)
                {
                    iconImage.color = Color.white;
                    // TODO: Load actual icon
                    // iconImage.sprite = Resources.Load<Sprite>(entry.iconPath);
                }
            }
            else
            {
                if (nameText != null) nameText.text = "???";
                if (lockOverlay != null) lockOverlay.SetActive(true);
                if (iconImage != null) iconImage.color = Color.black; // Or gray
            }
        }

        private void OnClick()
        {
            onClickCallback?.Invoke(entry);
        }
    }
}
