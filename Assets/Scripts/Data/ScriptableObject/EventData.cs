using UnityEngine;
using System.Collections.Generic;
using System;

public enum EventRarity
{
    Common,    // 일반
    Uncommon,  // 고급
    Rare,      // 희귀
    Epic,      // 에픽
    Legendary  // 레전드리 (특수 조건으로만 등장)
}

// 기연 이벤트의 단일 선택지(텍스트, 결과)를 정의하는 클래스입니다.
[Serializable]
public class EventChoice
{
    [TextArea] public string choiceText;
    [Tooltip("이 선택지의 성공 확률입니다. 100이면 항상 성공합니다.")]
    [Range(0, 100)] public int successChance = 100;

    [Tooltip("성공했을 때 발생할 모든 효과의 목록입니다.")]
    public List<GameEffect> successEffects;

    [Tooltip("실패했을 때 발생할 모든 효과의 목록입니다.")]
    public List<GameEffect> failureEffects;
}

// 하나의 기연 이벤트 전체를 담고 있는 ScriptableObject 애셋입니다.
[CreateAssetMenu(fileName = "New Event", menuName = "Game/EventData")]
public class EventData : ScriptableObject
{
    public string eventID;
    public string eventName;
    [TextArea(5, 10)] public string description;

    [Header("분류")]
    public EventRarity rarity; 
    
    [Header("선택지")]
    public List<EventChoice> choices;
}