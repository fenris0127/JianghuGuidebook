using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Economy;

namespace JianghuGuidebook.Events
{
    /// <summary>
    /// 이벤트 시스템을 관리하는 매니저
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private static EventManager _instance;

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EventManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("EventManager");
                        _instance = go.AddComponent<EventManager>();
                    }
                }
                return _instance;
            }
        }

        private EventData currentEvent;
        private EventChoice lastChoice;

        // Events
        public System.Action<EventData> OnEventStarted;
        public System.Action<EventChoice> OnChoiceSelected;
        public System.Action<List<EventOutcome>> OnOutcomesApplied;
        public System.Action OnEventCompleted;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        /// <summary>
        /// 랜덤 이벤트를 시작합니다
        /// </summary>
        public EventData StartRandomEvent()
        {
            EventData[] allEvents = DataManager.Instance.GetAllEvents();

            if (allEvents == null || allEvents.Length == 0)
            {
                Debug.LogError("이벤트 데이터가 없습니다");
                return null;
            }

            currentEvent = allEvents[Random.Range(0, allEvents.Length)];

            Debug.Log($"=== 이벤트 시작: {currentEvent.title} ===");
            Debug.Log(currentEvent.description);

            OnEventStarted?.Invoke(currentEvent);

            return currentEvent;
        }

        /// <summary>
        /// 특정 이벤트를 시작합니다
        /// </summary>
        public EventData StartEvent(string eventId)
        {
            currentEvent = DataManager.Instance.GetEventById(eventId);

            if (currentEvent == null)
            {
                Debug.LogError($"이벤트를 찾을 수 없습니다: {eventId}");
                return null;
            }

            Debug.Log($"=== 이벤트 시작: {currentEvent.title} ===");
            Debug.Log(currentEvent.description);

            OnEventStarted?.Invoke(currentEvent);

            return currentEvent;
        }

        /// <summary>
        /// 선택지를 선택합니다
        /// </summary>
        public bool SelectChoice(EventChoice choice, Player player)
        {
            if (currentEvent == null)
            {
                Debug.LogError("진행 중인 이벤트가 없습니다");
                return false;
            }

            if (!currentEvent.choices.Contains(choice))
            {
                Debug.LogError("유효하지 않은 선택지입니다");
                return false;
            }

            // 요구사항 체크
            if (!CheckRequirements(choice, player))
            {
                Debug.LogWarning("요구사항을 만족하지 못했습니다");
                return false;
            }

            Debug.Log($"선택: {choice.text}");

            lastChoice = choice;
            OnChoiceSelected?.Invoke(choice);

            // 결과 적용
            ApplyOutcomes(choice.outcomes, player);

            return true;
        }

        /// <summary>
        /// 선택지의 요구사항을 확인합니다
        /// </summary>
        private bool CheckRequirements(EventChoice choice, Player player)
        {
            if (choice.requirements == null || choice.requirements.Count == 0)
            {
                return true; // 요구사항 없음
            }

            foreach (EventRequirement req in choice.requirements)
            {
                switch (req.type)
                {
                    case RequirementType.MinHealth:
                        float healthPercent = (float)player.CurrentHealth / player.MaxHealth * 100f;
                        if (healthPercent < req.value)
                        {
                            Debug.Log($"요구사항 미충족: 체력 {healthPercent:F0}% < {req.value}%");
                            return false;
                        }
                        break;

                    case RequirementType.MinGold:
                        if (!GoldManager.Instance.HasEnoughGold(req.value))
                        {
                            Debug.Log($"요구사항 미충족: 골드 {GoldManager.Instance.CurrentGold} < {req.value}");
                            return false;
                        }
                        break;

                    case RequirementType.HasRelic:
                        if (!RelicManager.Instance.HasRelic(req.stringValue))
                        {
                            Debug.Log($"요구사항 미충족: 유물 '{req.stringValue}' 없음");
                            return false;
                        }
                        break;

                    case RequirementType.MaxHealth:
                        if (player.CurrentHealth < player.MaxHealth)
                        {
                            Debug.Log("요구사항 미충족: 최대 체력이 아님");
                            return false;
                        }
                        break;

                    case RequirementType.Random:
                        // 확률 체크 (성공률)
                        int roll = Random.Range(0, 100);
                        if (roll >= req.value)
                        {
                            Debug.Log($"확률 실패: {roll} >= {req.value}");
                            return false;
                        }
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 선택지 결과를 적용합니다
        /// </summary>
        private void ApplyOutcomes(List<EventOutcome> outcomes, Player player)
        {
            Debug.Log($"=== 결과 적용: {outcomes.Count}개 ===");

            foreach (EventOutcome outcome in outcomes)
            {
                ApplyOutcome(outcome, player);
            }

            OnOutcomesApplied?.Invoke(outcomes);

            // 결과 텍스트 표시
            if (!string.IsNullOrEmpty(lastChoice.resultText))
            {
                Debug.Log($"결과: {lastChoice.resultText}");
            }
        }

        /// <summary>
        /// 개별 결과를 적용합니다
        /// </summary>
        private void ApplyOutcome(EventOutcome outcome, Player player)
        {
            switch (outcome.type)
            {
                case OutcomeType.GainGold:
                    GoldManager.Instance.GainGold(outcome.value);
                    Debug.Log($"골드 +{outcome.value} (현재: {GoldManager.Instance.CurrentGold})");
                    break;

                case OutcomeType.LoseGold:
                    GoldManager.Instance.SpendGold(outcome.value);
                    Debug.Log($"골드 -{outcome.value} (현재: {GoldManager.Instance.CurrentGold})");
                    break;

                case OutcomeType.GainCard:
                    // TODO: 덱에 카드 추가
                    Debug.Log($"카드 획득: {outcome.stringValue} (덱 추가 필요)");
                    break;

                case OutcomeType.RemoveCard:
                    // TODO: 카드 제거 UI 표시
                    Debug.Log("카드 제거 선택 UI 표시 필요");
                    break;

                case OutcomeType.GainRelic:
                    // TODO: 유물 데이터 로드 후 추가
                    Debug.Log($"유물 획득: {outcome.stringValue} (RelicManager 추가 필요)");
                    break;

                case OutcomeType.Heal:
                    player.Heal(outcome.value);
                    Debug.Log($"체력 회복 +{outcome.value} (현재: {player.CurrentHealth}/{player.MaxHealth})");
                    break;

                case OutcomeType.TakeDamage:
                    player.TakeDamage(outcome.value);
                    Debug.Log($"피해 -{outcome.value} (현재: {player.CurrentHealth}/{player.MaxHealth})");
                    break;

                case OutcomeType.UpgradeCard:
                    // TODO: 카드 업그레이드 UI 표시
                    Debug.Log("카드 업그레이드 선택 UI 표시 필요");
                    break;

                case OutcomeType.StartCombat:
                    // TODO: 전투 시작 (적 ID로 적 로드)
                    Debug.Log($"전투 발생: {outcome.stringValue} (CombatManager 시작 필요)");
                    break;

                case OutcomeType.GainMaxHealth:
                    player.IncreaseMaxHealth(outcome.value);
                    Debug.Log($"최대 체력 +{outcome.value} (현재: {player.CurrentHealth}/{player.MaxHealth})");
                    break;
            }
        }

        /// <summary>
        /// 선택지가 사용 가능한지 확인합니다
        /// </summary>
        public bool IsChoiceAvailable(EventChoice choice, Player player)
        {
            return CheckRequirements(choice, player);
        }

        /// <summary>
        /// 현재 이벤트를 가져옵니다
        /// </summary>
        public EventData GetCurrentEvent()
        {
            return currentEvent;
        }

        /// <summary>
        /// 이벤트를 완료합니다
        /// </summary>
        public void CompleteEvent()
        {
            Debug.Log($"이벤트 완료: {currentEvent.title}");

            OnEventCompleted?.Invoke();

            currentEvent = null;
            lastChoice = null;
        }

        /// <summary>
        /// 이벤트 시스템을 리셋합니다
        /// </summary>
        public void ResetEvent()
        {
            currentEvent = null;
            lastChoice = null;
            Debug.Log("이벤트 시스템 리셋 완료");
        }
    }
}
