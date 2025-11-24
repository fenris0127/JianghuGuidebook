# JianghuGuidebook (강호무적)

플레이어는 이름 없는 낭인으로 시작하여, 매번 변화하는 강호를 유랑하며 자신만의 무공 덱을 구축하고, '내공'과 '무기술'의 경지를 돌파하여 강호의 패업을 이루는 게임

## 프로젝트 정보

**장르:** 무협 덱빌딩 로그라이크
**게임 엔진:** Unity (Universal Render Pipeline)
**개발 단계:** Phase 1 - 프로토타입
**타겟 플랫폼:** PC (Windows)

## 프로젝트 설정

### 요구사항
- Unity 2021.3 LTS 이상
- Visual Studio 2022 또는 Rider

### 프로젝트 구조
```
Assets/
├── Data/              # JSON 데이터 파일 (카드, 적, 이벤트)
├── Materials/         # 머티리얼 및 셰이더
├── Prefabs/          # UI 및 게임 오브젝트 프리팹
├── Scenes/           # 게임 씬
├── Scripts/          # C# 스크립트
│   ├── AI/          # 적 AI 및 행동 패턴
│   ├── Cards/       # 카드 시스템
│   ├── Combat/      # 전투 시스템
│   ├── Core/        # 핵심 매니저 및 시스템
│   ├── Data/        # 데이터 구조 및 관리
│   └── UI/          # UI 컴포넌트
├── ScriptableObjects/ # ScriptableObject 에셋
├── Sprites/          # 스프라이트 및 이미지
└── Tests/           # 유닛 및 통합 테스트
    ├── EditMode/    # Edit Mode 테스트
    └── PlayMode/    # Play Mode 테스트
```

### 설치 및 실행
1. 이 저장소를 클론합니다
2. Unity Hub에서 프로젝트를 엽니다
3. Scenes/MainScene.unity를 엽니다
4. Play 버튼을 눌러 게임을 실행합니다

## 개발 현황

현재 Phase 1 프로토타입 개발 중입니다. 자세한 작업 목록은 `tasks/tasks-murim-deckbuilder-prototype-KR.md`를 참조하세요.

### Phase 1 목표
- 플레이 가능한 턴제 전투 시스템
- 20장의 기능적 카드
- 3가지 적 타입
- 기본 전투 UI
- 완전한 단일 전투 루프
