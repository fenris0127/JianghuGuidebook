namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 게임에서 사용되는 상수 값들을 정의합니다
    /// </summary>
    public static class Constants
    {
        // ===== 전투 관련 상수 =====

        /// <summary>턴 시작 시 내공</summary>
        public const int STARTING_ENERGY = 3;

        /// <summary>턴 시작 시 드로우할 카드 수</summary>
        public const int STARTING_DRAW = 5;

        /// <summary>최대 손패 크기</summary>
        public const int MAX_HAND_SIZE = 10;

        /// <summary>최대 내공</summary>
        public const int MAX_ENERGY = 3;

        // ===== 플레이어 관련 상수 =====

        /// <summary>플레이어 시작 체력</summary>
        public const int PLAYER_STARTING_HEALTH = 80;

        /// <summary>플레이어 최대 체력</summary>
        public const int PLAYER_MAX_HEALTH = 80;

        // ===== 카드 관련 상수 =====

        /// <summary>시작 덱 카드 수</summary>
        public const int STARTING_DECK_SIZE = 10;

        // ===== 기타 상수 =====

        /// <summary>디버그 모드</summary>
        public const bool DEBUG_MODE = true;
    }
}
