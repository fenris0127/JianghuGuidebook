# CombatScene 설정 가이드

이 가이드는 Unity 에디터에서 CombatScene을 설정하는 방법을 설명합니다.

## 씬 설정 단계

### 1. 새 씬 생성
1. Unity 에디터에서 `File > New Scene` 선택
2. "CombatScene"으로 저장 (`Assets/Scenes/CombatScene.unity`)

### 2. 필수 GameObject 추가

#### GameManager GameObject
1. Hierarchy에서 `우클릭 > Create Empty`
2. 이름을 "GameManager"로 변경
3. `GameManager.cs` 컴포넌트 추가
   - Inspector에서 `Add Component` 클릭
   - "GameManager" 검색 후 추가

#### CombatManager GameObject
1. Hierarchy에서 `우클릭 > Create Empty`
2. 이름을 "CombatManager"로 변경
3. `CombatManager.cs` 컴포넌트 추가
   - Inspector에서 `Add Component` 클릭
   - "CombatManager" 검색 후 추가

### 3. 테스트 실행
1. Play 버튼 클릭
2. Console 창에서 다음 로그 확인:
   ```
   GameManager 초기화 완료
   === 전투 시작 ===
   전투 초기화 중...
   전투 상태 변경: None -> Initializing
   --- 플레이어 턴 시작 ---
   전투 상태 변경: Initializing -> PlayerTurn
   ```

### 4. 상태 전환 확인
- Console에서 플레이어 턴과 적 턴이 반복되는지 확인
- CombatManager의 Inspector에서 "Current State" 필드가 변경되는지 확인

## 다음 단계
씬이 정상적으로 동작하면 Task 1.5와 1.7이 완료됩니다.
이후 카드 시스템과 전투 로직을 추가할 예정입니다.
