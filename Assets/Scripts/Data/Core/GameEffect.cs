using System;
using UnityEngine;

// 효과가 적용될 대상을 정의합니다.
public enum EffectTarget 
{ 
    Self, // 시전자
    AllEnemies, 
    RandomEnemy, 
    SingleEnemy,
    Player
}

// 게임 내 모든 효과의 종류를 통합하여 정의합니다.
public enum GameEffectType 
{ 
    None,
    // 전투 효과
    Damage, 
    Block, 
    DrawCard, 
    GainNaegong, 
    ApplyStatus,

    // 기연/유물 효과
    Heal,
    GainGold, 
    LoseHealth, 
    SetHealthValue,
    SetHealthToPercentage,
    IncreaseMaxHealth,
    DecreaseMaxHealth,
    IncreaseMaxNaegong,
    LoseAllGold,
    GainXP,
    
    // 카드 효과
    ObtainCard,
    UpgradeRandomCard,
    UpgradeAllCards,
    DuplicateCard,
    RemoveCard,
    RemoveRandomCards,
    RemoveAllBasicCards,
    AddCurseToDeck,
    RemoveAllCurseCards,
    ResetAllUpgrades,

    // 유물 효과
    ObtainRelic,
    ObtainRandomRelic,
    DestroyRandomRelic,

    // 맵/전투 효과
    StartCombat,
    StartSequentialCombat,
    GoToPreviousNode,

    // 경지/성향 효과
    AscendSwordRealm,
    ChangeAlignment,
    
    // 지속 효과
    AddPersistentEffect
}

// 단일 게임 효과를 상세하게 정의하는 클래스입니다.
[Serializable]
public class GameEffect
{
    [Tooltip("효과의 종류")]
    public GameEffectType type;

    [Tooltip("효과의 수치 (피해량, 드로우 매수 등)")]
    public int value;

    [Tooltip("효과 타입이 ApplyStatus일 경우, 적용할 상태 이상 데이터를 연결합니다.")]
    public StatusEffectData statusEffectData;

    [Tooltip("효과 타입이 ObtainCard일 경우, 획득할 카드 데이터를 연결합니다.")]
    public CardData cardData;

    [Tooltip("효과 타입이 ObtainRelic일 경우, 획득할 유물 데이터를 연결합니다.")]
    public RelicData relicData;

    [Tooltip("효과가 적용될 대상")]
    public EffectTarget target = EffectTarget.SingleEnemy;
}