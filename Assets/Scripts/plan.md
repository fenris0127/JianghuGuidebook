# 강호비급 개발 계획 (TDD)

## 개발 원칙
- Kent Beck의 TDD (Red → Green → Refactor) 사이클 준수
- 각 테스트는 한 가지 동작만 검증
- 테스트를 통과시키기 위한 최소한의 코드만 구현
- 모든 테스트 통과 후 리팩토링
- Tidy First: 구조적 변경과 동작 변경을 분리

## Phase 1: 핵심 데이터 모델 (Data Layer)

### 1.1 카드 데이터 구조
- [x] Test: CardData ScriptableObject를 생성할 수 있다
- [x] Test: CardData에 기본 속성(id, name, cost)을 설정할 수 있다
- [x] Test: CardData에 효과 리스트를 추가할 수 있다
- [x] Test: CardEffect에 데미지 타입과 값을 설정할 수 있다
- [x] Test: CardEffect에 방어도 타입과 값을 설정할 수 있다
- [x] Test: CardData의 Rarity(등급)를 설정할 수 있다
- [ ] Test: CardData의 문파(faction)를 설정할 수 있다 (유보: Phase 1.4에서 처리)

### 1.2 적 데이터 구조
- [x] Test: EnemyData ScriptableObject를 생성할 수 있다
- [x] Test: EnemyData에 기본 속성(id, name, maxHealth)을 설정할 수 있다
- [x] Test: EnemyData에 ActionPattern 리스트를 추가할 수 있다
- [x] Test: ActionPattern에 Attack 타입과 데미지를 설정할 수 있다
- [x] Test: ActionPattern에 Block 타입과 방어도를 설정할 수 있다

### 1.3 유물 데이터 구조
- [x] Test: RelicData ScriptableObject를 생성할 수 있다
- [x] Test: RelicData에 기본 속성(id, name, description)을 설정할 수 있다
- [x] Test: RelicData의 Trigger 타입(OnBattleStart, OnTurnStart 등)을 설정할 수 있다
- [x] Test: RelicData의 효과를 정의할 수 있다

### 1.4 문파 데이터 구조
- [x] Test: FactionData ScriptableObject를 생성할 수 있다
- [x] Test: FactionData에 기본 속성(id, name)을 설정할 수 있다
- [x] Test: FactionData에 시작 덱 리스트를 설정할 수 있다
- [x] Test: FactionData에 패시브 능력을 설정할 수 있다

## Phase 2: 플레이어 시스템 (Core Gameplay)

### 2.1 플레이어 상태 관리
- [x] Test: Player 객체를 생성할 수 있다
- [x] Test: Player의 현재 체력을 설정하고 읽을 수 있다
- [x] Test: Player의 최대 체력을 설정하고 읽을 수 있다
- [x] Test: Player가 데미지를 받으면 체력이 감소한다
- [x] Test: Player의 체력이 0 이하가 되면 사망 상태가 된다
- [x] Test: Player를 치유하면 체력이 회복되고 최대 체력을 넘지 않는다
- [x] Test: Player의 방어도를 설정하고 읽을 수 있다
- [x] Test: Player가 방어도를 가지고 있을 때 데미지를 받으면 방어도가 먼저 감소한다
- [x] Test: Player의 내공(Naegong)을 설정하고 읽을 수 있다
- [x] Test: Player의 골드를 설정하고 읽을 수 있다

### 2.2 플레이어 덱 관리
- [x] Test: Player에게 덱(Deck)을 설정할 수 있다
- [x] Test: Player의 덱에 카드를 추가할 수 있다
- [x] Test: Player의 덱에서 카드를 제거할 수 있다
- [x] Test: Player의 덱을 셔플할 수 있다
- [x] Test: Player의 핸드(Hand)에 카드를 뽑을 수 있다
- [x] Test: Player의 핸드에서 카드를 버릴 수 있다
- [x] Test: Player의 버린 카드 더미(Discard Pile)를 관리할 수 있다
- [x] Test: 덱이 비면 버린 카드 더미를 셔플하여 새 덱으로 만든다

### 2.3 플레이어 경지 시스템
- [x] Test: Player의 수련치(XP)를 획득할 수 있다
- [x] Test: 수련치가 100%에 도달하면 경지 돌파 가능 상태가 된다
- [x] Test: Player의 내공 경지를 상승시킬 수 있다
- [x] Test: 내공 경지가 상승하면 최대 내공이 증가한다
- [x] Test: Player의 무기술 경지를 기록할 수 있다
- [x] Test: 특정 업적을 달성하면 무기술 경지가 상승한다

