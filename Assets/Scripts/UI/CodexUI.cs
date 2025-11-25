using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Codex;

namespace JianghuGuidebook.UI
{
    public class CodexUI : MonoBehaviour
    {
        [Header("Category Tabs")]
        [SerializeField] private Button cardTab;
        [SerializeField] private Button relicTab;
        [SerializeField] private Button enemyTab;
        [SerializeField] private Button eventTab;

        [Header("Content")]
        [SerializeField] private Transform gridContainer;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private TextMeshProUGUI progressText;

        [Header("Detail View")]
        [SerializeField] private GameObject detailPanel;
        [SerializeField] private TextMeshProUGUI detailName;
        [SerializeField] private TextMeshProUGUI detailDescription;
        [SerializeField] private Image detailIcon;

        [Header("Navigation")]
        [SerializeField] private Button closeButton;

        private CodexCategory currentCategory = CodexCategory.Card;

        private void Start()
        {
            if (cardTab != null) cardTab.onClick.AddListener(() => SwitchCategory(CodexCategory.Card));
            if (relicTab != null) relicTab.onClick.AddListener(() => SwitchCategory(CodexCategory.Relic));
            if (enemyTab != null) enemyTab.onClick.AddListener(() => SwitchCategory(CodexCategory.Enemy));
            if (eventTab != null) eventTab.onClick.AddListener(() => SwitchCategory(CodexCategory.Event));
            
            if (closeButton != null) closeButton.onClick.AddListener(Close);

            // Hide detail initially
            if (detailPanel != null) detailPanel.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            SwitchCategory(CodexCategory.Card); // Default
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void SwitchCategory(CodexCategory category)
        {
            currentCategory = category;
            Refresh();
        }

        private void Refresh()
        {
            // Clear grid
            foreach (Transform child in gridContainer)
            {
                Destroy(child.gameObject);
            }

            if (CodexManager.Instance == null) return;

            List<CodexEntry> entries = CodexManager.Instance.GetEntries(currentCategory);
            (int current, int total) = CodexManager.Instance.GetProgress(currentCategory);

            if (progressText != null)
            {
                progressText.text = $"수집: {current} / {total} ({(total > 0 ? (float)current/total*100 : 0):F1}%)";
            }

            foreach (var entry in entries)
            {
                GameObject obj = Instantiate(slotPrefab, gridContainer);
                CodexSlotUI slot = obj.GetComponent<CodexSlotUI>();
                if (slot != null)
                {
                    slot.Setup(entry, OnSlotClicked);
                }
            }
        }

        private void OnSlotClicked(CodexEntry entry)
        {
            if (detailPanel != null)
            {
                detailPanel.SetActive(true);
                
                if (entry.isDiscovered)
                {
                    if (detailName != null) detailName.text = entry.name;
                    if (detailDescription != null) detailDescription.text = entry.description;
                    if (detailIcon != null) detailIcon.color = Color.white;
                }
                else
                {
                    if (detailName != null) detailName.text = "???";
                    if (detailDescription != null) detailDescription.text = "아직 발견하지 못했습니다.";
                    if (detailIcon != null) detailIcon.color = Color.black;
                }
            }
        }
    }
}
