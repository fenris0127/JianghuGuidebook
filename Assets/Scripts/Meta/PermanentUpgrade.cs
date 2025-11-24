using UnityEngine;

namespace JianghuGuidebook.Meta
{
    /// <summary>
    /// 영구 업그레이드 타입
    /// </summary>
    public enum UpgradeType
    {
        IncreaseMaxHealth,      // 시작 최대 체력 증가
        IncreaseStartingGold,   // 시작 골드 증가
        UnlockStartingCard,     // 시작 카드 해금
        IncreaseMaxEnergy,      // 최대 내공 증가
        StartWithRelic,         // 시작 시 유물 보유
        IncreaseCardRewards,    // 카드 보상 개수 증가
        IncreaseGoldRewards,    // 골드 보상 증가
        ReduceShopPrices        // 상점 가격 할인
    }

    /// <summary>
    /// 영구 업그레이드 클래스
    /// </summary>
    [System.Serializable]
    public class PermanentUpgrade
    {
        public string id;               // 업그레이드 ID
        public string name;             // 업그레이드 이름
        public string description;      // 업그레이드 설명
        public UpgradeType type;        // 업그레이드 타입
        public int cost;                // 무공 정수 비용
        public int value;               // 효과 수치
        public string stringValue;      // 문자열 값 (카드 ID, 유물 ID 등)
        public bool isUnlocked;         // 해금 여부
        public int maxPurchases;        // 최대 구매 횟수 (0 = 무제한, 1 = 1회만)
        public int timesPurchased;      // 구매 횟수

        public PermanentUpgrade()
        {
            isUnlocked = false;
            maxPurchases = 1;
            timesPurchased = 0;
        }

        public PermanentUpgrade(string id, string name, string description, UpgradeType type, int cost, int value = 0, string stringValue = "")
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.type = type;
            this.cost = cost;
            this.value = value;
            this.stringValue = stringValue;
            this.isUnlocked = false;
            this.maxPurchases = 1;
            this.timesPurchased = 0;
        }

        /// <summary>
        /// 업그레이드를 해금합니다
        /// </summary>
        public bool Unlock()
        {
            if (isUnlocked && timesPurchased >= maxPurchases && maxPurchases > 0)
            {
                Debug.LogWarning($"업그레이드 {name}은 이미 최대 횟수만큼 구매되었습니다");
                return false;
            }

            isUnlocked = true;
            timesPurchased++;

            Debug.Log($"업그레이드 해금: {name} ({timesPurchased}/{maxPurchases})");
            return true;
        }

        /// <summary>
        /// 추가 구매 가능한지 확인합니다
        /// </summary>
        public bool CanPurchaseMore()
        {
            if (maxPurchases == 0) return true; // 무제한
            return timesPurchased < maxPurchases;
        }

        /// <summary>
        /// 현재 비용을 반환합니다 (구매 횟수에 따라 증가할 수 있음)
        /// </summary>
        public int GetCurrentCost()
        {
            // 일부 업그레이드는 구매 횟수에 따라 비용 증가
            if (maxPurchases == 0 || maxPurchases > 1)
            {
                return cost + (timesPurchased * 5); // 구매마다 +5 정수
            }
            return cost;
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("PermanentUpgrade: ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"PermanentUpgrade [{id}]: 이름이 비어있습니다");
                return false;
            }

            if (cost < 0)
            {
                Debug.LogError($"PermanentUpgrade [{id}]: 비용이 음수입니다");
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string status = isUnlocked ? $"해금됨 ({timesPurchased}/{maxPurchases})" : "잠김";
            return $"{name} - {cost} 정수 ({status})";
        }
    }

    /// <summary>
    /// 영구 업그레이드 데이터베이스 (JSON 로딩용)
    /// </summary>
    [System.Serializable]
    public class UpgradeDatabase
    {
        public System.Collections.Generic.List<PermanentUpgrade> upgrades;

        public UpgradeDatabase()
        {
            upgrades = new System.Collections.Generic.List<PermanentUpgrade>();
        }
    }
}
