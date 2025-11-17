using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Core;

namespace GangHoBiGeup.Managers
{
    /// <summary>
    /// 보상(카드, 유물, 골드) 생성을 담당하는 매니저
    /// GameManager에서 보상 로직을 분리했습니다.
    /// </summary>
    public class RewardManager : Singleton<RewardManager>
    {
        [SerializeField] private GameBalanceConfig balanceConfig;

        /// <summary>
        /// 카드 보상 선택지를 생성합니다.
        /// </summary>
        /// <param name="count">생성할 카드 개수 (기본값: config 설정)</param>
        public List<CardData> GenerateCardRewards(int count = -1)
        {
            if (count < 0)
                count = balanceConfig.cardRewardChoiceCount;

            List<CardData> rewards = new List<CardData>();
            List<CardData> availableCards = new List<CardData>(ResourceManager.Instance.GetAllCards());

            for (int i = 0; i < count; i++)
            {
                if (availableCards.Count == 0) break;

                CardRarity rarity = balanceConfig.GetRandomCardRarity();
                List<CardData> candidates = availableCards
                    .Where(c => c.rarity == rarity && !c.isUpgraded)
                    .ToList();

                // 해당 희귀도가 없으면 일반 카드로 대체
                if (candidates.Count == 0)
                {
                    candidates = availableCards
                        .Where(c => c.rarity == CardRarity.Common && !c.isUpgraded)
                        .ToList();
                }

                if (candidates.Count == 0) continue;

                CardData choice = candidates[Random.Range(0, candidates.Count)];
                rewards.Add(choice);
                availableCards.Remove(choice);
            }

            return rewards;
        }

        /// <summary>
        /// 유물 보상 선택지를 생성합니다.
        /// </summary>
        /// <param name="count">생성할 유물 개수 (기본값: config 설정)</param>
        public List<RelicData> GenerateRelicRewards(int count = -1)
        {
            if (count < 0)
                count = balanceConfig.relicRewardChoiceCount;

            List<RelicData> allRelics = ResourceManager.Instance.GetAllRelics();
            return allRelics.OrderBy(r => Random.value).Take(count).ToList();
        }

        /// <summary>
        /// 전투 승리 보상 골드를 생성합니다.
        /// </summary>
        public int GenerateCombatRewardGold()
        {
            return balanceConfig.GetCombatRewardGold();
        }

        /// <summary>
        /// 깨달음 포인트를 계산합니다 (게임 오버 시).
        /// </summary>
        public int CalculateEnlightenmentPoints(int currentFloor)
        {
            return balanceConfig.GetEnlightenmentPointsForFloor(currentFloor);
        }

        /// <summary>
        /// 승리 시 깨달음 포인트를 계산합니다.
        /// </summary>
        public int CalculateVictoryEnlightenmentPoints(int currentFloor)
        {
            return balanceConfig.GetVictoryEnlightenmentPoints(currentFloor);
        }

        /// <summary>
        /// 플레이어 보너스 체력을 계산합니다.
        /// </summary>
        public int CalculateBonusHealth(int bonusHealthLevel)
        {
            return bonusHealthLevel * balanceConfig.healthBonusPerLevel;
        }

        /// <summary>
        /// 플레이어 시작 골드를 계산합니다.
        /// </summary>
        public int CalculateStartingGold(int startingGoldLevel)
        {
            return startingGoldLevel * balanceConfig.startingGoldPerLevel;
        }

        /// <summary>
        /// 플레이어 기본 최대 체력을 반환합니다.
        /// </summary>
        public int GetBaseMaxHealth()
        {
            return balanceConfig.baseMaxHealth;
        }
    }
}
