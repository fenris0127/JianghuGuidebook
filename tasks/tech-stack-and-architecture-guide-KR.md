# 강호무적 - 기술 스택 및 아키텍처 설계 가이드

## 목차
1. [기술 스택 선택 가이드](#1-기술-스택-선택-가이드)
2. [아키텍처 설계](#2-아키텍처-설계)
3. [개발 환경 설정](#3-개발-환경-설정)
4. [프로젝트 구조](#4-프로젝트-구조)
5. [핵심 디자인 패턴](#5-핵심-디자인-패턴)
6. [데이터 관리 전략](#6-데이터-관리-전략)
7. [성능 최적화 전략](#7-성능-최적화-전략)
8. [주니어 개발자를 위한 팁](#8-주니어-개발자를-위한-팁)

---

## 1. 기술 스택 선택 가이드

### 1.1 게임 엔진 비교: Unity vs Godot

#### **Unity (권장 ⭐)**

**장점:**
- ✅ **풍부한 학습 자료**: YouTube, Udemy, 공식 문서가 방대함
- ✅ **에셋 스토어**: UI 템플릿, 카드 시스템 에셋 등 구매 가능
- ✅ **C# 언어**: 타입 안정성, 강력한 IDE 지원 (Visual Studio, Rider)
- ✅ **커뮤니티 크기**: 문제 발생 시 Stack Overflow, Unity Forum에서 빠른 답변
- ✅ **플랫폼 지원**: Windows, Mac, Linux, WebGL, 모바일까지 쉽게 빌드
- ✅ **2D 툴킷**: Sprite Atlas, Tilemap, Animator 등 2D 게임 제작 도구 완비
- ✅ **취업 시장**: Unity 경험은 게임 업계에서 높은 가치

**단점:**
- ❌ **라이선스 비용**: 수익이 일정 금액 이상이면 유료 (2024년 기준 $200k/년 이상)
- ❌ **엔진 크기**: 다운로드 및 빌드 시간이 다소 길음
- ❌ **런타임 비용**: Unity Runtime Fee 정책 (2024년 논란 있었지만 완화됨)

**추천 대상:**
- 게임 개발 처음 시작하는 주니어 개발자
- C# 언어에 익숙하거나 배우고 싶은 개발자
- 빠르게 프로토타입을 만들고 싶은 경우
- 향후 모바일 출시도 고려하는 경우

---

#### **Godot**

**장점:**
- ✅ **완전 무료 오픈소스**: MIT 라이선스, 수익 제한 없음
- ✅ **가벼움**: 엔진 용량 50MB 미만, 빠른 시작
- ✅ **GDScript**: Python과 유사한 간결한 문법
- ✅ **노드 기반 씬 시스템**: 직관적인 게임 오브젝트 구성
- ✅ **빠른 반복**: 핫 리로드 지원으로 빠른 테스트

**단점:**
- ❌ **학습 자료 부족**: Unity에 비해 튜토리얼, 강의가 적음
- ❌ **에셋 스토어 부족**: 상용 에셋이 거의 없음
- ❌ **커뮤니티 규모**: 작은 커뮤니티로 문제 해결이 느릴 수 있음
- ❌ **C# 지원 제한적**: GDScript가 주력이며, C#은 2순위
- ❌ **취업 시장**: Godot 경험은 Unity보다 수요 적음

**추천 대상:**
- 비용이 전혀 없어야 하는 경우
- Python에 익숙한 개발자
- 엔진 소스코드를 직접 수정하고 싶은 고급 개발자
- 가벼운 2D 게임 제작

---

### 1.2 최종 권장: **Unity (C#)**

**이유:**
1. **주니어 개발자 친화적**: 학습 자료가 압도적으로 많음
2. **덱 빌딩 게임 레퍼런스 존재**: Slay the Spire 모드 툴, 유사 프로젝트 많음
3. **UI 시스템 강력**: Unity UI (UGUI) 또는 UI Toolkit
4. **JSON 파싱 쉬움**: Newtonsoft.Json 또는 Unity JsonUtility
5. **미래 확장성**: 모바일, 콘솔 이식 가능

**설치 버전:**
- **Unity 2022.3 LTS** (Long Term Support) 권장
- 2D 템플릿으로 프로젝트 생성

---

### 1.3 프로그래밍 언어: C#

**주요 라이브러리:**
- **Newtonsoft.Json** (JSON 파싱)
- **DOTween** (애니메이션 - 옵션)
- **Unity Test Framework** (유닛 테스트)

---

### 1.4 버전 관리: Git + GitHub

**권장 설정:**
- `.gitignore`: Unity 전용 gitignore 사용
- **LFS (Large File Storage)**: 이미지, 사운드 파일용
- **브랜치 전략**: Git Flow (feature/phase1, feature/phase2 등)

---

### 1.5 데이터 저장 형식: JSON

**이유:**
- 사람이 읽기 쉬움 (디버깅 용이)
- Unity에서 기본 지원 (`JsonUtility`)
- 외부 툴(Google Sheets → JSON 변환)로 밸런싱 쉬움

**대안:**
- ScriptableObject (Unity 전용, 빠르지만 외부 편집 어려움)
- SQLite (오버엔지니어링, 필요 없음)

---

### 1.6 IDE: Visual Studio 2022 또는 JetBrains Rider

**Visual Studio 2022 Community (무료):**
- Unity와 완벽한 통합
- 강력한 디버거
- IntelliSense (자동 완성)

**JetBrains Rider (유료, 학생 무료):**
- VS보다 빠르고 가벼움
- 더 좋은 리팩토링 도구
- Unity 전용 기능 많음

**권장:** 처음이라면 Visual Studio, 익숙하다면 Rider

---

## 2. 아키텍처 설계

### 2.1 전체 아키텍처 다이어그램

```
┌─────────────────────────────────────────────────────────────┐
│                       Game Manager (Singleton)              │
│  - 씬 전환 관리                                               │
│  - 전역 상태 관리                                             │
└─────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        │                     │                     │
┌───────▼────────┐  ┌─────────▼──────────┐  ┌──────▼─────────┐
│  Data Manager  │  │  Combat Manager    │  │  Map Manager   │
│  - JSON 로드   │  │  - 전투 흐름       │  │  - 맵 생성     │
│  - 카드 DB     │  │  - 턴 관리         │  │  - 노드 진행   │
│  - 적 DB       │  │  - 승패 판정       │  │  - 현재 위치   │
└────────────────┘  └────────────────────┘  └────────────────┘
        │                     │                     │
┌───────▼────────┐  ┌─────────▼──────────┐  ┌──────▼─────────┐
│  Deck Manager  │  │  Player / Enemy    │  │   Map UI       │
│  - 드로우      │  │  - 체력, 내공      │  │  - 노드 표시   │
│  - 셔플        │  │  - 상태 효과       │  │  - 이동 처리   │
│  - 사용/버림   │  └────────────────────┘  └────────────────┘
└────────────────┘            │
        │            ┌─────────▼──────────┐
        │            │   Relic Manager    │
        │            │  - 유물 효과 적용  │
        │            └────────────────────┘
        │
┌───────▼────────────────────────────────────────────────┐
│                      UI Layer                          │
│  - Combat UI, Map UI, Shop UI, Event UI, Menu UI      │
└────────────────────────────────────────────────────────┘
```

---

### 2.2 디자인 패턴

#### **2.2.1 Singleton 패턴**

**사용처:** GameManager, DataManager, AudioManager

**구현 예시:**
```csharp
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

**주의사항:**
- 씬 전환 시에도 유지되어야 하는 매니저만 Singleton 사용
- 과도한 Singleton은 테스트를 어렵게 함 → 최소화

---

#### **2.2.2 State 패턴 (전투 상태)**

**구현 예시:**
```csharp
public enum CombatState
{
    Initializing,
    PlayerTurnStart,
    PlayerTurn,
    PlayerTurnEnd,
    EnemyTurn,
    Victory,
    Defeat
}

public class CombatManager : MonoBehaviour
{
    private CombatState currentState;

    void ChangeState(CombatState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(currentState);
    }

    void EnterState(CombatState state)
    {
        switch (state)
        {
            case CombatState.PlayerTurnStart:
                OnPlayerTurnStart();
                break;
            case CombatState.EnemyTurn:
                OnEnemyTurn();
                break;
            // ...
        }
    }

    void ExitState(CombatState state)
    {
        // 상태 종료 시 정리 작업
    }
}
```

---

#### **2.2.3 Observer 패턴 (이벤트 시스템)**

**사용처:** 카드 사용 시 유물 효과 트리거, UI 업데이트

**구현 예시:**
```csharp
// C# 이벤트 사용
public class Player : MonoBehaviour
{
    public event Action<int> OnHealthChanged;
    public event Action<int> OnEnergyChanged;

    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthChanged?.Invoke(health);
        }
    }
}

// UI에서 구독
public class PlayerHealthUI : MonoBehaviour
{
    void Start()
    {
        Player.Instance.OnHealthChanged += UpdateHealthDisplay;
    }

    void UpdateHealthDisplay(int newHealth)
    {
        healthText.text = newHealth.ToString();
    }
}
```

**또는 Unity Events 사용:**
```csharp
using UnityEngine.Events;

[System.Serializable]
public class CardPlayedEvent : UnityEvent<Card> { }

public class CombatManager : MonoBehaviour
{
    public CardPlayedEvent onCardPlayed;

    void PlayCard(Card card)
    {
        // 카드 효과 적용
        onCardPlayed?.Invoke(card);
    }
}
```

---

#### **2.2.4 Factory 패턴 (카드/유물 생성)**

**구현 예시:**
```csharp
public class CardFactory
{
    public static Card CreateCard(CardData data)
    {
        Card card = new Card
        {
            id = data.id,
            name = data.name,
            cost = data.cost,
            type = data.type,
            baseDamage = data.baseDamage,
            // ...
        };
        return card;
    }

    public static Card CreateUpgradedCard(Card original)
    {
        Card upgraded = CreateCard(original.data);
        upgraded.isUpgraded = true;
        upgraded.baseDamage += 3; // 업그레이드 효과
        return upgraded;
    }
}
```

---

#### **2.2.5 Command 패턴 (카드 효과)**

**이유:** 카드 효과를 되돌리거나 재생하기 쉬움 (미래 애니메이션 리플레이)

**구현 예시:**
```csharp
public interface ICardEffect
{
    void Execute(Player player, Enemy target);
    void Undo(); // 선택적
}

public class DamageEffect : ICardEffect
{
    private int damage;

    public DamageEffect(int damage)
    {
        this.damage = damage;
    }

    public void Execute(Player player, Enemy target)
    {
        target.TakeDamage(damage);
    }

    public void Undo() { /* 필요시 구현 */ }
}

public class Card
{
    public List<ICardEffect> effects;

    public void Play(Player player, Enemy target)
    {
        foreach (var effect in effects)
        {
            effect.Execute(player, target);
        }
    }
}
```

---

### 2.3 레이어 분리 (Clean Architecture 간소화)

```
┌──────────────────────────────────────┐
│         Presentation Layer           │ ← UI, 애니메이션
│  (UI Scripts, Visual Feedback)       │
└──────────────────────────────────────┘
                 │
┌──────────────────────────────────────┐
│          Logic Layer                 │ ← 게임 로직
│  (Managers, Game Rules)              │
└──────────────────────────────────────┘
                 │
┌──────────────────────────────────────┐
│          Data Layer                  │ ← 데이터 저장/로드
│  (DataManager, SaveManager, JSON)    │
└──────────────────────────────────────┘
```

**원칙:**
- **UI는 로직을 직접 수정하지 않음** → Manager를 통해서만 상호작용
- **로직은 UI를 몰라야 함** → 이벤트로 UI에 알림
- **데이터는 읽기 전용** → 한 번 로드하면 런타임에서 수정 안 함

---

## 3. 개발 환경 설정

### 3.1 Unity 프로젝트 초기 설정

**1단계: Unity Hub 설치**
- [Unity Hub 다운로드](https://unity.com/download)
- Unity 2022.3 LTS 설치
- 모듈: Windows Build Support, Mac Build Support (선택)

**2단계: 프로젝트 생성**
- Template: **2D (URP)** 선택 (Universal Render Pipeline)
- 프로젝트명: `MurimDeckbuilder`

**3단계: 패키지 설치**
- Window → Package Manager
- **Newtonsoft.Json** 설치 (JSON 파싱)
- **Input System** (옵션, 키보드/마우스 입력)
- **TextMeshPro** (더 나은 텍스트 렌더링)

**4단계: 프로젝트 설정**
- Edit → Project Settings → Player
  - Company Name: 본인 이름
  - Product Name: 강호무적
  - Default Icon 설정 (나중에)
- Edit → Project Settings → Quality
  - VSync: Every V Blank (60 FPS 고정)

---

### 3.2 Git 설정

**1단계: .gitignore 생성**
- GitHub의 Unity .gitignore 템플릿 사용
- [Unity .gitignore 링크](https://github.com/github/gitignore/blob/main/Unity.gitignore)

**2단계: Git 초기화**
```bash
cd MurimDeckbuilder
git init
git add .
git commit -m "Initial Unity project setup"
```

**3단계: GitHub 레포 생성 및 푸시**
```bash
git remote add origin https://github.com/yourusername/murim-deckbuilder.git
git branch -M main
git push -u origin main
```

**4단계: 브랜치 전략**
```bash
# Phase 1 시작
git checkout -b feature/phase1-prototype

# Phase 1 완료 후
git checkout main
git merge feature/phase1-prototype
git tag v0.1.0-phase1

# Phase 2 시작
git checkout -b feature/phase2-vertical-slice
```

---

### 3.3 폴더 구조 설정

```
MurimDeckbuilder/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/              # GameManager, CombatManager 등
│   │   ├── Data/              # CardData, EnemyData, DataManager
│   │   ├── Cards/             # Card, DeckManager, CardEffects
│   │   ├── Combat/            # Player, Enemy, StatusEffect
│   │   ├── AI/                # EnemyAI, IntentSystem
│   │   ├── UI/                # 모든 UI 스크립트
│   │   ├── Map/               # MapGenerator, MapManager (Phase 2)
│   │   ├── Relics/            # Relic, RelicManager (Phase 2)
│   │   ├── Shop/              # ShopManager (Phase 2)
│   │   ├── Events/            # EventManager (Phase 2)
│   │   ├── Meta/              # MetaProgressionManager (Phase 2)
│   │   ├── Save/              # SaveManager, SaveData
│   │   └── Utils/             # 유틸리티 함수
│   ├── Data/
│   │   ├── Cards/
│   │   │   └── CardDatabase.json
│   │   ├── Enemies/
│   │   │   └── EnemyDatabase.json
│   │   ├── Relics/            # (Phase 2)
│   │   ├── Events/            # (Phase 2)
│   │   └── Meta/              # (Phase 2)
│   ├── Prefabs/
│   │   ├── Cards/
│   │   │   └── CardPrefab.prefab
│   │   ├── Enemies/
│   │   └── UI/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── CombatScene.unity
│   │   ├── MapScene.unity     # (Phase 2)
│   │   └── ShopScene.unity    # (Phase 2)
│   ├── Sprites/               # 이미지 에셋
│   ├── Audio/                 # 음악, 효과음
│   └── Tests/
│       ├── PlayMode/
│       └── EditMode/
└── ProjectSettings/
```

---

## 4. 프로젝트 구조

### 4.1 핵심 스크립트 구조

#### **GameManager.cs**
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 전역 상태
    public Player player;
    public RunData currentRun;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewRun()
    {
        // 새 런 초기화
        currentRun = new RunData();
        player = new Player();
        // 맵 씬 로드
        SceneManager.LoadScene("MapScene");
    }

    public void EndRun(bool victory)
    {
        // 무공 정수 계산 및 저장
        int essence = CalculateMugongEssence();
        MetaProgressionManager.Instance.AddEssence(essence);
        // 메인 메뉴로 복귀
        SceneManager.LoadScene("MainMenu");
    }
}
```

---

#### **CombatManager.cs**
```csharp
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    public CombatState currentState;
    public Player player;
    public List<Enemy> enemies;
    public DeckManager deckManager;

    void Start()
    {
        Instance = this;
        InitializeCombat();
    }

    void InitializeCombat()
    {
        player = GameManager.Instance.player;
        // 적 생성
        enemies = SpawnEnemies();
        // 덱 초기화
        deckManager = new DeckManager(player.deck);
        // 전투 시작
        ChangeState(CombatState.PlayerTurnStart);
    }

    void ChangeState(CombatState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case CombatState.PlayerTurnStart:
                OnPlayerTurnStart();
                break;
            case CombatState.EnemyTurn:
                OnEnemyTurn();
                break;
            case CombatState.Victory:
                OnVictory();
                break;
            case CombatState.Defeat:
                OnDefeat();
                break;
        }
    }

    void OnPlayerTurnStart()
    {
        player.currentEnergy = player.maxEnergy;
        player.block = 0; // 방어도 초기화
        deckManager.DrawCards(5);
        currentState = CombatState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        deckManager.DiscardHand();
        ChangeState(CombatState.EnemyTurn);
    }

    void OnEnemyTurn()
    {
        foreach (var enemy in enemies)
        {
            enemy.ExecuteIntent(player);
        }
        // 적 턴 종료 후 플레이어 턴 시작
        StartCoroutine(DelayedPlayerTurnStart());
    }

    IEnumerator DelayedPlayerTurnStart()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(CombatState.PlayerTurnStart);
    }

    void CheckVictoryCondition()
    {
        if (enemies.All(e => e.currentHealth <= 0))
        {
            ChangeState(CombatState.Victory);
        }
    }

    void CheckDefeatCondition()
    {
        if (player.currentHealth <= 0)
        {
            ChangeState(CombatState.Defeat);
        }
    }
}
```

---

#### **DeckManager.cs**
```csharp
public class DeckManager
{
    public List<Card> drawPile;
    public List<Card> hand;
    public List<Card> discardPile;
    public List<Card> exhaustPile;

    public DeckManager(List<CardData> startingDeck)
    {
        drawPile = new List<Card>();
        hand = new List<Card>();
        discardPile = new List<Card>();
        exhaustPile = new List<Card>();

        // 시작 덱 생성
        foreach (var cardData in startingDeck)
        {
            drawPile.Add(CardFactory.CreateCard(cardData));
        }
        Shuffle(drawPile);
    }

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (hand.Count >= 10) break; // 손 제한
            DrawCard();
        }
    }

    public Card DrawCard()
    {
        if (drawPile.Count == 0)
        {
            // 버리기 더미를 섞어서 뽑기 더미로
            if (discardPile.Count == 0) return null;
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(drawPile);
        }

        Card card = drawPile[0];
        drawPile.RemoveAt(0);
        hand.Add(card);
        return card;
    }

    public void PlayCard(Card card, Enemy target)
    {
        if (!hand.Contains(card)) return;
        if (CombatManager.Instance.player.currentEnergy < card.cost) return;

        // 내공 소모
        CombatManager.Instance.player.currentEnergy -= card.cost;

        // 카드 효과 실행
        card.Play(CombatManager.Instance.player, target);

        // 손에서 제거
        hand.Remove(card);

        // 소진 카드가 아니면 버리기 더미로
        if (card.isExhaust)
        {
            exhaustPile.Add(card);
        }
        else
        {
            discardPile.Add(card);
        }
    }

    public void DiscardHand()
    {
        discardPile.AddRange(hand);
        hand.Clear();
    }

    void Shuffle(List<Card> deck)
    {
        // Fisher-Yates 셔플
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }
}
```

---

### 4.2 데이터 구조

#### **CardData.cs**
```csharp
[System.Serializable]
public class CardData
{
    public string id;
    public string name;
    public int cost;
    public CardType type;        // Attack, Defense, Skill, Secret
    public CardCategory category; // Sword, Saber, Spear, Palm, Fist, Misc
    public Rarity rarity;        // Common, Uncommon, Rare, Legendary
    public int baseDamage;
    public int baseBlock;
    public string description;
    public bool isExhaust;

    // 업그레이드 수치
    public int upgradedDamage;
    public int upgradedBlock;
    public int upgradedCost;
}

public enum CardType { Attack, Defense, Skill, Secret }
public enum CardCategory { Sword, Saber, Spear, Palm, Fist, Misc }
public enum Rarity { Common, Uncommon, Rare, Legendary }
```

#### **Card.cs (런타임 카드 인스턴스)**
```csharp
public class Card
{
    public CardData data;
    public bool isUpgraded;

    // 런타임 계산 속성
    public int cost => isUpgraded ? data.upgradedCost : data.cost;
    public int damage => isUpgraded ? data.upgradedDamage : data.baseDamage;
    public int block => isUpgraded ? data.upgradedBlock : data.baseBlock;

    public void Play(Player player, Enemy target)
    {
        switch (data.type)
        {
            case CardType.Attack:
                target.TakeDamage(damage);
                break;
            case CardType.Defense:
                player.GainBlock(block);
                break;
            // ... 기타 효과
        }
    }
}
```

---

## 5. 핵심 디자인 패턴

### 5.1 이벤트 기반 아키텍처

**왜 필요한가?**
- 유물 효과가 카드 사용, 전투 시작, 턴 시작 등 다양한 타이밍에 발동
- UI가 게임 상태 변화에 즉시 반응해야 함
- 시스템 간 결합도를 낮춰 유지보수 용이

**구현 예시:**
```csharp
// 이벤트 정의
public static class GameEvents
{
    public static event Action<Card> OnCardPlayed;
    public static event Action OnCombatStart;
    public static event Action OnTurnStart;
    public static event Action OnTurnEnd;
    public static event Action<Enemy> OnEnemyDeath;

    public static void CardPlayed(Card card) => OnCardPlayed?.Invoke(card);
    public static void CombatStart() => OnCombatStart?.Invoke();
    public static void TurnStart() => OnTurnStart?.Invoke();
    public static void TurnEnd() => OnTurnEnd?.Invoke();
    public static void EnemyDeath(Enemy enemy) => OnEnemyDeath?.Invoke(enemy);
}

// 유물이 이벤트 구독
public class RelicManager : MonoBehaviour
{
    void OnEnable()
    {
        GameEvents.OnCardPlayed += HandleCardPlayed;
        GameEvents.OnCombatStart += HandleCombatStart;
    }

    void OnDisable()
    {
        GameEvents.OnCardPlayed -= HandleCardPlayed;
        GameEvents.OnCombatStart -= HandleCombatStart;
    }

    void HandleCardPlayed(Card card)
    {
        // 유물 효과 체크 및 발동
        foreach (var relic in player.relics)
        {
            relic.OnCardPlayed(card);
        }
    }
}
```

---

### 5.2 ScriptableObject 활용 (선택적)

**카드를 ScriptableObject로 관리하는 방법:**

**장점:**
- Unity 에디터에서 직접 편집 가능
- Inspector에서 시각적으로 확인

**단점:**
- 외부 툴(Excel, Google Sheets)과 연동 어려움
- JSON보다 버전 관리 어려움

**추천:**
- **Phase 1**: JSON으로 시작 (빠른 반복)
- **Phase 3**: 안정화 후 ScriptableObject 전환 고려

---

## 6. 데이터 관리 전략

### 6.1 JSON 데이터 구조

#### **CardDatabase.json**
```json
{
  "cards": [
    {
      "id": "card_strike",
      "name": "일검",
      "cost": 1,
      "type": "Attack",
      "category": "Sword",
      "rarity": "Common",
      "baseDamage": 6,
      "upgradedDamage": 9,
      "upgradedCost": 1,
      "baseBlock": 0,
      "upgradedBlock": 0,
      "description": "적에게 {damage} 피해를 준다.",
      "isExhaust": false
    },
    {
      "id": "card_defend",
      "name": "철포삼",
      "cost": 1,
      "type": "Defense",
      "category": "Misc",
      "rarity": "Common",
      "baseDamage": 0,
      "baseBlock": 5,
      "upgradedBlock": 8,
      "description": "방어도 {block}을 얻는다.",
      "isExhaust": false
    }
  ]
}
```

---

### 6.2 DataManager.cs

```csharp
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public Dictionary<string, CardData> cardDatabase;
    public Dictionary<string, EnemyData> enemyDatabase;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllData();
    }

    void LoadAllData()
    {
        LoadCardDatabase();
        LoadEnemyDatabase();
    }

    void LoadCardDatabase()
    {
        TextAsset json = Resources.Load<TextAsset>("Data/Cards/CardDatabase");
        if (json == null)
        {
            Debug.LogError("CardDatabase.json not found!");
            return;
        }

        CardDatabaseWrapper wrapper = JsonConvert.DeserializeObject<CardDatabaseWrapper>(json.text);
        cardDatabase = new Dictionary<string, CardData>();
        foreach (var card in wrapper.cards)
        {
            cardDatabase[card.id] = card;
        }

        Debug.Log($"Loaded {cardDatabase.Count} cards");
    }

    public CardData GetCard(string id)
    {
        if (cardDatabase.TryGetValue(id, out CardData data))
        {
            return data;
        }
        Debug.LogError($"Card {id} not found!");
        return null;
    }

    [System.Serializable]
    class CardDatabaseWrapper
    {
        public List<CardData> cards;
    }
}
```

---

### 6.3 밸런싱을 위한 Google Sheets 연동 (선택적)

**워크플로우:**
1. Google Sheets에서 카드 데이터 편집
2. Sheets → JSON 변환 스크립트 실행
3. JSON 파일을 Unity에 복사
4. 게임 실행하여 테스트

**도구:**
- [Sheets to JSON Chrome Extension](https://workspace.google.com/marketplace)
- 또는 Python 스크립트로 자동화

---

## 7. 성능 최적화 전략

### 7.1 오브젝트 풀링 (카드 UI)

**문제:**
- 카드를 매번 Instantiate/Destroy하면 GC (Garbage Collection) 발생
- 프레임 드롭 원인

**해결:**
```csharp
public class CardUIPool : MonoBehaviour
{
    public GameObject cardPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetCard()
    {
        if (pool.Count > 0)
        {
            GameObject card = pool.Dequeue();
            card.SetActive(true);
            return card;
        }
        else
        {
            return Instantiate(cardPrefab);
        }
    }

    public void ReturnCard(GameObject card)
    {
        card.SetActive(false);
        pool.Enqueue(card);
    }
}
```

---

### 7.2 UI 최적화

**권장 사항:**
- **Canvas 분리**: 자주 업데이트되는 UI (카드 패)와 정적 UI (배경) 분리
- **Raycast Target 최소화**: 클릭 불필요한 UI는 Raycast Target 끄기
- **Sprite Atlas 사용**: 카드 이미지를 하나의 Atlas로 묶기 (드로우 콜 감소)

---

### 7.3 JSON 파싱 최적화

**문제:**
- 매 씬 로드마다 JSON 파싱은 비효율적

**해결:**
- DataManager를 DontDestroyOnLoad로 설정
- 게임 시작 시 한 번만 로드

---

## 8. 주니어 개발자를 위한 팁

### 8.1 개발 순서

**절대 금지:**
- ❌ 완벽한 코드 작성하려다가 진도 안 나가기
- ❌ 모든 기능 동시에 만들기
- ❌ 리팩토링만 계속하기

**추천:**
1. **먼저 작동하게 만들기** (Make it work)
2. **그 다음 제대로 만들기** (Make it right)
3. **마지막에 빠르게 만들기** (Make it fast)

---

### 8.2 디버깅 팁

**Unity Console 활용:**
```csharp
Debug.Log($"카드 사용: {card.name}, 남은 내공: {player.currentEnergy}");
Debug.LogWarning("체력이 낮습니다!");
Debug.LogError("치명적 오류!");
```

**조건부 디버그:**
```csharp
#if UNITY_EDITOR
    Debug.Log("에디터에서만 표시");
#endif
```

**Assert 사용:**
```csharp
using UnityEngine.Assertions;

void PlayCard(Card card)
{
    Assert.IsNotNull(card, "카드가 null입니다!");
    Assert.IsTrue(player.currentEnergy >= card.cost, "내공 부족!");
}
```

---

### 8.3 Git 커밋 메시지 규칙

**권장 형식:**
```
[타입] 요약 (50자 이내)

상세 설명 (선택적)

예시:
[feat] 카드 드로우 시스템 구현
[fix] 카드 중복 사용 버그 수정
[refactor] DeckManager 코드 정리
[test] DeckManager 유닛 테스트 추가
[docs] README에 설치 방법 추가
```

---

### 8.4 학습 자료

**Unity 공식 문서:**
- [Unity Learn](https://learn.unity.com/)
- [Scripting API](https://docs.unity3d.com/ScriptReference/)

**추천 YouTube 채널:**
- **Brackeys** (Unity 기초, 영어)
- **Code Monkey** (게임 패턴, 영어)
- **골드메탈** (Unity 한글 강의)

**덱 빌더 게임 참고:**
- Slay the Spire Modding Wiki
- [GitHub: Slay the Spire Clone Projects](https://github.com/topics/slay-the-spire)

---

### 8.5 시간 관리

**Phase 1 (1-2개월) 예상 시간 배분:**
- 프로젝트 설정: 1일
- 데이터 시스템: 3일
- 전투 시스템: 1주
- 카드 시스템: 1주
- UI: 1주
- 적 AI: 3일
- 테스트 및 버그 수정: 1주
- **총 4-6주**

**하루 권장 작업량:**
- 주니어: 2-4시간 집중 코딩
- 하루 1-2개 하위 작업 완료 목표
- 막히면 30분 이상 고민하지 말고 검색/질문

---

### 8.6 막혔을 때 대처법

1. **에러 메시지 복사 → Google 검색**
2. **Stack Overflow 검색**
3. **Unity Forum 검색**
4. **ChatGPT/Claude에게 질문** (코드와 에러 함께 제공)
5. **Discord 커뮤니티** (Unity Korea, Gamedev KR)

**질문 템플릿:**
```
[문제]
카드를 클릭했을 때 NullReferenceException 발생

[기대 동작]
카드 클릭 시 적에게 피해를 줘야 함

[실제 동작]
에러 발생: NullReferenceException: Object reference not set to an instance of an object

[코드]
(관련 코드 붙여넣기)

[시도한 것]
- Debug.Log로 card가 null인지 확인 → null이 아님
- target이 null인지 확인 → null임!
```

---

## 9. 체크리스트

### 9.1 Phase 1 시작 전 체크리스트

- [ ] Unity 2022.3 LTS 설치 완료
- [ ] Visual Studio 또는 Rider 설치 및 Unity 연동
- [ ] Git 설치 및 GitHub 레포 생성
- [ ] .gitignore 설정 완료
- [ ] 프로젝트 폴더 구조 생성
- [ ] Newtonsoft.Json 패키지 설치
- [ ] 첫 커밋 완료 ("Initial project setup")

### 9.2 Phase 1 완료 기준

- [ ] 전투 시스템 작동 (플레이어 vs 적)
- [ ] 20장 카드 로딩 및 사용 가능
- [ ] 3종 적 AI 작동
- [ ] 승리/패배 조건 작동
- [ ] UI에서 체력, 내공, 카드 패 표시
- [ ] 빌드 가능 (실행 파일 생성)
- [ ] 2-3명 테스트 후 긍정 피드백

### 9.3 Phase 2 완료 기준

- [ ] 1개 지역 맵 생성 및 탐험 가능
- [ ] 상점, 휴식, 이벤트 노드 작동
- [ ] 보스 전투 승리 가능
- [ ] 50장 카드 밸런스 완료
- [ ] 20개 유물 작동
- [ ] 메타 진행 시스템 작동 (무공 정수, 업그레이드)
- [ ] 세이브/로드 작동
- [ ] 외부 테스터 긍정 반응 80% 이상

---

## 10. 마무리

### 10.1 최종 권장 기술 스택

```
게임 엔진:    Unity 2022.3 LTS
언어:         C#
IDE:          Visual Studio 2022 Community
버전 관리:    Git + GitHub
데이터 형식:  JSON (Newtonsoft.Json)
테스트:       Unity Test Framework
빌드 타겟:    Windows (Phase 1-2), Mac/Linux (Phase 3+)
```

### 10.2 핵심 아키텍처 원칙

1. **매니저 패턴**: GameManager, DataManager, CombatManager 등 Singleton 사용
2. **이벤트 기반**: 시스템 간 결합도 최소화
3. **레이어 분리**: UI - Logic - Data
4. **데이터 기반**: JSON으로 밸런싱 용이하게
5. **단순함 유지**: 오버엔지니어링 금지, YAGNI 원칙

### 10.3 성공을 위한 마인드셋

- ✅ **완벽보다 완성**: 일단 작동하게 만들기
- ✅ **작게 시작**: Phase 1 → Phase 2 → Phase 3 순차 진행
- ✅ **피드백 중시**: 빨리 테스트하고 개선
- ✅ **문서화**: 나중의 나를 위해 주석과 README 작성
- ✅ **즐기기**: 무협 게임을 만든다는 즐거움 잊지 말기!

---

**이제 준비가 끝났습니다. 강호로 떠나볼까요? 🗡️**

```bash
git checkout -b feature/phase1-prototype
# 첫 번째 작업 시작!
```

**행운을 빕니다, 개발자님! 💪**
