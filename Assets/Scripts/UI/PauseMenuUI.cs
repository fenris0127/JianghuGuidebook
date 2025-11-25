using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 일시정지 메뉴 UI (ESC 키)
    /// 런 포기 옵션 포함
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("메뉴 패널")]
        [SerializeField] private GameObject pauseMenuPanel;

        [Header("버튼")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button abandonRunButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [Header("런 포기 확인 다이얼로그")]
        [SerializeField] private GameObject abandonConfirmDialog;
        [SerializeField] private TextMeshProUGUI abandonConfirmMessageText;
        [SerializeField] private TextMeshProUGUI essenceInfoText;
        [SerializeField] private Button abandonConfirmYesButton;
        [SerializeField] private Button abandonConfirmNoButton;

        [Header("입력 설정")]
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

        private bool isPaused = false;

        private void Start()
        {
            // 버튼 리스너 등록
            if (resumeButton != null)
                resumeButton.onClick.AddListener(OnResumeClicked);

            if (abandonRunButton != null)
                abandonRunButton.onClick.AddListener(OnAbandonRunClicked);

            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);

            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitClicked);

            if (abandonConfirmYesButton != null)
                abandonConfirmYesButton.onClick.AddListener(OnAbandonConfirmYes);

            if (abandonConfirmNoButton != null)
                abandonConfirmNoButton.onClick.AddListener(OnAbandonConfirmNo);

            // 초기 상태
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);

            if (abandonConfirmDialog != null)
                abandonConfirmDialog.SetActive(false);

            isPaused = false;
        }

        private void Update()
        {
            // ESC 키 입력 감지
            if (Input.GetKeyDown(pauseKey))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        /// <summary>
        /// 게임을 일시정지합니다
        /// </summary>
        private void Pause()
        {
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(true);
            }

            // 게임 시간 정지
            Time.timeScale = 0f;
            isPaused = true;

            Debug.Log("[PauseMenuUI] 게임 일시정지");
        }

        /// <summary>
        /// 게임을 재개합니다
        /// </summary>
        private void Resume()
        {
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }

            // 확인 다이얼로그도 닫기
            if (abandonConfirmDialog != null)
            {
                abandonConfirmDialog.SetActive(false);
            }

            // 게임 시간 재개
            Time.timeScale = 1f;
            isPaused = false;

            Debug.Log("[PauseMenuUI] 게임 재개");
        }

        /// <summary>
        /// 재개 버튼 클릭
        /// </summary>
        private void OnResumeClicked()
        {
            Resume();
        }

        /// <summary>
        /// 런 포기 버튼 클릭
        /// </summary>
        private void OnAbandonRunClicked()
        {
            Debug.Log("[PauseMenuUI] 런 포기 버튼 클릭");
            ShowAbandonConfirmDialog();
        }

        /// <summary>
        /// 설정 버튼 클릭
        /// </summary>
        private void OnSettingsClicked()
        {
            Debug.Log("[PauseMenuUI] 설정 버튼 클릭");
            // TODO: 설정 화면 구현
            Debug.LogWarning("설정 화면은 아직 구현되지 않았습니다");
        }

        /// <summary>
        /// 게임 종료 버튼 클릭
        /// </summary>
        private void OnQuitClicked()
        {
            Debug.Log("[PauseMenuUI] 게임 종료 버튼 클릭");

            // 게임 시간 정상화
            Time.timeScale = 1f;

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
        /// 런 포기 확인 다이얼로그를 표시합니다
        /// </summary>
        private void ShowAbandonConfirmDialog()
        {
            if (abandonConfirmDialog == null)
            {
                Debug.LogWarning("런 포기 확인 다이얼로그가 설정되지 않았습니다");
                return;
            }

            // 현재 획득 가능한 무공 정수 계산
            UpdateAbandonEssenceInfo();

            // 확인 메시지 설정
            if (abandonConfirmMessageText != null)
            {
                abandonConfirmMessageText.text =
                    "정말로 런을 포기하시겠습니까?\n\n" +
                    "포기 시 현재까지의 진행 상황이 사라지며,\n" +
                    "무공 정수를 절반만 획득합니다.";
            }

            abandonConfirmDialog.SetActive(true);
            Debug.Log("[PauseMenuUI] 런 포기 확인 다이얼로그 표시");
        }

        /// <summary>
        /// 획득 가능한 무공 정수 정보를 업데이트합니다
        /// </summary>
        private void UpdateAbandonEssenceInfo()
        {
            if (essenceInfoText == null)
                return;

            // GameManager에서 현재 무공 정수 계산
            if (GameManager.Instance != null)
            {
                int fullEssence = GameManager.Instance.GetEstimatedEssence(false);
                int abandonEssence = GameManager.Instance.GetAbandonEssence();

                essenceInfoText.text = $"<b>포기 시 획득 무공 정수</b>\n\n" +
                                      $"현재 진행도: <color=#FFD700>{fullEssence}</color> 정수\n" +
                                      $"포기 시 획득: <color=#FFA500>{abandonEssence}</color> 정수 (50%)";
            }
            else
            {
                essenceInfoText.text = "포기 시 획득: 현재 진행도의 50%";
            }
        }

        /// <summary>
        /// 런 포기 확인 - 예
        /// </summary>
        private void OnAbandonConfirmYes()
        {
            Debug.Log("[PauseMenuUI] 런 포기 확정");

            if (abandonConfirmDialog != null)
            {
                abandonConfirmDialog.SetActive(false);
            }

            // 게임 시간 정상화
            Time.timeScale = 1f;
            isPaused = false;

            // GameManager를 통해 런 포기
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AbandonRun();
            }
            else
            {
                Debug.LogError("GameManager가 없습니다");
            }
        }

        /// <summary>
        /// 런 포기 확인 - 아니오
        /// </summary>
        private void OnAbandonConfirmNo()
        {
            Debug.Log("[PauseMenuUI] 런 포기 취소");

            if (abandonConfirmDialog != null)
            {
                abandonConfirmDialog.SetActive(false);
            }
        }

        /// <summary>
        /// 강제로 메뉴를 닫습니다 (외부에서 호출)
        /// </summary>
        public void ForceClose()
        {
            if (isPaused)
            {
                Resume();
            }
        }

        private void OnDestroy()
        {
            // 씬 전환 시 게임 시간 정상화
            Time.timeScale = 1f;
        }
    }
}
