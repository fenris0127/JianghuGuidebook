using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Gameplay
{
    /// <summary>
    /// 골드와 유물을 관리하는 컴포넌트
    /// </summary>
    public class InventoryComponent : MonoBehaviour
    {
        public event Action<int> OnGoldChanged;
        public event Action<List<RelicData>> OnRelicsChanged;

        public int Gold { get; private set; }
        public List<RelicData> Relics { get; private set; } = new List<RelicData>();

        /// <summary>
        /// 인벤토리를 초기화합니다.
        /// </summary>
        public void Initialize(int startingGold = 0)
        {
            Gold = startingGold;
            Relics.Clear();

            OnGoldChanged?.Invoke(Gold);
            OnRelicsChanged?.Invoke(new List<RelicData>(Relics));
        }

        /// <summary>
        /// 골드를 획득합니다.
        /// </summary>
        public void GainGold(int amount)
        {
            Gold += amount;
            OnGoldChanged?.Invoke(Gold);
        }

        /// <summary>
        /// 골드를 소비합니다.
        /// </summary>
        /// <returns>충분한 골드가 있으면 true, 없으면 false</returns>
        public bool SpendGold(int amount)
        {
            if (Gold < amount) return false;
            Gold -= amount;
            OnGoldChanged?.Invoke(Gold);
            return true;
        }

        /// <summary>
        /// 유물을 추가합니다.
        /// </summary>
        public void AddRelic(RelicData newRelic, IDamageable owner)
        {
            Relics.Add(newRelic);

            // 획득 시 발동하는 유물 효과 처리
            if (newRelic.trigger == RelicTrigger.OnPickup)
            {
                foreach (var effect in newRelic.effects)
                    EffectProcessor.Process(effect, owner, owner);
            }

            OnRelicsChanged?.Invoke(new List<RelicData>(Relics));
        }

        /// <summary>
        /// 특정 트리거와 효과 타입에 해당하는 유물 효과 값의 합을 반환합니다.
        /// </summary>
        public int GetRelicEffectValue(RelicTrigger trigger, GameEffectType type)
        {
            return Relics
                .Where(r => r.trigger == trigger)
                .SelectMany(r => r.effects)
                .Where(e => e.type == type)
                .Sum(e => e.value);
        }

        /// <summary>
        /// 전투 시작 시 발동하는 유물 효과를 적용합니다.
        /// </summary>
        public void ApplyCombatStartRelicEffects(IDamageable owner)
        {
            var combatStartEffects = Relics
                .Where(r => r.trigger == RelicTrigger.OnCombatStart)
                .SelectMany(r => r.effects);

            foreach (var effect in combatStartEffects)
                EffectProcessor.Process(effect, owner, null);
        }

        /// <summary>
        /// 상태를 복원합니다 (세이브 로드 시).
        /// </summary>
        public void RestoreState(int gold, List<RelicData> relics)
        {
            Gold = gold;
            Relics = new List<RelicData>(relics);

            OnGoldChanged?.Invoke(Gold);
            OnRelicsChanged?.Invoke(new List<RelicData>(Relics));
        }
    }
}
