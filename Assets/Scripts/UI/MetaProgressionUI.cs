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
        [SerializeField] private Button backButton;

        private List<MetaUpgradeItemUI> upgradeItems = new List<MetaUpgradeItemUI>();

        private void Start()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackClicked);
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

            var upgrades = MetaProgressionManager.Instance.AllUpgrades;

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
