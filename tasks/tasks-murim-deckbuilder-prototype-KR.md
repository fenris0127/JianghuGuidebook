# 작업 목록: 강호무적 (무협 덱빌더 로그라이크) - Phase 1 프로토타입

## 프로젝트 개요

이 작업 목록은 무협 덱 빌딩 로그라이크 게임 "강호무적"의 **Phase 1: 프로토타입** 개발을 다룹니다. 목표는 1-2개월 내에 핵심 게임플레이 메커니즘을 구현하고 검증하는 것입니다.

**프로토타입 성공 기준:**
- 플레이 가능한 턴제 전투 시스템
- 20장의 기능적 카드 (공격 7장, 방어 7장, 스킬 6장)
- 기본 AI를 가진 3가지 적 타입
- 카드 패, 내공 표시, 적 의도가 포함된 전투 UI
- 시작부터 승리/패배까지 완전한 단일 전투 루프

**참고:** 상세 요구사항은 `prd-murim-deckbuilder-roguelike-KR.md`를 참조하세요.

---

## 작업 완료 방법

**중요:** 각 작업을 완료할 때마다 이 마크다운 파일에서 `- [ ]`를 `- [x]`로 변경하여 체크 표시를 해야 합니다. 이는 진행 상황을 추적하고 단계를 건너뛰지 않도록 보장합니다.

예시:
- `- [ ] 1.1 파일 읽기` → `- [x] 1.1 파일 읽기` (완료 후)

전체 상위 작업을 완료한 후가 아니라, 각 하위 작업을 완료한 후 파일을 업데이트하세요.

---

## 관련 파일

**핵심 시스템 파일:**
- `Assets/Scripts/Core/GameManager.cs` - 메인 게임 루프 및 상태 관리
- `Assets/Scripts/Core/CombatManager.cs` - 전투 흐름 제어, 턴 관리
- `Assets/Scripts/Core/Constants.cs` - 게임 상수 (시작 내공, 드로우 수 등)

**데이터 관리:**
- `Assets/Data/Cards/CardDatabase.json` - 카드 정의 및 스탯
- `Assets/Data/Enemies/EnemyDatabase.json` - 적 데이터 및 행동 패턴
- `Assets/Scripts/Data/CardData.cs` - 카드 데이터 구조
- `Assets/Scripts/Data/EnemyData.cs` - 적 데이터 구조
- `Assets/Scripts/Data/DataManager.cs` - 게임 데이터 로드 및 관리

**카드 시스템:**
- `Assets/Scripts/Cards/Card.cs` - 속성과 효과를 가진 카드 클래스
- `Assets/Scripts/Cards/CardEffects.cs` - 카드 효과 구현
- `Assets/Scripts/Cards/DeckManager.cs` - 덱, 패, 버리기 더미 관리
- `Assets/Scripts/Cards/CardTypes.cs` - 카드 타입 열거형 (초식, 신법, 기공, 비기)

**전투 시스템:**
- `Assets/Scripts/Combat/Player.cs` - 플레이어 스탯 (체력, 내공, 버프)
- `Assets/Scripts/Combat/Enemy.cs` - 적 스탯 및 행동
- `Assets/Scripts/Combat/CombatState.cs` - 전투 상태 열거형 (플레이어턴, 적턴, 승리, 패배)
- `Assets/Scripts/Combat/StatusEffect.cs` - 버프/디버프 시스템

**적 AI:**
- `Assets/Scripts/AI/EnemyAI.cs` - 적 의사결정 로직
- `Assets/Scripts/AI/IntentSystem.cs` - 적 의도 표시 시스템

**UI:**
- `Assets/Scripts/UI/CombatUI.cs` - 메인 전투 UI 컨트롤러
- `Assets/Scripts/UI/CardUI.cs` - 개별 카드 시각 컴포넌트
- `Assets/Scripts/UI/EnemyIntentUI.cs` - 적 의도 표시
- `Assets/Scripts/UI/PlayerStatsUI.cs` - 플레이어 체력, 내공 표시
- `Assets/Prefabs/CardPrefab.prefab` - 카드 UI 프리팹
- `Assets/Prefabs/EnemyPrefab.prefab` - 적 시각 프리팹

