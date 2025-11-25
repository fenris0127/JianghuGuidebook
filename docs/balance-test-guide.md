# 밸런스 테스트 시스템 사용 가이드

## 개요

이 가이드는 강호무적 프로토타입의 밸런스 테스트 도구 사용법을 설명합니다.

---

## 1. 밸런스 테스트 시뮬레이터 (BalanceTestSimulator)

### 1.1 설정 방법

1. Unity 씬에 빈 GameObject 생성
2. `BalanceTestSimulator.cs` 컴포넌트 추가
3. Inspector에서 설정:
   - **Simulations Per Enemy**: 100 (각 적당 시뮬레이션 횟수)
   - **Max Turns Per Simulation**: 50 (최대 턴 수)
   - **Player Starting Health**: 70
   - **Player Max Energy**: 3
   - **Cards Drawn Per Turn**: 5

### 1.2 테스트 실행

#### 방법 1: 에디터 컨텍스트 메뉴
1. Inspector에서 `BalanceTestSimulator` 우클릭
2. "Run Full Balance Test" 선택
3. Console에서 결과 확인

#### 방법 2: 코드로 실행
```csharp
var simulator = FindObjectOfType<BalanceTestSimulator>();
simulator.RunFullBalanceTest();
```

### 1.3 결과 해석

시뮬레이터는 다음 정보를 출력합니다:

```
=== BALANCE TEST RESULTS ===

--- 산적 (enemy_bandit) ---
Total Simulations: 100
Player Wins: 75 (75.0%)
Player Losses: 25 (25.0%)
Average Turns to Win: 7
Average Health Remaining: 45 HP
Health Range: 20 - 65 HP
Balance Assessment: WELL BALANCED - Good difficulty

--- 마도수련자 (enemy_dark_cultivator) ---
...
```

#### 밸런스 평가 기준:
- **70%+ 승률**: TOO EASY - 적을 강화하거나 더 좋은 카드 필요
- **50-70% 승률**: WELL BALANCED - 적절한 난이도
- **30-50% 승률**: CHALLENGING - 약간 어렵지만 공정함
- **30% 미만 승률**: TOO HARD - 적을 약화하거나 플레이어 강화 필요

---

## 2. 전투 테스트 씬 설정 (CombatTestSceneSetup)

### 2.1 설정 방법

1. Unity 씬에 빈 GameObject 생성
2. `CombatTestSceneSetup.cs` 컴포넌트 추가
3. Inspector에서 설정:
   - **Enemy Id To Test**: "enemy_bandit" (테스트할 적 ID)
   - **Auto Start**: true (씬 시작 시 자동 설정)
   - **Enable Debug Mode**: true (디버그 로그 활성화)

### 2.2 테스트 방법

#### 빠른 테스트 (컨텍스트 메뉴)
1. Inspector에서 `CombatTestSceneSetup` 우클릭
2. 다음 중 하나 선택:
   - "Test: Bandit"
   - "Test: Dark Cultivator"
   - "Test: Armored Guard"

#### 전투 상태 확인
1. Inspector에서 우클릭
2. "Print Combat Status" 선택
3. Console에서 현재 상태 확인:
```
[Player] HP: 65/70, Energy: 2/3, Block: 5
[Enemy] 산적 HP: 28/40, Block: 0, Intent: Attack (6)
```

### 2.3 커스텀 적 테스트

```csharp
var testSetup = FindObjectOfType<CombatTestSceneSetup>();
testSetup.enemyIdToTest = "your_custom_enemy_id";
testSetup.SetupTestCombat();
```

---

## 3. 수동 밸런스 테스트 체크리스트

### 3.1 각 적 타입별 테스트

**산적 (Bandit) 테스트:**
- [ ] 5-10회 전투 플레이
- [ ] 예상 전투 시간: 5-8턴
- [ ] 예상 승률: 70-80%
- [ ] 체력 남은 상태 체크
- [ ] 너무 쉽다/어렵다 느낌 체크

**마도수련자 (Dark Cultivator) 테스트:**
- [ ] 5-10회 전투 플레이
- [ ] 버프 메커니즘 확인 (구현 시)
- [ ] 예상 전투 시간: 4-6턴
- [ ] 예상 승률: 55-70%
- [ ] 높은 피해량 위협 느껴지는지 체크

**철갑위병 (Armored Guard) 테스트:**
- [ ] 5-10회 전투 플레이
- [ ] 방어 메커니즘 효과 확인
- [ ] 예상 전투 시간: 8-12턴
- [ ] 예상 승률: 45-60%
- [ ] 탱커 느낌이 나는지 체크

### 3.2 엣지 케이스 테스트

- [ ] 플레이어가 첫 턴에 모든 방어 카드만 뽑는 경우
- [ ] 플레이어가 첫 턴에 모든 공격 카드만 뽑는 경우
- [ ] 적이 연속으로 강 공격을 사용하는 경우
- [ ] 적이 연속으로 방어만 하는 경우
- [ ] 긴 전투 (15턴 이상) 시나리오

