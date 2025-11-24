# 강호무적 - 완전 개발 가이드 (올인원)

> **무협 덱 빌딩 로그라이크 게임 제작 종합 가이드**
> 초보 개발자도 따라할 수 있는 A to Z 완전판

**버전:** 1.0
**최종 수정일:** 2025-11-22
**예상 독서 시간:** 3-4시간
**난이도:** 초급 ~ 중급

---

## 📖 목차

### PART 1: 시작하기
- [Chapter 1: 빠른 시작 가이드 (30분)](#chapter-1-빠른-시작-가이드-30분)
- [Chapter 2: 프로젝트 개요 및 목표](#chapter-2-프로젝트-개요-및-목표)
- [Chapter 3: 개발 환경 준비](#chapter-3-개발-환경-준비)

### PART 2: 기술 스택 및 아키텍처
- [Chapter 4: 기술 스택 선택 가이드](#chapter-4-기술-스택-선택-가이드)
- [Chapter 5: 아키텍처 설계](#chapter-5-아키텍처-설계)
- [Chapter 6: 핵심 디자인 패턴](#chapter-6-핵심-디자인-패턴)

### PART 3: Unity 개발
- [Chapter 7: Unity 설치 및 프로젝트 생성](#chapter-7-unity-설치-및-프로젝트-생성)
- [Chapter 8: Unity 프로젝트 구조](#chapter-8-unity-프로젝트-구조)
- [Chapter 9: Unity 핵심 개념](#chapter-9-unity-핵심-개념)

### PART 4: Git 및 버전 관리
- [Chapter 10: Git 설치 및 기본 설정](#chapter-10-git-설치-및-기본-설정)
- [Chapter 11: Git 워크플로우](#chapter-11-git-워크플로우)
- [Chapter 12: 협업 전략](#chapter-12-협업-전략)

### PART 5: 카드 게임 제작
- [Chapter 13: 덱 빌더 장르 분석](#chapter-13-덱-빌더-장르-분석)
- [Chapter 14: 게임 메커니즘 설계](#chapter-14-게임-메커니즘-설계)
- [Chapter 15: 밸런싱 방법론](#chapter-15-밸런싱-방법론)

### PART 6: 데이터 관리
- [Chapter 16: JSON 데이터 구조 설계](#chapter-16-json-데이터-구조-설계)
- [Chapter 17: Google Sheets 연동](#chapter-17-google-sheets-연동)
- [Chapter 18: 데이터 검증 및 테스트](#chapter-18-데이터-검증-및-테스트)

### PART 7: UI/UX 디자인
- [Chapter 19: 카드 UI 설계](#chapter-19-카드-ui-설계)
- [Chapter 20: 무협 테마 비주얼 디자인](#chapter-20-무협-테마-비주얼-디자인)
- [Chapter 21: 애니메이션 및 피드백](#chapter-21-애니메이션-및-피드백)

### PART 8: 테스트 및 디버깅
- [Chapter 22: Unity Test Framework](#chapter-22-unity-test-framework)
- [Chapter 23: 디버깅 기법](#chapter-23-디버깅-기법)
- [Chapter 24: 성능 최적화](#chapter-24-성능-최적화)

### PART 9: 빌드 및 배포
- [Chapter 25: 빌드 설정](#chapter-25-빌드-설정)
- [Chapter 26: Steam 준비](#chapter-26-steam-준비)
- [Chapter 27: itch.io 배포](#chapter-27-itchio-배포)

### PART 10: 주니어 개발자 가이드
- [Chapter 28: 학습 로드맵](#chapter-28-학습-로드맵)
- [Chapter 29: 문제 해결 프로세스](#chapter-29-문제-해결-프로세스)
- [Chapter 30: 커리어 개발](#chapter-30-커리어-개발)

### 부록
- [부록 A: 완전한 코드 예시](#부록-a-완전한-코드-예시)
- [부록 B: 추천 리소스](#부록-b-추천-리소스)
- [부록 C: FAQ](#부록-c-faq)
- [부록 D: 용어 사전](#부록-d-용어-사전)

---

# PART 1: 시작하기

## Chapter 1: 빠른 시작 가이드 (30분)

> "지금 당장 시작하고 싶다면 이 챕터만 따라하세요!"

### 1.1 목표

30분 내에 Unity 프로젝트를 생성하고 첫 번째 씬을 실행하는 것.

---

### 1.2 사전 준비물

✅ **필수:**
- Windows 10/11 또는 macOS 10.15+
- 최소 10GB 여유 공간
- 인터넷 연결

✅ **권장:**
- SSD (빠른 빌드)
- 8GB+ RAM
- 듀얼 모니터 (에디터 + 문서)

---

### 1.3 30분 체크리스트

#### **0-10분: Unity Hub 설치**

1. **Unity Hub 다운로드**
   - [https://unity.com/download](https://unity.com/download) 접속
   - "Download Unity Hub" 클릭
   - 설치 파일 실행

2. **Unity 계정 생성**
   - Unity Hub 실행
   - "Create account" 클릭
   - 무료 Personal 라이선스 선택

3. **Unity 2022.3 LTS 설치**
   - Unity Hub → Installs → Install Editor
   - 버전: **2022.3 LTS** 선택
   - 모듈 선택:
     - ✅ Visual Studio Community (또는 이미 있으면 체크 해제)
     - ✅ Windows Build Support (Windows 사용자)
     - ✅ Mac Build Support (Mac 사용자)
   - "Install" 클릭 (약 5-7분 소요)

---

#### **10-15분: 프로젝트 생성**

1. **새 프로젝트 생성**
   - Unity Hub → Projects → New Project
   - Template: **2D (URP)** 선택
   - Project name: `MurimDeckbuilder`
   - Location: 원하는 경로 (예: `C:\Projects\` 또는 `~/Projects/`)
   - "Create project" 클릭

2. **Unity 에디터 로딩 대기**
   - 처음 실행 시 1-2분 소요

---

#### **15-20분: 기본 설정**

1. **Layout 설정**
   - 우측 상단: Window → Layouts → **2 by 3**
   - 또는 자신에게 편한 레이아웃 선택

2. **Package Manager 열기**
   - Window → Package Manager
   - 왼쪽 상단: Packages: **Unity Registry** 선택
   - 검색: "TextMeshPro" → Install
   - 검색: "Input System" → Install (선택적)

3. **첫 씬 저장**
   - File → Save As
   - 이름: `TestScene`
   - 위치: `Assets/Scenes/` (폴더 생성)

---

#### **20-25분: 첫 GameObject 생성**

1. **Canvas 생성**
   - Hierarchy 우클릭 → UI → Canvas
   - Canvas가 생성되고 EventSystem도 자동 생성됨

2. **Text 추가**
   - Canvas 우클릭 → UI → Text - TextMeshPro
   - "TMP Importer" 창이 뜨면 "Import TMP Essentials" 클릭
   - Inspector에서 Text 내용: **"강호무적 - 개발 시작!"**
   - Font Size: 48
   - Alignment: 중앙 정렬

3. **Button 추가**
   - Canvas 우클릭 → UI → Button - TextMeshPro
   - 버튼 위치 조정 (Text 아래로)
   - 버튼 내 Text: "시작하기"

---

#### **25-30분: 첫 스크립트 작성**

1. **Scripts 폴더 생성**
   - Project 창 → Assets 우클릭 → Create → Folder
   - 이름: `Scripts`

2. **스크립트 생성**
   - Scripts 폴더 우클릭 → Create → C# Script
   - 이름: `HelloWorld`

3. **코드 작성**
   ```csharp
   using UnityEngine;

   public class HelloWorld : MonoBehaviour
   {
       void Start()
       {
           Debug.Log("강호무적 개발 시작!");
       }
   }
   ```

4. **스크립트 연결**
   - HelloWorld 스크립트를 Canvas에 드래그 앤 드롭

5. **실행!**
   - 상단 재생 버튼 (▶️) 클릭
   - Console 창 (Window → General → Console)에서 메시지 확인

---

### 1.4 축하합니다! 🎉

Unity 프로젝트 생성 완료! 이제 본격적인 개발을 시작할 준비가 되었습니다.

**다음 단계:**
- Chapter 7으로 이동하여 자세한 Unity 사용법 학습
- 또는 Chapter 2로 이동하여 프로젝트 전체 개요 파악

---

## Chapter 2: 프로젝트 개요 및 목표

### 2.1 게임 컨셉

**강호무적**은 무협 세계관 덱 빌딩 로그라이크 게임입니다.

**핵심 장르:**
- 덱 빌딩 (Deckbuilding)
- 로그라이크 (Roguelike)
- 턴제 전투 (Turn-based Combat)

**주요 영감:**
- Slay the Spire (메커니즘)
- Monster Train (전략적 깊이)
- 김용/고룡 무협 소설 (세계관)

---

### 2.2 게임 핵심 루프

```
게임 시작
    ↓
시작 분파 선택 (예: 화산파 - 검술 특화)
    ↓
맵 탐험 (노드 선택)
    ↓
┌─────────────────────────────┐
│  노드 타입별 이벤트 발생    │
│  - 전투: 적과 싸워 승리     │
│  - 상점: 카드/유물 구매     │
│  - 휴식: 체력 회복/카드 강화│
│  - 이벤트: 선택지 이벤트    │
│  - 보스: 강력한 적          │
└─────────────────────────────┘
    ↓
보상 획득 (카드, 유물, 골드)
    ↓
덱 강화 (카드 추가/제거/업그레이드)
    ↓
다음 노드로 이동 (반복)
    ↓
보스 격파 → 지역 클리어
    ↓
다음 지역 또는 게임 클리어
    ↓
사망 시: 무공 정수 획득 → 영구 업그레이드 → 재시작
```

---

### 2.3 개발 Phase 구조

#### **Phase 1: 프로토타입** (1-2개월)
**목표:** 핵심 전투 시스템 검증

- ✅ 턴제 전투
- ✅ 20장 카드
- ✅ 3종 적
- ✅ 기본 UI
- ✅ 승리/패배

**완료 기준:** "재미있다!" 피드백

---

#### **Phase 2: 수직 슬라이스** (2-3개월)
**목표:** 완전한 1개 지역 경험

- ✅ 맵 시스템
- ✅ 50장 카드
- ✅ 20개 유물
- ✅ 상점/휴식/이벤트
- ✅ 보스 전투
- ✅ 메타 진행 (무공 정수)

**완료 기준:** 1개 지역 클리어 가능

---

#### **Phase 3: 콘텐츠 확장** (3-4개월)
**목표:** 전체 5개 지역

- ✅ 5개 지역 및 보스
- ✅ 120장 카드
- ✅ 60개 유물
- ✅ 내공/무기술 경지 시스템
- ✅ 5개 시작 분파

**완료 기준:** 게임 클리어 가능

---

#### **Phase 4: 폴리싱** (2-3개월)
**목표:** 출시 준비

- ✅ 최종 아트
- ✅ 사운드/음악
- ✅ 밸런싱
- ✅ 버그 수정
- ✅ 다국어 (영어)

**완료 기준:** Steam 출시 준비 완료

---

### 2.4 성공 지표

**기술적 목표:**
- 60 FPS 유지
- 로딩 시간 2초 이내
- 치명적 버그 0개
- 세이브/로드 100% 안정

**게임플레이 목표:**
- 1회차 플레이 시간: 2-3시간
- 클리어율: 15-25%
- 리플레이 횟수: 평균 10회 이상

**비즈니스 목표:**
- Steam 긍정 리뷰 80% 이상
- 첫 3개월 10,000장 판매
- 평균 플레이 시간 20시간 이상

---

### 2.5 타겟 플랫폼

**1순위: PC (Windows/Mac/Linux)**
- Steam 출시
- 키보드/마우스 조작

**2순위: 모바일** (Phase 5+)
- iOS/Android
- 터치 조작 최적화

---

## Chapter 3: 개발 환경 준비

### 3.1 하드웨어 요구사항

#### **최소 사양 (개발 가능)**
```
CPU:     Intel i5 / AMD Ryzen 5
RAM:     8GB
GPU:     통합 그래픽 (Intel HD 4000+)
저장:    HDD 10GB 여유
```

#### **권장 사양 (쾌적한 개발)**
```
CPU:     Intel i7 / AMD Ryzen 7
RAM:     16GB
GPU:     GTX 1060 / RX 580 이상
저장:    SSD 20GB+ 여유
모니터:  듀얼 모니터 (또는 24인치 이상)
```

---

### 3.2 소프트웨어 체크리스트

#### ✅ **필수 소프트웨어**

1. **Unity Hub + Unity 2022.3 LTS**
   - [https://unity.com/download](https://unity.com/download)
   - 라이선스: Personal (무료)

2. **Visual Studio 2022 Community** 또는 **JetBrains Rider**
   - VS: Unity 설치 시 자동 설치 가능
   - Rider: [https://www.jetbrains.com/rider/](https://www.jetbrains.com/rider/) (학생 무료)

3. **Git**
   - Windows: [https://git-scm.com/download/win](https://git-scm.com/download/win)
   - Mac: `brew install git` (Homebrew 사용 시)

4. **GitHub Desktop** (선택적, Git 초보자용)
   - [https://desktop.github.com/](https://desktop.github.com/)

---

#### 🎨 **아트 도구** (선택적)

1. **Photoshop** 또는 **Affinity Photo**
   - 카드 일러스트, UI 디자인

2. **Aseprite** (픽셀 아트)
   - [https://www.aseprite.org/](https://www.aseprite.org/)
   - 스프라이트 애니메이션

3. **Figma** (UI/UX 디자인)
   - [https://www.figma.com/](https://www.figma.com/)
   - 무료, 웹 기반

---

#### 🎵 **사운드 도구** (선택적)

1. **Audacity** (효과음 편집)
   - [https://www.audacityteam.org/](https://www.audacityteam.org/)
   - 무료 오픈소스

2. **BFXR** (8-bit 효과음 생성)
   - [https://www.bfxr.net/](https://www.bfxr.net/)
   - 브라우저 기반

---

### 3.3 계정 준비

#### 1️⃣ **Unity ID**
- [https://id.unity.com/](https://id.unity.com/) 가입
- Personal 라이선스 활성화

#### 2️⃣ **GitHub 계정**
- [https://github.com/](https://github.com/) 가입
- 레포지토리 생성 준비

#### 3️⃣ **Steam Developer 계정** (Phase 4에서 필요)
- [https://partner.steamgames.com/](https://partner.steamgames.com/)
- 등록비: $100 (나중에 준비)

#### 4️⃣ **itch.io 계정** (무료 배포용)
- [https://itch.io/](https://itch.io/)
- 무료, 즉시 배포 가능

---

### 3.4 폴더 구조 준비

개발 시작 전 추천 폴더 구조:

```
C:\Projects\                    (Windows)
~/Projects/                     (Mac/Linux)
    └── MurimDeckbuilder\
        ├── Game\              ← Unity 프로젝트
        ├── Art\               ← PSD, AI 원본 파일
        │   ├── Cards\
        │   ├── UI\
        │   └── Characters\
        ├── Audio\             ← 음원 원본 파일
        │   ├── BGM\
        │   └── SFX\
        ├── Design\            ← 기획 문서, 스프레드시트
        │   ├── PRD\
        │   └── Balancing\
        └── Build\             ← 빌드 출력물
            ├── Windows\
            └── Mac\
```

---

### 3.5 학습 자료 준비

#### 📚 **북마크 추천 사이트**

**Unity 공식:**
- [Unity Learn](https://learn.unity.com/)
- [Unity Manual](https://docs.unity3d.com/Manual/)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)

**커뮤니티:**
- [Unity Korea Facebook](https://www.facebook.com/groups/unitykorea/)
- [Reddit r/Unity3D](https://www.reddit.com/r/Unity3D/)
- [Stack Overflow - Unity Tag](https://stackoverflow.com/questions/tagged/unity3d)

**YouTube 채널:**
- **Brackeys** (Unity 기초, 영어)
- **Code Monkey** (게임 패턴, 영어)
- **골드메탈** (Unity 한글 강의)
- **Infallible Code** (고급 패턴, 영어)

---

### 3.6 체크리스트

다음 챕터로 넘어가기 전 확인:

- [ ] Unity Hub 설치 완료
- [ ] Unity 2022.3 LTS 설치 완료
- [ ] Visual Studio 또는 Rider 설치 완료
- [ ] Git 설치 및 `git --version` 명령 작동 확인
- [ ] GitHub 계정 생성
- [ ] 프로젝트 폴더 구조 생성
- [ ] Unity 첫 프로젝트 생성 및 실행 성공

✅ 모두 체크했다면 **PART 2**로 이동!

---

# PART 2: 기술 스택 및 아키텍처

## Chapter 4: 기술 스택 선택 가이드

### 4.1 게임 엔진 선택: Unity vs Godot

이 프로젝트에서 **Unity**를 권장하는 이유를 상세히 설명합니다.

---

#### 4.1.1 Unity

**✅ 장점:**

1. **방대한 학습 자료**
   - YouTube 튜토리얼 수십만 개
   - Udemy, Coursera 강의 수백 개
   - Unity Learn 공식 무료 강의

2. **에셋 스토어**
   - UI 템플릿 (카드 UI 키트 등)
   - 사운드 에셋
   - 파티클 이펙트
   - 시간 절약 가능

3. **강력한 2D 도구**
   - Sprite Atlas (텍스처 최적화)
   - Tilemap (맵 제작)
   - Animator (애니메이션)
   - Cinemachine (카메라)

4. **커뮤니티 규모**
   - Stack Overflow 질문 30만+ 개
   - Unity Forum 활성 사용자 수백만
   - Discord 서버 다수

5. **취업 시장 가치**
   - 게임 회사 90% 이상이 Unity 사용
   - 포트폴리오로 유리

6. **크로스 플랫폼**
   - 클릭 몇 번으로 Windows/Mac/Linux/모바일/WebGL 빌드

**❌ 단점:**

1. **라이선스 비용**
   - 수익 $200k/년 이상 시 유료 ($2,040/년)
   - 개인 개발자는 대부분 무료 범위

2. **엔진 무게**
   - 다운로드: 2-3GB
   - 프로젝트 크기: 500MB+
   - 빌드 시간: 느린 편

3. **Runtime Fee 논란**
   - 2023년 설치당 과금 발표 → 커뮤니티 반발 → 정책 완화
   - 현재는 수익 기반 과금으로 변경

---

#### 4.1.2 Godot

**✅ 장점:**

1. **완전 무료 오픈소스**
   - MIT 라이선스
   - 수익 제한 없음
   - 상업 프로젝트 자유 사용

2. **가벼움**
   - 엔진 크기: 50MB 미만
   - 프로젝트 크기: 작음
   - 빌드 빠름

3. **GDScript**
   - Python과 유사한 간결한 문법
   - 배우기 쉬움

4. **노드 기반 씬 시스템**
   - 직관적인 계층 구조
   - 재사용 용이

**❌ 단점:**

1. **학습 자료 부족**
   - Unity 대비 1/10 수준
   - 한글 자료 매우 적음

2. **에셋 스토어 부족**
   - 상용 에셋 거의 없음
   - 대부분 직접 제작 필요

3. **커뮤니티 규모**
   - Unity 대비 작음
   - 문제 발생 시 해결 느림

4. **C# 지원 제한적**
   - GDScript가 주력
   - C#은 2순위 시민

5. **3D 성능**
   - Unity/Unreal보다 약함
   - 2D는 괜찮음

---

#### 4.1.3 최종 결정: Unity ⭐

**이 프로젝트에 Unity를 선택한 이유:**

| 기준 | Unity | Godot | 승자 |
|------|-------|-------|------|
| 학습 자료 | ⭐⭐⭐⭐⭐ | ⭐⭐ | Unity |
| 초보 친화성 | ⭐⭐⭐⭐ | ⭐⭐⭐ | Unity |
| 2D 도구 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | Unity |
| 에셋 스토어 | ⭐⭐⭐⭐⭐ | ⭐ | Unity |
| 커뮤니티 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | Unity |
| 비용 | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | Godot |
| 가벼움 | ⭐⭐ | ⭐⭐⭐⭐⭐ | Godot |
| 취업 가치 | ⭐⭐⭐⭐⭐ | ⭐⭐ | Unity |

**결론:** 주니어 개발자에게는 **Unity가 압도적으로 유리**

---

### 4.2 프로그래밍 언어: C#

**왜 C#인가?**

1. **타입 안정성**
   - 컴파일 타임 에러 검출
   - IDE 자동 완성 강력

2. **Unity 공식 언어**
   - 모든 API가 C# 기준
   - 최적화 잘 됨

3. **배우기 쉬움**
   - Java와 유사
   - Python보다 엄격하지만 C++보다 쉬움

4. **강력한 IDE 지원**
   - Visual Studio IntelliSense
   - Rider 리팩토링 도구

---

### 4.3 IDE 선택: Visual Studio vs Rider

#### **Visual Studio 2022 Community (무료)**

**장점:**
- Unity와 완벽 통합
- 강력한 디버거
- 무료
- IntelliSense 자동 완성

**단점:**
- 무거움 (설치 크기 수 GB)
- 느린 시작 시간

**추천 대상:** 처음 시작하는 개발자

---

#### **JetBrains Rider (유료, 학생 무료)**

**장점:**
- VS보다 빠르고 가벼움
- 더 나은 리팩토링
- Unity 전용 기능 많음
- 검색 기능 강력

**단점:**
- 유료 ($149/년)
- 학습 곡선 약간 있음

**추천 대상:** 이미 IntelliJ/WebStorm 사용 경험자

---

### 4.4 버전 관리: Git + GitHub

**왜 Git인가?**

1. **업계 표준**
   - 게임 업계 90% 이상 사용
   - 취업 시 필수 스킬

2. **무료**
   - GitHub 무료 플랜으로 충분

3. **협업 가능**
   - Pull Request
   - 코드 리뷰

4. **버전 되돌리기**
   - 실수해도 언제든 복구 가능

---

### 4.5 데이터 저장: JSON

**왜 JSON인가?**

**✅ 장점:**
- 사람이 읽을 수 있음 (디버깅 쉬움)
- Unity에서 기본 지원 (`JsonUtility`)
- 외부 툴 연동 쉬움 (Google Sheets → JSON)
- 텍스트 기반이라 Git 관리 가능

**대안 비교:**

| 형식 | 장점 | 단점 | 추천도 |
|------|------|------|--------|
| JSON | 읽기 쉬움, Unity 기본 지원 | 파싱 느림 (큰 파일) | ⭐⭐⭐⭐⭐ |
| ScriptableObject | Unity 전용, 빠름 | 외부 편집 어려움 | ⭐⭐⭐ |
| XML | 표준, 구조적 | 장황함, 읽기 어려움 | ⭐⭐ |
| SQLite | 쿼리 가능, 빠름 | 오버엔지니어링 | ⭐ |
| CSV | 간단함 | 복잡한 구조 어려움 | ⭐⭐ |

**결론:** **JSON 사용 (Phase 1-2)**, 나중에 최적화 필요 시 ScriptableObject 전환 고려

---

### 4.6 패키지 및 라이브러리

#### **필수 패키지:**

1. **Newtonsoft.Json** (JSON 파싱)
   - Unity Package Manager에서 설치
   - 더 강력한 JSON 처리 (JsonUtility보다)

2. **TextMeshPro** (텍스트 렌더링)
   - Unity 기본 포함
   - 더 선명한 텍스트

3. **Unity UI** (UGUI)
   - 기본 포함
   - Canvas 기반 UI

---

#### **선택적 패키지:**

1. **DOTween** (애니메이션)
   - [http://dotween.demigiant.com/](http://dotween.demigiant.com/)
   - 부드러운 애니메이션 쉽게 구현
   - 예: `transform.DOMove(target, 0.5f);`

2. **Odin Inspector** (에디터 확장, 유료)
   - [https://odininspector.com/](https://odininspector.com/)
   - Inspector 커스터마이징
   - 개발 속도 향상 (비싸지만 worth it)

---

### 4.7 최종 기술 스택 요약

```
┌─────────────────────────────────────────┐
│          기술 스택 구성                  │
├─────────────────────────────────────────┤
│ 게임 엔진:     Unity 2022.3 LTS         │
│ 언어:          C# 9.0                   │
│ IDE:           Visual Studio 2022 / Rider│
│ 버전 관리:     Git + GitHub             │
│ 데이터:        JSON (Newtonsoft.Json)   │
│ UI:            Unity UI (UGUI)          │
│ 텍스트:        TextMeshPro              │
│ 애니메이션:    DOTween (선택)           │
│ 테스트:        Unity Test Framework     │
│ 빌드 타겟:     Windows (Phase 1-2)      │
│               Mac/Linux (Phase 3+)      │
│               모바일 (Phase 5+)         │
└─────────────────────────────────────────┘
```

---

## Chapter 5: 아키텍처 설계

### 5.1 전체 시스템 아키텍처

```
┌───────────────────────────────────────────────────────┐
│                   Game Manager                         │
│  - DontDestroyOnLoad 싱글톤                           │
│  - 전역 상태 관리 (currentRun, player)                │
│  - 씬 전환 제어                                         │
└────────────────────┬──────────────────────────────────┘
                     │
      ┌──────────────┼──────────────┐
      │              │              │
┌─────▼──────┐ ┌────▼─────┐ ┌──────▼────────┐
│  Data      │ │  Combat  │ │  Map          │
│  Manager   │ │  Manager │ │  Manager      │
│            │ │          │ │  (Phase 2)    │
│ - JSON 로드│ │ - 전투   │ │ - 맵 생성     │
│ - 카드 DB  │ │   상태   │ │ - 노드 진행   │
│ - 적 DB    │ │ - 턴 관리│ │               │
└─────┬──────┘ └────┬─────┘ └───────────────┘
      │             │
┌─────▼──────┐ ┌────▼──────────────┐
│  Deck      │ │  Player / Enemy   │
│  Manager   │ │  - 체력, 내공     │
│            │ │  - 상태 효과      │
│ - 드로우   │ └────┬──────────────┘
│ - 셔플     │      │
│ - 사용     │ ┌────▼──────────────┐
└────────────┘ │  Relic Manager    │
               │  - 유물 효과 적용 │
               │  - 이벤트 구독    │
               └───────────────────┘
                      │
┌─────────────────────▼─────────────────────┐
│              UI Layer                      │
│  - Combat UI, Map UI, Shop UI, Event UI   │
│  - 게임 로직과 분리 (이벤트로 통신)       │
└────────────────────────────────────────────┘
```

---

### 5.2 레이어 아키텍처 (Clean Architecture 간소화)

```
┌──────────────────────────────────────────┐
│      Presentation Layer (UI)             │  ← 사용자 입력, 시각화
│  - CombatUI.cs                           │
│  - CardUI.cs                             │
│  - MapUI.cs                              │
└──────────────────┬───────────────────────┘
                   │ (이벤트 통신)
┌──────────────────▼───────────────────────┐
│      Business Logic Layer                │  ← 게임 규칙, 로직
│  - CombatManager.cs                      │
│  - DeckManager.cs                        │
│  - Player.cs, Enemy.cs                   │
│  - RelicManager.cs                       │
└──────────────────┬───────────────────────┘
                   │ (데이터 요청)
┌──────────────────▼───────────────────────┐
│      Data Layer                          │  ← 데이터 저장/로드
│  - DataManager.cs                        │
│  - SaveManager.cs                        │
│  - CardDatabase.json                     │
└──────────────────────────────────────────┘
```

**원칙:**
1. **UI는 로직을 직접 수정하지 않음**
   - ❌ `CombatUI.cs`에서 `player.health -= 10`
   - ✅ `CombatManager.DamagePlayer(10)` 호출

2. **로직은 UI를 몰라야 함**
   - ❌ `CombatManager`에서 `healthText.text = ...`
   - ✅ `OnHealthChanged` 이벤트 발생 → UI가 구독

3. **데이터는 읽기 전용**
   - JSON 로드 후 런타임에서 수정 안 함
   - 수정이 필요하면 복사본 사용

---

### 5.3 폴더 구조 (Unity 프로젝트)

```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   ├── CombatScene.unity
│   ├── MapScene.unity
│   └── ShopScene.unity
│
├── Scripts/
│   ├── Core/                    # 핵심 매니저
│   │   ├── GameManager.cs
│   │   ├── Constants.cs
│   │   └── GameEvents.cs
│   │
│   ├── Data/                    # 데이터 구조 및 관리
│   │   ├── CardData.cs
│   │   ├── EnemyData.cs
│   │   ├── RelicData.cs
│   │   └── DataManager.cs
│   │
│   ├── Combat/                  # 전투 시스템
│   │   ├── CombatManager.cs
│   │   ├── CombatState.cs
│   │   ├── Player.cs
│   │   ├── Enemy.cs
│   │   └── StatusEffect.cs
│   │
│   ├── Cards/                   # 카드 시스템
│   │   ├── Card.cs
│   │   ├── CardTypes.cs
│   │   ├── CardEffects.cs
│   │   ├── DeckManager.cs
│   │   └── CardFactory.cs
│   │
│   ├── AI/                      # 적 AI
│   │   ├── EnemyAI.cs
│   │   ├── Intent.cs
│   │   └── IntentSystem.cs
│   │
│   ├── UI/                      # 모든 UI 스크립트
│   │   ├── Combat/
│   │   │   ├── CombatUI.cs
│   │   │   ├── CardUI.cs
│   │   │   ├── PlayerStatsUI.cs
│   │   │   └── EnemyIntentUI.cs
│   │   ├── Map/
│   │   │   ├── MapUI.cs
│   │   │   └── NodeUI.cs
│   │   └── Menu/
│   │       └── MainMenuUI.cs
│   │
│   ├── Map/                     # 맵 시스템 (Phase 2)
│   │   ├── MapGenerator.cs
│   │   ├── MapManager.cs
│   │   ├── MapNode.cs
│   │   └── NodeType.cs
│   │
│   ├── Relics/                  # 유물 시스템 (Phase 2)
│   │   ├── Relic.cs
│   │   ├── RelicEffect.cs
│   │   └── RelicManager.cs
│   │
│   ├── Shop/                    # 상점 (Phase 2)
│   │   ├── ShopManager.cs
│   │   └── ShopItem.cs
│   │
│   ├── Events/                  # 이벤트 (Phase 2)
│   │   ├── EventManager.cs
│   │   └── EventData.cs
│   │
│   ├── Meta/                    # 메타 진행 (Phase 2)
│   │   ├── MetaProgressionManager.cs
│   │   ├── MugongEssence.cs
│   │   └── PermanentUpgrade.cs
│   │
│   ├── Save/                    # 세이브/로드
│   │   ├── SaveManager.cs
│   │   ├── SaveData.cs
│   │   └── RunData.cs
│   │
│   └── Utils/                   # 유틸리티
│       ├── Extensions.cs
│       └── Helpers.cs
│
├── Data/                        # JSON 데이터 파일
│   ├── Cards/
│   │   └── CardDatabase.json
│   ├── Enemies/
│   │   └── EnemyDatabase.json
│   ├── Relics/
│   │   └── RelicDatabase.json
│   └── Events/
│       └── EventDatabase.json
│
├── Prefabs/                     # 프리팹
│   ├── Cards/
│   │   └── CardPrefab.prefab
│   ├── Enemies/
│   │   ├── Bandit.prefab
│   │   └── ...
│   └── UI/
│       ├── DamagePopup.prefab
│       └── ...
│
├── Sprites/                     # 이미지
│   ├── Cards/
│   ├── Enemies/
│   ├── UI/
│   └── Backgrounds/
│
├── Audio/                       # 사운드
│   ├── BGM/
│   └── SFX/
│
└── Tests/                       # 테스트
    ├── EditMode/                # 유닛 테스트
    │   ├── DeckManagerTests.cs
    │   └── CardEffectsTests.cs
    └── PlayMode/                # 통합 테스트
        └── CombatSystemTests.cs
```

---

### 5.4 싱글톤 매니저 계층

```
GameManager (최상위)
    - DontDestroyOnLoad
    - 씬 전환 관리
    - 전역 참조 제공

    ├─ DataManager
    │   - 게임 시작 시 한 번 로드
    │   - 모든 정적 데이터 제공
    │
    ├─ AudioManager
    │   - BGM, SFX 재생
    │   - 볼륨 조절
    │
    ├─ SaveManager
    │   - 세이브/로드
    │   - 자동 저장
    │
    └─ MetaProgressionManager (Phase 2)
        - 무공 정수 관리
        - 영구 업그레이드

CombatManager (씬별)
    - CombatScene에서만 존재
    - 전투 종료 시 파괴
    - GameManager를 통해 전역 데이터 접근

MapManager (씬별, Phase 2)
    - MapScene에서만 존재
    - 맵 진행 관리
```

---

## Chapter 6: 핵심 디자인 패턴

### 6.1 Singleton 패턴

**목적:** 게임 전체에서 하나만 존재하는 매니저

**사용처:**
- GameManager
- DataManager
- AudioManager
- SaveManager

**구현:**

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없으면 찾기
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                // 그래도 없으면 생성
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
        // 이미 인스턴스가 있으면 자신을 파괴
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지

        Initialize();
    }

    void Initialize()
    {
        Debug.Log("GameManager initialized");
        // 초기화 로직
    }
}
```

**주의사항:**
- ❌ 과도한 싱글톤 사용 (모든 클래스를 싱글톤으로 만들지 말 것)
- ❌ 싱글톤 간 순환 참조
- ✅ 진짜 전역적으로 필요한 것만 싱글톤으로

---

### 6.2 State 패턴 (전투 상태)

**목적:** 전투의 다양한 상태를 명확히 관리

**상태 정의:**

```csharp
public enum CombatState
{
    Initializing,      // 전투 초기화
    PlayerTurnStart,   // 플레이어 턴 시작
    PlayerTurn,        // 플레이어 행동 중
    PlayerTurnEnd,     // 플레이어 턴 종료
    EnemyTurn,         // 적 턴
    Victory,           // 승리
    Defeat             // 패배
}
```

**상태 머신 구현:**

```csharp
public class CombatManager : MonoBehaviour
{
    public CombatState currentState { get; private set; }

    // 상태 변경
    public void ChangeState(CombatState newState)
    {
        // 현재 상태 종료
        ExitState(currentState);

        // 상태 전환
        CombatState oldState = currentState;
        currentState = newState;

        Debug.Log($"State: {oldState} → {newState}");

        // 새 상태 진입
        EnterState(currentState);
    }

    void EnterState(CombatState state)
    {
        switch (state)
        {
            case CombatState.Initializing:
                OnInitializing();
                break;

            case CombatState.PlayerTurnStart:
                OnPlayerTurnStart();
                break;

            case CombatState.PlayerTurn:
                OnPlayerTurn();
                break;

            case CombatState.PlayerTurnEnd:
                OnPlayerTurnEnd();
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

    void ExitState(CombatState state)
    {
        // 상태 종료 시 정리 작업
        switch (state)
        {
            case CombatState.PlayerTurn:
                // 예: 타이머 정지
                break;
        }
    }

    // 각 상태별 로직
    void OnInitializing()
    {
        // 전투 초기화
        player.currentEnergy = player.maxEnergy;
        deckManager.InitializeDeck(player.deck);

        // 적 생성
        SpawnEnemies();

        // 플레이어 턴 시작으로 전환
        ChangeState(CombatState.PlayerTurnStart);
    }

    void OnPlayerTurnStart()
    {
        // 내공 회복
        player.currentEnergy = player.maxEnergy;

        // 방어도 초기화
        player.block = 0;

        // 카드 드로우
        deckManager.DrawCards(5);

        // 턴 시작 효과 발동
        GameEvents.TurnStart();

        // 플레이어 행동 단계로
        ChangeState(CombatState.PlayerTurn);
    }

    void OnPlayerTurn()
    {
        // 플레이어가 카드 사용하거나 턴 종료 대기
        // UI에서 조작 가능 상태
    }

    void OnPlayerTurnEnd()
    {
        // 손패 버리기
        deckManager.DiscardHand();

        // 턴 종료 효과 발동
        GameEvents.TurnEnd();

        // 적 턴으로 전환
        ChangeState(CombatState.EnemyTurn);
    }

    void OnEnemyTurn()
    {
        StartCoroutine(EnemyTurnCoroutine());
    }

    IEnumerator EnemyTurnCoroutine()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.currentHealth <= 0) continue;

            // 적 행동 실행
            enemy.ExecuteIntent(player);

            // 애니메이션 대기
            yield return new WaitForSeconds(1f);
        }

        // 승패 체크
        if (CheckVictory())
        {
            ChangeState(CombatState.Victory);
        }
        else if (CheckDefeat())
        {
            ChangeState(CombatState.Defeat);
        }
        else
        {
            // 다음 플레이어 턴
            ChangeState(CombatState.PlayerTurnStart);
        }
    }

    void OnVictory()
    {
        Debug.Log("전투 승리!");
        // 보상 화면 표시
        ShowRewards();
    }

    void OnDefeat()
    {
        Debug.Log("전투 패배!");
        // 게임 오버 처리
        GameManager.Instance.GameOver();
    }

    bool CheckVictory()
    {
        return enemies.All(e => e.currentHealth <= 0);
    }

    bool CheckDefeat()
    {
        return player.currentHealth <= 0;
    }
}
```

---

### 6.3 Observer 패턴 (이벤트 시스템)

**목적:** 시스템 간 느슨한 결합 (Loose Coupling)

**문제:**
- 유물 효과가 "카드 사용 시", "턴 시작 시", "전투 시작 시" 등 다양한 타이밍에 발동
- 모든 곳에서 유물을 직접 체크하면 코드가 지저분해짐

**해결:** 이벤트 시스템

**구현:**

```csharp
using System;
using UnityEngine;

// 전역 이벤트 정의
public static class GameEvents
{
    // 전투 관련
    public static event Action OnCombatStart;
    public static event Action OnCombatEnd;
    public static event Action OnTurnStart;
    public static event Action OnTurnEnd;

    // 카드 관련
    public static event Action<Card> OnCardPlayed;
    public static event Action<Card> OnCardDrawn;
    public static event Action<Card> OnCardDiscarded;

    // 적 관련
    public static event Action<Enemy> OnEnemySpawned;
    public static event Action<Enemy> OnEnemyDeath;

    // 플레이어 관련
    public static event Action<int> OnPlayerDamaged;
    public static event Action<int> OnPlayerHealed;

    // 이벤트 발생 메서드
    public static void CombatStart() => OnCombatStart?.Invoke();
    public static void CombatEnd() => OnCombatEnd?.Invoke();
    public static void TurnStart() => OnTurnStart?.Invoke();
    public static void TurnEnd() => OnTurnEnd?.Invoke();

    public static void CardPlayed(Card card) => OnCardPlayed?.Invoke(card);
    public static void CardDrawn(Card card) => OnCardDrawn?.Invoke(card);
    public static void CardDiscarded(Card card) => OnCardDiscarded?.Invoke(card);

    public static void EnemySpawned(Enemy enemy) => OnEnemySpawned?.Invoke(enemy);
    public static void EnemyDeath(Enemy enemy) => OnEnemyDeath?.Invoke(enemy);

    public static void PlayerDamaged(int amount) => OnPlayerDamaged?.Invoke(amount);
    public static void PlayerHealed(int amount) => OnPlayerHealed?.Invoke(amount);
}
```

**사용 예시 1: 유물 효과**

```csharp
public class RelicManager : MonoBehaviour
{
    public List<Relic> activeRelics = new List<Relic>();

    void OnEnable()
    {
        // 이벤트 구독
        GameEvents.OnCardPlayed += HandleCardPlayed;
        GameEvents.OnTurnStart += HandleTurnStart;
        GameEvents.OnCombatStart += HandleCombatStart;
    }

    void OnDisable()
    {
        // 이벤트 구독 해제 (메모리 누수 방지!)
        GameEvents.OnCardPlayed -= HandleCardPlayed;
        GameEvents.OnTurnStart -= HandleTurnStart;
        GameEvents.OnCombatStart -= HandleCombatStart;
    }

    void HandleCardPlayed(Card card)
    {
        foreach (var relic in activeRelics)
        {
            relic.OnCardPlayed(card);
        }
    }

    void HandleTurnStart()
    {
        foreach (var relic in activeRelics)
        {
            relic.OnTurnStart();
        }
    }

    void HandleCombatStart()
    {
        foreach (var relic in activeRelics)
        {
            relic.OnCombatStart();
        }
    }
}
```

**사용 예시 2: UI 업데이트**

```csharp
public class PlayerHealthUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image healthBar;

    void OnEnable()
    {
        GameEvents.OnPlayerDamaged += UpdateHealth;
        GameEvents.OnPlayerHealed += UpdateHealth;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerDamaged -= UpdateHealth;
        GameEvents.OnPlayerHealed -= UpdateHealth;
    }

    void UpdateHealth(int amount)
    {
        Player player = CombatManager.Instance.player;
        healthText.text = $"{player.currentHealth} / {player.maxHealth}";
        healthBar.fillAmount = (float)player.currentHealth / player.maxHealth;
    }
}
```

**주의사항:**
- ✅ 반드시 `OnDisable`에서 구독 해제 (메모리 누수 방지)
- ❌ 이벤트 핸들러에서 무거운 작업 금지 (프레임 드롭)
- ✅ 이벤트는 읽기 전용 (상태 변경은 Manager를 통해)

---

### 6.4 Factory 패턴 (카드/유물 생성)

**목적:** 객체 생성 로직을 한 곳에 집중

**구현:**

```csharp
public class CardFactory
{
    // 카드 데이터로부터 런타임 Card 인스턴스 생성
    public static Card CreateCard(CardData data)
    {
        Card card = new Card
        {
            data = data,
            id = data.id,
            name = data.name,
            cost = data.cost,
            type = data.type,
            isUpgraded = false
        };

        return card;
    }

    // 업그레이드된 카드 생성
    public static Card CreateUpgradedCard(CardData data)
    {
        Card card = CreateCard(data);
        card.isUpgraded = true;
        return card;
    }

    // 카드 복사 (덱 빌딩 시 같은 카드 여러 장)
    public static Card Clone(Card original)
    {
        Card clone = CreateCard(original.data);
        clone.isUpgraded = original.isUpgraded;
        return clone;
    }

    // ID로 카드 생성 (편의 메서드)
    public static Card CreateCardById(string cardId)
    {
        CardData data = DataManager.Instance.GetCard(cardId);
        if (data == null)
        {
            Debug.LogError($"Card {cardId} not found in database!");
            return null;
        }
        return CreateCard(data);
    }

    // 랜덤 카드 생성 (보상 시스템용)
    public static Card CreateRandomCard(Rarity rarity)
    {
        var cards = DataManager.Instance.GetCardsByRarity(rarity);
        if (cards.Count == 0) return null;

        CardData randomData = cards[UnityEngine.Random.Range(0, cards.Count)];
        return CreateCard(randomData);
    }
}
```

**사용 예시:**

```csharp
// 시작 덱 생성
List<Card> startingDeck = new List<Card>();
for (int i = 0; i < 5; i++)
{
    startingDeck.Add(CardFactory.CreateCardById("card_strike"));
}
for (int i = 0; i < 4; i++)
{
    startingDeck.Add(CardFactory.CreateCardById("card_defend"));
}

// 보상 카드 생성
Card rewardCard = CardFactory.CreateRandomCard(Rarity.Uncommon);

// 카드 업그레이드
Card upgradedCard = CardFactory.CreateUpgradedCard(originalCard.data);
```

---

### 6.5 Command 패턴 (카드 효과)

**목적:** 카드 효과를 객체로 캡슐화, 실행 취소 가능

**구현:**

```csharp
// 커맨드 인터페이스
public interface ICardEffect
{
    void Execute(Player player, Enemy target);
    void Undo(); // 선택적: 리플레이 시스템용
    string GetDescription(); // UI 표시용
}

// 피해 효과
public class DamageEffect : ICardEffect
{
    private int damage;
    private Enemy lastTarget;

    public DamageEffect(int damage)
    {
        this.damage = damage;
    }

    public void Execute(Player player, Enemy target)
    {
        lastTarget = target;
        target.TakeDamage(damage);
        GameEvents.CardPlayed(null); // 필요시 카드 참조 전달
    }

    public void Undo()
    {
        // 리플레이 시스템에서 사용
        lastTarget.currentHealth += damage;
    }

    public string GetDescription()
    {
        return $"적에게 {damage} 피해를 준다.";
    }
}

// 방어도 효과
public class BlockEffect : ICardEffect
{
    private int block;

    public BlockEffect(int block)
    {
        this.block = block;
    }

    public void Execute(Player player, Enemy target)
    {
        player.GainBlock(block);
    }

    public void Undo() { /* 필요시 구현 */ }

    public string GetDescription()
    {
        return $"방어도 {block}을 얻는다.";
    }
}

// 드로우 효과
public class DrawCardsEffect : ICardEffect
{
    private int count;

    public DrawCardsEffect(int count)
    {
        this.count = count;
    }

    public void Execute(Player player, Enemy target)
    {
        DeckManager.Instance.DrawCards(count);
    }

    public void Undo() { /* 어려움 */ }

    public string GetDescription()
    {
        return $"카드 {count}장을 뽑는다.";
    }
}

// 복합 효과 (피해 + 방어)
public class CompositeEffect : ICardEffect
{
    private List<ICardEffect> effects = new List<ICardEffect>();

    public void AddEffect(ICardEffect effect)
    {
        effects.Add(effect);
    }

    public void Execute(Player player, Enemy target)
    {
        foreach (var effect in effects)
        {
            effect.Execute(player, target);
        }
    }

    public void Undo()
    {
        // 역순으로 실행 취소
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].Undo();
        }
    }

    public string GetDescription()
    {
        return string.Join(" ", effects.Select(e => e.GetDescription()));
    }
}
```

**Card 클래스에 적용:**

```csharp
public class Card
{
    public CardData data;
    public bool isUpgraded;

    public List<ICardEffect> effects = new List<ICardEffect>();

    // 카드 생성 시 효과 추가
    public void InitializeEffects()
    {
        switch (data.id)
        {
            case "card_strike":
                effects.Add(new DamageEffect(isUpgraded ? 9 : 6));
                break;

            case "card_defend":
                effects.Add(new BlockEffect(isUpgraded ? 8 : 5));
                break;

            case "card_clear_mind":
                // 복합 효과
                effects.Add(new BlockEffect(3));
                effects.Add(new DrawCardsEffect(1));
                break;
        }
    }

    public void Play(Player player, Enemy target)
    {
        foreach (var effect in effects)
        {
            effect.Execute(player, target);
        }
    }
}
```

**장점:**
- 카드 효과 조합 용이
- 새로운 효과 추가 쉬움
- 리플레이 시스템 구현 가능 (미래 기능)

---

### 6.6 Object Pool 패턴 (카드 UI)

**목적:** 카드 UI GameObject를 재사용하여 GC(Garbage Collection) 감소

**문제:**
```csharp
// ❌ 나쁜 예: 매번 생성/파괴
void DrawCard(Card card)
{
    GameObject cardObj = Instantiate(cardPrefab); // GC 발생!
    // ... 사용
    Destroy(cardObj); // GC 발생!
}
```

**해결:**
```csharp
public class CardUIPool : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform poolParent; // 비활성 카드 보관 장소

    private Queue<GameObject> pool = new Queue<GameObject>();

    // 초기 풀 생성
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateNewCard();
        }
    }

    GameObject CreateNewCard()
    {
        GameObject card = Instantiate(cardPrefab, poolParent);
        card.SetActive(false);
        pool.Enqueue(card);
        return card;
    }

    // 풀에서 카드 가져오기
    public GameObject GetCard()
    {
        if (pool.Count == 0)
        {
            return CreateNewCard();
        }

        GameObject card = pool.Dequeue();
        card.SetActive(true);
        return card;
    }

    // 풀로 카드 반환
    public void ReturnCard(GameObject card)
    {
        card.SetActive(false);
        card.transform.SetParent(poolParent);
        pool.Enqueue(card);
    }
}
```

**사용:**
```csharp
// ✅ 좋은 예
GameObject cardObj = CardUIPool.Instance.GetCard();
// ... 카드 사용
CardUIPool.Instance.ReturnCard(cardObj);
```

---

## 디자인 패턴 요약

| 패턴 | 사용처 | 목적 |
|------|--------|------|
| Singleton | GameManager, DataManager | 전역 접근, 단일 인스턴스 |
| State | CombatManager | 전투 상태 관리 |
| Observer | GameEvents | 느슨한 결합, 유물 효과 |
| Factory | CardFactory | 카드 생성 로직 집중 |
| Command | CardEffect | 효과 캡슐화, 조합 |
| Object Pool | CardUI | 성능 최적화, GC 감소 |

---

**다음 챕터 미리보기:**
- Chapter 7: Unity 설치 및 프로젝트 생성 (실전)
- Chapter 8: Unity 프로젝트 구조 (폴더 설정)
- Chapter 9: Unity 핵심 개념 (GameObject, Component, Prefab)

---

# PART 3: Unity 개발

## Chapter 7: Unity 설치 및 프로젝트 생성

### 7.1 Unity Hub 설치

#### **Windows:**

1. [https://unity.com/download](https://unity.com/download) 접속
2. "Download Unity Hub" 클릭
3. `UnityHubSetup.exe` 다운로드
4. 실행 → "I accept the terms" → Install
5. 설치 완료 후 Unity Hub 실행

#### **macOS:**

1. 위 링크에서 "Download Unity Hub" 클릭
2. `UnityHubSetup.dmg` 다운로드
3. DMG 파일 열기 → Unity Hub 드래그하여 Applications 폴더로 이동
4. Unity Hub 실행

---

### 7.2 Unity 계정 생성

1. Unity Hub 실행
2. 우측 상단 사람 아이콘 클릭
3. "Sign in" 클릭
4. "Create account" 선택
5. 이메일, 비밀번호 입력
6. 이메일 인증
7. 로그인 완료

---

### 7.3 Unity 라이선스 활성화

1. Unity Hub → 톱니바퀴(Settings) → License Management
2. "Add" 클릭
3. "Get a free personal license" 선택
4. "I don't use Unity in a professional capacity" 체크
5. "Done"

---

### 7.4 Unity Editor 설치

1. Unity Hub → "Installs" 탭
2. "Install Editor" 클릭
3. **"Recommended Release"** 섹션에서 **2022.3 LTS** 선택
   - LTS = Long Term Support (2년 동안 버그 수정 보장)
4. "Install" 클릭

**모듈 선택 (중요!):**
```
✅ Visual Studio Community (또는 이미 설치되어 있으면 체크 해제)
✅ Documentation
✅ Windows Build Support (IL2CPP) [Windows 사용자]
✅ Mac Build Support (Mono) [Mac 사용자]
□ Linux Build Support (필요 시)
□ Android Build Support (Phase 5+)
□ iOS Build Support (Phase 5+)
□ WebGL Build Support (선택적)
```

5. "Continue" → 다운로드 시작 (5-10GB, 10-20분 소요)

---

### 7.5 프로젝트 생성

1. Unity Hub → "Projects" 탭
2. "New project" 클릭
3. Unity 버전 선택: **2022.3.x**
4. 템플릿 선택: **2D (URP)**
   - URP = Universal Render Pipeline (최신 렌더링, 더 나은 성능)
5. 프로젝트 설정:
   ```
   Project name: MurimDeckbuilder
   Location: C:\Projects\  (또는 원하는 경로)
   ```
6. "Create project" 클릭
7. Unity Editor 로딩 대기 (1-2분)

---

### 7.6 Unity 에디터 레이아웃 설정

#### **첫 실행 시:**

1. Unity Editor가 열리면 기본 레이아웃이 표시됨
2. 우측 상단: **Layouts → 2 by 3** 선택 (권장)

#### **레이아웃 구성 설명:**

```
┌─────────────────────────────────────────────────────┐
│  Menu Bar (File, Edit, Assets, GameObject...)       │
├───────────────────┬─────────────────────────────────┤
│                   │                                 │
│   Hierarchy       │         Scene / Game            │
│   (씬 내 객체 목록)│       (3D/2D 뷰 / 실행 화면)    │
│                   │                                 │
├───────────────────┼─────────────────────────────────┤
│                   │                                 │
│    Project        │         Inspector               │
│  (Assets 폴더 내용)│    (선택한 객체의 속성)          │
│                   │                                 │
└───────────────────┴─────────────────────────────────┘
│            Console (로그 메시지)                      │
└──────────────────────────────────────────────────────┘
```

---

### 7.7 Package Manager 설정

1. **Window → Package Manager** 클릭
2. 좌측 상단: **Packages: Unity Registry** 선택

**필수 패키지 설치:**

3. **TextMeshPro** 검색 → "Install" (더 나은 텍스트 렌더링)
4. **Input System** 검색 → "Install" (새 입력 시스템, 선택적)

---

### 7.8 프로젝트 설정

#### **1. Player Settings**

1. **Edit → Project Settings → Player**
2. **Company Name**: 본인 이름 또는 팀명 입력 (예: "MurimStudio")
3. **Product Name**: "강호무적"

#### **2. Quality Settings**

1. **Edit → Project Settings → Quality**
2. **VSync Count**: "Every V Blank" (60 FPS 고정)
3. **Anti Aliasing**: 2x Multi Sampling (선택적, 부드러운 그래픽)

#### **3. Editor Settings**

1. **Edit → Preferences → External Tools**
2. **External Script Editor**: Visual Studio 또는 Rider 확인

---

### 7.9 첫 씬 저장

1. **File → Save As**
2. 폴더: `Assets/Scenes` 생성
3. 파일명: `TestScene` 입력
4. "Save"

---

### 7.10 첫 GameObject 생성

#### **Canvas 생성 (UI 컨테이너):**

1. **Hierarchy 우클릭 → UI → Canvas**
2. 자동으로 다음이 생성됨:
   - Canvas
   - EventSystem (UI 입력 처리)

#### **Text 추가:**

1. **Canvas 우클릭 → UI → Text - TextMeshPro**
2. "TMP Importer" 창이 뜨면:
   - "Import TMP Essentials" 클릭
   - "Import TMP Examples & Extras" (선택적)
3. Inspector에서:
   - **Text**: "강호무적 - 개발 시작!"
   - **Font Size**: 48
   - **Alignment**: 중앙 정렬 (가운데 버튼)
   - **Color**: 흰색

---

### 7.11 첫 스크립트 작성

#### **1. Scripts 폴더 생성:**

1. **Project 창 → Assets 우클릭 → Create → Folder**
2. 이름: `Scripts`

#### **2. 스크립트 생성:**

1. **Scripts 폴더 우클릭 → Create → C# Script**
2. 이름: `HelloWorld` (엔터 전에 이름 입력!)

#### **3. 코드 작성:**

1. `HelloWorld.cs` 더블 클릭 (Visual Studio 열림)
2. 다음 코드 입력:

```csharp
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    void Start()
    {
        Debug.Log("강호무적 개발 시작!");
        Debug.Log("Unity 버전: " + Application.unityVersion);
    }

    void Update()
    {
        // 스페이스바 누르면 메시지 출력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스바 눌림!");
        }
    }
}
```

3. **Ctrl + S** (저장)
4. Unity로 돌아옴 (자동 컴파일)

#### **4. 스크립트 연결:**

1. **Hierarchy에서 Canvas 선택**
2. **Project 창에서 HelloWorld 스크립트를 드래그 → Canvas로 드롭**
3. Inspector에서 HelloWorld 컴포넌트 확인

---

### 7.12 게임 실행

1. **상단 재생 버튼 (▶️) 클릭**
2. **Console 창 확인** (Window → General → Console)
   - "강호무적 개발 시작!" 메시지 확인
   - Unity 버전 확인
3. **Game 뷰에서 스페이스바 누르기**
   - Console에 "스페이스바 눌림!" 메시지 확인
4. **재생 버튼 다시 클릭 (정지)**

---

### 7.13 Git 초기화 (중요!)

#### **1. .gitignore 파일 생성:**

프로젝트 폴더 (`C:\Projects\MurimDeckbuilder\`)에서:

1. `.gitignore` 파일 생성
2. Unity 전용 gitignore 내용 복사:
   - [GitHub Unity gitignore](https://github.com/github/gitignore/blob/main/Unity.gitignore)

#### **2. Git 초기화:**

```bash
cd C:\Projects\MurimDeckbuilder
git init
git add .
git commit -m "Initial Unity project setup"
```

#### **3. GitHub 레포 생성 및 푸시:**

1. [GitHub](https://github.com/) 로그인
2. "New repository" 클릭
3. Repository name: `murim-deckbuilder`
4. Private 선택 (또는 Public)
5. "Create repository"
6. 터미널에서:

```bash
git remote add origin https://github.com/yourusername/murim-deckbuilder.git
git branch -M main
git push -u origin main
```

---

### 7.14 체크리스트

다음 챕터로 넘어가기 전 확인:

- [ ] Unity Hub 설치 완료
- [ ] Unity 2022.3 LTS 설치 완료
- [ ] 프로젝트 "MurimDeckbuilder" 생성 완료
- [ ] TextMeshPro 패키지 설치 완료
- [ ] Canvas 및 Text 생성 확인
- [ ] HelloWorld 스크립트 작동 확인
- [ ] Git 초기화 및 첫 커밋 완료
- [ ] GitHub에 푸시 완료

✅ 모두 완료! 이제 본격적인 개발을 시작할 준비가 되었습니다!

---

## Chapter 8: Unity 프로젝트 구조

### 8.1 Assets 폴더 구조 Best Practice

Unity 프로젝트의 성공은 잘 조직된 폴더 구조에서 시작됩니다.

#### **권장 폴더 구조:**

```
Assets/
├── _Project/                    # 프로젝트 전용 폴더 (앞에 _ 로 최상단 정렬)
│   ├── Scenes/                 # 씬 파일
│   │   ├── MainMenu.unity
│   │   ├── CombatScene.unity
│   │   ├── MapScene.unity
│   │   └── _Test/              # 테스트용 씬 (배포 제외)
│   │       └── TestCombat.unity
│   │
│   ├── Scripts/                # C# 스크립트
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── Constants.cs
│   │   │   └── GameEvents.cs
│   │   ├── Data/
│   │   ├── Combat/
│   │   ├── Cards/
│   │   ├── UI/
│   │   └── Utils/
│   │
│   ├── Prefabs/               # 프리팹
│   │   ├── Cards/
│   │   │   └── CardPrefab.prefab
│   │   ├── Enemies/
│   │   ├── UI/
│   │   └── VFX/
│   │
│   ├── Data/                  # 게임 데이터 (JSON, ScriptableObject)
│   │   ├── Cards/
│   │   │   └── CardDatabase.json
│   │   ├── Enemies/
│   │   └── Relics/
│   │
│   ├── Art/                   # 아트 에셋
│   │   ├── Sprites/
│   │   │   ├── Cards/
│   │   │   ├── UI/
│   │   │   ├── Characters/
│   │   │   └── Backgrounds/
│   │   ├── Textures/
│   │   └── Animations/
│   │
│   ├── Audio/                 # 사운드
│   │   ├── BGM/
│   │   │   ├── Menu.mp3
│   │   │   ├── Combat.mp3
│   │   │   └── Boss.mp3
│   │   └── SFX/
│   │       ├── CardPlay.wav
│   │       ├── Damage.wav
│   │       └── Victory.wav
│   │
│   └── Resources/             # Resources.Load() 전용
│       └── Data/
│           ├── CardDatabase.json
│           └── EnemyDatabase.json
│
├── Plugins/                   # 외부 플러그인 (DOTween 등)
│
└── Tests/                     # 테스트 코드
    ├── EditMode/              # 에디터 모드 테스트 (유닛 테스트)
    │   ├── DeckManagerTests.cs
    │   └── CardFactoryTests.cs
    └── PlayMode/              # 플레이 모드 테스트 (통합 테스트)
        └── CombatSystemTests.cs
```

---

### 8.2 Naming Convention (명명 규칙)

일관된 명명 규칙은 협업과 유지보수에 필수입니다.

#### **파일명 규칙:**

```
✅ 좋은 예:
- CardPrefab.prefab
- PlayerController.cs
- CardDatabase.json
- MainMenu.unity

❌ 나쁜 예:
- card prefab.prefab         (공백 금지)
- playercontroller.cs        (PascalCase 사용)
- CardDB.json                (약어보다 전체 단어)
- main_menu.unity            (snake_case 금지, PascalCase 사용)
```

#### **스크립트 명명 규칙:**

```csharp
// 클래스: PascalCase
public class GameManager : MonoBehaviour { }
public class CardFactory { }

// 인터페이스: I 접두사 + PascalCase
public interface ICardEffect { }
public interface IDamageable { }

// 메서드: PascalCase
public void DrawCard() { }
private void OnPlayerTurnStart() { }

// 변수: camelCase
private int currentHealth;
public float moveSpeed;

// Private 필드: _camelCase (선택적, 팀 규칙에 따라)
private int _cachedValue;
private Player _player;

// 상수: UPPER_SNAKE_CASE
public const int MAX_HAND_SIZE = 10;
private const float CARD_SPACING = 1.5f;

// 프로퍼티: PascalCase
public int CurrentHealth { get; set; }
public string PlayerName { get; private set; }
```

#### **씬 명명 규칙:**

```
MainMenu.unity          # 메인 메뉴
CombatScene.unity       # 전투
MapScene.unity          # 맵
ShopScene.unity         # 상점
_TestCombat.unity       # 테스트 씬 (언더스코어로 구분)
```

---

### 8.3 Scene 조직 (Hierarchy 구조)

씬 내 GameObject도 잘 정리해야 합니다.

#### **Hierarchy 권장 구조:**

```
CombatScene
├── === MANAGERS ===        # 빈 GameObject (구분자)
│   ├── GameManager
│   ├── CombatManager
│   └── AudioManager
│
├── === CAMERA ===
│   └── Main Camera
│
├── === UI ===
│   ├── Canvas
│   │   ├── PlayerStatsUI
│   │   ├── EnemyIntentUI
│   │   ├── HandArea
│   │   └── DeckPiles
│   └── EventSystem
│
├── === GAME OBJECTS ===
│   ├── Player
│   ├── Enemies
│   │   ├── Enemy_Bandit_1
│   │   └── Enemy_Bandit_2
│   └── VFX
│
└── === ENVIRONMENT ===
    ├── Background
    └── Lighting
```

**규칙:**
- 빈 GameObject로 카테고리 구분 (`=== NAME ===`)
- 카테고리는 알파벳 순서가 아닌 중요도 순
- Prefab 인스턴스는 명확한 이름 (Enemy_Bandit_1, 2...)

---

### 8.4 Prefab 조직

#### **Prefab 변형 (Prefab Variants) 사용:**

```
Prefabs/
├── Cards/
│   ├── CardBase.prefab          # 기본 카드
│   ├── AttackCard.prefab        # Variant (CardBase 상속)
│   ├── DefenseCard.prefab       # Variant (CardBase 상속)
│   └── SkillCard.prefab         # Variant (CardBase 상속)
```

**장점:**
- 기본 프리팹 수정 시 모든 Variant에 자동 적용
- 개별 Variant는 고유한 설정 가능

**생성 방법:**
1. CardBase.prefab 생성
2. Hierarchy에 CardBase 드래그
3. 수정 후 우클릭 → Prefab → Create Variant
4. 이름: AttackCard

---

### 8.5 Resources 폴더 주의사항

**Resources 폴더는 최소화!**

```
❌ 나쁜 예:
Resources/
├── Sprites/           # 모든 스프라이트를 Resources에 넣지 말 것!
├── Prefabs/
└── Audio/

✅ 좋은 예:
Resources/
└── Data/              # JSON 파일만 (동적 로드 필요한 것만)
    ├── CardDatabase.json
    └── EnemyDatabase.json
```

**이유:**
- Resources 폴더의 모든 파일은 빌드에 포함됨 (사용 안 해도)
- AssetBundle이나 Addressables 사용이 더 나음
- 단, JSON 데이터는 예외 (동적 로드 필요)

---

### 8.6 .gitignore 설정

Unity 프로젝트의 Git 관리를 위한 필수 설정.

**`.gitignore` 내용:**

```gitignore
# Unity generated
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# Asset meta data should be version controlled
!*.meta

# Unity3D generated meta files
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Visual Studio cache
.vs/
.vscode/

# Rider
.idea/

# OS files
.DS_Store
Thumbs.db

# Builds
*.apk
*.aab
*.unitypackage
*.app
*.exe

# User-specific files
*.csproj
*.sln
*.suo
*.user
*.userosscache
*.sln.docstates
```

**중요:**
- `.meta` 파일은 반드시 커밋! (없으면 GUID 충돌)
- `Library/` 폴더는 절대 커밋 금지 (자동 생성)

---

### 8.7 체크리스트

프로젝트 구조 설정 완료 확인:

- [ ] Assets/_Project 폴더 생성
- [ ] Scenes, Scripts, Prefabs, Data, Art, Audio 폴더 생성
- [ ] Naming Convention 문서화
- [ ] Scene Hierarchy 템플릿 적용
- [ ] .gitignore 설정 완료
- [ ] 첫 커밋 완료

---

## Chapter 9: Unity 핵심 개념

### 9.1 GameObject와 Component

Unity의 가장 기본이 되는 개념입니다.

#### **GameObject란?**

씬에 존재하는 모든 것의 컨테이너입니다.

```csharp
// GameObject 생성
GameObject player = new GameObject("Player");

// 위치 설정
player.transform.position = new Vector3(0, 0, 0);

// 태그 설정
player.tag = "Player";

// 레이어 설정
player.layer = LayerMask.NameToLayer("PlayerLayer");
```

---

#### **Component란?**

GameObject에 기능을 추가하는 모듈입니다.

```csharp
// Component 추가
SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
renderer.sprite = mySprite;

// Component 가져오기
SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

// Component 찾기
Player playerScript = FindObjectOfType<Player>();

// Component 제거
Destroy(player.GetComponent<Rigidbody2D>());
```

---

#### **MonoBehaviour 생명주기:**

```csharp
public class Player : MonoBehaviour
{
    // 1. GameObject가 활성화되기 전 (한 번)
    void Awake()
    {
        Debug.Log("Awake: 초기화, Singleton 설정");
    }

    // 2. 첫 Update 전 (한 번)
    void Start()
    {
        Debug.Log("Start: 다른 객체 참조, 게임 시작 로직");
    }

    // 3. GameObject 활성화될 때마다
    void OnEnable()
    {
        Debug.Log("OnEnable: 이벤트 구독");
    }

    // 4. 매 프레임 (불규칙)
    void Update()
    {
        // 입력 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    // 5. 고정 간격 (물리 연산용, 기본 0.02초)
    void FixedUpdate()
    {
        // 물리 이동
        rb.AddForce(Vector2.up * jumpForce);
    }

    // 6. Update 후 (카메라 추적용)
    void LateUpdate()
    {
        // 카메라가 플레이어 따라가기
    }

    // 7. GameObject 비활성화될 때마다
    void OnDisable()
    {
        Debug.Log("OnDisable: 이벤트 구독 해제");
    }

    // 8. GameObject 파괴될 때 (한 번)
    void OnDestroy()
    {
        Debug.Log("OnDestroy: 정리 작업");
    }
}
```

**호출 순서:**
```
Awake → OnEnable → Start → Update/FixedUpdate/LateUpdate (반복) → OnDisable → OnDestroy
```

---

### 9.2 Prefab 시스템

Prefab = 재사용 가능한 GameObject 템플릿

#### **Prefab 생성:**

1. **Hierarchy에서 GameObject 생성 및 설정**
   ```
   - CardUI (GameObject)
     - Background (Image)
     - CardName (TextMeshProUGUI)
     - Cost (TextMeshProUGUI)
     - Description (TextMeshProUGUI)
   ```

2. **Project 창으로 드래그**
   - Hierarchy의 CardUI를 Project의 Prefabs/Cards/ 폴더로 드래그
   - CardUI.prefab 생성됨

3. **Prefab 사용**
   ```csharp
   public class HandUI : MonoBehaviour
   {
       public GameObject cardPrefab;

       void CreateCard()
       {
           // Prefab 인스턴스화
           GameObject card = Instantiate(cardPrefab, handTransform);

           // 위치 설정
           card.transform.localPosition = new Vector3(0, 0, 0);
       }
   }
   ```

---

#### **Prefab Variants (변형):**

```
CardBase.prefab               # 기본
├── AttackCard.prefab         # 빨간색 배경
├── DefenseCard.prefab        # 파란색 배경
└── SkillCard.prefab          # 초록색 배경
```

**장점:**
- CardBase 수정 시 모든 Variant 자동 업데이트
- 각 Variant는 색상만 다르게 설정

**생성:**
1. CardBase Prefab을 Hierarchy에 드래그
2. 배경색 변경
3. 우클릭 → Prefab → Create Variant
4. 저장: AttackCard.prefab

---

### 9.3 ScriptableObject

데이터 전용 에셋입니다.

#### **사용 목적:**

- ❌ GameObject에 붙이지 않는 데이터
- ✅ 여러 GameObject가 공유하는 데이터
- ✅ JSON 대신 Unity Inspector에서 편집 가능

#### **카드 데이터 예시:**

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Game/Card")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public int cost;
    public int baseDamage;
    public Sprite artwork;
    [TextArea(3, 10)]
    public string description;
}
```

**생성:**
1. Project 창 우클릭 → Create → Game → Card
2. Inspector에서 값 입력
3. 저장: Strike.asset

**사용:**
```csharp
public class Card : MonoBehaviour
{
    public CardDataSO data;

    void Start()
    {
        Debug.Log($"카드: {data.cardName}, 비용: {data.cost}");
    }
}
```

**장점:**
- Unity Inspector에서 직접 편집
- 드래그 앤 드롭으로 할당
- 변경 즉시 반영

**단점:**
- 외부 툴(Excel) 연동 어려움
- 버전 관리 어려움
- 추천: Phase 1은 JSON, 나중에 ScriptableObject 고려

---

### 9.4 Coroutine (코루틴)

비동기 작업을 간단하게 처리하는 Unity의 핵심 기능입니다.

#### **기본 사용법:**

```csharp
using System.Collections;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // 코루틴 시작
        StartCoroutine(MyCoroutine());
    }

    IEnumerator MyCoroutine()
    {
        Debug.Log("시작");

        // 1초 대기
        yield return new WaitForSeconds(1f);

        Debug.Log("1초 후");

        // 다음 프레임까지 대기
        yield return null;

        Debug.Log("다음 프레임");

        // 조건까지 대기
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        Debug.Log("스페이스 눌림!");
    }
}
```

---

#### **실전 예시: 적 턴 처리**

```csharp
public class CombatManager : MonoBehaviour
{
    void OnEnemyTurn()
    {
        StartCoroutine(EnemyTurnCoroutine());
    }

    IEnumerator EnemyTurnCoroutine()
    {
        foreach (var enemy in enemies)
        {
            // 적 공격 애니메이션
            enemy.Attack();

            // 애니메이션 재생 대기
            yield return new WaitForSeconds(0.5f);

            // 피해 적용
            player.TakeDamage(enemy.attackDamage);

            // 피해 이펙트 대기
            yield return new WaitForSeconds(0.5f);
        }

        // 모든 적 행동 완료 후 플레이어 턴
        ChangeState(CombatState.PlayerTurnStart);
    }
}
```

---

#### **카드 드로우 애니메이션:**

```csharp
IEnumerator DrawCardAnimation(Card card)
{
    GameObject cardObj = Instantiate(cardPrefab, deckPosition);

    // 덱 위치에서 시작
    cardObj.transform.position = deckPosition;

    // 손패 위치로 이동 (0.3초)
    float elapsed = 0f;
    while (elapsed < 0.3f)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / 0.3f;

        cardObj.transform.position = Vector3.Lerp(
            deckPosition,
            handPosition,
            t
        );

        yield return null; // 다음 프레임
    }

    // 최종 위치 보정
    cardObj.transform.position = handPosition;

    Debug.Log("카드 드로우 완료!");
}
```

---

#### **코루틴 정지:**

```csharp
private Coroutine myCoroutine;

void Start()
{
    // 코루틴 참조 저장
    myCoroutine = StartCoroutine(MyCoroutine());
}

void StopMyCoroutine()
{
    if (myCoroutine != null)
    {
        StopCoroutine(myCoroutine);
        myCoroutine = null;
    }
}
```

---

### 9.5 Unity Events vs C# Events

#### **C# Events (코드 기반):**

```csharp
public class Player : MonoBehaviour
{
    // 이벤트 선언
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    public void Die()
    {
        // 이벤트 발생
        OnDeath?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChanged?.Invoke(health);
    }
}

// 구독
public class UI : MonoBehaviour
{
    void Start()
    {
        player.OnDeath += HandlePlayerDeath;
        player.OnHealthChanged += UpdateHealthBar;
    }

    void OnDestroy()
    {
        player.OnDeath -= HandlePlayerDeath;
        player.OnHealthChanged -= UpdateHealthBar;
    }
}
```

**장점:** 코드로 완전 제어, 타입 안전

---

#### **Unity Events (Inspector 연결):**

```csharp
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // Inspector에서 연결 가능
    public UnityEvent OnDeath;
    public UnityEvent<int> OnHealthChanged;

    public void Die()
    {
        OnDeath?.Invoke();
    }
}
```

**Inspector에서:**
1. Player GameObject 선택
2. OnDeath 이벤트의 + 버튼 클릭
3. 대상 GameObject 드래그 (예: GameOverUI)
4. 함수 선택 (예: GameOverUI.Show())

**장점:** 코드 없이 연결, 디자이너 친화적
**단점:** 런타임 에러 가능 (연결 끊어지면)

---

### 9.6 Transform 다루기

#### **위치:**

```csharp
// 월드 좌표
transform.position = new Vector3(10, 5, 0);

// 로컬 좌표 (부모 기준)
transform.localPosition = new Vector3(1, 0, 0);

// 이동
transform.Translate(Vector3.right * speed * Time.deltaTime);

// 부드러운 이동
transform.position = Vector3.Lerp(start, end, t);
```

#### **회전:**

```csharp
// 오일러 각도
transform.rotation = Quaternion.Euler(0, 90, 0);

// 특정 방향 보기
transform.LookAt(target);

// 부드러운 회전
transform.rotation = Quaternion.Lerp(start, end, t);
```

#### **크기:**

```csharp
// 로컬 스케일
transform.localScale = new Vector3(2, 2, 2);

// 2배 확대
transform.localScale *= 2f;
```

---

### 9.7 체크리스트

Unity 핵심 개념 이해 확인:

- [ ] GameObject와 Component 차이 이해
- [ ] MonoBehaviour 생명주기 암기
- [ ] Prefab 생성 및 Instantiate 가능
- [ ] ScriptableObject 생성 및 사용 가능
- [ ] Coroutine으로 대기/애니메이션 구현 가능
- [ ] C# Events와 Unity Events 차이 이해
- [ ] Transform 조작 능숙

---

# PART 4: Git 및 버전 관리

## Chapter 10: Git 설치 및 기본 설정

### 10.1 Git 설치

#### **Windows:**

1. [https://git-scm.com/download/win](https://git-scm.com/download/win) 접속
2. 64-bit Git for Windows Setup 다운로드
3. 실행 후 다음 설정:
   - Editor: Visual Studio Code 또는 기본
   - PATH: Git from the command line and also from 3rd-party software
   - Line ending: Checkout Windows-style, commit Unix-style
   - Terminal: MinTTY
   - 나머지는 기본값

#### **Mac:**

```bash
# Homebrew가 설치되어 있다면
brew install git

# 또는 Xcode Command Line Tools
xcode-select --install
```

#### **확인:**

```bash
git --version
# git version 2.40.0 (또는 유사)
```

### 10.2 Git 기본 설정

#### **사용자 정보 설정:**

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

#### **에디터 설정:**

```bash
# VS Code
git config --global core.editor "code --wait"
```

#### **설정 확인:**

```bash
git config --list
```

### 10.3 Unity용 .gitignore

Unity 프로젝트에 필수적인 `.gitignore` 파일:

```gitignore
# Unity generated
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# Visual Studio cache
.vs/
*.csproj
*.sln
*.suo

# Rider
.idea/

# OS generated
.DS_Store
Thumbs.db
```

### 10.4 체크리스트

- [ ] Git 설치 완료
- [ ] Git 사용자 정보 설정
- [ ] .gitignore 파일 생성
- [ ] git status 명령어 실행 가능

> **💡 상세 가이드**: Git 워크플로우에 대한 더 자세한 내용은 `git-workflow-guide-KR.md` 문서를 참조하세요.

---

## Chapter 11: Git 기본 워크플로우

> "Git은 개발자의 타임머신입니다. 언제든 과거로 돌아갈 수 있습니다."

### 11.1 Git 기본 명령어 완전 가이드

#### **저장소 초기화**

```bash
# 새 Git 저장소 생성
cd C:\Projects\MurimDeckBuilder
git init

# 결과 확인
ls -la  # .git 폴더가 생성됨
```

**📌 `.git` 폴더란?**
- Git의 모든 이력이 저장되는 곳
- 절대 직접 수정하지 말 것
- 백업 시 이 폴더만 있으면 전체 이력 복구 가능

---

#### **파일 추가 (Staging)**

```bash
# 특정 파일만 추가
git add Assets/Scripts/GameManager.cs

# 여러 파일 추가
git add Assets/Scripts/*.cs

# 모든 변경사항 추가
git add .

# 변경사항 확인
git status
```

**📌 Staging Area란?**
- 커밋할 파일을 선택하는 중간 단계
- 작업 디렉토리 → Staging Area → 저장소

```
[작업 디렉토리]  --git add-->  [Staging Area]  --git commit-->  [저장소]
```

---

#### **커밋 (Commit)**

```bash
# 기본 커밋
git commit -m "Add GameManager script"

# 상세 커밋 (에디터 사용)
git commit

# 에디터에서 작성:
# Add combat system foundation
#
# - Implement CombatManager class
# - Add player health tracking
# - Create basic turn logic
```

**📌 좋은 커밋 메시지 작성법:**

```bash
# ❌ 나쁜 예시
git commit -m "fix"
git commit -m "update"
git commit -m "asdf"

# ✅ 좋은 예시
git commit -m "Fix: Resolve null reference in CombatManager.EndTurn()"
git commit -m "Add: Implement card draw system"
git commit -m "Update: Improve enemy AI decision making"
```

**커밋 메시지 규칙:**
- **Add**: 새 기능 추가
- **Fix**: 버그 수정
- **Update**: 기존 기능 개선
- **Refactor**: 코드 리팩토링
- **Docs**: 문서 작성/수정
- **Test**: 테스트 코드

---

#### **상태 확인**

```bash
# 현재 상태 확인
git status

# 짧게 보기
git status -s

# 변경 내용 확인
git diff

# Staging된 변경 내용 확인
git diff --staged

# 커밋 이력 확인
git log

# 한 줄로 보기
git log --oneline

# 그래프로 보기
git log --oneline --graph --all
```

---

### 11.2 브랜치 전략 (Git Flow)

#### **브랜치란?**

브랜치는 **독립적인 개발 라인**입니다.

```
main      ─────●─────●─────●─────●
               │           │
feature/card   └─●─●─●─────┘
```

---

#### **Git Flow 브랜치 구조**

```
main (배포용)
 │
 ├─ develop (개발 메인)
 │   │
 │   ├─ feature/combat-system
 │   ├─ feature/card-ui
 │   └─ feature/save-system
 │
 └─ hotfix/critical-bug
```

**브랜치 종류:**

1. **`main`**: 배포 가능한 안정 버전만
2. **`develop`**: 개발 중인 최신 코드
3. **`feature/xxx`**: 새 기능 개발
4. **`hotfix/xxx`**: 긴급 버그 수정
5. **`release/x.x.x`**: 배포 준비

---

#### **브랜치 생성 및 전환**

```bash
# 브랜치 생성
git branch feature/combat-system

# 브랜치 전환
git checkout feature/combat-system

# 생성 + 전환 (한 번에)
git checkout -b feature/combat-system

# 최신 Git (2.23+)
git switch -c feature/combat-system

# 현재 브랜치 확인
git branch
* feature/combat-system
  develop
  main
```

---

#### **브랜치 병합 (Merge)**

```bash
# develop 브랜치로 전환
git checkout develop

# feature 브랜치 병합
git merge feature/combat-system

# 병합 후 브랜치 삭제
git branch -d feature/combat-system
```

**📌 병합 충돌(Conflict) 해결:**

```bash
# 병합 시 충돌 발생
git merge feature/card-ui
# Auto-merging Assets/Scripts/GameManager.cs
# CONFLICT (content): Merge conflict in Assets/Scripts/GameManager.cs

# 충돌 파일 열기 (VS Code)
code Assets/Scripts/GameManager.cs

# 파일 내용:
<<<<<<< HEAD
public int maxHealth = 100;
=======
public int maxHealth = 80;
>>>>>>> feature/card-ui

# 수동으로 선택 또는 수정:
public int maxHealth = 100;  // HEAD 버전 선택

# 충돌 해결 후
git add Assets/Scripts/GameManager.cs
git commit -m "Merge: Resolve conflict in GameManager"
```

---

### 11.3 커밋 메시지 작성 베스트 프랙티스

#### **Conventional Commits 규칙**

```
<타입>(<범위>): <제목>

<본문>

<푸터>
```

**예시:**

```bash
git commit -m "feat(combat): Add mana system

- Implement ManaManager class
- Add mana regeneration per turn
- Create mana cost validation

Closes #42"
```

**타입 (Type):**
- `feat`: 새 기능
- `fix`: 버그 수정
- `docs`: 문서 변경
- `style`: 코드 포맷팅
- `refactor`: 리팩토링
- `test`: 테스트 추가
- `chore`: 빌드/설정 변경

---

### 11.4 실전 워크플로우

#### **시나리오 1: 새 기능 개발**

```bash
# 1. develop 브랜치로 전환
git checkout develop

# 2. 최신 코드 받기
git pull origin develop

# 3. feature 브랜치 생성
git checkout -b feature/deck-shuffle

# 4. 개발 작업
# ... 코드 작성 ...

# 5. 변경사항 확인
git status
git diff

# 6. 커밋
git add .
git commit -m "feat(deck): Implement deck shuffle algorithm"

# 7. 추가 작업 및 커밋
# ... 더 많은 코드 ...
git commit -m "test(deck): Add shuffle unit tests"

# 8. develop에 병합
git checkout develop
git merge feature/deck-shuffle

# 9. 브랜치 삭제
git branch -d feature/deck-shuffle
```

---

#### **시나리오 2: 버그 수정**

```bash
# 1. hotfix 브랜치 생성 (main에서)
git checkout main
git checkout -b hotfix/card-null-error

# 2. 버그 수정
# ... 코드 수정 ...

# 3. 커밋
git commit -m "fix(card): Prevent null reference in CardManager.DrawCard()"

# 4. main에 병합
git checkout main
git merge hotfix/card-null-error

# 5. develop에도 병합
git checkout develop
git merge hotfix/card-null-error

# 6. 브랜치 삭제
git branch -d hotfix/card-null-error
```

---

### 11.5 일반적인 실수와 해결법

#### **실수 1: 잘못된 커밋**

```bash
# 마지막 커밋 메시지 수정
git commit --amend -m "새로운 메시지"

# 마지막 커밋에 파일 추가
git add forgotten_file.cs
git commit --amend --no-edit

# ⚠️ 주의: 이미 push한 커밋은 amend 금지!
```

---

#### **실수 2: 잘못된 브랜치에서 작업**

```bash
# feature 브랜치에서 작업해야 하는데 develop에서 작업함
# 아직 커밋 안 함

# 해결: stash 사용
git stash
git checkout feature/correct-branch
git stash pop
```

---

#### **실수 3: 커밋을 되돌리고 싶을 때**

```bash
# 마지막 커밋 취소 (변경사항 유지)
git reset --soft HEAD~1

# 마지막 커밋 취소 (변경사항 Staging 유지)
git reset --mixed HEAD~1  # 기본값

# 마지막 커밋 완전 취소 (변경사항 삭제)
git reset --hard HEAD~1  # ⚠️ 위험!

# 특정 커밋으로 되돌리기
git reset --hard abc1234
```

**📌 reset vs revert:**

```bash
# reset: 커밋 이력 삭제 (로컬 전용)
git reset --hard HEAD~1

# revert: 새 커밋으로 되돌림 (협업 시 사용)
git revert HEAD
```

---

### 11.6 .gitignore 심화

#### **Unity 프로젝트용 .gitignore**

```gitignore
# Unity generated files
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# Unity Asset Store Tools
/[Aa]ssets/AssetStoreTools*

# Visual Studio cache directory
.vs/

# Gradle cache directory (Android)
.gradle/

# Autogenerated VS/MD/Consulo solution and project files
ExportedObj/
.consulo/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.mdb
*.opendb
*.VC.db

# Unity3D generated meta files
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Unity3D generated file on crash reports
sysinfo.txt

# Builds
*.apk
*.aab
*.unitypackage
*.app

# Crashlytics generated file
crashlytics-build.properties

# Packed Addressables
/[Aa]ssets/[Aa]ddressable[Aa]ssets[Dd]ata/*/*.bin*

# Temporary auto-generated Android Assets
/[Aa]ssets/[Ss]treamingAssets/aa.meta
/[Aa]ssets/[Ss]treamingAssets/aa/*

# OS generated
.DS_Store
.DS_Store?
._*
.Spotlight-V100
.Trashes
ehthumbs.db
Thumbs.db

# Rider
.idea/

# Custom (게임 데이터)
/UserData/
/SaveData/
*.log
```

---

### 11.7 실습 예제

#### **실습 1: 첫 Unity 프로젝트 Git 설정**

```bash
# 1. Unity 프로젝트 폴더로 이동
cd "C:\Projects\MurimDeckBuilder"

# 2. Git 초기화
git init

# 3. .gitignore 생성
# (위의 Unity .gitignore 내용 복사)
notepad .gitignore

# 4. 첫 커밋
git add .
git commit -m "Initial commit: Unity project setup"

# 5. 브랜치 확인
git branch
# * main (또는 master)
```

---

#### **실습 2: Feature 브랜치 워크플로우**

```bash
# 1. develop 브랜치 생성
git checkout -b develop

# 2. feature 브랜치 생성
git checkout -b feature/game-manager

# 3. 파일 생성 및 커밋
# Unity에서 GameManager.cs 생성
git add Assets/Scripts/GameManager.cs
git commit -m "feat(core): Add GameManager singleton"

# 4. 추가 작업
# GameManager에 기능 추가
git add Assets/Scripts/GameManager.cs
git commit -m "feat(core): Add game state management"

# 5. develop에 병합
git checkout develop
git merge feature/game-manager

# 6. 로그 확인
git log --oneline --graph
```

---

#### **실습 3: 충돌 해결**

```bash
# 1. 두 개의 feature 브랜치 생성
git checkout -b feature/health-100
# GameManager.cs 수정: maxHealth = 100
git commit -am "Set max health to 100"

git checkout develop
git checkout -b feature/health-80
# GameManager.cs 수정: maxHealth = 80
git commit -am "Set max health to 80"

# 2. develop에 첫 번째 브랜치 병합
git checkout develop
git merge feature/health-100  # 성공

# 3. 두 번째 브랜치 병합 시도
git merge feature/health-80  # 충돌!

# 4. 충돌 해결
code Assets/Scripts/GameManager.cs
# 수동으로 100으로 결정

# 5. 병합 완료
git add Assets/Scripts/GameManager.cs
git commit -m "Merge: Resolve health value conflict (chose 100)"
```

---

### 11.8 체크리스트

**기본 명령어:**
- [ ] `git init`, `git add`, `git commit` 이해
- [ ] `git status`, `git log` 사용 가능
- [ ] `git diff`로 변경사항 확인 가능

**브랜치:**
- [ ] 브랜치 생성 및 전환 (`git checkout -b`)
- [ ] 브랜치 병합 (`git merge`)
- [ ] Git Flow 구조 이해

**고급:**
- [ ] 커밋 메시지 규칙 적용
- [ ] 충돌 해결 가능
- [ ] `git reset`, `git revert` 차이 이해
- [ ] `.gitignore` 작성 및 적용

---

## Chapter 12: GitHub 협업 전략

> "GitHub는 개발자의 소셜 네트워크입니다."

### 12.1 GitHub 시작하기

#### **GitHub 계정 생성**

1. [https://github.com](https://github.com) 접속
2. Sign up 클릭
3. 이메일, 비밀번호 입력
4. Username 선택 (신중하게!)

**📌 좋은 Username:**
- 전문적인 이름 (실명 또는 별명)
- 짧고 기억하기 쉬움
- 특수문자 최소화

❌ 나쁜 예: `xx_gamer_2005_xx`
✅ 좋은 예: `john-smith`, `devjohn`, `jsmith-dev`

---

#### **SSH 키 설정 (비밀번호 없이 push)**

```bash
# 1. SSH 키 생성
ssh-keygen -t ed25519 -C "your_email@example.com"

# Enter 3번 (기본 경로, 비밀번호 없음)

# 2. SSH 키 복사
cat ~/.ssh/id_ed25519.pub
# 출력된 내용 복사

# Windows라면:
clip < ~/.ssh/id_ed25519.pub
```

**3. GitHub에 SSH 키 등록:**
1. GitHub → Settings → SSH and GPG keys
2. New SSH key 클릭
3. 복사한 키 붙여넣기
4. Add SSH key 클릭

**4. 연결 테스트:**
```bash
ssh -T git@github.com
# Hi username! You've successfully authenticated...
```

---

### 12.2 원격 저장소 연결

#### **시나리오 1: 로컬 프로젝트를 GitHub에 업로드**

```bash
# 1. GitHub에서 새 저장소 생성
# (웹에서) New repository → murim-deckbuilder → Create

# 2. 로컬 프로젝트에서 원격 연결
git remote add origin git@github.com:username/murim-deckbuilder.git

# 3. 브랜치 이름 확인 및 변경 (필요 시)
git branch -M main

# 4. 첫 push
git push -u origin main
# -u: upstream 설정 (다음부터는 git push만 입력)
```

---

#### **시나리오 2: GitHub 프로젝트를 로컬로 복제**

```bash
# 1. 저장소 복제
git clone git@github.com:username/murim-deckbuilder.git

# 2. 폴더 이동
cd murim-deckbuilder

# 3. 원격 확인
git remote -v
# origin  git@github.com:username/murim-deckbuilder.git (fetch)
# origin  git@github.com:username/murim-deckbuilder.git (push)
```

---

#### **원격 저장소 관리**

```bash
# 원격 저장소 확인
git remote -v

# 원격 저장소 추가
git remote add upstream git@github.com:original/repo.git

# 원격 저장소 이름 변경
git remote rename origin github

# 원격 저장소 제거
git remote remove origin

# 원격 저장소 URL 변경
git remote set-url origin git@github.com:new-url.git
```

---

### 12.3 Pull Request (PR) 워크플로우

#### **PR이란?**

Pull Request는 **"내 코드를 메인 브랜치에 병합해달라는 요청"**입니다.

```
feature/card-system (내 브랜치)
  |
  └─── Pull Request ─────> develop (메인 브랜치)
         ↑
     코드 리뷰, 테스트, 논의
```

---

#### **PR 워크플로우 (단계별)**

**Step 1: Feature 브랜치에서 개발**

```bash
# 1. 최신 develop 받기
git checkout develop
git pull origin develop

# 2. feature 브랜치 생성
git checkout -b feature/card-draw-system

# 3. 개발 작업
# ... 코드 작성 ...

# 4. 커밋
git add .
git commit -m "feat(card): Implement card draw system"
```

---

**Step 2: GitHub에 Push**

```bash
# feature 브랜치를 GitHub에 push
git push -u origin feature/card-draw-system
```

---

**Step 3: GitHub에서 PR 생성**

1. GitHub 저장소 페이지 방문
2. "Compare & pull request" 버튼 클릭
3. PR 정보 작성:

```markdown
## Summary
카드 뽑기 시스템 구현

## Changes
- `CardManager.DrawCard()` 메서드 추가
- 덱이 비었을 때 자동 셔플
- 드로우 애니메이션 추가

## Testing
- [ ] 카드 20장 모두 뽑기 테스트
- [ ] 덱 비었을 때 셔플 확인
- [ ] 애니메이션 정상 작동

## Screenshots
![card-draw-demo](https://...)

Closes #42
```

4. Create pull request 클릭

---

**Step 4: 코드 리뷰**

**리뷰어가 할 일:**
- 코드 읽기
- 피드백 남기기
- Approve 또는 Request Changes

**PR 작성자가 할 일:**
- 피드백 반영
- 추가 커밋

```bash
# 피드백 반영 후 추가 커밋
git add .
git commit -m "refactor(card): Use Queue instead of List for deck"
git push
# 자동으로 PR에 반영됨!
```

---

**Step 5: 병합 (Merge)**

PR이 승인되면:
1. "Merge pull request" 클릭
2. 병합 방식 선택:
   - **Merge commit**: 모든 커밋 유지
   - **Squash and merge**: 여러 커밋을 하나로 압축
   - **Rebase and merge**: 선형 히스토리 유지
3. Confirm merge 클릭
4. 브랜치 삭제 (옵션)

---

#### **PR 템플릿 만들기**

`.github/pull_request_template.md` 생성:

```markdown
## Description
<!-- 변경 사항을 간단히 설명하세요 -->

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)

## Testing
<!-- 테스트 방법을 설명하세요 -->

## Checklist
- [ ] 코드가 빌드됩니다
- [ ] 모든 테스트가 통과합니다
- [ ] 코드 스타일 가이드를 따릅니다
- [ ] 문서를 업데이트했습니다 (필요 시)

## Related Issues
Closes #
```

---

### 12.4 이슈 (Issue) 관리

#### **이슈란?**

이슈는 **버그, 기능 요청, 질문 등을 추적하는 티켓**입니다.

---

#### **이슈 생성**

**버그 리포트 예시:**

```markdown
**Title:** Card draw animation freezes the game

**Description:**
게임이 5턴째에 카드를 뽑을 때 1-2초간 멈춥니다.

**Steps to Reproduce:**
1. 게임 시작
2. 5턴까지 진행
3. 카드 뽑기

**Expected Behavior:**
부드럽게 카드가 뽑혀야 함

**Actual Behavior:**
게임이 1-2초 멈춤

**Environment:**
- Unity 2022.3.10f1
- Windows 11
- Build: Development

**Screenshots:**
![freeze](...)

**Additional Context:**
프로파일러를 보니 `Instantiate()` 호출이 느립니다.
```

---

**기능 요청 예시:**

```markdown
**Title:** Add card tooltip on hover

**Description:**
카드에 마우스를 올렸을 때 자세한 설명이 나왔으면 좋겠습니다.

**Proposed Solution:**
- 툴팁 UI 추가
- 카드 효과 상세 설명
- 데미지/방어 계산 공식 표시

**Alternatives:**
- 우클릭으로 상세 정보 보기
- 하단에 고정 패널로 표시

**Priority:**
Medium

**Estimated Effort:**
2-3 days
```

---

#### **이슈 템플릿 만들기**

`.github/ISSUE_TEMPLATE/bug_report.md`:

```markdown
---
name: Bug Report
about: 버그를 발견하셨나요?
title: '[BUG] '
labels: bug
assignees: ''
---

**Describe the bug**
버그에 대한 명확한 설명

**To Reproduce**
재현 방법:
1. Go to '...'
2. Click on '...'
3. See error

**Expected behavior**
예상 동작

**Screenshots**
스크린샷 (있다면)

**Environment:**
 - OS: [e.g. Windows 11]
 - Unity Version: [e.g. 2022.3.10f1]
 - Build Type: [e.g. Development]

**Additional context**
추가 정보
```

---

#### **이슈 라벨 활용**

**추천 라벨:**
- `bug`: 버그
- `feature`: 새 기능
- `enhancement`: 기능 개선
- `documentation`: 문서
- `help wanted`: 도움 필요
- `good first issue`: 초보자 환영
- `priority:high`: 높은 우선순위
- `priority:low`: 낮은 우선순위
- `wontfix`: 수정하지 않음

---

### 12.5 코드 리뷰 가이드

#### **좋은 코드 리뷰 작성법**

**✅ DO:**

```markdown
**명확하고 건설적인 피드백:**

> Line 42: `CardManager.cs`
>
> 현재 `List<Card>`를 사용하고 있는데, 카드를 자주 뽑는다면
> `Queue<Card>`가 더 효율적일 것 같습니다.
>
> ```csharp
> private Queue<Card> deck = new Queue<Card>();
>
> public Card DrawCard() {
>     if (deck.Count == 0) Shuffle();
>     return deck.Dequeue();
> }
> ```
>
> 성능 측정 결과를 공유해주시면 감사하겠습니다!
```

**❌ DON'T:**

```markdown
**모호하고 비판적인 피드백:**

> 이 코드 별로네요. 다시 작성하세요.
```

---

#### **리뷰 체크리스트**

**기능성:**
- [ ] 코드가 의도한 대로 작동하는가?
- [ ] 엣지 케이스가 처리되는가?
- [ ] 버그가 없는가?

**가독성:**
- [ ] 코드가 이해하기 쉬운가?
- [ ] 변수/함수 이름이 명확한가?
- [ ] 주석이 필요한 곳에 있는가?

**성능:**
- [ ] 불필요한 연산이 없는가?
- [ ] 메모리 누수가 없는가?
- [ ] 최적화가 필요한 부분은?

**유지보수:**
- [ ] 코드가 재사용 가능한가?
- [ ] 테스트가 작성되었는가?
- [ ] 문서가 업데이트되었는가?

---

### 12.6 GitHub Actions (CI/CD 기초)

#### **GitHub Actions란?**

코드를 push할 때마다 **자동으로 빌드/테스트를 실행**하는 시스템입니다.

---

#### **Unity 프로젝트용 GitHub Actions**

`.github/workflows/test.yml` 생성:

```yaml
name: Unity Tests

on:
  push:
    branches: [ develop, main ]
  pull_request:
    branches: [ develop ]

jobs:
  test:
    name: Run Unity Tests
    runs-on: ubuntu-latest

    steps:
    # 1. 코드 체크아웃
    - uses: actions/checkout@v3

    # 2. Unity 테스트 실행
    - uses: game-ci/unity-test-runner@v2
      env:
        UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
      with:
        projectPath: .
        testMode: all

    # 3. 테스트 결과 업로드
    - uses: actions/upload-artifact@v3
      if: always()
      with:
        name: Test results
        path: artifacts
```

**설정 방법:**
1. Unity 라이선스 파일 얻기
2. GitHub Secrets에 `UNITY_LICENSE` 추가
3. 파일 커밋 및 push
4. GitHub Actions 탭에서 결과 확인

---

### 12.7 협업 베스트 프랙티스

#### **원칙 1: 작은 PR**

```bash
# ❌ 나쁜 예: 100개 파일 변경
git commit -m "Add entire combat system"

# ✅ 좋은 예: 작은 단위로 나누기
git commit -m "feat(combat): Add CombatManager class"
git commit -m "feat(combat): Add turn system"
git commit -m "feat(combat): Add damage calculation"
```

**이유:**
- 리뷰하기 쉬움
- 버그 찾기 쉬움
- 되돌리기 쉬움

---

#### **원칙 2: 자주 동기화**

```bash
# 매일 아침 develop 동기화
git checkout develop
git pull origin develop

# feature 브랜치 업데이트
git checkout feature/my-feature
git merge develop  # 또는 rebase
```

---

#### **원칙 3: 명확한 커밋 메시지**

```bash
# ❌ 나쁜 예
git commit -m "fix"
git commit -m "update"

# ✅ 좋은 예
git commit -m "fix(combat): Prevent negative health values"
git commit -m "refactor(ui): Extract CardUI component"
```

---

#### **원칙 4: PR에 충분한 설명**

**최소 포함 사항:**
1. 무엇을 변경했는가?
2. 왜 변경했는가?
3. 어떻게 테스트했는가?
4. 스크린샷 (UI 변경 시)

---

### 12.8 실습 예제

#### **실습 1: 첫 GitHub 저장소 생성**

```bash
# 1. GitHub에서 새 저장소 생성
# Repository name: murim-deckbuilder
# Public
# Add README

# 2. 로컬에 복제
git clone git@github.com:username/murim-deckbuilder.git
cd murim-deckbuilder

# 3. Unity 프로젝트 생성
# (Unity Hub에서 생성)

# 4. .gitignore 추가
# (위의 Unity .gitignore 복사)

# 5. 첫 커밋
git add .
git commit -m "Initial commit: Unity project setup"
git push
```

---

#### **실습 2: PR 워크플로우**

```bash
# 1. develop 브랜치 생성
git checkout -b develop
git push -u origin develop

# 2. GitHub에서 develop을 기본 브랜치로 설정
# Settings → Branches → Default branch → develop

# 3. feature 브랜치에서 작업
git checkout -b feature/add-health-bar

# Unity에서 HealthBar.cs 생성
git add Assets/Scripts/UI/HealthBar.cs
git commit -m "feat(ui): Add health bar component"
git push -u origin feature/add-health-bar

# 4. GitHub에서 PR 생성
# base: develop ← compare: feature/add-health-bar
# Title: "Add health bar UI component"
# Description: (템플릿 작성)
# Create pull request

# 5. (코드 리뷰 후) 병합
# Merge pull request → Squash and merge

# 6. 로컬에서 정리
git checkout develop
git pull origin develop
git branch -d feature/add-health-bar
```

---

#### **실습 3: 이슈 및 프로젝트 보드**

```bash
# 1. GitHub Issues에서 새 이슈 생성
# Title: "Implement card tooltip"
# Label: feature, enhancement
# Assign to: yourself
# → 이슈 번호 확인 (예: #5)

# 2. 브랜치 생성 (이슈 번호 포함)
git checkout -b feature/5-card-tooltip

# 3. 개발 및 커밋
git commit -m "feat(ui): Add card tooltip (#5)"

# 4. PR 생성 시 이슈 연결
# Description에 "Closes #5" 추가
# → PR 병합 시 이슈 자동 닫힘
```

---

### 12.9 체크리스트

**GitHub 기본:**
- [ ] GitHub 계정 생성
- [ ] SSH 키 설정
- [ ] 저장소 생성 및 복제
- [ ] README.md 작성

**협업:**
- [ ] 브랜치 전략 이해 (Git Flow)
- [ ] PR 생성 및 병합
- [ ] 이슈 생성 및 관리
- [ ] 코드 리뷰 작성

**고급:**
- [ ] PR 템플릿 작성
- [ ] 이슈 템플릿 작성
- [ ] GitHub Actions 설정 (선택)
- [ ] 프로젝트 보드 활용 (선택)

---

# PART 5: 카드 게임 디자인

## Chapter 13: 덱 빌딩 로그라이크 분석

> "좋은 덱 빌더는 플레이어가 매 판마다 다른 전략을 발견하게 만듭니다."

### 13.1 장르 핵심 요소

#### **덱 빌딩이란?**

덱 빌딩은 **게임 중에 점진적으로 덱을 구축하는 메커니즘**입니다.

```
게임 시작: 10장의 기본 카드
   ↓
전투 승리 → 새 카드 획득
   ↓
상점 → 카드 구매/제거
   ↓
게임 종료: 30-40장의 최적화된 덱
```

**핵심 재미 요소:**
1. **발견의 재미**: 새로운 시너지 조합 발견
2. **최적화의 재미**: 덱을 점점 강하게 만들기
3. **선택의 재미**: 어떤 카드를 가져갈지 결정

---

#### **로그라이크 요소**

**1. 랜덤 생성 (Procedural Generation)**
```
맵 구조 → 랜덤
적 배치 → 랜덤
카드 보상 → 랜덤
이벤트 → 랜덤
```

**2. 영구 죽음 (Permadeath)**
- 죽으면 처음부터 다시 시작
- 하지만 메타 진행은 유지

**3. 메타 진행 (Meta Progression)**
```
1회차: 기본 카드 15장만 사용 가능
2회차: 새 카드 5장 언락
3회차: 새 캐릭터 언락
...
10회차: 모든 콘텐츠 언락
```

**4. 짧은 런타임 (Short Runs)**
- 1회 플레이: 30분 ~ 2시간
- 부담 없이 시작 가능
- "한 판만 더!" 중독성

---

### 13.2 참고 게임 분석

#### **Slay the Spire (2019) - 장르의 정의자**

**핵심 메커니즘:**

1. **3개의 캐릭터**
   - 아이언클래드 (전사): 공격 중심
   - 사일런트 (도적): 독/약화 중심
   - 디펙트 (로봇): 오브 시스템

2. **카드 시스템**
   - 기본 덱: 5 공격 + 5 방어
   - 매 전투 후 신규 카드 획득
   - 카드 업그레이드 (모닥불)
   - 카드 제거 (상점)

3. **유물 시스템**
   - 엘리트 처치 시 획득
   - 강력한 패시브 효과
   - 시너지 중심 빌드

4. **맵 시스템**
   - 3층 구조 (각 층 15-17개 방)
   - 경로 선택의 중요성
   - 엘리트 vs 안전 경로 트레이드오프

**배울 점:**
- ✅ **깊이 있는 시너지**: 카드 간 조합이 핵심
- ✅ **유물의 영향력**: 빌드를 완전히 바꿀 수 있음
- ✅ **선택의 무게**: 모든 선택이 중요함

---

#### **Monster Train (2020) - 멀티 레인 혁신**

**핵심 메커니즘:**

1. **3층 기차 구조**
   - 각 층에 유닛 배치
   - 적이 1층부터 올라옴
   - 공간 전략의 중요성

2. **듀얼 클랜 시스템**
   - 주 클랜 + 서브 클랜
   - 256가지 조합 (8 × 8)
   - 엄청난 리플레이 가치

3. **유닛 업그레이드**
   - 카드뿐 아니라 유닛도 업그레이드
   - 챔피언 육성 시스템
   - 한 유닛을 극한까지 강화

**배울 점:**
- ✅ **공간 전략**: 2D 레이아웃 활용
- ✅ **깊이 있는 커스터마이징**: 업그레이드 경로
- ✅ **난이도 조절**: Covenant 시스템

---

#### **Inscryption (2021) - 스토리텔링**

**핵심 메커니즘:**

1. **독특한 분위기**
   - 공포 + 덱 빌딩
   - 메타 스토리
   - 탈출 방 요소

2. **혁신적인 카드 시스템**
   - 카드를 희생해서 강력한 카드 소환
   - 카드에 눈이 달림 (공포 요소)
   - 카드 합성 시스템

**배울 점:**
- ✅ **독특한 테마**: 장르의 고정관념 탈피
- ✅ **스토리 통합**: 게임플레이와 스토리 융합

---

### 13.3 강호무적 차별화 전략

#### **1. 무협 테마 (기존 게임과 차별화)**

**대부분의 덱 빌더:**
- 판타지 (Slay the Spire)
- SF (Griftlands)
- 공포 (Inscryption)

**강호무적:**
- **무협 세계관** (무림, 강호, 무공)
- 한국/중국 플레이어에게 친숙
- 서양 플레이어에게는 새로운 경험

---

#### **2. 내공 / 무기술 이원화 시스템**

**기존 게임: 에너지 단일 시스템**
```
에너지 3 → 카드 3장 사용
```

**강호무적: 이원화**
```
내공 2 + 무기술 2
  ↓
내공 카드: 방어/회복/버프
무기술 카드: 공격/디버프
```

**전략적 깊이:**
- 내공만 많아도 안 됨 (공격 못함)
- 무기술만 많아도 안 됨 (방어 못함)
- **균형**이 중요

**예시:**
```
[턴 시작]
내공: 2 / 무기술: 2

[손패]
- 금강불괴 (내공 1): 방어 8
- 일검 (무기술 1): 공격 6
- 쌍검난무 (무기술 2): 공격 12
- 내공회복 (내공 1): 내공 +1

[선택 1: 공격 중심]
1. 쌍검난무 (무기술 2) → 공격 12
2. 금강불괴 (내공 1) → 방어 8
→ 남은 자원: 내공 1, 무기술 0

[선택 2: 내공 충전]
1. 내공회복 (내공 1) → 내공 +1
2. 금강불괴 (내공 1) → 방어 8
3. 일검 (무기술 1) → 공격 6
→ 다음 턴 내공 풍부
```

---

#### **3. 경지 시스템 (카드 진화)**

**기존 게임: 카드 업그레이드 (1단계)**
```
타격 → 타격+ (6 → 9 피해)
```

**강호무적: 경지 시스템 (3단계)**
```
입문 → 소성 → 대성

예시: 일검
- 입문: 공격 6
- 소성: 공격 8, 연타 가능
- 대성: 공격 12, 연타 2회, 출혈 추가
```

**획득 방법:**
- 무공 수련 (이벤트)
- 비급 획득 (엘리트 처치)
- 사부 조우 (특별 이벤트)

---

#### **4. 무공 정수 (메타 진행)**

**기존 게임: 단순 언락**
```
카드 언락
유물 언락
```

**강호무적: 무공 정수 시스템**
```
[매 판마다 무공 정수 획득]

1판 실패: 무공 정수 +50
2판 1층 클리어: 무공 정수 +100
3판 2층 클리어: 무공 정수 +200
...

[무공 정수 사용]
- 신규 무공 언락 (100 정수)
- 초기 덱 개선 (50 정수)
- 시작 유물 언락 (200 정수)
- 특수 이벤트 언락 (300 정수)
```

**진행감:**
- 죽어도 무공 정수는 유지
- 매 판마다 조금씩 강해짐
- 점점 깊은 콘텐츠 경험

---

### 13.4 핵심 루프 설계

#### **전투 루프 (Micro Loop)**

```
[1턴]
1. 카드 5장 드로우
2. 내공/무기술 충전 (각 3)
3. 카드 플레이
4. 적 행동
5. 턴 종료 (버리기 더미로)

[2턴]
반복...
```

**전투 목표:**
- 적 처치
- 체력 최대한 유지
- 덱 활용도 테스트

---

#### **던전 루프 (Macro Loop)**

```
[1층 - 입문자 지역]
전투 → 전투 → 보상 (카드)
   ↓
휴식 (회복 or 수련)
   ↓
상점 (카드 구매/제거)
   ↓
엘리트 전투 (유물 획득)
   ↓
보스 전투 (1층 클리어!)
   ↓
[2층 - 고수 지역]
...
```

**던전 목표:**
- 보스 처치
- 덱 최적화
- 유물 수집

---

#### **메타 루프 (Long-term Loop)**

```
[1판]
사망 → 무공 정수 +100
   ↓
신규 카드 언락
   ↓
[2판]
더 깊이 진행 → 무공 정수 +300
   ↓
시작 유물 언락
   ↓
[3판]
보스 처치 → 무공 정수 +1000
   ↓
새로운 캐릭터 언락!
```

---

### 13.5 성공적인 덱 빌더의 7가지 원칙

#### **원칙 1: 명확한 시너지**

```
❌ 나쁜 예:
- 카드들이 각자 독립적
- 조합해도 별 이득 없음

✅ 좋은 예:
- 출혈 부여 카드 + 출혈 피해 증폭 카드
- 약화 카드 + 약화된 적 추가 피해 카드
```

---

#### **원칙 2: 리스크 vs 리워드**

```
[안전 경로]
일반 전투 × 5 → 카드 5장

[위험 경로]
엘리트 전투 × 3 → 유물 3개 (체력 감소)
```

**플레이어는 항상 선택해야 함:**
- 안전하게 갈까?
- 위험을 감수할까?

---

#### **원칙 3: 덱 씨닝 (Thinning)**

```
[게임 초반]
덱 30장 (기본 카드 많음)
 → 원하는 카드 뽑기 어려움

[게임 후반]
덱 20장 (약한 카드 제거)
 → 강력한 카드만 순환
```

**메커니즘:**
- 상점에서 카드 제거 (골드 사용)
- 이벤트로 카드 제거 (대가 필요)
- 유물로 자동 제거

---

#### **원칙 4: 난이도 곡선**

```
1층: 쉬움 (덱 실험)
2층: 중간 (전략 확정)
3층: 어려움 (최적화 필수)
보스: 매우 어려움 (완성된 덱 테스트)
```

---

#### **원칙 5: 유의미한 선택**

```
❌ 나쁜 선택:
A: 공격 6
B: 공격 7 (명백히 B가 나음)

✅ 좋은 선택:
A: 공격 10 (단일)
B: 공격 6 (전체)
→ 상황에 따라 다름!
```

---

#### **원칙 6: 정보 제공**

```
[적의 의도 표시]
산적: 다음 턴 공격 12
 → 플레이어는 방어 준비 가능

[카드 정보 명확화]
❌ "적에게 피해를 입힌다" (얼마?)
✅ "적에게 6의 피해를 입힌다"
```

---

#### **원칙 7: 예상 외의 조합**

```
[디자이너 의도]
카드 A + 카드 B = 시너지

[플레이어 발견]
카드 A + 카드 C + 유물 D = 무한 콤보!
```

**허용할 것:**
- 창의적인 조합
- 강력한 빌드

**제한할 것:**
- 너무 쉬운 무한 콤보
- 게임을 지루하게 만드는 조합

---

### 13.6 체크리스트

**장르 이해:**
- [ ] 덱 빌딩 vs 덱 구축 차이 이해
- [ ] 로그라이크 핵심 요소 파악
- [ ] 메타 진행의 중요성 인지

**참고 게임:**
- [ ] Slay the Spire 플레이 (최소 5시간)
- [ ] 다른 덱 빌더 1개 이상 플레이
- [ ] 왜 재밌는지 분석

**차별화:**
- [ ] 무협 테마 이해
- [ ] 내공/무기술 시스템 설계
- [ ] 경지 시스템 구조 파악
- [ ] 무공 정수 메타 진행 계획

**핵심 원칙:**
- [ ] 7가지 원칙 숙지
- [ ] 자신의 게임에 적용 계획

---

## Chapter 14: 게임 메커니즘 설계

> "카드는 도구입니다. 중요한 건 그 도구로 무엇을 할 수 있는가입니다."

### 14.1 카드 타입 체계

#### **주요 카드 타입**

**1. 공격 카드 (무기술)**

```
[기본 구조]
- 에너지 비용: 무기술 1-3
- 주 효과: 피해
- 부가 효과: 디버프, 출혈, 기절 등

[예시]
일검 (무기술 1)
- 적에게 6의 피해를 입힌다.

쌍검난무 (무기술 2)
- 적에게 4의 피해를 2회 입힌다.

패검(覇劍) (무기술 3)
- 적에게 18의 피해를 입힌다.
- 출혈 3을 부여한다.
```

---

**2. 방어 카드 (내공)**

```
[기본 구조]
- 에너지 비용: 내공 1-2
- 주 효과: 보호막
- 부가 효과: 회복, 버프 등

[예시]
금강불괴 (내공 1)
- 보호막 8을 얻는다.

철벽진 (내공 2)
- 보호막 14를 얻는다.
- 다음 턴까지 유지.

무적금강 (내공 3)
- 이번 턴 모든 피해 무효.
```

---

**3. 기술 카드 (혼합)**

```
[기본 구조]
- 에너지 비용: 다양
- 주 효과: 유틸리티
- 부가 효과: 드로우, 에너지 조작, 버프

[예시]
내공축적 (내공 1)
- 카드를 2장 뽑는다.

검기폭발 (무기술 2 + 내공 1)
- 모든 적에게 8의 피해.
- 보호막 5.

태극권 (내공 0)
- 이번 턴 받는 피해만큼
  다음 턴 공격력 증가.
```

---

**4. 저주 / 상태 카드 (비전투)**

```
[저주]
부상 (비용 없음)
- 플레이 불가.
- 버릴 수 없음.
- 정화 필요.

[상태]
통찰 (소모품)
- 카드 3장 뽑기.
- 소모.
```

---

### 14.2 카드 속성 설계

#### **카드 데이터 구조**

```csharp
[System.Serializable]
public class CardData
{
    // 기본 정보
    public string id;              // "card_001"
    public string nameKR;          // "일검"
    public string nameEN;          // "Single Strike"

    // 타입 및 레어도
    public CardType type;          // Attack, Defend, Skill
    public Rarity rarity;          // Common, Rare, Epic, Legendary

    // 비용
    public int qiCost;             // 내공 비용
    public int martialCost;        // 무기술 비용

    // 효과
    public int damage;             // 피해량
    public int block;              // 방어량
    public List<CardEffect> effects; // 추가 효과

    // UI
    public string description;     // 설명
    public Sprite artwork;         // 일러스트
    public Sprite icon;            // 아이콘

    // 경지 시스템
    public MasteryLevel mastery;   // 입문, 소성, 대성
    public bool isUpgradeable;     // 업그레이드 가능 여부

    // 키워드
    public List<Keyword> keywords; // Bleed, Weak, Vulnerable
}
```

---

#### **카드 효과 시스템**

```csharp
[System.Serializable]
public class CardEffect
{
    public EffectType type;        // Buff, Debuff, Draw, Energy
    public EffectTarget target;    // Self, Enemy, AllEnemies
    public int value;              // 효과 수치
    public int duration;           // 지속 턴 (0 = 즉시)
}

// 예시
{
    type: EffectType.Bleed,
    target: EffectTarget.Enemy,
    value: 3,
    duration: 3
}
// → "적에게 출혈 3을 3턴간 부여"
```

---

### 14.3 키워드 시스템

#### **핵심 키워드 정의**

**1. 공격 키워드**

```
[출혈 (Bleed)]
- 매 턴 종료 시 피해
- 중첩 가능
- 예: 출혈 5 → 5턴간 매 턴 5 피해

[중독 (Poison)]
- 매 턴 종료 시 피해, 매 턴 1 감소
- 중첩 가능
- 예: 중독 10 → 10+9+8+...+1 = 총 55 피해

[화상 (Burn)]
- 이번 턴 종료 시 피해
- 중첩 가능
```

---

**2. 방어 키워드**

```
[보호막 (Block)]
- 다음 피해 흡수
- 턴 종료 시 사라짐 (기본)
- 일부 카드는 지속

[회피 (Dodge)]
- 다음 공격 무효화
- 1회용
```

---

**3. 버프 키워드**

```
[힘 (Strength)]
- 공격 카드 피해 +X
- 영구 (전투 끝까지)
- 중첩 가능

[민첩 (Dexterity)]
- 방어 카드 보호막 +X
- 영구
- 중첩 가능

[집중 (Focus)]
- 카드 드로우 +1
- 지속 턴 제한
```

---

**4. 디버프 키워드**

```
[약화 (Weak)]
- 공격 피해 25% 감소
- 2-3턴 지속

[취약 (Vulnerable)]
- 받는 피해 50% 증가
- 2-3턴 지속

[연약 (Frail)]
- 보호막 25% 감소
- 2-3턴 지속
```

---

### 14.4 시너지 설계

#### **시너지 유형 1: 키워드 시너지**

```
[출혈 빌드]

카드 A: "일검" (무기술 1)
→ 공격 6, 출혈 1

카드 B: "연환검" (무기술 2)
→ 공격 4 × 3회, 각 공격마다 출혈 1

카드 C: "혈검난무" (무기술 3)
→ 공격 20
→ 적이 출혈 상태면 공격력 2배

유물: "혈마검"
→ 출혈 피해 +50%

[시너지 결과]
1. 연환검 → 출혈 3 부여
2. 혈검난무 → 40 피해 (출혈 상태 보너스)
3. 매 턴 출혈 피해 3 × 1.5 (유물) = 4.5
```

---

#### **시너지 유형 2: 무공 파벌 시너지**

```
[검술 빌드]

검술 카드 3장 이상 → 특별 보너스

카드 A: "기본검술" (무기술 1)
→ 공격 6
→ 검술

카드 B: "고급검술" (무기술 2)
→ 공격 10
→ 검술
→ 손에 검술 카드 3장 이상: 비용 -1

카드 C: "검황비검" (무기술 3)
→ 공격 20
→ 검술
→ 이번 턴 사용한 검술 카드 1장당 피해 +5

유물: "검성의 비급"
→ 전투 시작 시 검술 카드 +1 드로우

[시너지 결과]
1턴: 검술 4장 손에 → 고급검술 무기술 1로 사용 가능
2턴: 검술 3장 사용 후 검황비검 → 20 + (3×5) = 35 피해
```

---

#### **시너지 유형 3: 경지 시너지**

```
[대성 무공 빌드]

카드 A: "일검" (입문)
→ 공격 6

카드 A+: "일검" (소성)
→ 공격 8
→ 대성 무공이 손에 있으면: 공격 +4

카드 B: "천검" (대성)
→ 공격 15
→ 모든 아군 공격 카드 피해 +3

유물: "고수의 깨달음"
→ 소성 카드가 대성 카드를 드로우

[시너지 결과]
일검(소성) + 천검(대성) 조합
→ 일검: 8 + 4 (시너지) + 3 (유물) = 15
→ 천검: 15 + 3 (유물) = 18
→ 총 33 피해 (에너지 4)
```

---

### 14.5 카드 밸런싱 기초

#### **DPE (Damage Per Energy) 기준**

```
[표준 DPE]
일반: 5-6 DPE
희귀: 6-7 DPE
영웅: 7-8 DPE
전설: 8-10 DPE

[예시]
일검 (무기술 1): 6 피해
→ DPE = 6/1 = 6 (표준)

쌍검난무 (무기술 2): 12 피해
→ DPE = 12/2 = 6 (표준)

패검 (무기술 3): 20 피해
→ DPE = 20/3 = 6.67 (약간 높음, 희귀 등급)
```

---

#### **조건부 카드 밸런싱**

```
[기본 원칙]
조건이 어려울수록 → 효과가 강해짐

[예시 1: 단순 조건]
카드: "반격" (내공 1)
→ 이번 턴 공격받으면: 공격 10
→ DPE = 10/1 = 10 (매우 높음)
→ 조건: 공격받아야 함 (쉬움)
→ 밸런스 OK

[예시 2: 어려운 조건]
카드: "절대검역" (무기술 3)
→ 손에 검술 5장 이상: 공격 50
→ DPE = 50/3 = 16.67 (극도로 높음)
→ 조건: 검술 5장 (매우 어려움)
→ 밸런스 OK (빌드 아라운드 카드)
```

---

### 14.6 카드 디자인 실습

#### **실습 1: 기본 공격 카드 만들기**

```
[요구사항]
- 무기술 1 비용
- 일반 등급
- 6 DPE 목표

[디자인 A: 단순]
일검
- 무기술 1
- 공격 6
→ DPE = 6 ✅

[디자인 B: 효과 추가]
독검
- 무기술 1
- 공격 4
- 중독 2
→ DPE = (4 + 2×2) / 1 = 8 (약간 높음)
→ 조정: 중독 1로 변경
→ DPE = (4 + 2) / 1 = 6 ✅
```

---

#### **실습 2: 시너지 카드 만들기**

```
[목표]
출혈 빌드를 위한 피니셔 카드

[1단계: 컨셉]
- 출혈 상태의 적에게 강력한 공격
- 무기술 2-3
- 희귀 등급

[2단계: 초안]
혈검난무
- 무기술 3
- 공격 15
- 적이 출혈 상태면: 공격 2배

[3단계: 밸런싱]
조건 없음: DPE = 15/3 = 5 (낮음)
조건 있음: DPE = 30/3 = 10 (높음)
→ 밸런스: 출혈은 비교적 쉬운 조건
→ 조정: 공격 12, 2배 → 24
→ DPE = 12/3 = 4 (기본)
→ DPE = 24/3 = 8 (조건부, 희귀 등급 적절)

[최종]
혈검난무 (희귀)
- 무기술 3
- 적에게 12의 피해.
- 출혈 상태면: 피해 2배.
```

---

### 14.7 체크리스트

**카드 타입:**
- [ ] 공격/방어/기술 카드 이해
- [ ] 각 타입의 역할 파악
- [ ] 균형잡힌 비율 설계 (4:3:3)

**카드 속성:**
- [ ] CardData 구조 설계
- [ ] 효과 시스템 구현 계획
- [ ] UI 표시 방법 결정

**키워드:**
- [ ] 핵심 키워드 10개 선정
- [ ] 키워드 정의 문서 작성
- [ ] 중첩 규칙 정의

**시너지:**
- [ ] 3가지 시너지 유형 설계
- [ ] 각 유형별 카드 5장 기획
- [ ] 유물 시너지 계획

**밸런싱:**
- [ ] DPE 기준 설정
- [ ] 레어도별 기준 정의
- [ ] 조건부 카드 공식 수립

---

## Chapter 15: 밸런싱 방법론

> "밸런싱은 과학이자 예술입니다. 수치와 플레이테스트의 조화가 필요합니다."

### 15.1 DPE / BPE 계산법

#### **DPE (Damage Per Energy) - 공격 효율**

```
DPE = 총 피해량 / 에너지 비용

[기본 예시]
일검 (무기술 1): 6 피해
→ DPE = 6/1 = 6

쌍검난무 (무기술 2): 12 피해
→ DPE = 12/2 = 6
```

**표준 DPE 기준 (Slay the Spire 분석):**
```
일반 (Common):    5-6 DPE
희귀 (Rare):      6-7 DPE
영웅 (Epic):      7-8 DPE
전설 (Legendary): 8-10 DPE
```

---

#### **조건부 DPE 계산**

```
[예시: 연환검]
무기술 2
공격 5 × 2회 = 10 피해
→ 기본 DPE = 10/2 = 5 (낮음)

BUT: 연타 효과는 추가 가치 있음
- 약화 적용 2회
- 유물 발동 2회
- 출혈 부여 2회

→ 실제 가치 = 5 + (보너스 2) = 7 DPE 상당
```

---

#### **BPE (Block Per Energy) - 방어 효율**

```
BPE = 총 방어량 / 에너지 비용

[Slay the Spire 기준]
Defend (에너지 1): 보호막 5
→ BPE = 5/1 = 5

Defend+ (에너지 1): 보호막 8
→ BPE = 8/1 = 8
```

**표준 BPE 기준:**
```
일반: 4-5 BPE
희귀: 5-7 BPE
영웅: 7-9 BPE

※ BPE는 DPE보다 약간 낮음
이유: 방어는 체력 보존 = 장기전 유리
```

---

### 15.2 레어도별 밸런싱

#### **일반 (Common) 카드**

**특징:**
- 덱의 기본 구성
- 단순하고 명확한 효과
- 시너지 요구 낮음

**밸런싱 기준:**
```
DPE: 5-6
BPE: 4-5
비용: 1-2

[예시]
일검 (무기술 1)
- 공격 6
→ DPE = 6/1 = 6 ✅

금강불괴 (내공 1)
- 보호막 5
→ BPE = 5/1 = 5 ✅
```

---

#### **희귀 (Rare) 카드**

**특징:**
- 약간의 시너지 필요
- 추가 효과 1-2개
- 빌드의 중심

**밸런싱 기준:**
```
DPE: 6-7
BPE: 5-7
비용: 2-3

[예시]
혈검 (무기술 2) - 희귀
- 공격 8
- 출혈 2
→ DPE = (8 + 4) / 2 = 6 (기본)
→ 출혈 지속피해 고려 시 약 7 DPE

철벽진 (내공 2) - 희귀
- 보호막 14
- 다음 턴까지 유지
→ BPE = 14/2 = 7
→ 지속 효과로 실제 가치 더 높음
```

---

#### **영웅 (Epic) 카드**

**특징:**
- 빌드 전환점
- 강력한 조건부 효과
- 2-3개 효과 조합

**밸런싱 기준:**
```
DPE: 7-8 (조건부 시 10+)
BPE: 7-9
비용: 2-4

[예시]
검황비검 (무기술 3) - 영웅
- 공격 15
- 이번 턴 사용한 검술 카드 1장당 +5 피해
→ 기본 DPE = 15/3 = 5
→ 검술 3장 사용 후: (15+15)/3 = 10 DPE

무적금강 (내공 3) - 영웅
- 이번 턴 피해 무효
- 다음 턴 힘 +5
→ 방어 = 무한 (조건적)
→ 공격 버프 제공
```

---

#### **전설 (Legendary) 카드**

**특징:**
- 게임 체인저
- 빌드 아라운드
- 독특한 메커니즘

**밸런싱 기준:**
```
DPE: 8-12 (조건부)
빌드 의존도: 매우 높음
비용: 3-5

[예시]
천마해체대법 (내공 4 + 무기술 3) - 전설
- 모든 적에게 50 피해
- 모든 카드를 소모
- 전투 종료 시 체력 50% 감소
→ DPE = 50/7 = 7.14 (기본)
→ BUT: 전체 공격 + 엄청난 피해
→ 대가: 카드 소모 + 체력 감소
→ 피니셔 카드로 밸런스 맞음

구음진경 (내공 0) - 전설
- 매 턴 내공 +1 (영구)
- 내공 카드 비용 -1
→ 에너지 생성 카드 (매우 강력)
→ 획득 난이도 높음으로 밸런스
```

---

### 15.3 밸런싱 스프레드시트

#### **Google Sheets 구조**

```
[시트 1: 카드 데이터]

| ID | 이름 | 타입 | 레어도 | 내공 | 무기술 | 피해 | 방어 | 효과 | DPE | BPE | 밸런스 |
|----|------|------|--------|------|--------|------|------|------|-----|-----|--------|
| 001| 일검 |공격|일반| 0 | 1 | 6 | 0 | - | 6.0 | - | OK ✅ |
| 002| 금강|방어|일반| 1 | 0 | 0 | 5 | - | - | 5.0 | OK ✅ |
| 003| 혈검|공격|희귀| 0 | 2 | 8 |0|출혈2| 6.0| - |OK✅|
```

**자동 계산 수식:**
```
DPE = IF(무기술>0, (피해+효과피해)/무기술, "-")
BPE = IF(내공>0, 방어/내공, "-")

밸런스 = IF(AND(DPE>=레어도기준MIN, DPE<=레어도기준MAX), "OK✅", "조정필요⚠️")
```

---

#### **레어도별 기준 시트**

```
[시트 2: 밸런스 기준]

| 레어도 | DPE MIN | DPE MAX | BPE MIN | BPE MAX |
|--------|---------|---------|---------|---------|
| 일반   | 5.0     | 6.0     | 4.0     | 5.0     |
| 희귀   | 6.0     | 7.5     | 5.0     | 7.0     |
| 영웅   | 7.0     | 9.0     | 7.0     | 9.0     |
| 전설   | 8.0     | 12.0    | -       | -       |
```

---

#### **효과 가치 환산표**

```
[시트 3: 효과 가치]

| 효과 | 가치 (피해 환산) | 비고 |
|------|------------------|------|
| 출혈 1 (3턴) | +2 피해 | 지속피해 |
| 중독 5 | +15 피해 | 5+4+3+2+1 |
| 약화 2턴 | +4 피해 | 적 공격 25% 감소 |
| 취약 2턴 | +5 피해 | 받는 피해 50% 증가 |
| 카드 드로우 1 | +3 가치 | 핸드 어드밴티지 |
| 에너지 +1 | +5 가치 | 매우 강력 |
```

**활용 예시:**
```
혈검 (무기술 2)
- 공격 8
- 출혈 2 (×3턴)

피해 환산 = 8 + (2 × 2) = 12
DPE = 12 / 2 = 6 ✅
```

---

### 15.4 플레이테스트 기반 밸런싱

#### **밸런싱 프로세스**

```
[1단계: 이론 설계]
→ 스프레드시트로 DPE/BPE 계산
→ 수치상 밸런스 맞춤

[2단계: 내부 테스트]
→ 개발자가 10판 플레이
→ 너무 강한 카드 / 약한 카드 파악

[3단계: 조정]
→ 사용률 높은 카드: 너프
→ 사용률 낮은 카드: 버프

[4단계: 외부 테스트]
→ 외부 플레이테스터 20명
→ 데이터 수집 (50판 이상)

[5단계: 최종 조정]
→ 통계 분석
→ 패치노트 작성
→ 업데이트
```

---

#### **데이터 수집 항목**

```
[카드 사용률]
카드 A: 80% 게임에서 선택 → 너무 강함
카드 B: 10% 게임에서 선택 → 너무 약함

[승률 영향]
카드 A 포함 덱: 승률 75%
카드 B 포함 덱: 승률 30%

[평균 피해량]
카드 A: 평균 12 피해/사용
카드 B: 평균 4 피해/사용

[선호도]
플레이테스터 설문: "가장 재밌는 카드?"
→ 재미 vs 밸런스 고려
```

---

### 15.5 실전 밸런싱 예시

#### **사례 1: 과도하게 강한 카드**

```
[문제]
무한검 (무기술 1) - 일반
- 공격 10
- 카드 드로우 1

DPE = (10 + 3) / 1 = 13 (너무 높음!)
→ 모든 게임에서 픽률 95%

[해결책 A: 너프]
→ 공격 6으로 감소
→ DPE = 9 (여전히 높지만 합리적)

[해결책 B: 비용 증가]
→ 무기술 2로 변경
→ DPE = 13/2 = 6.5 (적절)

[해결책 C: 레어도 상승]
→ 희귀 등급으로 변경
→ 획득 난이도 증가
```

---

#### **사례 2: 사용률 낮은 카드**

```
[문제]
약한일격 (무기술 2) - 일반
- 공격 6
- 적에게 약화 1

DPE = (6 + 2) / 2 = 4 (너무 낮음)
→ 픽률 5%

[해결책 A: 피해 증가]
→ 공격 10
→ DPE = 6 ✅

[해결책 B: 효과 강화]
→ 약화 2로 변경
→ DPE = (6 + 4) / 2 = 5 (개선)

[해결책 C: 비용 감소]
→ 무기술 1로 변경
→ DPE = 8 (일반 등급 상한)
```

---

### 15.6 특수 케이스 밸런싱

#### **무한 콤보 방지**

```
[문제 조합]
카드 A: "에너지 충전" (내공 0)
→ 무기술 +2

카드 B: "연환격" (무기술 1)
→ 공격 4
→ 카드 드로우 1

유물: "무한의 팔찌"
→ 카드 드로우 시 내공 +1

[무한 콤보]
1. 에너지 충전 → 무기술 +2
2. 연환격 (무기술 1) → 카드 드로우
3. 드로우로 에너지 충전 다시 뽑기
4. 반복 → 무한 피해

[해결책]
A: 에너지 충전에 "소모" 추가
B: 연환격 "턴당 1회만"
C: 유물 "턴당 3회 제한"
```

---

#### **시너지 과보상 방지**

```
[문제]
출혈 카드 5장 + 출혈 증폭 유물 3개
→ 매 턴 출혈 100+ 피해

[해결책]
- 출혈 상한선 설정 (최대 50)
- 유물 효과 중첩 감소 (50% → 25%)
- 출혈 카드 획득 빈도 조절
```

---

### 15.7 체크리스트

**DPE/BPE:**
- [ ] 모든 카드 DPE/BPE 계산
- [ ] 레어도별 기준 준수
- [ ] 조건부 효과 가치 환산

**스프레드시트:**
- [ ] Google Sheets 템플릿 생성
- [ ] 자동 계산 수식 적용
- [ ] 효과 가치 환산표 작성

**플레이테스트:**
- [ ] 내부 테스트 10판 이상
- [ ] 사용률/승률 데이터 수집
- [ ] 조정 필요 카드 리스트업

**특수 케이스:**
- [ ] 무한 콤보 가능성 체크
- [ ] 시너지 과보상 확인
- [ ] 상한선 설정 (필요 시)

---

# PART 6: 데이터 관리

## Chapter 16: JSON 데이터 구조 설계

> "좋은 데이터 구조는 버그의 90%를 예방합니다."

### 16.1 JSON vs ScriptableObject

#### **데이터 저장 방식 비교**

**1. JSON (추천)**

```
장점:
✅ 외부 도구(Google Sheets, Excel)와 연동 쉬움
✅ 버전 관리 용이 (텍스트 파일)
✅ 런타임 수정 가능
✅ 크로스 플랫폼

단점:
❌ Unity 에디터에서 직접 편집 불편
❌ 파싱 비용
❌ 타입 안정성 낮음
```

**2. ScriptableObject**

```
장점:
✅ Unity 에디터 통합
✅ 타입 안정성
✅ 참조 관리 쉬움
✅ 즉시 로딩

단점:
❌ 외부 도구 연동 어려움
❌ 버전 관리 어려움 (바이너리)
❌ 런타임 수정 불가
```

**권장 방식:**

```
개발 단계: JSON (밸런싱 작업 많음)
   ↓
배포 단계: ScriptableObject (성능 최적화)
```

---

### 16.2 카드 데이터 JSON 완전 구조

#### **카드 데이터 스키마**

```json
{
  "version": "1.0.0",
  "lastUpdated": "2025-11-23",
  "cards": [
    {
      "id": "card_001",
      "nameKR": "일검",
      "nameEN": "Single Strike",
      "type": "Attack",
      "rarity": "Common",

      "costs": {
        "qi": 0,
        "martial": 1
      },

      "effects": {
        "damage": 6,
        "block": 0,
        "additional": []
      },

      "keywords": [],

      "mastery": {
        "current": "Beginner",
        "upgradePath": ["Beginner", "Proficient", "Master"]
      },

      "ui": {
        "description": "적에게 6의 피해를 입힌다.",
        "artworkPath": "Assets/Art/Cards/card_001.png",
        "iconPath": "Assets/Art/Icons/sword.png"
      },

      "gameplay": {
        "isPlayable": true,
        "isExhaustable": false,
        "isEthereal": false,
        "maxCopiesInDeck": 3
      },

      "metadata": {
        "designer": "MinJae",
        "createdDate": "2025-11-01",
        "balanceNotes": "Standard 1-cost attack"
      }
    },

    {
      "id": "card_002",
      "nameKR": "혈검",
      "nameEN": "Blood Blade",
      "type": "Attack",
      "rarity": "Rare",

      "costs": {
        "qi": 0,
        "martial": 2
      },

      "effects": {
        "damage": 8,
        "block": 0,
        "additional": [
          {
            "type": "Bleed",
            "value": 2,
            "target": "Enemy",
            "duration": 3
          }
        ]
      },

      "keywords": ["Bleed"],

      "mastery": {
        "current": "Beginner",
        "upgradePath": ["Beginner", "Proficient", "Master"],
        "proficientBonus": {
          "damage": 10,
          "bleed": 3
        },
        "masterBonus": {
          "damage": 14,
          "bleed": 5,
          "newEffect": "MultiHit"
        }
      },

      "ui": {
        "description": "적에게 8의 피해를 입히고 출혈 2를 부여한다.",
        "artworkPath": "Assets/Art/Cards/card_002.png",
        "iconPath": "Assets/Art/Icons/blood_sword.png"
      },

      "gameplay": {
        "isPlayable": true,
        "isExhaustable": false,
        "isEthereal": false,
        "maxCopiesInDeck": 2
      },

      "synergies": ["Bleed", "Sword"],

      "metadata": {
        "designer": "MinJae",
        "createdDate": "2025-11-05",
        "balanceNotes": "Bleed archetype centerpiece"
      }
    }
  ]
}
```

---

#### **C# 데이터 클래스**

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardDatabase
{
    public string version;
    public string lastUpdated;
    public List<CardData> cards;
}

[Serializable]
public class CardData
{
    // 기본 정보
    public string id;
    public string nameKR;
    public string nameEN;
    public CardType type;
    public Rarity rarity;

    // 비용
    public EnergyCost costs;

    // 효과
    public CardEffects effects;

    // 키워드
    public List<string> keywords;

    // 경지 시스템
    public MasteryData mastery;

    // UI
    public UIData ui;

    // 게임플레이
    public GameplayData gameplay;

    // 시너지
    public List<string> synergies;

    // 메타데이터
    public MetaData metadata;
}

[Serializable]
public class EnergyCost
{
    public int qi;        // 내공
    public int martial;   // 무기술
}

[Serializable]
public class CardEffects
{
    public int damage;
    public int block;
    public List<AdditionalEffect> additional;
}

[Serializable]
public class AdditionalEffect
{
    public string type;      // "Bleed", "Poison", "Weak", etc.
    public int value;
    public string target;    // "Enemy", "Self", "AllEnemies"
    public int duration;
}

[Serializable]
public class MasteryData
{
    public string current;   // "Beginner", "Proficient", "Master"
    public List<string> upgradePath;
    public EffectBonus proficientBonus;
    public EffectBonus masterBonus;
}

[Serializable]
public class EffectBonus
{
    public int damage;
    public int bleed;
    public string newEffect;
}

[Serializable]
public class UIData
{
    public string description;
    public string artworkPath;
    public string iconPath;
}

[Serializable]
public class GameplayData
{
    public bool isPlayable;
    public bool isExhaustable;  // 소모
    public bool isEthereal;     // 영체 (턴 종료 시 소멸)
    public int maxCopiesInDeck;
}

[Serializable]
public class MetaData
{
    public string designer;
    public string createdDate;
    public string balanceNotes;
}

// Enums
public enum CardType { Attack, Defend, Skill, Curse, Status }
public enum Rarity { Common, Rare, Epic, Legendary }
```

---

### 16.3 적 데이터 JSON

```json
{
  "version": "1.0.0",
  "enemies": [
    {
      "id": "enemy_001",
      "nameKR": "산적",
      "nameEN": "Bandit",
      "tier": 1,

      "stats": {
        "maxHealth": 40,
        "damage": 6,
        "block": 0
      },

      "ai": {
        "intents": [
          {
            "type": "Attack",
            "weight": 50,
            "damage": 6,
            "times": 1
          },
          {
            "type": "Defend",
            "weight": 30,
            "block": 5
          },
          {
            "type": "Buff",
            "weight": 20,
            "buffType": "Strength",
            "value": 2
          }
        ],
        "firstTurnIntent": "Attack",
        "repeatLimit": 2
      },

      "rewards": {
        "gold": {
          "min": 10,
          "max": 20
        },
        "cardReward": true,
        "relicChance": 0.0
      },

      "ui": {
        "spritePath": "Assets/Art/Enemies/bandit.png",
        "animationController": "BanditAnimator"
      }
    },

    {
      "id": "enemy_boss_001",
      "nameKR": "혈마교주",
      "nameEN": "Blood Demon Lord",
      "tier": 5,
      "isBoss": true,

      "stats": {
        "maxHealth": 300,
        "damage": 15,
        "block": 0
      },

      "phases": [
        {
          "healthThreshold": 1.0,
          "intents": [
            {
              "type": "Attack",
              "weight": 60,
              "damage": 15,
              "times": 2
            },
            {
              "type": "Defend",
              "weight": 40,
              "block": 20
            }
          ]
        },
        {
          "healthThreshold": 0.5,
          "intents": [
            {
              "type": "Attack",
              "weight": 40,
              "damage": 25,
              "times": 1
            },
            {
              "type": "AOE",
              "weight": 30,
              "damage": 12
            },
            {
              "type": "Buff",
              "weight": 30,
              "buffType": "Strength",
              "value": 5
            }
          ]
        }
      ],

      "rewards": {
        "gold": {
          "min": 100,
          "max": 150
        },
        "cardReward": true,
        "relicReward": true
      }
    }
  ]
}
```

---

### 16.4 유물 데이터 JSON

```json
{
  "version": "1.0.0",
  "relics": [
    {
      "id": "relic_001",
      "nameKR": "혈마검",
      "nameEN": "Blood Demon Sword",
      "rarity": "Rare",

      "effect": {
        "type": "PassiveModifier",
        "description": "출혈 피해 +50%",
        "trigger": "OnBleedDamage",
        "value": 0.5
      },

      "ui": {
        "description": "출혈 피해가 50% 증가합니다.",
        "spritePath": "Assets/Art/Relics/blood_sword.png",
        "flavorText": "혈마교의 보물. 피를 빨아들인다."
      }
    },

    {
      "id": "relic_002",
      "nameKR": "무한의 팔찌",
      "nameEN": "Infinite Bracelet",
      "rarity": "Legendary",

      "effect": {
        "type": "OnCardDraw",
        "description": "카드 드로우 시 내공 +1 (턴당 3회)",
        "trigger": "OnCardDraw",
        "value": 1,
        "limitPerTurn": 3
      },

      "ui": {
        "description": "카드를 뽑을 때마다 내공을 1 얻습니다. (턴당 최대 3회)",
        "spritePath": "Assets/Art/Relics/infinite_bracelet.png",
        "flavorText": "무한한 기운이 흐른다."
      }
    }
  ]
}
```

---

### 16.5 JSON 파싱 완전 가이드

#### **DataManager 클래스**

```csharp
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // 데이터 캐시
    private CardDatabase cardDatabase;
    private EnemyDatabase enemyDatabase;
    private RelicDatabase relicDatabase;

    // 경로
    private const string DATA_PATH = "Data/";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 모든 데이터 로드
    /// </summary>
    public void LoadAllData()
    {
        cardDatabase = LoadJSON<CardDatabase>("cards");
        enemyDatabase = LoadJSON<EnemyDatabase>("enemies");
        relicDatabase = LoadJSON<RelicDatabase>("relics");

        Debug.Log($"Data loaded: {cardDatabase.cards.Count} cards, " +
                  $"{enemyDatabase.enemies.Count} enemies, " +
                  $"{relicDatabase.relics.Count} relics");
    }

    /// <summary>
    /// 제네릭 JSON 로더
    /// </summary>
    private T LoadJSON<T>(string filename)
    {
        // Resources 폴더에서 로드
        TextAsset jsonFile = Resources.Load<TextAsset>(DATA_PATH + filename);

        if (jsonFile == null)
        {
            Debug.LogError($"JSON file not found: {DATA_PATH + filename}");
            return default(T);
        }

        try
        {
            T data = JsonUtility.FromJson<T>(jsonFile.text);
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse JSON {filename}: {e.Message}");
            return default(T);
        }
    }

    /// <summary>
    /// ID로 카드 검색
    /// </summary>
    public CardData GetCard(string id)
    {
        return cardDatabase.cards.Find(c => c.id == id);
    }

    /// <summary>
    /// 레어도별 카드 검색
    /// </summary>
    public List<CardData> GetCardsByRarity(Rarity rarity)
    {
        return cardDatabase.cards.FindAll(c => c.rarity == rarity);
    }

    /// <summary>
    /// 타입별 카드 검색
    /// </summary>
    public List<CardData> GetCardsByType(CardType type)
    {
        return cardDatabase.cards.FindAll(c => c.type == type);
    }

    /// <summary>
    /// 랜덤 카드 3장 (보상용)
    /// </summary>
    public List<CardData> GetRandomCardReward(Rarity rarity, int count = 3)
    {
        var pool = GetCardsByRarity(rarity);
        var result = new List<CardData>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    /// <summary>
    /// ID로 적 검색
    /// </summary>
    public EnemyData GetEnemy(string id)
    {
        return enemyDatabase.enemies.Find(e => e.id == id);
    }

    /// <summary>
    /// 티어별 랜덤 적
    /// </summary>
    public EnemyData GetRandomEnemyByTier(int tier)
    {
        var pool = enemyDatabase.enemies.FindAll(e => e.tier == tier && !e.isBoss);
        if (pool.Count == 0) return null;

        return pool[Random.Range(0, pool.Count)];
    }
}
```

---

#### **Newtonsoft.Json 사용 (고급)**

Unity의 `JsonUtility`는 기능이 제한적입니다. 더 강력한 파싱을 위해 Newtonsoft.Json을 사용할 수 있습니다.

**설치:**
```
Unity Package Manager → Add package from git URL:
com.unity.nuget.newtonsoft-json
```

**사용 예시:**

```csharp
using Newtonsoft.Json;
using System.IO;

public class DataManagerAdvanced
{
    public static T LoadJSON<T>(string path)
    {
        string json = File.ReadAllText(path);

        // Newtonsoft.Json 사용
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.DeserializeObject<T>(json, settings);
    }

    public static void SaveJSON<T>(string path, T data)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(data, settings);
        File.WriteAllText(path, json);
    }
}
```

---

### 16.6 실습: 카드 데이터 로드 및 표시

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplayTest : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescText;
    public TextMeshProUGUI cardCostText;
    public Image cardArtwork;

    void Start()
    {
        // 일검 카드 로드
        CardData card = DataManager.Instance.GetCard("card_001");

        if (card != null)
        {
            DisplayCard(card);
        }
        else
        {
            Debug.LogError("Card not found!");
        }
    }

    void DisplayCard(CardData card)
    {
        cardNameText.text = card.nameKR;
        cardDescText.text = card.ui.description;
        cardCostText.text = $"내공: {card.costs.qi} / 무기술: {card.costs.martial}";

        // 아트워크 로드
        Sprite artwork = Resources.Load<Sprite>(card.ui.artworkPath);
        if (artwork != null)
        {
            cardArtwork.sprite = artwork;
        }

        Debug.Log($"Loaded card: {card.nameKR}");
        Debug.Log($"Damage: {card.effects.damage}");
        Debug.Log($"Keywords: {string.Join(", ", card.keywords)}");
    }
}
```

---

### 16.7 체크리스트

**데이터 구조:**
- [ ] JSON vs ScriptableObject 결정
- [ ] CardData 클래스 설계
- [ ] EnemyData 클래스 설계
- [ ] RelicData 클래스 설계

**JSON 파일:**
- [ ] cards.json 작성
- [ ] enemies.json 작성
- [ ] relics.json 작성
- [ ] JSON 유효성 검증 (https://jsonlint.com)

**코드:**
- [ ] DataManager 싱글톤 구현
- [ ] JSON 로드 함수 작성
- [ ] 검색 함수 작성 (ID, 레어도, 타입)
- [ ] 에러 핸들링

---

## Chapter 17: Google Sheets 연동

> "스프레드시트는 밸런싱의 최고의 친구입니다."

### 17.1 왜 Google Sheets인가?

**장점:**
```
✅ 팀 협업 용이 (실시간 동시 편집)
✅ 버전 히스토리 자동 저장
✅ 수식으로 DPE/BPE 자동 계산
✅ 필터, 정렬 기능
✅ 접근성 (웹 브라우저만 있으면 됨)
✅ CSV Export 기능
```

---

### 17.2 Google Sheets 템플릿 설정

#### **카드 밸런싱 시트 구조**

```
[시트 1: Cards]

A     B      C      D      E    F        G      H      I      J      K    L       M
ID    Name   Type   Rare   Qi   Martial  Dmg    Blk    Eff    DPE    BPE  Status  Notes
001   일검   Attack Common 0    1        6      0      -      6.0    -    OK✅    Standard
002   금강   Defend Common 1    0        0      5      -      -      5.0  OK✅    Standard
003   혈검   Attack Rare   0    2        8      0      출혈2  6.0    -    OK✅    Bleed
```

**자동 계산 수식:**

```
J2 (DPE):
=IF(F2>0, (G2+I2)/F2, "-")

K2 (BPE):
=IF(E2>0, H2/E2, "-")

L2 (Status):
=IF(
  AND(J2>=VLOOKUP(D2,RarityTable,2,FALSE),
      J2<=VLOOKUP(D2,RarityTable,3,FALSE)),
  "OK✅",
  "조정필요⚠️"
)
```

---

#### **레어도 기준 시트**

```
[시트 2: RarityTable]

A        B        C
Rarity   MIN_DPE  MAX_DPE
Common   5.0      6.0
Rare     6.0      7.5
Epic     7.0      9.0
Legend   8.0      12.0
```

---

### 17.3 Google Sheets API 설정 (고급)

#### **1. Google Cloud Console 설정**

```
1. https://console.cloud.google.com 접속
2. 새 프로젝트 생성: "MurimDeckBuilder"
3. API 및 서비스 → 라이브러리
4. "Google Sheets API" 검색 → 사용 설정
5. 사용자 인증 정보 → 서비스 계정 만들기
6. 역할: Editor
7. JSON 키 다운로드
```

---

#### **2. Unity에서 Google Sheets API 사용**

**Package 설치:**
```
Unity Package Manager → Add package from git URL:
https://github.com/googleworkspace/unity-google-sheets.git
```

**C# 코드:**

```csharp
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using System.IO;
using UnityEngine;

public class GoogleSheetsLoader : MonoBehaviour
{
    private const string SPREADSHEET_ID = "your-spreadsheet-id";
    private const string RANGE = "Cards!A2:M";

    private SheetsService sheetsService;

    void Start()
    {
        InitializeService();
        LoadCardsFromSheet();
    }

    void InitializeService()
    {
        // JSON 키 로드
        string credPath = Path.Combine(Application.streamingAssetsPath, "credentials.json");
        GoogleCredential credential;

        using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
        }

        // Sheets Service 초기화
        sheetsService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Murim Deckbuilder"
        });
    }

    void LoadCardsFromSheet()
    {
        var request = sheetsService.Spreadsheets.Values.Get(SPREADSHEET_ID, RANGE);
        var response = request.Execute();
        var values = response.Values;

        if (values != null && values.Count > 0)
        {
            foreach (var row in values)
            {
                // row[0] = ID, row[1] = Name, etc.
                Debug.Log($"Card: {row[1]}, Damage: {row[6]}");

                // CardData 객체 생성
                // ...
            }
        }
    }
}
```

---

### 17.4 CSV Export 방식 (추천)

Google Sheets API보다 **간단하고 실용적**입니다.

#### **워크플로우**

```
[1단계: Google Sheets에서 편집]
→ 카드 밸런싱 작업
→ DPE/BPE 자동 계산
→ 팀원들과 협업

[2단계: CSV Export]
→ File → Download → Comma-separated values (.csv)
→ cards.csv 저장

[3단계: Unity로 임포트]
→ Assets/StreamingAssets/Data/ 폴더에 복사
→ Unity 에디터에서 "Convert CSV to JSON" 실행

[4단계: 자동 변환]
→ CSV → JSON 변환
→ 데이터 검증
→ 완료!
```

---

#### **CSV to JSON 변환기**

```csharp
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CSVToJSONConverter : EditorWindow
{
    [MenuItem("Tools/Convert CSV to JSON")]
    static void ShowWindow()
    {
        GetWindow<CSVToJSONConverter>("CSV Converter");
    }

    void OnGUI()
    {
        GUILayout.Label("CSV to JSON Converter", EditorStyles.boldLabel);

        if (GUILayout.Button("Convert Cards.csv"))
        {
            ConvertCardsCSV();
        }

        if (GUILayout.Button("Convert Enemies.csv"))
        {
            ConvertEnemiesCSV();
        }
    }

    void ConvertCardsCSV()
    {
        string csvPath = Path.Combine(Application.streamingAssetsPath, "Data/cards.csv");
        string jsonPath = Path.Combine(Application.dataPath, "Resources/Data/cards.json");

        if (!File.Exists(csvPath))
        {
            Debug.LogError($"CSV file not found: {csvPath}");
            return;
        }

        // CSV 읽기
        string[] lines = File.ReadAllLines(csvPath);
        CardDatabase database = new CardDatabase
        {
            version = "1.0.0",
            lastUpdated = System.DateTime.Now.ToString("yyyy-MM-dd"),
            cards = new List<CardData>()
        };

        // 첫 줄은 헤더이므로 스킵
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 9)
            {
                Debug.LogWarning($"Line {i} has insufficient data");
                continue;
            }

            CardData card = new CardData
            {
                id = values[0],
                nameKR = values[1],
                type = ParseEnum<CardType>(values[2]),
                rarity = ParseEnum<Rarity>(values[3]),
                costs = new EnergyCost
                {
                    qi = int.Parse(values[4]),
                    martial = int.Parse(values[5])
                },
                effects = new CardEffects
                {
                    damage = int.Parse(values[6]),
                    block = int.Parse(values[7]),
                    additional = ParseAdditionalEffects(values[8])
                }
            };

            database.cards.Add(card);
        }

        // JSON 저장
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(jsonPath, json);

        AssetDatabase.Refresh();
        Debug.Log($"Converted {database.cards.Count} cards to JSON");
    }

    T ParseEnum<T>(string value) where T : System.Enum
    {
        return (T)System.Enum.Parse(typeof(T), value);
    }

    List<AdditionalEffect> ParseAdditionalEffects(string effectString)
    {
        var effects = new List<AdditionalEffect>();

        if (string.IsNullOrEmpty(effectString) || effectString == "-")
            return effects;

        // 예: "출혈2" → Bleed 2
        // 간단한 파싱 (실제로는 더 정교하게)
        if (effectString.Contains("출혈"))
        {
            int value = int.Parse(effectString.Replace("출혈", ""));
            effects.Add(new AdditionalEffect
            {
                type = "Bleed",
                value = value,
                target = "Enemy",
                duration = 3
            });
        }

        return effects;
    }

    void ConvertEnemiesCSV()
    {
        // 유사한 방식으로 적 데이터 변환
        Debug.Log("Enemy conversion not implemented yet");
    }
}
```

---

### 17.5 자동화 스크립트 (고급)

**Google Apps Script**로 자동 변환을 만들 수 있습니다.

```javascript
// Google Sheets에서: Extensions → Apps Script

function exportToJSON() {
  var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("Cards");
  var data = sheet.getDataRange().getValues();

  var cards = [];

  // 첫 줄은 헤더
  for (var i = 1; i < data.length; i++) {
    var row = data[i];

    var card = {
      id: row[0],
      nameKR: row[1],
      type: row[2],
      rarity: row[3],
      costs: {
        qi: row[4],
        martial: row[5]
      },
      effects: {
        damage: row[6],
        block: row[7]
      }
    };

    cards.push(card);
  }

  var database = {
    version: "1.0.0",
    lastUpdated: new Date().toISOString().split('T')[0],
    cards: cards
  };

  // JSON 생성
  var json = JSON.stringify(database, null, 2);

  // Drive에 저장
  var file = DriveApp.createFile("cards.json", json);
  Logger.log("JSON exported: " + file.getUrl());
}
```

---

### 17.6 체크리스트

**Google Sheets:**
- [ ] 카드 밸런싱 시트 생성
- [ ] 자동 계산 수식 작성 (DPE/BPE)
- [ ] 레어도 기준 시트 작성
- [ ] 팀원 공유 설정

**CSV Export:**
- [ ] CSV 다운로드
- [ ] Unity StreamingAssets에 복사
- [ ] CSV to JSON 변환기 작성
- [ ] 변환 테스트

**고급 (선택):**
- [ ] Google Sheets API 설정
- [ ] Unity에서 직접 로드
- [ ] Apps Script 자동화

---

## Chapter 18: 데이터 검증 및 테스트

> "검증되지 않은 데이터는 버그의 온상입니다."

### 18.1 데이터 무결성 체크

#### **검증 시스템**

```csharp
using System.Collections.Generic;
using UnityEngine;

public class DataValidator
{
    public static List<string> ValidateCardDatabase(CardDatabase database)
    {
        var errors = new List<string>();

        // 중복 ID 체크
        var idSet = new HashSet<string>();
        foreach (var card in database.cards)
        {
            if (idSet.Contains(card.id))
            {
                errors.Add($"[ERROR] Duplicate card ID: {card.id}");
            }
            idSet.Add(card.id);

            // 개별 카드 검증
            errors.AddRange(ValidateCard(card));
        }

        return errors;
    }

    static List<string> ValidateCard(CardData card)
    {
        var errors = new List<string>();

        // 기본 정보 검증
        if (string.IsNullOrEmpty(card.id))
            errors.Add($"[ERROR] Card has no ID");

        if (string.IsNullOrEmpty(card.nameKR))
            errors.Add($"[ERROR] Card {card.id} has no Korean name");

        // 비용 검증
        if (card.costs.qi < 0)
            errors.Add($"[ERROR] {card.nameKR}: Negative Qi cost");

        if (card.costs.martial < 0)
            errors.Add($"[ERROR] {card.nameKR}: Negative Martial cost");

        if (card.costs.qi == 0 && card.costs.martial == 0)
            errors.Add($"[WARNING] {card.nameKR}: Free card (0 cost)");

        // 효과 검증
        if (card.type == CardType.Attack && card.effects.damage <= 0)
            errors.Add($"[WARNING] {card.nameKR}: Attack card with 0 damage");

        if (card.type == CardType.Defend && card.effects.block <= 0)
            errors.Add($"[WARNING] {card.nameKR}: Defend card with 0 block");

        // DPE 검증
        if (card.costs.martial > 0)
        {
            float dpe = (float)card.effects.damage / card.costs.martial;
            float minDPE = GetMinDPE(card.rarity);
            float maxDPE = GetMaxDPE(card.rarity);

            if (dpe < minDPE || dpe > maxDPE)
            {
                errors.Add($"[WARNING] {card.nameKR}: DPE {dpe:F1} out of range " +
                          $"({minDPE}-{maxDPE} for {card.rarity})");
            }
        }

        // UI 검증
        if (string.IsNullOrEmpty(card.ui.description))
            errors.Add($"[ERROR] {card.nameKR}: Missing description");

        // 아트워크 존재 확인
        if (!string.IsNullOrEmpty(card.ui.artworkPath))
        {
            var artwork = Resources.Load<Sprite>(card.ui.artworkPath);
            if (artwork == null)
                errors.Add($"[WARNING] {card.nameKR}: Artwork not found at {card.ui.artworkPath}");
        }

        return errors;
    }

    static float GetMinDPE(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 5.0f;
            case Rarity.Rare: return 6.0f;
            case Rarity.Epic: return 7.0f;
            case Rarity.Legendary: return 8.0f;
            default: return 0f;
        }
    }

    static float GetMaxDPE(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 6.0f;
            case Rarity.Rare: return 7.5f;
            case Rarity.Epic: return 9.0f;
            case Rarity.Legendary: return 12.0f;
            default: return 999f;
        }
    }
}
```

---

#### **Unity 에디터 툴**

```csharp
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DataValidationTool : EditorWindow
{
    private Vector2 scrollPosition;
    private List<string> validationResults;

    [MenuItem("Tools/Validate Game Data")]
    static void ShowWindow()
    {
        GetWindow<DataValidationTool>("Data Validator");
    }

    void OnGUI()
    {
        GUILayout.Label("Game Data Validation", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (GUILayout.Button("Validate All Data", GUILayout.Height(30)))
        {
            ValidateAllData();
        }

        EditorGUILayout.Space();

        if (validationResults != null && validationResults.Count > 0)
        {
            GUILayout.Label($"Results ({validationResults.Count} issues):", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (var result in validationResults)
            {
                if (result.Contains("[ERROR]"))
                {
                    GUI.color = Color.red;
                }
                else if (result.Contains("[WARNING]"))
                {
                    GUI.color = Color.yellow;
                }
                else
                {
                    GUI.color = Color.green;
                }

                EditorGUILayout.LabelField(result, EditorStyles.wordWrappedLabel);
                GUI.color = Color.white;
            }

            EditorGUILayout.EndScrollView();
        }
    }

    void ValidateAllData()
    {
        validationResults = new List<string>();

        // 카드 데이터 검증
        var cardData = Resources.Load<TextAsset>("Data/cards");
        if (cardData != null)
        {
            var database = JsonUtility.FromJson<CardDatabase>(cardData.text);
            var errors = DataValidator.ValidateCardDatabase(database);
            validationResults.AddRange(errors);

            if (errors.Count == 0)
            {
                validationResults.Add("[OK] Card database validation passed");
            }
        }
        else
        {
            validationResults.Add("[ERROR] Card database not found");
        }

        // 적 데이터 검증
        // ...

        // 유물 데이터 검증
        // ...

        if (validationResults.Count == 0)
        {
            validationResults.Add("[OK] All data validation passed! ✅");
        }

        Repaint();
    }
}
```

---

### 18.2 단위 테스트

#### **Unity Test Framework 설정**

```
Window → General → Test Runner
→ Create PlayMode Test Assembly Folder
→ Create EditMode Test Assembly Folder
```

---

#### **데이터 로딩 테스트**

```csharp
using NUnit.Framework;
using UnityEngine;

public class DataLoadingTests
{
    private CardDatabase cardDatabase;

    [SetUp]
    public void Setup()
    {
        // 테스트 시작 전 데이터 로드
        var jsonFile = Resources.Load<TextAsset>("Data/cards");
        cardDatabase = JsonUtility.FromJson<CardDatabase>(jsonFile.text);
    }

    [Test]
    public void TestCardDatabaseLoads()
    {
        Assert.IsNotNull(cardDatabase, "Card database should load");
        Assert.Greater(cardDatabase.cards.Count, 0, "Should have at least 1 card");
    }

    [Test]
    public void TestAllCardsHaveValidIDs()
    {
        foreach (var card in cardDatabase.cards)
        {
            Assert.IsFalse(string.IsNullOrEmpty(card.id),
                          $"Card '{card.nameKR}' has invalid ID");
        }
    }

    [Test]
    public void TestNoDuplicateIDs()
    {
        var idSet = new System.Collections.Generic.HashSet<string>();

        foreach (var card in cardDatabase.cards)
        {
            Assert.IsFalse(idSet.Contains(card.id),
                          $"Duplicate card ID found: {card.id}");
            idSet.Add(card.id);
        }
    }

    [Test]
    public void TestAllAttackCardsHaveDamage()
    {
        foreach (var card in cardDatabase.cards)
        {
            if (card.type == CardType.Attack)
            {
                Assert.Greater(card.effects.damage, 0,
                              $"Attack card '{card.nameKR}' has 0 damage");
            }
        }
    }

    [Test]
    public void TestDPEWithinRange()
    {
        foreach (var card in cardDatabase.cards)
        {
            if (card.costs.martial > 0 && card.type == CardType.Attack)
            {
                float dpe = (float)card.effects.damage / card.costs.martial;

                // 일반 카드 DPE 범위: 5-6
                if (card.rarity == Rarity.Common)
                {
                    Assert.GreaterOrEqual(dpe, 5.0f,
                                         $"{card.nameKR} DPE too low: {dpe}");
                    Assert.LessOrEqual(dpe, 6.5f,
                                      $"{card.nameKR} DPE too high: {dpe}");
                }
            }
        }
    }
}
```

---

#### **게임플레이 테스트**

```csharp
using NUnit.Framework;
using UnityEngine;

public class GameplayTests
{
    [Test]
    public void TestCardCanBeDrawn()
    {
        // 카드를 덱에 추가하고 드로우 테스트
        var deck = new Deck();
        var card = DataManager.Instance.GetCard("card_001");

        deck.AddCard(card);
        Assert.AreEqual(1, deck.Count);

        var drawnCard = deck.Draw();
        Assert.IsNotNull(drawnCard);
        Assert.AreEqual(card.id, drawnCard.id);
        Assert.AreEqual(0, deck.Count);
    }

    [Test]
    public void TestCardEffectsApply()
    {
        var card = DataManager.Instance.GetCard("card_001"); // 일검 (피해 6)
        var enemy = new Enemy { health = 100 };

        // 카드 효과 적용
        enemy.TakeDamage(card.effects.damage);

        Assert.AreEqual(94, enemy.health);
    }
}
```

---

### 18.3 통합 테스트

```csharp
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.TestTools;

public class IntegrationTests
{
    [UnityTest]
    public IEnumerator TestFullCombat()
    {
        // 전투 씬 로드
        yield return LoadScene("CombatScene");

        // 플레이어 초기화
        var player = GameObject.FindObjectOfType<Player>();
        Assert.IsNotNull(player);

        // 적 생성
        var enemy = GameObject.FindObjectOfType<Enemy>();
        Assert.IsNotNull(enemy);

        // 카드 드로우
        var combatManager = CombatManager.Instance;
        combatManager.StartPlayerTurn();

        yield return new WaitForSeconds(0.5f);

        // 손패 확인
        Assert.Greater(player.hand.Count, 0, "Player should have cards in hand");

        // 카드 플레이
        var card = player.hand[0];
        combatManager.PlayCard(card, enemy);

        yield return new WaitForSeconds(0.5f);

        // 적 체력 감소 확인
        Assert.Less(enemy.currentHealth, enemy.maxHealth, "Enemy should take damage");
    }

    IEnumerator LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        yield return null;
    }
}
```

---

### 18.4 체크리스트

**검증:**
- [ ] DataValidator 클래스 구현
- [ ] 중복 ID 체크
- [ ] DPE/BPE 범위 체크
- [ ] 필수 필드 체크
- [ ] 아트워크 존재 확인

**테스트:**
- [ ] Unity Test Framework 설치
- [ ] 데이터 로딩 테스트 작성
- [ ] 게임플레이 테스트 작성
- [ ] 모든 테스트 통과 확인

**에디터 툴:**
- [ ] Data Validation Tool 구현
- [ ] 검증 결과 UI 표시
- [ ] 에러/경고 구분
- [ ] 한 번에 모든 데이터 검증

---

# PART 7: UI/UX 개발

## Chapter 19: 카드 UI 시스템

> "좋은 UI는 보이지 않습니다. 플레이어가 게임에만 집중하게 만듭니다."

### 19.1 카드 UI Prefab 완전 구조

#### **계층 구조**

```
CardUI (GameObject)
├─ Canvas (Canvas)
│   └─ CardContainer (RectTransform)
│       ├─ Background (Image)
│       │   └─ Border (Image)
│       ├─ Header (RectTransform)
│       │   ├─ CardName (TextMeshProUGUI)
│       │   └─ RarityIcon (Image)
│       ├─ Artwork (Image)
│       │   └─ ArtworkMask (Mask)
│       ├─ CostPanel (RectTransform)
│       │   ├─ QiCost (TextMeshProUGUI)
│       │   └─ MartialCost (TextMeshProUGUI)
│       ├─ Description (TextMeshProUGUI)
│       ├─ Keywords (HorizontalLayoutGroup)
│       │   ├─ Keyword1 (Image + Text)
│       │   └─ Keyword2 (Image + Text)
│       └─ Footer (RectTransform)
│           ├─ TypeIcon (Image)
│           └─ MasteryLevel (TextMeshProUGUI)
└─ VFXContainer (RectTransform)
    ├─ GlowEffect (Image)
    ├─ ParticleEffect (ParticleSystem)
    └─ HoverHighlight (Image)
```

---

#### **CardUI 스크립트**

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
                      IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public CardData cardData;

    [Header("UI Elements")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI qiCostText;
    public TextMeshProUGUI martialCostText;
    public Image artworkImage;
    public Image backgroundImage;
    public Image borderImage;
    public Image glowEffect;

    [Header("Keyword Display")]
    public Transform keywordContainer;
    public GameObject keywordPrefab;

    [Header("Colors")]
    public Color commonColor = new Color(0.8f, 0.8f, 0.8f);
    public Color rareColor = new Color(0.3f, 0.5f, 1f);
    public Color epicColor = new Color(0.7f, 0.3f, 1f);
    public Color legendaryColor = new Color(1f, 0.8f, 0.2f);

    [Header("Animation")]
    public float hoverScale = 1.1f;
    public float hoverDuration = 0.2f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Transform originalParent;
    private int originalSiblingIndex;
    private bool isDragging = false;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalScale = transform.localScale;
    }

    /// <summary>
    /// 카드 데이터로 UI 초기화
    /// </summary>
    public void Initialize(CardData data)
    {
        cardData = data;

        // 텍스트 설정
        cardNameText.text = data.nameKR;
        descriptionText.text = data.ui.description;
        qiCostText.text = data.costs.qi.ToString();
        martialCostText.text = data.costs.martial.ToString();

        // 아트워크 로드
        Sprite artwork = Resources.Load<Sprite>(data.ui.artworkPath);
        if (artwork != null)
            artworkImage.sprite = artwork;

        // 레어도에 따른 색상 설정
        SetRarityColors(data.rarity);

        // 키워드 표시
        DisplayKeywords(data.keywords);
    }

    void SetRarityColors(Rarity rarity)
    {
        Color color = rarity switch
        {
            Rarity.Common => commonColor,
            Rarity.Rare => rareColor,
            Rarity.Epic => epicColor,
            Rarity.Legendary => legendaryColor,
            _ => Color.white
        };

        borderImage.color = color;
        glowEffect.color = color;
    }

    void DisplayKeywords(System.Collections.Generic.List<string> keywords)
    {
        // 기존 키워드 제거
        foreach (Transform child in keywordContainer)
            Destroy(child.gameObject);

        // 새 키워드 생성
        foreach (string keyword in keywords)
        {
            GameObject keywordObj = Instantiate(keywordPrefab, keywordContainer);
            var text = keywordObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = keyword;
        }
    }

    // ===== 호버 효과 =====

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;

        // 확대 애니메이션
        transform.DOScale(originalScale * hoverScale, hoverDuration)
                 .SetEase(Ease.OutBack);

        // 글로우 효과
        glowEffect.DOFade(0.5f, hoverDuration);

        // 앞으로 가져오기
        transform.SetAsLastSibling();

        // 툴팁 표시 (선택)
        // TooltipManager.Instance.ShowCardTooltip(cardData, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;

        // 원래 크기로
        transform.DOScale(originalScale, hoverDuration)
                 .SetEase(Ease.InBack);

        // 글로우 제거
        glowEffect.DOFade(0f, hoverDuration);

        // 툴팁 숨김
        // TooltipManager.Instance.HideTooltip();
    }

    // ===== 드래그 앤 드롭 =====

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작
        isDragging = true;
        originalPosition = transform.position;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // 반투명하게
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        // 최상위로
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        // 살짝 회전
        transform.DORotate(new Vector3(0, 0, 5), 0.1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 따라가기
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        transform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 적에게 드롭했는지 확인
        if (IsOverEnemy(eventData))
        {
            PlayCard();
        }
        else
        {
            // 원래 위치로 되돌리기
            ReturnToHand();
        }
    }

    bool IsOverEnemy(PointerEventData eventData)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    void PlayCard()
    {
        // 카드 플레이 애니메이션
        Sequence sequence = DOTween.Sequence();

        // 1. 적에게 날아가기
        Vector3 targetPosition = GetTargetEnemyPosition();
        sequence.Append(transform.DOMove(targetPosition, 0.3f).SetEase(Ease.InQuad));

        // 2. 회전
        sequence.Join(transform.DORotate(new Vector3(0, 0, 720), 0.3f, RotateMode.FastBeyond360));

        // 3. 축소
        sequence.Join(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));

        // 4. 효과 적용 및 파괴
        sequence.OnComplete(() =>
        {
            CombatManager.Instance.ApplyCardEffect(cardData);
            Destroy(gameObject);
        });
    }

    void ReturnToHand()
    {
        // 부드럽게 원래 위치로
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(originalPosition, 0.3f).SetEase(Ease.OutQuad));
        sequence.Join(transform.DORotate(Vector3.zero, 0.2f));

        sequence.OnComplete(() =>
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        });
    }

    Vector3 GetTargetEnemyPosition()
    {
        // 타겟 적 찾기 (간단한 예시)
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
            return enemy.transform.position;

        return Vector3.zero;
    }
}
```

---

### 19.2 손패 (Hand) 시스템

#### **HandManager 스크립트**

```csharp
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandManager : MonoBehaviour
{
    [Header("Settings")]
    public Transform handContainer;
    public GameObject cardPrefab;
    public int maxHandSize = 10;

    [Header("Layout")]
    public float cardSpacing = 150f;
    public float curveHeight = 50f;
    public float fanAngle = 15f;

    private List<CardUI> cardsInHand = new List<CardUI>();

    /// <summary>
    /// 손에 카드 추가 (드로우)
    /// </summary>
    public void AddCard(CardData cardData)
    {
        if (cardsInHand.Count >= maxHandSize)
        {
            Debug.LogWarning("Hand is full!");
            return;
        }

        // 카드 생성
        GameObject cardObj = Instantiate(cardPrefab, handContainer);
        CardUI cardUI = cardObj.GetComponent<CardUI>();
        cardUI.Initialize(cardData);

        // 초기 위치 (덱 위치)
        cardObj.transform.position = GetDeckPosition();
        cardObj.transform.localScale = Vector3.zero;

        // 손에 추가
        cardsInHand.Add(cardUI);

        // 드로우 애니메이션
        DrawCardAnimation(cardUI);

        // 손패 재정렬
        ArrangeHand();
    }

    void DrawCardAnimation(CardUI card)
    {
        Sequence sequence = DOTween.Sequence();

        // 1. 확대
        sequence.Append(card.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));

        // 2. 회전 (뒤집기 효과)
        sequence.Join(card.transform.DORotate(new Vector3(0, 180, 0), 0.15f))
                .Append(card.transform.DORotate(Vector3.zero, 0.15f));
    }

    /// <summary>
    /// 손패를 부채꼴로 정렬
    /// </summary>
    void ArrangeHand()
    {
        int cardCount = cardsInHand.Count;

        for (int i = 0; i < cardCount; i++)
        {
            CardUI card = cardsInHand[i];

            // 위치 계산
            float normalizedPosition = cardCount > 1 ? (float)i / (cardCount - 1) : 0.5f;
            normalizedPosition -= 0.5f; // -0.5 ~ 0.5

            // X 위치 (간격)
            float xPos = normalizedPosition * cardSpacing * (cardCount - 1);

            // Y 위치 (곡선)
            float yPos = -curveHeight * Mathf.Pow(normalizedPosition * 2, 2);

            // 회전 (부채꼴)
            float rotation = normalizedPosition * fanAngle * (cardCount - 1);

            Vector3 targetPosition = new Vector3(xPos, yPos, 0);
            Vector3 targetRotation = new Vector3(0, 0, -rotation);

            // 부드러운 이동
            card.transform.DOLocalMove(targetPosition, 0.3f).SetEase(Ease.OutQuad);
            card.transform.DOLocalRotate(targetRotation, 0.3f).SetEase(Ease.OutQuad);

            // Z-order (왼쪽이 앞에)
            card.transform.SetSiblingIndex(i);
        }
    }

    /// <summary>
    /// 카드 사용 후 제거
    /// </summary>
    public void RemoveCard(CardUI card)
    {
        cardsInHand.Remove(card);
        ArrangeHand();
    }

    Vector3 GetDeckPosition()
    {
        // 덱 위치 (우하단)
        return new Vector3(Screen.width * 0.9f, Screen.height * 0.1f, 0);
    }

    /// <summary>
    /// 손패 전체 버리기
    /// </summary>
    public void DiscardHand()
    {
        foreach (CardUI card in cardsInHand)
        {
            DiscardCardAnimation(card);
        }

        cardsInHand.Clear();
    }

    void DiscardCardAnimation(CardUI card)
    {
        Vector3 discardPosition = GetDiscardPosition();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(card.transform.DOMove(discardPosition, 0.4f).SetEase(Ease.InQuad));
        sequence.Join(card.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack));
        sequence.OnComplete(() => Destroy(card.gameObject));
    }

    Vector3 GetDiscardPosition()
    {
        // 버리기 더미 위치 (우하단)
        return new Vector3(Screen.width * 0.85f, Screen.height * 0.1f, 0);
    }
}
```

---

### 19.3 카드 애니메이션 라이브러리

#### **CardAnimations 스크립트**

```csharp
using UnityEngine;
using DG.Tweening;

public static class CardAnimations
{
    /// <summary>
    /// 드로우 애니메이션
    /// </summary>
    public static void Draw(Transform card, Vector3 startPos, Vector3 endPos, float duration = 0.5f)
    {
        card.position = startPos;
        card.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();

        // 경로 (포물선)
        Vector3[] path = new Vector3[]
        {
            startPos,
            startPos + Vector3.up * 200f,
            endPos
        };

        sequence.Append(card.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.OutQuad));
        sequence.Join(card.DOScale(Vector3.one, duration * 0.5f).SetEase(Ease.OutBack));
        sequence.Join(card.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360));
    }

    /// <summary>
    /// 공격 애니메이션
    /// </summary>
    public static void Attack(Transform card, Transform target, System.Action onComplete = null)
    {
        Vector3 originalPos = card.position;

        Sequence sequence = DOTween.Sequence();

        // 1. 타겟으로 돌진
        sequence.Append(card.DOMove(target.position, 0.3f).SetEase(Ease.InQuad));
        sequence.Join(card.DORotate(new Vector3(0, 0, 720), 0.3f, RotateMode.FastBeyond360));

        // 2. 임팩트 효과
        sequence.AppendCallback(() =>
        {
            // VFX 생성
            PlayImpactVFX(target.position);
            CameraShake();
        });

        // 3. 뒤로 빠지기
        sequence.Append(card.DOMove(originalPos, 0.2f).SetEase(Ease.OutQuad));

        // 4. 소멸
        sequence.Append(card.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

        sequence.OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 방어 애니메이션
    /// </summary>
    public static void Defend(Transform card, System.Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();

        // 1. 플레이어 앞으로
        Vector3 shieldPos = new Vector3(0, -100, 0);
        sequence.Append(card.DOLocalMove(shieldPos, 0.3f).SetEase(Ease.OutQuad));

        // 2. 확대 (방패 효과)
        sequence.Join(card.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.OutBack));

        // 3. 플래시 효과
        var image = card.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            sequence.Append(image.DOFade(0.5f, 0.1f).SetLoops(4, LoopType.Yoyo));
        }

        // 4. 소멸
        sequence.Append(card.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

        sequence.OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 버리기 애니메이션
    /// </summary>
    public static void Discard(Transform card, Vector3 discardPos, System.Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(card.DOMove(discardPos, 0.4f).SetEase(Ease.InQuad));
        sequence.Join(card.DORotate(new Vector3(0, 0, 360), 0.4f, RotateMode.FastBeyond360));
        sequence.Join(card.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack));

        sequence.OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 소멸 (Exhaust) 애니메이션
    /// </summary>
    public static void Exhaust(Transform card, System.Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();

        // 타오르는 효과
        sequence.Append(card.DOShakePosition(0.5f, strength: 10f));
        sequence.Join(card.DOScale(Vector3.one * 1.2f, 0.5f));

        var image = card.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            // 빨간색으로 변하면서 소멸
            sequence.Join(image.DOColor(Color.red, 0.3f));
            sequence.Append(image.DOFade(0f, 0.2f));
        }

        sequence.OnComplete(() => onComplete?.Invoke());
    }

    // ===== 헬퍼 함수 =====

    static void PlayImpactVFX(Vector3 position)
    {
        // VFX 재생 (예시)
        GameObject vfx = Resources.Load<GameObject>("VFX/Impact");
        if (vfx != null)
        {
            GameObject instance = Object.Instantiate(vfx, position, Quaternion.identity);
            Object.Destroy(instance, 1f);
        }
    }

    static void CameraShake()
    {
        Camera.main.transform.DOShakePosition(0.2f, strength: 5f, vibrato: 20);
    }
}
```

---

### 19.4 실습: 카드 UI 생성

#### **Step 1: Prefab 생성**

```
1. Unity Hierarchy: 우클릭 → UI → Image
2. 이름: CardUI
3. RectTransform 설정:
   - Width: 200
   - Height: 300
   - Anchor: Middle Center

4. 자식 오브젝트 추가:
   - Background (Image)
   - CardName (TextMeshProUGUI)
   - Description (TextMeshProUGUI)
   - Cost (TextMeshProUGUI)
   - Artwork (Image)

5. CardUI 스크립트 부착

6. Prefab으로 저장: Assets/Prefabs/CardUI.prefab
```

---

#### **Step 2: 테스트 씬 설정**

```csharp
using UnityEngine;

public class CardUITest : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handContainer;

    void Start()
    {
        // 테스트 카드 데이터
        CardData testCard = new CardData
        {
            id = "card_001",
            nameKR = "일검",
            costs = new EnergyCost { qi = 0, martial = 1 },
            effects = new CardEffects { damage = 6 },
            ui = new UIData
            {
                description = "적에게 6의 피해를 입힌다."
            },
            rarity = Rarity.Common
        };

        // 카드 생성
        GameObject cardObj = Instantiate(cardPrefab, handContainer);
        CardUI cardUI = cardObj.GetComponent<CardUI>();
        cardUI.Initialize(testCard);
    }
}
```

---

### 19.5 체크리스트

**Prefab:**
- [ ] CardUI Prefab 생성
- [ ] 모든 UI 요소 배치
- [ ] CardUI 스크립트 부착
- [ ] 레어도별 색상 설정

**스크립트:**
- [ ] CardUI.cs 구현
- [ ] HandManager.cs 구현
- [ ] CardAnimations.cs 구현
- [ ] 드래그 앤 드롭 기능

**애니메이션:**
- [ ] 드로우 애니메이션
- [ ] 호버 효과
- [ ] 플레이 애니메이션
- [ ] 버리기 애니메이션

---

## Chapter 20: 무협 테마 디자인

> "테마는 일관성입니다. 모든 UI 요소가 무협 세계관을 표현해야 합니다."

### 20.1 색상 팔레트 (Color Palette)

#### **주요 색상**

```csharp
public class MurimColorPalette
{
    // === 에너지 타입 ===

    // 내공 (Qi) - 청록색 계열
    public static Color QiPrimary = HexToColor("#2E8B57");      // 진한 청록
    public static Color QiSecondary = HexToColor("#3CB371");    // 중간 청록
    public static Color QiLight = HexToColor("#90EE90");        // 밝은 청록

    // 무기술 (Martial) - 적색 계열
    public static Color MartialPrimary = HexToColor("#DC143C");  // 진한 빨강
    public static Color MartialSecondary = HexToColor("#FF6347"); // 중간 빨강
    public static Color MartialLight = HexToColor("#FF7F7F");    // 밝은 빨강

    // === 레어도 ===

    // 일반 (Common) - 회색
    public static Color CommonBorder = HexToColor("#CCCCCC");
    public static Color CommonGlow = HexToColor("#E0E0E0");

    // 희귀 (Rare) - 파란색
    public static Color RareBorder = HexToColor("#4169E1");
    public static Color RareGlow = HexToColor("#6495ED");

    // 영웅 (Epic) - 보라색
    public static Color EpicBorder = HexToColor("#9370DB");
    public static Color EpicGlow = HexToColor("#BA55D3");

    // 전설 (Legendary) - 금색
    public static Color LegendaryBorder = HexToColor("#FFD700");
    public static Color LegendaryGlow = HexToColor("#FFA500");

    // === UI 기본 색상 ===

    // 배경
    public static Color BackgroundDark = HexToColor("#1A1A1A");
    public static Color BackgroundMedium = HexToColor("#2D2D2D");
    public static Color BackgroundLight = HexToColor("#3F3F3F");

    // 텍스트
    public static Color TextPrimary = HexToColor("#FFFFFF");
    public static Color TextSecondary = HexToColor("#CCCCCC");
    public static Color TextDisabled = HexToColor("#777777");

    // 버튼
    public static Color ButtonNormal = HexToColor("#4A4A4A");
    public static Color ButtonHover = HexToColor("#5A5A5A");
    public static Color ButtonPressed = HexToColor("#3A3A3A");
    public static Color ButtonDisabled = HexToColor("#2A2A2A");

    // === 상태 이상 ===

    public static Color BleedColor = HexToColor("#8B0000");      // 출혈
    public static Color PoisonColor = HexToColor("#228B22");     // 중독
    public static Color WeakColor = HexToColor("#FFD700");       // 약화
    public static Color VulnerableColor = HexToColor("#FF4500"); // 취약

    // === 헬퍼 함수 ===

    static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;
        return Color.white;
    }
}
```

---

#### **색상 사용 예시**

```csharp
using UnityEngine;
using UnityEngine.UI;

public class ColorThemeExample : MonoBehaviour
{
    public Image qiCostBackground;
    public Image martialCostBackground;
    public Image cardBorder;

    void Start()
    {
        // 내공 비용 배경
        qiCostBackground.color = MurimColorPalette.QiPrimary;

        // 무기술 비용 배경
        martialCostBackground.color = MurimColorPalette.MartialPrimary;

        // 카드 테두리 (레어도에 따라)
        cardBorder.color = MurimColorPalette.RareBorder;
    }
}
```

---

### 20.2 폰트 및 타이포그래피

#### **추천 폰트**

```
[한글 폰트]

제목/카드 이름:
- 윤고딕 (상업용)
- 나눔스퀘어 (무료)
- 배달의민족 주아체 (무료, 무협 분위기)

본문/설명:
- 나눔고딕 (무료)
- 본고딕 (무료)
- 스포카 한 Sans (무료)

숫자/스탯:
- Roboto (무료)
- Orbitron (무료, 미래적)
- Bebas Neue (무료, 강렬함)

강조/이펙트:
- 배달의민족 도현체 (무료, 캘리그라피)
```

---

#### **TextMeshPro 설정**

```
1. Window → TextMeshPro → Font Asset Creator

2. 나눔스퀘어 폰트 임포트:
   - Source Font File: NanumSquare.ttf
   - Sampling Point Size: 72
   - Character Set: Unicode Range (Korean)
   - Render Mode: SDFAA_HINTED

3. Generate Font Atlas

4. 저장: Assets/Fonts/NanumSquare SDF.asset

5. 사용:
   - TextMeshProUGUI 컴포넌트
   - Font Asset: NanumSquare SDF
```

---

#### **타이포그래피 가이드**

```csharp
public class TypographySettings
{
    // === 폰트 크기 ===

    public const float FontSize_H1 = 48f;   // 타이틀
    public const float FontSize_H2 = 36f;   // 서브타이틀
    public const float FontSize_H3 = 28f;   // 섹션 제목
    public const float FontSize_Body = 20f; // 본문
    public const float FontSize_Small = 16f;// 작은 텍스트
    public const float FontSize_Tiny = 12f; // 매우 작은 텍스트

    // === 카드 전용 ===

    public const float CardName_Size = 24f;
    public const float CardDesc_Size = 18f;
    public const float CardCost_Size = 32f;
    public const float CardKeyword_Size = 14f;

    // === 줄 간격 ===

    public const float LineSpacing_Tight = 0.8f;
    public const float LineSpacing_Normal = 1.0f;
    public const float LineSpacing_Loose = 1.2f;
}
```

---

### 20.3 UI 레이아웃 (Layout)

#### **전투 화면 레이아웃**

```
[1920x1080 기준]

┌─────────────────────────────────────────┐
│  [적1]    [적2]    [적3]               │ 상단 (적 영역)
│   HP       HP       HP                  │ y: 600~900
│  Intent  Intent  Intent                │
│                                         │
│                                         │
│                       [덱] [버리기더미] │ 우측 상단
│                        50    25        │ x: 1600~1900
│                                         │
│                                         │
│  [플레이어 상태]                        │ 좌측 하단
│   HP: 80/100                           │ x: 50~300
│   내공: 3  무기술: 3                   │ y: 50~250
│                                         │
│        [카드] [카드] [카드]            │ 하단 (손패)
│     [카드] [카드] [카드] [카드]        │ y: 50~350
│   (부채꼴 배치)                         │
│                                         │
│  [턴 종료]                   [설정]    │ 하단 버튼
└─────────────────────────────────────────┘
```

---

#### **레이아웃 스크립트**

```csharp
using UnityEngine;

public class CombatLayoutManager : MonoBehaviour
{
    [Header("Screen Regions")]
    public RectTransform enemyArea;      // 상단
    public RectTransform playerArea;     // 좌하단
    public RectTransform handArea;       // 중앙 하단
    public RectTransform deckArea;       // 우상단
    public RectTransform uiOverlay;      // UI 오버레이

    [Header("Anchor Positions")]
    public Vector2 enemyY = new Vector2(600f, 900f);
    public Vector2 handY = new Vector2(50f, 350f);

    void Start()
    {
        SetupLayout();
    }

    void SetupLayout()
    {
        // 적 영역 (상단 중앙)
        SetupRectTransform(enemyArea,
            anchorMin: new Vector2(0.5f, 1f),
            anchorMax: new Vector2(0.5f, 1f),
            pivot: new Vector2(0.5f, 1f),
            anchoredPosition: new Vector2(0, -100f));

        // 손패 영역 (하단 중앙)
        SetupRectTransform(handArea,
            anchorMin: new Vector2(0.5f, 0f),
            anchorMax: new Vector2(0.5f, 0f),
            pivot: new Vector2(0.5f, 0f),
            anchoredPosition: new Vector2(0, 100f));

        // 플레이어 영역 (좌하단)
        SetupRectTransform(playerArea,
            anchorMin: new Vector2(0f, 0f),
            anchorMax: new Vector2(0f, 0f),
            pivot: new Vector2(0f, 0f),
            anchoredPosition: new Vector2(50f, 50f));

        // 덱 영역 (우상단)
        SetupRectTransform(deckArea,
            anchorMin: new Vector2(1f, 1f),
            anchorMax: new Vector2(1f, 1f),
            pivot: new Vector2(1f, 1f),
            anchoredPosition: new Vector2(-50f, -50f));
    }

    void SetupRectTransform(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax,
                           Vector2 pivot, Vector2 anchoredPosition)
    {
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPosition;
    }
}
```

---

### 20.4 무협 테마 아이콘 및 그래픽

#### **아이콘 디자인 가이드**

```
[필수 아이콘]

1. 에너지:
   - 내공: 청록색 소용돌이
   - 무기술: 빨간색 검기

2. 카드 타입:
   - 공격: 검 아이콘
   - 방어: 방패 아이콘
   - 기술: 책/두루마리 아이콘

3. 키워드:
   - 출혈: 피방울
   - 중독: 독 구름
   - 약화: 아래 화살표
   - 취약: 금 간 방패

4. 레어도:
   - 일반: 별 (회색)
   - 희귀: 별 2개 (파란색)
   - 영웅: 별 3개 (보라색)
   - 전설: 용 (금색)

5. 버튼:
   - 확인: 체크마크
   - 취소: X
   - 설정: 톱니바퀴
   - 정보: ?

[무료 아이콘 리소스]
- Font Awesome (https://fontawesome.com)
- Game Icons (https://game-icons.net)
- Flaticon (https://www.flaticon.com)
```

---

#### **아이콘 매니저**

```csharp
using UnityEngine;
using System.Collections.Generic;

public class IconManager : MonoBehaviour
{
    public static IconManager Instance { get; private set; }

    [Header("Energy Icons")]
    public Sprite qiIcon;
    public Sprite martialIcon;

    [Header("Card Type Icons")]
    public Sprite attackIcon;
    public Sprite defendIcon;
    public Sprite skillIcon;

    [Header("Keyword Icons")]
    public Sprite bleedIcon;
    public Sprite poisonIcon;
    public Sprite weakIcon;
    public Sprite vulnerableIcon;

    [Header("Rarity Icons")]
    public Sprite commonIcon;
    public Sprite rareIcon;
    public Sprite epicIcon;
    public Sprite legendaryIcon;

    private Dictionary<string, Sprite> iconDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildIconDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void BuildIconDictionary()
    {
        iconDict = new Dictionary<string, Sprite>
        {
            // Energy
            {"qi", qiIcon},
            {"martial", martialIcon},

            // Card Types
            {"attack", attackIcon},
            {"defend", defendIcon},
            {"skill", skillIcon},

            // Keywords
            {"bleed", bleedIcon},
            {"poison", poisonIcon},
            {"weak", weakIcon},
            {"vulnerable", vulnerableIcon},

            // Rarity
            {"common", commonIcon},
            {"rare", rareIcon},
            {"epic", epicIcon},
            {"legendary", legendaryIcon}
        };
    }

    public Sprite GetIcon(string iconName)
    {
        if (iconDict.TryGetValue(iconName.ToLower(), out Sprite icon))
            return icon;

        Debug.LogWarning($"Icon not found: {iconName}");
        return null;
    }
}
```

---

### 20.5 반응형 UI (Responsive UI)

#### **다양한 해상도 지원**

```csharp
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveUIManager : MonoBehaviour
{
    [Header("Target Resolutions")]
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        SetupResponsiveUI();
    }

    void SetupResponsiveUI()
    {
        // Canvas Scaler 설정
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        // 화면 비율에 따라 조정
        float aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio > 1.7f) // 와이드스크린 (16:9 이상)
        {
            canvasScaler.matchWidthOrHeight = 0f; // Width 기준
        }
        else if (aspectRatio < 1.5f) // 스퀘어에 가까움 (4:3 등)
        {
            canvasScaler.matchWidthOrHeight = 1f; // Height 기준
        }
        else // 표준 (16:9)
        {
            canvasScaler.matchWidthOrHeight = 0.5f; // 중간
        }
    }
}
```

---

### 20.6 체크리스트

**색상:**
- [ ] 색상 팔레트 정의
- [ ] 에너지 타입별 색상 설정
- [ ] 레어도별 색상 설정
- [ ] 상태 이상 색상 설정

**폰트:**
- [ ] 한글 폰트 선택 및 임포트
- [ ] TextMeshPro Font Asset 생성
- [ ] 타이포그래피 가이드 작성
- [ ] 모든 UI 텍스트에 적용

**레이아웃:**
- [ ] 전투 화면 레이아웃 설계
- [ ] 메뉴 화면 레이아웃 설계
- [ ] 반응형 UI 설정
- [ ] 다양한 해상도 테스트

**아이콘:**
- [ ] 필수 아이콘 디자인/구매
- [ ] IconManager 구현
- [ ] 모든 아이콘 적용

---

## Chapter 21: 애니메이션 및 VFX

> "애니메이션은 게임에 생명을 불어넣습니다."

### 21.1 DOTween 설치 및 기본 사용법

#### **DOTween 설치**

```
방법 1: Unity Asset Store
1. Asset Store 열기
2. "DOTween" 검색
3. "DOTween (HOTween v2)" 다운로드 (무료)
4. Import

방법 2: Unity Package Manager
1. Window → Package Manager
2. Add package from git URL
3. https://github.com/Demigiant/dotween.git

초기 설정:
Tools → Demigiant → DOTween Utility Panel → Setup DOTween
```

---

#### **DOTween 기본 문법**

```csharp
using DG.Tweening;
using UnityEngine;

public class DOTweenBasics : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        // === 기본 Tween ===

        // 이동
        target.DOMove(new Vector3(5, 0, 0), 1f);

        // 회전
        target.DORotate(new Vector3(0, 180, 0), 1f);

        // 크기
        target.DOScale(Vector3.one * 2f, 1f);

        // 페이드 (Image/SpriteRenderer)
        GetComponent<SpriteRenderer>().DOFade(0f, 1f);

        // 색상
        GetComponent<SpriteRenderer>().DOColor(Color.red, 1f);

        // === Ease (가속도) ===

        target.DOMove(new Vector3(5, 0, 0), 1f).SetEase(Ease.OutQuad);
        // OutQuad: 점점 느려짐
        // InQuad: 점점 빨라짐
        // InOutQuad: 시작/끝 느림
        // OutBack: 오버슈팅 (튕김)

        // === 루프 ===

        target.DORotate(new Vector3(0, 360, 0), 2f)
              .SetLoops(-1, LoopType.Restart); // 무한 반복

        // === 딜레이 ===

        target.DOMove(new Vector3(5, 0, 0), 1f)
              .SetDelay(0.5f); // 0.5초 후 시작

        // === 콜백 ===

        target.DOMove(new Vector3(5, 0, 0), 1f)
              .OnComplete(() => Debug.Log("완료!"))
              .OnUpdate(() => Debug.Log("진행 중..."));

        // === Sequence (순차 실행) ===

        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOMove(new Vector3(5, 0, 0), 1f));
        sequence.Append(target.DORotate(new Vector3(0, 180, 0), 1f));
        sequence.Append(target.DOScale(Vector3.one * 2f, 1f));

        // === Join (동시 실행) ===

        Sequence seq2 = DOTween.Sequence();
        seq2.Append(target.DOMove(new Vector3(5, 0, 0), 1f));
        seq2.Join(target.DORotate(new Vector3(0, 180, 0), 1f)); // 이동과 동시에

        // === 경로 이동 ===

        Vector3[] path = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(5, 5, 0),
            new Vector3(10, 0, 0)
        };

        target.DOPath(path, 2f, PathType.CatmullRom).SetEase(Ease.Linear);
    }

    void OnDestroy()
    {
        // Tween 정리 (필수!)
        target.DOKill();
    }
}
```

---

### 21.2 카드 VFX 시스템

#### **VFXManager 스크립트**

```csharp
using UnityEngine;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("Card VFX")]
    public GameObject cardDrawVFX;
    public GameObject cardPlayVFX;
    public GameObject cardExhaustVFX;

    [Header("Combat VFX")]
    public GameObject slashVFX;
    public GameObject impactVFX;
    public GameObject shieldVFX;
    public GameObject healVFX;

    [Header("Status VFX")]
    public GameObject bleedVFX;
    public GameObject poisonVFX;
    public GameObject buffVFX;
    public GameObject debuffVFX;

    private Queue<GameObject> vfxPool = new Queue<GameObject>();

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

    /// <summary>
    /// VFX 재생
    /// </summary>
    public void PlayVFX(GameObject vfxPrefab, Vector3 position, float duration = 1f)
    {
        if (vfxPrefab == null) return;

        GameObject vfx = Instantiate(vfxPrefab, position, Quaternion.identity);
        Destroy(vfx, duration);
    }

    /// <summary>
    /// 카드 드로우 VFX
    /// </summary>
    public void PlayCardDraw(Vector3 position)
    {
        PlayVFX(cardDrawVFX, position, 0.5f);
    }

    /// <summary>
    /// 공격 VFX
    /// </summary>
    public void PlayAttack(Vector3 position, int damage)
    {
        PlayVFX(slashVFX, position, 0.5f);
        PlayVFX(impactVFX, position, 0.3f);

        // 데미지 텍스트
        ShowDamageText(position, damage);
    }

    /// <summary>
    /// 방어 VFX
    /// </summary>
    public void PlayDefend(Vector3 position, int block)
    {
        PlayVFX(shieldVFX, position, 1f);

        // 방어 텍스트
        ShowBlockText(position, block);
    }

    /// <summary>
    /// 출혈 VFX
    /// </summary>
    public void PlayBleed(Vector3 position)
    {
        GameObject vfx = Instantiate(bleedVFX, position, Quaternion.identity);
        // 지속 효과이므로 수동으로 관리
    }

    void ShowDamageText(Vector3 position, int damage)
    {
        // DamageText 프리팹 생성 및 애니메이션
        // (다음 섹션에서 구현)
    }

    void ShowBlockText(Vector3 position, int block)
    {
        // BlockText 프리팹 생성 및 애니메이션
    }
}
```

---

#### **데미지 텍스트 애니메이션**

```csharp
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public void Initialize(int damage, bool isCritical = false)
    {
        // 텍스트 설정
        textMesh.text = damage.ToString();

        if (isCritical)
        {
            textMesh.color = Color.yellow;
            textMesh.fontSize = 72;
        }
        else
        {
            textMesh.color = Color.white;
            textMesh.fontSize = 48;
        }

        // 애니메이션
        Sequence sequence = DOTween.Sequence();

        // 1. 위로 튀어오르기
        sequence.Append(transform.DOLocalMoveY(1f, 0.5f).SetEase(Ease.OutQuad));

        // 2. 페이드 아웃
        sequence.Join(textMesh.DOFade(0f, 0.5f).SetDelay(0.3f));

        // 3. 살짝 커지기
        sequence.Join(transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutBack));

        // 4. 파괴
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
```

---

### 21.3 파티클 시스템

#### **공격 파티클 생성**

```
1. Hierarchy: 우클릭 → Effects → Particle System
2. 이름: SlashVFX

3. Main Module:
   - Duration: 0.5
   - Start Lifetime: 0.3
   - Start Speed: 5
   - Start Size: 0.5
   - Start Color: Red → Yellow (Gradient)

4. Emission:
   - Rate over Time: 0
   - Bursts: Count 20, Time 0

5. Shape:
   - Shape: Cone
   - Angle: 30
   - Radius: 0.2

6. Color over Lifetime:
   - Gradient: 흰색 → 빨강 → 투명

7. Size over Lifetime:
   - Curve: 시작 작음 → 중간 큼 → 끝 작음

8. Renderer:
   - Material: Default-Particle
   - Render Mode: Billboard
```

---

#### **파티클 스크립트 제어**

```csharp
using UnityEngine;

public class AttackVFX : MonoBehaviour
{
    private ParticleSystem particles;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Play(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        // 파티클 재생
        particles.Play();

        // 자동 파괴
        Destroy(gameObject, particles.main.duration);
    }

    public void SetColor(Color color)
    {
        var main = particles.main;
        main.startColor = color;
    }

    public void SetSize(float size)
    {
        var main = particles.main;
        main.startSize = size;
    }
}
```

---

### 21.4 화면 효과 (Screen Effects)

#### **카메라 셰이크**

```csharp
using UnityEngine;
using DG.Tweening;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }

    private Camera mainCamera;
    private Vector3 originalPosition;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        originalPosition = mainCamera.transform.position;
    }

    /// <summary>
    /// 카메라 흔들기
    /// </summary>
    public void Shake(float intensity = 0.5f, float duration = 0.3f)
    {
        mainCamera.transform.DOShakePosition(duration, strength: intensity, vibrato: 20)
                           .OnComplete(() => mainCamera.transform.position = originalPosition);
    }

    /// <summary>
    /// 화면 플래시
    /// </summary>
    public void Flash(Color color, float duration = 0.1f)
    {
        // Flash 오버레이 이미지가 있다고 가정
        var flashImage = GetComponent<UnityEngine.UI.Image>();
        if (flashImage != null)
        {
            flashImage.color = color;
            flashImage.DOFade(0f, duration);
        }
    }

    /// <summary>
    /// 슬로우 모션
    /// </summary>
    public void SlowMotion(float timeScale = 0.5f, float duration = 0.5f)
    {
        Time.timeScale = timeScale;
        DOVirtual.DelayedCall(duration, () => Time.timeScale = 1f, ignoreTimeScale: true);
    }

    /// <summary>
    /// 히트스톱 (타격감)
    /// </summary>
    public void HitStop(float duration = 0.1f)
    {
        Time.timeScale = 0f;
        DOVirtual.DelayedCall(duration, () => Time.timeScale = 1f, ignoreTimeScale: true);
    }
}
```

---

### 21.5 UI 트랜지션

#### **씬 전환 효과**

```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    public Image fadeImage;
    public float fadeDuration = 0.5f;

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

    /// <summary>
    /// 페이드 인
    /// </summary>
    public void FadeIn(System.Action onComplete = null)
    {
        fadeImage.color = Color.black;
        fadeImage.DOFade(0f, fadeDuration).OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 페이드 아웃
    /// </summary>
    public void FadeOut(System.Action onComplete = null)
    {
        fadeImage.DOFade(1f, fadeDuration).OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 씬 전환 (페이드 포함)
    /// </summary>
    public void LoadScene(string sceneName)
    {
        FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
            FadeIn();
        });
    }
}
```

---

#### **패널 슬라이드 애니메이션**

```csharp
using UnityEngine;
using DG.Tweening;

public class PanelSlider : MonoBehaviour
{
    public RectTransform panel;
    public float slideDuration = 0.3f;

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    void Start()
    {
        visiblePosition = panel.anchoredPosition;
        hiddenPosition = visiblePosition + new Vector2(0, -1000f); // 화면 아래

        // 초기에는 숨김
        panel.anchoredPosition = hiddenPosition;
    }

    public void Show()
    {
        panel.DOAnchorPos(visiblePosition, slideDuration).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        panel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InBack);
    }

    public void Toggle()
    {
        if (panel.anchoredPosition == visiblePosition)
            Hide();
        else
            Show();
    }
}
```

---

### 21.6 실전 예제: 카드 콤보 효과

```csharp
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ComboVFX : MonoBehaviour
{
    public GameObject comboTextPrefab;
    public Transform comboTextContainer;

    private int currentCombo = 0;

    public void AddCombo()
    {
        currentCombo++;

        if (currentCombo >= 3)
        {
            ShowComboEffect();
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
    }

    void ShowComboEffect()
    {
        // 콤보 텍스트 생성
        GameObject comboObj = Instantiate(comboTextPrefab, comboTextContainer);
        var textMesh = comboObj.GetComponent<TMPro.TextMeshProUGUI>();
        textMesh.text = $"{currentCombo} COMBO!";

        // 애니메이션 시퀀스
        Sequence sequence = DOTween.Sequence();

        // 1. 작게 시작
        comboObj.transform.localScale = Vector3.zero;

        // 2. 터지듯 확대
        sequence.Append(comboObj.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutBack));

        // 3. 약간 축소
        sequence.Append(comboObj.transform.DOScale(Vector3.one, 0.1f));

        // 4. 흔들기
        sequence.Append(comboObj.transform.DOShakeRotation(0.3f, strength: 20f));

        // 5. 페이드 아웃
        sequence.Append(textMesh.DOFade(0f, 0.3f).SetDelay(0.5f));

        // 6. 파괴
        sequence.OnComplete(() => Destroy(comboObj));

        // 화면 효과
        CameraEffects.Instance.Shake(intensity: 1f);
        VFXManager.Instance.PlayVFX(VFXManager.Instance.buffVFX, Vector3.zero);
    }
}
```

---

### 21.7 체크리스트

**DOTween:**
- [ ] DOTween 설치 및 설정
- [ ] 기본 문법 숙지
- [ ] Sequence 이해
- [ ] Ease 타입 실험

**VFX:**
- [ ] VFXManager 구현
- [ ] 카드 VFX 생성
- [ ] 전투 VFX 생성
- [ ] 데미지 텍스트 구현

**파티클:**
- [ ] 공격 파티클 생성
- [ ] 방어 파티클 생성
- [ ] 상태 이상 파티클 생성

**화면 효과:**
- [ ] 카메라 셰이크
- [ ] 화면 플래시
- [ ] 히트스톱
- [ ] 슬로우 모션

**UI 트랜지션:**
- [ ] 씬 전환 페이드
- [ ] 패널 슬라이드
- [ ] 버튼 애니메이션

---

# PART 8: 테스트 및 디버깅

## Chapter 22: Unity Test Framework

**목표**: Unity Test Framework를 사용하여 게임 로직의 정확성을 검증하고, 버그를 조기에 발견하는 방법을 학습합니다.

**예상 시간**: 3-4시간

---

### 22.1 테스트 프레임워크 개요

#### **테스트의 중요성**

```
[테스트가 없는 개발]
코드 작성 → 플레이 → 버그 발견 → 수정 → 플레이 → 또 다른 버그 발견 → ...
⏱️ 시간: 매우 오래 걸림
😰 스트레스: 높음
🐛 버그 누적: 많음

[테스트가 있는 개발]
코드 작성 → 테스트 실행 (5초) → 통과/실패 확인 → 수정 → 테스트 재실행
⏱️ 시간: 빠름
😊 스트레스: 낮음
🐛 버그 누적: 적음
```

---

#### **Unity Test Framework 구조**

```
[두 가지 테스트 모드]

1. EditMode Test (에디터 모드 테스트)
   - Unity Editor에서 실행
   - 게임 로직만 테스트 (MonoBehaviour 없이)
   - 빠른 실행 속도
   - 예: 데이터 클래스, 유틸리티 함수

2. PlayMode Test (플레이 모드 테스트)
   - 실제 게임 플레이 시뮬레이션
   - MonoBehaviour, Coroutine 사용 가능
   - 느린 실행 속도
   - 예: 전투 시스템, UI 상호작용
```

---

### 22.2 Test Runner 설정

#### **Step 1: Test Runner 열기**

```
1. Unity 메뉴: Window → General → Test Runner

2. Test Runner 창이 열리면 두 개의 탭이 보임:
   - EditMode
   - PlayMode
```

---

#### **Step 2: EditMode Test Assembly 생성**

```
1. Test Runner → EditMode 탭 선택

2. "Create EditMode Test Assembly Folder" 클릭

3. 폴더 생성:
   Assets/
     └── Tests/
         └── EditMode/
             └── EditModeTests.asmdef

4. EditModeTests.asmdef 설정:
   - Name: EditModeTests
   - References: (자동 설정됨)
   - Platforms: Editor
```

---

#### **Step 3: PlayMode Test Assembly 생성**

```
1. Test Runner → PlayMode 탭 선택

2. "Create PlayMode Test Assembly Folder" 클릭

3. 폴더 생성:
   Assets/
     └── Tests/
         └── PlayMode/
             └── PlayModeTests.asmdef

4. PlayModeTests.asmdef 설정:
   - Name: PlayModeTests
   - References: (자동 설정됨)
   - Platforms: Any Platform
```

---

#### **Step 4: 게임 코드 Assembly Definition 생성**

테스트에서 게임 코드를 참조하려면 Assembly Definition이 필요합니다.

```
1. Assets/Scripts 폴더에서 우클릭

2. Create → Assembly Definition

3. 이름: GameScripts.asmdef

4. 설정:
   - Name: GameScripts
   - Auto Referenced: ✅ 체크
   - Platforms: Any Platform

5. Apply
```

---

#### **Step 5: Test Assembly에서 게임 코드 참조**

```
1. EditModeTests.asmdef 선택

2. Inspector에서:
   - Assembly Definition References 섹션
   - "+" 버튼 클릭
   - GameScripts 선택

3. PlayModeTests.asmdef도 동일하게 설정

4. Apply
```

---

### 22.3 EditMode 단위 테스트 작성

#### **첫 번째 테스트: CardData 검증**

**파일**: `Assets/Tests/EditMode/CardDataTests.cs`

```csharp
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// CardData 클래스의 기본 기능을 테스트합니다.
/// </summary>
public class CardDataTests
{
    // === 기본 생성 테스트 ===

    [Test]
    public void CardData_CreateCard_HasCorrectValues()
    {
        // Arrange (준비)
        CardData card = new CardData
        {
            id = "card_001",
            nameKR = "일격필살",
            nameEN = "Fatal Strike",
            qiCost = 2,
            martialCost = 0,
            damage = 10
        };

        // Act (실행)
        // - 이 테스트는 생성만 확인하므로 별도 실행 없음

        // Assert (검증)
        Assert.AreEqual("card_001", card.id);
        Assert.AreEqual("일격필살", card.nameKR);
        Assert.AreEqual(10, card.damage);
    }

    // === Null 체크 테스트 ===

    [Test]
    public void CardData_NameIsNull_ThrowsException()
    {
        // Arrange
        CardData card = new CardData();

        // Assert
        Assert.IsNull(card.nameKR); // 초기값은 null이어야 함
    }

    // === 범위 검증 테스트 ===

    [Test]
    public void CardData_NegativeCost_IsInvalid()
    {
        // Arrange
        CardData card = new CardData
        {
            qiCost = -1 // 잘못된 값
        };

        // Assert
        Assert.Less(card.qiCost, 0); // 음수임을 확인
    }
}
```

---

#### **두 번째 테스트: CombatManager 로직**

**파일**: `Assets/Tests/EditMode/CombatLogicTests.cs`

```csharp
using NUnit.Framework;

/// <summary>
/// 전투 계산 로직을 테스트합니다.
/// </summary>
public class CombatLogicTests
{
    // === 데미지 계산 테스트 ===

    [Test]
    public void CalculateDamage_NoArmor_ReturnFullDamage()
    {
        // Arrange
        int baseDamage = 10;
        int armor = 0;

        // Act
        int finalDamage = CombatCalculator.CalculateDamage(baseDamage, armor);

        // Assert
        Assert.AreEqual(10, finalDamage);
    }

    [Test]
    public void CalculateDamage_WithArmor_ReturnsReducedDamage()
    {
        // Arrange
        int baseDamage = 10;
        int armor = 3;

        // Act
        int finalDamage = CombatCalculator.CalculateDamage(baseDamage, armor);

        // Assert
        Assert.AreEqual(7, finalDamage); // 10 - 3 = 7
    }

    [Test]
    public void CalculateDamage_ArmorExceedsDamage_ReturnsZero()
    {
        // Arrange
        int baseDamage = 5;
        int armor = 10;

        // Act
        int finalDamage = CombatCalculator.CalculateDamage(baseDamage, armor);

        // Assert
        Assert.AreEqual(0, finalDamage); // 최소 데미지는 0
    }

    // === 취약 상태 데미지 계산 ===

    [Test]
    public void CalculateDamage_WithVulnerable_IncreaseDamage()
    {
        // Arrange
        int baseDamage = 10;
        int armor = 0;
        bool isVulnerable = true;

        // Act
        int finalDamage = CombatCalculator.CalculateDamage(
            baseDamage, armor, isVulnerable
        );

        // Assert
        Assert.AreEqual(15, finalDamage); // 50% 증가
    }

    // === 크리티컬 히트 테스트 ===

    [Test]
    public void CalculateDamage_CriticalHit_DoublesDamage()
    {
        // Arrange
        int baseDamage = 10;
        bool isCritical = true;

        // Act
        int finalDamage = CombatCalculator.CalculateDamage(
            baseDamage, 0, false, isCritical
        );

        // Assert
        Assert.AreEqual(20, finalDamage);
    }
}
```

**필요한 헬퍼 클래스**: `CombatCalculator.cs`

```csharp
/// <summary>
/// 전투 계산을 담당하는 정적 유틸리티 클래스
/// (테스트를 위해 MonoBehaviour와 분리)
/// </summary>
public static class CombatCalculator
{
    public static int CalculateDamage(
        int baseDamage,
        int armor,
        bool isVulnerable = false,
        bool isCritical = false
    )
    {
        int damage = baseDamage;

        // 크리티컬
        if (isCritical)
            damage *= 2;

        // 취약
        if (isVulnerable)
            damage = Mathf.RoundToInt(damage * 1.5f);

        // 방어력
        damage -= armor;

        // 최소 0
        return Mathf.Max(0, damage);
    }
}
```

---

#### **세 번째 테스트: DeckManager**

**파일**: `Assets/Tests/EditMode/DeckManagerTests.cs`

```csharp
using NUnit.Framework;
using System.Collections.Generic;

/// <summary>
/// 덱 관리 로직을 테스트합니다.
/// </summary>
public class DeckManagerTests
{
    private DeckLogic deckLogic;

    // === 테스트 전 설정 ===

    [SetUp]
    public void Setup()
    {
        // 각 테스트 전에 실행됨
        deckLogic = new DeckLogic();
    }

    [TearDown]
    public void Teardown()
    {
        // 각 테스트 후에 실행됨
        deckLogic = null;
    }

    // === 덱 생성 테스트 ===

    [Test]
    public void CreateDeck_WithCards_HasCorrectCount()
    {
        // Arrange
        List<string> cardIds = new List<string>
        {
            "card_001", "card_002", "card_003"
        };

        // Act
        deckLogic.CreateDeck(cardIds);

        // Assert
        Assert.AreEqual(3, deckLogic.DrawPile.Count);
    }

    // === 카드 뽑기 테스트 ===

    [Test]
    public void DrawCard_FromFullDeck_ReturnsCard()
    {
        // Arrange
        deckLogic.CreateDeck(new List<string> { "card_001" });

        // Act
        string drawnCard = deckLogic.DrawCard();

        // Assert
        Assert.AreEqual("card_001", drawnCard);
        Assert.AreEqual(0, deckLogic.DrawPile.Count);
        Assert.AreEqual(1, deckLogic.Hand.Count);
    }

    [Test]
    public void DrawCard_FromEmptyDeck_ShufflesDiscardPile()
    {
        // Arrange
        deckLogic.CreateDeck(new List<string> { "card_001" });
        deckLogic.DrawCard(); // 덱을 비움
        deckLogic.DiscardCard("card_001"); // 버리기 더미로 이동

        // Act
        string drawnCard = deckLogic.DrawCard(); // 리셔플 발생

        // Assert
        Assert.AreEqual("card_001", drawnCard);
        Assert.AreEqual(0, deckLogic.DiscardPile.Count); // 버리기 더미가 비었음
    }

    // === 카드 버리기 테스트 ===

    [Test]
    public void DiscardCard_AddsToDiscardPile()
    {
        // Arrange
        deckLogic.Hand.Add("card_001");

        // Act
        deckLogic.DiscardCard("card_001");

        // Assert
        Assert.AreEqual(0, deckLogic.Hand.Count);
        Assert.AreEqual(1, deckLogic.DiscardPile.Count);
    }

    // === 셔플 테스트 ===

    [Test]
    public void ShuffleDeck_ChangesOrder()
    {
        // Arrange
        List<string> originalOrder = new List<string>
        {
            "card_001", "card_002", "card_003", "card_004", "card_005"
        };
        deckLogic.CreateDeck(originalOrder);

        // Act
        deckLogic.Shuffle();

        // Assert
        // 순서가 변경되었는지 확인 (항상 통과하진 않지만 대부분 통과)
        bool orderChanged = false;
        for (int i = 0; i < originalOrder.Count; i++)
        {
            if (deckLogic.DrawPile[i] != originalOrder[i])
            {
                orderChanged = true;
                break;
            }
        }
        Assert.IsTrue(orderChanged);
    }
}
```

**필요한 헬퍼 클래스**: `DeckLogic.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 덱 관리 로직 (MonoBehaviour와 분리하여 테스트 가능)
/// </summary>
public class DeckLogic
{
    public List<string> DrawPile { get; private set; } = new List<string>();
    public List<string> Hand { get; private set; } = new List<string>();
    public List<string> DiscardPile { get; private set; } = new List<string>();

    public void CreateDeck(List<string> cardIds)
    {
        DrawPile = new List<string>(cardIds);
        Shuffle();
    }

    public string DrawCard()
    {
        // 덱이 비었으면 버리기 더미를 셔플해서 덱으로
        if (DrawPile.Count == 0)
        {
            DrawPile = new List<string>(DiscardPile);
            DiscardPile.Clear();
            Shuffle();
        }

        if (DrawPile.Count == 0)
            return null;

        string card = DrawPile[0];
        DrawPile.RemoveAt(0);
        Hand.Add(card);
        return card;
    }

    public void DiscardCard(string cardId)
    {
        Hand.Remove(cardId);
        DiscardPile.Add(cardId);
    }

    public void Shuffle()
    {
        for (int i = 0; i < DrawPile.Count; i++)
        {
            int randomIndex = Random.Range(i, DrawPile.Count);
            string temp = DrawPile[i];
            DrawPile[i] = DrawPile[randomIndex];
            DrawPile[randomIndex] = temp;
        }
    }
}
```

---

### 22.4 PlayMode 통합 테스트

PlayMode 테스트는 실제 게임 환경에서 실행되므로, MonoBehaviour와 Coroutine을 사용할 수 있습니다.

#### **첫 번째 PlayMode 테스트: 카드 플레이**

**파일**: `Assets/Tests/PlayMode/CardPlayTests.cs`

```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// 실제 게임 환경에서 카드 플레이를 테스트합니다.
/// </summary>
public class CardPlayTests
{
    private GameObject combatManagerObj;
    private CombatManager combatManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // 씬 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene("CombatScene");
        yield return null;

        // CombatManager 찾기
        combatManagerObj = GameObject.Find("CombatManager");
        combatManager = combatManagerObj.GetComponent<CombatManager>();

        Assert.IsNotNull(combatManager);
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        // 씬 정리
        yield return null;
    }

    // === 카드 플레이 테스트 ===

    [UnityTest]
    public IEnumerator PlayCard_DamageCard_DealsCorrectDamage()
    {
        // Arrange
        CardData attackCard = new CardData
        {
            id = "card_attack",
            nameKR = "일격필살",
            damage = 10,
            qiCost = 2
        };

        EnemyController enemy = combatManager.GetEnemy(0);
        int initialHealth = enemy.currentHealth;

        // Act
        combatManager.PlayCard(attackCard, enemy);
        yield return new WaitForSeconds(1f); // 애니메이션 대기

        // Assert
        Assert.AreEqual(initialHealth - 10, enemy.currentHealth);
    }

    [UnityTest]
    public IEnumerator PlayCard_InsufficientEnergy_FailsToPlay()
    {
        // Arrange
        CardData expensiveCard = new CardData
        {
            id = "card_expensive",
            qiCost = 10 // 플레이어가 가진 것보다 많음
        };

        int handCountBefore = combatManager.hand.Count;

        // Act
        bool playSuccess = combatManager.PlayCard(expensiveCard, null);
        yield return null;

        // Assert
        Assert.IsFalse(playSuccess);
        Assert.AreEqual(handCountBefore, combatManager.hand.Count); // 손패 변화 없음
    }

    // === 연쇄 효과 테스트 ===

    [UnityTest]
    public IEnumerator PlayCard_DrawCard_IncreasesHandSize()
    {
        // Arrange
        CardData drawCard = new CardData
        {
            id = "card_draw",
            nameKR = "심호흡",
            qiCost = 1,
            drawCards = 2
        };

        int handCountBefore = combatManager.hand.Count;

        // Act
        combatManager.PlayCard(drawCard, null);
        yield return new WaitForSeconds(0.5f);

        // Assert
        // -1 (플레이한 카드) + 2 (뽑은 카드) = +1
        Assert.AreEqual(handCountBefore + 1, combatManager.hand.Count);
    }
}
```

---

#### **두 번째 PlayMode 테스트: UI 상호작용**

**파일**: `Assets/Tests/PlayMode/UIInteractionTests.cs`

```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

/// <summary>
/// UI 상호작용을 테스트합니다.
/// </summary>
public class UIInteractionTests
{
    [UnityTest]
    public IEnumerator EndTurnButton_WhenClicked_EndsTurn()
    {
        // Arrange
        UnityEngine.SceneManagement.SceneManager.LoadScene("CombatScene");
        yield return null;

        Button endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        CombatManager combat = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        int currentTurn = combat.currentTurn;

        // Act
        endTurnButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        // Assert
        Assert.AreEqual(currentTurn + 1, combat.currentTurn);
    }

    [UnityTest]
    public IEnumerator CardUI_WhenHovered_ShowsTooltip()
    {
        // Arrange
        UnityEngine.SceneManagement.SceneManager.LoadScene("CombatScene");
        yield return null;

        CardUI cardUI = GameObject.FindObjectOfType<CardUI>();
        GameObject tooltip = GameObject.Find("Tooltip");

        Assert.IsNotNull(cardUI);
        Assert.IsFalse(tooltip.activeSelf); // 초기에는 비활성화

        // Act
        cardUI.OnPointerEnter(null);
        yield return null;

        // Assert
        Assert.IsTrue(tooltip.activeSelf); // 호버 시 활성화
    }
}
```

---

### 22.5 테스트 실행 및 결과 확인

#### **테스트 실행 방법**

```
1. Test Runner 열기 (Window → General → Test Runner)

2. EditMode 탭:
   - "Run All" 클릭 → 모든 EditMode 테스트 실행
   - 개별 테스트 클릭 → "Run Selected" → 선택한 테스트만 실행

3. PlayMode 탭:
   - "Run All" 클릭 (실제 게임 플레이 시뮬레이션)
   - ⚠️ 주의: 시간이 오래 걸릴 수 있음

4. 결과 확인:
   ✅ 녹색 체크: 테스트 통과
   ❌ 빨간 X: 테스트 실패
```

---

#### **테스트 결과 해석**

```
[성공 예시]
✅ CardDataTests.CardData_CreateCard_HasCorrectValues (0.001s)
✅ CombatLogicTests.CalculateDamage_NoArmor_ReturnFullDamage (0.002s)
✅ DeckManagerTests.DrawCard_FromFullDeck_ReturnsCard (0.003s)

총 3개 테스트, 3개 통과, 0개 실패

[실패 예시]
❌ CombatLogicTests.CalculateDamage_WithArmor_ReturnsReducedDamage (0.005s)

   Expected: 7
   But was: 10

   at CombatLogicTests.CalculateDamage_WithArmor_ReturnsReducedDamage()

→ 방어력 계산 로직에 버그가 있음!
```

---

### 22.6 테스트 주도 개발 (TDD)

#### **TDD 사이클: Red-Green-Refactor**

```
1️⃣ RED: 실패하는 테스트 작성
   - 기능을 먼저 테스트로 정의
   - 아직 구현하지 않았으므로 실패함

2️⃣ GREEN: 최소한의 코드로 테스트 통과
   - 테스트를 통과하는 최소한의 코드 작성
   - 완벽하지 않아도 OK

3️⃣ REFACTOR: 코드 개선
   - 테스트가 통과하는 상태에서 리팩토링
   - 테스트가 계속 통과하는지 확인

🔁 반복
```

---

#### **TDD 실전 예제: 출혈 상태 구현**

**Step 1: RED - 실패하는 테스트 작성**

```csharp
[Test]
public void ApplyBleed_ToEnemy_DecreasesHealthOverTime()
{
    // Arrange
    Enemy enemy = new Enemy { currentHealth = 100 };

    // Act
    enemy.ApplyStatusEffect(new BleedEffect(stackCount: 3));
    enemy.ProcessTurnStart(); // 턴 시작 시 출혈 데미지

    // Assert
    Assert.AreEqual(97, enemy.currentHealth); // 3 출혈 = 3 데미지
}
```

실행 결과: ❌ (ApplyStatusEffect 메서드가 없음)

---

**Step 2: GREEN - 최소한의 코드 작성**

```csharp
public class Enemy
{
    public int currentHealth;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public void ApplyStatusEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
    }

    public void ProcessTurnStart()
    {
        foreach (var effect in statusEffects)
        {
            effect.Apply(this);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}

public abstract class StatusEffect
{
    public abstract void Apply(Enemy target);
}

public class BleedEffect : StatusEffect
{
    private int stackCount;

    public BleedEffect(int stackCount)
    {
        this.stackCount = stackCount;
    }

    public override void Apply(Enemy target)
    {
        target.TakeDamage(stackCount);
    }
}
```

실행 결과: ✅ 테스트 통과!

---

**Step 3: REFACTOR - 코드 개선**

```csharp
// 개선: 출혈은 매 턴 1씩 감소해야 함
public class BleedEffect : StatusEffect
{
    public int stackCount { get; private set; }

    public BleedEffect(int stackCount)
    {
        this.stackCount = stackCount;
    }

    public override void Apply(Enemy target)
    {
        target.TakeDamage(stackCount);
        stackCount--; // 매 턴 1 감소

        if (stackCount <= 0)
            isExpired = true; // 효과 만료
    }
}
```

추가 테스트 작성:

```csharp
[Test]
public void BleedEffect_DecreasesEachTurn()
{
    // Arrange
    Enemy enemy = new Enemy { currentHealth = 100 };
    BleedEffect bleed = new BleedEffect(3);

    // Act & Assert
    enemy.ApplyStatusEffect(bleed);

    enemy.ProcessTurnStart(); // 턴 1: 3 데미지
    Assert.AreEqual(97, enemy.currentHealth);
    Assert.AreEqual(2, bleed.stackCount);

    enemy.ProcessTurnStart(); // 턴 2: 2 데미지
    Assert.AreEqual(95, enemy.currentHealth);
    Assert.AreEqual(1, bleed.stackCount);

    enemy.ProcessTurnStart(); // 턴 3: 1 데미지
    Assert.AreEqual(94, enemy.currentHealth);
    Assert.AreEqual(0, bleed.stackCount);
    Assert.IsTrue(bleed.isExpired);
}
```

실행 결과: ✅ 모든 테스트 통과!

---

### 22.7 코드 커버리지 (Code Coverage)

#### **코드 커버리지란?**

```
코드 커버리지 = (테스트된 코드 줄 수 / 전체 코드 줄 수) × 100%

예:
전체 코드: 100줄
테스트된 코드: 80줄
커버리지: 80%
```

---

#### **Unity에서 코드 커버리지 측정**

```
1. Package Manager 열기

2. Code Coverage 패키지 설치:
   - Window → Package Manager
   - "Code Coverage" 검색
   - Install

3. 커버리지 측정:
   - Window → Analysis → Code Coverage
   - "Enable Code Coverage" 체크
   - Test Runner에서 테스트 실행
   - Code Coverage 창에서 결과 확인

4. 결과 해석:
   - 녹색: 테스트됨
   - 빨간색: 테스트 안됨
   - 목표: 80% 이상
```

---

### 22.8 실전 체크리스트

**테스트 설정:**
- [ ] Test Runner 열기
- [ ] EditMode Test Assembly 생성
- [ ] PlayMode Test Assembly 생성
- [ ] GameScripts.asmdef 생성 및 참조 설정

**EditMode 테스트:**
- [ ] CardData 테스트 작성
- [ ] CombatCalculator 테스트 작성
- [ ] DeckLogic 테스트 작성
- [ ] 모든 테스트 통과 확인

**PlayMode 테스트:**
- [ ] CombatScene 로드 테스트
- [ ] 카드 플레이 테스트
- [ ] UI 상호작용 테스트
- [ ] 모든 테스트 통과 확인

**TDD 실습:**
- [ ] 출혈 효과 TDD로 구현
- [ ] 중독 효과 TDD로 구현
- [ ] 약화 효과 TDD로 구현

**코드 커버리지:**
- [ ] Code Coverage 패키지 설치
- [ ] 커버리지 80% 이상 달성

---

**다음 챕터**: Chapter 23에서는 Unity Profiler와 디버깅 기법을 학습합니다!

---

## Chapter 23: 디버깅 기법

**목표**: 버그를 빠르게 찾아내고 해결하는 다양한 디버깅 기법을 마스터합니다.

**예상 시간**: 3-4시간

---

### 23.1 로깅 전략 (Logging)

#### **Debug.Log의 세 가지 레벨**

```csharp
using UnityEngine;

public class LoggingExample : MonoBehaviour
{
    void Start()
    {
        // === 일반 로그 (정보성) ===
        Debug.Log("게임 시작됨");
        Debug.Log($"카드 {cardCount}장 로드 완료");

        // === 경고 (Warning) ===
        Debug.LogWarning("에너지가 부족합니다!");
        Debug.LogWarning($"적 {enemyId}의 AI가 응답하지 않음");

        // === 에러 (Error) ===
        Debug.LogError("카드 데이터를 찾을 수 없습니다!");
        Debug.LogError($"NullReferenceException: {cardData}");

        // === 예외 (Exception) ===
        try
        {
            // 위험한 작업
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
```

**Console 창 필터링:**
```
1. Window → General → Console

2. 필터 버튼:
   - [ ] Clear: 로그 모두 삭제
   - [✓] Collapse: 중복 로그 합치기
   - [✓] Clear on Play: 플레이 시 로그 자동 삭제
   - [ ] Error Pause: 에러 발생 시 일시정지

3. 로그 레벨 필터:
   - [i] 정보 로그 보기/숨기기
   - [!] 경고 로그 보기/숨기기
   - [X] 에러 로그 보기/숨기기
```

---

#### **구조화된 로깅 시스템**

프로젝트가 커지면 로그가 너무 많아집니다. 구조화된 로깅 시스템을 만들어 관리하세요.

**파일**: `Assets/Scripts/Utils/GameLogger.cs`

```csharp
using UnityEngine;

/// <summary>
/// 구조화된 로깅 시스템
/// 카테고리별로 로그를 관리하고, 빌드에서는 자동으로 비활성화
/// </summary>
public static class GameLogger
{
    // === 로그 활성화 플래그 ===

    public static bool EnableCombatLogs = true;
    public static bool EnableCardLogs = true;
    public static bool EnableUILogs = true;
    public static bool EnableDataLogs = true;

    // === 카테고리별 로그 함수 ===

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Combat(string message)
    {
        if (EnableCombatLogs)
            Debug.Log($"<color=red>[COMBAT]</color> {message}");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Card(string message)
    {
        if (EnableCardLogs)
            Debug.Log($"<color=blue>[CARD]</color> {message}");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void UI(string message)
    {
        if (EnableUILogs)
            Debug.Log($"<color=green>[UI]</color> {message}");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Data(string message)
    {
        if (EnableDataLogs)
            Debug.Log($"<color=yellow>[DATA]</color> {message}");
    }

    // === 에러는 항상 출력 ===

    public static void Error(string category, string message)
    {
        Debug.LogError($"[{category}] ERROR: {message}");
    }

    // === 성능 측정 ===

    public static void Perf(string operation, float milliseconds)
    {
        if (milliseconds > 16.6f) // 60 FPS 기준
        {
            Debug.LogWarning($"<color=orange>[PERF]</color> {operation} took {milliseconds:F2}ms (>16.6ms!)");
        }
        else
        {
            Debug.Log($"<color=cyan>[PERF]</color> {operation} took {milliseconds:F2}ms");
        }
    }
}
```

**사용 예시:**

```csharp
public class CombatManager : MonoBehaviour
{
    void PlayCard(CardData card)
    {
        GameLogger.Card($"플레이: {card.nameKR} (비용: {card.qiCost})");

        if (currentQi < card.qiCost)
        {
            GameLogger.Error("COMBAT", "에너지 부족!");
            return;
        }

        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

        // 카드 효과 실행
        ExecuteCardEffects(card);

        sw.Stop();
        GameLogger.Perf("카드 효과 실행", (float)sw.Elapsed.TotalMilliseconds);
    }
}
```

**Console 출력:**
```
[CARD] 플레이: 일격필살 (비용: 2)
[PERF] 카드 효과 실행 took 2.35ms
```

---

### 23.2 Breakpoint 디버깅 (Visual Studio)

#### **Breakpoint 기본 사용법**

```
1. Visual Studio에서 코드 열기

2. 중단점 설정:
   - 코드 줄 번호 왼쪽 클릭 (빨간 점 생김)
   - 또는 F9 키

3. Unity Debugger 연결:
   - Visual Studio: Debug → Attach to Unity
   - Unity가 자동으로 감지됨

4. Unity에서 게임 플레이 (Play 버튼)

5. Breakpoint에 걸리면:
   - 코드 실행이 멈춤
   - 변수 값을 확인할 수 있음
```

---

#### **Breakpoint 예시**

**디버깅하려는 코드**: `CardManager.cs`

```csharp
public class CardManager : MonoBehaviour
{
    public void DrawCard()
    {
        // 🔴 여기에 Breakpoint 설정
        if (drawPile.Count == 0)
        {
            // 🔴 여기에도 Breakpoint 설정
            ReshuffleDeck();
        }

        CardData card = drawPile[0]; // 🔴 여기에도
        drawPile.RemoveAt(0);
        hand.Add(card);

        Debug.Log($"Drew card: {card.nameKR}");
    }
}
```

**Breakpoint에서 확인할 것:**
```
1. drawPile.Count 값은 얼마인가?
2. drawPile[0]이 정말 존재하는가?
3. card 변수가 null이 아닌가?
4. hand.Add가 성공했는가?
```

---

#### **조건부 Breakpoint (Conditional Breakpoint)**

특정 조건에서만 중단하고 싶을 때 사용합니다.

```
1. Breakpoint에서 우클릭

2. "Conditions..." 선택

3. 조건 입력:
   예: drawPile.Count == 0
   예: card.id == "card_legendary_001"
   예: currentHealth <= 0

4. OK
```

**예시:**

```csharp
public void TakeDamage(int damage)
{
    currentHealth -= damage; // 🔴 Breakpoint: currentHealth <= 0

    if (currentHealth <= 0)
    {
        Die();
    }
}
```

이렇게 하면 `currentHealth`가 0 이하일 때만 중단됩니다.

---

#### **디버그 도구**

Breakpoint에 걸린 상태에서 사용할 수 있는 도구들:

```
1. Watch 창 (디버그 → 창 → 조사식):
   - 변수 추가: drawPile.Count
   - 표현식 추가: hand.Count + drawPile.Count
   - 실시간으로 값 변화 추적

2. Locals 창 (디버그 → 창 → 로컬):
   - 현재 스코프의 모든 지역 변수 표시
   - this, card, drawPile 등

3. Call Stack 창 (디버그 → 창 → 호출 스택):
   - 어떤 함수에서 이 함수를 호출했는지 추적
   - 예: Main() → CombatManager.StartTurn() → DrawCard()

4. Immediate 창 (디버그 → 창 → 직접 실행):
   - 코드 실행 중 명령어 입력
   - 예: Debug.Log(card.nameKR)
   - 예: drawPile.Count
```

---

### 23.3 Unity Profiler (성능 분석)

#### **Profiler 열기**

```
1. Unity 메뉴: Window → Analysis → Profiler

2. 게임 플레이 (Play 버튼)

3. Profiler가 자동으로 데이터 수집 시작
```

---

#### **CPU Usage (CPU 사용량)**

```
[Profiler - CPU Usage 탭]

Time ms
  30 |     ┌──┐
  25 |     │  │
  20 |  ┌──┘  └──┐
  15 |  │        │
  10 |──┘        └──
   0 └───────────────
     Frame 120-150

⚠️ 빨간색 선: 16.6ms (60 FPS 기준)
✅ 이 선 아래로 유지해야 함
```

**주요 항목:**
```
Rendering          (렌더링)
Scripts            (스크립트 실행)
Physics            (물리 연산)
Animation          (애니메이션)
GC.Collect         (가비지 컬렉션)
```

**문제 발견:**
```
✅ Scripts가 10ms 이상: 스크립트 최적화 필요
✅ Rendering이 15ms 이상: Draw Call 줄이기
✅ GC.Collect 자주 발생: 메모리 할당 줄이기
```

---

#### **Memory (메모리 사용량)**

```
[Profiler - Memory 탭]

Total Allocated: 250 MB
  - Textures: 120 MB
  - Meshes: 30 MB
  - Audio: 40 MB
  - Scripts: 20 MB
  - Other: 40 MB
```

**문제 발견:**
```
✅ Textures가 너무 큼: Sprite Atlas 사용
✅ Audio가 너무 큼: 압축 설정 확인
✅ 메모리가 계속 증가: 메모리 누수 의심
```

---

#### **Rendering (렌더링)**

```
[Profiler - Rendering 탭]

Draw Calls: 250
Batches: 80
Tris: 45K
Verts: 32K
```

**목표:**
```
Draw Calls: < 100 (모바일: < 50)
Batches: < 50
Tris: < 100K (모바일: < 50K)
```

**Draw Call 줄이기:**
```
1. Sprite Atlas 사용 (여러 스프라이트를 하나로)
2. Static Batching 활성화
3. UI 레이어 최소화
```

---

#### **Profiler 실전 예시**

**문제**: 카드를 뽑을 때마다 프레임 드롭 발생

**Step 1: Profiler에서 확인**

```
Frame 150: 25ms (너무 느림!)
  - Scripts: 20ms ← 문제 발견!
    - CardManager.DrawCard: 18ms
```

**Step 2: Deep Profile 활성화**

```
Profiler → Deep Profile 체크 → 게임 플레이

CardManager.DrawCard: 18ms
  - Instantiate(cardPrefab): 15ms ← 주범!
  - UpdateUI: 3ms
```

**Step 3: 문제 해결**

```csharp
// 문제 코드 (매번 Instantiate)
public void DrawCard()
{
    GameObject cardObj = Instantiate(cardPrefab); // 느림!
    // ...
}

// 해결 코드 (Object Pooling)
public void DrawCard()
{
    GameObject cardObj = cardPool.Get(); // 빠름!
    // ...
}
```

**Step 4: 결과 확인**

```
Frame 150: 5ms (개선!)
  - Scripts: 2ms
    - CardManager.DrawCard: 0.5ms
```

---

### 23.4 Frame Debugger (렌더링 디버깅)

#### **Frame Debugger 사용법**

```
1. Window → Analysis → Frame Debugger

2. Enable 클릭

3. 게임 플레이 → 일시정지

4. 프레임별로 렌더링 순서 확인:
   - Draw Call 1: 배경
   - Draw Call 2: 적 1
   - Draw Call 3: 적 2
   - Draw Call 4: 카드 1
   - ...
```

**문제 발견:**
```
✅ 같은 Material인데 Draw Call이 여러 개: Batching 실패
✅ UI가 여러 번 나뉘어 렌더링: Canvas 구조 문제
✅ 투명 오브젝트가 너무 많음: Overdraw 문제
```

---

### 23.5 Conditional Compilation (조건부 컴파일)

#### **빌드 타입에 따라 코드 활성화/비활성화**

```csharp
using UnityEngine;

public class ConditionalExample : MonoBehaviour
{
    void Start()
    {
        // === Unity Editor에서만 실행 ===
        #if UNITY_EDITOR
            Debug.Log("에디터에서만 보임");
        #endif

        // === 개발 빌드에서만 실행 ===
        #if DEVELOPMENT_BUILD
            Debug.Log("개발 빌드에서만 보임");
        #endif

        // === 릴리스 빌드에서는 제외 ===
        #if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            // 릴리스 코드
        #endif

        // === 플랫폼별 ===
        #if UNITY_STANDALONE
            Debug.Log("PC 빌드");
        #elif UNITY_ANDROID
            Debug.Log("안드로이드 빌드");
        #elif UNITY_IOS
            Debug.Log("iOS 빌드");
        #endif
    }

    // === 메서드 레벨 조건부 컴파일 ===

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    void EditorOnlyMethod()
    {
        // 이 메서드는 에디터에서만 존재
        // 빌드에서는 호출 코드까지 완전히 제거됨
        Debug.Log("에디터 전용 메서드");
    }
}
```

---

#### **커스텀 Define Symbol**

```
1. Edit → Project Settings → Player

2. Scripting Define Symbols:
   - ENABLE_CHEATS
   - DEBUG_MODE
   - TESTING

3. Apply
```

**사용 예시:**

```csharp
public class CheatManager : MonoBehaviour
{
    void Update()
    {
        #if ENABLE_CHEATS
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GainGold(1000);
            Debug.Log("치트: 골드 1000 획득");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            HealFull();
            Debug.Log("치트: 체력 완전 회복");
        }
        #endif
    }

    #if ENABLE_CHEATS
    void GainGold(int amount)
    {
        GameManager.Instance.gold += amount;
    }

    void HealFull()
    {
        GameManager.Instance.playerHealth = GameManager.Instance.playerMaxHealth;
    }
    #endif
}
```

릴리스 빌드에서는 `ENABLE_CHEATS`를 제거하면, 치트 코드가 완전히 사라집니다.

---

### 23.6 Gizmos와 Debug Draw (시각적 디버깅)

#### **Gizmos로 게임 오브젝트 시각화**

```csharp
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float attackRange = 3f;
    public float detectionRange = 10f;

    // === Gizmos: Scene 뷰에서만 보이는 디버그 드로잉 ===
    void OnDrawGizmos()
    {
        // 탐지 범위 (노란색 와이어 구)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위 (빨간색 와이어 구)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // === 선택된 오브젝트에서만 Gizmos 표시 ===
    void OnDrawGizmosSelected()
    {
        // 시야 방향 (파란색 선)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}
```

**Scene 뷰에서 확인:**
```
[Scene 뷰]

      🎯 Player
       ↑
       │
    ╱─────╲  ← 탐지 범위 (노란색)
   ╱   😈   ╲ ← Enemy
  │    ↓    │
   ╲───────╱
    ╲─ ─╱    ← 공격 범위 (빨간색)
```

---

#### **Debug.DrawLine/Ray (게임 중 시각화)**

```csharp
using UnityEngine;

public class RaycastExample : MonoBehaviour
{
    void Update()
    {
        // 플레이어 앞으로 Raycast
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            // 충돌 시 빨간색 선
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            // 충돌 지점 표시
            Debug.DrawLine(hit.point, hit.point + Vector3.up, Color.green);
        }
        else
        {
            // 충돌 없으면 흰색 선
            Debug.DrawRay(transform.position, transform.forward * 10f, Color.white);
        }
    }
}
```

**Game 뷰 + Scene 뷰에서 확인:**
```
- Scene 뷰에서 선이 보임
- Game 뷰에서는 안 보임 (Gizmos 버튼 클릭 시 보임)
```

---

### 23.7 메모리 프로파일러 (Memory Profiler)

#### **Memory Profiler 패키지 설치**

```
1. Window → Package Manager

2. "Memory Profiler" 검색

3. Install

4. Window → Analysis → Memory Profiler
```

---

#### **메모리 스냅샷 촬영**

```
1. Memory Profiler 창 열기

2. "Capture" 버튼 클릭 → 현재 메모리 상태 저장

3. 게임 플레이 (예: 10번 전투)

4. 다시 "Capture" 버튼 클릭

5. "Compare Snapshots" → 메모리 변화 확인
```

---

#### **메모리 누수 발견 예시**

**문제**: 전투가 끝나도 메모리가 계속 증가

**Step 1: 스냅샷 비교**

```
Snapshot 1 (전투 전):  200 MB
Snapshot 2 (전투 10번 후): 350 MB

변화:
+ Texture2D: +50 MB ← 이상함!
+ AudioClip: +30 MB ← 이상함!
+ GameObject: +1000개 ← 문제!
```

**Step 2: 원인 파악**

```
GameObject 1000개:
- CardUI (100개)
- EnemyUI (50개)
- DamageText (850개) ← 주범!
```

**Step 3: 코드 확인**

```csharp
// 문제 코드
public void ShowDamage(int damage)
{
    GameObject damageText = Instantiate(damageTextPrefab);
    damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();

    // ❌ Destroy를 안 함!
}

// 해결 코드
public void ShowDamage(int damage)
{
    GameObject damageText = Instantiate(damageTextPrefab);
    damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();

    // ✅ 2초 후 삭제
    Destroy(damageText, 2f);
}
```

---

### 23.8 일반적인 버그와 해결 방법

#### **NullReferenceException**

**증상:**
```
NullReferenceException: Object reference not set to an instance of an object
CardManager.PlayCard (CardData card) (at Assets/Scripts/CardManager.cs:45)
```

**원인:**
```csharp
public class CardManager : MonoBehaviour
{
    public CombatManager combatManager; // Inspector에서 할당 안 함!

    void PlayCard(CardData card)
    {
        combatManager.ApplyDamage(card.damage); // ← Null!
    }
}
```

**해결 1: Inspector에서 할당**
```
Hierarchy → CardManager → Inspector → Combat Manager 할당
```

**해결 2: 자동으로 찾기**
```csharp
void Awake()
{
    if (combatManager == null)
        combatManager = FindObjectOfType<CombatManager>();
}
```

**해결 3: Null 체크**
```csharp
void PlayCard(CardData card)
{
    if (combatManager == null)
    {
        Debug.LogError("CombatManager가 할당되지 않음!");
        return;
    }

    combatManager.ApplyDamage(card.damage);
}
```

---

#### **IndexOutOfRangeException**

**증상:**
```
IndexOutOfRangeException: Index was outside the bounds of the array.
DeckManager.DrawCard () (at Assets/Scripts/DeckManager.cs:78)
```

**원인:**
```csharp
public void DrawCard()
{
    CardData card = drawPile[0]; // ← drawPile이 비어있음!
    drawPile.RemoveAt(0);
    hand.Add(card);
}
```

**해결:**
```csharp
public void DrawCard()
{
    if (drawPile.Count == 0)
    {
        Debug.LogWarning("덱이 비어있어 리셔플합니다.");
        ReshuffleDeck();
    }

    if (drawPile.Count == 0)
    {
        Debug.LogError("리셔플 후에도 덱이 비어있음!");
        return;
    }

    CardData card = drawPile[0];
    drawPile.RemoveAt(0);
    hand.Add(card);
}
```

---

#### **MissingReferenceException (Destroyed 오브젝트 참조)**

**증상:**
```
MissingReferenceException: The object of type 'CardUI' has been destroyed but you are still trying to access it.
```

**원인:**
```csharp
public class CardUI : MonoBehaviour
{
    void OnDestroy()
    {
        // 카드 UI가 파괴됨
    }
}

public class HandManager : MonoBehaviour
{
    List<CardUI> cardsInHand = new List<CardUI>();

    void DiscardAll()
    {
        foreach (var card in cardsInHand)
        {
            Destroy(card.gameObject); // 카드 파괴
        }

        // 나중에...
        cardsInHand[0].UpdateUI(); // ← 이미 파괴된 오브젝트 접근!
    }
}
```

**해결:**
```csharp
void DiscardAll()
{
    foreach (var card in cardsInHand)
    {
        Destroy(card.gameObject);
    }

    cardsInHand.Clear(); // ✅ 리스트도 비우기
}
```

---

### 23.9 디버깅 체크리스트

**로깅:**
- [ ] GameLogger 시스템 구현
- [ ] 카테고리별 로그 활성화/비활성화
- [ ] 빌드에서 로그 자동 제거 (Conditional 사용)

**Breakpoint 디버깅:**
- [ ] Visual Studio에서 Unity Debugger 연결
- [ ] 주요 함수에 Breakpoint 설정
- [ ] Watch 창으로 변수 추적
- [ ] Call Stack으로 호출 경로 확인

**Profiler:**
- [ ] CPU Usage 확인 (16.6ms 이하 유지)
- [ ] Memory 사용량 확인
- [ ] Rendering Draw Call 최적화
- [ ] GC.Collect 빈도 줄이기

**시각적 디버깅:**
- [ ] Gizmos로 범위 표시
- [ ] Debug.DrawLine으로 Raycast 시각화
- [ ] Frame Debugger로 렌더링 순서 확인

**메모리 누수:**
- [ ] Memory Profiler로 스냅샷 비교
- [ ] Instantiate 후 Destroy 확인
- [ ] 이벤트 리스너 제거 확인

---

**다음 챕터**: Chapter 24에서는 성능 최적화 기법을 학습합니다!

---

## Chapter 24: 성능 최적화

**목표**: 게임이 60 FPS로 부드럽게 실행되도록 최적화 기법을 적용합니다.

**예상 시간**: 4-5시간

---

### 24.1 성능 목표 설정

#### **프레임 타임 예산**

```
60 FPS = 16.6ms/frame
30 FPS = 33.3ms/frame

[16.6ms 예산 배분 예시]

스크립트:      5ms  (30%)
렌더링:        7ms  (42%)
물리:          2ms  (12%)
애니메이션:    1ms  (6%)
오디오:        0.5ms (3%)
기타:          1.1ms (7%)
────────────────────
총합:          16.6ms (100%)
```

---

#### **성능 측정 도구**

```csharp
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// 성능 측정 유틸리티
/// </summary>
public class PerformanceMonitor : MonoBehaviour
{
    private float deltaTime = 0f;
    private bool showStats = true;

    void Update()
    {
        // FPS 계산
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // F3 키로 토글
        if (Input.GetKeyDown(KeyCode.F3))
            showStats = !showStats;
    }

    void OnGUI()
    {
        if (!showStats) return;

        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = Color.white;

        float ms = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = $"{fps:0.} FPS ({ms:0.0} ms)";

        // 색상 코드: 60fps 이상 = 녹색, 30-60 = 노란색, 30 이하 = 빨간색
        if (fps >= 55)
            style.normal.textColor = Color.green;
        else if (fps >= 30)
            style.normal.textColor = Color.yellow;
        else
            style.normal.textColor = Color.red;

        GUI.Label(rect, text, style);
    }
}
```

---

### 24.2 Object Pooling (오브젝트 풀링)

#### **문제: Instantiate/Destroy의 성능 비용**

```csharp
// ❌ 나쁜 예: 매번 생성/파괴 (느림!)
void ShowDamage(int damage)
{
    GameObject damageText = Instantiate(damageTextPrefab);
    damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
    Destroy(damageText, 1f);
}

// 전투 중 50번 호출 → 50번 Instantiate → 느림!
```

**성능 비용:**
```
Instantiate: 약 1-3ms
Destroy: 약 0.5-1ms
총합: 1.5-4ms per call

50번 호출 = 75-200ms = 프레임 드롭!
```

---

#### **해결: Object Pooling**

**파일**: `Assets/Scripts/Utils/ObjectPool.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 범용 오브젝트 풀
/// Instantiate/Destroy 대신 재사용으로 성능 향상
/// </summary>
public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Queue<T> pool = new Queue<T>();
    private Transform parent;

    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        // 초기 풀 생성
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// 풀에서 오브젝트 가져오기
    /// </summary>
    public T Get()
    {
        T obj;

        if (pool.Count > 0)
        {
            // 풀에 있으면 재사용
            obj = pool.Dequeue();
        }
        else
        {
            // 풀이 비었으면 새로 생성
            obj = Object.Instantiate(prefab, parent);
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 오브젝트를 풀로 반환
    /// </summary>
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    /// <summary>
    /// 전체 풀 클리어
    /// </summary>
    public void Clear()
    {
        while (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            Object.Destroy(obj.gameObject);
        }
    }
}
```

---

#### **사용 예시: 카드 풀**

**파일**: `Assets/Scripts/Card/CardPoolManager.cs`

```csharp
using UnityEngine;

/// <summary>
/// 카드 UI 오브젝트 풀 매니저
/// </summary>
public class CardPoolManager : MonoBehaviour
{
    public static CardPoolManager Instance { get; private set; }

    [Header("Prefabs")]
    public CardUI cardUIPrefab;

    [Header("Pool Settings")]
    public int initialCardPoolSize = 20;

    private ObjectPool<CardUI> cardPool;

    void Awake()
    {
        Instance = this;

        // 카드 풀 생성
        cardPool = new ObjectPool<CardUI>(
            cardUIPrefab,
            initialCardPoolSize,
            transform
        );
    }

    public CardUI GetCard()
    {
        return cardPool.Get();
    }

    public void ReturnCard(CardUI card)
    {
        cardPool.Return(card);
    }
}
```

**사용:**

```csharp
public class HandManager : MonoBehaviour
{
    public void DrawCard(CardData data)
    {
        // ✅ 좋은 예: 풀에서 가져오기 (빠름!)
        CardUI card = CardPoolManager.Instance.GetCard();
        card.Initialize(data);
        hand.Add(card);
    }

    public void DiscardCard(CardUI card)
    {
        hand.Remove(card);

        // ✅ 풀로 반환 (재사용)
        CardPoolManager.Instance.ReturnCard(card);
    }
}
```

**성능 개선:**
```
Before: 50번 Instantiate = 75-200ms
After:  50번 Pool.Get() = 1-3ms

약 25-70배 빠름!
```

---

#### **시간 제한 풀 반환 (Auto-Return)**

```csharp
using UnityEngine;
using System.Collections;

/// <summary>
/// 일정 시간 후 자동으로 풀로 반환되는 컴포넌트
/// </summary>
public class PooledObject : MonoBehaviour
{
    private ObjectPool<PooledObject> pool;
    private Coroutine returnCoroutine;

    public void SetPool(ObjectPool<PooledObject> pool)
    {
        this.pool = pool;
    }

    public void ReturnAfter(float seconds)
    {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        returnCoroutine = StartCoroutine(ReturnCoroutine(seconds));
    }

    IEnumerator ReturnCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pool.Return(this);
    }
}

// 사용 예시
void ShowDamageText(int damage)
{
    var damageText = damageTextPool.Get();
    damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
    damageText.ReturnAfter(1.5f); // 1.5초 후 자동 반환
}
```

---

### 24.3 메모리 최적화

#### **Sprite Atlas (스프라이트 아틀라스)**

**문제**: 개별 스프라이트는 각각 Draw Call 발생

```
[개별 스프라이트]
card_01.png → Draw Call 1
card_02.png → Draw Call 2
card_03.png → Draw Call 3
...
card_20.png → Draw Call 20

총 20개 Draw Call
```

**해결**: Sprite Atlas로 합치기

```
[Sprite Atlas]
cards_atlas.png (모든 카드 포함)
 ├─ card_01
 ├─ card_02
 ├─ card_03
 └─ ... card_20

총 1개 Draw Call!
```

---

#### **Sprite Atlas 생성**

```
1. Assets/Sprites 폴더 생성

2. 우클릭 → Create → 2D → Sprite Atlas

3. 이름: CardsAtlas

4. Objects for Packing:
   - "Select" 버튼 클릭
   - Assets/Sprites/Cards 폴더 선택
   - ✅ Include in Build 체크

5. Settings:
   - Max Texture Size: 2048
   - Format: Automatic
   - Compression: Normal Quality

6. Pack Preview → "Pack" 버튼 클릭
```

**코드에서 사용:**

```csharp
using UnityEngine;
using UnityEngine.U2D;

public class CardUI : MonoBehaviour
{
    [Header("Atlas")]
    public SpriteAtlas cardsAtlas;

    public void SetCardSprite(string cardId)
    {
        // Atlas에서 스프라이트 가져오기
        Sprite sprite = cardsAtlas.GetSprite(cardId);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
```

---

#### **텍스처 압축**

```
1. 스프라이트 선택

2. Inspector:
   - Texture Type: Sprite (2D and UI)
   - Max Size: 적절한 크기 선택
     • 카드: 512x512
     • 배경: 2048x2048
     • 아이콘: 128x128
   - Compression: High Quality (PC), Normal Quality (모바일)
   - Use Crunch Compression: ✅ 체크 (빌드 크기 감소)

3. Apply
```

**메모리 절약:**
```
Before: 2048x2048 RGBA32 = 16 MB
After:  2048x2048 Compressed = 2 MB

8배 절약!
```

---

#### **오디오 압축**

```
1. 오디오 클립 선택

2. Inspector:

[BGM (배경음악)]
- Load Type: Streaming
- Compression Format: Vorbis
- Quality: 70%

[SFX (효과음)]
- Load Type: Decompress On Load
- Compression Format: ADPCM
- Quality: 100%

[보이스]
- Load Type: Compressed In Memory
- Compression Format: Vorbis
- Quality: 50%

3. Apply
```

**메모리 절약:**
```
Before: WAV 10MB
After:  Vorbis 1MB

10배 절약!
```

---

### 24.4 렌더링 최적화

#### **Draw Call 줄이기**

**문제 확인:**

```
1. Window → Analysis → Frame Debugger

2. Enable

3. 게임 플레이

4. Draw Call 개수 확인:
   - SetPass calls: 250 ← 너무 많음!
```

---

#### **해결 1: Static Batching (정적 배칭)**

움직이지 않는 오브젝트는 Static으로 표시하면 자동으로 배칭됩니다.

```
1. 배경, UI 프레임 등 선택

2. Inspector → Static 체크박스 ✅

3. Batching Static 선택

4. Yes, change children
```

**효과:**
```
Before: 배경 100개 → 100 Draw Calls
After:  배경 100개 → 1 Draw Call (Static Batching)
```

---

#### **해결 2: Dynamic Batching (동적 배칭)**

```
1. Edit → Project Settings → Player

2. Other Settings → Dynamic Batching ✅ 체크

조건:
- 같은 Material 사용
- Vertex 개수 < 300
- 같은 Sorting Layer
```

---

#### **해결 3: Canvas 최적화**

```
[문제]
Canvas
 ├─ Panel 1
 │   └─ Text 1 (변화함)
 ├─ Panel 2
 │   └─ Text 2 (변화함)
 └─ Panel 3
     └─ Text 3 (고정)

→ Text 1 변경 시 Canvas 전체 재구성 (느림!)

[해결]
Canvas (Static UI)
 └─ Panel 3
     └─ Text 3 (고정)

Canvas (Dynamic UI)
 ├─ Panel 1
 │   └─ Text 1 (변화함)
 └─ Panel 2
     └─ Text 2 (변화함)

→ Dynamic Canvas만 재구성 (빠름!)
```

**규칙:**
```
1. 자주 변하는 UI → 별도 Canvas
2. 고정된 UI → 별도 Canvas
3. Canvas 개수: 3-5개 적당
```

---

### 24.5 스크립트 최적화

#### **Update() 최적화**

**문제: 매 프레임 호출 (60 FPS = 60번/초)**

```csharp
// ❌ 나쁜 예: 매 프레임 불필요한 계산
void Update()
{
    // 카메라는 변하지 않는데 매 프레임 찾음!
    Camera cam = Camera.main;

    // 매 프레임 GetComponent (느림!)
    transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

    // 매 프레임 Find (매우 느림!)
    GameObject player = GameObject.Find("Player");
}
```

---

**해결: 캐싱 및 조건 체크**

```csharp
// ✅ 좋은 예: 캐싱 및 최적화
public class OptimizedExample : MonoBehaviour
{
    // 캐싱
    private Camera mainCam;
    private Rigidbody rb;
    private GameObject player;

    void Awake()
    {
        // 한 번만 찾기
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // 필요할 때만 실행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // 재사용
        rb.velocity = Vector3.zero;
    }
}
```

---

#### **LINQ 사용 주의**

```csharp
// ❌ 나쁜 예: LINQ는 GC 발생
void Update()
{
    var aliveEnemies = enemies.Where(e => e.isAlive).ToList(); // 매 프레임 GC!
}

// ✅ 좋은 예: 전통적인 루프
void Update()
{
    aliveEnemies.Clear();
    for (int i = 0; i < enemies.Count; i++)
    {
        if (enemies[i].isAlive)
            aliveEnemies.Add(enemies[i]);
    }
}
```

---

#### **박싱/언박싱 회피**

```csharp
// ❌ 나쁜 예: 박싱 발생
void LogHealth(int health)
{
    Debug.Log("Health: " + health); // int → object 박싱!
}

// ✅ 좋은 예: string interpolation
void LogHealth(int health)
{
    Debug.Log($"Health: {health}"); // 박싱 없음
}
```

---

#### **코루틴 vs Update**

```csharp
// ❌ 나쁜 예: 매 프레임 체크
void Update()
{
    if (Time.time - lastCheckTime > 1f)
    {
        CheckEnemies();
        lastCheckTime = Time.time;
    }
}

// ✅ 좋은 예: 코루틴
IEnumerator Start()
{
    while (true)
    {
        CheckEnemies();
        yield return new WaitForSeconds(1f); // 1초마다만 실행
    }
}
```

---

### 24.6 물리 최적화

#### **Fixed Timestep 조정**

```
1. Edit → Project Settings → Time

2. Fixed Timestep: 0.02 (기본값)
   → 50번/초 물리 업데이트

3. 카드 게임은 물리를 많이 안 쓰므로:
   Fixed Timestep: 0.04
   → 25번/초 (충분함)
```

---

#### **레이어 충돌 매트릭스**

```
1. Edit → Project Settings → Physics 2D

2. Layer Collision Matrix:
   - Player vs Enemy: ✅
   - Player vs PlayerBullet: ❌ (불필요)
   - Enemy vs Enemy: ❌ (불필요)
   - Card vs Card: ❌ (불필요)

불필요한 충돌 체크 제거 = 성능 향상
```

---

### 24.7 빌드 크기 최적화

#### **Code Stripping**

```
1. Edit → Project Settings → Player

2. Other Settings → Managed Stripping Level:
   - Disabled: 빌드 큼, 빠름
   - Low: 중간
   - Medium: 추천 (균형)
   - High: 빌드 작음, 느림

3. Medium 선택
```

**효과:**
```
Before: 150 MB
After:  80 MB

47% 감소!
```

---

#### **압축 방식**

```
1. File → Build Settings

2. Compression Method:
   - None: 압축 없음 (빠른 로딩, 큰 용량)
   - LZ4: 추천 (빠른 압축해제, 중간 용량)
   - LZ4HC: 느린 압축, 작은 용량
   - LZMA: 매우 느린 압축해제, 가장 작은 용량

3. LZ4 선택 (게임용 추천)
```

---

### 24.8 성능 프로파일링 워크플로우

#### **Step-by-Step 최적화 프로세스**

```
1️⃣ 측정 (Measure)
   - Profiler 실행
   - 가장 느린 부분 찾기

2️⃣ 분석 (Analyze)
   - CPU? Memory? Rendering?
   - 어느 함수가 문제?

3️⃣ 최적화 (Optimize)
   - Object Pooling
   - 캐싱
   - Sprite Atlas
   - 등등...

4️⃣ 재측정 (Re-measure)
   - 개선되었는가?
   - 목표 달성?

5️⃣ 반복 (Repeat)
```

---

#### **실전 예시: 카드 드로우 최적화**

**문제**: 카드 5장 뽑을 때 프레임 드롭

**Step 1: Profiler 확인**

```
Frame 120: 45ms (너무 느림!)
  - CardManager.DrawCards: 40ms
    - Instantiate: 30ms
    - UpdateUI: 10ms
```

**Step 2: 원인 파악**

```csharp
void DrawCards(int count)
{
    for (int i = 0; i < count; i++)
    {
        GameObject cardObj = Instantiate(cardPrefab); // ← 30ms!
        CardUI cardUI = cardObj.GetComponent<CardUI>();
        cardUI.UpdateUI(cardData[i]); // ← 10ms!
    }
}
```

**Step 3: Object Pooling 적용**

```csharp
void DrawCards(int count)
{
    for (int i = 0; i < count; i++)
    {
        CardUI cardUI = cardPool.Get(); // ← 0.5ms!
        cardUI.UpdateUI(cardData[i]);
    }
}
```

**Step 4: UpdateUI 최적화**

```csharp
// Before: 10ms
public void UpdateUI(CardData data)
{
    nameText.text = data.nameKR; // 2ms
    descText.text = data.desc; // 2ms
    costText.text = data.cost.ToString(); // 1ms
    iconImage.sprite = Resources.Load<Sprite>(data.iconPath); // 5ms! ← 문제

    UpdateLayout(); // 레이아웃 재계산
}

// After: 2ms
public void UpdateUI(CardData data)
{
    nameText.text = data.nameKR;
    descText.text = data.desc;
    costText.text = data.cost.ToString();
    iconImage.sprite = iconAtlas.GetSprite(data.iconPath); // 0.1ms!

    // 레이아웃은 한 번만
    LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
}
```

**Step 5: 결과**

```
Before: 45ms
After:  3ms

15배 빠름!
```

---

### 24.9 성능 체크리스트

**Object Pooling:**
- [ ] CardUI 풀 구현
- [ ] DamageText 풀 구현
- [ ] VFX 풀 구현
- [ ] 적 풀 구현 (필요 시)

**메모리:**
- [ ] Sprite Atlas 생성 (카드, UI, 배경)
- [ ] 텍스처 압축 설정
- [ ] 오디오 압축 설정
- [ ] Resources.UnloadUnusedAssets() 호출

**렌더링:**
- [ ] Draw Call < 100 달성
- [ ] Static Batching 적용
- [ ] Dynamic Batching 활성화
- [ ] Canvas 분리 (Static/Dynamic)

**스크립트:**
- [ ] Update() 최적화 (캐싱)
- [ ] GetComponent 캐싱
- [ ] LINQ 제거 (핫 패스)
- [ ] 박싱/언박싱 회피

**물리:**
- [ ] Fixed Timestep 조정 (0.04)
- [ ] Layer Collision Matrix 최적화
- [ ] Rigidbody Sleeping 활용

**빌드:**
- [ ] Code Stripping: Medium
- [ ] Compression: LZ4
- [ ] Splash Screen 최적화
- [ ] 빌드 크기 < 200MB

**프로파일링:**
- [ ] CPU < 10ms (60 FPS)
- [ ] Memory < 500MB
- [ ] GC.Collect < 5ms
- [ ] 프레임 드롭 없음

---

**다음 챕터**: PART 9에서는 빌드 및 배포를 학습합니다!

---

# PART 9: 빌드 및 배포

## Chapter 25: 빌드 설정 및 최적화

**목표**: 플레이어에게 배포할 수 있는 완성된 빌드를 생성합니다.

**예상 시간**: 2-3시간

---

### 25.1 Build Settings (빌드 설정)

#### **빌드 설정 창 열기**

```
1. Unity 메뉴: File → Build Settings

2. Build Settings 창이 열림:
   ┌────────────────────────────────────┐
   │ Platform:                          │
   │  [✓] PC, Mac & Linux Standalone   │
   │  [ ] iOS                           │
   │  [ ] Android                       │
   │  [ ] WebGL                         │
   │                                    │
   │ Scenes In Build:                   │
   │  [✓] MainMenu                      │
   │  [✓] CombatScene                   │
   │  [✓] MapScene                      │
   │                                    │
   │ [Build] [Build And Run]            │
   └────────────────────────────────────┘
```

---

#### **Step 1: 플랫폼 선택**

```
[PC 게임 배포]

1. Platform 목록에서 "PC, Mac & Linux Standalone" 선택

2. "Switch Platform" 클릭 (이미 선택되어 있으면 생략)

3. Target Platform 선택:
   - Windows ✅ (추천)
   - Mac OS X
   - Linux

4. Architecture:
   - Windows: x86_64 (64-bit)
   - Mac: Apple Silicon or Intel
```

---

#### **Step 2: 씬 추가**

```
빌드에 포함할 씬을 추가합니다.

방법 1: Drag & Drop
- Hierarchy에서 씬 선택
- Build Settings 창으로 드래그

방법 2: Add Open Scenes
- 씬을 열어둔 상태
- "Add Open Scenes" 버튼 클릭

순서 정렬:
- 첫 번째 씬 = 게임 시작 씬 (MainMenu)
- 드래그로 순서 변경 가능

체크박스:
- ✅ = 빌드에 포함
- ❌ = 빌드에서 제외
```

---

### 25.2 Player Settings (플레이어 설정)

#### **Player Settings 열기**

```
1. Build Settings → "Player Settings..." 버튼

2. Inspector 창에 Player Settings 표시

3. 주요 탭:
   - Company / Product
   - Icon
   - Resolution and Presentation
   - Splash Image
   - Other Settings
   - Publishing Settings
```

---

#### **Company / Product Name**

```
[Company]
Company Name: "YourStudio"
- Steam, itch.io에 표시되는 개발사 이름
- 나중에 변경 어려우므로 신중히 선택

[Product]
Product Name: "강호무적"
- 게임 제목
- Windows 설치 폴더명에 사용
- 예: C:\Program Files\YourStudio\강호무적\

Default Icon:
- 512x512 PNG 파일
- 게임 실행 파일(.exe) 아이콘
- 투명 배경 사용 가능
```

---

#### **Version (버전 관리)**

```
Version: "0.1.0"

버전 표기법 (Semantic Versioning):
Major.Minor.Patch

예:
0.1.0 = 프로토타입 (Phase 1)
0.2.0 = 수직 슬라이스 (Phase 2)
0.5.0 = 알파 (Phase 3)
0.9.0 = 베타 (Phase 4)
1.0.0 = 정식 출시 (Phase 5)

1.1.0 = 첫 번째 콘텐츠 업데이트
1.1.1 = 버그 수정 패치
```

---

#### **Resolution and Presentation (해상도 및 표시)**

```
[Fullscreen Mode]
- Fullscreen Window (추천)
  • 전체화면이지만 Alt+Tab 빠름
  • 창 모드와 전환 쉬움
- Exclusive Fullscreen
  • 진정한 전체화면
  • 성능 최적
- Windowed
  • 창 모드

[Default Screen Size]
Width: 1920
Height: 1080

[Resizable Window]
✅ 체크 (플레이어가 창 크기 조절 가능)

[Supported Aspect Ratios]
16:9 ✅ (1920x1080, 2560x1440, 3840x2160)
16:10 ✅ (1920x1200)
21:9 ✅ (3440x1440, 초광각 모니터)
```

---

#### **Splash Image (스플래시 화면)**

```
[Unity Logo]
Show Unity Logo: ❌ (Unity Pro/Plus만 가능)

[Splash Screen]
Show Splash Screen: ✅

Application Config Dialog:
- Mode: Enabled (플레이어가 설정 선택 가능)
  • 해상도 선택
  • 그래픽 품질 선택
  • 전체화면/창모드 선택

Preview:
[Preview] 버튼으로 스플래시 화면 미리보기
```

---

#### **Icon (아이콘)**

```
[Default Icon]
512x512 PNG 권장

[Icon 생성 가이드]

1. Photoshop/GIMP/Figma에서 512x512 캔버스 생성

2. 게임 로고/심볼 디자인:
   - 심플한 디자인 (작은 크기에서도 식별 가능)
   - 강렬한 색상
   - 투명 배경 (PNG)

3. Export:
   - 512x512 PNG
   - 256x256 PNG (백업)

4. Unity에서 설정:
   - Default Icon에 512x512 PNG 드래그
   - Override for PC: 256x256 (선택사항)

예시 (강호무적):
- 검 + 주먹 심볼
- 빨간색/금색 그라디언트
- 한자 "武" (무) 배경
```

---

### 25.3 Other Settings (기타 설정)

#### **Rendering (렌더링)**

```
[Color Space]
- Gamma (기본값, 빠름)
- Linear (더 사실적, 느림)

카드 게임 → Gamma 추천

[Auto Graphics API]
✅ 체크 (자동 선택)

[Static Batching]
✅ 체크 (Draw Call 감소)

[Dynamic Batching]
✅ 체크 (성능 향상)
```

---

#### **Scripting (스크립팅)**

```
[Scripting Backend]
- Mono (기본값, 호환성 좋음)
- IL2CPP (더 빠름, 빌드 느림)

첫 배포 → Mono 추천

[API Compatibility Level]
- .NET Framework (많은 기능)
- .NET Standard 2.1 (추천, 균형)

[Managed Stripping Level]
- Disabled (빌드 크지만 안전)
- Low
- Medium ✅ (추천, 균형)
- High (빌드 작지만 위험)

Medium 선택 → 빌드 크기 50% 감소!
```

---

#### **Configuration (구성)**

```
[Scripting Define Symbols]
개발/릴리스 모드 분리용

개발 빌드:
DEVELOPMENT_BUILD;ENABLE_CHEATS

릴리스 빌드:
(비워둠)

코드에서 사용:
```

```csharp
#if DEVELOPMENT_BUILD
    Debug.Log("개발 빌드입니다");
    ShowDebugMenu();
#endif

#if ENABLE_CHEATS
    if (Input.GetKeyDown(KeyCode.F1))
        GainGold(1000);
#endif
```

---

### 25.4 빌드 실행

#### **Development Build vs Release Build**

```
[Development Build]
용도: 테스트, 디버깅
특징:
✅ Profiler 연결 가능
✅ 디버그 로그 출력
✅ 빌드 빠름
❌ 성능 느림
❌ 파일 크기 큼

체크:
- [✓] Development Build
- [✓] Autoconnect Profiler
- [✓] Script Debugging

---

[Release Build]
용도: 실제 배포
특징:
✅ 성능 최적화
✅ 파일 크기 작음
✅ 보안 강화
❌ 디버깅 불가

체크:
- [ ] Development Build
```

---

#### **빌드 프로세스**

```
Step 1: 빌드 폴더 생성
- 프로젝트 폴더 밖에 생성
- 예: D:\Builds\Murim-Deckbuilder\

Step 2: Build Settings → Build
- "Build" 버튼 클릭 (빌드만)
- "Build And Run" 버튼 클릭 (빌드 후 실행)

Step 3: 폴더 선택
- Builds/Murim-Deckbuilder/ 선택
- "Select Folder" 클릭

Step 4: 빌드 대기
[빌드 진행 중...]
Step 1/5: Compiling scripts...
Step 2/5: Building scenes...
Step 3/5: Building player...
Step 4/5: Compressing data...
Step 5/5: Finalizing build...

완료!

Step 5: 빌드 결과 확인
Builds/
  └── Murim-Deckbuilder/
      ├── 강호무적.exe (실행 파일)
      ├── UnityPlayer.dll
      ├── UnityCrashHandler64.exe
      └── 강호무적_Data/
          ├── Managed/ (C# 코드)
          ├── Resources/ (리소스)
          └── level0, level1... (씬 데이터)
```

---

#### **빌드 시간 단축**

```
[빌드 시간 문제]
첫 빌드: 10-30분 (프로젝트 크기에 따라)
재빌드: 2-10분

[빌드 시간 단축 팁]

1. 증분 빌드 (Incremental Build)
   - Edit → Preferences → Build
   - Build App Bundle (Google Android AAB): OFF

2. Compression 방식 변경
   - File → Build Settings
   - Compression Method: LZ4 (빠름)
   - LZ4HC/LZMA는 느림

3. Asset Bundle 사용
   - 자주 변하지 않는 에셋 → Asset Bundle
   - 빌드 시간 50% 감소

4. SSD 사용
   - HDD: 20분
   - SSD: 5분 (4배 빠름!)
```

---

### 25.5 빌드 테스트

#### **테스트 체크리스트**

```
빌드 후 반드시 테스트!

[ ] 게임 실행 확인
    - .exe 더블클릭
    - 스플래시 화면 표시
    - 메인 메뉴 로드

[ ] 씬 전환 확인
    - 메인 메뉴 → 전투
    - 전투 → 맵
    - 맵 → 상점

[ ] 세이브/로드 확인
    - 게임 저장
    - 종료 후 재실행
    - 세이브 데이터 로드

[ ] 해상도 변경 확인
    - 1920x1080
    - 1280x720
    - 전체화면/창모드 전환

[ ] 설정 확인
    - 볼륨 조절
    - 그래픽 품질 변경
    - 키 바인딩

[ ] 성능 확인
    - F3으로 FPS 표시
    - 60 FPS 유지 확인
    - 프레임 드롭 없는지 확인

[ ] 크래시 테스트
    - 10분 이상 플레이
    - 빠르게 씬 전환
    - Alt+Tab 여러 번
```

---

### 25.6 빌드 에러 해결

#### **일반적인 빌드 에러**

**에러 1: Scenes in build is empty**

```
Error building Player: Scenes in build is empty

원인: 빌드에 씬이 하나도 없음

해결:
1. File → Build Settings
2. Scenes In Build에 씬 추가
3. MainMenu 씬부터 추가
```

---

**에러 2: Missing Assembly Reference**

```
error CS0246: The type or namespace name 'TextMeshPro' could not be found

원인: Package가 빌드에 포함 안 됨

해결:
1. Window → Package Manager
2. TextMeshPro 확인 및 설치
3. Reimport All
```

---

**에러 3: Build target not installed**

```
Build target 'Windows' not installed

원인: Unity Hub에서 플랫폼 모듈 미설치

해결:
1. Unity Hub 열기
2. Installs → 버전 옆 톱니바퀴
3. Add Modules
4. Windows Build Support 체크
5. Install
```

---

### 25.7 빌드 배포 준비

#### **배포 전 최종 체크리스트**

```
[ ] 버전 번호 업데이트
    - Player Settings → Version
    - 예: 0.1.0 → 0.2.0

[ ] 빌드 타입 확인
    - Development Build: ❌ 체크 해제
    - Release 모드로 빌드

[ ] 로그 제거
    - Debug.Log 제거 또는 Conditional
    - 치트 코드 제거

[ ] 크레딧 추가
    - 메인 메뉴 → Credits
    - 사용한 에셋 출처 명시

[ ] README.txt 작성
    - 시스템 요구사항
    - 설치 방법
    - 알려진 이슈
    - 연락처

[ ] 라이선스 확인
    - 사용한 에셋의 라이선스
    - Font 라이선스
    - Audio 라이선스

[ ] 압축 및 백업
    - 빌드 폴더 ZIP 압축
    - 버전별로 보관
    - 예: Murim-v0.2.0-Windows.zip
```

---

#### **README.txt 템플릿**

```
===================================
강호무적 (Murim Deckbuilder)
Version 0.2.0
===================================

[게임 소개]
무협 세계관의 덱빌딩 로그라이크 카드 게임입니다.
강호의 패자가 되어 적들을 물리치세요!

[시스템 요구사항]
최소 사양:
- OS: Windows 10 (64-bit)
- Processor: Intel Core i3 / AMD Ryzen 3
- Memory: 4 GB RAM
- Graphics: GTX 660 / Radeon HD 7850
- Storage: 500 MB available space

권장 사양:
- OS: Windows 10/11 (64-bit)
- Processor: Intel Core i5 / AMD Ryzen 5
- Memory: 8 GB RAM
- Graphics: GTX 1060 / Radeon RX 580
- Storage: 500 MB available space

[설치 방법]
1. ZIP 파일 압축 해제
2. 강호무적.exe 실행

[조작법]
- 마우스: 카드 선택 및 플레이
- ESC: 메뉴 열기
- F3: FPS 표시 (디버그)

[알려진 이슈]
- 일부 해상도에서 UI가 잘릴 수 있음
- 세이브 슬롯 3개로 제한

[문의]
Email: your.email@example.com
Discord: https://discord.gg/yourserver
GitHub: https://github.com/yourusername/murim-deckbuilder

[크레딧]
개발: YourStudio
폰트: 나눔스퀘어 (네이버)
음악: [출처]
효과음: [출처]

[라이선스]
이 게임은 [라이선스]로 배포됩니다.

===================================
© 2025 YourStudio. All rights reserved.
===================================
```

---

### 25.8 빌드 체크리스트

**빌드 설정:**
- [ ] 플랫폼: PC, Mac & Linux Standalone
- [ ] 씬 추가: MainMenu, CombatScene, MapScene
- [ ] Build 순서 확인 (MainMenu가 첫 번째)

**Player Settings:**
- [ ] Company Name 설정
- [ ] Product Name: 강호무적
- [ ] Version: 0.x.0
- [ ] Icon: 512x512 PNG
- [ ] Splash Screen 설정

**Other Settings:**
- [ ] Color Space: Gamma
- [ ] Scripting Backend: Mono
- [ ] Managed Stripping Level: Medium
- [ ] Static/Dynamic Batching 활성화

**빌드 실행:**
- [ ] Development Build ❌ (릴리스용)
- [ ] Compression: LZ4
- [ ] 빌드 폴더 선택 및 빌드

**빌드 테스트:**
- [ ] 게임 실행 확인
- [ ] 씬 전환 확인
- [ ] 세이브/로드 확인
- [ ] 60 FPS 유지 확인
- [ ] 크래시 없는지 10분 플레이

**배포 준비:**
- [ ] README.txt 작성
- [ ] 라이선스 확인
- [ ] ZIP 압축
- [ ] 버전별 백업

---

**다음 챕터**: Chapter 26에서는 Steam 배포 방법을 학습합니다!

---

## Chapter 26: Steam 배포 가이드

**목표**: Steam에 게임을 출시하여 전 세계 플레이어에게 배포합니다.

**예상 시간**: 4-6시간 (심사 대기 시간 제외)

**비용**: $100 (Steam Direct 수수료, 1회 결제)

---

### 26.1 Steamworks 파트너 등록

#### **Step 1: Steam 계정 준비**

```
1. Steam 계정 생성 (https://store.steampowered.com)

2. Steam Guard 활성화 (필수):
   - 모바일 인증 or 이메일 인증
   - 보안 강화

3. 계정 제한 해제:
   - $5 이상 구매 이력 필요
   - 또는 Steam Wallet에 $5 충전
```

---

#### **Step 2: Steamworks 파트너 프로그램 가입**

```
1. https://partner.steamgames.com 접속

2. "Sign In" → Steam 계정 로그인

3. "Get Started" 클릭

4. 약관 동의:
   - Steam Distribution Agreement 읽기
   - 동의 체크박스 ✅
   - "I Agree" 클릭

5. 회사/개인 정보 입력:

   [Company Information]
   - Company Name: YourStudio
   - Country: South Korea
   - Address: 상세 주소
   - Phone: 연락처

   [Tax Information]
   - Tax ID (사업자등록번호 or 주민등록번호)
   - W-8BEN 양식 작성 (미국 외 거주자)

6. 결제 정보:
   - Credit Card 등록
   - PayPal 연동 (선택)

7. Steam Direct 수수료 결제:
   - $100 USD
   - 신용카드 or PayPal
   - 게임 1개당 $100 (환불 불가)
   - ⚠️ 게임이 $1,000 이상 수익을 내면 환급
```

---

### 26.2 앱(게임) 생성

#### **Step 1: 새 앱 생성**

```
1. Steamworks 파트너 홈 (https://partner.steamgames.com)

2. "Apps & Packages" → "All Applications"

3. "Create New App" 버튼 클릭

4. 앱 정보 입력:

   App Name: 강호무적 (Murim Deckbuilder)
   - 영문 권장 (검색 최적화)
   - 나중에 변경 가능

   App Type: Game

   Select Package:
   - Create new package ✅

5. "Create" 클릭

6. App ID 부여받음:
   - 예: App ID 123456
   - 이 ID는 고유하며 변경 불가
   - SDK 통합 시 사용
```

---

#### **Step 2: 앱 설정**

```
1. App Admin → "Edit Steamworks Settings"

2. [General Installation]

   Installation Folder: murim-deckbuilder
   - C:\Program Files (x86)\Steam\steamapps\common\murim-deckbuilder\

   Launch Options:
   - Executable: 강호무적.exe
   - Launch Type: Default
   - Operating System: Windows

3. [Supported Languages]

   ✅ English (기본 필수)
   ✅ Korean (한국어)

   - 나중에 추가 가능:
     • Japanese
     • Simplified Chinese
     • Traditional Chinese

4. [Cloud]

   Enable Steam Cloud: ✅
   - 세이브 파일 자동 동기화
   - Maximum Bytes: 100 MB (충분)

   Files to Sync:
   - Path: SaveData/*.sav
   - Pattern: *.sav
```

---

### 26.3 Store Page 작성

#### **Required Assets (필수 에셋)**

```
[Header Capsule]
- 크기: 460 x 215 px
- 포맷: PNG, JPG
- 용도: 스토어 메인 이미지
- 디자인: 게임 로고 + 핵심 비주얼

[Library Capsule]
- 크기: 600 x 900 px
- 포맷: PNG, JPG
- 용도: Steam 라이브러리
- 디자인: 세로 포스터 스타일

[Hero Capsule]
- 크기: 1920 x 620 px (선택사항)
- 포맷: PNG, JPG
- 용도: 특집 페이지
- 디자인: 와이드 배너

[Screenshots]
- 최소 5장, 권장 10장
- 크기: 1920 x 1080 px (16:9)
- 포맷: PNG, JPG
- 내용:
  1. 전투 화면 (카드 플레이)
  2. 손패 + 적 UI
  3. 맵 화면
  4. 상점 화면
  5. 유물 선택
  6. 카드 업그레이드
  7. 보스 전투
  8. 메인 메뉴
  9. 설정 화면
  10. 크레딧

[Trailer Video]
- 길이: 30초 - 2분
- 해상도: 1920 x 1080 (Full HD)
- 포맷: MP4
- 내용:
  • 0-5초: 로고 + 타이틀
  • 5-30초: 핵심 게임플레이
  • 30-50초: 다양한 기능
  • 50-60초: 출시일 + CTA

YouTube 업로드 후 URL 입력
```

---

#### **Store Description (스토어 설명)**

```
[Short Description] (영문, 최대 300자)

Enter the world of martial arts in this deckbuilding roguelike!
Build your ultimate deck, master powerful techniques, and
conquer the Murim. Each run offers unique cards, relics, and
challenges. Will you become the supreme martial artist?

무협 세계의 덱빌딩 로그라이크! 강력한 무술을 익히고,
최강의 덱을 구성하여 강호를 정복하세요.

---

[Long Description] (영문/한글, 최대 8,000자)

# About This Game

**강호무적 (Murim Deckbuilder)** is a deckbuilding roguelike
card game set in a vibrant martial arts world.

## Core Features

### 🎴 Strategic Deckbuilding
- 70+ unique cards across 5 martial disciplines
- Qi and Martial Energy dual-resource system
- Synergize cards for devastating combos

### ⚔️ Intense Combat
- Face diverse enemies with unique AI patterns
- 15 challenging boss encounters
- Master timing and resource management

### 🗺️ Procedural Map Generation
- Every run is unique
- Multiple paths and choices
- Risk vs. reward decisions

### 💎 Powerful Relics
- 50+ relics with game-changing effects
- Build around relic synergies
- Create unstoppable combinations

### 📈 Meta Progression
- Unlock new cards and relics
- Permanent upgrades
- Multiple playable characters (coming soon)

## Game Modes

- **Standard Run**: Complete all 5 regions
- **Daily Challenge**: Compete on leaderboards
- **Endless Mode**: How far can you go?

## Martial Arts Disciplines

1. **Inner Power (내공)**: Qi manipulation and defense
2. **Sword Arts (검술)**: Precision strikes
3. **Fist Arts (권법)**: Raw power attacks
4. **Movement Arts (신법)**: Dodge and counter
5. **Forbidden Arts (사술)**: High risk, high reward

---

[한글 설명]

# 게임 정보

**강호무적**은 무협 세계관을 배경으로 한 덱빌딩 로그라이크
카드 게임입니다.

## 핵심 특징

### 🎴 전략적 덱 구성
- 5가지 무술 계열의 70장 이상의 카드
- 내공과 무기술 이중 자원 시스템
- 카드 시너지를 활용한 강력한 콤보

### ⚔️ 치열한 전투
- 독특한 AI 패턴을 가진 다양한 적
- 15종의 도전적인 보스 전투
- 타이밍과 자원 관리의 숙달

[계속...]
```

---

#### **Pricing (가격 설정)**

```
[Price]

권장 가격 (인디 카드 게임):
- $9.99 USD (약 13,000원)
- $14.99 USD (약 20,000원)
- $19.99 USD (약 26,000원)

강호무적 권장:
- Base Price: $14.99 USD
- 이유: 중간 가격대, 콘텐츠 양 적절

[Regional Pricing]

Steam 자동 변환 (권장):
✅ Use suggested prices

한국:
- USD $14.99 → KRW ₩16,500

지역별 조정:
- 아르헨티나: -70% (경제 고려)
- 중국: -30%
- 러시아: -50%

[Discounts]

출시 할인 (선택):
- Launch Week: -10% ($13.49)
- 첫 구매 유도

계절 세일:
- Summer Sale: -25%
- Winter Sale: -30%
- Spring Sale: -20%
```

---

### 26.4 빌드 업로드

#### **SteamPipe (Steam 빌드 업로드 도구)**

**Step 1: Steamworks SDK 다운로드**

```
1. Steamworks 파트너 → "Downloads" → "Steamworks SDK"

2. 다운로드 및 압축 해제:
   - steamworks_sdk_xxx.zip
   - 압축 해제: D:\SteamworksSDK\

3. 폴더 구조:
   SteamworksSDK/
     ├── tools/
     │   └── ContentBuilder/
     │       ├── builder/
     │       │   └── steamcmd.exe
     │       ├── scripts/
     │       └── content/
     └── sdk/
```

---

**Step 2: 빌드 스크립트 작성**

**파일**: `D:\SteamworksSDK\tools\ContentBuilder\scripts\app_build_123456.vdf`

```vdf
"AppBuild"
{
    "AppID" "123456"  // 여기에 실제 App ID 입력
    "Desc" "Murim Deckbuilder v0.2.0"  // 빌드 설명

    "BuildOutput" "D:\SteamworksSDK\output\"  // 빌드 로그 저장 위치

    "ContentRoot" "D:\Builds\Murim-Deckbuilder\"  // 게임 빌드 폴더

    "SetLive" "default"  // 빌드 배포 브랜치

    "Depots"
    {
        "123457"  // Depot ID (App Admin에서 확인)
        {
            "FileMapping"
            {
                "LocalPath" "*"  // 모든 파일
                "DepotPath" "."  // Steam에서의 경로
                "Recursive" "1"  // 하위 폴더 포함
            }
        }
    }
}
```

---

**Step 3: Depot 설정**

```
1. Steamworks 파트너 → App Admin → "Depots"

2. "Add New Depot" 클릭

3. Depot 정보:
   - Name: Windows Content
   - Depot ID: 자동 생성 (예: 123457)
   - Operating System: Windows
   - Architecture: 64-bit

4. 저장
```

---

**Step 4: 빌드 업로드 실행**

```
1. 명령 프롬프트 열기 (관리자 권한)

2. ContentBuilder 폴더로 이동:
   cd D:\SteamworksSDK\tools\ContentBuilder\

3. 업로드 명령 실행:
   builder\steamcmd.exe +login your_steam_username +run_app_build ..\scripts\app_build_123456.vdf +quit

4. Steam 로그인:
   - Username: your_steam_username
   - Password: ********
   - Steam Guard Code: XXXXX (모바일 인증)

5. 업로드 진행:
   [업로드 중...]
   Uploading depot 123457...
   Progress: [=========>    ] 65%

   완료!
   Build ID: 9876543 created successfully

6. 업로드 시간:
   - 500 MB 게임: 5-15분 (인터넷 속도에 따라)
```

---

#### **빌드 브랜치 설정**

```
[Beta Branches]

용도: 테스트, 얼리 액세스, 레거시 버전

1. Steamworks → App Admin → "Builds" → "Branches"

2. 브랜치 생성:

   Branch Name: "beta"
   Description: "Beta Testing Branch"
   Password: "test123" (선택사항)

   Build to Live: [선택 안 함]

3. 빌드 할당:
   - beta 브랜치 → Build ID 9876543 선택
   - Set Live

4. 플레이어 접근:
   - Steam → 게임 우클릭 → Properties
   - Betas → 코드 입력: test123
   - beta 브랜치 선택

[Main Branch]

- 일반 플레이어용
- "default" 브랜치
- 가장 안정적인 빌드만 배포
```

---

### 26.5 Steamworks 기능 통합 (선택사항)

#### **Steamworks.NET 설치**

```
1. GitHub에서 다운로드:
   https://github.com/rlabrecque/Steamworks.NET/releases

2. Unity Package 임포트:
   - Steamworks.NET.unitypackage 다운로드
   - Assets → Import Package → Custom Package
   - Steamworks.NET.unitypackage 선택
   - Import All

3. steam_appid.txt 생성:
   - 프로젝트 루트에 파일 생성
   - 내용: 123456 (실제 App ID 입력)
   - 이 파일은 에디터 테스트용 (빌드에서 제외)
```

---

#### **기본 Steam 초기화**

**파일**: `Assets/Scripts/Steam/SteamManager.cs`

```csharp
using UnityEngine;
using Steamworks;

/// <summary>
/// Steam 초기화 및 관리
/// Steamworks.NET 기본 제공 스크립트 사용
/// </summary>
public class SteamInitializer : MonoBehaviour
{
    private static SteamInitializer instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Steam 초기화
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam 초기화 실패! Steam이 실행 중인지 확인하세요.");
            return;
        }

        Debug.Log($"Steam 초기화 성공! User: {SteamFriends.GetPersonaName()}");

        // App ID 확인
        Debug.Log($"App ID: {SteamUtils.GetAppID()}");
    }

    void OnDestroy()
    {
        // Steam 종료
        SteamAPI.Shutdown();
    }
}
```

---

#### **Steam 업적 (Achievements)**

**파일**: `Assets/Scripts/Steam/SteamAchievements.cs`

```csharp
using UnityEngine;
using Steamworks;

/// <summary>
/// Steam 업적 관리
/// </summary>
public class SteamAchievements : MonoBehaviour
{
    public static SteamAchievements Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 업적 잠금 해제
    /// </summary>
    public void UnlockAchievement(string achievementID)
    {
        if (!SteamManager.Initialized)
            return;

        // 업적 설정
        SteamUserStats.SetAchievement(achievementID);

        // Steam에 전송
        SteamUserStats.StoreStats();

        Debug.Log($"업적 잠금 해제: {achievementID}");
    }

    /// <summary>
    /// 업적 달성 여부 확인
    /// </summary>
    public bool IsAchievementUnlocked(string achievementID)
    {
        if (!SteamManager.Initialized)
            return false;

        bool achieved;
        SteamUserStats.GetAchievement(achievementID, out achieved);
        return achieved;
    }
}
```

**사용 예시:**

```csharp
// 첫 승리 업적
public void OnFirstVictory()
{
    SteamAchievements.Instance.UnlockAchievement("ACH_FIRST_VICTORY");
}

// 보스 처치 업적
public void OnBossDefeated(string bossID)
{
    if (bossID == "boss_final")
    {
        SteamAchievements.Instance.UnlockAchievement("ACH_FINAL_BOSS");
    }
}
```

**Steamworks 설정:**

```
1. Steamworks → App Admin → "Stats & Achievements"

2. "Add New Achievement" 클릭

3. 업적 정보:
   - API Name: ACH_FIRST_VICTORY
   - Display Name (English): First Victory
   - Display Name (Korean): 첫 승리
   - Description (English): Win your first combat
   - Description (Korean): 첫 전투에서 승리하기
   - Icon: 업로드 (64x64 PNG)
   - Hidden: No

4. 저장 및 Publish
```

---

### 26.6 출시 준비 및 체크리스트

#### **출시 전 최종 점검**

```
[ ] Store Page 작성 완료
    - Header Capsule (460x215)
    - Library Capsule (600x900)
    - Screenshots (최소 5장)
    - Trailer Video
    - Short Description
    - Long Description
    - Tags (최소 5개)

[ ] 가격 설정
    - Base Price 결정
    - Regional Pricing 확인
    - Discount 계획 (선택)

[ ] 빌드 업로드
    - SteamPipe로 업로드 완료
    - default 브랜치에 배포
    - 빌드 테스트 (Steam에서 다운로드 후 실행)

[ ] Steamworks 설정
    - Launch Options 확인
    - Supported Languages
    - Steam Cloud 활성화
    - Achievements 설정 (선택)

[ ] Community Hub
    - 토론 게시판 모니터링 준비
    - 가이드 작성 (선택)
    - Workshop 설정 (선택)

[ ] 출시일 설정
    - Release Date 선택
    - 권장: 2-4주 후 (마케팅 준비)
    - Coming Soon 페이지 먼저 공개

[ ] 세금 정보
    - Tax Interview 완료
    - Payment Info 확인
    - Revenue Share: 70% (개발자) / 30% (Steam)

[ ] 법적 검토
    - Steam Distribution Agreement 재확인
    - 저작권 침해 없는지 확인
    - 라이선스 준수
```

---

#### **출시 프로세스**

```
Step 1: 출시 검토 요청

1. Steamworks → App Admin → "Release"

2. "Mark as Ready for Review" 클릭

3. Steam 검토 (1-5일 소요):
   - Store Page 검토
   - 빌드 검토
   - 콘텐츠 정책 확인

Step 2: 승인 후 출시일 설정

1. 승인 이메일 수신

2. App Admin → "Release" → "Set Release Date"

3. 출시 옵션:
   - Release Now (즉시)
   - Release on [날짜] (예약)

4. "Confirm Release" 클릭

Step 3: 출시!

- Store Page 공개
- 플레이어 구매 가능
- Community Hub 활성화
- 축하합니다! 🎉
```

---

### 26.7 출시 후 관리

```
[첫 주]
- 리뷰 모니터링 (매일)
- 버그 리포트 빠르게 대응
- 커뮤니티 게시판 답변
- 긴급 패치 준비

[첫 달]
- 판매 데이터 분석
- 플레이어 피드백 수집
- 업데이트 로드맵 공유
- 할인 이벤트 계획

[장기]
- 정기 업데이트 (월 1회)
- DLC 개발 고려
- 플레이어 커뮤니티 육성
- 이벤트/콘테스트 개최
```

---

**다음 챕터**: Chapter 27에서는 itch.io 배포 방법을 학습합니다!

---

## Chapter 27: itch.io 배포 가이드

**목표**: itch.io에 게임을 무료로 출시하거나 판매합니다.

**예상 시간**: 1-2시간

**비용**: 무료 (수익의 10% 수수료, 선택적으로 0%로 낮출 수 있음)

**장점**:
- 빠른 출시 (심사 없음)
- 인디 개발자 친화적
- 유연한 가격 정책 (Pay What You Want)
- Steam보다 낮은 진입장벽

---

### 27.1 itch.io 계정 생성 및 프로젝트 설정

#### **Step 1: itch.io 계정 생성**

```
1. https://itch.io 접속

2. "Register" 클릭

3. 계정 정보 입력:
   - Username: yourstudio (URL에 사용됨)
   - Email: your.email@example.com
   - Password: ********

4. 이메일 인증

5. 프로필 설정 (선택사항):
   - Profile Picture (200x200 px)
   - Bio (짧은 소개)
   - Links (웹사이트, Twitter 등)
```

---

#### **Step 2: 새 프로젝트 생성**

```
1. 대시보드: https://itch.io/dashboard

2. "Create new project" 버튼 클릭

3. 기본 정보 입력:

   [Title]
   강호무적 (Murim Deckbuilder)

   [Project URL]
   https://yourstudio.itch.io/murim-deckbuilder
   - 한 번 설정하면 변경 어려움
   - 영문 소문자, 하이픈(-) 사용 권장

   [Classification]
   - Games ✅ (게임)

   [Kind of project]
   - Downloadable ✅ (다운로드 가능)
   - HTML (웹 게임, 해당 없음)
```

---

### 27.2 게임 정보 작성

#### **기본 정보 (Basic info)**

```
[Short description] (짧은 설명, 최대 140자)

A deckbuilding roguelike set in a martial arts world.
무협 세계의 덱빌딩 로그라이크 카드 게임.

---

[Metadata]

Genre:
✅ Card Game
✅ Roguelike
✅ Strategy

Tags (최대 10개):
- deckbuilding
- roguelike
- card-game
- singleplayer
- procedural-generation
- turn-based
- martial-arts
- difficult
- pixel-art (아트 스타일에 맞게)

Release status:
- Released ✅ (정식 출시)
- In development (개발 중)
- Prototype (프로토타입)

---

[Pricing]

No payments (무료):
- Free ✅

Paid:
- $14.99 USD

Pay what you want (원하는 가격):
- Minimum: $0 (무료)
- Suggested: $9.99
- 플레이어가 원하는 금액 지불 가능

---

[Embed options]

- Viewport dimensions: 1920 x 1080
- Fullscreen button: ✅ (다운로드 게임은 해당 없음)
```

---

### 27.3 빌드 업로드

#### **방법 1: 웹 인터페이스로 업로드 (간단)**

**Step 1: 빌드 압축**

```
1. 빌드 폴더로 이동:
   D:\Builds\Murim-Deckbuilder\

2. 모든 파일 선택 (강호무적.exe, Data 폴더 등)

3. 우클릭 → "Send to" → "Compressed (zipped) folder"

4. 파일명: murim-deckbuilder-v0.2.0-windows.zip

5. 파일 크기 확인:
   - itch.io 업로드 제한: 1 GB (웹 인터페이스)
   - 4 GB (Butler CLI 사용 시)
```

---

**Step 2: 웹에서 업로드**

```
1. itch.io 프로젝트 페이지 → "Edit game"

2. "Upload files" 섹션으로 스크롤

3. "Upload files" 버튼 클릭

4. murim-deckbuilder-v0.2.0-windows.zip 선택

5. 업로드 대기 (크기에 따라 1-10분)

6. 업로드 완료 후 설정:

   Display name: Windows (64-bit)

   ✅ This file will be played in the browser
   ❌ (다운로드 게임이므로 체크 해제)

   Kind of project:
   - Executable ✅ Windows

   Architecture:
   - 64-bit ✅

7. "Save" 클릭
```

---

#### **방법 2: Butler CLI로 업로드 (고급, 권장)**

**Butler 장점:**
- 빠른 업로드 (차분 업로드)
- 대용량 파일 지원 (4GB+)
- 자동화 가능
- 버전 관리 자동

**Step 1: Butler 설치**

```
[Windows]

1. https://itch.io/docs/butler/installing.html

2. 다운로드:
   - butler-windows-amd64.zip

3. 압축 해제:
   - C:\Tools\butler\

4. 환경 변수 설정:
   - 시스템 속성 → 환경 변수
   - Path에 추가: C:\Tools\butler\

5. 확인:
   cmd → butler -V
   출력: butler version 15.x.x
```

---

**Step 2: Butler 로그인**

```
1. cmd 열기

2. 로그인:
   butler login

3. 브라우저가 열림 → "Allow" 클릭

4. 인증 완료:
   "Credentials saved to C:\Users\...\butler_creds"
```

---

**Step 3: Butler로 업로드**

```
1. 빌드 폴더로 이동:
   cd D:\Builds\Murim-Deckbuilder\

2. 업로드 명령:
   butler push . yourstudio/murim-deckbuilder:windows-64bit --userversion 0.2.0

   설명:
   - . = 현재 폴더의 모든 파일
   - yourstudio/murim-deckbuilder = 프로젝트 URL
   - :windows-64bit = 채널 이름 (플랫폼 식별)
   - --userversion 0.2.0 = 버전 표시

3. 업로드 진행:
   [업로드 중...]
   ∙ Pushing 500 MB (128 files)
   ∙ Progress: 65% [=========>    ]

   완료!
   ✓ Build uploaded successfully

4. 업로드 시간:
   - 첫 업로드: 5-20분 (전체 파일)
   - 이후 업로드: 1-5분 (변경된 파일만)
```

---

**Step 4: 채널 설정 (웹에서)**

```
1. itch.io → Edit game → Upload files

2. Butler로 업로드한 채널이 표시됨:
   - windows-64bit (v0.2.0)

3. 설정:
   Display name: Windows (64-bit)
   Architecture: 64-bit ✅

4. "Save"
```

---

### 27.4 페이지 디자인

#### **커버 이미지 (Cover Image)**

```
[권장 크기]
- 최소: 630 x 500 px
- 권장: 1260 x 1000 px (Retina)
- 비율: 5:4

[디자인 팁]
- 게임 로고 크게
- 핵심 비주얼 (캐릭터, 카드)
- 짧은 태그라인
  예: "Master the Martial Arts. Build Your Deck. Conquer the Murim."

[예시 레이아웃]

┌─────────────────────────┐
│                         │
│      [게임 로고]        │
│   강호무적              │
│   Murim Deckbuilder    │
│                         │
│   [캐릭터 + 카드]      │
│                         │
│   Master Martial Arts  │
│   Build Your Deck      │
│                         │
└─────────────────────────┘
```

---

#### **스크린샷 (Screenshots)**

```
[최소 3장, 권장 5-10장]

크기: 1920 x 1080 px (16:9)
포맷: PNG, JPG

순서:
1. 전투 화면 (가장 중요!)
2. 카드 플레이 클로즈업
3. 맵 화면
4. 상점 화면
5. 유물 선택
6. 보스 전투
7. 카드 업그레이드
8. 다양한 카드 조합
9. 승리 화면
10. 메인 메뉴

팁:
- UI를 명확하게 보여주기
- 게임의 다양성 강조
- 첫 3장이 가장 중요 (썸네일로 표시)
```

---

#### **GIF / Video**

```
[Animated GIF]

크기: 640 x 360 px (작게)
길이: 3-10초
용량: < 5 MB

내용:
- 카드 플레이 애니메이션
- 공격 → 데미지 → 적 제거
- 짧고 임팩트 있게

제작 도구:
- ScreenToGif (무료)
- LICEcap (무료)
- Photoshop (유료)

---

[YouTube Video]

길이: 30초 - 2분

1. 유튜브에 업로드
2. itch.io → Edit game → "Add video"
3. YouTube URL 입력

예: https://www.youtube.com/watch?v=xxxxx
```

---

#### **상세 설명 (Description)**

itch.io는 Markdown 지원!

```markdown
# 강호무적 (Murim Deckbuilder)

무협 세계의 덱빌딩 로그라이크 카드 게임입니다.

## ✨ 핵심 특징

### 🎴 전략적 덱 구성
- **70장 이상의 카드**: 5가지 무술 계열
- **이중 자원 시스템**: 내공 vs 무기술
- **시너지 콤보**: 강력한 조합 발견

### ⚔️ 치열한 전투
- **다양한 적**: 독특한 AI 패턴
- **15종의 보스**: 도전적인 전투
- **실시간 전략**: 타이밍이 승부를 결정

### 🗺️ 절차적 생성
- **매번 다른 맵**: 무한한 재도전
- **선택의 연속**: 위험 vs 보상
- **여러 경로**: 나만의 루트 선택

### 💎 강력한 유물
- **50개 이상의 유물**: 게임 체인저
- **유물 시너지**: 궁극의 빌드 완성

## 🎮 게임 모드

- **표준 런**: 5개 지역 정복
- **데일리 챌린지**: 리더보드 경쟁
- **엔드리스 모드**: 얼마나 버틸 수 있나요?

## 🥋 무술 계열

1. **내공 (Inner Power)**: 기 조작과 방어
2. **검술 (Sword Arts)**: 정밀한 타격
3. **권법 (Fist Arts)**: 순수한 파워
4. **신법 (Movement Arts)**: 회피와 반격
5. **사술 (Forbidden Arts)**: 하이 리스크, 하이 리턴

---

## 💻 시스템 요구사항

**최소 사양:**
- OS: Windows 10 (64-bit)
- Processor: Intel Core i3 / AMD Ryzen 3
- Memory: 4 GB RAM
- Graphics: GTX 660 / Radeon HD 7850
- Storage: 500 MB

**권장 사양:**
- OS: Windows 10/11 (64-bit)
- Processor: Intel Core i5 / AMD Ryzen 5
- Memory: 8 GB RAM
- Graphics: GTX 1060 / Radeon RX 580

---

## 🎯 로드맵

**v0.2.0 (현재)**: 프로토타입
- ✅ 핵심 전투 시스템
- ✅ 20장의 카드
- ✅ 3종의 적

**v0.5.0 (3개월)**: 알파
- 70장의 카드
- 맵 시스템
- 유물 시스템

**v1.0.0 (9개월)**: 정식 출시
- 5개 지역
- 15종 보스
- 메타 진행

---

## 📧 문의 및 피드백

- Email: your.email@example.com
- Discord: https://discord.gg/yourserver
- Twitter: @yourstudio

**여러분의 피드백을 기다립니다!**

---

## 🙏 크레딧

- 개발: YourStudio
- 폰트: 나눔스퀘어 (네이버)
- 음악: [출처]

---

**강호의 패자가 되세요!** 🗡️💪
```

---

### 27.5 가격 및 접근성 설정

#### **가격 옵션 비교**

```
[Option 1: Free (무료)]

장점:
✅ 많은 다운로드
✅ 빠른 커뮤니티 형성
✅ 피드백 수집 쉬움

단점:
❌ 수익 없음
❌ 개발 지속성 어려움

권장: 프로토타입, 데모

---

[Option 2: Pay What You Want]

설정:
- Minimum price: $0 (무료)
- Suggested price: $9.99

장점:
✅ 무료로도 플레이 가능
✅ 일부 플레이어가 자발적 후원
✅ 유연성

단점:
❌ 예상 수익 불확실

권장: 인디 게임, 실험적 가격

---

[Option 3: Fixed Price (고정 가격)]

설정:
- Price: $14.99

장점:
✅ 안정적 수익
✅ 프로페셔널한 인상

단점:
❌ 다운로드 수 감소

권장: 완성도 높은 게임
```

---

#### **Community & Access 설정**

```
[Visibility]

- Public ✅ (누구나 볼 수 있음)
- Unlisted (링크가 있는 사람만)
- Restricted (특정 사용자만)

[Community]

- Enable comments: ✅
- Enable ratings: ✅
- Enable devlog: ✅ (개발 일지)

[Age rating]

- Everyone
- Teen
- Mature ✅ (폭력 표현 있는 경우)

[Accessibility]

- Color blind friendly: ✅ (해당되면)
- Subtitles: ✅ (음성이 있으면)
- Configurable difficulty: ✅
```

---

### 27.6 출시 및 홍보

#### **출시 프로세스**

```
1. 모든 정보 입력 완료 확인

2. "Save & view page" 클릭

3. 페이지 미리보기:
   - 모든 이미지 로드 확인
   - 설명 포맷 확인
   - 다운로드 버튼 작동 확인

4. "Edit game" → "Release status"

5. "Published" 선택

6. "Save" 클릭

7. 출시 완료! 🎉

즉시 접근 가능:
https://yourstudio.itch.io/murim-deckbuilder
```

---

#### **itch.io 홍보 전략**

```
[Launch Announcement]

1. Devlog 작성:
   - "Murim Deckbuilder is OUT NOW!"
   - 게임 소개, 스크린샷, 트레일러
   - 개발 이야기

2. itch.io 커뮤니티 게시:
   - Release Announcements 게시판
   - Relevant tags (#deckbuilder, #roguelike)

---

[External Promotion]

1. Reddit:
   - r/IndieGaming
   - r/roguelikes
   - r/gamedev (개발 이야기)

2. Twitter:
   - #indiegame #deckbuilder #roguelike
   - GIF 또는 짧은 영상 첨부

3. Discord:
   - Indie game servers
   - Roguelike/Deckbuilder communities

4. Game Jolt, IndieDB 등 크로스 포스팅

---

[Update Strategy]

정기 업데이트 (2주마다):
1. Devlog 작성
   - "v0.2.1 Update: New Cards!"
   - 변경 사항 상세히 설명

2. Butler로 새 빌드 업로드

3. 플레이어에게 알림 발송

4. 커뮤니티 피드백 수집
```

---

### 27.7 분석 및 수익 관리

#### **Analytics (분석)**

```
1. itch.io Dashboard → "Analytics"

2. 주요 지표:

   [Views]
   - 페이지 조회수
   - 유입 경로 (Reddit, Twitter 등)

   [Downloads]
   - 총 다운로드 수
   - 일별 다운로드

   [Revenue]
   - 총 수익
   - 평균 지불 금액 (PWYW)

   [Demographics]
   - 국가별 분포
   - 브라우저, OS

3. 개선:
   - 높은 조회, 낮은 다운로드 → 커버 이미지 개선
   - 특정 국가에서 인기 → 해당 언어 추가
```

---

#### **Payout (수익 정산)**

```
[itch.io 수수료]

기본: 10% (권장)
최소: 0% (가능, 개발자에게 100%)

설정:
1. Dashboard → "Settings" → "Seller settings"
2. "Open Revenue Sharing" 슬라이더 조정
3. 0% - 10% 사이 선택

권장: 5-10% (itch.io 지원)

---

[Payout Methods]

1. PayPal ✅ (추천)
   - 최소 지급: $5
   - 수수료: 약 5%
   - 빠른 처리 (1-3일)

2. Payoneer
   - 국제 송금
   - 수수료: 2-3%

3. 은행 계좌 직접
   - 국제 송금 수수료 높음

설정:
Dashboard → "Settings" → "Payout"

---

[Tax Information]

한국 거주자:
- W-8BEN 양식 작성 (미국 세금 면제)
- 한국 국세청에 해외 소득 신고 필요
- 전문가 상담 권장
```

---

### 27.8 체크리스트

**출시 전:**
- [ ] itch.io 계정 생성
- [ ] 프로젝트 생성 및 URL 설정
- [ ] 커버 이미지 (1260x1000 px)
- [ ] 스크린샷 최소 5장
- [ ] GIF 또는 트레일러
- [ ] 상세 설명 작성 (Markdown)
- [ ] 빌드 업로드 (ZIP 또는 Butler)
- [ ] 가격 설정
- [ ] Tags 설정 (최소 5개)

**출시:**
- [ ] Published 상태로 변경
- [ ] 페이지 링크 확인
- [ ] 다운로드 테스트

**출시 후:**
- [ ] Devlog 작성
- [ ] Reddit/Twitter 홍보
- [ ] 피드백 모니터링
- [ ] Analytics 확인
- [ ] 정기 업데이트 (2주)

**수익 관리:**
- [ ] Payout 방식 설정 (PayPal)
- [ ] W-8BEN 양식 제출 (세금)
- [ ] Revenue Sharing 설정 (5-10%)

---

**축하합니다!** 🎉

**PART 9 (빌드 및 배포) 완료!**

지금까지 학습한 내용:
- Chapter 25: Unity 빌드 설정 및 최적화
- Chapter 26: Steam 배포 (상세 가이드)
- Chapter 27: itch.io 배포 (빠른 출시)

이제 게임을 전 세계 플레이어에게 배포할 준비가 되었습니다!

---

**다음 파트**: PART 10에서는 주니어 개발자를 위한 학습 로드맵과 문제 해결 가이드를 학습합니다!

---

# PART 10: 지속적 개발 및 성장

## Chapter 28: 학습 로드맵

> **목표**: 주니어 개발자가 체계적으로 Unity 게임 개발을 학습하고, 강호무적 프로젝트를 완성하며, 게임 개발자로 성장하는 로드맵을 제공합니다.

이 챕터에서는 0부터 시작하는 개발자부터 첫 프로젝트를 완성하기까지의 **12개월 학습 로드맵**을 제공합니다.

---

### 28.1 주니어 개발자 로드맵

#### **전체 개요 (12개월)**

```
Month 1-2:  기초 학습 (Unity + C# + Git)
Month 3-4:  Phase 1 개발 (프로토타입)
Month 5-7:  Phase 2 개발 (수직 슬라이스)
Month 8-10: Phase 3-4 개발 (콘텐츠 확장)
Month 11:   폴리싱 및 테스트
Month 12:   배포 및 마케팅
```

---

#### **Month 1: Unity 기초**

**Week 1: 설치 및 환경 설정**

```
Day 1-2: Unity 설치
- Unity Hub 다운로드
- Unity 2022.3 LTS 설치
- Visual Studio 2022 설치 (Unity 연동)
- 첫 프로젝트 생성: "HelloUnity"

Day 3-4: Unity 인터페이스 익히기
- Scene 뷰, Game 뷰, Inspector
- Hierarchy, Project 창
- 기본 GameObject 생성 (Cube, Sphere)
- Transform (Position, Rotation, Scale)

Day 5-7: 첫 스크립트 작성
- C# 스크립트 생성
- MonoBehaviour 기초
- Start(), Update() 함수
- Debug.Log() 사용법
```

**실습 프로젝트:**

```csharp
// PlayerMovement.cs - 간단한 이동 스크립트
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        transform.Translate(movement);

        Debug.Log($"Position: {transform.position}");
    }
}
```

**Week 2: Unity 핵심 개념**

```
Day 8-9: Prefab 시스템
- Prefab 생성 및 사용
- Prefab Variant
- Prefab Override

Day 10-11: 물리 시스템
- Rigidbody, Collider
- OnCollisionEnter, OnTriggerEnter
- Physics Material

Day 12-14: UI 기초
- Canvas, RectTransform
- Button, Text, Image
- Event System
```

**실습 프로젝트: "Cube Shooter"**

```csharp
using UnityEngine;

public class SimpleShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * 20f;

        Destroy(bullet, 3f); // 3초 후 삭제
    }
}
```

**Week 3: C# 기초 복습**

```
Day 15-16: 변수와 자료형
- int, float, string, bool
- List, Array, Dictionary
- public vs private

Day 17-18: 제어문
- if-else, switch
- for, while, foreach
- break, continue

Day 19-21: 함수와 클래스
- 메서드 정의 및 호출
- 접근 제한자
- 생성자
- static, const
```

**실습 예제:**

```csharp
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<string> items = new List<string>();
    private int maxCapacity = 10;

    public bool AddItem(string itemName)
    {
        if (items.Count >= maxCapacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        items.Add(itemName);
        Debug.Log($"Added {itemName}. Total: {items.Count}");
        return true;
    }

    public void RemoveItem(string itemName)
    {
        if (items.Remove(itemName))
        {
            Debug.Log($"Removed {itemName}");
        }
        else
        {
            Debug.Log($"{itemName} not found");
        }
    }

    public void ShowInventory()
    {
        Debug.Log("=== Inventory ===");
        foreach (var item in items)
        {
            Debug.Log($"- {item}");
        }
    }
}
```

**Week 4: Git 기초**

```
Day 22-23: Git 설치 및 설정
- Git 다운로드 및 설치
- GitHub 계정 생성
- git config 설정 (name, email)
- .gitignore 이해

Day 24-25: Git 기본 명령어
- git init, clone
- git add, commit
- git push, pull
- git status, log

Day 26-28: Unity + Git
- Unity .gitignore 템플릿
- LFS (Large File Storage) 설정
- 첫 커밋 및 푸시
- GitHub Desktop 사용법
```

**실습:**

```bash
# Git 초기 설정
git config --global user.name "Your Name"
git config --global user.email "your@email.com"

# 새 프로젝트 초기화
cd /path/to/UnityProject
git init
git add .
git commit -m "Initial commit: Unity project setup"

# GitHub 원격 저장소 연결
git remote add origin https://github.com/yourusername/murim-deckbuilder.git
git push -u origin main
```

**Month 1 체크리스트:**

```
✅ Unity 설치 및 인터페이스 숙지
✅ GameObject, Component, Prefab 이해
✅ C# 기본 문법 (변수, 제어문, 함수)
✅ Git 기본 명령어 (add, commit, push)
✅ 간단한 프로젝트 3개 완성
   - HelloUnity (기본 이동)
   - CubeShooter (발사체)
   - SimpleInventory (인벤토리)
```

---

#### **Month 2: Unity 중급 + 디자인 패턴**

**Week 5-6: ScriptableObject & 데이터 관리**

```csharp
// CardData.cs - ScriptableObject 예제
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int damage;
    public int cost;
    [TextArea] public string description;
    public Sprite artwork;
}
```

**실습: "카드 데이터 시스템"**

```csharp
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public List<CardData> allCards;

    void Start()
    {
        // Resources 폴더에서 모든 카드 로드
        allCards = new List<CardData>(Resources.LoadAll<CardData>("Cards"));

        Debug.Log($"Loaded {allCards.Count} cards");

        foreach (var card in allCards)
        {
            Debug.Log($"{card.cardName}: {card.damage} damage, {card.cost} cost");
        }
    }

    public CardData GetCard(string cardName)
    {
        return allCards.Find(c => c.cardName == cardName);
    }
}
```

**Week 7: 디자인 패턴 (Singleton, State)**

```csharp
// GameManager.cs - Singleton 패턴
using UnityEngine;

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

    public int gold = 100;
    public int health = 50;

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Gold: {gold}");
    }
}
```

```csharp
// State 패턴 예제
public enum GameState { MainMenu, Combat, Map, Shop }

public class GameStateManager : MonoBehaviour
{
    private GameState currentState;

    public void ChangeState(GameState newState)
    {
        // 이전 상태 종료
        OnStateExit(currentState);

        // 새 상태로 전환
        currentState = newState;

        // 새 상태 시작
        OnStateEnter(newState);
    }

    void OnStateEnter(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                Debug.Log("Entering Main Menu");
                break;
            case GameState.Combat:
                Debug.Log("Starting Combat");
                break;
        }
    }

    void OnStateExit(GameState state)
    {
        Debug.Log($"Exiting {state}");
    }
}
```

**Week 8: 이벤트 시스템**

```csharp
// 이벤트 기반 프로그래밍
using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    private int currentHealth;
    public int maxHealth = 100;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);

        OnHealthChanged?.Invoke(currentHealth);
    }
}

// HealthUI.cs - 이벤트 구독
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public HealthSystem healthSystem;
    public Text healthText;

    void Start()
    {
        // 이벤트 구독
        healthSystem.OnHealthChanged += UpdateHealthUI;
        healthSystem.OnDeath += ShowDeathScreen;
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        healthSystem.OnHealthChanged -= UpdateHealthUI;
        healthSystem.OnDeath -= ShowDeathScreen;
    }

    void UpdateHealthUI(int newHealth)
    {
        healthText.text = $"HP: {newHealth}";
    }

    void ShowDeathScreen()
    {
        Debug.Log("You died!");
    }
}
```

**Month 2 체크리스트:**

```
✅ ScriptableObject 사용법
✅ Singleton 패턴 구현
✅ State 패턴 구현
✅ 이벤트 시스템 (Action, UnityEvent)
✅ Resources.Load 사용법
✅ 실습 프로젝트: "미니 RPG" 완성
   - 캐릭터 이동
   - 적과 전투
   - 체력 시스템
   - 간단한 인벤토리
```

---

#### **Month 3-4: Phase 1 개발 (프로토타입)**

**목표: `tasks-murim-deckbuilder-prototype-KR.md` 작업 0.0 ~ 9.0 완료**

**Month 3 계획:**

```
Week 9: 작업 0.0-1.0 (프로젝트 설정, Unity 구조)
- 브랜치 생성 (feature/phase1-prototype)
- Unity 프로젝트 생성
- 폴더 구조 설정
- Manager 스크립트 작성

Week 10: 작업 2.0-3.0 (데이터 관리, 전투 로직)
- JSON 데이터 구조 설계
- CardData, EnemyData ScriptableObject
- CombatManager 구현
- 턴제 전투 로직

Week 11: 작업 4.0 (카드 시스템)
- Card 클래스 설계
- DeckManager 구현
- 카드 드로우/버리기
- 손패 관리

Week 12: 작업 5.0 (적 AI)
- Enemy 클래스
- Intent 시스템
- 기본 AI 패턴 3종
```

**Month 4 계획:**

```
Week 13: 작업 6.0 (전투 UI)
- Canvas 설정
- 카드 UI Prefab
- 손패 레이아웃
- 체력/에너지 표시

Week 14: 작업 7.0 (초기 카드 20장)
- 카드 디자인 (공격 10장, 방어 7장, 보조 3장)
- 카드 밸런싱 (DPE/BPE)
- ScriptableObject 생성

Week 15: 작업 8.0 (적 타입 3종)
- 잡몹, 엘리트, 보스 디자인
- 체력/피해 밸런싱
- 행동 패턴 구현

Week 16: 작업 9.0 (통합 테스트)
- 전투 시뮬레이션
- 버그 수정
- 플레이테스트
- Phase 1 완료!
```

**Phase 1 완료 기준:**

```
✅ 전투 시스템 작동 (턴제)
✅ 카드 20장 플레이 가능
✅ 적 3종 등장
✅ 승리/패배 조건 작동
✅ 기본 UI 표시
✅ 버그 없이 1회 전투 완료 가능
```

---

#### **Month 5-7: Phase 2 개발 (수직 슬라이스)**

**목표: 1개 지역 완전 구현 (맵 → 전투 → 보상 → 상점 → 보스)**

**Month 5:**

```
Week 17-18: 맵 시스템
- 노드 기반 맵 생성
- 노드 타입 (전투, 상점, 휴식, 이벤트, 보스)
- 맵 UI

Week 19-20: 유물 시스템
- 유물 20개 디자인
- 유물 효과 구현
- 유물 UI
```

**Month 6:**

```
Week 21-22: 상점 & 휴식
- 상점 UI
- 카드 구매/제거
- 휴식 (체력 회복 vs 카드 업그레이드)

Week 23-24: 이벤트 시스템
- 이벤트 10개 작성
- 선택지 구현
- 보상 시스템
```

**Month 7:**

```
Week 25-26: 보스 전투
- 보스 디자인
- 2페이즈 메커니즘
- 보스 전용 패턴

Week 27-28: 메타 진행 & 세이브/로드
- 무공 정수 시스템
- 영구 업그레이드
- PlayerPrefs 세이브
- Phase 2 완료!
```

---

#### **Month 8-10: Phase 3-4 (콘텐츠 확장)**

**목표: 5개 지역, 카드 100장, 유물 50개**

```
Month 8: 지역 2-3 추가, 카드 30장 추가
Month 9: 지역 4-5 추가, 유물 30개 추가
Month 10: 이벤트 30개, 밸런싱
```

---

#### **Month 11: 폴리싱**

```
Week 41-42: 사운드 & 음악
- 배경음악 3곡
- 효과음 20개
- Audio Mixer

Week 43-44: 아트 & 애니메이션
- 카드 아트 (AI 생성 or 위탁)
- UI 폴리싱
- 카드 애니메이션 (DOTween)
```

---

#### **Month 12: 배포**

```
Week 45-46: 빌드 & 최적화
- Windows/Mac 빌드
- 빌드 크기 최적화
- 성능 테스트

Week 47: itch.io 배포
- 스토어 페이지 작성
- 스크린샷/GIF
- 첫 배포!

Week 48: 마케팅
- Reddit r/IndieGaming 포스팅
- Twitter 공유
- Discord 커뮤니티
- 피드백 수집
```

**12개월 완료 기준:**

```
✅ 플레이 가능한 덱빌더 로그라이크 완성
✅ itch.io 배포 완료
✅ 5-10명 플레이테스터 피드백 수집
✅ GitHub 레포지토리 공개 (포트폴리오)
✅ 개발 과정 블로그 작성
```

---

### 28.2 추천 학습 자료

#### **Unity 공식 리소스**

**1. Unity Learn (무료)**

[https://learn.unity.com](https://learn.unity.com)

추천 코스:
- **"Essentials Pathway"** (초급, 10시간)
  - Unity 에디터 기초
  - 스크립팅 입문
  - 물리 및 UI

- **"Junior Programmer"** (중급, 30시간)
  - C# 프로그래밍
  - GameObject 프로그래밍
  - 데이터 관리

- **"Create with Code"** (실전, 40시간)
  - 5개 미니 게임 제작
  - 프로토타입 제작법

**2. Unity Manual**

[https://docs.unity3d.com/Manual/index.html](https://docs.unity3d.com/Manual/index.html)

필독 섹션:
- Scripting
- UI
- Physics
- Animation

**3. Unity Scripting API**

[https://docs.unity3d.com/ScriptReference/](https://docs.unity3d.com/ScriptReference/)

자주 찾게 될 클래스:
- `MonoBehaviour`
- `GameObject`
- `Transform`
- `Rigidbody`
- `Canvas`, `RectTransform`

---

#### **YouTube 채널**

**1. Brackeys (영어, 초급~중급)**

[https://www.youtube.com/@Brackeys](https://www.youtube.com/@Brackeys)

추천 재생목록:
- "How to make a Video Game in Unity" (시리즈)
- "Unity Basics" (재생목록)
- "Inventory System" (카드 시스템과 유사)

**2. Jason Weimann (영어, 중급~고급)**

[https://www.youtube.com/@Unity3dCollege](https://www.youtube.com/@Unity3dCollege)

추천 영상:
- "Design Patterns in Unity"
- "Unity Performance Optimization"
- "Clean Code in Unity"

**3. Blackthornprod (영어, 게임 디자인)**

[https://www.youtube.com/@Blackthornprod](https://www.youtube.com/@Blackthornprod)

게임 디자인 및 아트 팁

**4. 케이시 (한국어, 초급)**

Unity 한국어 튜토리얼 검색 시 추천

---

#### **C# 학습 자료**

**1. Microsoft C# 공식 문서**

[https://docs.microsoft.com/ko-kr/dotnet/csharp/](https://docs.microsoft.com/ko-kr/dotnet/csharp/)

필독:
- C# 프로그래밍 가이드
- 언어 참조
- LINQ

**2. 책: "Head First C#" (4th Edition)**

- 초보자 친화적
- 그림과 예제 풍부
- Unity 예제 포함

**3. 책: "C# in Depth" (Jon Skeet)**

- 중급~고급
- C# 언어 깊이 이해

**4. 온라인 코스: "C# Fundamentals" (Pluralsight)**

무료 체험 가능

---

#### **게임 디자인**

**1. 책: "The Art of Game Design" (Jesse Schell)**

게임 디자인 바이블

핵심 개념:
- 렌즈 이론 (100가지 렌즈)
- 플레이어 경험 설계
- 밸런싱

**2. 책: "Game Programming Patterns" (Robert Nystrom)**

[https://gameprogrammingpatterns.com](https://gameprogrammingpatterns.com) (무료 온라인)

Unity에 유용한 패턴:
- Singleton
- State
- Observer
- Command
- Object Pool

**3. GDC Talks (YouTube)**

추천 영상:
- **"Slay the Spire: Metrics Driven Design"** (덱빌더 필수 시청!)
- "Designing Game Narrative"
- "The 4 Layers of Difficulty"

**4. 웹사이트: "Gamasutra / Game Developer"**

[https://www.gamedeveloper.com](https://www.gamedeveloper.com)

게임 개발 기사, 포스트모템

---

#### **덱빌딩 로그라이크 특화**

**1. Slay the Spire 개발자 블로그**

[https://www.reddit.com/r/slaythespire](https://www.reddit.com/r/slaythespire)

개발자가 직접 밸런싱 설명

**2. "Balancing Turn Based RPGs" (YouTube 시리즈)**

**3. "Roguelike Celebration" (컨퍼런스)**

[https://roguelike.club](https://roguelike.club)

---

#### **무료 에셋 & 도구**

**1. 아트 리소스**

- **Kenney.nl**: 2D/3D 에셋 (CC0 라이선스)
- **OpenGameArt.org**: 커뮤니티 에셋
- **itch.io Free Game Assets**: 무료 에셋 컬렉션

**2. 폰트**

- 나눔고딕, 나눔스퀘어 (무료)
- Google Fonts: Noto Sans KR
- 또박또박체 (무협 느낌)

**3. 사운드**

- **freesound.org**: 효과음
- **incompetech.com**: 배경음악 (Kevin MacLeod)
- **purple-planet.com**: 무료 음악

**4. Unity 에셋 (무료)**

- **DOTween**: 애니메이션
- **TextMeshPro**: 텍스트 렌더링 (내장)
- **Cinemachine**: 카메라 시스템 (내장)
- **Post-Processing**: 후처리 효과

---

### 28.3 학습 방법론

#### **효과적인 학습 전략**

**1. "Learn by Doing" 접근법**

```
❌ 잘못된 방법:
1. 튜토리얼 100개 시청
2. 책 3권 완독
3. 그 다음 프로젝트 시작

✅ 올바른 방법:
1. 기초 튜토리얼 1개 완료 (2-3시간)
2. 즉시 작은 프로젝트 시작
3. 막히는 부분만 검색/학습
4. 반복
```

**2. "30분 규칙"**

```
문제가 생겼을 때:

1. 스스로 해결 시도 (30분)
   - 오류 메시지 읽기
   - Debug.Log 추가
   - 코드 리뷰

2. 검색 (30분)
   - Google: "Unity [오류 메시지]"
   - Stack Overflow
   - Unity Forum

3. 질문하기
   - Discord 커뮤니티
   - Reddit r/Unity3D
   - 구체적인 질문 작성
```

**3. "Pomodoro Technique"**

```
25분 집중 → 5분 휴식

하루 일정 예시:
09:00-09:25  코딩 (전투 시스템)
09:25-09:30  휴식
09:30-09:55  코딩 (계속)
09:55-10:00  휴식
10:00-10:25  코딩
10:25-10:30  휴식
10:30-10:55  코딩
10:55-11:15  긴 휴식 (20분)

하루 4-6 Pomodoro면 충분 (2-3시간)
```

**4. "Feynman Technique" (개념 이해)**

```
1. 개념 선택 (예: "Singleton 패턴")
2. 누군가에게 설명하듯 작성
3. 막히는 부분 찾기
4. 그 부분 다시 학습
5. 단순화하기
```

예시:

```
Singleton이란?

"게임에서 딱 하나만 있어야 하는 매니저를 만드는 방법입니다.
예를 들어 GameManager는 게임 전체에서 하나만 있어야 하죠.
static 변수를 사용해서 인스턴스를 저장하고,
Awake()에서 이미 다른 인스턴스가 있으면 자기 자신을 삭제합니다."

→ 이렇게 설명할 수 있으면 이해한 것!
```

---

#### **코드 학습 팁**

**1. 타이핑으로 배우기 (복붙 금지)**

```
❌ 나쁜 습관:
- 튜토리얼 코드 복사/붙여넣기
- "일단 돌아가네" 하고 넘어가기

✅ 좋은 습관:
- 코드 한 줄 한 줄 직접 타이핑
- 각 줄이 무엇을 하는지 주석 작성
- 변수명 바꿔보기
- 숫자 바꿔서 실험
```

예시:

```csharp
// 튜토리얼 코드
void Update() {
    transform.Translate(Vector3.forward * 5 * Time.deltaTime);
}

// 내가 실험한 코드
void Update() {
    // 앞으로 이동 (5 → 10으로 바꾸면 더 빠름)
    float speed = 10f;

    // Time.deltaTime을 곱하면 프레임 독립적 (빼면 프레임에 따라 속도 변함)
    Vector3 movement = Vector3.forward * speed * Time.deltaTime;

    transform.Translate(movement);

    // 현재 위치 출력
    Debug.Log($"Position: {transform.position}");
}
```

**2. "작은 변화" 실험**

```
코드를 이해하려면:

1. 원본 코드 실행
2. 한 가지만 바꾸기
3. 결과 관찰
4. 이해한 내용 주석 작성
```

예시:

```csharp
// 실험 1: speed를 5에서 10으로 변경
// 결과: 2배 빠르게 이동함

// 실험 2: Vector3.forward를 Vector3.up으로 변경
// 결과: 위로 이동함

// 실험 3: Time.deltaTime 제거
// 결과: 프레임에 따라 속도가 달라짐 (60fps에서 매우 빠름)
```

---

#### **프로젝트 학습 팁**

**1. "버전 관리" 습관**

```
Git 커밋을 자주!

나쁜 습관:
- 2주 작업 후 한 번 커밋

좋은 습관:
- 기능 하나 완성 → 즉시 커밋

예시:
git commit -m "feat: Add card draw system"
git commit -m "fix: Fix null reference in DeckManager"
git commit -m "refactor: Clean up CombatManager"
```

**2. "주간 리뷰"**

```
매주 일요일 저녁:

1. 이번 주 한 일 정리
2. 배운 내용 3가지 작성
3. 다음 주 목표 설정

예시:
=== Week 10 Review ===

완료한 작업:
✅ CombatManager 기본 로직 구현
✅ 카드 드로우 시스템
✅ 에너지 시스템

배운 내용:
1. Queue<T>를 사용하면 덱 관리가 쉬움
2. enum으로 상태 관리하면 코드가 깔끔함
3. Debug.Log는 진짜 많이 써야 함

다음 주 목표:
- 카드 플레이 애니메이션
- 손패 UI 레이아웃
```

---

### 28.4 실전 연습 프로젝트

학습 과정에서 작은 프로젝트를 완성하면 자신감과 포트폴리오를 동시에 얻을 수 있습니다.

#### **프로젝트 1: "Pong Clone" (1-2일)**

**목표**: Unity 기초 + 물리 시스템

```
기능:
- 패들 이동 (키보드 입력)
- 공 물리 (Rigidbody2D)
- 점수 시스템
- UI (Text)
```

**배울 내용**:
- Input.GetAxis()
- Rigidbody2D
- OnCollision2D
- UI Canvas

---

#### **프로젝트 2: "Inventory System" (3-5일)**

**목표**: 데이터 관리 + UI

```
기능:
- 아이템 ScriptableObject
- Grid 기반 인벤토리
- Drag & Drop
- 아이템 정보 표시
```

**배울 내용**:
- ScriptableObject
- List<T>
- IPointerClickHandler
- Prefab Instantiate

---

#### **프로젝트 3: "Turn-Based Combat" (1주)**

**목표**: 턴제 시스템 (강호무적 프로토타입)

```
기능:
- 플레이어 vs 적 턴제 전투
- 행동 선택 (공격, 방어, 스킬)
- HP/MP 시스템
- 승리/패배 조건
```

**배울 내용**:
- State Machine
- enum
- Coroutine
- UI Button Event

---

#### **프로젝트 4: "Card Game Prototype" (2주)**

**목표**: 카드 시스템 (강호무적 Phase 1)

```
기능:
- 카드 10장 (공격 5, 방어 5)
- 덱 / 손패 / 버리기 pile
- 카드 드로우 애니메이션
- 에너지 시스템
```

**배울 내용**:
- Queue<T> (덱 관리)
- DOTween (애니메이션)
- Object Pooling
- 카드 밸런싱

---

### 28.5 커뮤니티 참여

#### **온라인 커뮤니티**

**1. Discord**

- **Unity 한국 커뮤니티**
- **게임 개발자 모임**
- **인디 게임 개발자 코리아**

장점:
- 실시간 질문/답변
- 코드 리뷰
- 네트워킹

**2. Reddit**

- r/Unity3D (영어)
- r/gamedev (영어)
- r/IndieDev (영어)

장점:
- 글로벌 개발자
- 포트폴리오 피드백
- 게임 쇼케이스

**3. Unity Forum (공식)**

[https://forum.unity.com](https://forum.unity.com)

장점:
- Unity 직원 답변
- 깊이 있는 기술 토론

---

#### **오프라인 활동**

**1. 게임 잼 참가**

- **Global Game Jam** (매년 1월, 48시간)
- **Ludum Dare** (온라인, 48-72시간)
- **게임메이커즈** (한국)

장점:
- 짧은 시간 내 게임 완성 경험
- 팀워크
- 포트폴리오

**2. 밋업 참석**

- Unity Korea 밋업
- 게임 개발자 모임 (지역별)

장점:
- 네트워킹
- 취업 정보
- 멘토 찾기

---

#### **학습 체크리스트 (월별)**

**Month 1-2 (기초):**
```
✅ Unity 인터페이스 숙지
✅ C# 기본 문법 (변수, 제어문, 클래스)
✅ Git 기본 명령어
✅ 프로젝트 3개 완성
✅ Unity Learn "Essentials" 완료
```

**Month 3-4 (Phase 1):**
```
✅ tasks-murim-deckbuilder-prototype-KR.md 0.0-9.0 완료
✅ 전투 시스템 작동
✅ 카드 20장 구현
✅ 첫 플레이테스트 완료
```

**Month 5-7 (Phase 2):**
```
✅ 맵 시스템 구현
✅ 유물 20개 구현
✅ 1개 지역 완성 (시작 → 보스)
✅ 세이브/로드 구현
```

**Month 8-10 (Phase 3-4):**
```
✅ 5개 지역 완성
✅ 카드 100장
✅ 유물 50개
✅ 밸런싱 완료
```

**Month 11-12 (폴리싱 & 배포):**
```
✅ 사운드/음악 추가
✅ 아트 폴리싱
✅ 빌드 최적화
✅ itch.io 배포
✅ Reddit 포스팅
```

---

**다음 챕터 예고:**

이제 학습 로드맵을 따라가다 보면 반드시 **막히는 순간**이 옵니다. Chapter 29에서는 주니어 개발자가 자주 겪는 **일반적인 문제와 해결 방법**을 다룹니다!

---

## Chapter 29: 일반적인 문제 해결

> **목표**: 주니어 개발자가 Unity 개발 중 반드시 겪게 되는 **일반적인 오류와 문제**를 빠르게 식별하고 해결하는 방법을 익힙니다.

이 챕터는 **실전 문제 해결 가이드**입니다. 오류 메시지, 증상, 해결 방법을 함께 제공합니다.

---

### 29.1 자주 발생하는 오류 TOP 10

#### **1. NullReferenceException**

**오류 메시지:**
```
NullReferenceException: Object reference not set to an instance of an object
```

**원인:**
- 참조가 설정되지 않은 변수에 접근
- Inspector에서 드래그하지 않음
- GetComponent가 실패함

**해결 방법:**

```csharp
// ❌ 문제 코드
public class CombatManager : MonoBehaviour
{
    public Player player; // Inspector에서 할당하지 않음

    void Start()
    {
        player.TakeDamage(10); // NullReferenceException 발생!
    }
}

// ✅ 해결 1: Null 체크
void Start()
{
    if (player != null)
    {
        player.TakeDamage(10);
    }
    else
    {
        Debug.LogError("Player is not assigned!");
    }
}

// ✅ 해결 2: GetComponent로 자동 찾기
void Awake()
{
    player = FindObjectOfType<Player>();
    if (player == null)
    {
        Debug.LogError("Player not found in scene!");
    }
}

// ✅ 해결 3: RequireComponent로 강제
[RequireComponent(typeof(Rigidbody))]
public class MyScript : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // 항상 존재 보장
    }
}
```

**디버깅 팁:**

```csharp
void Start()
{
    Debug.Log($"player is null? {player == null}");
    Debug.Log($"player object: {player}");
}
```

---

#### **2. Missing Reference Exception**

**오류 메시지:**
```
The object of type 'GameObject' has been destroyed but you are still trying to access it.
MissingReferenceException: The object of type 'CardUI' has been destroyed
```

**원인:**
- GameObject가 Destroy()되었는데 참조를 계속 사용

**해결 방법:**

```csharp
// ❌ 문제 코드
public class Hand : MonoBehaviour
{
    private List<CardUI> cardsInHand = new List<CardUI>();

    public void DiscardCard(CardUI card)
    {
        Destroy(card.gameObject);
        // cardsInHand에는 여전히 참조가 남아있음!
    }

    public void ShowAllCards()
    {
        foreach (var card in cardsInHand)
        {
            card.Show(); // MissingReferenceException!
        }
    }
}

// ✅ 해결: 리스트에서도 제거
public void DiscardCard(CardUI card)
{
    cardsInHand.Remove(card); // 먼저 리스트에서 제거
    Destroy(card.gameObject); // 그 다음 삭제
}

// ✅ 해결 2: Null 체크
public void ShowAllCards()
{
    foreach (var card in cardsInHand)
    {
        if (card != null) // 파괴되었는지 확인
        {
            card.Show();
        }
    }
}
```

---

#### **3. IndexOutOfRangeException**

**오류 메시지:**
```
IndexOutOfRangeException: Index was outside the bounds of the array.
```

**원인:**
- List/Array의 범위를 벗어난 인덱스 접근

**해결 방법:**

```csharp
// ❌ 문제 코드
List<Card> deck = new List<Card>(); // 빈 리스트

void DrawCard()
{
    Card drawn = deck[0]; // IndexOutOfRangeException!
}

// ✅ 해결: 크기 확인
void DrawCard()
{
    if (deck.Count > 0)
    {
        Card drawn = deck[0];
        deck.RemoveAt(0);
    }
    else
    {
        Debug.LogWarning("Deck is empty!");
    }
}

// ✅ 해결 2: 안전한 접근
Card GetCardAt(int index)
{
    if (index >= 0 && index < deck.Count)
    {
        return deck[index];
    }
    else
    {
        Debug.LogError($"Invalid index: {index}, deck size: {deck.Count}");
        return null;
    }
}
```

---

#### **4. ArgumentException: The thing you want to Instantiate is null**

**오류 메시지:**
```
ArgumentException: The thing you want to instantiate is null.
```

**원인:**
- Prefab 참조가 설정되지 않음

**해결 방법:**

```csharp
// ❌ 문제 코드
public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab; // Inspector에 할당 안 함

    void Start()
    {
        Instantiate(cardPrefab); // ArgumentException!
    }
}

// ✅ 해결: Null 체크
void Start()
{
    if (cardPrefab != null)
    {
        Instantiate(cardPrefab);
    }
    else
    {
        Debug.LogError("cardPrefab is not assigned in Inspector!");
    }
}

// ✅ 해결 2: Resources.Load
void Start()
{
    if (cardPrefab == null)
    {
        cardPrefab = Resources.Load<GameObject>("Prefabs/CardUI");
    }

    if (cardPrefab != null)
    {
        Instantiate(cardPrefab);
    }
}
```

---

#### **5. DivideByZeroException**

**오류 메시지:**
```
DivideByZeroException: Attempted to divide by zero.
```

**원인:**
- 0으로 나눔 (정수 나눗셈)

**해결 방법:**

```csharp
// ❌ 문제 코드
int CalculateDPE(int damage, int cost)
{
    return damage / cost; // cost가 0이면 오류!
}

// ✅ 해결
int CalculateDPE(int damage, int cost)
{
    if (cost == 0)
    {
        Debug.LogWarning("Cost is 0, cannot calculate DPE");
        return 0;
    }
    return damage / cost;
}

// ✅ 해결 2: float 사용
float CalculateDPE(int damage, int cost)
{
    if (cost == 0)
        return Mathf.Infinity; // 무한대 반환

    return (float)damage / cost;
}
```

---

#### **6. UI가 보이지 않음**

**증상:**
- UI를 만들었는데 게임 화면에 안 보임
- Inspector에서는 보이는데 실행하면 안 보임

**원인 및 해결:**

**원인 1: Canvas 설정 문제**

```
해결:
1. Canvas → Render Mode 확인
   - Screen Space - Overlay (가장 간단)
   - Screen Space - Camera (메인 카메라 할당 필요)

2. Canvas Scaler 확인
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920x1080
```

**원인 2: Sorting Order / Layer**

```csharp
// Canvas의 Sorting Order 확인
Canvas canvas = GetComponent<Canvas>();
canvas.sortingOrder = 10; // 다른 UI보다 위에 표시
```

**원인 3: RectTransform 위치**

```
Inspector → RectTransform:
- Anchors가 화면 밖에 있음
- Position이 (0, 0, 0)이 아님

해결:
1. RectTransform 우클릭 → Reset
2. Anchors Presets에서 중앙 선택
```

**원인 4: Alpha가 0**

```csharp
// Image나 Text의 Alpha 확인
Image image = GetComponent<Image>();
Color color = image.color;
color.a = 1f; // Alpha를 1로 설정
image.color = color;
```

---

#### **7. 카드가 클릭되지 않음**

**증상:**
- Button이나 CardUI를 클릭해도 반응 없음

**원인 및 해결:**

**원인 1: EventSystem 없음**

```
해결:
1. Hierarchy → 우클릭 → UI → Event System
2. Scene에 EventSystem이 하나만 있어야 함
```

**원인 2: Graphic Raycaster 없음**

```
Canvas에 Graphic Raycaster 컴포넌트 확인:
- 없으면 Add Component → Graphic Raycaster
```

**원인 3: Raycast Target 꺼짐**

```
Image/Text 컴포넌트:
- ✅ Raycast Target 체크 확인
```

**원인 4: 다른 UI가 가림**

```csharp
// 디버깅: 어떤 UI가 클릭되는지 확인
using UnityEngine.EventSystems;

public class DebugRaycast : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log($"Clicked {results.Count} UI elements:");
            foreach (var result in results)
            {
                Debug.Log($"- {result.gameObject.name}");
            }
        }
    }
}
```

---

#### **8. 프레임이 너무 느림 (Low FPS)**

**증상:**
- 게임이 뚝뚝 끊김
- FPS가 30 이하

**원인 및 해결:**

**원인 1: Update()에서 무거운 작업**

```csharp
// ❌ 나쁜 코드
void Update()
{
    // 매 프레임 Find! (매우 느림)
    GameObject enemy = GameObject.Find("Enemy");

    // 매 프레임 GetComponent! (느림)
    HealthSystem health = GetComponent<HealthSystem>();
}

// ✅ 좋은 코드
private GameObject enemy;
private HealthSystem health;

void Start()
{
    enemy = GameObject.Find("Enemy"); // 한 번만
    health = GetComponent<HealthSystem>(); // 한 번만
}

void Update()
{
    // 캐싱된 변수 사용
}
```

**원인 2: Instantiate가 너무 많음**

```csharp
// ❌ 나쁜 코드: 매 프레임 생성/삭제
void Update()
{
    if (needBullet)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        Destroy(bullet, 1f);
    }
}

// ✅ 좋은 코드: Object Pooling
private Queue<GameObject> bulletPool = new Queue<GameObject>();

void Start()
{
    // 미리 생성
    for (int i = 0; i < 20; i++)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}

GameObject GetBullet()
{
    if (bulletPool.Count > 0)
    {
        GameObject bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        return bullet;
    }
    else
    {
        return Instantiate(bulletPrefab);
    }
}

void ReturnBullet(GameObject bullet)
{
    bullet.SetActive(false);
    bulletPool.Enqueue(bullet);
}
```

**원인 3: 너무 많은 Draw Call**

```
해결:
1. Window → Analysis → Frame Debugger
2. Draw Calls 수 확인 (500 이하 권장)
3. Sprite Atlas 사용 (UI 이미지 묶기)
4. Static Batching (배경 오브젝트)
```

---

#### **9. 빌드 후 실행이 안 됨**

**증상:**
- Unity 에디터에서는 잘 되는데 빌드 후 실행하면 오류

**원인 및 해결:**

**원인 1: Scene이 Build Settings에 없음**

```
해결:
1. File → Build Settings
2. Scenes In Build에 모든 Scene 추가
3. 순서 확인 (0번이 시작 Scene)
```

**원인 2: Resources 폴더 경로 오류**

```csharp
// ❌ 잘못된 경로
var card = Resources.Load<CardData>("Assets/Resources/Cards/card_001");

// ✅ 올바른 경로
var card = Resources.Load<CardData>("Cards/card_001");
// Resources 폴더 이후 경로만 작성
```

**원인 3: DLL 누락 (Plugins)**

```
해결:
1. Plugins 폴더 확인
2. Build Settings → Player Settings → Other Settings
3. Api Compatibility Level: .NET Framework (or .NET Standard 2.1)
```

---

#### **10. Git Merge Conflict (씬 파일)**

**오류 메시지:**
```
CONFLICT (content): Merge conflict in Assets/Scenes/Combat.unity
```

**원인:**
- 여러 명이 같은 Scene 파일 수정

**해결 방법:**

**해결 1: Scene을 나누기 (사전 예방)**

```
작업 분리:
- CombatScene.unity (전투만)
- UIScene.unity (UI만)
- DataScene.unity (Manager들만)

→ Additive Scene으로 로드
```

**해결 2: Prefab 사용**

```
Scene에 직접 작업하지 않고:
1. Prefab으로 만들기
2. Prefab 수정
3. Scene에는 Prefab Instance만
```

**해결 3: Conflict 발생 시**

```bash
# 방법 1: 내 버전 사용
git checkout --ours Assets/Scenes/Combat.unity
git add Assets/Scenes/Combat.unity

# 방법 2: 상대 버전 사용
git checkout --theirs Assets/Scenes/Combat.unity
git add Assets/Scenes/Combat.unity

# 방법 3: Scene 새로 만들기 (최후의 수단)
1. Scene 백업
2. 빈 Scene 생성
3. 필요한 GameObject 다시 추가
```

---

### 29.2 체계적인 디버깅 전략

개발 중 버그가 생겼을 때 **체계적으로 접근**하면 해결 시간을 크게 줄일 수 있습니다.

#### **5단계 디버깅 프로세스**

**Step 1: 문제 재현 (Reproduce)**

```
1. 정확히 언제 발생하는가?
   - "카드를 플레이할 때마다"
   - "3번째 턴에서만"
   - "가끔 랜덤하게"

2. 재현 가능한가?
   - Yes → 디버깅 가능
   - No → 로그를 더 많이 남겨야 함

3. 재현 단계 기록
   - 1. 게임 시작
   - 2. 카드 3장 드로우
   - 3. 첫 번째 카드 플레이
   - 4. 오류 발생
```

**Step 2: 문제 범위 좁히기 (Isolate)**

```
Binary Search 방식:

1. 코드를 반으로 나눔
2. 앞부분 주석 처리
3. 문제가 사라지면 → 앞부분에 버그
4. 문제가 계속되면 → 뒷부분에 버그
5. 반복
```

예시:

```csharp
public void PlayCard(Card card)
{
    // 1단계: 에너지 체크
    if (energy < card.cost)
    {
        Debug.Log("Not enough energy");
        return;
    }
    Debug.Log("1. Energy check passed"); // ✅ 여기까지 OK

    // 2단계: 에너지 소모
    energy -= card.cost;
    Debug.Log("2. Energy deducted"); // ✅ 여기까지 OK

    // 3단계: 카드 효과 실행
    card.Execute(target);
    Debug.Log("3. Card executed"); // ❌ 여기서 오류 발생!

    // 4단계: 카드 버리기
    MoveToDiscard(card);
    Debug.Log("4. Card discarded");
}

// 결론: card.Execute(target)에 버그가 있음!
```

**Step 3: 로그 추가 (Log)**

```csharp
public void DealDamage(int damage, Enemy target)
{
    Debug.Log($"=== DealDamage Start ===");
    Debug.Log($"Damage: {damage}");
    Debug.Log($"Target: {target?.name ?? "null"}");
    Debug.Log($"Target Health: {target?.currentHealth ?? -1}");

    if (target == null)
    {
        Debug.LogError("Target is null!");
        return;
    }

    target.TakeDamage(damage);

    Debug.Log($"Target Health After: {target.currentHealth}");
    Debug.Log($"=== DealDamage End ===");
}
```

**Step 4: Breakpoint 사용 (Visual Studio)**

```
1. Visual Studio에서 코드 줄 번호 클릭 → 빨간 점
2. Unity에서 Play
3. Unity 상단 메뉴 → Attach to Unity
4. 코드 실행 시 해당 줄에서 멈춤
5. 변수 값 확인 (마우스 올리기 or Watch)

유용한 단축키:
- F5: Continue
- F10: Step Over (다음 줄)
- F11: Step Into (함수 안으로)
- Shift+F11: Step Out (함수 밖으로)
```

**Step 5: 구글 검색 / 커뮤니티 질문**

**효과적인 검색:**

```
❌ 나쁜 검색:
"unity error"
"card doesn't work"

✅ 좋은 검색:
"Unity NullReferenceException Instantiate"
"Unity UI Button not clickable EventSystem"
"Unity List IndexOutOfRangeException remove"
```

**효과적인 질문:**

```
제목: [Unity] NullReferenceException when playing card

내용:

**환경:**
- Unity 2022.3.10f1
- C# / Windows 11

**문제:**
카드를 플레이할 때 NullReferenceException이 발생합니다.

**재현 방법:**
1. 게임 시작
2. 카드 드로우
3. 카드 클릭
4. 오류 발생

**오류 메시지:**
```
NullReferenceException: Object reference not set to an instance
at CardUI.OnPointerClick() (at Assets/Scripts/CardUI.cs:42)
```

**관련 코드:**
```csharp
public void OnPointerClick(PointerEventData eventData)
{
    CombatManager.Instance.PlayCard(cardData); // Line 42
}
```

**시도한 해결책:**
- Inspector에서 cardData 할당 확인
- CombatManager가 Scene에 있는지 확인

**질문:**
어떻게 해결할 수 있을까요?
```

---

#### **디버깅 도구 활용**

**1. Unity Console 필터링**

```
Console 창:
- Clear: 로그 전체 삭제
- Collapse: 같은 로그 묶기
- Error Pause: 오류 발생 시 일시정지

검색:
- "COMBAT" 입력 → COMBAT 로그만 표시
```

**2. Conditional Compilation**

```csharp
public class GameLogger
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(string message)
    {
        Debug.Log(message);
        // 빌드 시 자동으로 제거됨
    }
}

// 사용:
GameLogger.Log("This only prints in Editor");
```

**3. Assert 사용**

```csharp
using UnityEngine.Assertions;

public void PlayCard(Card card)
{
    Assert.IsNotNull(card, "Card is null!");
    Assert.IsTrue(energy >= card.cost, "Not enough energy!");

    // 실행 코드
}
```

---

### 29.3 성능 문제 해결

#### **성능 프로파일링 단계**

**Step 1: 증상 확인**

```
FPS 측정:

1. Window → Analysis → Profiler
2. CPU Usage 그래프 확인
3. 스파이크 (급증) 구간 찾기
```

**FPS 모니터 추가:**

```csharp
public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0f;
    private GUIStyle style;

    void Awake()
    {
        Application.targetFrameRate = 60; // 60fps 목표
        style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.green;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float ms = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = $"{fps:0.} FPS ({ms:0.0} ms)";

        // FPS에 따라 색상 변경
        if (fps >= 50)
            style.normal.textColor = Color.green;
        else if (fps >= 30)
            style.normal.textColor = Color.yellow;
        else
            style.normal.textColor = Color.red;

        GUI.Label(new Rect(10, 10, 200, 30), text, style);
    }
}
```

**Step 2: Profiler로 병목 찾기**

```
CPU Profiler:

1. Hierarchy 모드
2. Total 시간순 정렬
3. 10ms 이상 함수 찾기

주요 병목:
- GC.Alloc (가비지 생성)
- Camera.Render
- Canvas.SendWillRenderCanvases
- Update() 함수들
```

**Step 3: 최적화**

**최적화 1: GC Allocation 줄이기**

```csharp
// ❌ 나쁜 코드: 매 프레임 new
void Update()
{
    Vector3 pos = new Vector3(x, y, z); // GC Alloc!
    string text = "Score: " + score; // GC Alloc!
}

// ✅ 좋은 코드: 재사용
private Vector3 tempPos;
private System.Text.StringBuilder sb = new System.Text.StringBuilder();

void Update()
{
    tempPos.Set(x, y, z); // GC 없음
    transform.position = tempPos;

    sb.Clear();
    sb.Append("Score: ");
    sb.Append(score); // GC 없음
    scoreText.text = sb.ToString();
}
```

**최적화 2: LINQ 피하기**

```csharp
// ❌ 느림: LINQ (GC 발생)
var attackCards = allCards.Where(c => c.type == CardType.Attack).ToList();

// ✅ 빠름: foreach
List<Card> attackCards = new List<Card>();
foreach (var card in allCards)
{
    if (card.type == CardType.Attack)
    {
        attackCards.Add(card);
    }
}
```

**최적화 3: Update 최적화**

```csharp
// ❌ 나쁜 코드
void Update()
{
    // 매 프레임 실행 (60 FPS = 초당 60번)
    CheckWinCondition();
}

// ✅ 좋은 코드: 주기적으로만
private float checkInterval = 0.5f; // 0.5초마다
private float lastCheckTime;

void Update()
{
    if (Time.time - lastCheckTime >= checkInterval)
    {
        CheckWinCondition();
        lastCheckTime = Time.time;
    }
}

// ✅ 더 좋은 코드: 이벤트 기반
public event Action OnEnemyDeath;

void KillEnemy()
{
    OnEnemyDeath?.Invoke(); // 필요할 때만 체크
}
```

---

#### **메모리 문제 해결**

**증상: 메모리 사용량 계속 증가**

**원인 1: 메모리 누수**

```csharp
// ❌ 문제: 이벤트 구독 해제 안 함
public class HealthUI : MonoBehaviour
{
    void Start()
    {
        HealthSystem.OnHealthChanged += UpdateUI;
    }

    // OnDestroy()가 없음 → 메모리 누수!
}

// ✅ 해결
void OnDestroy()
{
    HealthSystem.OnHealthChanged -= UpdateUI;
}
```

**원인 2: 텍스처가 너무 큼**

```
해결:
1. Texture Import Settings
2. Max Size: 2048 → 1024 or 512
3. Compression: None → ASTC (모바일) or DXT (PC)
4. Mip Maps: ✅ Generate (3D만)
```

**원인 3: Audio Clip이 Decompress On Load**

```
Audio Import Settings:
- Load Type: Decompress On Load → Compressed In Memory
- Compression Format: PCM → Vorbis (배경음악) or ADPCM (효과음)
```

---

### 29.4 Unity 특유의 문제들

#### **1. Awake vs Start 순서 문제**

```csharp
// ScriptA.cs
void Awake()
{
    Debug.Log("A Awake");
}

void Start()
{
    Debug.Log("A Start");
}

// ScriptB.cs
void Awake()
{
    Debug.Log("B Awake");
}

void Start()
{
    Debug.Log("B Start");
}

// 실행 순서:
// A Awake
// B Awake  ← 모든 Awake 먼저
// A Start
// B Start  ← 그 다음 모든 Start
```

**해결: Script Execution Order**

```
Edit → Project Settings → Script Execution Order
- DataManager: -100 (가장 먼저)
- GameManager: -50
- (Default Time): 0
- UIManager: 50
- CombatManager: 100 (가장 나중)
```

---

#### **2. Time.deltaTime vs Time.unscaledDeltaTime**

```csharp
// Time.deltaTime: Time.timeScale 영향 받음
void Update()
{
    transform.position += Vector3.forward * speed * Time.deltaTime;
    // Time.timeScale = 0 (일시정지) → 멈춤
}

// Time.unscaledDeltaTime: 영향 안 받음
void Update()
{
    // UI 애니메이션 (일시정지 중에도 동작)
    rotateSpeed += Time.unscaledDeltaTime;
}
```

---

#### **3. Coroutine vs Invoke**

```csharp
// Invoke: 간단한 지연
Invoke("DealDamage", 1.0f); // 1초 후 DealDamage() 호출
InvokeRepeating("CheckHealth", 0, 0.5f); // 0.5초마다 반복

// Coroutine: 복잡한 시퀀스
StartCoroutine(CardPlaySequence());

IEnumerator CardPlaySequence()
{
    cardUI.MoveTo(targetPosition);
    yield return new WaitForSeconds(0.3f);

    target.TakeDamage(damage);
    yield return new WaitForSeconds(0.2f);

    cardUI.MoveToDiscard();
    yield return new WaitForSeconds(0.3f);

    EndTurn();
}
```

---

#### **4. Physics2D vs Physics (3D)**

```
Unity에는 2개의 물리 엔진:

**2D 게임:**
- Rigidbody2D
- Collider2D (BoxCollider2D, CircleCollider2D)
- OnCollisionEnter2D()
- OnTriggerEnter2D()

**3D 게임:**
- Rigidbody
- Collider (BoxCollider, SphereCollider)
- OnCollisionEnter()
- OnTriggerEnter()

❌ 섞어 쓰면 작동 안 함!
```

---

### 29.5 Git 문제 해결

#### **1. "I accidentally committed to main!"**

```bash
# main에 커밋해버렸을 때

# 방법 1: 마지막 커밋 취소 (커밋 전으로)
git reset --soft HEAD~1

# 방법 2: 커밋을 유지하되 브랜치 이동
git branch feature/my-work  # 새 브랜치 생성
git reset --hard HEAD~1     # main에서 커밋 제거
git checkout feature/my-work # 새 브랜치로 이동
```

---

#### **2. "I want to undo my last commit"**

```bash
# 커밋 취소하고 변경 사항은 유지
git reset --soft HEAD~1

# 커밋 취소하고 변경 사항도 삭제 (주의!)
git reset --hard HEAD~1

# 커밋 취소하고 작업 디렉토리는 유지
git reset --mixed HEAD~1
```

---

#### **3. ".gitignore가 작동 안 함"**

```bash
# 이미 추적 중인 파일은 .gitignore 무시됨

# 해결: Git 캐시에서 제거
git rm -r --cached .
git add .
git commit -m "fix: Apply .gitignore"
```

---

#### **4. "Merge conflict 해결 못하겠어!"**

```bash
# Merge 취소
git merge --abort

# 또는 Rebase 취소
git rebase --abort

# 처음부터 다시
git reset --hard origin/main
```

---

### 29.6 빌드 및 배포 문제

#### **1. "Build succeeded but game doesn't run"**

**체크리스트:**

```
✅ File → Build Settings → Scenes In Build에 모든 Scene 추가
✅ Player Settings → Company Name, Product Name 설정
✅ Player Settings → Icon 설정
✅ Resources 폴더 경로 확인
✅ PlayerPrefs 경로 (빌드 시 다름)
✅ DLL / Plugins 확인
```

---

#### **2. "Build size too large (500MB+)"**

**최적화:**

```
1. Audio:
   - Compression Format: Vorbis
   - Quality: 70%

2. Texture:
   - Max Size: 1024
   - Compression: ASTC (모바일), DXT (PC)

3. Code Stripping:
   - Player Settings → Managed Stripping Level: High

4. 불필요한 에셋 제거:
   - 사용 안 하는 폰트, 이미지 삭제
```

---

### 29.7 문제 해결 체크리스트

**버그 발생 시:**

```
□ Console 확인 (오류 메시지)
□ Debug.Log 추가
□ Null 체크
□ Inspector 참조 확인
□ Breakpoint 디버깅
□ Google 검색
□ Unity Forum 검색
□ 커뮤니티 질문
```

**성능 문제 시:**

```
□ Profiler 실행
□ FPS 측정
□ Update() 최적화
□ GC.Alloc 확인
□ Object Pooling
□ Sprite Atlas
□ Draw Call 확인
```

**빌드 문제 시:**

```
□ Scene In Build 확인
□ Resources 경로 확인
□ Platform 설정 확인
□ Player Settings 확인
□ Console 오류 확인
```

---

**다음 챕터 예고:**

문제를 해결하는 방법을 배웠다면, 이제 **커리어를 개발**할 차례입니다! Chapter 30에서는 포트폴리오 구축, 취업 준비, 그리고 게임 개발자로서의 지속적인 성장을 다룹니다!

---

## Chapter 30: 게임 개발 커리어

> **목표**: 강호무적 프로젝트를 완성한 주니어 개발자가 **포트폴리오를 구축**하고, **취업을 준비**하며, **게임 개발자로서 지속적으로 성장**하는 방법을 배웁니다.

이 챕터는 기술이 아니라 **커리어**에 대한 챕터입니다. 게임을 만드는 것을 넘어, 게임 개발자로 살아가는 방법을 다룹니다.

---

### 30.1 포트폴리오 구축

포트폴리오는 취업, 프리랜서, 인디 개발 모두에게 **가장 중요한 자산**입니다.

#### **포트폴리오 3대 원칙**

```
1. 완성도: 미완성 10개보다 완성 1개
2. 퀄리티: 양보다 질
3. 설명력: 코드보다 스토리
```

---

#### **1. 프로젝트 완성하기**

**강호무적 프로젝트 체크리스트:**

```
✅ Phase 1-2 완료 (최소)
✅ itch.io 배포 (플레이 가능)
✅ 버그 없이 1번 플레이스루 가능
✅ 스크린샷 5장 이상
✅ 게임플레이 영상 1분
✅ README.md 작성
```

**최소 완성 기준 (Phase 1-2):**

```
전투 시스템: ✅ 작동
카드 20-30장: ✅ 구현
맵 시스템: ✅ 구현
유물 시스템: ✅ 구현
1개 지역 완성: ✅ 시작 → 보스
세이브/로드: ✅ 작동

→ 이 정도면 포트폴리오로 충분!
```

---

#### **2. GitHub README.md 작성**

**강호무적 README.md 템플릿:**

````markdown
# 강호무적 - 무협 덱빌딩 로그라이크

> **장르**: 덱빌딩 로그라이크 카드 게임
> **개발 기간**: 2024.01 ~ 2024.06 (6개월)
> **개발 인원**: 1인 (개인 프로젝트)
> **엔진**: Unity 2022.3 LTS
> **언어**: C#

---

## 📖 프로젝트 소개

**강호무적**은 Slay the Spire에서 영감을 받은 무협 테마 덱빌딩 로그라이크 게임입니다.
플레이어는 낭인(떠돌이 무사)이 되어 5개 지역을 돌파하며 무공을 익히고, 강호의 패자가 되는 것이 목표입니다.

**핵심 특징:**
- 🎴 100종 카드 (무기술, 내공, 비급)
- 🗺️ 5개 지역 + 50개 노드
- 💎 50종 유물
- ⚔️ 30종 적 (잡몹, 엘리트, 보스)
- 🎯 메타 진행 (무공 정수)

---

## 🎮 플레이 방법

**itch.io에서 플레이:** [https://your-username.itch.io/murim-deckbuilder](https://your-username.itch.io/murim-deckbuilder)

**조작법:**
- 마우스: 카드 드래그 & 드롭
- 스페이스: 턴 종료
- ESC: 메뉴

---

## 🎥 게임플레이 영상

![gameplay](https://user-images.githubusercontent.com/your-gif.gif)

---

## 🛠️ 기술 스택

**엔진 & 언어:**
- Unity 2022.3 LTS
- C# (.NET Standard 2.1)

**아키텍처:**
- Singleton Manager 패턴
- State Machine (전투 상태)
- Observer 패턴 (이벤트 시스템)
- Command 패턴 (카드 효과)
- Object Pool (UI 최적화)

**데이터 관리:**
- ScriptableObject (카드, 유물, 적)
- JSON (세이브/로드)
- Google Sheets → JSON 파이프라인

**UI/UX:**
- TextMeshPro
- DOTween (애니메이션)
- Canvas Group (페이드)

---

## 📂 프로젝트 구조

```
Assets/
├── Scripts/
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── CombatManager.cs
│   │   ├── DataManager.cs
│   │   └── UIManager.cs
│   ├── Combat/
│   │   ├── Player.cs
│   │   ├── Enemy.cs
│   │   └── DeckManager.cs
│   ├── Cards/
│   │   ├── Card.cs
│   │   ├── CardData.cs (ScriptableObject)
│   │   └── CardEffect.cs
│   ├── UI/
│   │   ├── CardUI.cs
│   │   ├── HandManager.cs
│   │   └── CombatUI.cs
│   └── Data/
│       ├── SaveData.cs
│       └── PlayerData.cs
├── Resources/
│   ├── Data/
│   │   ├── cards.json
│   │   ├── enemies.json
│   │   └── relics.json
│   ├── Cards/ (ScriptableObject 에셋)
│   └── Prefabs/
├── Scenes/
│   ├── MainMenu.unity
│   ├── Combat.unity
│   └── Map.unity
└── Art/
    ├── Cards/
    ├── UI/
    └── VFX/
```

---

## 💡 핵심 구현 내용

### 1. 턴제 전투 시스템

```csharp
public enum CombatState { PlayerTurn, EnemyTurn, Victory, Defeat }

public class CombatManager : MonoBehaviour
{
    private CombatState currentState;

    public void StartPlayerTurn()
    {
        currentState = CombatState.PlayerTurn;
        player.energy = player.maxEnergy;
        deckManager.DrawCards(5);
    }

    public void EndPlayerTurn()
    {
        currentState = CombatState.EnemyTurn;
        deckManager.DiscardHand();
        ExecuteEnemyTurn();
    }
}
```

### 2. 카드 시스템 (Command 패턴)

```csharp
[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cost;
    public CardType type;
    public int damage;
    public int block;

    public void Execute(Enemy target)
    {
        switch (type)
        {
            case CardType.Attack:
                target.TakeDamage(damage);
                break;
            case CardType.Skill:
                CombatManager.Instance.player.GainBlock(block);
                break;
        }
    }
}
```

### 3. 적 AI (Intent 시스템)

```csharp
public class Enemy : MonoBehaviour
{
    public EnemyIntent currentIntent;

    void SelectIntent()
    {
        float roll = Random.value;
        if (roll < 0.5f)
            currentIntent = new AttackIntent(damage: 8);
        else
            currentIntent = new BlockIntent(block: 5);

        uiIntent.UpdateIntent(currentIntent);
    }

    public void ExecuteIntent()
    {
        currentIntent.Execute(this);
    }
}
```

---

## 📊 개발 성과

**개발 통계:**
- 개발 기간: 6개월 (파트타임)
- 총 커밋: 250+
- 코드 라인: 약 15,000 줄
- 플레이 시간: 30-60분 (1회)

**플레이테스트 결과:**
- 테스터: 10명
- 평균 평점: 4.2 / 5.0
- "카드 밸런스가 좋다" (60%)
- "더 많은 콘텐츠 원함" (80%)

**성능:**
- 타겟 FPS: 60
- 평균 FPS: 58-60
- 빌드 크기: 120 MB

---

## 🎯 기술적 도전 과제

### 1. 카드 밸런싱

**문제:**
초기 카드들이 너무 강력하거나 약해서 밸런스가 무너짐

**해결:**
DPE/BPE 지표 도입 및 스프레드시트 자동 계산

```
DPE (Damage Per Energy) = 피해 / 비용
목표: 일반 카드 5.0-6.0, 희귀 카드 7.0-8.0
```

### 2. UI 성능 최적화

**문제:**
카드 50장 생성 시 FPS 30 이하로 하락

**해결:**
Object Pooling 도입 → FPS 60 유지

**Before:**
```csharp
GameObject card = Instantiate(cardPrefab);
Destroy(card);
```

**After:**
```csharp
GameObject card = cardPool.Get();
cardPool.Return(card);
```

**결과:** GC Alloc 90% 감소

### 3. 세이브/로드 구조 설계

**문제:**
런마다 카드 덱, 유물, 체력 등 저장해야 할 데이터 증가

**해결:**
JSON Serialization + ScriptableObject 재연결

```csharp
[System.Serializable]
public class SaveData
{
    public List<string> cardIDs; // ScriptableObject ID만 저장
    public int currentHealth;
    public int gold;
    // ...

    public void Save()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("SaveData", json);
    }
}
```

---

## 📚 배운 점

**기술적 성장:**
- Unity 아키텍처 설계 (Singleton, State, Observer)
- ScriptableObject 활용한 데이터 주도 설계
- Object Pooling을 통한 성능 최적화
- JSON 기반 데이터 관리

**프로젝트 관리:**
- Git Feature Branch 전략
- 체계적인 커밋 메시지 (feat/fix/refactor)
- 주간 플레이테스트 및 피드백 반영

**게임 디자인:**
- 카드 게임 밸런싱 방법론 (DPE/BPE)
- 로그라이크 랜덤 생성 시스템
- 플레이어 피드백 기반 iteration

---

## 🚀 향후 계획

**v1.1 (1개월):**
- [ ] 카드 30장 추가
- [ ] 유물 20개 추가
- [ ] 버그 수정

**v2.0 (3개월):**
- [ ] 새로운 클래스 2종
- [ ] Steam 출시
- [ ] 멀티플랫폼 (Mac, Linux)

---

## 📞 연락처

**Email**: your.email@example.com
**Portfolio**: https://your-portfolio.com
**LinkedIn**: https://linkedin.com/in/yourprofile

---

## 📝 라이선스

이 프로젝트는 포트폴리오 목적으로 제작되었습니다.
코드는 MIT 라이선스, 아트 에셋은 별도 라이선스를 따릅니다.
````

---

#### **3. 포트폴리오 사이트 제작**

**옵션 1: GitHub Pages (무료, 간단)**

```bash
# 1. GitHub Pages 활성화
GitHub 레포지토리 → Settings → Pages
Source: Deploy from a branch → main → /docs

# 2. index.html 작성
docs/index.html:
<!DOCTYPE html>
<html>
<head>
    <title>강호무적 - 포트폴리오</title>
</head>
<body>
    <h1>강호무적 (Murim Deckbuilder)</h1>
    <iframe src="https://itch.io/embed/your-game" width="552" height="167"></iframe>
    <img src="screenshot1.png">
</body>
</html>
```

**옵션 2: Notion (무료, 빠름)**

```
1. Notion 페이지 생성
2. 템플릿:
   - 프로젝트 제목 + 커버 이미지
   - 간단한 설명
   - 게임플레이 GIF
   - 기술 스택
   - GitHub 링크
   - itch.io 링크

3. 공유 → 웹에 게시 → 검색 엔진 허용
4. 링크를 이력서에 추가
```

**옵션 3: 개인 웹사이트 (권장)**

```
도메인 + 호스팅:
- Netlify (무료)
- Vercel (무료)
- GitHub Pages (무료)

템플릿:
- HTML/CSS/JS 직접 작성
- Hugo (정적 사이트 생성기)
- Jekyll (GitHub Pages 기본)

구성:
- About Me
- Projects (강호무적, 기타 프로젝트)
- Skills
- Contact
```

---

#### **4. 영상 및 GIF 제작**

**게임플레이 영상 (1-2분):**

```
녹화:
- OBS Studio (무료)
- 해상도: 1920x1080
- FPS: 60
- 비트레이트: 6000 kbps

편집:
- DaVinci Resolve (무료)
- Shotcut (무료)

내용:
0:00-0:10  게임 제목 + 장르
0:10-0:30  전투 시스템 (카드 플레이)
0:30-0:50  맵 진행
0:50-1:10  유물 획득
1:10-1:30  보스 전투
1:30-1:40  승리 화면

자막:
- "Unity + C#로 개발"
- "6개월 개인 프로젝트"
- "100종 카드, 50종 유물"

음악:
- 무료 음악 (YouTube Audio Library)
- Kevin MacLeod (incompetech.com)
```

**GIF 제작 (5-10초):**

```
녹화:
- ScreenToGif (무료, Windows)
- LICEcap (무료, Mac)

최적화:
- 해상도: 800x600 (가벼움)
- FPS: 30 (부드러움)
- ezgif.com (압축)

내용:
- 카드 플레이 시퀀스
- 카드 드래그 & 드롭
- 적 공격 애니메이션
- 유물 획득 이펙트

용도:
- README.md
- itch.io 페이지
- Twitter/Reddit 홍보
```

---

### 30.2 취업 준비

#### **1. 주니어 Unity 개발자 역량 체크리스트**

**필수 역량:**

```
✅ Unity 기초
   - GameObject, Component, Prefab 이해
   - Scene, Inspector, Hierarchy 사용
   - 스크립팅 (C#)

✅ C# 프로그래밍
   - 변수, 제어문, 함수
   - 클래스, 상속, 인터페이스
   - List, Dictionary
   - LINQ 기초

✅ Git 버전 관리
   - add, commit, push, pull
   - branch, merge
   - .gitignore

✅ 포트폴리오
   - 완성된 프로젝트 1개 이상
   - GitHub 레포지토리
   - itch.io 배포

✅ 디자인 패턴 (최소 2-3개)
   - Singleton
   - State
   - Observer
```

**우대 역량:**

```
⭐ UI 시스템
   - Canvas, RectTransform
   - Layout Group
   - Event System

⭐ 애니메이션
   - Animator Controller
   - Animation Clip
   - DOTween

⭐ 성능 최적화
   - Profiler 사용
   - Object Pooling
   - Sprite Atlas

⭐ 테스트
   - Unity Test Framework
   - PlayMode/EditMode Test

⭐ 배포 경험
   - Build Settings
   - Steam/itch.io
   - Mobile (Android/iOS)
```

---

#### **2. 이력서 작성**

**이력서 구조:**

```
=== 이력서 (1-2페이지) ===

[사진] (선택)

홍길동
Unity 게임 개발자
Email: your.email@example.com
GitHub: github.com/yourname
Portfolio: yoursite.com

---

[간단한 소개] (2-3줄)
Unity와 C#을 활용한 게임 개발 경험이 있는 주니어 개발자입니다.
6개월 동안 덱빌딩 로그라이크 게임을 개인 제작하여 itch.io에 배포하였으며,
클린 코드와 디자인 패턴을 적용한 아키텍처 설계에 관심이 많습니다.

---

[기술 스택]

게임 엔진:
- Unity 2022.3 LTS (6개월)
- Unity Test Framework
- DOTween, TextMeshPro

프로그래밍:
- C# (.NET Standard 2.1)
- Git / GitHub
- Visual Studio 2022

디자인 패턴:
- Singleton, State, Observer, Command, Object Pool

---

[프로젝트]

1. 강호무적 - 무협 덱빌딩 로그라이크 (개인 프로젝트)
   기간: 2024.01 ~ 2024.06 (6개월)
   역할: 기획 / 개발 / 밸런싱
   기술: Unity, C#, ScriptableObject, JSON
   성과: itch.io 배포, 10명 플레이테스트 완료

   주요 구현:
   - 턴제 전투 시스템 (State Machine)
   - 카드 100종 (ScriptableObject + Command 패턴)
   - 적 AI (Intent 시스템)
   - 맵 생성 알고리즘 (Procedural Generation)
   - Object Pooling을 통한 UI 최적화 (FPS 30→60)

   GitHub: github.com/yourname/murim-deckbuilder
   플레이: yourname.itch.io/murim-deckbuilder

2. [추가 프로젝트 있으면 작성]

---

[학력]
○○대학교 컴퓨터공학과 졸업 (또는 재학)
2020.03 ~ 2024.02

[자격증] (있으면)
- 정보처리기사

---

[GitHub 기여]
- 총 커밋: 250+
- 연속 기여: 90일
- 주요 레포: murim-deckbuilder (⭐ 15)
```

**이력서 작성 팁:**

```
DO:
✅ 구체적인 숫자 (6개월, 100종 카드, FPS 30→60)
✅ 기술 스택 명확히 (Unity 2022.3, C# .NET Standard 2.1)
✅ GitHub 링크 (코드 확인 가능)
✅ 플레이 가능한 빌드 (itch.io)
✅ 1-2페이지 (간결함)

DON'T:
❌ 미완성 프로젝트 나열
❌ "기초 수준", "공부 중" (자신감 없어 보임)
❌ 너무 긴 설명 (채용 담당자는 30초만 봄)
❌ 오타, 맞춤법 실수
❌ 링크 안 열림
```

---

#### **3. 기술 면접 준비**

**Unity 예상 질문 TOP 20:**

**기초 (1-5):**

1. **GameObject와 Component의 관계를 설명하세요.**
   ```
   답변 예시:
   GameObject는 씬의 기본 단위이며, 그 자체로는 아무 기능이 없습니다.
   Component를 붙여서 기능을 추가하는 방식입니다.
   예를 들어 SpriteRenderer를 붙이면 이미지를 그리고,
   Rigidbody2D를 붙이면 물리 법칙을 따르게 됩니다.
   ```

2. **Awake()와 Start()의 차이는 무엇인가요?**
   ```
   답변 예시:
   Awake()는 스크립트가 초기화될 때 호출되며, 모든 Awake()가 먼저 실행됩니다.
   Start()는 첫 Update() 전에 호출되며, 모든 Awake() 이후에 실행됩니다.
   따라서 Awake()에서는 자기 자신의 초기화를,
   Start()에서는 다른 오브젝트를 참조하는 초기화를 합니다.
   ```

3. **Prefab이 무엇이고 왜 사용하나요?**
   ```
   답변 예시:
   Prefab은 재사용 가능한 GameObject 템플릿입니다.
   예를 들어 적 캐릭터 Prefab을 만들면,
   여러 씬에서 같은 적을 생성할 수 있고,
   Prefab을 수정하면 모든 인스턴스가 자동으로 업데이트됩니다.
   ```

4. **ScriptableObject는 언제 사용하나요?**
   ```
   답변 예시:
   데이터만 저장하는 에셋을 만들 때 사용합니다.
   예를 들어 카드 게임에서 카드마다 이름, 비용, 효과가 다르면,
   CardData ScriptableObject를 만들어서 관리합니다.
   MonoBehaviour와 달리 씬에 붙일 필요가 없고,
   여러 GameObject가 같은 데이터를 참조할 수 있습니다.
   ```

5. **Coroutine은 언제 사용하나요?**
   ```
   답변 예시:
   시간을 두고 실행되는 작업에 사용합니다.
   예를 들어 3초 후 폭탄 터지기, 0.5초마다 체력 회복 등입니다.
   yield return new WaitForSeconds(3f)를 사용하면
   다음 프레임이 아니라 3초 후에 재개됩니다.
   ```

**중급 (6-10):**

6. **Singleton 패턴을 설명하고 Unity에서 어떻게 구현하나요?**
   ```csharp
   답변 예시:
   "Singleton은 클래스의 인스턴스가 딱 하나만 존재하도록 하는 패턴입니다.
   Unity에서는 GameManager처럼 씬 전체에서 하나만 있어야 하는 매니저에 사용합니다."

   코드:
   public class GameManager : MonoBehaviour
   {
       private static GameManager _instance;
       public static GameManager Instance
       {
           get
           {
               if (_instance == null)
                   _instance = FindObjectOfType<GameManager>();
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

7. **Object Pooling이 무엇이고 왜 사용하나요?**
   ```
   답변 예시:
   "매번 Instantiate/Destroy하지 않고 미리 생성한 오브젝트를 재사용하는 기법입니다.
   GC Allocation을 줄여서 성능을 향상시킵니다.
   예를 들어 총알 50개를 미리 만들어두고 SetActive(true/false)로 켜고 끄는 방식입니다."
   ```

8. **Unity의 이벤트 시스템 (UnityEvent, C# event)을 설명하세요.**
   ```
   답변 예시:
   "이벤트는 발행-구독 패턴입니다.
   예를 들어 플레이어가 죽으면 OnPlayerDeath 이벤트를 발생시키고,
   UI, 사운드, GameManager 등이 구독해서 각자 처리합니다.
   C# event는 코드에서, UnityEvent는 Inspector에서 연결할 때 사용합니다."
   ```

9. **State Machine을 Unity에서 어떻게 구현하나요?**
   ```csharp
   답변 예시:
   "enum으로 상태를 정의하고 switch문으로 처리합니다."

   코드:
   public enum GameState { MainMenu, Combat, Map, Victory }

   private GameState currentState;

   public void ChangeState(GameState newState)
   {
       OnStateExit(currentState);
       currentState = newState;
       OnStateEnter(newState);
   }

   void OnStateEnter(GameState state)
   {
       switch (state)
       {
           case GameState.Combat:
               CombatManager.Instance.StartCombat();
               break;
       }
   }
   ```

10. **Unity Profiler를 사용해본 경험이 있나요?**
    ```
    답변 예시:
    "네, 강호무적 프로젝트에서 FPS가 30으로 떨어졌을 때 Profiler로 확인했습니다.
    카드 UI를 매 프레임 Instantiate하고 있어서 GC.Alloc이 발생했고,
    Object Pooling을 적용해서 FPS를 60으로 개선했습니다."
    ```

**고급 (11-15):**

11. **TextMeshPro와 일반 Text UI의 차이는 무엇인가요?**
12. **Unity의 렌더 파이프라인 (URP, HDRP)에 대해 아는 대로 설명하세요.**
13. **Addressable Assets 시스템이 무엇인가요?**
14. **Unity에서 멀티스레딩을 어떻게 구현하나요? (Job System, C# Thread)**
15. **Mobile 최적화 경험이 있나요?**

**프로젝트 기반 (16-20):**

16. **포트폴리오 프로젝트에서 가장 어려웠던 부분은 무엇인가요?**
17. **카드 밸런싱을 어떻게 했나요?**
18. **버그를 어떻게 디버깅했나요? (구체적인 예시)**
19. **코드 리뷰 경험이 있나요? / Git 사용 경험은?**
20. **다음에 만들고 싶은 게임은 무엇인가요?**

---

#### **4. 코딩 테스트 준비**

Unity 회사도 코딩 테스트를 보는 경우가 많습니다.

**알고리즘 문제 (C#):**

```
플랫폼:
- 백준 (BOJ)
- 프로그래머스
- LeetCode

난이도:
- Silver 3~1 (백준)
- Level 2 (프로그래머스)

주제:
- 배열, 리스트
- 정렬, 검색
- BFS, DFS (기초)
- 동적 계획법 (기초)
```

**Unity 실전 문제:**

```
예시 1: "간단한 카드 게임 만들기"
- 카드 Prefab 생성
- 덱에서 카드 5장 드로우
- 손패 UI 배치
- 카드 클릭 시 버리기
→ 2시간 내 구현

예시 2: "적 AI 구현"
- 플레이어와 적 1:1 전투
- 적은 70% 공격, 30% 방어
- 턴제로 구현
→ 1시간 내 구현

예시 3: "Object Pooling 구현"
- 총알 Prefab 제공
- 스페이스바 누르면 총알 발사
- Pooling으로 최적화
→ 30분 내 구현
```

---

### 30.3 지속적 성장

#### **1. 커리어 패스**

**주니어 → 시니어 (5-7년):**

```
주니어 개발자 (0-2년):
- 기능 구현 (주어진 태스크)
- 버그 수정
- 코드 리뷰 받기

중급 개발자 (2-5년):
- 시스템 설계 (전투, UI, 데이터)
- 기술 조사 및 선택
- 주니어 멘토링

시니어 개발자 (5년+):
- 아키텍처 설계
- 기술 리드
- 프로젝트 관리
```

**인디 개발자 vs 회사 취업:**

| | **인디 개발자** | **회사 (중소/대기업)** |
|---|---|---|
| **장점** | 자유로운 개발<br>100% 수익<br>나만의 게임 | 안정적인 급여<br>팀 협업 경험<br>대형 프로젝트 |
| **단점** | 수입 불안정<br>마케팅 필수<br>혼자 모든 일 | 야근 가능성<br>회사 방향 따라야 함<br>개인 프로젝트 제한 |
| **추천 대상** | 자유 중시<br>사업가 마인드<br>마케팅 가능 | 안정 중시<br>팀워크 선호<br>대형 게임 경험 |

**추천 전략:**

```
Option 1: 회사 먼저 → 인디 (안전)
- 2-3년 회사 경험
- 급여 저축
- 퇴사 후 인디 (1년 생활비 확보)

Option 2: 인디 먼저 → 회사 (도전)
- 6개월 인디 프로젝트
- 실패 시 회사 취업 (포트폴리오는 남음)
- 성공 시 인디 계속

Option 3: 병행 (현실적)
- 회사 다니며 사이드 프로젝트
- 주말/저녁 2-3시간
- 성공하면 전업 전환
```

---

#### **2. 학습 계획 (평생 학습)**

**매년 목표:**

```
Year 1-2 (주니어):
- Unity + C# 마스터
- 1개 프로젝트 완성
- 디자인 패턴 5종

Year 3-4 (중급):
- 전문 분야 선택 (UI, AI, 네트워크, 그래픽스)
- 2-3개 프로젝트 완성
- 오픈소스 기여

Year 5+ (시니어):
- 아키텍처 설계 능력
- 팀 리드 경험
- 컨퍼런스 발표
```

**월간 루틴:**

```
주중 (월-금):
- 회사 or 메인 프로젝트
- 저녁 1-2시간 학습 (YouTube, 문서)

주말 (토-일):
- 사이드 프로젝트 3-4시간
- 블로그 작성
- 게임 잼 참가 (월 1회)

월 1회:
- 기술 블로그 포스팅
- 커뮤니티 밋업 참석
- 새로운 기술 학습 (Shader, AI, Multiplayer)
```

---

#### **3. 커뮤니티 활동**

**오픈소스 기여:**

```
시작하기:
1. Unity 플러그인 레포지토리 찾기 (GitHub)
2. "good first issue" 라벨 검색
3. 작은 버그 수정부터 시작
4. Pull Request 제출

추천 프로젝트:
- DOTween (애니메이션)
- Odin Inspector
- Unity Test Tools
```

**게임 잼:**

```
추천 게임 잼:
- Global Game Jam (매년 1월, 48시간, 전 세계)
- Ludum Dare (연 3회, 48-72시간, 온라인)
- 게임메이커즈 (한국, 비정기)

장점:
- 48시간 안에 게임 완성 경험
- 빠른 프로토타이핑 능력 향상
- 팀워크 (디자이너, 아티스트 협업)
- 포트폴리오 (+3-5개 미니 게임)

팁:
- 작은 스코프 (큰 게임 욕심 금지)
- 팀 구성 (프로그래머 2, 아티스트 1, 디자이너 1)
- Git 사용 (협업 필수)
```

**블로그 운영:**

```
추천 플랫폼:
- Tistory (한국, SEO 좋음)
- Velog (개발자 친화적)
- Medium (영어, 글로벌)
- GitHub Pages (개발자용)

주제:
- 개발 과정 (강호무적 제작기)
- 문제 해결 (버그 해결 과정)
- 기술 정리 (Unity Profiler 사용법)
- 튜토리얼 (Deck Building 게임 만들기)

빈도:
- 월 1-2회 (부담 없이)
- 짧게 써도 OK (500-1000자)

효과:
- 취업 시 가산점
- 지식 정리
- 커뮤니티 기여
```

---

### 30.4 마무리 조언

#### **최종 체크리스트**

**개발자로서:**

```
✅ 완성한 프로젝트 1개 이상
✅ GitHub 활동 (커밋 90일 이상)
✅ itch.io 배포 경험
✅ Unity + C# 자신감
✅ 디자인 패턴 2종 이상 사용
✅ 포트폴리오 사이트
✅ 기술 블로그 (선택)
```

**취업 준비:**

```
✅ 이력서 작성
✅ 포트폴리오 정리
✅ GitHub README.md
✅ 기술 면접 준비 (예상 질문 20개)
✅ 코딩 테스트 준비 (알고리즘)
✅ LinkedIn 프로필
✅ 이메일 시그니처 (포트폴리오 링크)
```

**지속 성장:**

```
✅ 학습 루틴 (주 5-10시간)
✅ 커뮤니티 참여 (Discord, Reddit)
✅ 게임 잼 참가 (연 1-2회)
✅ 블로그 운영 (월 1-2회)
✅ 오픈소스 기여 (가능하면)
```

---

#### **주니어 개발자에게**

**DO:**

```
✅ 작은 것이라도 완성하기
   - 미완성 10개 < 완성 1개

✅ 매일 조금씩 코딩하기
   - 1시간도 좋음, 30분도 좋음

✅ 질문하기
   - 30분 고민 → 검색 → 질문

✅ 코드 리뷰 받기
   - 친구, 커뮤니티, 회사 동료

✅ 실패 받아들이기
   - 버그는 자연스러움
   - 첫 프로젝트는 항상 어려움

✅ 재미있게 하기
   - 번아웃 주의
   - 하루 1개 게임 플레이
```

**DON'T:**

```
❌ 완벽주의
   - 완벽한 코드는 없음
   - 일단 작동하게 만들기

❌ 튜토리얼 지옥
   - 튜토리얼 10개 < 프로젝트 1개

❌ 혼자 끙끙
   - 3일 막히면 질문하기

❌ 남과 비교
   - 시니어 개발자와 비교 금지
   - 1년 전 내 자신과 비교

❌ 번아웃
   - 주말에는 쉬기
   - 게임도 플레이하기
```

---

#### **마지막 응원 메시지**

```
강호무적 가이드를 여기까지 읽으셨다면,
당신은 이미 99%의 사람들보다 앞서 있습니다.

대부분의 사람들은 "게임 만들고 싶다"고만 말하고
실제로 만들지 않습니다.

하지만 당신은 다릅니다.

- Unity를 설치했고
- C#을 배웠고
- Git을 사용하고
- 카드 시스템을 구현하고
- 전투 시스템을 만들고
- itch.io에 배포했습니다.

그것만으로도 대단합니다.

---

처음 프로젝트는 항상 어렵습니다.
버그는 수없이 생기고,
밸런싱은 계속 무너지고,
FPS는 떨어지고,
Git Conflict는 발생합니다.

하지만 그 모든 과정이 당신을 성장시킵니다.

---

3개월 후, 6개월 후, 1년 후를 상상해보세요.

당신은:
- 강호무적을 완성하고
- itch.io에서 100+ 다운로드를 받고
- Reddit에서 긍정적인 피드백을 받고
- 첫 Unity 개발자 직무에 합격하거나
- 인디 개발자로 첫 수익을 벌고 있을 것입니다.

---

게임 개발은 마라톤입니다.
매일 조금씩, 꾸준히 나아가세요.

작은 승리를 축하하세요.
- 오늘 버그 1개 고쳤다 → 축하!
- 카드 1장 밸런싱했다 → 축하!
- 커밋 1개 했다 → 축하!

그 모든 작은 승리가 모여서
완성된 게임이 됩니다.

---

강호의 패자가 되는 그 날까지,
포기하지 마세요.

당신은 할 수 있습니다.

행운을 빕니다! 🎮🗡️
```

---

**축하합니다! 강호무적 완전 개발 가이드를 모두 완료하셨습니다!**

이제 부록 A-D를 확인하시고, `tasks-murim-deckbuilder-prototype-KR.md`의 작업 0.0부터 시작하세요!

---

# 부록

## 부록 A: 완전한 코드 예시

### A.1 GameManager (Complete)

```csharp
using UnityEngine;

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

    public CombatManager combatManager;
    public DataManager dataManager;
    public UIManager uiManager;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeManagers();
    }

    void InitializeManagers()
    {
        combatManager = GetComponent<CombatManager>();
        dataManager = GetComponent<DataManager>();
        uiManager = GetComponent<UIManager>();
    }

    public void StartCombat()
    {
        combatManager.InitializeCombat();
    }
}
```

### A.2 CombatManager (Complete)

```csharp
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Player player;
    public Enemy currentEnemy;
    public DeckManager deckManager;

    public int currentTurn = 0;
    public CombatState currentState;

    public void InitializeCombat()
    {
        currentTurn = 0;
        currentState = CombatState.PlayerTurn;
        deckManager.InitializeDeck();
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        currentTurn++;
        currentState = CombatState.PlayerTurn;
        player.energy = player.maxEnergy;
        deckManager.DrawCards(5);
    }

    public void EndPlayerTurn()
    {
        currentState = CombatState.EnemyTurn;
        deckManager.DiscardHand();
        ExecuteEnemyTurn();
    }

    void ExecuteEnemyTurn()
    {
        currentEnemy.ExecuteIntent();
        StartPlayerTurn();
    }

    public void PlayCard(Card card, Enemy target)
    {
        if (player.energy >= card.energyCost)
        {
            player.energy -= card.energyCost;
            card.Execute(target);
            deckManager.MoveToDiscard(card);
        }
    }
}

public enum CombatState
{
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}
```

### A.3 Card System (Complete)

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int energyCost;
    public CardType type;
    public Rarity rarity;

    [TextArea] public string description;

    public int damage;
    public int block;

    public virtual void Execute(Enemy target)
    {
        switch (type)
        {
            case CardType.Attack:
                target.TakeDamage(damage);
                break;
            case CardType.Skill:
                GameManager.Instance.combatManager.player.GainBlock(block);
                break;
        }
    }
}

public enum CardType { Attack, Skill, Power }
public enum Rarity { Common, Uncommon, Rare, Legendary }
```

> **💡 전체 코드**: GitHub 레포지토리 참조

---

## 부록 B: 추천 리소스 및 도구

### B.1 Unity 에셋

**무료:**
- TextMeshPro (내장)
- DOTween (애니메이션)
- Cinemachine (카메라)

**유료 (선택):**
- Odin Inspector ($55) - Inspector 강화
- Rewired ($45) - 입력 시스템
- Highlight Plus ($20) - 하이라이트 효과

### B.2 아트 리소스

**무료:**
- Kenney.nl (2D 에셋)
- OpenGameArt.org
- itch.io Free Assets

**폰트:**
- 나눔고딕 (무료)
- 나눔스퀘어 (무료)
- Google Fonts

### B.3 개발 도구

**필수:**
- Unity 2022.3 LTS
- Visual Studio 2022 / Rider
- Git + GitHub Desktop

**선택:**
- Notion (기획 문서)
- Trello (작업 관리)
- Discord (커뮤니티)

---

## 부록 C: FAQ

**Q1: Unity와 Godot 중 무엇을 선택해야 하나요?**
A: 주니어 개발자라면 Unity를 권장합니다. 학습 자료가 풍부하고 커뮤니티가 크기 때문입니다.

**Q2: 혼자서 게임을 완성할 수 있나요?**
A: 가능합니다! 이 가이드의 Phase 1-2를 따라하면 3-5개월 내에 프로토타입을 완성할 수 있습니다.

**Q3: 프로그래밍 경험이 없어도 되나요?**
A: C# 기초는 필수입니다. 먼저 C# 기초 튜토리얼을 완료하고 시작하세요.

**Q4: 아트 리소스는 어디서 구하나요?**
A: 프로토타입 단계에서는 무료 에셋(Kenney.nl)을 사용하고, 나중에 커스텀 아트를 의뢰하거나 구매하세요.

**Q5: 개발 기간은 얼마나 걸리나요?**
- Phase 1 (프로토타입): 1-2개월
- Phase 2 (수직 슬라이스): 2-3개월
- Phase 3-4 (완성): 4-6개월
- **총: 9-12개월 (파트타임 기준)**

**Q6: 막혔을 때 어디서 도움을 받나요?**
- Unity Forum
- Reddit r/Unity3D
- Discord (Unity 한국 커뮤니티)
- Stack Overflow

**Q7: Git을 꼭 써야 하나요?**
A: 네! 버전 관리는 필수입니다. 실수로 파일을 지우거나 버그를 만들었을 때 되돌릴 수 있습니다.

**Q8: 유료 에셋을 사야 하나요?**
A: 프로토타입 단계에서는 불필요합니다. Unity 내장 기능과 무료 에셋으로 충분합니다.

---

## 부록 D: 용어 사전

### D.1 Unity 용어

- **GameObject**: Unity 씬의 기본 오브젝트
- **Component**: GameObject에 부착되는 기능 모듈
- **Prefab**: 재사용 가능한 GameObject 템플릿
- **ScriptableObject**: 데이터 전용 에셋
- **Scene**: 게임의 화면 단위
- **Canvas**: UI를 그리는 영역

### D.2 프로그래밍 용어

- **Singleton**: 하나의 인스턴스만 존재하는 패턴
- **Coroutine**: Unity의 비동기 함수
- **Event**: 이벤트 기반 프로그래밍
- **Serialization**: 객체를 데이터로 변환

### D.3 게임 디자인 용어

- **Deck Building**: 게임 중 덱 구축 메커니즘
- **Roguelike**: 랜덤 생성 + 영구 죽음
- **Meta Progression**: 죽어도 유지되는 진행도
- **Vertical Slice**: 게임의 완전한 한 부분
- **DPE**: Damage Per Energy (에너지당 피해)
- **BPE**: Block Per Energy (에너지당 방어)

### D.4 무협 게임 용어

- **내공**: 방어/생존 계열 능력
- **무기술**: 공격/전투 계열 능력
- **경지**: 무공의 숙련도 단계
- **무공 정수**: 메타 진행 화폐
- **강호**: 무협 세계관
- **낭인**: 떠돌이 무사

---

# 마치며

## 축하합니다! 🎉

**강호무적 개발 가이드를 모두 읽으셨습니다!**

이제 여러분은:
- ✅ Unity 기초를 이해했습니다
- ✅ 게임 아키텍처를 설계할 수 있습니다
- ✅ 6가지 디자인 패턴을 적용할 수 있습니다
- ✅ Git으로 버전 관리를 할 수 있습니다
- ✅ Phase 1 개발을 시작할 준비가 되었습니다

---

## 다음 단계

### 1. 즉시 시작 (오늘)
```bash
# Unity 설치
# Git 설정
# 프로젝트 생성
```

### 2. 작업 시작 (내일부터)
- `tasks-murim-deckbuilder-prototype-KR.md` 열기
- 작업 0.0 시작
- 첫 커밋 만들기

### 3. 꾸준히 진행 (매일)
- 하루 1-2시간 개발
- 작은 목표 달성
- 체크리스트 완료

---

## 추가 리소스

### 상세 가이드 (필요 시 요청)

언제든 다음 가이드를 요청하세요:

1. **`git-workflow-guide-KR.md`** (~30페이지)
   - Git 완전 마스터
   - 협업 전략
   - 고급 명령어

2. **`card-game-design-guide-KR.md`** (~40페이지)
   - 덱 빌더 분석
   - 카드 디자인 방법론
   - 밸런싱 이론

3. **`balancing-guide-KR.md`** + **템플릿** (~25페이지)
   - DPE/BPE 계산법
   - 스프레드시트 사용법
   - 실전 밸런싱

4. **`ui-design-guide-KR.md`** (~35페이지)
   - Drag & Drop 완전 구현
   - 무협 테마 디자인
   - 애니메이션 예제

5. **`testing-debugging-guide-KR.md`** (~30페이지)
   - Unity Test Framework
   - 디버깅 완전 가이드
   - 성능 최적화

6. **`deployment-guide-KR.md`** (~25페이지)
   - Steam 배포 가이드
   - itch.io 배포 가이드
   - 빌드 최적화

---

## 커뮤니티

### 질문 및 피드백

- **GitHub Issues**: 버그 리포트 및 질문
- **Discord**: 실시간 토론 (Unity Korea)
- **Reddit**: r/Unity3D, r/gamedev

### 프로젝트 공유

완성하신 프로토타입을 공유해주세요!
- itch.io에 업로드
- GitHub에 코드 공개
- 개발 과정 블로그 작성

---

## 마지막 조언

### DO ✅
- 매일 조금씩 진행
- 작동하는 코드를 먼저 작성
- 막히면 30분 후 질문
- 플레이테스트 자주 하기
- 작은 성공을 축하하기

### DON'T ❌
- 완벽한 코드에 집착하지 않기
- 모든 기능을 동시에 만들지 않기
- 리팩토링만 계속하지 않기
- 문서를 건너뛰지 않기
- 포기하지 않기

---

## 강호의 패자가 되는 그 날까지! ⚔️

**이제 개발을 시작할 시간입니다.**

```
강호는 넓고, 무공은 무한합니다.
여러분의 게임 개발 여정에 행운이 함께하기를 바랍니다!
```

**개발 중 궁금한 점이 있으면 언제든 질문하세요!**

---

## 문서 정보

- **작성일**: 2025
- **버전**: 1.0
- **페이지**: ~150페이지 (상세 버전은 ~300페이지)
- **대상**: 주니어 게임 개발자
- **언어**: 한국어

**관련 문서:**
- `prd-murim-deckbuilder-roguelike-KR.md`
- `tasks-murim-deckbuilder-prototype-KR.md`
- `tasks-murim-deckbuilder-phase2-KR.md`
- `tech-stack-and-architecture-guide-KR.md`
- `summary-of-guides.md`
- `guide-completion.md`

---

**끝. 이제 시작입니다!** 🚀

