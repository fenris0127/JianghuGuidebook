using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 전투 로그 UI
    /// 전투 중 발생하는 이벤트를 표시합니다
    /// </summary>
    public class CombatLogUI : MonoBehaviour
    {
        [Header("UI 요소")]
        [SerializeField] private GameObject logPanel;
        [SerializeField] private RectTransform logContent;
        [SerializeField] private GameObject logEntryPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button toggleButton;
        [SerializeField] private Button clearButton;

        [Header("설정")]
        [SerializeField] private int maxVisibleEntries = 20;
        [SerializeField] private bool autoScroll = true;
        [SerializeField] private float fadeDuration = 0.3f;

        private List<GameObject> logEntryObjects = new List<GameObject>();
        private CanvasGroup canvasGroup;
        private bool isPanelOpen = true;

        private void Awake()
        {
            // CanvasGroup 초기화
            if (logPanel != null)
            {
                canvasGroup = logPanel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = logPanel.AddComponent<CanvasGroup>();
                }
            }

            // 버튼 이벤트 연결
            if (toggleButton != null)
            {
                toggleButton.onClick.AddListener(TogglePanel);
            }

            if (clearButton != null)
            {
                clearButton.onClick.AddListener(ClearLogs);
            }
        }

        private void Start()
        {
            // CombatManager 로그 이벤트 구독
            if (CombatManager.Instance != null && CombatManager.Instance.CombatLog != null)
            {
                CombatManager.Instance.CombatLog.OnLogAdded += OnLogEntryAdded;
            }
            else
            {
                Debug.LogWarning("[CombatLogUI] CombatManager 또는 CombatLog를 찾을 수 없습니다");
            }
        }

        /// <summary>
        /// 새 로그 엔트리가 추가되었을 때
        /// </summary>
        private void OnLogEntryAdded(CombatLogEntry entry)
        {
            AddLogEntry(entry);
        }

        /// <summary>
        /// 로그 엔트리 UI 추가
        /// </summary>
        private void AddLogEntry(CombatLogEntry entry)
        {
            if (logContent == null || logEntryPrefab == null)
            {
                Debug.LogWarning("[CombatLogUI] LogContent 또는 LogEntryPrefab이 설정되지 않았습니다");
                return;
            }

            // 로그 엔트리 생성
            GameObject entryObj = Instantiate(logEntryPrefab, logContent);
            TextMeshProUGUI textComponent = entryObj.GetComponent<TextMeshProUGUI>();

            if (textComponent != null)
            {
                textComponent.text = entry.message;
                textComponent.color = entry.color;
            }

            logEntryObjects.Add(entryObj);

            // 최대 개수 초과 시 오래된 엔트리 제거
            if (logEntryObjects.Count > maxVisibleEntries)
            {
                GameObject oldestEntry = logEntryObjects[0];
                logEntryObjects.RemoveAt(0);
                Destroy(oldestEntry);
            }

            // 자동 스크롤
            if (autoScroll && scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        /// <summary>
        /// 패널 토글
        /// </summary>
        public void TogglePanel()
        {
            isPanelOpen = !isPanelOpen;

            if (logPanel != null)
            {
                logPanel.SetActive(isPanelOpen);
            }
        }

        /// <summary>
        /// 로그 초기화
        /// </summary>
        public void ClearLogs()
        {
            // 모든 로그 엔트리 오브젝트 제거
            foreach (var entryObj in logEntryObjects)
            {
                if (entryObj != null)
                {
                    Destroy(entryObj);
                }
            }

            logEntryObjects.Clear();

            // CombatManager 로그도 초기화
            if (CombatManager.Instance != null && CombatManager.Instance.CombatLog != null)
            {
                CombatManager.Instance.CombatLog.ClearLogs();
            }
        }

        /// <summary>
        /// 자동 스크롤 토글
        /// </summary>
        public void ToggleAutoScroll()
        {
            autoScroll = !autoScroll;
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (CombatManager.Instance != null && CombatManager.Instance.CombatLog != null)
            {
                CombatManager.Instance.CombatLog.OnLogAdded -= OnLogEntryAdded;
            }
        }
    }

    /// <summary>
    /// 전투 속도 UI 컨트롤
    /// </summary>
    public class CombatSpeedUI : MonoBehaviour
    {
        [SerializeField] private Button speedButton;
        [SerializeField] private TextMeshProUGUI speedText;

        private void Awake()
        {
            if (speedButton != null)
            {
                speedButton.onClick.AddListener(OnSpeedButtonClicked);
            }

            UpdateSpeedText();
        }

        /// <summary>
        /// 속도 버튼 클릭
        /// </summary>
        private void OnSpeedButtonClicked()
        {
            if (CombatManager.Instance != null && CombatManager.Instance.SpeedSettings != null)
            {
                CombatManager.Instance.SpeedSettings.ToggleSpeed();
                UpdateSpeedText();
            }
        }

        /// <summary>
        /// 속도 텍스트 업데이트
        /// </summary>
        private void UpdateSpeedText()
        {
            if (speedText == null)
                return;

            if (CombatManager.Instance != null && CombatManager.Instance.SpeedSettings != null)
            {
                float currentSpeed = CombatManager.Instance.SpeedSettings.CurrentSpeed;
                speedText.text = $"{currentSpeed:F1}x";
            }
            else
            {
                speedText.text = "1.0x";
            }
        }
    }
}