**테스트:**
- `Assets/Tests/PlayMode/CombatSystemTests.cs` - 전투 시스템 통합 테스트
- `Assets/Tests/EditMode/CardSystemTests.cs` - 카드 시스템 유닛 테스트
- `Assets/Tests/EditMode/DeckManagerTests.cs` - 덱 관리 테스트

### 주의사항

- 이것은 Unity 기반 프로젝트 구조입니다. Godot을 사용하는 경우 경로를 조정하세요 (예: `Assets/Scripts/` 대신 `res://scripts/`)
- Unity의 경우 Unity Test Framework를 테스트에 사용하세요
- JSON 파일은 `Assets/Data/`에 배치하고 Unity에서 TextAsset으로 표시해야 합니다
- 모든 스크립트는 C# 코딩 규칙을 따라야 합니다

---

## 작업 목록

- [x] 0.0 기능 브랜치 생성 및 프로젝트 설정
  - [x] 0.1 새 브랜치 `feature/phase1-prototype` 생성 및 체크아웃
  - [x] 0.2 게임 엔진 선택 (Unity 또는 Godot) 및 새 프로젝트 생성
  - [x] 0.3 선택한 엔진에 맞는 .gitignore 설정
  - [x] 0.4 초기 폴더 구조 생성: `Assets/Scripts/`, `Assets/Data/`, `Assets/Prefabs/`, `Assets/Tests/`
  - [x] 0.5 README.md에 엔진 선택 및 설정 지침 문서화
  - [x] 0.6 초기 프로젝트 구조 커밋

- [ ] 1.0 Unity/Godot 프로젝트 구조 및 핵심 아키텍처 설정
  - [x] 1.1 `Core` 폴더 생성 및 싱글톤 패턴을 가진 `GameManager.cs` 추가
  - [x] 1.2 전투 흐름을 처리할 `CombatManager.cs` 생성
  - [x] 1.3 `CombatState` 열거형 정의 (PlayerTurn, EnemyTurn, Victory, Defeat)
  - [x] 1.4 게임 상수를 가진 `Constants.cs` 생성 (STARTING_ENERGY = 3, STARTING_DRAW = 5, MAX_HAND_SIZE = 10)
  - [ ] 1.5 씬 구조 설정: 메인 테스트 씬으로 "CombatScene" 생성
  - [x] 1.6 CombatManager에 기본 상태 머신 구현 (StartCombat, PlayerTurnStart, PlayerTurnEnd, EnemyTurn, EndCombat)
  - [ ] 1.7 씬 로딩 및 기본 전투 상태 전환 테스트

- [x] 2.0 카드 및 적 데이터 관리 시스템 구현
  - [x] 2.1 `CardData.cs` 클래스 생성 (속성: id, name, cost, type, category, rarity, baseDamage, baseBlock, description)
  - [x] 2.2 `EnemyData.cs` 클래스 생성 (속성: id, name, maxHealth, actions (type, value, weight))
  - [x] 2.3 20장의 카드를 위한 플레이스홀더 구조로 `CardDatabase.json` 생성
  - [x] 2.4 3개의 적 정의로 `EnemyDatabase.json` 생성
  - [x] 2.5 Unity의 JsonUtility 또는 Newtonsoft.Json을 사용하여 런타임에 JSON 파일을 로드하는 `DataManager.cs` 생성
  - [x] 2.6 CardData 역직렬화 및 유효성 검사 구현
  - [x] 2.7 EnemyData 역직렬화 및 유효성 검사 구현
  - [x] 2.8 누락/유효하지 않은 데이터 파일에 대한 에러 처리 추가
  - [x] 2.9 플레이 모드에서 데이터 로딩 테스트

