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
        /// 특수 공격 발동
        /// </summary>
        private void TriggerSpecialAttack()
        {
            Debug.Log($"=== 보스 특수 공격 발동! (턴 {turnCounter}) ===");

            OnSpecialAttackTriggered?.Invoke();

            // TODO: 특수 공격 로직 구현
            // 현재는 기본 공격보다 강력한 공격 의도로 설정
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