---

## 4. 밸런스 조정 가이드

### 4.1 적이 너무 약한 경우

**증상:**
- 승률 80% 이상
- 전투가 너무 빨리 끝남 (5턴 미만)
- 위협감이 없음

**해결책:**
1. 체력 +10 HP 증가
2. 피해량 +1-2 증가
3. 방어 행동 가중치 증가
4. 강력한 행동 추가

**예시 (산적 버프):**
```json
{
  "maxHealth": 50,  // 40 → 50
  "actions": [
    { "type": 0, "value": 8, "weight": 60 },  // 6 → 8
    { "type": 1, "value": 8, "weight": 40 }
  ]
}
```

### 4.2 적이 너무 강한 경우

**증상:**
- 승률 30% 미만
- 플레이어가 빠르게 사망
- 불공정하다고 느껴짐

**해결책:**
1. 체력 -10 HP 감소
2. 피해량 -1-2 감소
3. 공격 행동 가중치 감소
4. 플레이어 시작 체력 증가

**예시 (철갑위병 너프):**
```json
{
  "maxHealth": 45,  // 50 → 45
  "actions": [
    { "type": 0, "value": 4, "weight": 40 },  // 5 → 4
    { "type": 1, "value": 12, "weight": 40 },
    { "type": 0, "value": 12, "weight": 20 }  // 15 → 12
  ]
}
```

### 4.3 전투가 지루한 경우

**증상:**
- 전투가 너무 길음 (15턴 이상)
- 패턴이 단조로움
- 전략성이 부족함

**해결책:**
1. 피해량 증가 (양측 모두)
2. 다양한 행동 패턴 추가
3. 특수 능력 추가
4. 조건부 행동 패턴 구현

---

## 5. 데이터 수집 및 분석

### 5.1 시뮬레이션 데이터

시뮬레이터를 실행하고 다음 데이터를 기록:

| 적 타입 | 승률 | 평균 턴 (승리) | 평균 체력 (승리) | 평가 |
|---------|------|---------------|----------------|------|
| 산적 | 75% | 7 | 45 HP | 양호 |
| 마도수련자 | 62% | 5 | 38 HP | 양호 |
| 철갑위병 | 48% | 11 | 28 HP | 양호 |

### 5.2 플레이테스트 피드백 양식

```markdown
## 플레이테스트 피드백

**테스터**: [이름]
**날짜**: [날짜]
**적 타입**: [산적/마도수련자/철갑위병]

### 전투 결과
- 전투 횟수: 10
- 승리: 7
- 패배: 3
- 평균 턴 수: 8

### 난이도 평가 (1-5)
- 너무 쉬움 (1) - 적절함 (3) - 너무 어려움 (5): [ ]

### 재미 평가 (1-5)
- 지루함 (1) - 보통 (3) - 재미있음 (5): [ ]

### 코멘트
- [자유 의견]
```

---

## 6. 자동화된 테스트 스크립트

### 6.1 Unity Test Runner로 통합

```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BalanceTests
{
    [Test]
    public void Bandit_WinRate_ShouldBe_70_to_80_Percent()
    {
        var simulator = new BalanceTestSimulator();
        var result = simulator.SimulateEnemyEncounter("enemy_bandit", 100);

        Assert.IsTrue(result.winRate >= 70f && result.winRate <= 80f,
            $"Bandit win rate is {result.winRate}%, expected 70-80%");
    }

    [Test]
    public void DarkCultivator_WinRate_ShouldBe_55_to_70_Percent()
    {
        var simulator = new BalanceTestSimulator();
        var result = simulator.SimulateEnemyEncounter("enemy_dark_cultivator", 100);

        Assert.IsTrue(result.winRate >= 55f && result.winRate <= 70f,
            $"Dark Cultivator win rate is {result.winRate}%, expected 55-70%");
    }
}
```

---

## 7. 문제 해결

### 7.1 시뮬레이터가 작동하지 않음

**원인:**
- DataManager가 초기화되지 않음
- EnemyDatabase.json이 없거나 손상됨

**해결:**
1. DataManager가 씬에 있는지 확인
2. Resources/EnemyDatabase.json 파일 존재 확인
3. JSON 문법 오류 확인

### 7.2 승률이 예상과 크게 다름

**원인:**
- AI 로직이 최적이 아님
- 카드 드로우가 랜덤이 아님
- 시뮬레이션 횟수가 너무 적음

**해결:**
1. 시뮬레이션 횟수를 100 이상으로 증가
2. AI 로직 개선
3. 랜덤 시드 확인

---

## 8. 다음 단계

밸런스 테스트 완료 후:

1. ✅ 시뮬레이션 데이터 수집
2. ✅ 수동 플레이테스트 진행
3. ✅ 피드백 분석
4. ✅ 필요한 조정 적용
5. ✅ 재테스트
6. ✅ 문서 업데이트 (balance-analysis-phase1.md)
7. → Phase 2로 진행

---

**작성**: Phase 1 Prototype
**마지막 업데이트**: 2024