- [x] 3.0 핵심 전투 시스템 로직 구현
  - [x] 3.1 `Player.cs` 생성 (속성: currentHealth, maxHealth, currentEnergy, maxEnergy, block, statusEffects 리스트)
  - [x] 3.2 `Enemy.cs` 생성 (속성: currentHealth, maxHealth, currentIntent, statusEffects 리스트)
  - [x] 3.3 Player.TakeDamage(int amount) 구현 - 먼저 방어도로 피해 감소
  - [x] 3.4 Player.GainBlock(int amount) 구현 - 방어도 추가 (턴 종료 시 0으로 초기화)
  - [x] 3.5 Enemy.TakeDamage(int amount) 메서드 구현
  - [x] 3.6 가상 Apply() 및 Remove() 메서드를 가진 `StatusEffect.cs` 기본 클래스 생성
  - [x] 3.7 CombatManager에 턴 흐름 구현:
    - [x] 3.7.1 StartPlayerTurn: 내공을 최대치로 리셋, 방어도를 0으로 리셋, 카드 5장 드로우
    - [x] 3.7.2 EndPlayerTurn: 패 버리기, 턴 종료 효과 발동
    - [x] 3.7.3 StartEnemyTurn: 적 의도 실행, 다음 의도 준비
  - [x] 3.8 승리 조건 체크 구현 (모든 적 격파)
  - [x] 3.9 패배 조건 체크 구현 (플레이어 체력 <= 0)
  - [x] 3.10 디버그 로그로 전투 흐름 테스트 (UI 없이)

- [ ] 4.0 카드 시스템 구현 (드로우, 사용, 버리기 메커니즘)
  - [ ] 4.1 `CardTypes.cs` 열거형 생성: Attack, Defense, Skill, Secret
  - [ ] 4.2 ScriptableObject 또는 일반 클래스를 상속하는 `Card.cs` 클래스 생성
  - [ ] 4.3 CardData로부터 Card 생성자 구현
  - [ ] 4.4 리스트를 가진 `DeckManager.cs` 생성: drawPile, hand, discardPile, exhaustPile
  - [ ] 4.5 DeckManager.InitializeDeck(List<CardData>) 구현 - drawPile 채우고 셔플
  - [ ] 4.6 DeckManager.DrawCard() 구현 - drawPile에서 hand로 카드 이동, 필요시 버리기 더미 재셔플
  - [ ] 4.7 패 크기 제한(10)을 가진 DeckManager.DrawCards(int count) 구현
  - [ ] 4.8 DeckManager.PlayCard(Card card) 구현 - hand에서 제거, 효과 적용, discard에 추가
  - [ ] 4.9 DeckManager.DiscardHand() 구현 - 모든 hand 카드를 discard로 이동
  - [ ] 4.10 Fisher-Yates 알고리즘을 사용하여 DeckManager.Shuffle(List<Card>) 구현
  - [ ] 4.11 메서드를 가진 `CardEffects.cs` 생성: DealDamage(int, Enemy), GainBlock(int, Player), DrawCards(int)
  - [ ] 4.12 디버그 로그로 카드 드로우 및 사용 테스트

- [ ] 5.0 적 AI 및 의도 시스템 구현
  - [ ] 5.1 `IntentType` 열거형 생성: Attack, Defend, Buff, Debuff, Unknown
  - [ ] 5.2 속성을 가진 `Intent` 클래스 생성: type, value (피해/방어도 양)
  - [ ] 5.3 Enemy.DetermineIntent() 구현 - EnemyData의 가중치 랜덤으로 행동 선택
  - [ ] 5.4 ExecuteIntent(Enemy enemy, Player player) 메서드를 가진 `EnemyAI.cs` 생성
  - [ ] 5.5 공격 의도 실행 구현: 플레이어에게 피해
  - [ ] 5.6 방어 의도 실행 구현: 방어도 획득
  - [ ] 5.7 의도 미리보기 시스템 추가 - 적 턴 전에 다음 행동 표시
  - [ ] 5.8 3가지 다른 적 타입으로 적 행동 테스트