### 2.4 플레이어 상태이상 관리
- [x] Test: Player에게 상태이상(StatusEffect)을 적용할 수 있다
- [x] Test: Player에게 '힘(Strength)' 버프를 적용하면 공격력이 증가한다
- [x] Test: Player에게 '약화(Weak)' 디버프를 적용하면 공격력이 감소한다
- [x] Test: Player에게 '취약(Vulnerable)' 디버프를 적용하면 받는 피해가 증가한다
- [x] Test: 턴 종료 시 일시적 상태이상의 지속 시간이 감소한다

## Phase 3: 적(Enemy) 시스템

### 3.1 적 상태 관리
- [x] Test: Enemy 객체를 생성할 수 있다
- [x] Test: Enemy를 EnemyData로 초기화할 수 있다
- [x] Test: Enemy의 체력을 설정하고 읽을 수 있다
- [x] Test: Enemy가 데미지를 받으면 체력이 감소한다
- [x] Test: Enemy의 체력이 0 이하가 되면 사망 상태가 된다
- [x] Test: Enemy의 방어도를 설정하고 읽을 수 있다

### 3.2 적 행동 패턴
- [x] Test: Enemy의 다음 행동(Intent)을 예고할 수 있다
- [x] Test: Enemy가 ActionPattern에 따라 행동을 선택한다
- [x] Test: Enemy가 공격 행동을 수행하면 Player가 데미지를 받는다
- [x] Test: Enemy가 방어 행동을 수행하면 방어도를 얻는다
- [x] Test: Enemy가 버프/디버프 행동을 수행할 수 있다

### 3.3 적 상태이상 관리
- [x] Test: Enemy에게 '중독(Poison)' 디버프를 적용할 수 있다
- [x] Test: 턴 종료 시 중독 데미지를 받는다
- [x] Test: Enemy에게 여러 상태이상을 동시에 적용할 수 있다

## Phase 4: 전투 시스템 (Battle Manager)

### 4.1 전투 초기화
- [x] Test: BattleManager를 싱글톤으로 생성할 수 있다
- [x] Test: 전투를 EncounterData로 초기화할 수 있다
- [x] Test: 전투 시작 시 적들을 생성한다
- [x] Test: 전투 시작 시 Player의 덱을 셔플한다
- [x] Test: 전투 시작 시 초기 핸드를 뽑는다

### 4.2 턴 관리
- [x] Test: Player 턴을 시작할 수 있다
- [x] Test: Player 턴 시작 시 내공이 리필된다
- [x] Test: Player 턴 시작 시 카드를 뽑는다
- [x] Test: Enemy 턴을 시작할 수 있다
- [x] Test: Enemy 턴에 모든 적이 순서대로 행동한다
- [x] Test: 턴 종료 시 핸드의 카드를 버린다

### 4.3 카드 사용
- [ ] Test: Player가 카드를 사용할 수 있다
- [ ] Test: 카드 사용 시 내공이 소모된다
- [ ] Test: 내공이 부족하면 카드를 사용할 수 없다
- [ ] Test: 카드 효과가 올바르게 적용된다
- [ ] Test: 타겟이 필요한 카드는 타겟을 지정해야 사용할 수 있다
- [ ] Test: '절초' 카드는 사용 후 소멸된다

### 4.4 전투 종료
- [ ] Test: 모든 적이 사망하면 전투에서 승리한다
- [ ] Test: Player가 사망하면 전투에서 패배한다
- [ ] Test: 전투 승리 시 수련치를 획득한다
- [ ] Test: 전투 승리 시 보상(카드, 골드)를 받는다

## Phase 5: 연계 초식 시스템

### 5.1 연계 조건
- [ ] Test: 연계 초식의 조건을 정의할 수 있다
- [ ] Test: 카드 사용 순서를 추적할 수 있다
- [ ] Test: 특정 순서로 카드를 사용하면 연계가 발동한다
- [ ] Test: 연계 발동 시 추가 효과가 적용된다

## Phase 6: 게임 관리 시스템 (Game Manager)

### 6.1 게임 상태 관리
- [ ] Test: GameManager를 싱글톤으로 생성할 수 있다
- [ ] Test: 게임 상태(MainMenu, MapView, Battle 등)를 변경할 수 있다
- [ ] Test: 새 게임을 시작할 수 있다
- [ ] Test: 선택한 문파로 Player를 초기화한다

### 6.2 세이브/로드
- [ ] Test: 게임 진행 상황을 저장할 수 있다
- [ ] Test: 저장된 게임을 불러올 수 있다
- [ ] Test: 전투 종료 시 자동 저장된다

## Phase 7: 맵 시스템 (Map Manager)

### 7.1 맵 생성
- [ ] Test: MapManager를 생성할 수 있다
- [ ] Test: 절차적으로 맵 노드를 생성할 수 있다
- [ ] Test: 노드 간 연결 경로를 생성할 수 있다
- [ ] Test: 각 층마다 적절한 비율로 노드 타입이 배치된다

