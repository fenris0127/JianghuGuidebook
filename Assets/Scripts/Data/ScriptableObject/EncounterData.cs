using UnityEngine;
using System.Collections.Generic;
using System;

// 전투에 등장할 적과 그 위치를 정의하는 클래스입니다.
[Serializable]
public class EnemyInEncounter
{
    public EnemyData enemyData;
    public Vector2 position;
}

// 단일 전투(Encounter) 구성을 담고 있는 ScriptableObject 애셋입니다.
[CreateAssetMenu(fileName = "New Encounter", menuName = "Game/EncounterData")]
public class EncounterData : ScriptableObject
{
    public string encounterName;
    public bool isBossEncounter;

    [Header("등장 적 정보")]
    public List<EnemyInEncounter> enemies;

    [Header("보스 전용 페이즈(Phase) 정보")]
    [Tooltip("체력 조건에 따라 행동 패턴이 변하는 다중 페이즈를 사용하는가?")]
    public bool hasMultiplePhases;

    [Tooltip("체력이 이 비율(0~1) 이하로 떨어졌을 때 2페이즈로 전환됩니다.")]
    [Range(0.1f, 0.9f)] public float phaseTransitionHealthThreshold = 0.5f;

    [Tooltip("2페이즈일 때 사용할 행동 목록입니다.")]
    public List<EnemyAction> actionPatternPhase2;
}