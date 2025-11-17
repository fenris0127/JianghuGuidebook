using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GangHoBiGeup.Data;

// 전투의 모든 과정을 통제하는 '심판' 클래스입니다.
// BattleConfig를 통해 타이밍 설정값을 가져옵니다.
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("설정")]
    [SerializeField] private BattleConfig battleConfig;

    [Header("참조")]
    [SerializeField] private Player player;
    [SerializeField] private List<Enemy> enemies; // 여러 명의 적을 관리
    [SerializeField] private Transform[] enemyPositions; // 적이 스폰될 위치
    [SerializeField] private Enemy enemy;

    private EncounterData currentEncounter;

    void Awake()
    {
        Instance = this;

        // BattleConfig가 없으면 Resources에서 로드 시도
        if (battleConfig == null)
        {
            battleConfig = Resources.Load<BattleConfig>("Config/BattleConfig");
            if (battleConfig == null)
            {
                Debug.LogWarning("BattleConfig를 찾을 수 없습니다. 기본값을 사용합니다.");
            }
        }
    }

    public Player GetPlayer() => player;
    public List<Enemy> GetEnemies() => enemies.Where(e => e != null && e.currentHealth > 0).ToList();

    public void StartBattle(EncounterData encounter)
    {
        this.currentEncounter = encounter;

        // 이전 전투의 적들 정리
        foreach (Enemy enemy in enemies)
            Destroy(enemy.gameObject);

        enemies.Clear();

        // 새로운 적 생성
        for (int i = 0; i < encounter.enemies.Count; i++)
        {
            if (i < enemyPositions.Length)
            {
                // 적 프리팹을 생성하면, 그 안의 UI도 함께 생성되고 자동으로 연결됩니다.
                Enemy newEnemy = Instantiate(enemy, enemyPositions[i].position, Quaternion.identity, enemyPositions[i]);
                newEnemy.Setup(encounter.enemies[i].enemyData, encounter);
                enemies.Add(newEnemy);
            }
        }
        
        player.ApplyCombatStartRelicEffects();
        player.ResetCombatRecords();
        StartCoroutine(BattleRoutine());
    }

    private IEnumerator BattleRoutine()
    {
        float delay = battleConfig?.battleStartDelay ?? 0.5f;
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(StartPlayerTurn());
    }

    private IEnumerator StartPlayerTurn()
    {
        player.ProcessStatusEffectsOnTurnStart();
        player.StartTurn();
        yield return null;
    }

    public void EndPlayerTurn()
    {
        player.ProcessStatusEffectsOnTurnEnd();
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        float startDelay = battleConfig?.enemyTurnStartDelay ?? 0.5f;
        float endDelay = battleConfig?.enemyTurnEndDelay ?? 1f;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.currentHealth > 0)
            {
                yield return new WaitForSeconds(startDelay);
                enemy.TakeTurn(player);
                enemy.ProcessStatusEffectsOnTurnEnd();
            }
        }

        yield return new WaitForSeconds(endDelay);

        if (player.currentHealth > 0)
            StartCoroutine(StartPlayerTurn());
    }

    public void ProcessCardEffect(CardData card, IDamageable primaryTarget)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.cardPlaySound);

        foreach (var effect in card.effects)
        {
            // TODO: 거리 기반 데미지 증폭 시스템 (farRangeDamageMultiplier)
            // TODO: 적 밀어내기/당기기 메커닉 (pushAmount, pullAmount)
            EffectProcessor.Process(effect, player, primaryTarget);
        }
    }

    public void OnEnemyDied(Enemy deadEnemy)
    {
        if (enemies.Contains(deadEnemy))
            enemies.Remove(deadEnemy);

        // 남은 적이 없으면 전투를 승리로 종료합니다.
        if (enemies.Count == 0)
            GameManager.Instance.EndBattle(true, currentEncounter.isBossEncounter);
    }
}