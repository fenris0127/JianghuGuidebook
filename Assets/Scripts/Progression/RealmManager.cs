using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.Progression
{
    /// <summary>
    /// 내공 경지 시스템을 관리하는 매니저
    /// </summary>
    public class RealmManager : MonoBehaviour
    {
        private static RealmManager _instance;

        public static RealmManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RealmManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("RealmManager");
                        _instance = go.AddComponent<RealmManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("현재 상태")]
        [SerializeField] private RealmStage currentStage = RealmStage.Hucheon;
        [SerializeField] private InnerEnergyRealm currentRealmData;

        [Header("진행도")]
        [SerializeField] private RealmCondition currentCondition;

        private Dictionary<RealmStage, InnerEnergyRealm> realmDatabase = new Dictionary<RealmStage, InnerEnergyRealm>();
        private Dictionary<RealmStage, RealmCondition> conditionDatabase = new Dictionary<RealmStage, RealmCondition>();

        public RealmStage CurrentStage => currentStage;
        public InnerEnergyRealm CurrentRealmData => currentRealmData;

        // Events
        public System.Action<RealmStage> OnRealmAdvanced;
        public System.Action<int, int> OnProgressUpdated; // current, required

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeDatabase();
        }

        private void Start()
        {
            // 초기 상태 설정
            SetRealm(RealmStage.Hucheon);
        }

        /// <summary>
        /// 데이터베이스 초기화 (하드코딩 또는 JSON 로드)
        /// </summary>
        private void InitializeDatabase()
        {
            // 1. 후천 (기본)
            var hucheon = new InnerEnergyRealm(RealmStage.Hucheon, "후천", "무공의 기초를 닦는 단계", "기본 단계");
            realmDatabase.Add(RealmStage.Hucheon, hucheon);

            // 2. 선천
            var seoncheon = new InnerEnergyRealm(RealmStage.Seoncheon, "선천", "내공이 자연스럽게 흐르는 단계", "내공 카드 20회 사용");
            seoncheon.maxEnergyBonus = 1;
            realmDatabase.Add(RealmStage.Seoncheon, seoncheon);
            conditionDatabase.Add(RealmStage.Hucheon, new RealmCondition(RealmConditionType.UseEnergyCard, 20)); // 후천 -> 선천 조건

            // 3. 화경
            var hwagyeong = new InnerEnergyRealm(RealmStage.Hwagyeong, "화경", "내공을 자유자재로 다루는 단계", "한 전투에서 내공 50 이상 소모");
            hwagyeong.maxEnergyBonus = 1;
            hwagyeong.drawBonus = 1;
            realmDatabase.Add(RealmStage.Hwagyeong, hwagyeong);
            conditionDatabase.Add(RealmStage.Seoncheon, new RealmCondition(RealmConditionType.SpendEnergyInBattle, 50)); // 선천 -> 화경 조건

            // 4. 종사
            var jongsa = new InnerEnergyRealm(RealmStage.Jongsa, "종사", "일가를 이루어 존경받는 단계", "누적 내공 500 소모");
            jongsa.maxEnergyBonus = 2;
            jongsa.drawBonus = 1;
            jongsa.damageMultiplier = 1.5f;
            realmDatabase.Add(RealmStage.Jongsa, jongsa);
            conditionDatabase.Add(RealmStage.Hwagyeong, new RealmCondition(RealmConditionType.TotalEnergySpent, 500)); // 화경 -> 종사 조건

            // 5. 천인합일
            var cheonin = new InnerEnergyRealm(RealmStage.CheonInhapil, "천인합일", "자연과 하나가 된 신의 경지", "내공 비기 10종 사용");
            cheonin.maxEnergyBonus = 3;
            cheonin.drawBonus = 2;
            cheonin.damageMultiplier = 2.0f;
            cheonin.energyRefundChance = 0.5f;
            realmDatabase.Add(RealmStage.CheonInhapil, cheonin);
            conditionDatabase.Add(RealmStage.Jongsa, new RealmCondition(RealmConditionType.UseEnergySecret, 10)); // 종사 -> 천인합일 조건
        }

        /// <summary>
        /// 특정 경지로 설정
        /// </summary>
        public void SetRealm(RealmStage stage)
        {
            if (realmDatabase.TryGetValue(stage, out InnerEnergyRealm data))
            {
                currentStage = stage;
                currentRealmData = data;
                
                // 다음 단계 조건 설정
                if (conditionDatabase.TryGetValue(stage, out RealmCondition condition))
                {
                    currentCondition = condition;
                }
                else
                {
                    currentCondition = null; // 최고 단계
                }

                Debug.Log($"경지 설정됨: {data.name}");
                
                // 플레이어에게 스탯 적용
                if (CombatManager.Instance != null && CombatManager.Instance.Player != null)
                {
                    ApplyRealmBonuses(CombatManager.Instance.Player);
                }
            }
        }

        /// <summary>
        /// 조건 진행도 업데이트
        /// </summary>
        public void UpdateProgress(RealmConditionType type, int amount)
        {
            if (currentCondition == null) return;
            if (currentCondition.type != type) return;

            currentCondition.AddProgress(amount);
            OnProgressUpdated?.Invoke(currentCondition.currentValue, currentCondition.requiredValue);

            Debug.Log($"경지 진행도 업데이트: {type} {currentCondition.currentValue}/{currentCondition.requiredValue}");

            if (currentCondition.IsMet())
            {
                AdvanceRealm();
            }
        }

        /// <summary>
        /// 다음 경지로 승급
        /// </summary>
        private void AdvanceRealm()
        {
            if (currentStage == RealmStage.CheonInhapil) return;

            RealmStage nextStage = currentStage + 1;
            Debug.Log($"!!! 경지 돌파 !!! {currentStage} -> {nextStage}");

            SetRealm(nextStage);
            OnRealmAdvanced?.Invoke(nextStage);

            // TODO: 돌파 연출 및 UI 표시
        }

        /// <summary>
        /// 플레이어에게 경지 보너스 적용
        /// </summary>
        public void ApplyRealmBonuses(Player player)
        {
            if (player == null || currentRealmData == null) return;

            Debug.Log($"플레이어에게 경지 보너스 적용: {currentRealmData.name}");
            // Player 클래스에 해당 메서드나 프로퍼티가 필요함
            // player.SetRealmBonuses(currentRealmData);
        }
    }
}
