using UnityEngine;
using System.Collections.Generic;

// 적 개체 하나의 모든 기본 데이터를 담는 ScriptableObject 애셋입니다.
[CreateAssetMenu(fileName = "New EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public Sprite artwork;
    public int xpReward;
    public List<EnemyAction> actionPattern;
}