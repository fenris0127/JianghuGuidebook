using UnityEngine;
using JianghuGuidebook.Combat;

namespace JianghuGuidebook.AI
{
    /// <summary>
    /// 보스 전용 AI 패턴 클래스
    /// 보스의 복잡한 행동 패턴과 전략을 관리합니다
    /// </summary>
    public class BossAI : MonoBehaviour
    {
        [Header("보스 참조")]
        [SerializeField] private Boss boss;

        [Header("AI 설정")]
        [SerializeField] private bool useAdaptiveStrategy = true;  // 적응형 전략 사용
        [SerializeField] private int consecutiveAttackLimit = 3;   // 연속 공격 제한

        private int consecutiveAttackCount = 0;
        private EnemyActionType lastActionType;

        /// <summary>
        /// 보스 AI를 초기화합니다
        /// </summary>
        public void Initialize(Boss bossInstance)
        {
            boss = bossInstance;
            consecutiveAttackCount = 0;
            lastActionType = EnemyActionType.Attack;

            Debug.Log($"보스 AI 초기화: {boss.EnemyData.name}");
        }

        /// <summary>
        /// 보스의 다음 행동을 결정합니다
        /// </summary>
        public EnemyAction DecideNextAction(Player player)
        {
            if (boss == null)
            {
                Debug.LogError("보스 참조가 없습니다");
                return CreateDefaultAction();
            }

            // 특수 공격 턴인지 확인
            if (boss.TurnCounter > 0 && boss.TurnCounter % 3 == 0)
            {
                return CreateSpecialAttack();
            }

            // 적응형 전략 사용
            if (useAdaptiveStrategy)
            {
                return DecideAdaptiveAction(player);
            }

            // 기본 행동 패턴
            return DecideBasicAction();
        }

        /// <summary>
        /// 적응형 행동 결정 (플레이어 상태에 따라 변화)
        /// </summary>
        private EnemyAction DecideAdaptiveAction(Player player)
        {
            // 1. 체력이 낮으면 방어 우선
            float healthPercent = (float)boss.CurrentHealth / boss.MaxHealth;
            if (healthPercent < 0.3f && boss.Block < 10)
            {
                consecutiveAttackCount = 0;
                lastActionType = EnemyActionType.Defend;
                return CreateDefendAction(15);
            }

            // 2. 플레이어가 방어도가 높으면 강공격
            if (player.Block > 20)
            {
                consecutiveAttackCount++;
                lastActionType = EnemyActionType.Attack;
                return CreateAttackAction(GetAttackDamage() + 5);
            }

            // 3. 플레이어 체력이 낮으면 공격 집중
            float playerHealthPercent = (float)player.CurrentHealth / player.MaxHealth;
            if (playerHealthPercent < 0.4f)
            {
                consecutiveAttackCount++;
                lastActionType = EnemyActionType.Attack;
                return CreateAttackAction(GetAttackDamage());
            }

            // 4. 연속 공격 제한 체크
            if (consecutiveAttackCount >= consecutiveAttackLimit)
            {
                consecutiveAttackCount = 0;
                lastActionType = EnemyActionType.Defend;
                return CreateDefendAction(10);
            }

            // 5. 기본 행동 패턴
            return DecideBasicAction();
        }

        /// <summary>
        /// 기본 행동 패턴 결정
        /// </summary>
        private EnemyAction DecideBasicAction()
        {
            // 가중치 기반 랜덤 선택
            int roll = Random.Range(0, 100);

            // 페이즈에 따라 가중치 조정
            int attackWeight = 60;
            int defendWeight = 30;

            if (boss.CurrentPhase != null)
            {
                switch (boss.CurrentPhase.phaseType)
                {
                    case BossPhaseType.Phase2:
                        // Phase 2: 공격 비중 증가
                        attackWeight = 70;
                        defendWeight = 20;
                        break;

                    case BossPhaseType.Phase3:
                        // Phase 3: 공격 위주
                        attackWeight = 80;
                        defendWeight = 10;
                        break;
                }
            }

            if (roll < attackWeight)
            {
                consecutiveAttackCount++;
                lastActionType = EnemyActionType.Attack;
                return CreateAttackAction(GetAttackDamage());
            }
            else if (roll < attackWeight + defendWeight)
            {
                consecutiveAttackCount = 0;
                lastActionType = EnemyActionType.Defend;
                return CreateDefendAction(10);
            }
            else
            {
                // 버프 (힘 증가 등)
                consecutiveAttackCount = 0;
                lastActionType = EnemyActionType.Buff;
                return CreateBuffAction();
            }
        }

        /// <summary>
        /// 페이즈에 따른 공격 피해량 계산
        /// </summary>
        private int GetAttackDamage()
        {
            int baseDamage = 15;

            if (boss.CurrentPhase != null)
            {
                switch (boss.CurrentPhase.phaseType)
                {
                    case BossPhaseType.Phase1:
                        baseDamage = 15;
                        break;

                    case BossPhaseType.Phase2:
                        baseDamage = 20;
                        break;

                    case BossPhaseType.Phase3:
                        baseDamage = 25;
                        break;
                }
            }

            return baseDamage;
        }

        /// <summary>
        /// 특수 공격 생성
        /// </summary>
        private EnemyAction CreateSpecialAttack()
        {
            int specialDamage = 30;

            if (boss.CurrentPhase != null)
            {
                switch (boss.CurrentPhase.phaseType)
                {
                    case BossPhaseType.Phase2:
                        specialDamage = 40;
                        break;

                    case BossPhaseType.Phase3:
                        specialDamage = 50;
                        break;
                }
            }

            Debug.Log($"보스 특수 공격: {specialDamage} 피해");

            return new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = specialDamage,
                weight = 100
            };
        }

        /// <summary>
        /// 공격 액션 생성
        /// </summary>
        private EnemyAction CreateAttackAction(int damage)
        {
            return new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = damage,
                weight = 60
            };
        }

        /// <summary>
        /// 방어 액션 생성
        /// </summary>
        private EnemyAction CreateDefendAction(int blockAmount)
        {
            return new EnemyAction
            {
                actionType = EnemyActionType.Defend,
                value = blockAmount,
                weight = 30
            };
        }

        /// <summary>
        /// 버프 액션 생성
        /// </summary>
        private EnemyAction CreateBuffAction()
        {
            return new EnemyAction
            {
                actionType = EnemyActionType.Buff,
                value = 2, // 힘 +2
                weight = 10
            };
        }

        /// <summary>
        /// 기본 액션 생성 (오류 처리용)
        /// </summary>
        private EnemyAction CreateDefaultAction()
        {
            return new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = 10,
                weight = 100
            };
        }

        /// <summary>
        /// AI 상태 리셋
        /// </summary>
        public void ResetAI()
        {
            consecutiveAttackCount = 0;
            lastActionType = EnemyActionType.Attack;
            Debug.Log("보스 AI 리셋");
        }
    }
}
