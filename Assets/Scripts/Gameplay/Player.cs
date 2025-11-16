using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GangHoBiGeup.Gameplay
{
    // 플레이어 캐릭터의 모든 데이터를 관리하고 핵심 로직을 수행하는 클래스입니다.
    public class Player : MonoBehaviour, IDamageable
    {
    #region TDD Test Properties
    // TDD 테스트를 위한 간단한 프로퍼티들
    public int CurrentHealth 
    { 
    get => currentHealth; 
    set => currentHealth = value; 
    }
    
    public int MaxHealth 
    { 
    get => maxHealth; 
    set => maxHealth = value; 
    }
    
    public int Block 
    { 
    get => defense; 
    set => defense = value; 
    }
    
    public int Energy 
    { 
    get => currentNaegong; 
    set => currentNaegong = value; 
    }
    
    public int Gold 
    { 
    get => gold; 
    set => gold = value; 
    }
    
    public bool IsDead => currentHealth <= 0;
    
        // 덱 관리 프로퍼티
        public List<CardData> Deck
        {
            get => GetAllCardsInDeck();
            set
            {
                drawPile = new List<CardData>(value);
                hand.Clear();
                discardPile.Clear();
            }
        }
        
        public List<CardData> DrawPile
        {
            get => drawPile;
            set => drawPile = value;
        }
        
        public List<CardData> Hand
        {
            get => hand;
            set => hand = value;
        }
        
        public List<CardData> DiscardPile
        {
            get => discardPile;
            set => discardPile = value;
        }
        
        public void ShuffleDeck()
        {
            drawPile.Shuffle();
        }
        
        public void DiscardCard(CardData card)
        {
            if (hand.Remove(card))
            {
                discardPile.Add(card);
                OnHandChanged?.Invoke(new List<CardData>(hand));
                OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
            }
        }
        
        // 경지 시스템 프로퍼티
        public Realm CurrentRealm
        {
            get => currentRealm;
            set => currentRealm = value;
        }
        
        public SwordRealm CurrentSwordRealm
        {
            get => currentSwordRealm;
            set => currentSwordRealm = value;
        }
        
        public int CurrentXp
        {
            get => currentXp;
            set => currentXp = value;
        }
        
        public int XpToNextRealm
        {
            get => xpToNextRealm;
            set => xpToNextRealm = value;
        }
        
        public bool IsReadyToAscend
        {
            get => isReadyToAscend;
            set => isReadyToAscend = value;
        }
        
        public int MaxNaegong
        {
            get => maxNaegong;
            set => maxNaegong = value;
        }
        
        public void AscendRealm(Realm newRealm)
        {
            if (newRealm <= currentRealm) return;
            
            currentRealm = newRealm;
            
            // 경지 상승 시 최대 내공 증가
            maxNaegong = GetMaxNaegongForRealm(newRealm);
            
            // XP 초기화 및 다음 경지 목표 설정
            currentXp = 0;
            isReadyToAscend = false;
            xpToNextRealm = GetXpRequiredForNextRealm(newRealm);
        }
        
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
        
        public void RecordSwordEvent(string eventType)
        {
            switch (eventType)
            {
                case "Attack":
                    attacksThisCombat++;
                    if (currentSwordRealm == SwordRealm.None && attacksThisCombat >= 10)
                    {
                        AscendSwordRealm(SwordRealm.Geomgi);
                    }
                    break;
                case "ZeroDamageTurn":
                    zeroDamageTurns++;
                    if (currentSwordRealm == SwordRealm.Geomgi && zeroDamageTurns >= 3)
                    {
                        AscendSwordRealm(SwordRealm.Geomsa);
                    }
                    break;
            }
        }
        
        // 상태이상 관리 - TDD 테스트용 메서드
        public void ApplyStatusEffect(StatusEffectType type, int value, int duration = -1)
        {
            // StatusEffectData를 찾아서 StatusEffect 생성
            StatusEffectData data = GetStatusEffectData(type);
            if (data == null) return;
            
            StatusEffect effect = new StatusEffect(data, value);
            ApplyStatusEffect(effect);
        }
        
        private StatusEffectData GetStatusEffectData(StatusEffectType type)
        {
            // ResourceManager를 통해 데이터를 가져오거나, 간단하게 ScriptableObject 생성
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
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public int defense { get; private set; }
    public int gold { get; private set; }
    public int maxNaegong { get; private set; } = 3;
    public int currentNaegong { get; private set; }

    // Decks
    public List<CardData> drawPile { get; private set; } = new List<CardData>();
    public List<CardData> hand { get; private set; } = new List<CardData>();
    public List<CardData> discardPile { get; private set; } = new List<CardData>();
    public List<CardData> exhaustPile { get; private set; } = new List<CardData>();
    
    // Growth
    public List<RelicData> relics { get; private set; } = new List<RelicData>();
    private List<StatusEffectBehavior> statusEffects = new List<StatusEffectBehavior>();
    
    // Realm System
    public Realm currentRealm { get; private set; } = Realm.Samryu;
    public SwordRealm currentSwordRealm { get; private set; } = SwordRealm.None;
    public int currentXp { get; private set; } = 0;
    public int xpToNextRealm { get; private set; } = 10;
    public bool isReadyToAscend { get; private set; } = false;

    // Sword Realm Records
    private int attacksThisCombat;
    private int zeroDamageTurns;
    private bool wasDamagedThisTurn;
    private int zeroCostCardsThisCombat;
    private int jeolchoKills;
    #endregion
    
    public void Setup(List<CardData> startingDeck, int bonusHealth)
    {
        maxHealth = 80 + bonusHealth;
        currentHealth = maxHealth;
        gold = 0;
        
        drawPile = new List<CardData>(startingDeck);
        hand.Clear();
        discardPile.Clear();
        exhaustPile.Clear();
        relics.Clear();
        statusEffects.Clear();

        drawPile.Shuffle();
        
        NotifyStatsChanged();
        OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
        OnGoldChanged?.Invoke(gold);
        OnRelicsChanged?.Invoke(relics);
        OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
    }

    public void StartTurn()
    {
        defense = 0;
        wasDamagedThisTurn = false;
        currentNaegong = maxNaegong;

        ProcessStatusEffectsOnTurnStart();

        // "턴 시작 시 발동하는 드로우 카드 효과"의 총합을 가져옵니다.
        int extraDraw = GetRelicEffectValue(RelicTrigger.OnTurnStart, GameEffectType.DrawCard);
        DrawCards(5 + extraDraw);
        
        // "턴 시작 시 발동하는 방어도 획득 효과"의 총합을 가져옵니다.
        int startingBlock = GetRelicEffectValue(RelicTrigger.OnTurnStart, GameEffectType.Block);
        if(startingBlock > 0) GainDefense(startingBlock);
        NotifyStatsChanged();
    }

    public void EndTurn()
    {
        // 핸드의 모든 카드를 버린 카드 더미로 이동
        while (hand.Count > 0)
        {
            var card = hand[0];
            hand.RemoveAt(0);
            discardPile.Add(card);
        }

        OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
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
        
        RecordEvent("CardPlayed", card);
        
        OnHandChanged?.Invoke(new List<CardData>(hand));
        OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
        NotifyStatsChanged();
    }

    public void TakeDamage(int damage)
    {
        float multiplier = 1f;
        if (GetStatusEffectValue(StatusEffectType.Vulnerable) > 0) multiplier = 1.5f;

        int modifiedDamage = Mathf.RoundToInt(damage * multiplier);
        
        // 방어도가 먼저 감소함
        if (defense > 0)
        {
            int blockedDamage = Mathf.Min(defense, modifiedDamage);
            defense -= blockedDamage;
            modifiedDamage -= blockedDamage;
        }
        
        // 남은 데미지로 체력 감소
        currentHealth -= modifiedDamage;
        wasDamagedThisTurn = true;
        
        if (modifiedDamage > 0)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySound(AudioManager.Instance.takeDamageSound);
            if (FeedbackManager.Instance != null)
                FeedbackManager.Instance.ShowDamageNumber(modifiedDamage, transform.position);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (GameManager.Instance != null)
                GameManager.Instance.EndBattle(false, false);
        }

        NotifyStatsChanged();
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        NotifyStatsChanged();
    }
    
    #region Stats Up
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        Heal(amount);
        NotifyStatsChanged();
    }

    public void GainDefense(int amount)
    {
        defense += amount;
        NotifyStatsChanged();
    }

    public void GainNaegong(int amount)
    {
        currentNaegong = Mathf.Min(currentNaegong + amount, maxNaegong);
        NotifyStatsChanged();
    }
    
    #endregion

    #region Gold Management
    public void GainGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }
    #endregion
    
    #region Status Effect
    public void ApplyStatusEffect(StatusEffect effectToApply)
    {
        var existingEffect = statusEffects.Find(e => e.Effect.Data != null && e.Effect.Data.type == effectToApply.Data.type);
        if (existingEffect != null)
            existingEffect.Effect.Value += effectToApply.Value;
        else
        {
            {
                StatusEffectBehavior newBehavior = StatusEffectFactory.Create(effectToApply);
                newBehavior.Setup(effectToApply, this);
                statusEffects.Add(newBehavior);
                newBehavior.OnApplied();
            }
        }

        OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
    }

    public int GetStatusEffectValue(StatusEffectType type)
    {
        var behavior = statusEffects.Find(b => b.Effect.Data.type == type);
        return behavior?.Effect.Value ?? 0;
    }
    public void ProcessStatusEffectsOnTurnStart()
    {
        bool changed = false;
    for(int i = statusEffects.Count - 1; i >= 0; i--)
    {
        statusEffects[i].OnTurnStart();
        if(statusEffects[i].IsFinished())
        {
            statusEffects.RemoveAt(i);
            changed = true;
        }
    }
    
    if (changed)
        OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
    }
    
    public void ProcessStatusEffectsOnTurnEnd()
    {
        if (!wasDamagedThisTurn) RecordEvent("ZeroDamageTurn");

        bool changed = false;
        for(int i = statusEffects.Count - 1; i >= 0; i--)
        {
            statusEffects[i].OnTurnEnd();
            if(statusEffects[i].IsFinished())
            {
                statusEffects.RemoveAt(i);
                changed = true;
            }
        }
        
        if (changed)
            OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
    }
    #endregion

    #region Relic
    public void AddRelic(RelicData newRelic)
    {
        relics.Add(newRelic);
        if (newRelic.trigger == RelicTrigger.OnPickup)
        {
            foreach (var effect in newRelic.effects)
                EffectProcessor.Process(effect, this, this);
        }

        OnRelicsChanged?.Invoke(new List<RelicData>(relics));
    }

    // 1. 올바른 발동 시점(trigger)을 가진 유물만 필터링합니다.
    // 2. 각 유물이 가진 효과(effects) 목록을 하나의 큰 목록으로 펼칩니다.
    // 3. 펼쳐진 목록에서 우리가 원하는 효과 종류(type)만 다시 필터링합니다.
    // 4. 필터링된 모든 효과의 value 값을 합산하여 반환합니다.
    public int GetRelicEffectValue(RelicTrigger trigger, GameEffectType type) => relics.Where(r => r.trigger == trigger).SelectMany(r => r.effects).Where(e => e.type == type).Sum(e => e.value);
    public void ApplyCombatStartRelicEffects()
    {
         // '전투 시작 시' 발동하는 모든 유물 효과를 찾아서 처리합니다.
        var combatStartEffects = relics.Where(r => r.trigger == RelicTrigger.OnCombatStart).SelectMany(r => r.effects);

        foreach (var effect in combatStartEffects)
            EffectProcessor.Process(effect, this, null); // 전투 시작 시에는 주 대상(primaryTarget)이 없음
    }
    #endregion

    #region Upgrade Cards
    public bool UpgradeCard(CardData cardToUpgrade)
    {
        if (cardToUpgrade.upgradedVersion == null || cardToUpgrade.isUpgraded)
        {
            Debug.LogWarning($"[{cardToUpgrade.cardName}] 카드는 강화할 수 없습니다.");
            return false;
        }

        CardData upgradedCard = cardToUpgrade.upgradedVersion;

        // 덱의 모든 위치(뽑을 더미, 버린 더미, 핸드)에서 카드를 찾아 교체합니다.
        bool success = ReplaceCardInList(drawPile, cardToUpgrade, upgradedCard) ||
                       ReplaceCardInList(discardPile, cardToUpgrade, upgradedCard) ||
                       ReplaceCardInList(hand, cardToUpgrade, upgradedCard);

        if (success)
        {
            Debug.Log($"[{cardToUpgrade.cardName}] -> [{upgradedCard.cardName}] (으)로 강화 성공.");
            OnHandChanged?.Invoke(new List<CardData>(hand));
            OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
        }

        return success;
    }
       
    public void UpgradeRandomCard()
    {
        var upgradableCards = GetAllCardsInDeck().FindAll(c => !c.isUpgraded && c.upgradedVersion != null);
        if (upgradableCards.Count > 0)
        {
            CardData cardToUpgrade = upgradableCards[UnityEngine.Random.Range(0, upgradableCards.Count)];
            UpgradeCard(cardToUpgrade);
        }
    }
    #endregion

    #region Remove Cards
    public bool RemoveCardFromDeck(CardData cardToRemove)
    {
        // 핸드, 버린 덱, 뽑을 덱 순서로 카드를 찾아 제거 시도
        bool removed = hand.Remove(cardToRemove) ||
                    discardPile.Remove(cardToRemove) ||
                    drawPile.Remove(cardToRemove);
        
        if (removed)
        {
            Debug.Log($"[{cardToRemove.cardName}] 카드를 덱에서 제거했습니다.");
            // 덱 구성이 변경되었으므로 UI 갱신을 위해 이벤트를 호출합니다.
            OnHandChanged?.Invoke(new List<CardData>(hand));
            OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
        }
        else
        {
            Debug.LogWarning($"[{cardToRemove.cardName}] 카드를 덱에서 찾을 수 없어 제거에 실패했습니다.");
        }
        return removed;
    }

    public void RemoveRandomCards(int count)
    {
        // 기본 카드(공격/방어)가 아닌 카드만 제거 대상으로 필터링합니다.
        var removableCards = GetAllCardsInDeck().Where(c => c.rarity != CardRarity.Common).ToList();
        
        // 제거할 카드가 부족하면, 기본 카드도 포함시킵니다.
        if (removableCards.Count < count)
            removableCards = GetAllCardsInDeck();
        
        removableCards.Shuffle(); // 리스트를 섞어 무작위성을 보장합니다.

        // 지정된 개수만큼 카드를 제거합니다.
        for (int i = 0; i < count; i++)
        {
            if (removableCards.Count > 0)
            {
                CardData cardToRemove = removableCards[0];
                RemoveCardFromDeck(cardToRemove);
                removableCards.RemoveAt(0);
            }
        }
    }

    public void RemoveAllBasicCards()
    {
        // assetId를 사용하여 기본 카드인지 정확하게 식별합니다.
        var basicCards = GetAllCardsInDeck()
            .Where(c => c.assetID == "card_basic_strike" || c.assetID == "card_basic_defend")
            .ToList();
            
        foreach (var card in basicCards)
        {
            // 덱에 같은 기본 카드가 여러 장 있을 수 있으므로, while 루프를 사용해 모두 제거합니다.
            while(RemoveCardFromDeck(card)) { }
        }
    }

    public void RemoveAllCurseCards()
    {
        // CardData에 isCurse bool 변수가 있다고 가정합니다.
        var curseCards = GetAllCardsInDeck().Where(c => c.isCurse).ToList();
        foreach (var card in curseCards)
        {
            while(RemoveCardFromDeck(card)) { }
        }
    }
    #endregion
    
    #region Card Management
    public List<CardData> GetAllCardsInDeck() => drawPile.Concat(discardPile).Concat(hand).ToList();

    public void AddCardToDeck(CardData newCard)
    {
        discardPile.Add(newCard);
        OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
    }

    private bool ReplaceCardInList(List<CardData> list, CardData oldCard, CardData newCard)
    {
        int index = list.IndexOf(oldCard);
        if (index != -1)
        {
            list[index] = newCard;
            return true;
        }

        return false;
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawPile.Count == 0)
            {
                if (discardPile.Count == 0) return;
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                drawPile.Shuffle();
            }

            CardData newCard = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(newCard);
        }

        OnHandChanged?.Invoke(new List<CardData>(hand));
        OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
    }
    #endregion

    public void GainXp(int amount)
    {
        if (currentRealm == Realm.Saengsagyeong) return;
        currentXp += amount;
        if (currentXp >= xpToNextRealm) isReadyToAscend = true;
    }
    
    #region Technique Realm Methods
    public void RecordEvent(string eventType, CardData card = null, int value = 1)
    {
        switch (eventType)
        {
            case "CardPlayed":
                // if (card.cardType == .Attack) attacksThisCombat++;
                if (card.cost == 0) zeroCostCardsThisCombat++;
                if (card.isJeolcho) RecordEvent("JeolchoCardUsed", card);
                break;
            case "ZeroDamageTurn":
                zeroDamageTurns += value;
                break;
        }

        CheckForSwordRealmAscension();
    }

    public void ResetCombatRecords()
    {
        attacksThisCombat = 0;
        zeroCostCardsThisCombat = 0;
        jeolchoKills = 0;
    }

    private void CheckForSwordRealmAscension()
    {
        switch (currentSwordRealm)
        {
            case SwordRealm.None:
                if (attacksThisCombat >= 10) AscendSwordRealm(SwordRealm.Geomgi);
                break;
            case SwordRealm.Geomgi:
                if (zeroDamageTurns >= 3) AscendSwordRealm(SwordRealm.Geomsa);
                break;
        }
    }

    public void AscendSwordRealm(SwordRealm newRealm)
    {
        if (newRealm <= currentSwordRealm) return;
        currentSwordRealm = newRealm;
    }
    #endregion

    // 불러온 RunData를 기반으로 플레이어의 모든 상태를 복원합니다.
    public void RestoreState(RunData data)
    {
        currentHealth = data.playerCurrentHealth;
        maxHealth = data.playerMaxHealth;
        gold = data.playerGold;
        
        // assetId를 실제 ScriptableObject로 변환
        relics = data.relicIDs.Select(id => ResourceManager.Instance.GetRelicData(id)).ToList();
        drawPile = data.drawPileIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
        discardPile = data.discardPileIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
        hand = data.handIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
        exhaustPile = data.exhaustPileIDs.Select(id => ResourceManager.Instance.GetCardData(id)).ToList();
        
        currentRealm = data.currentRealm;
        currentXp = data.currentXp;
        xpToNextRealm = data.xpToNextRealm;
        currentSwordRealm = data.currentSwordRealm;

        // UI 갱신을 위해 모든 이벤트 호출
        NotifyAllEvents();
    }

    #region Notify
    private void NotifyStatsChanged() => OnStatsChanged?.Invoke(currentHealth, maxHealth, defense, currentNaegong, maxNaegong);
    
    private void NotifyAllEvents()
    {
        NotifyStatsChanged();
        OnHandChanged?.Invoke(new List<CardData>(hand));
        OnPilesChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
        OnGoldChanged?.Invoke(gold);
        OnRelicsChanged?.Invoke(relics);
        OnStatusEffectsChanged?.Invoke(statusEffects.Select(b => b.Effect).ToList());
    }
    #endregion
}
}