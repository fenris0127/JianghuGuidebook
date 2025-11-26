using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JianghuGuidebook.Core;
using JianghuGuidebook.Save;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 전투 종료 시 승리/패배 화면을 표시하는 UI 컴포넌트
    /// </summary>
    public class VictoryDefeatUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject defeatPanel;
        [SerializeField] private GameObject endingPanel;

        [Header("Victory UI")]
        [SerializeField] private TextMeshProUGUI victoryTitleText;
        [SerializeField] private TextMeshProUGUI victoryMessageText;
        [SerializeField] private TextMeshProUGUI victoryEssenceText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button victoryRetryButton;

        [Header("Defeat UI")]
        [SerializeField] private TextMeshProUGUI defeatTitleText;
        [SerializeField] private TextMeshProUGUI defeatMessageText;
        [SerializeField] private TextMeshProUGUI defeatEssenceText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Animation Settings")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float delayBeforeShow = 1f;

        [Header("Boss Dialogue UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button dialogueContinueButton;

        [Header("Ending UI")]
        [SerializeField] private TextMeshProUGUI endingTitleText;
        [SerializeField] private TextMeshProUGUI endingStoryText;
        [SerializeField] private TextMeshProUGUI endingStatsText;
        [SerializeField] private TextMeshProUGUI endingEssenceText;
        [SerializeField] private Button endingMenuButton;

        [Header("Audio")]
        [SerializeField] private AudioClip victorySound;
        [SerializeField] private AudioClip defeatSound;
        [SerializeField] private AudioClip endingSound;

        private CanvasGroup victoryCanvasGroup;
        private CanvasGroup defeatCanvasGroup;
        private AudioSource audioSource;

        private void Awake()
        {
            // CanvasGroup 초기화
            if (victoryPanel != null)
            {
                victoryCanvasGroup = victoryPanel.GetComponent<CanvasGroup>();
                if (victoryCanvasGroup == null)
                {
                    victoryCanvasGroup = victoryPanel.AddComponent<CanvasGroup>();
                }
                victoryPanel.SetActive(false);
            }

            if (defeatPanel != null)
            {
                defeatCanvasGroup = defeatPanel.GetComponent<CanvasGroup>();
                if (defeatCanvasGroup == null)
                {
                    defeatCanvasGroup = defeatPanel.AddComponent<CanvasGroup>();
                }
                defeatPanel.SetActive(false);
            }

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            // AudioSource 초기화
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // 버튼 이벤트 연결
            SetupButtons();
        }

        private void Start()
        {
            // CombatManager 이벤트 구독
            var combatManager = FindObjectOfType<Core.CombatManager>();
            if (combatManager != null)
            {
                // 승리/패배 이벤트 구독 (CombatManager에 이벤트가 있다고 가정)
                // combatManager.OnVictory += ShowVictory;
                // combatManager.OnDefeat += ShowDefeat;
            }

            // GameFlowManager 이벤트 구독 (엔딩용)
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnGameCleared += ShowEnding;
            }
        }

        /// <summary>
        /// 버튼 이벤트 설정
        /// </summary>
        private void SetupButtons()
        {
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
            }

            if (victoryRetryButton != null)
            {
                victoryRetryButton.onClick.AddListener(OnRetryClicked);
            }

            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            }

            if (dialogueContinueButton != null)
            {
                dialogueContinueButton.onClick.AddListener(OnDialogueContinueClicked);
            }

            if (endingMenuButton != null)
            {
                endingMenuButton.onClick.AddListener(OnMainMenuClicked);
            }
        }

        /// <summary>
        /// 승리 화면 표시
        /// </summary>
        public void ShowVictory(int turnsUsed = 0, int damageDealt = 0)
        {
            StartCoroutine(ShowVictoryCoroutine(turnsUsed, damageDealt));
        }

        private System.Collections.IEnumerator ShowVictoryCoroutine(int turnsUsed, int damageDealt)
        {
            // 지연
            yield return new WaitForSeconds(delayBeforeShow);

            // GameManager를 통해 런 완료 처리 (무공 정수 지급)
            int essenceEarned = 0;
            if (GameManager.Instance != null)
            {
                essenceEarned = GameManager.Instance.GetEstimatedEssence(true); // 승리 보너스 포함
                // 실제로 런 완료 처리는 보상 수령 후에 하는 것이 맞지만,
                // 여기서는 예상 정수만 계산
            }

            // 승리 사운드 재생
            if (victorySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(victorySound);
            }

            // 패널 활성화
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);

                // 승리 메시지 설정
                if (victoryTitleText != null)
                {
                    victoryTitleText.text = "승리!";
                }

                if (victoryMessageText != null)
                {
                    victoryMessageText.text = $"전투에서 승리했습니다!\n" +
                        $"사용한 턴: {turnsUsed}\n" +
                        $"가한 피해: {damageDealt}";
                }

                // 무공 정수 표시
                if (victoryEssenceText != null)
                {
                    victoryEssenceText.text = $"<color=#FFD700>예상 무공 정수: {essenceEarned}</color>";
                }

                // 페이드 인 애니메이션
                yield return StartCoroutine(FadeIn(victoryCanvasGroup));
            }
        }

        /// <summary>
        /// 패배 화면 표시
        /// </summary>
        public void ShowDefeat(int turnsSurvived = 0)
        {
            StartCoroutine(ShowDefeatCoroutine(turnsSurvived));
        }

        private System.Collections.IEnumerator ShowDefeatCoroutine(int turnsSurvived)
        {
            // 지연
            yield return new WaitForSeconds(delayBeforeShow);

            // GameManager를 통해 런 완료 처리 (무공 정수 지급)
            int essenceEarned = 0;
            if (GameManager.Instance != null)
            {
                essenceEarned = GameManager.Instance.GetEstimatedEssence(false); // 패배 (승리 보너스 없음)
                GameManager.Instance.CompleteRun(false); // 패배 처리
            }

            // 패배 사운드 재생
            if (defeatSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(defeatSound);
            }

            // 패널 활성화
            if (defeatPanel != null)
            {
                defeatPanel.SetActive(true);

                // 패배 메시지 설정
                if (defeatTitleText != null)
                {
                    defeatTitleText.text = "패배...";
                }

                if (defeatMessageText != null)
                {
                    defeatMessageText.text = $"전투에서 패배했습니다.\n" +
                        $"생존한 턴: {turnsSurvived}\n\n" +
                        $"다시 도전해보세요!";
                }

                // 무공 정수 표시
                if (defeatEssenceText != null)
                {
                    defeatEssenceText.text = $"<b>획득 무공 정수</b>\n" +
                                            $"<color=#FFA500>{essenceEarned}</color> 정수";
                }

                // 페이드 인 애니메이션
                yield return StartCoroutine(FadeIn(defeatCanvasGroup));
            }
        }

        /// <summary>
        /// 페이드 인 애니메이션
        /// </summary>
        private System.Collections.IEnumerator FadeIn(CanvasGroup canvasGroup)
        {
            if (canvasGroup == null) yield break;

            float elapsed = 0f;
            canvasGroup.alpha = 0f;

            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 계속하기 버튼 클릭 (승리 시 다음 단계로)
        /// </summary>
        private void OnContinueClicked()
        {
            Debug.Log("Continue button clicked - Showing rewards");

            // 보상 생성 및 UI 표시
            var rewardManager = Rewards.RewardManager.Instance;
            var rewardUI = FindObjectOfType<RewardUI>();
            
            if (rewardManager != null && rewardUI != null)
            {
                // 전투 타입 결정 (임시로 Boss인지 확인)
                var boss = FindObjectOfType<Combat.Boss>();
                Rewards.CombatType combatType = boss != null ? Rewards.CombatType.Boss : Rewards.CombatType.Normal;
                
                // 정예 적 확인 로직 추가 필요 (지금은 Boss 아니면 Normal)
                
                var rewards = rewardManager.GenerateCombatRewards(combatType);
                rewardUI.ShowRewards(rewards);
                
                // 승리 UI 숨기기
                HideAll();
            }
            else
            {
                Debug.LogError("RewardManager or RewardUI not found!");
                // 실패 시 맵으로 복귀
                JianghuGuidebook.Map.MapManager.Instance.ReturnToMap();
            }
        }

        /// <summary>
        /// 재시도 버튼 클릭
        /// </summary>
        private void OnRetryClicked()
        {
            Debug.Log("Retry button clicked - Reloading combat scene");

            // 현재 씬 재로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// 메인 메뉴 버튼 클릭
        /// </summary>
        private void OnMainMenuClicked()
        {
            Debug.Log("Main menu button clicked - Returning to main menu");

            // GameManager를 통해 메인 메뉴로 복귀
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToMainMenu();
            }
            else
            {
                // GameManager가 없으면 직접 씬 로드
                Debug.LogWarning("GameManager가 없습니다. 직접 MainMenuScene 로드");
                SceneManager.LoadScene("MainMenuScene");
            }
        }

        /// <summary>
        /// 모든 패널 숨기기
        /// </summary>
        public void HideAll()
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false);
            }

            if (defeatPanel != null)
            {
                defeatPanel.SetActive(false);
            }

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            if (endingPanel != null)
            {
                endingPanel.SetActive(false);
            }
        }

        /// <summary>
        /// 엔딩 화면 표시 (게임 클리어)
        /// </summary>
        public void ShowEnding()
        {
            StartCoroutine(ShowEndingCoroutine());
        }

        private System.Collections.IEnumerator ShowEndingCoroutine()
        {
            Debug.Log("[VictoryDefeatUI] 엔딩 화면 표시");

            // 지연
            yield return new WaitForSeconds(delayBeforeShow);

            // 엔딩 사운드 재생
            if (endingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(endingSound);
            }
            else if (victorySound != null && audioSource != null)
            {
                // 엔딩 사운드 없으면 승리 사운드 재생
                audioSource.PlayOneShot(victorySound);
            }

            // 런 완료 처리는 GameFlowManager에서 이미 호출됨
            // 무공 정수 획득량
            int essenceEarned = 0;
            if (GameManager.Instance != null)
            {
                essenceEarned = GameManager.Instance.GetEstimatedEssence(true);
            }

            // 패널 활성화
            if (endingPanel != null)
            {
                endingPanel.SetActive(true);

                // 엔딩 타이틀
                if (endingTitleText != null)
                {
                    endingTitleText.text = "강호 평정!";
                }

                // 엔딩 스토리
                if (endingStoryText != null)
                {
                    endingStoryText.text = GetEndingStoryText();
                }

                // 최종 통계 표시
                if (endingStatsText != null)
                {
                    RunData runData = SaveManager.Instance?.CurrentSaveData?.currentRun;
                    if (runData != null)
                    {
                        string stats = "=== 무림 여정 기록 ===\n\n";
                        stats += $"⚔️ 처치한 적: {runData.enemiesKilled}명\n";
                        stats += $"👑 격파한 보스: {runData.bossesDefeated}명\n";
                        stats += $"🗺️ 평정한 지역: {runData.regionsCompleted}개\n";
                        stats += $"🃏 수집한 카드: {runData.deckCardIds.Count}장\n";
                        stats += $"💎 획득한 유물: {runData.relicIds.Count}개\n";
                        stats += $"⏱️ 플레이 시간: {runData.GetFormattedPlayTime()}\n";
                        stats += $"🎯 총 턴 수: {runData.turnsPlayed}턴\n";
                        stats += $"💥 준 피해: {runData.damageDealt}\n";
                        stats += $"🛡️ 받은 피해: {runData.damageTaken}\n";

                        endingStatsText.text = stats;
                    }
                }

                // 무공 정수
                if (endingEssenceText != null)
                {
                    endingEssenceText.text = $"<b>획득 무공 정수</b>\n" +
                                            $"<color=#FFD700>{essenceEarned}</color> 정수";
                }

                // TODO: 페이드 인 애니메이션 (필요시)
            }
        }

        /// <summary>
        /// 엔딩 스토리 텍스트 반환
        /// </summary>
        private string GetEndingStoryText()
        {
            string story = "이름 없는 낭인으로 시작한 그대는\n";
            story += "무수한 시련을 극복하고\n";
            story += "강호의 패업을 이루었다.\n\n";

            story += "검과 내공을 갈고 닦으며\n";
            story += "수많은 적수들을 물리쳤고,\n";
            story += "마침내 천하제일인의 자리에 올랐다.\n\n";

            story += "그대의 이름은 영원히\n";
            story += "강호의 전설로 남을 것이다.";

            return story;
        }

        /// <summary>
        /// 보스 승리 연출 (대사 출력 후 승리 화면)
        /// </summary>
        public void ShowBossVictory(string bossName, string dialogue, int turnsUsed, int damageDealt)
        {
            StartCoroutine(ShowBossVictoryCoroutine(bossName, dialogue, turnsUsed, damageDealt));
        }

        private System.Collections.IEnumerator ShowBossVictoryCoroutine(string bossName, string dialogue, int turnsUsed, int damageDealt)
        {
            // 지연
            yield return new WaitForSeconds(delayBeforeShow);

            // 대사 패널 표시
            if (dialoguePanel != null && dialogueText != null)
            {
                dialoguePanel.SetActive(true);
                dialogueText.text = $"\"{dialogue}\"\n- {bossName}";
                
                // 승리 화면 데이터 저장 (대사 넘긴 후 표시용)
                this.pendingTurnsUsed = turnsUsed;
                this.pendingDamageDealt = damageDealt;
            }
            else
            {
                // 대사 패널 없으면 바로 승리 화면
                ShowVictory(turnsUsed, damageDealt);
            }
        }

        private int pendingTurnsUsed;
        private int pendingDamageDealt;

        private void OnDialogueContinueClicked()
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }
            
            // 승리 화면 표시
            ShowVictory(pendingTurnsUsed, pendingDamageDealt);
        }

        /// <summary>
        /// 테스트용 메서드
        /// </summary>
        [ContextMenu("Test Victory")]
        private void TestVictory()
        {
            ShowVictory(10, 500);
        }

        [ContextMenu("Test Defeat")]
        private void TestDefeat()
        {
            ShowDefeat(7);
        }
    }
}
