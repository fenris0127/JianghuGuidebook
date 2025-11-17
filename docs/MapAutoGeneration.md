# 맵 자동 생성 시스템

## 개요

MapManager는 **완전 자동화된 맵 생성 시스템**입니다. 프리팹이 없어도 기본 노드를 런타임에 동적으로 생성합니다.

## 자동 생성 기능

### 1. 노드 자동 생성
- 레이어 수, 노드 수, 간격 등은 `MapConfig`에서 설정
- 노드 타입 (전투, 이벤트, 상점, 휴식 등) 자동 배정
- 전투 노드는 랜덤 Encounter 할당
- 이벤트 노드는 랜덤 Event 할당

### 2. 경로 자동 생성
- 레이어 간 연결 경로 자동 생성
- 경로 밀도는 `MapConfig.maxPathDensity`로 조정
- 가까운 노드끼리 우선 연결

### 3. 프리팹 자동 로드
모든 프리팹은 Resources 폴더에서 자동으로 로드됩니다:

```
Resources/
└── Prefabs/
    └── MapNodes/
        ├── CombatNode.prefab      (여러 개 가능)
        ├── EventNode.prefab       (여러 개 가능)
        ├── ShopNode.prefab        (여러 개 가능)
        ├── RestNode.prefab        (여러 개 가능)
        ├── BossNode.prefab        (필수)
        ├── FinalBossNode.prefab   (필수)
        └── Path.prefab            (선택)
```

## 프리팹 없이 사용하기 (완전 자동)

프리팹이 하나도 없어도 **맵은 정상적으로 생성**됩니다:

### 프리팹이 없을 때 동작
1. **일반 노드**: 기본 GameObject + MapNode 컴포넌트 + SpriteRenderer (흰색)
2. **보스 노드**: 기본 GameObject + MapNode 컴포넌트 + SpriteRenderer (빨간색)
3. **경로**: 기본 LineRenderer (폭 0.1)

### 자동 생성되는 컴포넌트
```csharp
- MapNode 컴포넌트 (필수)
- SpriteRenderer (비주얼)
- CircleCollider2D (클릭 감지, radius: 0.5)
```

## 프리팹 사용하기 (권장)

더 나은 비주얼을 위해 프리팹을 사용할 수 있습니다.

### 프리팹 제작 가이드

#### 1. 일반 노드 프리팹
```
GameObject
├── MapNode (컴포넌트)
├── SpriteRenderer (아이콘)
├── Collider2D (클릭 감지)
└── (선택) 애니메이션, 파티클 등
```

**MapNode 컴포넌트 설정:**
- `nodeType`: Combat, Event, Shop, RestSite 중 선택
- 나머지는 런타임에 자동 할당

#### 2. 보스 노드 프리팹
```
GameObject (BossNode.prefab)
├── MapNode (nodeType = Combat)
├── SpriteRenderer (보스 아이콘)
└── (특별한 비주얼 효과)
```

#### 3. 경로 프리팹 (선택)
```
GameObject (Path.prefab)
└── LineRenderer
    ├── Width: 0.1
    ├── Color: 원하는 색상
    └── Material: 원하는 재질
```

### 프리팹 배치

1. 프리팹을 제작합니다
2. `Resources/Prefabs/MapNodes/` 폴더에 저장합니다
3. **끝!** MapManager가 자동으로 로드합니다

**중요:** Unity 에디터에서 수동 할당이 필요 없습니다!

## MapConfig 설정

`Assets/Resources/Config/MapConfig.asset` 파일에서 조정:

```
맵 구조:
- mapLength: 맵 길이 (기본 10)
- minNodesPerLayer: 레이어당 최소 노드 수 (기본 2)
- maxNodesPerLayer: 레이어당 최대 노드 수 (기본 4)
- layerSpacing: 레이어 간 간격 (기본 3.5)
- nodeVerticalSpacing: 노드 세로 간격 (기본 2.0)
- maxPathDensity: 최대 경로 수 (기본 2)
- finalFloor: 최종 보스 층 (기본 3)

이벤트 확률:
- rareEventChance: 희귀 이벤트 확률 (기본 15%)
- uncommonEventChance: 고급 이벤트 확률 (기본 50%)
```

## 사용 예시

### 게임 시작 시
```csharp
// GameManager.cs
public void StartNewGame(FactionData faction)
{
    // ...
    MapManager.Instance.GenerateMap(currentFloor);
    // 맵이 자동으로 생성됩니다!
}
```

### 다음 층으로 이동
```csharp
// GameManager.cs
public void ClearFloor()
{
    currentFloor++;
    MapManager.Instance.GenerateMap(currentFloor);
    // 새로운 맵이 자동으로 생성됩니다!
}
```

## 디버그 로그

프리팹이 없을 때 콘솔에 경고가 표시됩니다:

```
MapNodes 프리팹을 찾을 수 없습니다. Resources/Prefabs/MapNodes/ 폴더를 확인하세요.
→ 기본 노드가 자동 생성됩니다 (정상 작동)

노드 프리팹이 없어 기본 노드를 생성합니다. Layer: 0
→ 해당 레이어에 기본 노드 사용 (정상 작동)
```

**경고가 있어도 게임은 정상 작동**합니다!

## 요약

✅ **프리팹 없어도 작동**: 기본 노드 자동 생성
✅ **프리팹 자동 로드**: Resources 폴더에서 자동 검색
✅ **수동 할당 불필요**: Unity 에디터에서 드래그 앤 드롭 필요 없음
✅ **밸런스 조정 쉬움**: MapConfig에서 모든 설정 조정
✅ **완전 자동화**: `GenerateMap(floor)` 한 줄로 맵 생성

## 개선 사항

기존 시스템과 비교:
- ❌ 이전: SerializeField로 프리팹 수동 할당 필요
- ✅ 현재: Resources에서 자동 로드
- ✅ 프리팹 없어도 런타임에 동적 생성
- ✅ 에러 처리 강화 (null 체크, 폴백 로직)
