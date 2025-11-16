using UnityEngine;
using System;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Gameplay
{
    /// <summary>
    /// 경지 시스템과 검법 경지 시스템을 관리하는 컴포넌트
    /// </summary>
    public class RealmComponent : MonoBehaviour
    {
        public event Action<int> OnMaxNaegongChanged;
        public event Action<Realm, int, int, bool> OnRealmChanged; // currentRealm, currentXp, xpToNext, isReadyToAscend
        public event Action<SwordRealm> OnSwordRealmChanged;

        public Realm CurrentRealm { get; set; } = Realm.Samryu;
        public SwordRealm CurrentSwordRealm { get; set; } = SwordRealm.None;
        public int CurrentXp { get; set; } = 0;
        public int XpToNextRealm { get; set; } = 10;
        public bool IsReadyToAscend { get; set; } = false;
        public int MaxNaegong { get; set; } = 3;

        // 검법 경지 관련 카운터
        private int attacksThisCombat;
        private int zeroDamageTurns;
        private int zeroCostCardsThisCombat;

        /// <summary>
        /// 경지 시스템을 초기화합니다.
        /// </summary>
        public void Initialize(Realm startingRealm = Realm.Samryu)
        {
            CurrentRealm = startingRealm;
            CurrentSwordRealm = SwordRealm.None;
            CurrentXp = 0;
            MaxNaegong = GetMaxNaegongForRealm(startingRealm);
            XpToNextRealm = GetXpRequiredForNextRealm(startingRealm);
            IsReadyToAscend = false;

            NotifyRealmChanged();
            OnMaxNaegongChanged?.Invoke(MaxNaegong);
        }

        /// <summary>
        /// 경험치를 획득합니다.
        /// </summary>
        public void GainXp(int amount)
        {
            if (CurrentRealm == Realm.Saengsagyeong) return;

            CurrentXp += amount;
            if (CurrentXp >= XpToNextRealm)
                IsReadyToAscend = true;

            NotifyRealmChanged();
        }

        /// <summary>
        /// 경지를 상승시킵니다.
        /// </summary>
        public void AscendRealm(Realm newRealm)
        {
            if (newRealm <= CurrentRealm) return;

            CurrentRealm = newRealm;

            // 경지 상승 시 최대 내공 증가
            MaxNaegong = GetMaxNaegongForRealm(newRealm);

            // XP 초기화 및 다음 경지 목표 설정
            CurrentXp = 0;
            IsReadyToAscend = false;
            XpToNextRealm = GetXpRequiredForNextRealm(newRealm);

            NotifyRealmChanged();
            OnMaxNaegongChanged?.Invoke(MaxNaegong);
        }

        /// <summary>
        /// 검법 경지를 상승시킵니다.
        /// </summary>
        public void AscendSwordRealm(SwordRealm newRealm)
        {
            if (newRealm <= CurrentSwordRealm) return;
            CurrentSwordRealm = newRealm;
            OnSwordRealmChanged?.Invoke(CurrentSwordRealm);
        }

        /// <summary>
        /// 전투 기록을 초기화합니다 (전투 시작 시).
        /// </summary>
        public void ResetCombatRecords()
        {
            attacksThisCombat = 0;
            zeroCostCardsThisCombat = 0;
        }

        /// <summary>
        /// 이벤트를 기록하고 검법 경지 조건을 확인합니다.
        /// </summary>
        public void RecordEvent(string eventType, CardData card = null, int value = 1)
        {
            switch (eventType)
            {
                case EventConstants.CARD_PLAYED:
                    if (card != null && card.cost == 0)
                        zeroCostCardsThisCombat++;
                    break;
                case EventConstants.ZERO_DAMAGE_TURN:
                    zeroDamageTurns += value;
                    break;
                case EventConstants.ATTACK:
                    attacksThisCombat += value;
                    break;
            }

            CheckForSwordRealmAscension();
        }

        /// <summary>
        /// 검법 경지 상승 조건을 확인합니다.
        /// </summary>
        private void CheckForSwordRealmAscension()
        {
            switch (CurrentSwordRealm)
            {
                case SwordRealm.None:
                    if (attacksThisCombat >= 10)
                        AscendSwordRealm(SwordRealm.Geomgi);
                    break;
                case SwordRealm.Geomgi:
                    if (zeroDamageTurns >= 3)
                        AscendSwordRealm(SwordRealm.Geomsa);
                    break;
            }
        }

        /// <summary>
        /// 상태를 복원합니다 (세이브 로드 시).
        /// </summary>
        public void RestoreState(Realm realm, int xp, int xpToNext, SwordRealm swordRealm)
        {
            CurrentRealm = realm;
            CurrentXp = xp;
            XpToNextRealm = xpToNext;
            CurrentSwordRealm = swordRealm;
            MaxNaegong = GetMaxNaegongForRealm(realm);
            IsReadyToAscend = xp >= xpToNext && realm != Realm.Saengsagyeong;

            NotifyRealmChanged();
            OnMaxNaegongChanged?.Invoke(MaxNaegong);
            OnSwordRealmChanged?.Invoke(CurrentSwordRealm);
        }

        #region Configuration Methods
        // TODO: 이 부분을 나중에 ScriptableObject 설정 파일로 리팩토링
        private int GetMaxNaegongForRealm(Realm realm)
        {
            switch (realm)
            {
                case Realm.Samryu: return 3;
                case Realm.Iryu: return 4;
                case Realm.Illyu: return 4;
                case Realm.Jeoljeong: return 5;
                case Realm.Chojeoljeong: return 5;
                case Realm.Hwagyeong: return 6;
                case Realm.Saengsagyeong: return 7;
                default: return 3;
            }
        }

        private int GetXpRequiredForNextRealm(Realm realm)
        {
            switch (realm)
            {
                case Realm.Samryu: return 10;
                case Realm.Iryu: return 15;
                case Realm.Illyu: return 20;
                case Realm.Jeoljeong: return 30;
                case Realm.Chojeoljeong: return 40;
                case Realm.Hwagyeong: return 50;
                case Realm.Saengsagyeong: return 0; // 최고 경지
                default: return 10;
            }
        }
        #endregion

        private void NotifyRealmChanged()
        {
            OnRealmChanged?.Invoke(CurrentRealm, CurrentXp, XpToNextRealm, IsReadyToAscend);
        }
    }

    /// <summary>
    /// 이벤트 타입 상수 (문자열 리터럴 대신 사용)
    /// </summary>
    public static class EventConstants
    {
        public const string CARD_PLAYED = "CardPlayed";
        public const string ZERO_DAMAGE_TURN = "ZeroDamageTurn";
        public const string ATTACK = "Attack";
        public const string JEOLCHO_CARD_USED = "JeolchoCardUsed";
    }
}
