using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Combat
{
    /// <summary>
    /// 전투 시스템 고도화 기능 모음
    /// 연계, 범위 공격, 전투 로그 등
    /// </summary>
    public static class CombatEnhancements
    {
        // === 타겟팅 시스템 ===

        /// <summary>
        /// 카드 타겟 타입에 따라 타겟 목록을 반환합니다
        /// </summary>
        public static List<Enemy> GetTargets(TargetType targetType, Enemy primaryTarget, List<Enemy> allEnemies)
        {
            List<Enemy> targets = new List<Enemy>();

            switch (targetType)
            {
                case TargetType.SingleEnemy:
                    if (primaryTarget != null && primaryTarget.IsAlive())
                    {
                        targets.Add(primaryTarget);
                    }
                    break;

                case TargetType.AllEnemies:
                    foreach (var enemy in allEnemies)
                    {
                        if (enemy.IsAlive())
                        {
                            targets.Add(enemy);
                        }
                    }
                    break;

                case TargetType.RandomEnemy:
                    var aliveEnemies = allEnemies.FindAll(e => e.IsAlive());
                    if (aliveEnemies.Count > 0)
                    {
                        int randomIndex = Random.Range(0, aliveEnemies.Count);
                        targets.Add(aliveEnemies[randomIndex]);
                    }
                    break;

                case TargetType.Self:
                case TargetType.None:
                    // 타겟 없음
                    break;
            }

            return targets;
        }

        /// <summary>
        /// 범위 공격 피해 배율 반환 (타겟 수가 많을수록 감소)
        /// </summary>
        public static float GetAOEDamageMultiplier(int targetCount)
        {
            if (targetCount <= 1)
                return 1.0f;
            else if (targetCount == 2)
                return 0.8f;
            else if (targetCount == 3)
                return 0.6f;
            else
                return 0.5f;
        }
    }

    /// <summary>
    /// 연계 시스템 - 특정 카드 조합으로 추가 효과 발동
    /// </summary>
    [System.Serializable]
    public class ComboData
    {
        public string comboId;                  // 연계 ID
        public string comboName;                // 연계 이름
        public List<string> requiredCardIds;    // 필요한 카드 ID 목록
        public string description;              // 연계 설명
        public ComboEffect effect;              // 연계 효과
    }

    /// <summary>
    /// 연계 효과 데이터
    /// </summary>
    [System.Serializable]
    public class ComboEffect
    {
        public int bonusDamage;                 // 추가 피해
        public int bonusBlock;                  // 추가 방어도
        public int drawCards;                   // 카드 드로우
        public int gainEnergy;                  // 내공 획득
        public string statusEffectId;           // 부여할 상태 효과
        public int statusEffectStacks;          // 상태 효과 스택 수
    }

    /// <summary>
    /// 연계 시스템 매니저
    /// </summary>
    public class ComboSystem
    {
        private List<string> cardsPlayedThisTurn = new List<string>();
        private List<ComboData> comboDatabase = new List<ComboData>();

        /// <summary>
        /// 카드 사용 기록
        /// </summary>
        public void OnCardPlayed(string cardId)
        {
            cardsPlayedThisTurn.Add(cardId);
            Debug.Log($"[ComboSystem] 카드 사용: {cardId}, 이번 턴 총 {cardsPlayedThisTurn.Count}장");
        }

        /// <summary>
        /// 턴 종료 시 초기화
        /// </summary>
        public void OnTurnEnd()
        {
            cardsPlayedThisTurn.Clear();
        }

        /// <summary>
        /// 발동 가능한 연계 체크
        /// </summary>
        public List<ComboData> CheckActivatedCombos()
        {
            List<ComboData> activatedCombos = new List<ComboData>();

            foreach (var combo in comboDatabase)
            {
                if (IsComboActivated(combo))
                {
                    activatedCombos.Add(combo);
                    Debug.Log($"[ComboSystem] 연계 발동: {combo.comboName}");
                }
            }

            return activatedCombos;
        }

        /// <summary>
        /// 특정 연계가 발동 조건을 만족하는지 확인
        /// </summary>
        private bool IsComboActivated(ComboData combo)
        {
            foreach (var requiredCardId in combo.requiredCardIds)
            {
                if (!cardsPlayedThisTurn.Contains(requiredCardId))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 연계 데이터 추가
        /// </summary>
        public void AddCombo(ComboData combo)
        {
            comboDatabase.Add(combo);
        }

        /// <summary>
        /// 기본 연계 초기화
        /// </summary>
        public void InitializeDefaultCombos()
        {
            // 예시: 검법 연계 - "일검" + "회풍낙엽검" = 추가 피해
            ComboData swordCombo = new ComboData
            {
                comboId = "combo_sword_basic",
                comboName = "검법 연계",
                requiredCardIds = new List<string> { "card_strike", "card_slash" },
                description = "기본 검법을 연속으로 사용하여 추가 피해를 입힙니다",
                effect = new ComboEffect
                {
                    bonusDamage = 5,
                    bonusBlock = 0,
                    drawCards = 0,
                    gainEnergy = 0
                }
            };
            AddCombo(swordCombo);

            // 예시: 방어 연계 - 방어 카드 3장 사용 시 내공 회복
            ComboData defenseCombo = new ComboData
            {
                comboId = "combo_defense_master",
                comboName = "금강불괴",
                requiredCardIds = new List<string> { "card_iron_guard", "card_iron_guard", "card_iron_guard" },
                description = "완벽한 방어로 내공을 회복합니다",
                effect = new ComboEffect
                {
                    bonusDamage = 0,
                    bonusBlock = 10,
                    drawCards = 0,
                    gainEnergy = 1
                }
            };
            AddCombo(defenseCombo);
        }
    }

    /// <summary>
    /// 전투 로그 엔트리
    /// </summary>
    [System.Serializable]
    public class CombatLogEntry
    {
        public float timestamp;                 // 로그 발생 시간
        public CombatLogType logType;           // 로그 타입
        public string message;                  // 로그 메시지
        public Color color;                     // 텍스트 색상

        public CombatLogEntry(CombatLogType type, string msg, Color col)
        {
            timestamp = Time.time;
            logType = type;
            message = msg;
            color = col;
        }
    }

    /// <summary>
    /// 전투 로그 타입
    /// </summary>
    public enum CombatLogType
    {
        PlayerAction,       // 플레이어 행동
        EnemyAction,        // 적 행동
        Damage,             // 피해
        Block,              // 방어
        StatusEffect,       // 상태 효과
        Combo,              // 연계
        System              // 시스템 메시지
    }

    /// <summary>
    /// 전투 로그 시스템
    /// </summary>
    public class CombatLogSystem
    {
        private List<CombatLogEntry> logs = new List<CombatLogEntry>();
        private int maxLogEntries = 100;

        // 로그 이벤트
        public System.Action<CombatLogEntry> OnLogAdded;

        /// <summary>
        /// 로그 추가
        /// </summary>
        public void AddLog(CombatLogType type, string message, Color? color = null)
        {
            Color logColor = color ?? GetDefaultColor(type);
            CombatLogEntry entry = new CombatLogEntry(type, message, logColor);

            logs.Add(entry);

            // 최대 개수 초과 시 오래된 로그 제거
            if (logs.Count > maxLogEntries)
            {
                logs.RemoveAt(0);
            }

            OnLogAdded?.Invoke(entry);
            Debug.Log($"[CombatLog] {message}");
        }

        /// <summary>
        /// 로그 타입별 기본 색상 반환
        /// </summary>
        private Color GetDefaultColor(CombatLogType type)
        {
            switch (type)
            {
                case CombatLogType.PlayerAction:
                    return new Color(0.2f, 0.8f, 1f);   // 하늘색

                case CombatLogType.EnemyAction:
                    return new Color(1f, 0.4f, 0.4f);   // 빨간색

                case CombatLogType.Damage:
                    return new Color(1f, 0.6f, 0f);     // 주황색

                case CombatLogType.Block:
                    return new Color(0.5f, 0.5f, 1f);   // 파란색

                case CombatLogType.StatusEffect:
                    return new Color(0.8f, 0.2f, 1f);   // 보라색

                case CombatLogType.Combo:
                    return new Color(1f, 0.8f, 0.2f);   // 금색

                case CombatLogType.System:
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// 모든 로그 가져오기
        /// </summary>
        public List<CombatLogEntry> GetAllLogs()
        {
            return new List<CombatLogEntry>(logs);
        }

        /// <summary>
        /// 로그 초기화
        /// </summary>
        public void ClearLogs()
        {
            logs.Clear();
        }
    }

    /// <summary>
    /// 전투 속도 설정
    /// </summary>
    public class CombatSpeedSettings
    {
        private float currentSpeed = 1.0f;

        public float CurrentSpeed => currentSpeed;

        // 사용 가능한 속도 옵션
        public static readonly float[] AvailableSpeeds = { 0.5f, 1.0f, 1.5f, 2.0f, 3.0f };

        /// <summary>
        /// 전투 속도 설정
        /// </summary>
        public void SetSpeed(float speed)
        {
            currentSpeed = Mathf.Clamp(speed, 0.5f, 3.0f);
            Time.timeScale = currentSpeed;
            Debug.Log($"[CombatSpeed] 속도 변경: {currentSpeed}x");
        }

        /// <summary>
        /// 다음 속도로 변경 (토글)
        /// </summary>
        public void ToggleSpeed()
        {
            int currentIndex = System.Array.IndexOf(AvailableSpeeds, currentSpeed);
            int nextIndex = (currentIndex + 1) % AvailableSpeeds.Length;
            SetSpeed(AvailableSpeeds[nextIndex]);
        }

        /// <summary>
        /// 기본 속도로 리셋
        /// </summary>
        public void ResetSpeed()
        {
            SetSpeed(1.0f);
        }
    }
}
