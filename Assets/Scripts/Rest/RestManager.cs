using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Rest
{
    /// <summary>
    /// 휴식 시스템을 관리하는 매니저
    /// </summary>
    public class RestManager : MonoBehaviour
    {
        private static RestManager _instance;

        public static RestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RestManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("RestManager");
                        _instance = go.AddComponent<RestManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("휴식 설정")]
        [SerializeField] private int sleepHealPercent = 30;         // 수면 시 회복 %
        [SerializeField] private int meditationHealCost = 10;       // 타파심마 체력 손실 %
        [SerializeField] private float meditationHealthThreshold = 0.5f; // 타파심마 필요 체력 (50%)

        private List<RestChoice> availableChoices = new List<RestChoice>();

        // Events
        public System.Action<List<RestChoice>> OnRestChoicesGenerated;
        public System.Action<RestChoice> OnRestChoiceSelected;

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
        /// 휴식 선택지를 생성합니다
        /// </summary>
        public List<RestChoice> GenerateRestChoices(Player player)
        {
            availableChoices.Clear();

            Debug.Log("=== 휴식 선택지 생성 ===");

            // 1. 수면 (항상 사용 가능)
            int healAmount = Mathf.RoundToInt(player.MaxHealth * sleepHealPercent / 100f);
            RestChoice sleepChoice = new RestChoice(
                RestChoiceType.Sleep,
                "수면",
                $"최대 체력의 {sleepHealPercent}%를 회복합니다. ({healAmount} HP)"
            );
            availableChoices.Add(sleepChoice);

            // 2. 수련 (항상 사용 가능)
            RestChoice trainingChoice = new RestChoice(
                RestChoiceType.Training,
                "수련",
                "카드 1장을 업그레이드합니다."
            );
            availableChoices.Add(trainingChoice);

            // 3. 타파심마 (체력 50% 이상일 때만 가능)
            RestChoice meditationChoice = new RestChoice(
                RestChoiceType.Meditation,
                "타파심마",
                $"카드 2장을 업그레이드합니다. 최대 체력의 {meditationHealCost}%를 잃습니다."
            );

            float healthPercent = (float)player.CurrentHealth / player.MaxHealth;
            if (healthPercent < meditationHealthThreshold)
            {
                meditationChoice.SetUnavailable($"체력이 {(int)(meditationHealthThreshold * 100)}% 이상이어야 합니다.");
            }

            availableChoices.Add(meditationChoice);

            Debug.Log($"휴식 선택지 생성 완료: {availableChoices.Count}개");

            OnRestChoicesGenerated?.Invoke(availableChoices);

            return availableChoices;
        }

        /// <summary>
        /// 휴식 선택지를 실행합니다
        /// </summary>
        public bool ExecuteRestChoice(RestChoice choice, Player player)
        {
            if (choice == null)
            {
                Debug.LogError("선택지가 null입니다");
                return false;
            }

            if (!choice.isAvailable)
            {
                Debug.LogWarning($"선택지 사용 불가: {choice.unavailableReason}");
                return false;
            }

            Debug.Log($"=== 휴식 선택: {choice.name} ===");

            switch (choice.choiceType)
            {
                case RestChoiceType.Sleep:
                    ExecuteSleep(player);
                    break;

                case RestChoiceType.Training:
                    ExecuteTraining();
                    break;

                case RestChoiceType.Meditation:
                    ExecuteMeditation(player);
                    break;
            }

            OnRestChoiceSelected?.Invoke(choice);

            return true;
        }

        /// <summary>
        /// 수면 실행
        /// </summary>
        private void ExecuteSleep(Player player)
        {
            int healAmount = Mathf.RoundToInt(player.MaxHealth * sleepHealPercent / 100f);
            player.Heal(healAmount);
            Debug.Log($"수면 완료: {healAmount} HP 회복");

            // TODO: 유물 효과 발동 (OnRest)
        }

        /// <summary>
        /// 수련 실행
        /// </summary>
        private void ExecuteTraining()
        {
            Debug.Log("수련 선택 - 카드 업그레이드 UI가 열립니다.");
            // UI에서 CardUpgradeUI를 엽니다.
        }

        /// <summary>
        /// 타파심마 실행
        /// </summary>
        private void ExecuteMeditation(Player player)
        {
            // 체력 손실
            int healthLoss = Mathf.RoundToInt(player.MaxHealth * meditationHealCost / 100f);
            player.TakeDamage(healthLoss);
            Debug.Log($"타파심마: {healthLoss} HP 손실");
            
            Debug.Log("타파심마 선택 - 카드 업그레이드 UI가 열립니다.");
            // UI에서 CardUpgradeUI를 엽니다.
        }

        /// <summary>
        /// 카드 업그레이드 (UI에서 호출)
        /// </summary>
        public void UpgradeCard(Card card)
        {
            if (card == null)
            {
                Debug.LogError("업그레이드할 카드가 null입니다");
                return;
            }

            // TODO: 실제 카드 업그레이드 로직 구현
            Debug.Log($"카드 업그레이드: {card.Name}");

            // 예시: 피해/방어도 증가
            if (card.Damage > 0)
            {
                card.ModifyDamage(3);
                Debug.Log($"피해 +3 (현재: {card.Damage})");
            }

            if (card.Block > 0)
            {
                card.ModifyBlock(3);
                Debug.Log($"방어도 +3 (현재: {card.Block})");
            }

            // 비용 감소 (최소 0)
            if (card.Cost > 0)
            {
                card.ModifyCost(-1);
                Debug.Log($"비용 -1 (현재: {card.Cost})");
            }
        }

        /// <summary>
        /// 휴식 노드를 나갑니다
        /// </summary>
        public void ReturnToMap()
        {
            Debug.Log("맵으로 복귀");
            
            // 맵 매니저를 통해 맵 씬으로 이동
            if (Map.MapManager.Instance != null)
            {
                Map.MapManager.Instance.ReturnToMap();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
            }
        }

        /// <summary>
        /// 휴식 시스템을 리셋합니다
        /// </summary>
        public void ResetRest()
        {
            availableChoices.Clear();
            Debug.Log("휴식 시스템 리셋 완료");
        }
    }
}
