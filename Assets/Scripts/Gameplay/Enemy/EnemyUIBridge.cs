// Folder: Scripts/Gameplay/EnemyUIBridge.cs
using UnityEngine;

// Enemy 로직과 자식으로 있는 EnemyUI를 연결해주는 브릿지 클래스입니다.
public class EnemyUIBridge : MonoBehaviour
{
    [SerializeField] private Enemy enemyLogic;
    [SerializeField] private EnemyUI enemyUI;

    void Awake()
    {
        if (enemyLogic != null && enemyUI != null)
        {
            // EnemyUI가 자신의 Enemy를 알 수 있도록 설정
            enemyUI.Setup(enemyLogic);
        }
    }
}