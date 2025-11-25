using UnityEngine;
using System.Collections.Generic;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Faction
{
    /// <summary>
    /// 분파 패시브 능력 타입
    /// </summary>
    public enum FactionPassiveType
    {
        None,                       // 패시브 없음
        SwordDamageBonus,           // 검술 카드 피해 증가
        FistBlockGain,              // 권법 카드 사용 시 방어도 획득
        PalmGoldGain,               // 장법 카드 사용 시 골드 획득
        PoisonDoubleEffect,         // 중독 효과 2배
        HealthLossEnergyGain        // 체력 손실 시 내공 회복
    }

    /// <summary>
    /// 분파 패시브 능력 데이터
    /// </summary>
    [System.Serializable]
    public class FactionPassive
    {
        public FactionPassiveType type;
        public int value;                   // 수치 (피해 증가 %, 방어도 획득량 등)
        public string description;          // 패시브 설명

        public FactionPassive()
        {
            type = FactionPassiveType.None;
            value = 0;
            description = "";
        }

        public FactionPassive(FactionPassiveType type, int value, string description)
        {
            this.type = type;
            this.value = value;
            this.description = description;
        }

        public override string ToString()
        {
            return description;
        }
    }

    /// <summary>
    /// 분파 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class FactionData
    {
        public string id;                       // 분파 고유 ID
        public string name;                     // 분파 이름
        public string description;              // 분파 설명
        public List<string> startingDeck;       // 시작 덱 카드 ID 목록 (12장)
        public string startingRelic;            // 시작 유물 ID
        public FactionPassive passive;          // 분파 패시브 능력
        public WeaponType primaryWeapon;        // 주 무기 타입

        public FactionData()
        {
            startingDeck = new List<string>();
            passive = new FactionPassive();
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("[FactionData] ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"[FactionData] {id}: 이름이 비어있습니다");
                return false;
            }

            if (startingDeck == null || startingDeck.Count == 0)
            {
                Debug.LogError($"[FactionData] {id}: 시작 덱이 비어있습니다");
                return false;
            }

            if (startingDeck.Count < 10)
            {
                Debug.LogWarning($"[FactionData] {id}: 시작 덱이 10장 미만입니다 ({startingDeck.Count}장)");
            }

            if (string.IsNullOrEmpty(startingRelic))
            {
                Debug.LogWarning($"[FactionData] {id}: 시작 유물이 없습니다");
            }

            return true;
        }

        public override string ToString()
        {
            return $"{name} - {description}";
        }
    }

    /// <summary>
    /// 분파 데이터베이스 (JSON 직렬화용)
    /// </summary>
    [System.Serializable]
    public class FactionDatabase
    {
        public List<FactionData> factions;

        public FactionDatabase()
        {
            factions = new List<FactionData>();
        }
    }
}
