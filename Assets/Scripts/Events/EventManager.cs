using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Data;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Relics;
using JianghuGuidebook.Economy;
using JianghuGuidebook.Save;

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
        private Queue<string> chainEventQueue = new Queue<string>();

        // Events
        public System.Action<EventData> OnEventStarted;
        public System.Action<EventChoice> OnChoiceSelected;
        public System.Action<List<EventOutcome>> OnOutcomesApplied;
        public System.Action OnEventCompleted;
        public System.Action<string> OnChainEventTriggered;  // 연쇄 이벤트 트리거 시

        // 특수 결과 이벤트 (UI가 처리해야 함)
        public System.Action<int> OnCardRemovalRequested;      // 제거할 카드 수
        public System.Action<int> OnCardUpgradeRequested;      // 업그레이드할 카드 수
        public System.Action<string> OnCombatRequested;        // 전투할 적 ID

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
                    if (GoldManager.Instance != null)
                    {
                        GoldManager.Instance.GainGold(outcome.value);
                        Debug.Log($"[이벤트] 골드 +{outcome.value} (현재: {GoldManager.Instance.CurrentGold})");
                    }
                    break;

                case OutcomeType.LoseGold:
                    if (GoldManager.Instance != null)
                    {
                        GoldManager.Instance.SpendGold(outcome.value);
                        Debug.Log($"[이벤트] 골드 -{outcome.value} (현재: {GoldManager.Instance.CurrentGold})");
                    }
                    break;

                case OutcomeType.GainCard:
                    AddCardToDeck(outcome.stringValue);
                    break;

                case OutcomeType.RemoveCard:
                    RequestCardRemoval(outcome.value > 0 ? outcome.value : 1);
                    break;

                case OutcomeType.GainRelic:
                    AddRelic(outcome.stringValue);
                    break;

                case OutcomeType.Heal:
                    if (player != null)
                    {
                        player.Heal(outcome.value);
                        Debug.Log($"[이벤트] 체력 회복 +{outcome.value} (현재: {player.CurrentHealth}/{player.MaxHealth})");
                    }
                    break;

                case OutcomeType.TakeDamage:
                    if (player != null)
                    {
                        player.TakeDamage(outcome.value);
                        Debug.Log($"[이벤트] 피해 -{outcome.value} (현재: {player.CurrentHealth}/{player.MaxHealth})");
                    }
                    break;

                case OutcomeType.UpgradeCard:
                    RequestCardUpgrade(outcome.value > 0 ? outcome.value : 1);
                    break;

                case OutcomeType.StartCombat:
                    RequestCombat(outcome.stringValue);
                    break;

                case OutcomeType.GainMaxHealth:
                    if (player != null)
                    {
                        player.IncreaseMaxHealth(outcome.value);
                        Debug.Log($"[이벤트] 최대 체력 +{outcome.value} (현재: {player.CurrentHealth}/{player.MaxHealth})");
                    }
                    break;

                case OutcomeType.ChainEvent:
                    QueueChainEvent(outcome.stringValue);
                    break;

                default:
                    Debug.LogWarning($"처리되지 않은 결과 타입: {outcome.type}");
                    break;
            }
        }

        /// <summary>
        /// 연쇄 이벤트를 큐에 추가합니다
        /// </summary>
        private void QueueChainEvent(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                Debug.LogWarning("[이벤트] 연쇄 이벤트 ID가 비어있습니다");
                return;
            }

            // 이벤트 존재 여부 확인
            EventData chainEvent = DataManager.Instance?.GetEventById(eventId);
            if (chainEvent == null)
            {
                Debug.LogError($"[이벤트] 연쇄 이벤트를 찾을 수 없습니다: {eventId}");
                return;
            }

            chainEventQueue.Enqueue(eventId);
            Debug.Log($"[이벤트] 연쇄 이벤트 대기열 추가: {eventId} ({chainEvent.title})");

            OnChainEventTriggered?.Invoke(eventId);
        }

        /// <summary>
        /// 덱에 카드를 추가합니다
        /// </summary>
        private void AddCardToDeck(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                Debug.LogWarning("[이벤트] 카드 ID가 비어있습니다");
                return;
            }

            // 카드 데이터 검증
            CardData cardData = DataManager.Instance?.GetCardData(cardId);
            if (cardData == null)
            {
                Debug.LogError($"[이벤트] 카드를 찾을 수 없습니다: {cardId}");
                return;
            }

            // RunData에 카드 추가
            if (SaveManager.Instance != null && SaveManager.Instance.CurrentSaveData != null)
            {
                RunData runData = SaveManager.Instance.CurrentSaveData.currentRun;
                if (runData != null)
                {
                    runData.deckCardIds.Add(cardId);
                    runData.cardsCollected++;

                    Debug.Log($"[이벤트] 카드 획득: {cardData.name} (덱: {runData.deckCardIds.Count}장)");
                }
                else
                {
                    Debug.LogError("[이벤트] 현재 런 데이터가 없습니다");
                }
            }
            else
            {
                Debug.LogError("[이벤트] SaveManager 또는 SaveData가 없습니다");
            }
        }

        /// <summary>
        /// 유물을 추가합니다
        /// </summary>
        private void AddRelic(string relicId)
        {
            if (string.IsNullOrEmpty(relicId))
            {
                Debug.LogWarning("[이벤트] 유물 ID가 비어있습니다");
                return;
            }

            // 유물 데이터 로드
            Relic relic = DataManager.Instance?.GetRelicById(relicId);
            if (relic == null)
            {
                Debug.LogError($"[이벤트] 유물을 찾을 수 없습니다: {relicId}");
                return;
            }

            // RelicManager에 유물 추가
            if (RelicManager.Instance != null)
            {
                RelicManager.Instance.AddRelic(relic);
                Debug.Log($"[이벤트] 유물 획득: {relic.name}");
            }
            else
            {
                Debug.LogError("[이벤트] RelicManager가 없습니다");
            }

            // RunData에도 기록
            if (SaveManager.Instance != null && SaveManager.Instance.CurrentSaveData != null)
            {
                RunData runData = SaveManager.Instance.CurrentSaveData.currentRun;
                if (runData != null && !runData.relicIds.Contains(relicId))
                {
                    runData.relicIds.Add(relicId);
                }
            }
        }

        /// <summary>
        /// 카드 제거 요청 (UI가 처리)
        /// </summary>
        private void RequestCardRemoval(int count)
        {
            Debug.Log($"[이벤트] 카드 제거 요청: {count}장");
            OnCardRemovalRequested?.Invoke(count);
        }

        /// <summary>
        /// 카드 업그레이드 요청 (UI가 처리)
        /// </summary>
        private void RequestCardUpgrade(int count)
        {
            Debug.Log($"[이벤트] 카드 업그레이드 요청: {count}장");
            OnCardUpgradeRequested?.Invoke(count);
        }

        /// <summary>
        /// 전투 요청 (CombatManager나 EventUI가 처리)
        /// </summary>
        private void RequestCombat(string enemyId)
        {
            Debug.Log($"[이벤트] 전투 요청: {enemyId}");
            OnCombatRequested?.Invoke(enemyId);
            // TODO: CombatManager 통합 시 구현
            // CombatManager.Instance.StartEventCombat(enemyId);
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
            Debug.Log($"이벤트 완료: {currentEvent?.title}");

            OnEventCompleted?.Invoke();

            currentEvent = null;
            lastChoice = null;

            // 연쇄 이벤트가 있으면 자동 시작
            ProcessNextChainEvent();
        }

        /// <summary>
        /// 다음 연쇄 이벤트를 처리합니다
        /// </summary>
        private void ProcessNextChainEvent()
        {
            if (chainEventQueue.Count > 0)
            {
                string nextEventId = chainEventQueue.Dequeue();
                Debug.Log($"[이벤트] 연쇄 이벤트 자동 시작: {nextEventId}");
                StartEvent(nextEventId);
            }
            else
            {
                Debug.Log("[이벤트] 연쇄 이벤트 없음, 이벤트 종료");
            }
        }

        /// <summary>
        /// 대기 중인 연쇄 이벤트가 있는지 확인합니다
        /// </summary>
        public bool HasChainEventPending()
        {
            return chainEventQueue.Count > 0;
        }

        /// <summary>
        /// 연쇄 이벤트 큐를 초기화합니다
        /// </summary>
        public void ClearChainEventQueue()
        {
            chainEventQueue.Clear();
            Debug.Log("[이벤트] 연쇄 이벤트 큐 초기화");
        }

        /// <summary>
        /// 이벤트 시스템을 리셋합니다
        /// </summary>
        public void ResetEvent()
        {
            currentEvent = null;
            lastChoice = null;
            chainEventQueue.Clear();
            Debug.Log("이벤트 시스템 리셋 완료");
        }

        // ===== 외부에서 호출하는 완료 메서드 =====

        /// <summary>
        /// 카드 제거가 완료되었음을 알립니다
        /// </summary>
        public void CompleteCardRemoval(List<string> removedCardIds)
        {
            if (removedCardIds == null || removedCardIds.Count == 0)
            {
                Debug.Log("[이벤트] 카드 제거를 건너뛰었습니다");
                return;
            }

            // RunData에서 카드 제거
            if (SaveManager.Instance != null && SaveManager.Instance.CurrentSaveData != null)
            {
                RunData runData = SaveManager.Instance.CurrentSaveData.currentRun;
                if (runData != null)
                {
                    foreach (string cardId in removedCardIds)
                    {
                        runData.deckCardIds.Remove(cardId);
                        Debug.Log($"[이벤트] 덱에서 카드 제거: {cardId}");
                    }

                    Debug.Log($"[이벤트] 카드 제거 완료: {removedCardIds.Count}장 (덱: {runData.deckCardIds.Count}장)");
                }
            }
        }

        /// <summary>
        /// 카드 업그레이드가 완료되었음을 알립니다
        /// </summary>
        public void CompleteCardUpgrade(List<string> upgradedCardIds)
        {
            if (upgradedCardIds == null || upgradedCardIds.Count == 0)
            {
                Debug.Log("[이벤트] 카드 업그레이드를 건너뛰었습니다");
                return;
            }

            // 업그레이드된 카드 로그 (실제 업그레이드 로직은 CardUpgradeManager에서 처리)
            Debug.Log($"[이벤트] 카드 업그레이드 완료: {upgradedCardIds.Count}장");
            foreach (string cardId in upgradedCardIds)
            {
                Debug.Log($"  - {cardId}");
            }
        }

        /// <summary>
        /// 이벤트 전투가 완료되었음을 알립니다
        /// </summary>
        public void CompleteCombat(bool victory)
        {
            if (victory)
            {
                Debug.Log("[이벤트] 이벤트 전투 승리!");
            }
            else
            {
                Debug.Log("[이벤트] 이벤트 전투 패배...");
            }

            // 전투 결과에 따른 추가 처리는 여기서 가능
        }
    }
}
