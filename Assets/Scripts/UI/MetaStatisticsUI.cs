using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JianghuGuidebook.Save;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 메타 진행 통계를 보여주는 UI 팝업
    /// </summary>
    public class MetaStatisticsUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject contentPanel;
        [SerializeField] private Button closeButton;
        
        [Header("Stat Texts")]
        [SerializeField] private TextMeshProUGUI totalRunsText;
        [SerializeField] private TextMeshProUGUI totalVictoriesText;
        [SerializeField] private TextMeshProUGUI totalDeathsText;
        [SerializeField] private TextMeshProUGUI winRateText;
        [SerializeField] private TextMeshProUGUI totalEnemiesKilledText;
        [SerializeField] private TextMeshProUGUI totalBossesDefeatedText;
        [SerializeField] private TextMeshProUGUI totalPlayTimeText;

        private void Start()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Hide);
            }
            
            // 시작 시 숨김
            Hide();
        }

        public void Show()
        {
            if (contentPanel != null) contentPanel.SetActive(true);
            UpdateStatistics();
        }

        public void Hide()
        {
            if (contentPanel != null) contentPanel.SetActive(false);
        }

        private void UpdateStatistics()
        {
            if (SaveManager.Instance == null || SaveManager.Instance.CurrentSaveData == null) return;

            MetaSaveData meta = SaveManager.Instance.CurrentSaveData.metaData;
            if (meta == null) return;

            if (totalRunsText != null) totalRunsText.text = meta.totalRunsCompleted.ToString();
            if (totalVictoriesText != null) totalVictoriesText.text = meta.totalVictories.ToString();
            if (totalDeathsText != null) totalDeathsText.text = meta.totalDeaths.ToString();
            
            if (winRateText != null)
            {
                float winRate = meta.totalRunsCompleted > 0 
                    ? (float)meta.totalVictories / meta.totalRunsCompleted * 100f 
                    : 0f;
                winRateText.text = $"{winRate:F1}%";
            }

            if (totalEnemiesKilledText != null) totalEnemiesKilledText.text = meta.totalEnemiesKilled.ToString();
            if (totalBossesDefeatedText != null) totalBossesDefeatedText.text = meta.totalBossesDefeated.ToString();
            
            if (totalPlayTimeText != null)
            {
                System.TimeSpan time = System.TimeSpan.FromSeconds(meta.totalPlayTime);
                totalPlayTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                    time.Hours + (time.Days * 24), time.Minutes, time.Seconds);
            }
        }
    }
}
