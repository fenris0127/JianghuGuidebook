using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 경지(Realm) 시스템 설정을 관리하는 ScriptableObject
    /// 각 경지별 최대 내공과 필요 경험치를 설정합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "RealmConfig", menuName = "GangHoBiGeup/Config/Realm Config")]
    public class RealmConfig : ScriptableObject
    {
        [System.Serializable]
        public class RealmSettings
        {
            [Tooltip("경지 단계")]
            public Realm realm;

            [Tooltip("이 경지의 최대 내공")]
            public int maxNaegong;

            [Tooltip("다음 경지로 가기 위한 필요 경험치 (최고 경지는 0)")]
            public int xpRequired;
        }

        [Header("=== 경지별 설정 ===")]
        [Tooltip("각 경지별 최대 내공과 필요 경험치 설정")]
        public RealmSettings[] realmSettings = new RealmSettings[]
        {
            new RealmSettings { realm = Realm.Samryu, maxNaegong = 3, xpRequired = 10 },
            new RealmSettings { realm = Realm.Iryu, maxNaegong = 4, xpRequired = 15 },
            new RealmSettings { realm = Realm.Illyu, maxNaegong = 4, xpRequired = 20 },
            new RealmSettings { realm = Realm.Jeoljeong, maxNaegong = 5, xpRequired = 30 },
            new RealmSettings { realm = Realm.Chojeoljeong, maxNaegong = 5, xpRequired = 40 },
            new RealmSettings { realm = Realm.Hwagyeong, maxNaegong = 6, xpRequired = 50 },
            new RealmSettings { realm = Realm.Saengsagyeong, maxNaegong = 7, xpRequired = 0 }, // 최고 경지
        };

        [Header("=== 검법 경지 조건 ===")]
        [Tooltip("검기 경지 달성에 필요한 공격 횟수")]
        public int geomgiRequiredAttacks = 10;

        [Tooltip("검사 경지 달성에 필요한 무피해 턴 수")]
        public int geomsaRequiredZeroDamageTurns = 3;

        /// <summary>
        /// 특정 경지의 최대 내공을 반환합니다.
        /// </summary>
        public int GetMaxNaegongForRealm(Realm realm)
        {
            foreach (var settings in realmSettings)
            {
                if (settings.realm == realm)
                    return settings.maxNaegong;
            }

            // 기본값 (Samryu)
            return 3;
        }

        /// <summary>
        /// 특정 경지에서 다음 경지로 가기 위한 필요 경험치를 반환합니다.
        /// </summary>
        public int GetXpRequiredForNextRealm(Realm realm)
        {
            foreach (var settings in realmSettings)
            {
                if (settings.realm == realm)
                    return settings.xpRequired;
            }

            // 기본값
            return 10;
        }

        /// <summary>
        /// 검법 경지 상승 조건을 확인합니다.
        /// </summary>
        public bool CheckSwordRealmAscension(SwordRealm currentRealm, int attackCount, int zeroDamageTurns)
        {
            switch (currentRealm)
            {
                case SwordRealm.None:
                    return attackCount >= geomgiRequiredAttacks;
                case SwordRealm.Geomgi:
                    return zeroDamageTurns >= geomsaRequiredZeroDamageTurns;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 다음 검법 경지를 반환합니다.
        /// </summary>
        public SwordRealm GetNextSwordRealm(SwordRealm currentRealm)
        {
            switch (currentRealm)
            {
                case SwordRealm.None:
                    return SwordRealm.Geomgi;
                case SwordRealm.Geomgi:
                    return SwordRealm.Geomsa;
                default:
                    return currentRealm;
            }
        }
    }
}
