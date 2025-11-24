# Phase 1 프로토타입 진행 상황 요약

## 현재 상태: 핵심 시스템 구현 완료 (약 80%)

### ✅ 완료된 작업 (Tasks 0-5, 7-8)

#### Task 0.0: 프로젝트 설정 ✅
- `feature/phase1-prototype` 브랜치 생성
- 폴더 구조 설정 (Scripts, Data, Tests)
- README.md 업데이트
- .gitignore 설정

#### Task 1.0: 핵심 아키텍처 ✅
**구현된 파일:**
- `GameManager.cs` - 싱글톤 패턴으로 게임 전체 관리
- `CombatManager.cs` - 완전한 전투 흐름 제어
- `CombatState.cs` - 전투 상태 열거형
- `Constants.cs` - 게임 상수 정의

**기능:**
- 상태 머신 기반 전투 흐름
- Player/Enemy 초기화 및 관리
- 턴 순환 시스템 (플레이어 턴 ↔ 적 턴)
- 승리/패배 조건 자동 체크

#### Task 2.0: 데이터 관리 시스템 ✅
**구현된 파일:**
- `CardData.cs` - 카드 데이터 구조 (20장 정의)
- `EnemyData.cs` - 적 데이터 구조 (3종 정의)
- `DataManager.cs` - JSON 런타임 로딩
- `CardDatabase.json` - 20장의 완전한 카드
- `EnemyDatabase.json` - 3가지 적 타입

**기능:**
- Resources.Load를 통한 JSON 로딩
- 유효성 검증 및 에러 처리
- Dictionary 기반 빠른 조회
- 가중치 기반 적 행동 선택

#### Task 3.0: 전투 시스템 로직 ✅
**구현된 파일:**
- `Player.cs` - 체력, 내공, 방어도 관리
- `Enemy.cs` - 체력, 의도, 행동 패턴
- `StatusEffect.cs` - 상태 효과 기본 클래스

**기능:**
- TakeDamage: 방어도 우선 피해 감소
- GainBlock: 방어도 획득 (턴 종료 시 초기화)
- ResetEnergy: 턴 시작 시 내공 리셋
- ExecuteIntent: 적 행동 실행
- 사망 이벤트 및 전투 종료 처리

#### Task 4.0: 카드 시스템 ✅
**구현된 파일:**
- `Card.cs` - 런타임 카드 클래스
- `DeckManager.cs` - 완전한 덱 관리
- `CardEffects.cs` - 카드 효과 처리

**기능:**
- 4개 더미 관리 (drawPile, hand, discardPile, exhaustPile)
- Fisher-Yates 셔플 알고리즘
- 자동 재셔플 (뽑기 더미 소진 시)
- 손패 크기 제한 (MAX_HAND_SIZE)
- 카드 효과 실행 (피해, 방어도, 드로우, 회복)
- 소진 카드 처리

#### Task 5.0: 적 AI 및 의도 시스템 ✅
**구현 사항:**
- `EnemyActionType` 열거형 (Attack, Defend, Buff, Debuff, Special)
- `EnemyAction` 클래스 (가중치 기반 행동 패턴)
- 적 의도 결정 및 미리보기
- 행동 실행 로직

#### Task 7.0: 초기 카드 세트 (20장) ✅
**공격 카드 (7장):**
- 일검 (6 피해)
- 쌍검난무 (3 피해 x2)
- 참마검 (12 피해)
- 검기참 (7 피해)
- 연환삼검 (4 피해 x3)
- 용검섬 (8 피해)
- 천검귀일 (20 피해)

**방어 카드 (7장):**
- 철포삼 (방어도 5)
- 신법 (방어도 6)
- 무영신법 (방어도 12)
- 방어자세 (방어도 7)
- 금종죄 (방어도 10)
- 회광반조 (방어도 4)
- 절대방어 (방어도 18)

**스킬 카드 (6장):**
- 내공운기 (드로우 2장)
- 심검합일 (다음 공격 +3)
- 명경지수 (방어도 3 + 드로우 1장)
- 검기방출 (전체 피해 10, 소진)
- 기혈순환 (체력 회복 5)
- 천지무극 (드로우 3장 + 방어도 5)

