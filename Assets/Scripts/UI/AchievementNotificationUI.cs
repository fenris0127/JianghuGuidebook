using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using JianghuGuidebook.Achievement;

namespace JianghuGuidebook.UI
{
    public class AchievementNotificationUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private Image icon;

        [Header("Settings")]
        [SerializeField] private float displayDuration = 3f;
        [SerializeField] private float slideDuration = 0.5f;

        private Queue<Achievement.Achievement> notificationQueue = new Queue<Achievement.Achievement>();
        private bool isDisplaying = false;
        private RectTransform rectTransform;
        private Vector2 hiddenPosition;
        private Vector2 shownPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            if (panel != null) panel.SetActive(false);
            
            // Setup positions (assuming top-center or bottom-center anchor)
            // This might need adjustment based on actual UI layout
            // For now, assume it slides down from top
            float height = rectTransform.rect.height;
            hiddenPosition = new Vector2(0, height); // Just above screen
            shownPosition = new Vector2(0, -height / 2 - 20); // Slightly down
        }

        private void Start()
        {
            if (AchievementManager.Instance != null)
            {
                AchievementManager.Instance.OnAchievementUnlocked += Show;
            }
        }

        private void OnDestroy()
        {
            if (AchievementManager.Instance != null)
            {
                AchievementManager.Instance.OnAchievementUnlocked -= Show;
            }
        }

        public void Show(Achievement.Achievement achievement)
        {
            notificationQueue.Enqueue(achievement);
            if (!isDisplaying)
            {
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            isDisplaying = true;

            while (notificationQueue.Count > 0)
            {
                Achievement.Achievement achievement = notificationQueue.Dequeue();
                DisplayAchievement(achievement);
                
                // Animate In
                yield return StartCoroutine(AnimateSlide(hiddenPosition, shownPosition));

                // Wait
                yield return new WaitForSeconds(displayDuration);

                // Animate Out
                yield return StartCoroutine(AnimateSlide(shownPosition, hiddenPosition));
                
                yield return new WaitForSeconds(0.2f); // Small gap between notifications
            }

            if (panel != null) panel.SetActive(false);
            isDisplaying = false;
        }

        private void DisplayAchievement(Achievement.Achievement achievement)
        {
            if (panel != null) panel.SetActive(true);
            if (nameText != null) nameText.text = achievement.name;
            if (descText != null) descText.text = achievement.description;
            
            // TODO: Set icon based on achievement type or ID
            // if (icon != null) icon.sprite = ...
        }

        private IEnumerator AnimateSlide(Vector2 startPos, Vector2 endPos)
        {
            float elapsed = 0f;
            while (elapsed < slideDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / slideDuration;
                // Smooth step
                t = t * t * (3f - 2f * t);
                
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }
            rectTransform.anchoredPosition = endPos;
        }
    }
}
