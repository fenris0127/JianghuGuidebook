namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 맵 노드의 타입을 정의합니다
    /// </summary>
    public enum NodeType
    {
        /// <summary>일반 전투</summary>
        Combat,

        /// <summary>정예 전투 (더 강한 적, 더 좋은 보상)</summary>
        EliteCombat,

        /// <summary>휴식 지점 (체력 회복 또는 카드 업그레이드)</summary>
        Rest,

        /// <summary>상점 (카드, 유물, 서비스 구매)</summary>
        Shop,

        /// <summary>랜덤 이벤트 (선택지 기반)</summary>
        Event,

        /// <summary>보물 (무료 유물 또는 카드 획득)</summary>
        Treasure,

        /// <summary>보스 전투 (지역 최종 보스)</summary>
        Boss,

        /// <summary>시작 노드</summary>
        Start
    }
}
