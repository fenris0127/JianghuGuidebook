using UnityEngine;
using System.Collections.Generic;

// 카드의 타입을 정의합니다 (공격, 방어, 스킬).
public enum CardType 
{ 
    Attack, 
    Defend, 
    Skill 
}

// 카드의 희귀도를 5단계로 정의합니다.
public enum CardRarity 
{ 
    Common,
    Uncommon, 
    Rare, 
    Epic, 
    Legendary 
}

// 카드 한 장의 모든 데이터를 담고 있는 ScriptableObject 애셋입니다.
[CreateAssetMenu(fileName = "New CardData", menuName = "Game/CardData")]
public class CardData : ScriptableObject
{
    [Header("고유 식별자")]
    public string assetID;

    [Header("기본 정보")]
    public string cardName;
    [TextArea(2, 4)] public string description;
    public Sprite cardIcon;
    public int cost;

    [Header("덱빌딩 정보")]
    public CardRarity rarity;
    public int goldCost;

    [Header("키워드 효과")]
    public bool isJeolcho;
    public bool isCurse;

    [Header("효과 정보")]
    [Tooltip("이 카드의 주된 대상입니다.")]
    public EffectTarget targetType;
    [Tooltip("이 카드를 사용했을 때 발동될 모든 효과의 목록입니다.")]
    public List<GameEffect> effects;

    [Header("특수 메카닉")]
    public int pushAmount;
    public int pullAmount;
    public float farRangeDamageMultiplier = 1f;

    [Header("강화 정보")]
    public bool isUpgraded = false;
    public CardData upgradedVersion;
}