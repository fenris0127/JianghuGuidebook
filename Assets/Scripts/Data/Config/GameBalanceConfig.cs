using UnityEngine;

namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 게임 밸런스 설정을 관리하는 ScriptableObject
    /// 모든 매직 넘버를 이 파일에서 관리합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "GangHoBiGeup/Config/Game Balance Config")]
    public class GameBalanceConfig : ScriptableObject
    {
        [Header("=== 플레이어 기본 설정 ===")]
        [Tooltip("플레이어의 기본 최대 체력")]
        public int baseMaxHealth = 80;

        [Tooltip("체력 강화 레벨당 증가 체력")]
        public int healthBonusPerLevel = 5;

        [Tooltip("골드 강화 레벨당 시작 골드")]
        public int startingGoldPerLevel = 20;

        [Tooltip("턴 시작 시 기본 드로우 카드 수")]
        public int defaultCardDrawCount = 5;

        [Header("=== 보상 설정 ===")]
        [Tooltip("카드 보상 선택지 개수")]
        public int cardRewardChoiceCount = 3;

        [Tooltip("유물 보상 선택지 개수")]
        public int relicRewardChoiceCount = 3;

        [Tooltip("전투 승리 시 최소 골드")]
        public int combatRewardGoldMin = 10;

        [Tooltip("전투 승리 시 최대 골드")]
        public int combatRewardGoldMax = 21; // 10~20 포함

        [Header("=== 카드 희귀도 확률 ===")]
        [Tooltip("전설 카드 획득 확률 (0.0 ~ 1.0)")]
        [Range(0f, 1f)]
        public float legendaryCardChance = 0.02f; // 2%

        [Tooltip("영웅 카드 획득 확률 (0.0 ~ 1.0)")]
        [Range(0f, 1f)]
        public float epicCardChance = 0.10f; // 10%

        [Tooltip("희귀 카드 획득 확률 (0.0 ~ 1.0)")]
        [Range(0f, 1f)]
        public float rareCardChance = 0.30f; // 30%

        [Tooltip("고급 카드 획득 확률 (0.0 ~ 1.0)")]
        [Range(0f, 1f)]
        public float uncommonCardChance = 0.65f; // 65%

        // 나머지는 일반 카드 (Common)

        [Header("=== 메타 진행 설정 ===")]
        [Tooltip("게임 오버 시 층당 깨달음 포인트")]
        public int enlightenmentPointsPerFloor = 10;

        [Tooltip("승리 시 기본 깨달음 포인트")]
        public int victoryBaseEnlightenmentPoints = 100;

        [Header("=== 맵 설정 ===")]
        [Tooltip("최종 층 (보스 층)")]
        public int finalFloor = 3; // MapManager.FINAL_FLOOR와 동일

        /// <summary>
        /// 카드 희귀도를 랜덤으로 결정합니다.
        /// </summary>
        public CardRarity GetRandomCardRarity()
        {
            float roll = Random.value;

            if (roll <= legendaryCardChance)
                return CardRarity.Legendary;
            if (roll <= epicCardChance)
                return CardRarity.Epic;
            if (roll <= rareCardChance)
                return CardRarity.Rare;
            if (roll <= uncommonCardChance)
                return CardRarity.Uncommon;

            return CardRarity.Common;
        }

        /// <summary>
        /// 전투 승리 보상 골드를 계산합니다.
        /// </summary>
        public int GetCombatRewardGold()
        {
            return Random.Range(combatRewardGoldMin, combatRewardGoldMax);
        }

        /// <summary>
        /// 층수에 따른 깨달음 포인트를 계산합니다.
        /// </summary>
        public int GetEnlightenmentPointsForFloor(int floor)
        {
            return floor * enlightenmentPointsPerFloor;
        }

        /// <summary>
        /// 승리 시 총 깨달음 포인트를 계산합니다.
        /// </summary>
        public int GetVictoryEnlightenmentPoints(int currentFloor)
        {
            return victoryBaseEnlightenmentPoints + (currentFloor * enlightenmentPointsPerFloor);
        }
    }
}
