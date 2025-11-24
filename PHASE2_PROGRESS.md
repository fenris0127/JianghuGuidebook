# Phase 2 수직 슬라이스 진행 상황

## 현재 상태: 핵심 시스템 구현 완료 (약 50%)

### ✅ 완료된 작업

#### Task 0.0: Phase 2 브랜치 및 설정 ✅
- `feature/phase2-vertical-slice` 브랜치 생성
- Phase 2 폴더 구조 확장
- 새 폴더: Map/, Relics/, Shop/, Events/, Meta/, Rest/, Save/, Rewards/, Economy/

#### Task 1.0: 맵 생성 시스템 ✅
**구현된 파일:**
- `NodeType.cs` - 8가지 노드 타입 열거형
- `MapNode.cs` - 맵 노드 클래스
- `MapGenerator.cs` - 절차적 맵 생성 엔진
- `Region1Config.json` - 1지역 설정 파일

**기능:**
- 레이어 기반 노드 생성 (15 레이어)
- 노드 개수 조절 (시작 1개 → 중간 3-5개 → 보스 1개)
- 가중치 기반 노드 타입 분포
  - Combat: 50%
  - EliteCombat: 10%
  - Shop: 10%
  - Rest: 10%
  - Event: 15%
  - Treasure: 5%
- 시드 기반 재현 가능한 생성
- 노드 간 연결 로직 (1-3개 연결)
- BFS 경로 검증 (시작→보스 보장)

#### Task 3.0: 맵 진행 로직 ✅
**구현된 파일:**
- `MapManager.cs` - 맵 상태 및 진행 관리

**기능:**
- 맵 생성 및 초기화 (GenerateNewMap)
- 노드 이동 로직 (MoveToNode)
- 접근 가능한 노드 자동 업데이트
- 현재 노드 추적
- 노드 완료 처리
- 진행률 계산
- 이벤트 시스템 (OnNodeEntered, OnMapGenerated)

#### Task 4.0: 유물 시스템 ✅
**구현된 파일:**
- `Relic.cs` - 유물 클래스 및 데이터 구조
- `RelicEffect.cs` - 유물 효과 인터페이스
- `RelicManager.cs` - 유물 관리 시스템
- `RelicDatabase.json` - 20개 유물 데이터

**유물 희귀도:**
- Common (일반): 10개
- Uncommon (고급): 6개
- Rare (희귀): 3개
- Legendary (전설): 1개

**유물 예시:**
- **일반:** 낡은 검집 (전투 시작 시 방어도 6), 기혈환 (턴마다 체력 2 회복)
- **고급:** 백년인삼 (최대 체력 +15), 철검 (모든 공격 +3)
- **희귀:** 구양신공 서책 (턴마다 내공 +2), 용린검 (적 처치 시 체력 회복)
- **전설:** 소요신공 비급 (매 턴 첫 카드 무료)

**유물 효과 타입 (14종):**
- OnCombatStart, OnTurnStart, OnTurnEnd
- OnCardPlay, OnAttack, OnDefend
- OnDamageReceived, OnEnemyDeath
- OnDraw, OnDiscard
- OnRest, OnShop, OnVictory
- Passive (지속 효과)

#### Task 5.0: 골드 및 경제 시스템 ✅
**구현된 파일:**
- `GoldManager.cs` - 골드 관리

**기능:**
- 골드 획득/소비 (GainGold, SpendGold)
- 골드 충분 여부 체크 (HasEnoughGold)
- 전투 승리 보상 계산 (40-60 골드)
- 이벤트 시스템 (OnGoldChanged, OnGoldGained, OnGoldSpent)

---

### 🚧 남은 작업

#### Task 2.0: 맵 UI 및 네비게이션 (Unity 에디터 필요)
- MapScene 생성
- 노드 프리팹 (타입별 아이콘)
- 맵 시각화 및 렌더링
- 노드 클릭/호버 인터랙션
- 카메라 이동 및 줌

#### Task 3.4-3.8: 씬 전환 로직
- 노드 타입별 씬 로드
- 노드 완료 후 맵 복귀
- 세이브 포인트

