using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.Events
{
    /// <summary>
    /// 이벤트 화면의 UI를 관리합니다.
    /// </summary>
    public class EventUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image eventImage; // 이벤트 일러스트 (옵션)
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private GameObject choicePrefab; // EventChoiceUI 프리팹
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI resultText;

        private List<EventChoiceUI> choiceUIs = new List<EventChoiceUI>();

        private void Start()
        {
            // 이벤트 구독
            EventManager.Instance.OnEventStarted += SetupEventUI;
            EventManager.Instance.OnChoiceSelected += OnChoiceMade;
            
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
                continueButton.gameObject.SetActive(false); // 처음엔 숨김
            }

            if (resultText != null)
                resultText.text = "";

            // 이미 이벤트가 시작되었다면 UI 설정
            if (EventManager.Instance.GetCurrentEvent() != null)
            {
                SetupEventUI(EventManager.Instance.GetCurrentEvent());
            }
            else
            {
                // 테스트용 랜덤 이벤트 시작
                EventManager.Instance.StartRandomEvent();
            }
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnEventStarted -= SetupEventUI;
                EventManager.Instance.OnChoiceSelected -= OnChoiceMade;
            }
        }

        private void SetupEventUI(EventData eventData)
        {
            if (eventData == null) return;

            titleText.text = eventData.title;
            descriptionText.text = eventData.description;
            
            // TODO: 이벤트 이미지 설정
            // if (eventImage != null) eventImage.sprite = ...

            CreateChoiceButtons(eventData.choices);
            
            if (continueButton != null)
                continueButton.gameObject.SetActive(false);
            
            if (resultText != null)
                resultText.text = "";
        }

        private void CreateChoiceButtons(List<EventChoice> choices)
        {
            foreach (Transform child in choicesContainer)
            {
                Destroy(child.gameObject);
            }
            choiceUIs.Clear();

            foreach (var choice in choices)
            {
                GameObject obj = Instantiate(choicePrefab, choicesContainer);
                EventChoiceUI ui = obj.GetComponent<EventChoiceUI>();
                ui.Initialize(choice, this);
                choiceUIs.Add(ui);
            }
        }

        /// <summary>
        /// 선택지가 선택되었을 때 호출됩니다 (EventChoiceUI에서 호출)
        /// </summary>
        public void OnChoiceSelected(EventChoice choice)
        {
            Player player = FindObjectOfType<Player>();
            EventManager.Instance.SelectChoice(choice, player);
        }

        private void OnChoiceMade(EventChoice choice)
        {
            // 선택 후 UI 업데이트
            
            // 모든 선택지 버튼 비활성화 또는 제거
            foreach (var ui in choiceUIs)
            {
                ui.GetComponent<Button>().interactable = false;
            }

            // 결과 텍스트 표시
            if (resultText != null)
            {
                resultText.text = choice.resultText;
                // 결과에 따른 추가 정보 표시 가능
            }

            // 계속하기 버튼 표시
            if (continueButton != null)
                continueButton.gameObject.SetActive(true);
        }

        private void OnContinueClicked()
        {
            EventManager.Instance.CompleteEvent();
            // TODO: 맵으로 복귀
            // MapManager.Instance.ReturnToMap(); // EventManager.CompleteEvent에서 호출하거나 여기서 호출
            // 현재 구조상 EventManager는 로직만 처리하므로, 여기서 맵 복귀 호출이 적절할 수 있음
            // 하지만 MapManager 의존성을 줄이기 위해 EventManager가 이벤트를 통해 알리는 방식이 좋음
            // 일단은 여기서 직접 호출하거나 EventManager가 처리하도록 둠
            
            // 임시로 맵 복귀 호출
            JianghuGuidebook.Map.MapManager.Instance.ReturnToMap();
        }
    }
}
