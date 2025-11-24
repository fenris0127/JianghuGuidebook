using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Relics
{
    /// <summary>
    /// 유물을 관리하고 효과를 발동시키는 매니저
    /// </summary>
    public class RelicManager : MonoBehaviour
    {
        private static RelicManager _instance;

        public static RelicManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RelicManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("RelicManager");
                        _instance = go.AddComponent<RelicManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("보유 유물")]
        [SerializeField] private List<Relic> ownedRelics = new List<Relic>();

        [Header("유물 효과")]
        private Dictionary<string, IRelicEffect> relicEffects = new Dictionary<string, IRelicEffect>();

        // Properties
        public List<Relic> OwnedRelics => ownedRelics;

        // Events
        public System.Action<Relic> OnRelicAdded;
        public System.Action<Relic> OnRelicRemoved;
        public System.Action<Relic> OnRelicTriggered;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 유물을 추가합니다
        /// </summary>
        public void AddRelic(Relic relic)
        {
            if (relic == null)
            {
                Debug.LogError("추가하려는 유물이 null입니다");
                return;
            }

            // 중복 체크
            if (ownedRelics.Any(r => r.id == relic.id))
            {
                Debug.LogWarning($"유물 {relic.name}은 이미 보유 중입니다");
                // TODO: 중복 유물 처리 (스택 증가 또는 다른 보상으로 교환)
                return;
            }

            ownedRelics.Add(relic);
            Debug.Log($"유물 획득: {relic}");

            // 유물 효과 생성 및 등록
            RegisterRelicEffect(relic);

            OnRelicAdded?.Invoke(relic);
        }

        /// <summary>
        /// 유물을 제거합니다
        /// </summary>
        public void RemoveRelic(Relic relic)
        {
            if (ownedRelics.Contains(relic))
            {
                ownedRelics.Remove(relic);
                relicEffects.Remove(relic.id);
                Debug.Log($"유물 제거: {relic.name}");
                OnRelicRemoved?.Invoke(relic);
            }
        }

        /// <summary>
        /// ID로 유물을 찾습니다
        /// </summary>
        public Relic GetRelicById(string id)
        {
            return ownedRelics.FirstOrDefault(r => r.id == id);
        }

        /// <summary>
        /// 특정 희귀도의 유물 개수를 반환합니다
        /// </summary>
        public int GetRelicCountByRarity(RelicRarity rarity)
        {
            return ownedRelics.Count(r => r.rarity == rarity);
        }

        /// <summary>
        /// 특정 ID의 유물을 보유하고 있는지 확인합니다
        /// </summary>
        public bool HasRelic(string relicId)
        {
            return ownedRelics.Any(r => r.id == relicId);
        }

        /// <summary>
        /// 유물 효과를 등록합니다
        /// </summary>
        private void RegisterRelicEffect(Relic relic)
        {
            // 유물 ID에 따라 적절한 효과 클래스 생성
            IRelicEffect effect = CreateRelicEffect(relic);
            if (effect != null)
            {
                relicEffects[relic.id] = effect;
            }
        }

        /// <summary>
        /// 유물에 맞는 효과 객체를 생성합니다
        /// </summary>
        private IRelicEffect CreateRelicEffect(Relic relic)
        {
            // TODO: 각 유물 ID에 맞는 구체적인 효과 클래스 반환
            // 지금은 기본 효과 반환
            return new BaseRelicEffect(relic);
        }

        // ===== 유물 효과 트리거 메서드들 =====

        /// <summary>
        /// 전투 시작 시 유물 효과 발동
        /// </summary>
        public void TriggerOnCombatStart(Player player)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnCombatStart))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnCombatStart(player);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 턴 시작 시 유물 효과 발동
        /// </summary>
        public void TriggerOnTurnStart(Player player)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnTurnStart))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnTurnStart(player);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 턴 종료 시 유물 효과 발동
        /// </summary>
        public void TriggerOnTurnEnd(Player player)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnTurnEnd))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnTurnEnd(player);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 카드 사용 시 유물 효과 발동
        /// </summary>
        public void TriggerOnCardPlay(Card card, Player player, Enemy target)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnCardPlay))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnCardPlay(card, player, target);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 공격 시 유물 효과 발동
        /// </summary>
        public void TriggerOnAttack(int damage, Enemy target)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnAttack))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnAttack(damage, target);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 방어 시 유물 효과 발동
        /// </summary>
        public void TriggerOnDefend(int block, Player player)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnDefend))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnDefend(block, player);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 피해 받을 시 유물 효과 발동
        /// </summary>
        public void TriggerOnDamageReceived(int damage, Player player)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnDamageReceived))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnDamageReceived(damage, player);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 적 사망 시 유물 효과 발동
        /// </summary>
        public void TriggerOnEnemyDeath(Enemy enemy)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnEnemyDeath))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnEnemyDeath(enemy);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 카드 드로우 시 유물 효과 발동
        /// </summary>
        public void TriggerOnDraw(Card card)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnDraw))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnDraw(card);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 카드 버릴 시 유물 효과 발동
        /// </summary>
        public void TriggerOnDiscard(Card card)
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnDiscard))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnDiscard(card);
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 휴식 시 유물 효과 발동
        /// </summary>
        public void TriggerOnRest()
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnRest))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnRest();
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 상점 진입 시 유물 효과 발동
        /// </summary>
        public void TriggerOnShop()
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnShop))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnShop();
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 전투 승리 시 유물 효과 발동
        /// </summary>
        public void TriggerOnVictory()
        {
            foreach (var relic in ownedRelics.Where(r => r.effectType == RelicEffectType.OnVictory))
            {
                relic.Trigger();
                if (relicEffects.TryGetValue(relic.id, out IRelicEffect effect))
                {
                    effect.OnVictory();
                }
                OnRelicTriggered?.Invoke(relic);
            }
        }

        /// <summary>
        /// 지속 효과 유물 체크 (특정 조건 충족 여부)
        /// </summary>
        public bool HasPassiveRelic(string relicId)
        {
            return ownedRelics.Any(r => r.id == relicId && r.effectType == RelicEffectType.Passive);
        }

        /// <summary>
        /// 모든 유물을 초기화합니다
        /// </summary>
        public void ResetRelics()
        {
            ownedRelics.Clear();
            relicEffects.Clear();
            Debug.Log("유물 초기화 완료");
        }
    }
}
