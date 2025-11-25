using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 보스 전용 UI를 관리합니다.
    /// </summary>
    public class BossUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI phaseNameText;
        [SerializeField] private TextMeshProUGUI phaseDescriptionText;
        [SerializeField] private GameObject specialAttackWarning;
        [SerializeField] private TextMeshProUGUI specialAttackText;

        private Boss currentBoss;

        public void Initialize(Boss boss)
        {
            this.currentBoss = boss;
            
            if (currentBoss == null)
            {
                panel.SetActive(false);
                return;
            }

            panel.SetActive(true);
            
            // 이벤트 구독
            currentBoss.OnPhaseChanged += UpdatePhaseUI;
            currentBoss.OnSpecialAttackTriggered += ShowSpecialAttackWarning;
            
            // 초기 상태 업데이트
            if (currentBoss.CurrentPhase != null)
            {
                UpdatePhaseUI(currentBoss.CurrentPhase);
            }
            
            if (specialAttackWarning != null)
                specialAttackWarning.SetActive(false);
        }

        private void OnDestroy()
        {
            if (currentBoss != null)
            {
                currentBoss.OnPhaseChanged -= UpdatePhaseUI;
                currentBoss.OnSpecialAttackTriggered -= ShowSpecialAttackWarning;
            }
        }

        private void UpdatePhaseUI(BossPhase phase)
        {
            if (phaseNameText != null)
                phaseNameText.text = phase.phaseName;
            
            if (phaseDescriptionText != null)
                phaseDescriptionText.text = phase.phaseDescription;
                
            // 페이즈 전환 시각 효과 (예: 텍스트 깜빡임 등)
            // TODO: 애니메이션 추가
        }

        private void ShowSpecialAttackWarning()
        {
            if (specialAttackWarning != null)
            {
                specialAttackWarning.SetActive(true);
                
                if (specialAttackText != null)
                    specialAttackText.text = "보스가 강력한 공격을 준비합니다!";
                
                // 몇 초 후 자동으로 꺼지게 하거나 턴 종료 시 꺼지게 할 수 있음
                // 여기서는 2초 후 끄기
                Invoke(nameof(HideSpecialAttackWarning), 2f);
            }
        }

        private void HideSpecialAttackWarning()
        {
            if (specialAttackWarning != null)
                specialAttackWarning.SetActive(false);
        }
    }
}
