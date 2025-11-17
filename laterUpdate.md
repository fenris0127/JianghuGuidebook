# 향후 개선 사항 (Later Updates)

이 문서는 현재 리팩토링 작업 이후 나중에 추가하면 좋을 개선 사항들을 정리합니다.

## 📋 목차

1. [미완성 기능 구현](#미완성-기능-구현)
2. [아키텍처 개선](#아키텍처-개선)
3. [테스트 확장](#테스트-확장)
4. [성능 최적화](#성능-최적화)
5. [코드 품질](#코드-품질)

---

## 🚧 미완성 기능 구현

### 1. 거리 기반 데미지 증폭 시스템
**위치:** `BattleManager.cs:116`
**우선순위:** Medium

```csharp
// TODO: 거리 기반 데미지 증폭 시스템 (farRangeDamageMultiplier)
```

**구현 사항:**
- BattleManager에 현재 거리 추적 시스템 추가
- CardData의 farRangeDamageMultiplier 활용
- EffectProcessor에서 거리 기반 데미지 계산 로직 구현

**설정 파일 추가:**
```csharp
// GameBalanceConfig.cs에 추가
[Header("=== 거리 시스템 ===")]
[Tooltip("원거리 공격 기본 거리")]
public int defaultRange = 2;

[Tooltip("근거리 범위")]
public int closeRangeThreshold = 1;
```

---

### 2. 적 밀어내기/당기기 메커닉
**위치:** `BattleManager.cs:117`
**우선순위:** Medium

```csharp
// TODO: 적 밀어내기/당기기 메커닉 (pushAmount, pullAmount)
```

**구현 사항:**
- CardData의 pushAmount, pullAmount 활용
- 적 위치 관리 시스템 구현
- 위치 변경 애니메이션/피드백

---

## 🏗️ 아키텍처 개선

### 1. Singleton 패턴 마이그레이션
**우선순위:** Low
**소요 시간:** 2-3시간

현재 수동으로 Singleton 패턴을 구현한 Manager들을 `Singleton<T>` 추상 클래스로 마이그레이션:

**대상 클래스:**
- ✅ ConfigManager (이미 Singleton 사용 준비 완료)
- ⬜ GameManager
- ⬜ RewardManager
- ⬜ ResourceManager
- ⬜ AudioManager
- ⬜ MetaManager
- ⬜ SaveLoadManager
- ⬜ BattleManager
- ⬜ MapManager

**예시:**
```csharp
// Before
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

// After
using GangHoBiGeup.Core;

public class GameManager : Singleton<GameManager>
{
    protected override void OnAwake()
    {
        InitializeUIPanels();
    }
}
```

**장점:**
- 중복 코드 제거 (9개 클래스 × 10줄 = 90줄 감소)
- 일관된 Singleton 구현
- FindObjectOfType 자동 처리

---

### 2. UI Manager 분리
**우선순위:** Medium
**소요 시간:** 3-4시간

GameManager에서 UI 관리 로직을 별도의 UIManager로 분리:

**분리할 로직:**
- UI 패널 관리 (27개 SerializedField)
- ChangeState의 UI 활성화/비활성화
- 카드/유물 보상 화면 생성
- UI 이벤트 처리

**새로운 구조:**
```csharp
public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    private Dictionary<GameState, GameObject> uiPanels;

    [Header("Reward UI")]
    private GameObject cardRewardSlotPrefab;
    private Transform cardRewardContainer;

    public void ShowState(GameState state) { }
    public void ShowCardRewardScreen(List<CardData> rewards) { }
    public void ShowRelicRewardScreen(List<RelicData> rewards) { }
}

// GameManager는 게임 로직에만 집중
public class GameManager : Singleton<GameManager>
{
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        UIManager.Instance.ShowState(newState);
        AudioManager.Instance.PlayMusicForState(newState);
    }
}
```

**장점:**
- GameManager 크기 감소 (247 → ~150 lines)
- UI 로직과 게임 로직 명확히 분리
- UI 관련 수정 시 GameManager 건드리지 않음

---

### 3. 이벤트 시스템 강화
**우선순위:** Low
**소요 시간:** 4-5시간

AudioManager 등의 직접 호출을 이벤트 기반으로 변경:

**현재:**
```csharp
// GameManager.cs
AudioManager.Instance.PlayMusic(AudioManager.Instance.battleTheme);

// Player.cs
AudioManager.Instance.PlaySound(AudioManager.Instance.takeDamageSound);
```

**개선안:**
```csharp
// EventBus.cs (새로 생성)
public static class GameEvents
{
    public static event Action<GameState> OnStateChanged;
    public static event Action<int> OnPlayerDamaged;
    public static event Action<CardData> OnCardPlayed;
}

// GameManager.cs
public void ChangeState(GameState newState)
{
    CurrentState = newState;
    GameEvents.OnStateChanged?.Invoke(newState);
}

// AudioManager.cs
private void OnEnable()
{
    GameEvents.OnStateChanged += HandleStateChanged;
    GameEvents.OnPlayerDamaged += PlayDamageSound;
}

private void HandleStateChanged(GameState state)
{
    switch (state)
    {
        case GameState.Battle: PlayMusic(battleTheme); break;
        // ...
    }
}
```

**장점:**
- 결합도 감소
- AudioManager 의존성 제거
- 새로운 시스템 추가 시 기존 코드 수정 불필요

---

### 4. Config 파일 추가 생성
**우선순위:** Low

더 많은 매직 넘버들을 Config로 이동:

**AudioConfig.cs**
```csharp
[CreateAssetMenu(fileName = "AudioConfig", menuName = "GangHoBiGeup/Config/Audio Config")]
public class AudioConfig : ScriptableObject
{
    [Header("=== 볼륨 설정 ===")]
    [Range(0f, 1f)] public float defaultMusicVolume = 0.7f;
    [Range(0f, 1f)] public float defaultSfxVolume = 1.0f;

    [Header("=== 페이드 설정 ===")]
    public float musicFadeDuration = 1.0f;
    public float sfxFadeDuration = 0.3f;
}
```

**UIConfig.cs**
```csharp
[CreateAssetMenu(fileName = "UIConfig", menuName = "GangHoBiGeup/Config/UI Config")]
public class UIConfig : ScriptableObject
{
    [Header("=== 애니메이션 시간 ===")]
    public float cardDrawAnimationDuration = 0.3f;
    public float damageNumberDuration = 1.0f;
    public float panelFadeDuration = 0.5f;

    [Header("=== UI 색상 ===")]
    public Color commonCardColor = Color.white;
    public Color rareCardColor = Color.blue;
    public Color epicCardColor = Color.magenta;
    public Color legendaryCardColor = Color.yellow;
}
```

---

## 🧪 테스트 확장

### 1. 새 컴포넌트 단위 테스트
**우선순위:** High
**소요 시간:** 6-8시간

현재 컴포넌트들의 테스트 커버리지 확보:

**테스트 대상:**
- ✅ HealthComponent (일부 Player 테스트에서 간접 검증)
- ⬜ DeckComponent
- ⬜ RealmComponent
- ⬜ ComboComponent
- ⬜ InventoryComponent
- ⬜ StatusEffectContainer

**예시:**
```csharp
// HealthComponentTests.cs
[TestFixture]
public class HealthComponentTests
{
    private GameObject testObject;
    private HealthComponent health;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject();
        health = testObject.AddComponent<HealthComponent>();
    }

    [Test]
    public void Initialize_SetsCorrectValues()
    {
        health.Initialize(100);

        Assert.AreEqual(100, health.MaxHealth);
        Assert.AreEqual(100, health.CurrentHealth);
        Assert.AreEqual(0, health.Defense);
    }

    [Test]
    public void TakeDamage_WithDefense_BlocksDamageCorrectly()
    {
        health.Initialize(100);
        health.GainDefense(20);

        int actualDamage = health.TakeDamage(30);

        Assert.AreEqual(10, actualDamage);
        Assert.AreEqual(90, health.CurrentHealth);
        Assert.AreEqual(0, health.Defense);
    }

    // ... 더 많은 테스트
}
```

---

### 2. Config 통합 테스트
**우선순위:** Medium
**소요 시간:** 2-3시간

Config 파일들이 올바르게 로드되고 값을 반환하는지 테스트:

```csharp
[TestFixture]
public class ConfigIntegrationTests
{
    [Test]
    public void ConfigManager_LoadsAllConfigs()
    {
        var configManager = new GameObject().AddComponent<ConfigManager>();

        Assert.IsNotNull(configManager.GameBalance);
        Assert.IsNotNull(configManager.Realm);
        Assert.IsNotNull(configManager.Map);
        Assert.IsNotNull(configManager.Battle);
    }

    [Test]
    public void GameBalanceConfig_HasCorrectDefaultValues()
    {
        var config = Resources.Load<GameBalanceConfig>("Config/GameBalanceConfig");

        Assert.AreEqual(80, config.baseMaxHealth);
        Assert.AreEqual(1.5f, config.vulnerableDamageMultiplier);
        Assert.AreEqual(0.75f, config.weakDamageMultiplier);
    }
}
```

---

### 3. 엔드투엔드 테스트
**우선순위:** Low
**소요 시간:** 8-10시간

전체 게임 플레이 시나리오 테스트:

```csharp
[TestFixture]
public class GameplayE2ETests
{
    [Test]
    public void CompleteRun_FromStartToVictory()
    {
        // 1. 게임 시작
        // 2. 전투 진행
        // 3. 카드 선택
        // 4. 다음 층 진행
        // 5. 최종 보스 격파
        // 6. 승리 화면 확인
    }
}
```

---

## ⚡ 성능 최적화

### 1. Object Pooling 시스템
**우선순위:** Medium
**소요 시간:** 4-5시간

자주 생성/파괴되는 오브젝트를 풀링:

**대상:**
- Enemy 프리팹
- 카드 UI 슬롯
- 데미지 넘버 UI
- 파티클 이펙트

**구현:**
```csharp
public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    public T Get()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        return Object.Instantiate(prefab, parent);
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}

// PoolManager.cs
public class PoolManager : Singleton<PoolManager>
{
    private ObjectPool<Enemy> enemyPool;
    private ObjectPool<CardUI> cardUIPool;
    private ObjectPool<DamageNumber> damageNumberPool;
}
```

**예상 효과:**
- GC 압력 감소
- 프레임 드롭 방지
- 메모리 사용량 안정화

---

### 2. 추가 캐싱 기회
**우선순위:** Low

현재 GameManager의 FindObjectOfType 캐싱을 다른 곳에도 적용:

**대상:**
- BattleManager에서 Player 참조
- Enemy에서 BattleManager 참조
- UI 컴포넌트들의 Manager 참조

---

### 3. 코루틴 최적화
**우선순위:** Low

불필요한 코루틴 호출 최소화:

**현재:**
```csharp
yield return new WaitForSeconds(0.5f);  // 매번 새로운 객체 생성
```

**개선:**
```csharp
// BattleManager.cs
private WaitForSeconds halfSecondWait;
private WaitForSeconds oneSecondWait;

void Awake()
{
    halfSecondWait = new WaitForSeconds(0.5f);
    oneSecondWait = new WaitForSeconds(1f);
}

IEnumerator EnemyTurn()
{
    yield return halfSecondWait;  // 재사용
}
```

---

## 🎨 코드 품질

### 1. XML 문서 주석 확장
**우선순위:** Low

모든 public API에 XML 문서 주석 추가:

```csharp
/// <summary>
/// 플레이어에게 피해를 입힙니다.
/// </summary>
/// <param name="damage">기본 피해량</param>
/// <remarks>
/// 취약 상태일 경우 ConfigManager의 vulnerableDamageMultiplier가 적용됩니다.
/// 방어도가 있을 경우 먼저 방어도가 감소하고, 남은 피해가 체력에 적용됩니다.
/// </remarks>
public void TakeDamage(int damage)
{
    // ...
}
```

---

### 2. 네이밍 컨벤션 통일
**우선순위:** Low

현재 혼재된 네이밍 스타일 통일:

**이벤트 네이밍:**
```csharp
// 현재 혼재
OnStatsChanged
onHandChanged  // lowercase 시작

// 통일
OnStatsChanged
OnHandChanged
```

**Private 필드:**
```csharp
// 현재 혼재
private int health;
private int _health;

// 통일 (C# 컨벤션)
private int _health;
private Player _cachedPlayer;
```

---

### 3. 상수 추출
**우선순위:** Low

반복되는 문자열/숫자를 상수로 추출:

```csharp
// Constants.cs
public static class ResourcePaths
{
    public const string CONFIG_GAME_BALANCE = "Config/GameBalanceConfig";
    public const string CONFIG_REALM = "Config/RealmConfig";
    public const string CONFIG_MAP = "Config/MapConfig";
    public const string CONFIG_BATTLE = "Config/BattleConfig";
}

public static class SceneNames
{
    public const string MAIN_MENU = "MainMenu";
    public const string GAME_SCENE = "GameScene";
}
```

---

## 📊 우선순위 요약

### High Priority (즉시 또는 단기)
1. ✅ 새 컴포넌트 단위 테스트 작성
2. ⬜ UI Manager 분리

### Medium Priority (중기)
1. ⬜ 거리 기반 데미지 시스템 구현
2. ⬜ 밀어내기/당기기 메커닉 구현
3. ⬜ Object Pooling 시스템
4. ⬜ Config 통합 테스트

### Low Priority (장기/선택)
1. ⬜ Singleton 패턴 마이그레이션
2. ⬜ 이벤트 시스템 강화
3. ⬜ 추가 Config 파일 생성
4. ⬜ 코드 품질 개선
5. ⬜ 엔드투엔드 테스트

---

## 📝 참고사항

이 문서는 리팩토링 작업 후 남은 개선 기회들을 정리한 것입니다. 각 항목의 우선순위는 프로젝트 상황에 따라 조정될 수 있습니다.

**현재 완료된 주요 리팩토링:**
- ✅ Player God Class 분해 (824 → 461 lines)
- ✅ 컴포넌트 기반 아키텍처 구축
- ✅ Config 파일 시스템 (4개 파일, 55+ 매직 넘버 제거)
- ✅ GameManager UI 관리 개선
- ✅ 성능 최적화 (FindObjectOfType 캐싱)
- ✅ Singleton 추상 클래스 생성
- ✅ StatusEffectContainer 공통화

**문서 작성일:** 2025-11-17
**마지막 업데이트:** 2025-11-17
