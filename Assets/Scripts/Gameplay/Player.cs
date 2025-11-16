using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GangHoBiGeup.Gameplay
{
    // 플레이어 캐릭터의 모든 데이터를 관리하고 핵심 로직을 수행하는 클래스입니다.
    // 컴포넌트 기반 아키텍처로 리팩토링되었습니다.
    public class Player : MonoBehaviour, IDamageable
    {
        #region Components
        private HealthComponent _health;
        private DeckComponent _deck;
        private RealmComponent _realm;
        private ComboComponent _combo;
        private InventoryComponent _inventory;
        private StatusEffectContainer _statusEffects;

        // Lazy initialization for components
        private HealthComponent health => _health ?? (_health = GetComponent<HealthComponent>() ?? gameObject.AddComponent<HealthComponent>());
        private DeckComponent deck => _deck ?? (_deck = GetComponent<DeckComponent>() ?? gameObject.AddComponent<DeckComponent>());
        private RealmComponent realm => _realm ?? (_realm = GetComponent<RealmComponent>() ?? gameObject.AddComponent<RealmComponent>());
        private ComboComponent combo => _combo ?? (_combo = GetComponent<ComboComponent>() ?? gameObject.AddComponent<ComboComponent>());
        private InventoryComponent inventory => _inventory ?? (_inventory = GetComponent<InventoryComponent>() ?? gameObject.AddComponent<InventoryComponent>());
        private StatusEffectContainer statusEffectContainer => _statusEffects ?? (_statusEffects = GetComponent<StatusEffectContainer>() ?? gameObject.AddComponent<StatusEffectContainer>());
        #endregion

        #region TDD Test Properties
        // TDD 테스트를 위한 프로퍼티들 - 컴포넌트로 위임
        public int CurrentHealth
        {
            get => health.CurrentHealth;
            set => health.CurrentHealth = value;
        }

        public int MaxHealth
        {
            get => health.MaxHealth;
            set => health.MaxHealth = value;
        }

        public int Block
        {
            get => health.Defense;
            set => health.Defense = value;
        }

        public int Energy
        {
            get => currentNaegong;
            set => currentNaegong = value;
        }

        public int Gold
        {
            get => inventory.Gold;
            set
            {
                int diff = value - inventory.Gold;
                if (diff > 0) inventory.GainGold(diff);
                else if (diff < 0) inventory.SpendGold(-diff);
            }
        }

        public bool IsDead => health.IsDead;

        // 덱 관리 프로퍼티
        public List<CardData> Deck
        {
            get => deck.GetAllCardsInDeck();
            set => deck.Initialize(value);
        }

        public List<CardData> DrawPile
        {
            get => deck.DrawPile;
            set => deck.DrawPile = value;
        }

        public List<CardData> Hand
        {
            get => deck.Hand;
            set => deck.Hand = value;
        }

        public List<CardData> DiscardPile
        {
            get => deck.DiscardPile;
            set => deck.DiscardPile = value;
        }

        public void ShuffleDeck() => deck.ShuffleDeck();

        public void DiscardCard(CardData card) => deck.DiscardCard(card);

        // 경지 시스템 프로퍼티
        public Realm CurrentRealm
        {
            get => realm.CurrentRealm;
            set => realm.CurrentRealm = value;
        }

        public SwordRealm CurrentSwordRealm
        {
            get => realm.CurrentSwordRealm;
            set => realm.CurrentSwordRealm = value;
        }

        public int CurrentXp
        {
            get => realm.CurrentXp;
            set => realm.CurrentXp = value;
        }

        public int XpToNextRealm
        {
            get => realm.XpToNextRealm;
            set => realm.XpToNextRealm = value;
        }

        public bool IsReadyToAscend
        {
            get => realm.IsReadyToAscend;
            set => realm.IsReadyToAscend = value;
        }

        public int MaxNaegong
        {
            get => realm.MaxNaegong;
            set => realm.MaxNaegong = value;
        }

        public void AscendRealm(Realm newRealm) => realm.AscendRealm(newRealm);

        public void RecordSwordEvent(string eventType) => realm.RecordEvent(eventType);

        // 상태이상 관리 - TDD 테스트용 메서드
        public void ApplyStatusEffect(StatusEffectType type, int value, int duration = -1)
        {
            StatusEffectData data = GetStatusEffectData(type);
            if (data == null) return;

            StatusEffect effect = new StatusEffect(data, value);
            ApplyStatusEffect(effect);
        }

        private StatusEffectData GetStatusEffectData(StatusEffectType type)
        {
            var data = ScriptableObject.CreateInstance<StatusEffectData>();
            data.type = type;
            data.effectName = type.ToString();
            data.assetID = type.ToString();
            return data;
        }

        public int CalculateDamage(int baseDamage)
        {
            int strength = GetStatusEffectValue(StatusEffectType.Strength);
            int weak = GetStatusEffectValue(StatusEffectType.Weak);

            return baseDamage + strength - weak;
        }
        #endregion

        #region Events
        public event Action<int, int, int, int, int> OnStatsChanged; // curHP, maxHP, def, curN, maxN
        public event Action<List<CardData>> OnHandChanged;
        public event Action<int, int, int> OnPilesChanged; // draw, discard, exhaust
        public event Action<int> OnGoldChanged;
        public event Action<List<RelicData>> OnRelicsChanged;
        public event Action<List<StatusEffect>> OnStatusEffectsChanged;
        #endregion

        #region State Variables
        // Stats
        public int maxHealth => health.MaxHealth;
        public int currentHealth => health.CurrentHealth;
        public int defense => health.Defense;
        public int gold => inventory.Gold;
        public int maxNaegong => realm.MaxNaegong;
        public int currentNaegong { get; private set; }

        // Decks
        public List<CardData> drawPile => deck.DrawPile;
        public List<CardData> hand => deck.Hand;
        public List<CardData> discardPile => deck.DiscardPile;
        public List<CardData> exhaustPile => deck.ExhaustPile;

        // Growth
        public List<RelicData> relics => inventory.Relics;

        // Realm System
        public Realm currentRealm => realm.CurrentRealm;
        public SwordRealm currentSwordRealm => realm.CurrentSwordRealm;
        public int currentXp => realm.CurrentXp;
        public int xpToNextRealm => realm.XpToNextRealm;
        public bool isReadyToAscend => realm.IsReadyToAscend;

        // Combat tracking
        private bool wasDamagedThisTurn;
        #endregion

        void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Lazy initialization으로 컴포넌트는 자동 생성되므로, 이벤트만 구독
            // 컴포넌트 접근 시 자동으로 생성됨을 보장
            var _ = health; // Force initialization
            var __ = statusEffectContainer;

            // 이벤트 구독
            health.OnStatsChanged += (curHP, maxHP, def) =>
            {
                OnStatsChanged?.Invoke(curHP, maxHP, def, currentNaegong, realm.MaxNaegong);
            };

            deck.OnHandChanged += (cards) => OnHandChanged?.Invoke(cards);
            deck.OnPilesChanged += (draw, discard, exhaust) => OnPilesChanged?.Invoke(draw, discard, exhaust);

            inventory.OnGoldChanged += (g) => OnGoldChanged?.Invoke(g);
            inventory.OnRelicsChanged += (r) => OnRelicsChanged?.Invoke(r);

            realm.OnMaxNaegongChanged += (maxN) =>
            {
                currentNaegong = maxN;
                NotifyStatsChanged();
            };

            statusEffectContainer.OnStatusEffectsChanged += (effects) =>
            {
                OnStatusEffectsChanged?.Invoke(effects);
            };
        }

        public void Setup(List<CardData> startingDeck, int bonusHealth)
        {
            health.Initialize(80 + bonusHealth);
            deck.Initialize(startingDeck);
            realm.Initialize(Realm.Samryu);
            combo.ResetComboHistory();
            inventory.Initialize(0);
            statusEffectContainer.Initialize(this);

            currentNaegong = realm.MaxNaegong;

            NotifyAllEvents();
        }

        public void StartTurn()
        {
            health.ResetDefense();
            wasDamagedThisTurn = false;
            currentNaegong = realm.MaxNaegong;

            ProcessStatusEffectsOnTurnStart();

            int extraDraw = inventory.GetRelicEffectValue(RelicTrigger.OnTurnStart, GameEffectType.DrawCard);
            deck.DrawCards(5 + extraDraw);

            int startingBlock = inventory.GetRelicEffectValue(RelicTrigger.OnTurnStart, GameEffectType.Block);
            if (startingBlock > 0) health.GainDefense(startingBlock);

            NotifyStatsChanged();
        }

        public void EndTurn()
        {
            deck.DiscardHand();
            combo.ResetComboHistory();
        }

        public void PlayCard(CardData card, Component target)
        {
            if (currentNaegong < card.cost) return;

            currentNaegong -= card.cost;
            hand.Remove(card);

            if (card.isJeolcho)
                exhaustPile.Add(card);
            else
                discardPile.Add(card);

            BattleManager.Instance.ProcessCardEffect(card, this);

            realm.RecordEvent(EventConstants.CARD_PLAYED, card);

            OnHandChanged?.Invoke(new List<CardData>(hand));
            OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
            NotifyStatsChanged();
        }

        public void TakeDamage(int damage)
        {
            float multiplier = 1f;
            if (GetStatusEffectValue(StatusEffectType.Vulnerable) > 0)
                multiplier = 1.5f;

            int actualDamage = health.TakeDamage(damage, multiplier);
            wasDamagedThisTurn = actualDamage > 0;

            if (actualDamage > 0)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.takeDamageSound);
                if (FeedbackManager.Instance != null)
                    FeedbackManager.Instance.ShowDamageNumber(actualDamage, transform.position);
            }

            if (health.IsDead)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.EndBattle(false, false);
            }
        }

        public void Heal(int amount) => health.Heal(amount);

        #region Stats Up
        public void IncreaseMaxHealth(int amount) => health.IncreaseMaxHealth(amount);

        public void GainDefense(int amount) => health.GainDefense(amount);

        public void GainNaegong(int amount)
        {
            currentNaegong = Mathf.Min(currentNaegong + amount, realm.MaxNaegong);
            NotifyStatsChanged();
        }
        #endregion

        #region Gold Management
        public void GainGold(int amount) => inventory.GainGold(amount);

        public bool SpendGold(int amount) => inventory.SpendGold(amount);
        #endregion

        #region Status Effect
        public void ApplyStatusEffect(StatusEffect effectToApply)
        {
            statusEffectContainer.ApplyStatusEffect(effectToApply);
        }

        public int GetStatusEffectValue(StatusEffectType type)
        {
            return statusEffectContainer.GetStatusEffectValue(type);
        }

        public void ProcessStatusEffectsOnTurnStart()
        {
            statusEffectContainer.ProcessStatusEffectsOnTurnStart();
        }

        public void ProcessStatusEffectsOnTurnEnd()
        {
            if (!wasDamagedThisTurn)
                realm.RecordEvent(EventConstants.ZERO_DAMAGE_TURN);

            statusEffectContainer.ProcessStatusEffectsOnTurnEnd();
        }
        #endregion

        #region Relic
        public void AddRelic(RelicData newRelic) => inventory.AddRelic(newRelic, this);

        public int GetRelicEffectValue(RelicTrigger trigger, GameEffectType type) =>
            inventory.GetRelicEffectValue(trigger, type);

        public void ApplyCombatStartRelicEffects() => inventory.ApplyCombatStartRelicEffects(this);
        #endregion

        #region Upgrade Cards
        public bool UpgradeCard(CardData cardToUpgrade) => deck.UpgradeCard(cardToUpgrade);

        public void UpgradeRandomCard() => deck.UpgradeRandomCard();
        #endregion

        #region Remove Cards
        public bool RemoveCardFromDeck(CardData cardToRemove) => deck.RemoveCardFromDeck(cardToRemove);

        public void RemoveRandomCards(int count) => deck.RemoveRandomCards(count);

        public void RemoveAllBasicCards() => deck.RemoveAllBasicCards();

        public void RemoveAllCurseCards() => deck.RemoveAllCurseCards();
        #endregion

        #region Card Management
        public List<CardData> GetAllCardsInDeck() => deck.GetAllCardsInDeck();

        public void AddCardToDeck(CardData newCard) => deck.AddCardToDeck(newCard);

        public void DrawCards(int amount) => deck.DrawCards(amount);
        #endregion

        public void GainXp(int amount) => realm.GainXp(amount);

        #region Technique Realm Methods
        public void RecordEvent(string eventType, CardData card = null, int value = 1)
        {
            realm.RecordEvent(eventType, card, value);
        }

        public void ResetCombatRecords() => realm.ResetCombatRecords();

        public void AscendSwordRealm(SwordRealm newRealm) => realm.AscendSwordRealm(newRealm);
        #endregion

        // 불러온 RunData를 기반으로 플레이어의 모든 상태를 복원합니다.
        public void RestoreState(RunData data)
        {
            health.RestoreState(data.playerMaxHealth, data.playerCurrentHealth);

            var loadedRelics = data.relicIDs.Select(id => ResourceManager.Instance.GetRelicData(id)).ToList();
            var loadedDrawPile = data.drawPileIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
            var loadedDiscardPile = data.discardPileIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
            var loadedHand = data.handIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
            var loadedExhaustPile = data.exhaustPileIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();

            deck.RestoreState(loadedDrawPile, loadedDiscardPile, loadedHand, loadedExhaustPile);
            realm.RestoreState(data.currentRealm, data.currentXp, data.xpToNextRealm, data.currentSwordRealm);
            inventory.RestoreState(data.playerGold, loadedRelics);

            currentNaegong = realm.MaxNaegong;

            NotifyAllEvents();
        }

        #region Combo System
        public void RegisterCombo(ComboData comboData) => combo.RegisterCombo(comboData);

        public bool RecordCardPlay(CardData card) => combo.RecordCardPlay(card);

        public List<CardData> GetCardPlayHistory() => combo.GetCardPlayHistory();
        #endregion

        #region Notify
        private void NotifyStatsChanged()
        {
            OnStatsChanged?.Invoke(health.CurrentHealth, health.MaxHealth, health.Defense,
                                  currentNaegong, realm.MaxNaegong);
        }

        private void NotifyAllEvents()
        {
            NotifyStatsChanged();
            OnHandChanged?.Invoke(new List<CardData>(hand));
            OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
            OnGoldChanged?.Invoke(gold);
            OnRelicsChanged?.Invoke(relics);
            OnStatusEffectsChanged?.Invoke(statusEffectContainer.GetAllStatusEffects());
        }
        #endregion
    }
}
