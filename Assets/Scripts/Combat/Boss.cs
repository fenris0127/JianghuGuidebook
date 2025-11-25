using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace JianghuGuidebook.Combat
{
    /// <summary>
    /// 보스 적 클래스 (Enemy 상속)
    /// 페이즈 시스템과 특수 능력을 추가합니다
    /// </summary>
    public class Boss : Enemy
    {
        [Header("보스 전용")]
        [SerializeField] private List<BossPhase> phases = new List<BossPhase>();
        [SerializeField] private BossPhase currentPhase;
        [SerializeField] private int turnCounter = 0;          // 턴 카운터 (특수 패턴용)
        [SerializeField] private int specialAttackInterval = 3; // 특수 공격 주기

        // Properties
        public BossPhase CurrentPhase => currentPhase;
        public int TurnCounter => turnCounter;
        public string DefeatDialogue { get; private set; }

        // Events
        public System.Action<BossPhase> OnPhaseChanged;
        public System.Action OnSpecialAttackTriggered;

        /// <summary>
        /// 보스를 초기화합니다
        /// </summary>
        public void InitializeBoss(BossData bossData)
        {
            // 기본 Enemy 초기화
            Initialize(bossData.enemyData);

            // 보스 페이즈 설정
            phases.Clear();
            if (bossData.phases != null)
            {
                phases.AddRange(bossData.phases);
            }

            // 시작 페이즈 설정 (Phase1)
            currentPhase = phases.FirstOrDefault(p => p.phaseType == BossPhaseType.Phase1);

            turnCounter = 0;
            specialAttackInterval = bossData.specialAttackInterval;
            DefeatDialogue = bossData.defeatDialogue;

            Debug.Log($"보스 생성: {bossData.enemyData.name}");
            Debug.Log($"페이즈: {currentPhase}");
        }

        /// <summary>
        /// 보스 턴 시작
        /// </summary>
        public new void OnTurnStart()
        {
            base.OnTurnStart();

            turnCounter++;

            // 페이즈 체크
            CheckPhaseTransition();

            // 특수 공격 체크 (N턴마다)
            if (turnCounter % specialAttackInterval == 0)
            {
                TriggerSpecialAttack();
            }

            Debug.Log($"보스 턴 {turnCounter} 시작 (페이즈: {currentPhase.phaseName})");
        }

        /// <summary>
        /// 페이즈 전환 체크
        /// </summary>
        private void CheckPhaseTransition()
        {
            foreach (var phase in phases)
            {
                // 현재 페이즈보다 높은 페이즈인지 확인
                if (phase.phaseType > currentPhase.phaseType)
                {
                    // 체력이 임계값 이하인지 확인
                    if (phase.IsPhaseTriggered(CurrentHealth, MaxHealth))
                    {
                        TransitionToPhase(phase);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 페이즈 전환 실행
        /// </summary>
        private void TransitionToPhase(BossPhase newPhase)
        {
            if (currentPhase == newPhase) return;

            Debug.Log($"=== 페이즈 전환: {currentPhase.phaseName} → {newPhase.phaseName} ===");
            Debug.Log(newPhase.phaseDescription);

            currentPhase = newPhase;

            // 페이즈 전환 효과 (예: 버프 획득)
            ApplyPhaseTransitionEffects(newPhase);

            OnPhaseChanged?.Invoke(newPhase);
        }

        /// <summary>
        /// 페이즈 전환 시 효과 적용
        /// </summary>
        private void ApplyPhaseTransitionEffects(BossPhase phase)
        {
            switch (phase.phaseType)
            {
                case BossPhaseType.Phase2:
                    // 예: Phase 2 전환 시 힘 버프 획득
                    // TODO: 상태 효과 시스템 통합
                    Debug.Log("보스가 광폭화! 공격력 증가!");
                    break;

                case BossPhaseType.Phase3:
                    // 예: Phase 3 전환 시 강력한 버프
                    Debug.Log("보스가 최후의 발악! 모든 능력치 증가!");
                    break;
            }
        }

        /// <summary>
        /// 특수 공격 실행
        /// </summary>
        private void TriggerSpecialAttack()
        {
            Debug.Log($"보스 특수 공격 발동! (ID: {EnemyData.id})");
            OnSpecialAttackTriggered?.Invoke();

            // 보스별 특수 패턴 구현
            switch (EnemyData.id)
            {
                case "boss_blood_wolf": // 1지역 보스
                    ApplyBuffToSelf("buff_enrage"); // 분노: 공격력 증가
                    break;

                case "boss_iron_master": // 2지역 보스: 철장문주
                    if (turnCounter % 6 == 0) // 6턴마다 소환
                    {
                        SummonMinion("minion_iron_disciple");
                        SummonMinion("minion_iron_disciple");
                    }
                    else
                    {
                        ApplyBuffToSelf("buff_counter"); // 반격 태세
                    }
                    break;

                case "boss_demonic_guardian": // 3지역 보스: 마교 호법
                    // 생명력 흡수 (플레이어에게 피해를 주고 회복)
                    // TODO: 실제 흡혈 로직은 Intent 시스템에서 처리하거나 여기서 직접 구현
                    Heal(20);
                    ApplyDebuffToPlayer("debuff_weak"); // 약화
                    break;

                case "boss_ice_elder": // 4지역 보스: 빙궁장로
                    if (turnCounter % 5 == 0)
                    {
                        ApplyBuffToSelf("buff_ice_wall"); // 빙벽: 방어도 대량 획득
                    }
                    else
                    {
                        ApplyDebuffToPlayer("debuff_freeze"); // 빙결
                    }
                    break;

                case "boss_supreme_master": // 5지역 보스: 천하제일인
                    // 페이즈에 따른 패턴 변화
                    if (currentPhase.phaseType == BossPhaseType.Phase3)
                    {
                        // 필살기: 천하무적
                        ApplyBuffToSelf("buff_invincible");
                    }
                    else
                    {
                        // 일반 필살기
                        ApplyDebuffToPlayer("debuff_vulnerable");
                    }
                    break;

                default:
                    Debug.LogWarning($"알 수 없는 보스 ID: {EnemyData.id}");
                    break;
            }
        }

        /// <summary>
        /// 하수인 소환
        /// </summary>
        private void SummonMinion(string enemyId)
        {
            if (Core.CombatManager.Instance != null)
            {
                Core.CombatManager.Instance.SpawnEnemy(enemyId);
                Debug.Log($"보스가 하수인 소환: {enemyId}");
            }
        }

        /// <summary>
        /// 자신에게 버프 적용
        /// </summary>
        private void ApplyBuffToSelf(string buffId)
        {
            // TODO: StatusEffectManager를 통해 버프 적용
            Debug.Log($"보스 버프 적용: {buffId}");
        }

        /// <summary>
        /// 플레이어에게 디버프 적용
        /// </summary>
        private void ApplyDebuffToPlayer(string debuffId)
        {
            // TODO: StatusEffectManager를 통해 디버프 적용
            Debug.Log($"플레이어에게 디버프 적용: {debuffId}");
        }

        /// <summary>
        /// 보스 의도 결정 (오버라이드)
        /// 페이즈와 턴 카운터에 따라 다른 행동 패턴 사용
        /// </summary>
        public new void DetermineIntent()
        {
            // 특수 공격 턴인지 확인
            if (turnCounter > 0 && turnCounter % specialAttackInterval == 0)
            {
                DetermineSpecialAttackIntent();
            }
            else
            {
                // 페이즈에 따른 기본 행동 패턴
                DeterminePhaseIntent();
            }

            OnIntentChanged?.Invoke(CurrentIntent);
        }

        /// <summary>
        /// 페이즈별 의도 결정
        /// </summary>
        private void DeterminePhaseIntent()
        {
            // 기본 Enemy의 행동 패턴 사용
            base.DetermineIntent();

            // 페이즈에 따라 행동 강화
            if (currentPhase != null)
            {
                switch (currentPhase.phaseType)
                {
                    case BossPhaseType.Phase2:
                        // Phase 2: 공격력 1.5배
                        if (CurrentIntent != null && CurrentIntent.actionType == EnemyActionType.Attack)
                        {
                            CurrentIntent.value = Mathf.RoundToInt(CurrentIntent.value * 1.5f);
                        }
                        break;

                    case BossPhaseType.Phase3:
                        // Phase 3: 공격력 2배
                        if (CurrentIntent != null && CurrentIntent.actionType == EnemyActionType.Attack)
                        {
                            CurrentIntent.value = Mathf.RoundToInt(CurrentIntent.value * 2f);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 특수 공격 의도 결정
        /// </summary>
        private void DetermineSpecialAttackIntent()
        {
            // 강력한 공격 의도 생성
            int specialDamage = 30; // 기본 특수 공격 피해

            // 페이즈에 따라 특수 공격 강화
            if (currentPhase != null)
            {
                switch (currentPhase.phaseType)
                {
                    case BossPhaseType.Phase2:
                        specialDamage = 40;
                        break;
                    case BossPhaseType.Phase3:
                        specialDamage = 50;
                        break;
                }
            }

            // 특수 공격 의도 설정
            var specialIntent = new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = specialDamage
            };

            // private 필드에 직접 접근할 수 없으므로 리플렉션 사용하거나
            // Enemy 클래스에 SetIntent 메서드 추가 필요
            // 임시로 Debug 로그만 출력
            Debug.Log($"보스 특수 공격 의도: {specialDamage} 피해");
        }

        public override string ToString()
        {
            return $"{EnemyData.name} [보스] (HP: {CurrentHealth}/{MaxHealth}, 페이즈: {currentPhase?.phaseName})";
        }
    }

    /// <summary>
    /// 보스 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class BossData
    {
        public string id;
        public EnemyData enemyData;             // 기본 적 데이터
        public List<BossPhase> phases;          // 페이즈 리스트
        public int specialAttackInterval;       // 특수 공격 주기 (턴)
        public string defeatDialogue;           // 처치 시 대사

        public BossData()
        {
            phases = new List<BossPhase>();
            specialAttackInterval = 3;
            defeatDialogue = "크윽... 강하구나...";
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("BossData: ID가 비어있습니다");
                return false;
            }

            if (enemyData == null || !enemyData.IsValid())
            {
                Debug.LogError($"BossData [{id}]: enemyData가 유효하지 않습니다");
                return false;
            }

            if (phases == null || phases.Count == 0)
            {
                Debug.LogError($"BossData [{id}]: 페이즈가 없습니다");
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 보스 데이터베이스 (JSON 로딩용)
    /// </summary>
    [System.Serializable]
    public class BossDatabase
    {
        public List<BossData> bosses;

        public BossDatabase()
        {
            bosses = new List<BossData>();
        }
    }
}
