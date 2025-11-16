using System.Collections.Generic;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Gameplay
{
    /// <summary>
    /// 연계 초식 시스템을 관리하는 컴포넌트
    /// </summary>
    public class ComboComponent : MonoBehaviour
    {
        private List<CardData> cardPlayHistory = new List<CardData>();
        private List<ComboData> registeredCombos = new List<ComboData>();

        /// <summary>
        /// 연계 초식을 등록합니다.
        /// </summary>
        public void RegisterCombo(ComboData comboData)
        {
            if (!registeredCombos.Contains(comboData))
            {
                registeredCombos.Add(comboData);
            }
        }

        /// <summary>
        /// 카드 사용을 기록하고 연계 초식을 확인합니다.
        /// </summary>
        /// <returns>연계가 발동되었는지 여부</returns>
        public bool RecordCardPlay(CardData card)
        {
            cardPlayHistory.Add(card);

            // 등록된 모든 연계 초식을 확인
            foreach (var combo in registeredCombos)
            {
                if (CheckComboTriggered(combo))
                {
                    // 연계 발동! 히스토리 초기화하고 true 반환
                    cardPlayHistory.Clear();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 연계 카운터를 초기화합니다 (턴 종료 시).
        /// </summary>
        public void ResetComboHistory()
        {
            cardPlayHistory.Clear();
        }

        /// <summary>
        /// 현재 카드 사용 히스토리를 반환합니다.
        /// </summary>
        public List<CardData> GetCardPlayHistory()
        {
            return new List<CardData>(cardPlayHistory);
        }

        /// <summary>
        /// 특정 연계 초식이 발동되었는지 확인합니다.
        /// </summary>
        private bool CheckComboTriggered(ComboData combo)
        {
            if (combo.requiredCardIDs == null || combo.requiredCardIDs.Count == 0)
                return false;

            int requiredLength = combo.requiredCardIDs.Count;
            if (cardPlayHistory.Count < requiredLength)
                return false;

            // 최근 N개의 카드가 요구되는 순서와 일치하는지 확인
            int startIndex = cardPlayHistory.Count - requiredLength;
            for (int i = 0; i < requiredLength; i++)
            {
                if (cardPlayHistory[startIndex + i].id != combo.requiredCardIDs[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
