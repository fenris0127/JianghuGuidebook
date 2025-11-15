using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 전투의 모든 과정을 통제하는 '심판' 클래스입니다.
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("참조")]
    [SerializeField] private Player player;
    [SerializeField] private List<Enemy> enemies; // 여러 명의 적을 관리
    [SerializeField] private Transform[] enemyPositions; // 적이 스폰될 위치
    [SerializeField] private Enemy enemy;

    private EncounterData currentEncounter;

    void Awake()
    {
        Instance = this;
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
        yield return new WaitForSeconds(0.5f);
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
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.currentHealth > 0)
            {
                yield return new WaitForSeconds(0.5f);
                enemy.TakeTurn(player);
                enemy.ProcessStatusEffectsOnTurnEnd();
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        if (player.currentHealth > 0)
            StartCoroutine(StartPlayerTurn());
    }

    public void ProcessCardEffect(CardData card, IDamageable primaryTarget)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.cardPlaySound);

        foreach (var effect in card.effects)
        {
            if (card.farRangeDamageMultiplier > 1f)
            {
                // if (currentRange == Range.Far) // BattleManager가 현재 거리를 알고 있어야 함
                // {
                //     // TODO: GameEffect의 피해량 값을 임시로 증폭시키는 로직
                // }
            }
            EffectProcessor.Process(effect, player, primaryTarget);
        }

        // if (card.pushAmount > 0)
        //     PushEnemy(card.pushAmount); // 적을 밀어내는 메서드 호출

        // if (card.pullAmount > 0)
        //     PullEnemy(card.pullAmount); // 적을 당겨오는 메서드 호출
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