#### Task 4.8-4.12: 유물 UI 및 시너지
- RelicDisplayUI 구현
- 유물 툴팁
- 유물 획득 연출
- 유물 조합 시너지

#### Task 5.3-5.4: 골드 UI
- 골드 표시 UI
- 획득 애니메이션

#### Task 6.0: 상점 시스템
- ShopManager, ShopUI
- 상품 생성 알고리즘
- 구매/판매 시스템
- 카드 제거/업그레이드 서비스

#### Task 7.0: 휴식 시스템
- RestManager, RestUI
- 3가지 휴식 선택지
  - 수면 (체력 30% 회복)
  - 수련 (카드 1장 업그레이드)
  - 타파심마 (카드 2장 업그레이드, 체력 10% 손실)

#### Task 8.0-12.0: 보스, 이벤트, 메타 진행 등
- 보스 시스템 및 페이즈
- 랜덤 이벤트 시스템
- 메타 진행 (무공 정수, 영구 업그레이드)
- 세이브/로드 시스템
- 보상 시스템
- 카드 50장 확장

---

## 기술 스택 (Phase 2 추가)

**새 시스템:**
- 절차적 맵 생성 (레이어 기반)
- 유물 효과 시스템 (14가지 트리거)
- 경제 시스템 (골드 관리)

**데이터 파일:**
- Region1Config.json (지역 설정)
- RelicDatabase.json (20개 유물)

## 프로젝트 구조 (Phase 2 추가)

```
Assets/
├── Data/
│   ├── Maps/
│   │   └── Region1Config.json
│   ├── Relics/
│   ├── Events/
│   ├── Meta/
│   └── Bosses/
├── Resources/
│   └── RelicDatabase.json
└── Scripts/
    ├── Map/                # 맵 시스템
    │   ├── NodeType.cs
    │   ├── MapNode.cs
    │   ├── MapGenerator.cs
    │   └── MapManager.cs
    ├── Relics/             # 유물 시스템
    │   ├── Relic.cs
    │   ├── RelicEffect.cs
    │   └── RelicManager.cs
    ├── Economy/            # 경제 시스템
    │   └── GoldManager.cs
    ├── Shop/               # 상점 (미구현)
    ├── Rest/               # 휴식 (미구현)
    ├── Events/             # 이벤트 (미구현)
    ├── Meta/               # 메타 진행 (미구현)
    ├── Save/               # 세이브 (미구현)
    └── Rewards/            # 보상 (미구현)
```

## Phase 1 + Phase 2 통합 상태

### Phase 1 시스템 (완료)
- ✅ 전투 시스템 (Player, Enemy, StatusEffect)
- ✅ 카드 시스템 (Card, DeckManager, CardEffects)
- ✅ 데이터 관리 (CardData, EnemyData, DataManager)
- ✅ 20장 카드 + 3가지 적

### Phase 2 시스템 (진행 중)
- ✅ 맵 생성 (MapGenerator, MapManager)
- ✅ 유물 (Relic, RelicManager) - 20개
- ✅ 경제 (GoldManager)
- 🚧 상점 (미구현)
- 🚧 휴식 (미구현)
- 🚧 이벤트 (미구현)
- 🚧 보스 (미구현)
- 🚧 메타 진행 (미구현)

## 다음 단계

### 우선순위 1: Unity 에디터 작업
1. MapScene 생성 및 MapUI 구현
2. 노드 프리팹 및 시각화
3. 상점/휴식 씬 생성

### 우선순위 2: 게임 루프 완성
1. 맵 → 전투 → 보상 → 맵 사이클 구현
2. 씬 전환 로직
3. 세이브/로드 시스템

### 우선순위 3: 콘텐츠 확장
1. 카드 50장으로 확장
2. 랜덤 이벤트 10개 구현
3. 보스 전투 구현

---

**작성일:** 2025-11-24
**브랜치:** feature/phase2-vertical-slice
**상태:** 핵심 시스템 50% 완료
**Phase 1 + Phase 2 총 진행률:** ~60%
