using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Core;
using System;

namespace JianghuGuidebook.UI
{
    public class DifficultyUI : MonoBehaviour
    {
        [Header("Difficulty Buttons")]
        [SerializeField] private Button introButton;
        [SerializeField] private Button masterButton;
        [SerializeField] private Button grandmasterButton;
        [SerializeField] private Button supremeButton;

        [Header("Info Display")]
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI modifierText;

        [Header("Control")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button backButton;

        public Action OnDifficultyConfirmed;
        public Action OnBack;

        private void Start()
        {
            if (introButton != null) introButton.onClick.AddListener(() => SelectDifficulty(DifficultyLevel.Intro));
            if (masterButton != null) masterButton.onClick.AddListener(() => SelectDifficulty(DifficultyLevel.Master));
            if (grandmasterButton != null) grandmasterButton.onClick.AddListener(() => SelectDifficulty(DifficultyLevel.Grandmaster));
            if (supremeButton != null) supremeButton.onClick.AddListener(() => SelectDifficulty(DifficultyLevel.Supreme));

            if (startButton != null) startButton.onClick.AddListener(ConfirmDifficulty);
            if (backButton != null) backButton.onClick.AddListener(GoBack);

            // Default selection
            SelectDifficulty(DifficultyLevel.Intro);
        }

        private void OnEnable()
        {
            UpdateButtonsState();
            SelectDifficulty(DifficultyManager.Instance.SelectedDifficulty);
        }

        private void UpdateButtonsState()
        {
            if (DifficultyManager.Instance == null) return;

            UpdateButton(introButton, DifficultyLevel.Intro);
            UpdateButton(masterButton, DifficultyLevel.Master);
            UpdateButton(grandmasterButton, DifficultyLevel.Grandmaster);
            UpdateButton(supremeButton, DifficultyLevel.Supreme);
        }

        private void UpdateButton(Button btn, DifficultyLevel level)
        {
            if (btn == null) return;

            bool isUnlocked = DifficultyManager.Instance.IsDifficultyUnlocked(level);
            btn.interactable = isUnlocked;
            
            // Optional: Change visual if locked (e.g., lock icon)
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                if (!isUnlocked) btnText.text = "Locked";
            }
        }

        private void SelectDifficulty(DifficultyLevel level)
        {
            if (DifficultyManager.Instance == null) return;
            if (!DifficultyManager.Instance.IsDifficultyUnlocked(level)) return;

            DifficultyManager.Instance.SelectedDifficulty = level;
            UpdateInfoDisplay(level);
        }

        private void UpdateInfoDisplay(DifficultyLevel level)
        {
            DifficultyModifier mod = DifficultyManager.Instance.GetModifier(level);

            if (descriptionText != null) descriptionText.text = mod.description;
            
            if (modifierText != null)
            {
                modifierText.text = $"적 체력: {mod.enemyHealthMultiplier * 100}%\n" +
                                    $"적 공격력: {mod.enemyDamageMultiplier * 100}%\n" +
                                    $"골드 보상: {mod.goldRewardMultiplier * 100}%\n" +
                                    $"내공 보상: {mod.essenceRewardMultiplier * 100}%";
            }
        }

        private void ConfirmDifficulty()
        {
            OnDifficultyConfirmed?.Invoke();
            gameObject.SetActive(false);
        }

        private void GoBack()
        {
            OnBack?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
