using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JianghuGuidebook.Save;
using JianghuGuidebook.Core;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 메인 메뉴 UI 관리
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("메뉴 버튼")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button metaProgressionButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button achievementsButton;
        [SerializeField] private Button codexButton;
        [SerializeField] private Button quitButton;

        [Header("세이브 슬롯 선택 (이어하기)")]
        [SerializeField] private GameObject continueSlotPanel;
        [SerializeField] private Button[] slotButtons = new Button[3];
        [SerializeField] private TextMeshProUGUI[] slotInfoTexts = new TextMeshProUGUI[3];
        [SerializeField] private GameObject[] slotEmptyIndicators = new GameObject[3];
        [SerializeField] private Button closeSlotsButton;

        [Header("새 게임 슬롯 선택")]
        [SerializeField] private GameObject newGameSlotPanel;
        [SerializeField] private Button[] newGameSlotButtons = new Button[3];
        [SerializeField] private TextMeshProUGUI[] newGameSlotInfoTexts = new TextMeshProUGUI[3];
        [SerializeField] private Button closeNewGameSlotsButton;

        [Header("서브 UI")]
        [SerializeField] private FactionSelectionUI factionSelectionUI;
        [SerializeField] private AchievementUI achievementUI;
        [SerializeField] private CodexUI codexUI;

        [Header("씬 설정")]
        [SerializeField] private string mapSceneName = "MapScene";
        [SerializeField] private string metaProgressionSceneName = "MetaProgressionScene";

        [Header("이어하기 버튼 비활성화 설정")]
        [SerializeField] private Color disabledButtonColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        [SerializeField] private Color enabledButtonColor = Color.white;

        private int selectedSlot = -1;

        private void Start()
        {
            // 버튼 리스너 등록
            if (newGameButton != null)
                newGameButton.onClick.AddListener(OnNewGameClicked);

            if (continueButton != null)
                continueButton.onClick.AddListener(OnContinueClicked);

            if (metaProgressionButton != null)
                metaProgressionButton.onClick.AddListener(OnMetaProgressionClicked);

            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);

            if (achievementsButton != null)
            {
                achievementsButton.onClick.AddListener(OnAchievementsClicked);
            }

            if (codexButton != null)
            {
                codexButton.onClick.AddListener(OnCodexClicked);
            }

            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitClicked);

            // 슬롯 버튼 리스너 등록
            for (int i = 0; i < slotButtons.Length; i++)
            {
                if (slotButtons[i] != null)
                {
                    int slotIndex = i; // 클로저 문제 방지
                    slotButtons[i].onClick.AddListener(() => OnSlotSelected(slotIndex));
                }
            }

            // 새 게임 슬롯 버튼 리스너 등록
            for (int i = 0; i < newGameSlotButtons.Length; i++)
            {
                if (newGameSlotButtons[i] != null)
                {
                    int slotIndex = i;
                    newGameSlotButtons[i].onClick.AddListener(() => OnNewGameSlotSelected(slotIndex));
                }
            }

            if (closeSlotsButton != null)
                closeSlotsButton.onClick.AddListener(CloseContinueSlotPanel);

            if (closeNewGameSlotsButton != null)
                closeNewGameSlotsButton.onClick.AddListener(CloseNewGameSlotPanel);

            // 분파 선택 UI 이벤트 등록
            if (factionSelectionUI != null)
            {
                factionSelectionUI.OnFactionConfirmed += OnFactionSelected;
            }

            // 패널 초기화
            if (continueSlotPanel != null)
                continueSlotPanel.SetActive(false);

            if (newGameSlotPanel != null)
                newGameSlotPanel.SetActive(false);

            // 이어하기 버튼 상태 업데이트
            UpdateContinueButtonState();
        }

        /// <summary>
        /// 이어하기 버튼 활성화 상태를 업데이트합니다
        /// </summary>
        private void UpdateContinueButtonState()
        {
            if (continueButton == null)
                return;

            // 세이브 파일이 하나라도 있는지 확인
            bool hasSaveFile = false;

            if (SaveManager.Instance != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (SaveManager.Instance.SaveFileExists(i))
                    {
                        hasSaveFile = true;
                        break;
                    }
                }
            }

            // 버튼 활성화/비활성화
            continueButton.interactable = hasSaveFile;

            // 시각적 피드백
            Image buttonImage = continueButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = hasSaveFile ? enabledButtonColor : disabledButtonColor;
            }

            // 텍스트 색상 변경 (선택적)
            TextMeshProUGUI buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.color = hasSaveFile ? Color.white : new Color(0.7f, 0.7f, 0.7f);
            }

            Debug.Log($"[MainMenuUI] 이어하기 버튼 상태: {(hasSaveFile ? "활성화" : "비활성화")}");
        }

        /// <summary>
        /// 새 게임 버튼 클릭
        /// </summary>
        private void OnNewGameClicked()
        {
            Debug.Log("[MainMenuUI] 새 게임 버튼 클릭");

            // 새 게임 슬롯 선택 패널 표시
            if (newGameSlotPanel != null)
            {
                newGameSlotPanel.SetActive(true);
                UpdateNewGameSlotInfo();
            }
            else
            {
                // 패널이 없으면 바로 슬롯 0으로 시작
                StartNewGame(0);
            }
        }

        /// <summary>
        /// 이어하기 버튼 클릭
        /// </summary>
        private void OnContinueClicked()
        {
            Debug.Log("[MainMenuUI] 이어하기 버튼 클릭");

            // 슬롯 선택 패널 표시
            if (continueSlotPanel != null)
            {
                continueSlotPanel.SetActive(true);
                UpdateSlotInfo();
            }
            else
            {
                // 패널이 없으면 첫 번째 세이브 파일 로드
                LoadGameFromSlot(0);
            }
        }

        /// <summary>
        /// 무공비전 버튼 클릭
        /// </summary>
        private void OnMetaProgressionClicked()
        {
            Debug.Log("[MainMenuUI] 무공비전 버튼 클릭");

            // 메타 진행 씬 로드
            if (!string.IsNullOrEmpty(metaProgressionSceneName))
            {
                SceneManager.LoadScene(metaProgressionSceneName);
            }
            else
            {
                Debug.LogWarning("메타 진행 씬 이름이 설정되지 않았습니다");
            }
        }

        /// <summary>
        /// 설정 버튼 클릭
        /// </summary>
        private void OnSettingsClicked()
        {
            Debug.Log("[MainMenuUI] 설정 버튼 클릭");
            // TODO: 설정 화면 구현
            Debug.LogWarning("설정 화면은 아직 구현되지 않았습니다");
        }

        /// <summary>
        /// 업적 버튼 클릭
        /// </summary>
        private void OnAchievementsClicked()
        {
            Debug.Log("[MainMenuUI] 업적 버튼 클릭");
            if (achievementUI != null)
            {
                achievementUI.Open();
            }
            else
            {
                Debug.LogWarning("AchievementUI가 설정되지 않았습니다");
            }
        }

        /// <summary>
        /// 종료 버튼 클릭
        /// </summary>
        private void OnQuitClicked()
        {
            Debug.Log("[MainMenuUI] 게임 종료");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitGame();
            }
            else
            {
                Application.Quit();
            }
        }

        /// <summary>
        /// 슬롯 정보를 업데이트합니다
        /// </summary>
        private void UpdateSlotInfo()
        {
            if (SaveManager.Instance == null)
                return;

            for (int i = 0; i < slotButtons.Length; i++)
            {
                bool saveExists = SaveManager.Instance.SaveFileExists(i);

                // 슬롯 정보 텍스트 업데이트
                if (slotInfoTexts[i] != null)
                {
                    if (saveExists)
                    {
                        // 세이브 파일 정보 로드 (간단히 표시)
                        slotInfoTexts[i].text = $"슬롯 {i + 1}\n저장된 게임";
                    }
                    else
                    {
                        slotInfoTexts[i].text = $"슬롯 {i + 1}\n비어 있음";
                    }
                }

                // 빈 슬롯 표시
                if (slotEmptyIndicators[i] != null)
                {
                    slotEmptyIndicators[i].SetActive(!saveExists);
                }

                // 버튼 활성화/비활성화
                if (slotButtons[i] != null)
                {
                    slotButtons[i].interactable = saveExists;
                }
            }
        }

        /// <summary>
        /// 새 게임 슬롯 정보를 업데이트합니다
        /// </summary>
        private void UpdateNewGameSlotInfo()
        {
            if (SaveManager.Instance == null)
                return;

            for (int i = 0; i < newGameSlotButtons.Length; i++)
            {
                bool saveExists = SaveManager.Instance.SaveFileExists(i);

                // 슬롯 정보 텍스트 업데이트
                if (newGameSlotInfoTexts[i] != null)
                {
                    if (saveExists)
                    {
                        newGameSlotInfoTexts[i].text = $"슬롯 {i + 1}\n(기존 저장 덮어쓰기)";
                    }
                    else
                    {
                        newGameSlotInfoTexts[i].text = $"슬롯 {i + 1}\n(새 게임)";
                    }
                }

                // 모든 슬롯 활성화 (새 게임은 덮어쓰기 가능)
                if (newGameSlotButtons[i] != null)
                {
                    newGameSlotButtons[i].interactable = true;
                }
            }
        }

        /// <summary>
        /// 슬롯을 선택합니다 (이어하기)
        /// </summary>
        private void OnSlotSelected(int slotIndex)
        {
            Debug.Log($"[MainMenuUI] 슬롯 {slotIndex} 선택 (이어하기)");
            selectedSlot = slotIndex;

            LoadGameFromSlot(slotIndex);
        }

        /// <summary>
        /// 슬롯을 선택합니다 (새 게임)
        /// </summary>
        private void OnNewGameSlotSelected(int slotIndex)
        {
            Debug.Log($"[MainMenuUI] 슬롯 {slotIndex} 선택 (새 게임)");
            selectedSlot = slotIndex;

            // 새 게임 슬롯 패널 닫기
            CloseNewGameSlotPanel();

            // 분파 선택 화면 표시
            if (factionSelectionUI != null)
            {
                // FactionSelectionUI 초기화 및 표시
                factionSelectionUI.gameObject.SetActive(true);
                factionSelectionUI.Initialize();
                
                // 선택 완료 콜백 설정 (익명 함수로 슬롯 인덱스 전달)
                // 기존 이벤트 리스너 제거 후 새로 등록
                factionSelectionUI.OnFactionConfirmed = (faction) => OnFactionSelected(faction, slotIndex);
            }
            else
            {
                Debug.LogWarning("FactionSelectionUI가 설정되지 않았습니다. 기본 분파로 시작합니다.");
                // 기본 분파 (화산파)로 시작
                FactionData defaultFaction = FactionManager.Instance?.GetFactionById("hwasan");
                if (defaultFaction != null)
                {
                    StartNewGameWithFaction(defaultFaction, slotIndex);
                }
                else
                {
                    StartNewGame(slotIndex);
                }
            }
        }

        /// <summary>
        /// 분파 선택 완료 이벤트 핸들러
        /// </summary>
        private void OnFactionSelected(FactionData faction, int slotIndex)
        {
            Debug.Log($"[MainMenuUI] 분파 선택 완료: {faction.name}, 슬롯: {slotIndex}");
            StartNewGameWithFaction(faction, slotIndex);
        }

        /// <summary>
        /// 슬롯에서 게임을 로드합니다
        /// </summary>
        private void LoadGameFromSlot(int slotIndex)
        {
            if (SaveManager.Instance == null)
            {
                Debug.LogError("SaveManager가 없습니다");
                return;
            }

            // 게임 로드
            bool success = SaveManager.Instance.LoadGame(slotIndex);

            if (success)
            {
                Debug.Log($"[MainMenuUI] 슬롯 {slotIndex}에서 게임 로드 성공");

                // 맵 씬으로 이동
                if (!string.IsNullOrEmpty(mapSceneName))
                {
                    SceneManager.LoadScene(mapSceneName);
                }
                else
                {
                    Debug.LogError("맵 씬 이름이 설정되지 않았습니다");
                }
            }
            else
            {
                Debug.LogError($"[MainMenuUI] 슬롯 {slotIndex}에서 게임 로드 실패");
                // TODO: 로드 실패 팝업 표시
            }
        }

        /// <summary>
        /// 새 게임을 시작합니다 (기본값)
        /// </summary>
        private void StartNewGame(int slotIndex)
        {
            Debug.Log($"[MainMenuUI] 슬롯 {slotIndex}에서 새 게임 시작 (기본 설정)");

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager가 없습니다");
                return;
            }

            // 선택한 슬롯 저장 (나중에 저장할 때 사용)
            PlayerPrefs.SetInt("CurrentSaveSlot", slotIndex);
            PlayerPrefs.Save();

            // 새 게임 시작 (GameManager가 맵 씬 로드까지 처리)
            GameManager.Instance.StartNewRun();
        }

        /// <summary>
        /// 선택한 분파로 새 게임을 시작합니다
        /// </summary>
        private void StartNewGameWithFaction(FactionData faction, int slotIndex)
        {
            Debug.Log($"[MainMenuUI] 슬롯 {slotIndex}에서 {faction.name} 분파로 새 게임 시작");

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager가 없습니다");
                return;
            }

            // 선택한 슬롯 저장
            PlayerPrefs.SetInt("CurrentSaveSlot", slotIndex);

            // 선택한 분파 ID 저장 (GameManager에서 사용)
            PlayerPrefs.SetString("SelectedFactionId", faction.id);

            PlayerPrefs.Save();

            Debug.Log($"[MainMenuUI] 분파 ID 저장: {faction.id}");

            // 새 게임 시작 (GameManager가 맵 씬 로드까지 처리)
            GameManager.Instance.StartNewRun();
        }

        /// <summary>
        /// 이어하기 슬롯 패널을 닫습니다
        /// </summary>
        private void CloseContinueSlotPanel()
        {
            if (continueSlotPanel != null)
            {
                continueSlotPanel.SetActive(false);
            }
        }

        /// <summary>
        /// 새 게임 슬롯 패널을 닫습니다
        /// </summary>
        private void CloseNewGameSlotPanel()
        {
            if (newGameSlotPanel != null)
            {
                newGameSlotPanel.SetActive(false);
            }
        }

        /// <summary>
        /// 메인 메뉴로 돌아왔을 때 버튼 상태 갱신
        /// </summary>
        private void OnEnable()
        {
            UpdateContinueButtonState();
        }
    }
}
