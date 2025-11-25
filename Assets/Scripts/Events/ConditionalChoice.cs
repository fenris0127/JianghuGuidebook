using UnityEngine;
using JianghuGuidebook.Save;
using JianghuGuidebook.Progression;
using JianghuGuidebook.Data;
using System.Linq;

namespace JianghuGuidebook.Events
{
    /// <summary>
    /// 조건부 선택지 검증 로직
    /// </summary>
    public static class ConditionalChoice
    {
        /// <summary>
        /// 요구사항이 충족되었는지 확인
        /// </summary>
        public static bool CheckRequirement(EventRequirement requirement, RunData runData)
        {
            if (runData == null)
            {
                Debug.LogWarning("[ConditionalChoice] RunData가 null입니다");
                return false;
            }

            switch (requirement.type)
            {
                case RequirementType.MinHealth:
                    return CheckMinHealth(requirement.value, runData);

                case RequirementType.MinGold:
                    return CheckMinGold(requirement.value, runData);

                case RequirementType.HasRelic:
                    return CheckHasRelic(requirement.stringValue, runData);

                case RequirementType.MaxHealth:
                    return CheckMaxHealth(runData);

                case RequirementType.Random:
                    return CheckRandom(requirement.value);

                case RequirementType.HasWeaponMastery:
                    return CheckWeaponMastery(requirement.stringValue, requirement.value);

                case RequirementType.HasInnerRealm:
                    return CheckInnerRealm(requirement.value);

                case RequirementType.MinCardsInDeck:
                    return CheckMinCardsInDeck(requirement.value, runData);

                case RequirementType.HasCardInDeck:
                    return CheckHasCardInDeck(requirement.stringValue, runData);

                case RequirementType.MinDeckSize:
                    return CheckMinDeckSize(requirement.value, runData);

                case RequirementType.MaxDeckSize:
                    return CheckMaxDeckSize(requirement.value, runData);

                default:
                    Debug.LogWarning($"[ConditionalChoice] 알 수 없는 요구사항 타입: {requirement.type}");
                    return false;
            }
        }

        /// <summary>
        /// 모든 요구사항이 충족되었는지 확인
        /// </summary>
        public static bool CheckAllRequirements(EventChoice choice, RunData runData)
        {
            if (choice.requirements == null || choice.requirements.Count == 0)
            {
                return true; // 요구사항이 없으면 항상 true
            }

            foreach (var requirement in choice.requirements)
            {
                if (!CheckRequirement(requirement, runData))
                {
                    return false;
                }
            }

            return true;
        }

        // ========== 개별 조건 체크 메서드 ==========

        private static bool CheckMinHealth(int minHealthPercent, RunData runData)
        {
            if (runData.maxHealth <= 0) return false;

            int healthPercent = (runData.currentHealth * 100) / runData.maxHealth;
            return healthPercent >= minHealthPercent;
        }

        private static bool CheckMinGold(int minGold, RunData runData)
        {
            return runData.currentGold >= minGold;
        }

        private static bool CheckHasRelic(string relicId, RunData runData)
        {
            if (string.IsNullOrEmpty(relicId)) return false;
            return runData.relicIds != null && runData.relicIds.Contains(relicId);
        }

        private static bool CheckMaxHealth(RunData runData)
        {
            return runData.currentHealth >= runData.maxHealth;
        }

        private static bool CheckRandom(int successRate)
        {
            int roll = Random.Range(0, 100);
            return roll < successRate;
        }

        private static bool CheckWeaponMastery(string weaponTypeString, int requiredTier)
        {
            // WeaponMasteryManager 확인
            if (WeaponMasteryManager.Instance == null)
            {
                Debug.LogWarning("[ConditionalChoice] WeaponMasteryManager가 없습니다");
                return false;
            }

            // WeaponType 파싱
            if (!System.Enum.TryParse<WeaponType>(weaponTypeString, out WeaponType weaponType))
            {
                Debug.LogWarning($"[ConditionalChoice] 잘못된 WeaponType: {weaponTypeString}");
                return false;
            }

            // 현재 경지 확인
            MasteryTier currentTier = WeaponMasteryManager.Instance.GetCurrentTier(weaponType);
            return (int)currentTier >= requiredTier;
        }

        private static bool CheckInnerRealm(int requiredRealm)
        {
            // InnerEnergyManager 확인
            if (InnerEnergyManager.Instance == null)
            {
                Debug.LogWarning("[ConditionalChoice] InnerEnergyManager가 없습니다");
                return false;
            }

            // 현재 경지 확인
            InnerEnergyRealm currentRealm = InnerEnergyManager.Instance.GetCurrentRealm();
            return (int)currentRealm >= requiredRealm;
        }

        private static bool CheckMinCardsInDeck(int minCards, RunData runData)
        {
            if (runData.deckCardIds == null) return false;
            return runData.deckCardIds.Count >= minCards;
        }

        private static bool CheckHasCardInDeck(string cardId, RunData runData)
        {
            if (string.IsNullOrEmpty(cardId)) return false;
            if (runData.deckCardIds == null) return false;
            return runData.deckCardIds.Contains(cardId);
        }

        private static bool CheckMinDeckSize(int minSize, RunData runData)
        {
            if (runData.deckCardIds == null) return false;
            return runData.deckCardIds.Count >= minSize;
        }

        private static bool CheckMaxDeckSize(int maxSize, RunData runData)
        {
            if (runData.deckCardIds == null) return true;
            return runData.deckCardIds.Count <= maxSize;
        }

        // ========== 유틸리티 메서드 ==========

        /// <summary>
        /// 선택지가 선택 가능한지 확인 (UI 표시용)
        /// </summary>
        public static bool IsChoiceAvailable(EventChoice choice, RunData runData)
        {
            return CheckAllRequirements(choice, runData);
        }

        /// <summary>
        /// 선택지에 대한 설명 텍스트 생성
        /// </summary>
        public static string GetChoiceDescription(EventChoice choice, RunData runData)
        {
            if (choice.requirements == null || choice.requirements.Count == 0)
            {
                return choice.text;
            }

            bool available = CheckAllRequirements(choice, runData);
            string requirementText = string.Join(", ", choice.requirements.Select(r => r.ToString()));

            if (available)
            {
                return $"{choice.text}\n<color=green>({requirementText})</color>";
            }
            else
            {
                return $"{choice.text}\n<color=red>({requirementText} - 조건 미달)</color>";
            }
        }

        /// <summary>
        /// 연쇄 이벤트 처리를 위한 다음 이벤트 ID 반환
        /// </summary>
        public static string GetChainEventId(EventOutcome outcome)
        {
            // ChainEvent 타입이 추가되면 여기서 처리
            // 지금은 stringValue에 다음 이벤트 ID를 저장한다고 가정
            return outcome.stringValue;
        }

        /// <summary>
        /// 복합 결과 처리 (여러 결과를 동시에 적용)
        /// </summary>
        public static void ProcessComplexOutcome(EventChoice choice, RunData runData)
        {
            if (choice.outcomes == null) return;

            foreach (var outcome in choice.outcomes)
            {
                // 각 결과를 순차적으로 처리
                // 실제 처리는 EventManager에서 수행
                Debug.Log($"[ConditionalChoice] 결과 처리: {outcome}");
            }
        }
    }
}
