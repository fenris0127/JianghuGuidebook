using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.Rest
{
    /// <summary>
    /// 휴식 화면의 UI를 관리합니다.
    /// </summary>
    public class RestUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button sleepButton;
        [SerializeField] private Button trainingButton;
        [SerializeField] private Button meditationButton;
        [SerializeField] private Button leaveButton;

        [Header("Text References")]
        [SerializeField] private TextMeshProUGUI sleepDescText;
        [SerializeField] private TextMeshProUGUI trainingDescText;
        [SerializeField] private TextMeshProUGUI meditationDescText;

        [Header("Sub UI")]
        [SerializeField] private CardUpgradeUI cardUpgradeUI;

        private List<RestChoice> currentChoices;

        private void Start()
        {
            // 이벤트 구독
            RestManager.Instance.OnRestChoicesGenerated += UpdateUI;
            
            if (leaveButton != null)
            {
                leaveButton.onClick.AddListener(OnLeaveClicked);
                leaveButton.gameObject.SetActive(false); // 처음엔 숨김
            }

            if (cardUpgradeUI != null)
                cardUpgradeUI.Initialize(this);

            // 버튼 리스너 설정
            sleepButton.onClick.AddListener(() => OnChoiceClicked(RestChoiceType.Sleep));
            trainingButton.onClick.AddListener(() => OnChoiceClicked(RestChoiceType.Training));
            meditationButton.onClick.AddListener(() => OnChoiceClicked(RestChoiceType.Meditation));

            // 플레이어 찾기 (임시)
            Player player = FindObjectOfType<Player>();
            if (player == null)
            {
                // 테스트용 플레이어 생성 또는 경고
                Debug.LogWarning("Player not found for RestManager");
            }

            // 선택지 생성 요청
            RestManager.Instance.GenerateRestChoices(player);
        }

        private void OnDestroy()
        {
            if (RestManager.Instance != null)
            {
                RestManager.Instance.OnRestChoicesGenerated -= UpdateUI;
            }
        }

        private void UpdateUI(List<RestChoice> choices)
        {
            currentChoices = choices;

            foreach (var choice in choices)
            {
                Button btn = GetButtonForType(choice.choiceType);
                TextMeshProUGUI txt = GetTextForType(choice.choiceType);

                if (btn != null)
                {
                    btn.interactable = choice.isAvailable;
                }

                if (txt != null)
                {
                    txt.text = choice.description;
                    if (!choice.isAvailable)
                    {
                        txt.text += $"\n<color=red>{choice.unavailableReason}</color>";
                    }
                }
            }
        }

        private Button GetButtonForType(RestChoiceType type)
        {
            switch (type)
            {
                case RestChoiceType.Sleep: return sleepButton;
                case RestChoiceType.Training: return trainingButton;
                case RestChoiceType.Meditation: return meditationButton;
                default: return null;
            }
        }

        private TextMeshProUGUI GetTextForType(RestChoiceType type)
        {
            switch (type)
            {
                case RestChoiceType.Sleep: return sleepDescText;
                case RestChoiceType.Training: return trainingDescText;
                case RestChoiceType.Meditation: return meditationDescText;
                default: return null;
            }
        }

        private void OnChoiceClicked(RestChoiceType type)
        {
            var choice = currentChoices.Find(c => c.choiceType == type);
            if (choice == null || !choice.isAvailable) return;

            // 선택지 실행 전 UI 처리 (카드 선택 등)
            if (type == RestChoiceType.Training)
            {
                cardUpgradeUI.Open(1);
            }
            else if (type == RestChoiceType.Meditation)
            {
                // 타파심마는 체력 감소가 먼저 일어나야 하는지, 카드 선택 후 일어나야 하는지 결정 필요
                // 여기서는 카드 선택 후 확정 시 체력 감소하도록 로직 수정 필요할 수 있음
                // 일단 UI 열기
                cardUpgradeUI.Open(2);
                
                // RestManager의 Execute는 확정 시 호출하도록 변경하거나 여기서 호출
                // 현재 구조상 RestManager.ExecuteRestChoice가 즉시 실행되므로, 
                // 카드 선택이 필요한 경우 분리해야 함.
                // 임시로 여기서 Execute 호출하여 체력 감소 등 처리 (실제로는 카드 선택 후가 좋음)
                RestManager.Instance.ExecuteRestChoice(choice, FindObjectOfType<Player>());
            }
            else // Sleep
            {
                RestManager.Instance.ExecuteRestChoice(choice, FindObjectOfType<Player>());
                OnRestComplete();
            }
        }

        public void OnUpgradeComplete()
        {
            // 카드 업그레이드 완료 후 처리
            // Training인 경우 여기서 완료 처리
            OnRestComplete();
        }

        private void OnRestComplete()
        {
            // 모든 버튼 비활성화
            sleepButton.interactable = false;
            trainingButton.interactable = false;
            meditationButton.interactable = false;

            // 나가기 버튼 활성화
            if (leaveButton != null)
                leaveButton.gameObject.SetActive(true);
        }

        private void OnLeaveClicked()
        {
            RestManager.Instance.ExitRest();
        }
    }
}
