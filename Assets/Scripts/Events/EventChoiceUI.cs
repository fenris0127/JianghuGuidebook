using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Combat; // Player 접근용

namespace JianghuGuidebook.Events
{
    /// <summary>
    /// 이벤트 선택지 UI를 관리합니다.
    /// </summary>
    public class EventChoiceUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private TextMeshProUGUI requirementText;
        [SerializeField] private TextMeshProUGUI outcomeText;

        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color disabledColor = Color.gray;
        [SerializeField] private Color requirementMetColor = Color.green;
        [SerializeField] private Color requirementNotMetColor = Color.red;

        private EventChoice eventChoice;
        private EventUI eventUI;
        private bool isAvailable;

        public void Initialize(EventChoice choice, EventUI ui)
        {
            this.eventChoice = choice;
            this.eventUI = ui;

            UpdateUI();
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnChoiceClicked);
        }

        private void UpdateUI()
        {
            if (eventChoice == null) return;

            // 텍스트 설정
            choiceText.text = eventChoice.text;
            
            // 요구사항 및 결과 텍스트 구성
            FormatRequirementsAndOutcomes();

            // 사용 가능 여부 확인
            Player player = FindObjectOfType<Player>();
            isAvailable = EventManager.Instance.IsChoiceAvailable(eventChoice, player);

            // 버튼 상태 설정
            button.interactable = isAvailable;
            
            // 텍스트 색상 등 시각적 피드백
            if (isAvailable)
            {
                choiceText.color = normalColor;
            }
            else
            {
                choiceText.color = disabledColor;
            }
        }

        private void FormatRequirementsAndOutcomes()
        {
            // 요구사항 텍스트
            string reqStr = "";
            if (eventChoice.requirements != null && eventChoice.requirements.Count > 0)
            {
                foreach (var req in eventChoice.requirements)
                {
                    reqStr += $"{req.ToString()}\n";
                }
            }
            requirementText.text = reqStr;

            // 결과 텍스트
            string outStr = "";
            if (eventChoice.outcomes != null && eventChoice.outcomes.Count > 0)
            {
                foreach (var outcome in eventChoice.outcomes)
                {
                    outStr += $"{outcome.ToString()}\n";
                }
            }
            outcomeText.text = outStr;
        }

        private void OnChoiceClicked()
        {
            if (isAvailable)
            {
                eventUI.OnChoiceSelected(eventChoice);
            }
        }
    }
}
