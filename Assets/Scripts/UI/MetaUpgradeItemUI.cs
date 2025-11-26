using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 개별 영구 업그레이드 항목의 UI를 관리합니다.
    /// </summary>
    public class MetaUpgradeItemUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Image iconImage; // 아이콘이 있다면

        private PermanentUpgrade upgrade;
        private System.Action<PermanentUpgrade> onPurchaseCallback;

        /// <summary>
        /// UI 초기화
        /// </summary>
        public void Initialize(PermanentUpgrade upgrade, System.Action<PermanentUpgrade> onPurchase)
        {
            this.upgrade = upgrade;
            this.onPurchaseCallback = onPurchase;

            if (purchaseButton != null)
            {
                purchaseButton.onClick.RemoveAllListeners();
                purchaseButton.onClick.AddListener(OnPurchaseClicked);
            }

            UpdateUI();
        }

        /// <summary>
        /// UI 상태 업데이트
        /// </summary>
        public void UpdateUI()
        {
            if (upgrade == null) return;

            if (nameText != null)
                nameText.text = upgrade.name;

            if (descriptionText != null)
                descriptionText.text = upgrade.description;

            if (costText != null)
                costText.text = $"{upgrade.GetCurrentCost()} 정수";

            if (progressText != null)
            {
                if (upgrade.maxPurchases == 0)
                    progressText.text = $"구매: {upgrade.timesPurchased}";
                else
                    progressText.text = $"{upgrade.timesPurchased} / {upgrade.maxPurchases}";
            }

            UpdatePurchaseButtonState();
            UpdateLockedState();
        }

        private void UpdateLockedState()
        {
            if (upgrade == null) return;

            bool isMet = upgrade.IsPrerequisiteMet();
            
            if (!isMet)
            {
                // 잠김 상태 표시
                if (purchaseButton != null) purchaseButton.interactable = false;
                
                // 선행 조건 이름 가져오기
                var prereq = MetaProgressionManager.Instance.GetUpgradeById(upgrade.prerequisiteId);
                string prereqName = prereq != null ? prereq.name : "선행 조건";

                if (descriptionText != null)
                {
                    descriptionText.text = $"<color=red>[잠김] 선행 조건: {prereqName}</color>\n{upgrade.description}";
                }
            }
        }

        private void UpdatePurchaseButtonState()
        {
            if (purchaseButton == null) return;

            bool canAfford = MugongEssence.Instance.HasEnoughEssence(upgrade.GetCurrentCost());
            bool canPurchaseMore = upgrade.CanPurchaseMore();
            bool isPrerequisiteMet = upgrade.IsPrerequisiteMet();

            purchaseButton.interactable = canAfford && canPurchaseMore && isPrerequisiteMet;

            // 버튼 텍스트 변경 (옵션)
            var buttonText = purchaseButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                if (!canPurchaseMore)
                    buttonText.text = "완료";
                else if (!isPrerequisiteMet)
                    buttonText.text = "잠김";
                else if (!canAfford)
                    buttonText.text = "부족";
                else
                    buttonText.text = "구매";
            }
        }

        private void OnPurchaseClicked()
        {
            onPurchaseCallback?.Invoke(upgrade);
        }
    }
}
