# 분파 패시브 통합 가이드

## 개요

분파 시스템의 패시브 능력을 전투 시스템에 통합하기 위한 가이드입니다.

## 패시브 타입별 통합 지점

### 1. SwordDamageBonus (화산파 - 검술 피해 +10%)

**통합 위치**: `CardEffectHandler.cs` 또는 `CombatManager.cs`의 카드 피해 계산 부분

```csharp
// 카드 피해 계산 시
public int CalculateCardDamage(CardData card, int baseDamage)
{
    int damage = baseDamage;

    // 검술 카드인 경우 화산파 패시브 적용
    if (card.weaponType == WeaponType.Sword && FactionManager.Instance != null)
    {
        damage = FactionManager.Instance.ApplySwordDamageBonus(damage);
    }

    return damage;
}
```

**필요한 변경사항**:
- `CardData.cs`에 `WeaponType weaponType` 필드 추가
- `CardDatabase.json`의 모든 카드에 `weaponType` 값 설정

---

### 2. FistBlockGain (소림사 - 권법 카드 사용 시 방어도 2 획득)

**통합 위치**: `CombatManager.cs`의 카드 사용 후 처리 부분

```csharp
// 카드 사용 후
public void OnCardPlayed(CardData card, Player player)
{
    // 권법 카드인 경우 소림사 패시브 적용
    if (card.weaponType == WeaponType.Fist && FactionManager.Instance != null)
    {
        FactionManager.Instance.OnFistCardPlayed(player);
    }
}
```

**필요한 변경사항**:
- 카드 사용 후 콜백에 FactionManager 호출 추가

---

### 3. PalmGoldGain (개방 - 장법 카드 사용 시 골드 1 획득)

**통합 위치**: `CombatManager.cs`의 카드 사용 후 처리 부분

```csharp
// 카드 사용 후
public void OnCardPlayed(CardData card)
{
    // 장법 카드인 경우 개방 패시브 적용
    if (card.weaponType == WeaponType.Palm && FactionManager.Instance != null)
    {
        FactionManager.Instance.OnPalmCardPlayed();

        // GoldManager를 통해 골드 추가
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.GainGold(1);
        }
    }
}
```

**필요한 변경사항**:
- FactionManager.OnPalmCardPlayed()에서 GoldManager.GainGold() 호출 추가

---

### 4. PoisonDoubleEffect (당문 - 중독 효과 2배)

**통합 위치**: `StatusEffect.cs`의 중독 상태 효과 적용 부분

```csharp
// 중독 상태 효과 적용 시
public void ApplyPoison(Enemy enemy, int poisonStacks)
{
    int finalStacks = poisonStacks;

    // 당문 패시브 적용
    if (FactionManager.Instance != null)
    {
        finalStacks = FactionManager.Instance.ApplyPoisonDoubleEffect(poisonStacks);
    }

    enemy.AddStatusEffect(StatusEffectType.Poison, finalStacks);
}
```

**필요한 변경사항**:
- 중독 부여 시 FactionManager 체크 추가
- StatusEffect 시스템에 중독 효과 구현 필요

---

### 5. HealthLossEnergyGain (혈마신교 - 체력 손실 시 내공 1 회복)

**통합 위치**: `Player.cs`의 `TakeDamage()` 메서드

```csharp
// Player.cs
public void TakeDamage(int damage)
{
    // 방어도 우선 감소
    int remainingDamage = damage;
    if (currentBlock > 0)
    {
        int blockUsed = Mathf.Min(currentBlock, damage);
        currentBlock -= blockUsed;
        remainingDamage -= blockUsed;
    }

    // 체력 감소
    if (remainingDamage > 0)
    {
        int oldHealth = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - remainingDamage);
        int actualHealthLost = oldHealth - currentHealth;

        // 혈마신교 패시브 트리거 (실제로 체력을 잃었을 때만)
        if (actualHealthLost > 0 && FactionManager.Instance != null)
        {
            FactionManager.Instance.OnHealthLost(this, actualHealthLost);
        }

        Debug.Log($"[Player] 체력 손실: {actualHealthLost} (현재: {currentHealth}/{maxHealth})");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
```

**필요한 변경사항**:
- Player.cs의 TakeDamage()에 FactionManager 호출 추가
- Player.cs에 `GainEnergy(int amount)` 메서드 추가 필요

---

## 런 시작 시 분파 적용

**통합 위치**: `RunManager.cs` 또는 게임 시작 시점

```csharp
// 새 런 시작 시
public void StartNewRun(string factionId)
{
    // 분파 선택
    FactionManager.Instance.SelectFaction(factionId);

    // 시작 덱 적용
    List<string> startingDeck = FactionManager.Instance.GetStartingDeck();
    if (SaveManager.Instance != null && SaveManager.Instance.CurrentSaveData != null)
    {
        RunData runData = SaveManager.Instance.CurrentSaveData.currentRun;
        runData.deckCardIds = startingDeck;
    }

    // 시작 유물 적용
    string startingRelic = FactionManager.Instance.GetStartingRelic();
    if (!string.IsNullOrEmpty(startingRelic))
    {
        Relic relic = DataManager.Instance.GetRelicById(startingRelic);
        if (relic != null)
        {
            RelicManager.Instance.AddRelic(relic);
        }
    }
}
```

---

## 필요한 추가 작업

### 1. CardData 확장
```csharp
// CardData.cs
[System.Serializable]
public class CardData
{
    // 기존 필드들...
    public WeaponType weaponType;  // 추가 필요
}
```

### 2. RunData 확장
```csharp
// RunData.cs
[System.Serializable]
public class RunData
{
    // 기존 필드들...
    public string factionId;  // 현재 분파 ID 추가
}
```

### 3. Player 확장
```csharp
// Player.cs
public void GainEnergy(int amount)
{
    currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
    Debug.Log($"[Player] 내공 회복: +{amount} (현재: {currentEnergy}/{maxEnergy})");
}
```

---

## 테스트 체크리스트

- [ ] 화산파: 검술 카드 사용 시 피해 10% 증가 확인
- [ ] 소림사: 권법 카드 사용 시 방어도 2 획득 확인
- [ ] 개방: 장법 카드 사용 시 골드 1 획득 확인
- [ ] 당문: 중독 부여 시 2배 효과 확인
- [ ] 혈마신교: 체력 손실 시 내공 1 회복 확인
- [ ] 분파 선택 시 시작 덱 12장 정상 적용 확인
- [ ] 분파 선택 시 시작 유물 정상 적용 확인
- [ ] 세이브/로드 시 분파 정보 유지 확인

---

## 주의사항

1. **WeaponType 필수**: 모든 카드에 `weaponType` 필드가 있어야 패시브가 정상 작동합니다.
2. **통합 순서**: FactionManager는 Player, CombatManager보다 먼저 초기화되어야 합니다.
3. **null 체크**: 항상 `FactionManager.Instance != null` 체크를 해야 합니다.
4. **이벤트 구독**: 패시브 효과 발동 시 UI에 표시하려면 `OnPassiveTriggered` 이벤트를 구독하세요.

---

생성일: 2025-11-25
작성자: Claude Code