- [ ] 6.0 전투 UI 및 시각적 피드백 생성
  - [ ] 6.1 CardPrefab 생성: 카드 이미지 배경, 이름 텍스트, 비용 텍스트, 설명 텍스트, 타입 아이콘
  - [ ] 6.2 카드 데이터를 시각적으로 표시하는 `CardUI.cs` 컴포넌트 생성
  - [ ] 6.3 카드 호버 효과 구현 (확대, 툴팁 표시)
  - [ ] 6.4 카드를 사용하기 위한 드래그 앤 드롭 구현
  - [ ] 6.5 패 레이아웃 매니저 생성 (카드 가로 배치)
  - [ ] 6.6 체력 바와 내공 카운터를 표시하는 `PlayerStatsUI.cs` 생성
  - [ ] 6.7 적 의도 아이콘과 값을 표시하는 `EnemyIntentUI.cs` 생성
  - [ ] 6.8 적 체력 바 UI 생성
  - [ ] 6.9 onClick 리스너를 가진 "턴 종료" 버튼 생성
  - [ ] 6.10 클릭하여 볼 수 있는 더미 카운터 추가 (뽑기 더미, 버리기 더미, 소진 더미)
  - [ ] 6.11 피해 숫자 팝업 애니메이션 구현 (간단한 텍스트 페이드 아웃)
  - [ ] 6.12 카드 사용에 대한 시각적 피드백 추가 (빛나는 효과, 이동 애니메이션)
  - [ ] 6.13 플레이어/적에 방어 방패 아이콘 표시 추가
  - [ ] 6.14 "메뉴로 돌아가기" 버튼이 있는 승리/패배 화면 생성
  - [ ] 6.15 모든 UI 상호작용 및 반응성 테스트

- [ ] 7.0 초기 카드 세트 생성 (20장)
  - [ ] 7.1 PRD 사양에 따라 7장의 공격 카드 디자인:
    - [ ] 7.1.1 일검 (Strike) - 1 내공, 6 피해
    - [ ] 7.1.2 쌍검난무 (Double Strike) - 1 내공, 3 피해 2회
    - [ ] 7.1.3 참마검 (Heavy Slash) - 2 내공, 12 피해
    - [ ] 7.1.4 검기참 (Sword Wave) - 1 내공, 7 피해
    - [ ] 7.1.5 연환삼검 (Triple Slash) - 2 내공, 4 피해 3회
    - [ ] 7.1.6 용검섬 (Dragon Sword Flash) - 1 내공, 8 피해
    - [ ] 7.1.7 천검귀일 (Thousand Swords Return) - 3 내공, 20 피해
  - [ ] 7.2 7장의 방어 카드 디자인:
    - [ ] 7.2.1 철포삼 (Iron Guard) - 1 내공, 방어도 5 획득
    - [ ] 7.2.2 신법 (Swift Dodge) - 1 내공, 방어도 6 획득
    - [ ] 7.2.3 무영신법 (Shadow Step) - 2 내공, 방어도 12 획득
    - [ ] 7.2.4 방어자세 (Defensive Stance) - 1 내공, 방어도 7 획득
    - [ ] 7.2.5 금종죄 (Golden Bell) - 2 내공, 방어도 10 획득
    - [ ] 7.2.6 회광반조 (Reflect) - 1 내공, 방어도 4 획득
    - [ ] 7.2.7 절대방어 (Absolute Defense) - 3 내공, 방어도 18 획득
  - [ ] 7.3 6장의 스킬 카드 디자인:
    - [ ] 7.3.1 내공운기 (Qi Circulation) - 1 내공, 카드 2장 드로우
    - [ ] 7.3.2 심검합일 (Sword Heart Unity) - 0 내공, 다음 공격 +3 피해
    - [ ] 7.3.3 명경지수 (Clear Mind) - 1 내공, 방어도 3 획득, 카드 1장 드로우
    - [ ] 7.3.4 검기방출 (Sword Aura Release) - 2 내공, 모든 적에게 10 피해, 소진
    - [ ] 7.3.5 기혈순환 (Energy Flow) - 1 내공, 체력 5 회복
    - [ ] 7.3.6 천지무극 (Heaven Earth Infinite) - 2 내공, 카드 3장 드로우, 방어도 5 획득
  - [ ] 7.4 적절한 형식으로 CardDatabase.json에 모든 20장의 카드 추가
  - [ ] 7.5 JSON 문법 및 카드 밸런스 검증
  - [ ] 7.6 게임 내 모든 카드 로딩 테스트

