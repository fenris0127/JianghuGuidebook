using UnityEngine;
using UnityEngine.UI;
using JianghuGuidebook.Achievement;

namespace JianghuGuidebook.UI
{
    public class AchievementSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject lockOverlay;
        [SerializeField] private Text dateText;

        [Header("Progress")]
        [SerializeField] private Slider progressBar;
        [SerializeField] private Text progressText;

        [Header("Reward")]
        [SerializeField] private GameObject rewardGroup;
        [SerializeField] private Text rewardText;

        [Header("Colors")]
        [SerializeField] private Color unlockedColor = Color.white;
        [SerializeField] private Color lockedColor = Color.gray;

        public void Setup(Achievement.Achievement achievement)
        {
            if (achievement == null) return;

            if (nameText != null) nameText.text = achievement.name;
            if (descriptionText != null) descriptionText.text = achievement.description;

            bool isUnlocked = achievement.isUnlocked;

            if (lockOverlay != null)
            {
                lockOverlay.SetActive(!isUnlocked);
            }

            if (dateText != null)
            {
                if (isUnlocked)
                {
                    dateText.text = achievement.unlockedDate;
                    dateText.gameObject.SetActive(true);
                }
                else
                {
                    dateText.gameObject.SetActive(false);
                }
            }

            // Progress Bar
            if (progressBar != null)
            {
                if (achievement.targetProgress > 0)
                {
                    progressBar.gameObject.SetActive(true);
                    progressBar.maxValue = achievement.targetProgress;
                    progressBar.value = achievement.currentProgress;
                    
                    if (progressText != null)
                    {
                        progressText.gameObject.SetActive(true);
                        progressText.text = $"{achievement.currentProgress} / {achievement.targetProgress}";
                    }
                }
                else
                {
                    progressBar.gameObject.SetActive(false);
                    if (progressText != null) progressText.gameObject.SetActive(false);
                }
            }

            // Reward
            if (rewardGroup != null)
            {
                if (achievement.rewardType != AchievementRewardType.None)
                {
                    rewardGroup.SetActive(true);
                    if (rewardText != null)
                    {
                        string rewardStr = "";
                        switch (achievement.rewardType)
                        {
                            case AchievementRewardType.Essence: rewardStr = $"{achievement.rewardValue} 정수"; break;
                            case AchievementRewardType.UnlockRelic: rewardStr = "유물 해금"; break;
                            case AchievementRewardType.UnlockCard: rewardStr = "카드 해금"; break;
                        }
                        rewardText.text = rewardStr;
                    }
                }
                else
                {
                    rewardGroup.SetActive(false);
                }
            }

            // Optional: Change text color or icon based on state
            if (nameText != null)
            {
                nameText.color = isUnlocked ? unlockedColor : lockedColor;
            }
            
            // TODO: Set icon based on achievement type or ID
        }
    }
}