### 7.2 노드 상호작용
- [ ] Test: Player가 노드를 선택할 수 있다
- [ ] Test: 노드 선택 시 해당 노드 타입의 이벤트가 발생한다
- [ ] Test: 전투 노드 선택 시 전투가 시작된다
- [ ] Test: 휴식처 노드에서 체력 회복/카드 강화/경지 돌파를 선택할 수 있다
- [ ] Test: 상점 노드에서 카드를 구매할 수 있다
- [ ] Test: 기연 노드에서 이벤트가 발생한다

## Phase 8: 유물 시스템

### 8.1 유물 획득과 효과
- [ ] Test: Player가 유물을 획득할 수 있다
- [ ] Test: 전투 시작 시 발동하는 유물이 작동한다
- [ ] Test: 턴 시작 시 발동하는 유물이 작동한다
- [ ] Test: 카드 사용 시 발동하는 유물이 작동한다
- [ ] Test: 여러 유물의 효과가 동시에 적용된다

## Phase 9: 이벤트 시스템

### 9.1 기연 이벤트
- [ ] Test: EventData로 이벤트를 정의할 수 있다
- [ ] Test: 이벤트 선택지를 표시할 수 있다
- [ ] Test: 선택지 선택 시 결과가 적용된다
- [ ] Test: 확률적 결과가 있는 이벤트가 작동한다

## Phase 10: UI 시스템

### 10.1 전투 UI
- [ ] Test: Player의 체력/내공을 UI에 표시한다
- [ ] Test: Enemy의 체력/Intent를 UI에 표시한다
- [ ] Test: 핸드의 카드를 UI에 표시한다
- [ ] Test: 카드 호버 시 툴팁이 표시된다
- [ ] Test: 상태이상 아이콘이 표시된다

### 10.2 맵 UI
- [ ] Test: 맵 노드가 UI에 표시된다
- [ ] Test: 현재 위치가 표시된다
- [ ] Test: 이동 가능한 노드가 강조 표시된다

## Phase 11: 메타 시스템

### 11.1 깨달음 시스템
- [ ] Test: 게임 종료 시 깨달음 포인트를 획득한다
- [ ] Test: 깨달음 포인트로 영구 강화를 구매할 수 있다
- [ ] Test: 영구 강화가 새 게임에 적용된다

## Phase 12: 심마 전투 시스템

### 12.1 폐관수련과 심마
- [ ] Test: 휴식처에서 폐관수련을 선택할 수 있다
- [ ] Test: 폐관수련 선택 시 심마와의 전투가 시작된다
- [ ] Test: 심마 전투에서 승리하면 경지가 상승한다
- [ ] Test: 심마 전투에서 패배하면 주화입마 상태가 된다

## Phase 13: 문파별 특수 메카닉

### 13.1 화산파 연계 시스템
- [ ] Test: 화산파 전용 연계 초식이 작동한다
- [ ] Test: 연계 콤보 카운터가 작동한다

### 13.2 천마신교 체력 소모 시스템
- [ ] Test: 체력을 대가로 하는 카드가 작동한다
- [ ] Test: 천마 상태가 올바르게 적용된다

### 13.3 사천당문 중독 시스템
- [ ] Test: 중독 중첩이 올바르게 작동한다
- [ ] Test: 중독 시너지 카드가 작동한다

### 13.4 하북팽가 무방비/힘 시스템
- [ ] Test: 무방비 상태가 올바르게 적용된다
- [ ] Test: 무방비와 힘의 시너지가 작동한다

## Phase 14: 최종 통합 테스트

### 14.1 전체 게임 플로우
- [ ] Test: 메인 메뉴에서 게임을 시작할 수 있다
- [ ] Test: 문파를 선택하고 게임을 시작할 수 있다
- [ ] Test: 맵을 탐험하며 전투를 진행할 수 있다
- [ ] Test: 3층 보스를 격파하면 게임에서 승리한다
- [ ] Test: 사망 시 게임 오버가 된다
- [ ] Test: 게임 종료 후 메타 진행이 저장된다

---

## 실행 지침

1. "go" 명령 시:
   - 위 목록에서 다음 미완료 테스트를 찾는다
   - 해당 테스트를 먼저 작성한다 (Red)
   - 테스트를 통과시킬 최소한의 코드를 작성한다 (Green)
   - 필요시 리팩토링한다 (Refactor)
   - 테스트 완료 표시: [ ] → [x]

2. 각 테스트마다:
   - 명확한 테스트 이름 사용
   - 한 가지 동작만 검증
   - Arrange-Act-Assert 패턴 준수

3. 커밋 규칙:
   - 테스트 추가: "test: [기능] 테스트 추가"
   - 구현: "feat: [기능] 구현"
   - 리팩토링: "refactor: [내용]"