- [ ] 8.0 적 타입 생성 및 밸런스 테스트
  - [ ] 8.1 적 타입 1 생성: 산적 (Bandit)
    - [ ] 8.1.1 스탯: 40 HP
    - [ ] 8.1.2 행동: 공격 (6 피해, 60% 가중치), 방어 (8 방어도, 40% 가중치)
  - [ ] 8.2 적 타입 2 생성: 마도수련자 (Dark Cultivator)
    - [ ] 8.2.1 스탯: 30 HP
    - [ ] 8.2.2 행동: 공격 (8 피해, 50%), 버프 (+2 힘, 30%), 공격 (4 피해 2회, 20%)
  - [ ] 8.3 적 타입 3 생성: 철갑위병 (Armored Guard)
    - [ ] 8.3.1 스탯: 50 HP
    - [ ] 8.3.2 행동: 공격 (5 피해, 40%), 방어 (12 방어도, 40%), 강 공격 (15 피해, 20%)
  - [ ] 8.4 EnemyDatabase.json에 모든 적 데이터 추가
  - [ ] 8.5 각 적 타입을 개별적으로 테스트하는 씬 생성
  - [ ] 8.6 밸런스 테스트: 플레이어가 시작 덱으로 각 적을 물리칠 수 있는가?
  - [ ] 8.7 플레이테스트 피드백을 기반으로 카드/적 스탯 조정
  - [ ] 8.8 밸런스 변경 사항 및 이유 문서화

- [ ] 9.0 통합 테스트 및 폴리싱
  - [ ] 9.1 포괄적인 플레이테스트 시나리오 생성 (플레이어 vs 각 적 타입)
  - [ ] 9.2 엣지 케이스 테스트:
    - [ ] 9.2.1 빈 덱에서 드로우 (버리기 더미 재셔플해야 함)
    - [ ] 9.2.2 패 제한 초과 (10장 이상 드로우 방지해야 함)
    - [ ] 9.2.3 내공 부족으로 카드 사용 (방지되어야 함)
    - [ ] 9.2.4 전투 중 적 격파 (승리로 전환되어야 함)
    - [ ] 9.2.5 플레이어 격파 (패배 화면으로 전환되어야 함)
  - [ ] 9.3 발견된 버그 수정
  - [ ] 9.4 기본 효과음 추가 (카드 사용, 피해, 승리, 패배) - 플레이스홀더 오디오 허용
  - [ ] 9.5 성능 최적화 (60 FPS 목표)
  - [ ] 9.6 테스트를 위한 디버그 콘솔/치트 명령 추가 (무적 모드, 즉시 승리, 카드 생성)
  - [ ] 9.7 Windows용 빌드 생성 (실행 파일)
  - [ ] 9.8 2-3명의 외부 테스터와 최종 플레이테스트 진행
  - [ ] 9.9 알려진 이슈 및 향후 개선 사항 문서화
  - [ ] 9.10 핵심 게임플레이를 보여주는 데모 비디오 준비 (30-60초)
  - [ ] 9.11 모든 변경 사항 커밋 및 푸시
  - [ ] 9.12 프로토타입 요약과 함께 풀 리퀘스트 생성

---

## 상태

**현재 단계:** Phase 1 - 프로토타입
**생성된 작업:** ✅ 완료 (상위 + 하위 작업)
**총 작업 수:** 9개 상위 작업, 100+ 하위 작업
**예상 시간:** 1-2개월 (PRD Phase 1 기준)

---

## 완료 후 다음 단계

Phase 1 프로토타입이 완료되고 검증된 후:
1. 플레이테스트로부터 피드백 수집
2. PRD Phase 2 요구사항 검토 (수직 슬라이스)
3. Phase 2를 위한 새 작업 목록 생성
4. 맵 시스템, 상점, 메타 진행 구현 고려