#### Task 8.0: 적 타입 생성 ✅
**적 타입 (3종):**
1. **산적** (40 HP)
   - 공격 6 피해 (60%)
   - 방어 8 방어도 (40%)

2. **마도수련자** (30 HP)
   - 공격 8 피해 (50%)
   - 버프 +2 힘 (30%)
   - 연속 공격 4x2 (20%)

3. **철갑위병** (50 HP)
   - 공격 5 피해 (40%)
   - 방어 12 방어도 (40%)
   - 강공격 15 피해 (20%)

---

### 🚧 남은 작업

#### Task 1.5, 1.7: Unity 씬 설정 및 테스트
- CombatScene 생성 (Unity 에디터 필요)
- GameManager, CombatManager GameObject 추가
- 전투 상태 전환 테스트
- 참고: `Assets/Scenes/COMBAT_SCENE_SETUP.md`

#### Task 6.0: 전투 UI 구현 (Unity 에디터 작업)
- CardPrefab 생성
- CardUI.cs 컴포넌트
- PlayerStatsUI (체력, 내공 표시)
- EnemyIntentUI (적 의도 표시)
- 손패 레이아웃 매니저
- 카드 드래그 앤 드롭
- 턴 종료 버튼
- 승리/패배 화면

#### Task 8.5-8.8: 밸런스 테스트
- 각 적 타입 개별 테스트 씬
- 시작 덱으로 승리 가능 여부 확인
- 스탯 조정
- 밸런스 변경 사항 문서화

#### Task 9.0: 통합 테스트 및 폴리싱
- 엣지 케이스 테스트
- 버그 수정
- 기본 효과음 추가
- 성능 최적화
- 디버그 치트 명령
- Windows 빌드
- 외부 플레이테스트
- PR 생성

---

## 기술 스택

**게임 엔진:** Unity (URP)
**언어:** C#
**데이터 포맷:** JSON
**아키텍처:** 싱글톤 패턴, 이벤트 기반 시스템

## 프로젝트 구조

```
Assets/
├── Resources/              # JSON 데이터 (런타임 로드)
│   ├── CardDatabase.json
│   └── EnemyDatabase.json
├── Scripts/
│   ├── Cards/             # 카드 시스템
│   │   ├── Card.cs
│   │   ├── DeckManager.cs
│   │   └── CardEffects.cs
│   ├── Combat/            # 전투 로직
│   │   ├── Player.cs
│   │   ├── Enemy.cs
│   │   └── StatusEffect.cs
│   ├── Core/              # 핵심 매니저
│   │   ├── GameManager.cs
│   │   ├── CombatManager.cs
│   │   ├── CombatState.cs
│   │   └── Constants.cs
│   └── Data/              # 데이터 구조
│       ├── CardData.cs
│       ├── EnemyData.cs
│       └── DataManager.cs
├── Scenes/
│   └── MainScene.unity
└── Tests/
    ├── EditMode/
    └── PlayMode/
```

## 다음 단계

1. **Unity 에디터에서 CombatScene 설정**
   - `COMBAT_SCENE_SETUP.md` 가이드 참조
   - GameManager, CombatManager GameObject 추가
   - 테스트 실행하여 콘솔 로그 확인

2. **기본 UI 구현**
   - CardPrefab 생성
   - 플레이어 스탯 표시
   - 적 의도 표시

3. **통합 테스트**
   - 전투 플레이테스트
   - 밸런스 조정
   - 버그 수정

## 커밋 히스토리

1. `feat: Phase 1 프로토타입 초기 설정`
2. `feat: 핵심 전투 시스템 아키텍처 구현`
3. `feat: 카드 및 적 데이터 관리 시스템 구현`
4. `feat: 핵심 전투 시스템 로직 구현 완료`
5. `feat: 카드 시스템 완전 구현`
6. `chore: Task 5.0 완료로 표시`
7. `chore: Tasks 7.0 및 8.0 대부분 완료로 표시`

---

**작성일:** 2025-11-24
**브랜치:** feature/phase1-prototype
**상태:** 핵심 시스템 구현 완료, UI 작업 대기 중
