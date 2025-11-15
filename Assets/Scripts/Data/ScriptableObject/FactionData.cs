using UnityEngine;
using System.Collections.Generic;
using System;

// 문파 패시브가 발동되는 '조건'을 정의합니다.
public enum FactionPassiveTrigger
{
    None,
    OnCombo,
    OnHealthBelow25Percent,
    OnApplyPoison,
    OnBeingVulnerable,
    OnRangeIsFar
}

// 문파의 고유 패시브 능력을 구체적으로 정의하는 클래스입니다.
[Serializable]
public class FactionPassive
{
    public string name;
    [TextArea] public string description;
    public FactionPassiveTrigger trigger;
    public int value;
}

// 하나의 플레이어블 문파에 대한 모든 정보를 담고 있는 ScriptableObject 애셋입니다.
[CreateAssetMenu(fileName = "New FactionData", menuName = "Game/FactionData")]
public class FactionData : ScriptableObject
{
    public string factionName;
    [TextArea] public string factionDescription;
    public Sprite factionIcon;

    [Header("게임 플레이 정보")]
    public List<CardData> startingDeck;

    [Header("고유 능력")]
    public FactionPassive passive;
}