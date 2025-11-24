using UnityEngine;
using System;

namespace JianghuGuidebook.Relics
{
    /// <summary>
    /// 유물 희귀도
    /// </summary>
    public enum RelicRarity
    {
        Common,     // 일반
        Uncommon,   // 고급
        Rare,       // 희귀
        Legendary   // 전설
    }

    /// <summary>
    /// 유물 효과 타입
    /// </summary>
    public enum RelicEffectType
    {
        OnCombatStart,      // 전투 시작 시
        OnTurnStart,        // 턴 시작 시
        OnTurnEnd,          // 턴 종료 시
        OnCardPlay,         // 카드 사용 시
        OnAttack,           // 공격 시
        OnDefend,           // 방어 시
        OnDamageReceived,   // 피해 받을 시
        OnEnemyDeath,       // 적 사망 시
        OnDraw,             // 카드 드로우 시
        OnDiscard,          // 카드 버릴 시
        Passive,            // 지속 효과
        OnRest,             // 휴식 시
        OnShop,             // 상점 진입 시
        OnVictory           // 전투 승리 시
    }

    /// <summary>
    /// 유물 클래스
    /// </summary>
    [Serializable]
    public class Relic
    {
        public string id;
        public string name;
        public string description;
        public RelicRarity rarity;
        public RelicEffectType effectType;

        // 효과 수치
        public int effectValue1;
        public int effectValue2;

        // 특수 데이터
        public string specialData;

        // 상태
        public bool isActive = true;
        public int triggerCount = 0;  // 트리거된 횟수 (통계용)

        public Relic(string id, string name, string description, RelicRarity rarity, RelicEffectType effectType)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.rarity = rarity;
            this.effectType = effectType;
        }

        /// <summary>
        /// 유물 효과를 트리거합니다
        /// </summary>
        public void Trigger()
        {
            if (!isActive) return;

            triggerCount++;
            Debug.Log($"유물 효과 발동: {name} (타입: {effectType}, 횟수: {triggerCount})");
        }

        /// <summary>
        /// 유물 정보를 문자열로 반환합니다
        /// </summary>
        public override string ToString()
        {
            return $"[{rarity}] {name} - {description}";
        }

        /// <summary>
        /// 유물의 가격을 계산합니다 (상점용)
        /// </summary>
        public int GetPrice()
        {
            switch (rarity)
            {
                case RelicRarity.Common:
                    return 100;
                case RelicRarity.Uncommon:
                    return 150;
                case RelicRarity.Rare:
                    return 200;
                case RelicRarity.Legendary:
                    return 300;
                default:
                    return 100;
            }
        }
    }

    /// <summary>
    /// 유물 데이터베이스 래퍼
    /// </summary>
    [Serializable]
    public class RelicDatabase
    {
        public RelicData[] relics;
    }

    /// <summary>
    /// JSON용 유물 데이터 구조
    /// </summary>
    [Serializable]
    public class RelicData
    {
        public string id;
        public string name;
        public string description;
        public int rarity;          // 0=Common, 1=Uncommon, 2=Rare, 3=Legendary
        public int effectType;
        public int effectValue1;
        public int effectValue2;
        public string specialData;

        /// <summary>
        /// RelicData를 Relic 객체로 변환합니다
        /// </summary>
        public Relic ToRelic()
        {
            Relic relic = new Relic(
                id,
                name,
                description,
                (RelicRarity)rarity,
                (RelicEffectType)effectType
            );

            relic.effectValue1 = effectValue1;
            relic.effectValue2 = effectValue2;
            relic.specialData = specialData;

            return relic;
        }

        /// <summary>
        /// 유효성을 검증합니다
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("RelicData: ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"RelicData: 유물 {id}의 이름이 비어있습니다");
                return false;
            }

            return true;
        }
    }
}
