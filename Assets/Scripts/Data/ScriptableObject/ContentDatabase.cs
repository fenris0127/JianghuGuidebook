using UnityEngine;
using System.Collections.Generic;

// 게임에 존재하는 모든 핵심 콘텐츠의 원본 목록을 담고 있는 중앙 데이터베이스 애셋입니다.
[CreateAssetMenu(fileName = "ContentDatabase", menuName = "Game/Content Database")]
public class ContentDatabase : ScriptableObject
{
    [Header("카드 데이터베이스")]
    public List<CardData> allCards;

    [Header("유물 데이터베이스")]
    public List<RelicData> allRelics;

    [Header("기연 데이터베이스")]
    public List<EventData> allEvents;
    
    [Header("적 데이터베이스")]
    public List<EncounterData> allEncounters;
    public List<EnemyData> allEnemies;

    [Header("연계초식 데이터베이스")]
    public List<ComboData> allCombos;

    [Header("상태 이상 데이터베이스")]
    public List<StatusEffectData> allStatusEffects;
}