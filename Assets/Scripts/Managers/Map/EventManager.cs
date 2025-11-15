using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

// 기연 이벤트의 표시와 로직 처리를 담당하는 관리자 클래스입니다.
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("UI 요소")]
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI eventNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private List<Button> choiceButtons;

    private Player player;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        player = FindObjectOfType<Player>(true);
        eventPanel.SetActive(false);
    }

    public void ShowEvent(EventData eventData)
    {
        GameManager.Instance.ChangeState(GameState.Event);
        
        eventNameText.text = eventData.eventName;
        descriptionText.text = eventData.description;

        // 버튼들을 이벤트 선택지에 맞게 설정
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < eventData.choices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = eventData.choices[i].choiceText;
                
                EventChoice choice = eventData.choices[i];
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choice));
            }
            else
                choiceButtons[i].gameObject.SetActive(false);
        }
    }

    private void OnChoiceSelected(EventChoice choice)
    {
         // 전투 시작 여부를 체크하기 위한 변수
        bool isCombatStarting = false;

        // 1. 성공/실패 판정
        float roll = Random.Range(0, 100);
        bool success = roll < choice.successChance;
        List<GameEffect> resultsToApply = success ? choice.successEffects : choice.failureEffects;

        // 2. 결과 목록을 순회하며 EffectProcessor에게 실행 위임
        foreach (var effect in resultsToApply)
        {
            // EffectProcessor에 player를 '시전자(caster)'로 전달
            EffectProcessor.Process(effect, player, player);
            
            if (effect.type == GameEffectType.StartCombat)
                isCombatStarting = true;
        }

        // 3. 전투가 시작되지 않는 경우에만 맵 뷰로 돌아감
        if (!isCombatStarting)
            GameManager.Instance.ChangeState(GameState.MapView);
    }
}