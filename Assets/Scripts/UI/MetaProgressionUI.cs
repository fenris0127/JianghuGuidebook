using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 메타 진행(무공비전) 화면의 UI를 관리합니다.
    /// </summary>
    public class MetaProgressionUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform upgradesContainer;
        [SerializeField] private GameObject upgradeItemPrefab;
        [SerializeField] private TextMeshProUGUI currentEssenceText;
        [SerializeField] private TextMeshProUGUI currentEssenceText;
        [SerializeField] private Button backButton;
        [SerializeField] private Button statisticsButton;
        [SerializeField] private MetaStatisticsUI statisticsPopup;

        private List<MetaUpgradeItemUI> upgradeItems = new List<MetaUpgradeItemUI>();

        private void Start()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackClicked);
                backButton.onClick.AddListener(OnBackClicked);
            }

            if (statisticsButton != null)
            {
                statisticsButton.onClick.AddListener(OnStatisticsClicked);
            }

            // MetaProgressionManager가 초기화되었는지 확인하고 로드
            if (MetaProgressionManager.Instance != null)
            {
                LoadUpgrades();
                UpdateEssenceDisplay();
            }
        }

        private void LoadUpgrades()
        {
            // 기존 아이템 제거
            foreach (Transform child in upgradesContainer)
            {
                Destroy(child.gameObject);
            }
            upgradeItems.Clear();

            var upgrades = new List<PermanentUpgrade>(MetaProgressionManager.Instance.AllUpgrades);
            
            // 선행 조건이 있는 업그레이드가 뒤에 오도록 정렬 (간단한 위상 정렬 대용)
            // ID 순으로 정렬하되, 선행 조건이 있으면 그 뒤로 보냄
            upgrades.Sort((a, b) => {
                if (a.prerequisiteId == b.id) return 1; // a가 b를 필요로 함 -> b가 먼저
                if (b.prerequisiteId == a.id) return -1; // b가 a를 필요로 함 -> a가 먼저
                
                // 선행 조건 유무로 1차 정렬
                bool aHasPrereq = !string.IsNullOrEmpty(a.prerequisiteId);
                bool bHasPrereq = !string.IsNullOrEmpty(b.prerequisiteId);
                
                if (aHasPrereq && !bHasPrereq) return 1;
                if (!aHasPrereq && bHasPrereq) return -1;
                
                return 0;
            });

            foreach (var upgrade in upgrades)
            {
                GameObject obj = Instantiate(upgradeItemPrefab, upgradesContainer);
                MetaUpgradeItemUI itemUI = obj.GetComponent<MetaUpgradeItemUI>();

                if (itemUI != null)
                {
                    itemUI.Initialize(upgrade, OnUpgradePurchased);
                    upgradeItems.Add(itemUI);
                }
            }
        }

        private void OnUpgradePurchased(PermanentUpgrade upgrade)
        {
            bool success = MetaProgressionManager.Instance.PurchaseUpgrade(upgrade);

            if (success)
            {
                // UI 업데이트
                UpdateEssenceDisplay();
                
                // 모든 아이템 UI 업데이트 (비용 부족 상태 등 변경될 수 있으므로)
                foreach (var item in upgradeItems)
                {
                    item.UpdateUI();
                    // 메인 메뉴로 돌아가기
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        private void OnStatisticsClicked()
        {
            if (statisticsPopup != null)
            {
                statisticsPopup.Show();
            }
        }
            }
        }

        private void UpdateEssenceDisplay()
        {
            if (currentEssenceText != null)
            {
                currentEssenceText.text = $"무공 정수: {MugongEssence.Instance.CurrentEssence}";
            }
        }

        private void OnBackClicked()
        {
            // 메인 메뉴로 돌아가기
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
