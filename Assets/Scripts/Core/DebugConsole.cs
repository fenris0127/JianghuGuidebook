using UnityEngine;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 테스트 및 디버깅을 위한 치트 콘솔
    /// </summary>
    public class DebugConsole : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject consolePanel;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI outputText;
        [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote; // ~ 키

        [Header("Settings")]
        [SerializeField] private bool enableInBuild = false;
        [SerializeField] private int maxOutputLines = 20;

        private List<string> outputHistory = new List<string>();
        private List<string> commandHistory = new List<string>();
        private int historyIndex = -1;
        private bool isOpen = false;

        private Player player;
        private Enemy currentEnemy;
        private DeckManager deckManager;

        private void Awake()
        {
            // 빌드에서 비활성화 (옵션)
            if (!Application.isEditor && !enableInBuild)
            {
                gameObject.SetActive(false);
                return;
            }

            if (consolePanel != null)
            {
                consolePanel.SetActive(false);
            }

            // 초기 메시지
            AddOutput("=== 강호무적 디버그 콘솔 ===");
            AddOutput("'help'를 입력하여 명령어 목록 확인");
        }

        private void Start()
        {
            // 컴포넌트 찾기
            RefreshReferences();
        }

        private void Update()
        {
            // 콘솔 토글
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleConsole();
            }

            // 콘솔이 열려있을 때
            if (isOpen)
            {
                // Enter로 명령 실행
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    ExecuteCommand(inputField.text);
                    inputField.text = "";
                    inputField.ActivateInputField();
                }

                // 위/아래 화살표로 명령 히스토리 탐색
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    NavigateHistory(-1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    NavigateHistory(1);
                }
            }
        }

        /// <summary>
        /// 콘솔 토글
        /// </summary>
        private void ToggleConsole()
        {
            isOpen = !isOpen;

            if (consolePanel != null)
            {
                consolePanel.SetActive(isOpen);
            }

            if (isOpen && inputField != null)
            {
                inputField.ActivateInputField();
            }
        }

        /// <summary>
        /// 명령 히스토리 탐색
        /// </summary>
        private void NavigateHistory(int direction)
        {
            if (commandHistory.Count == 0) return;

            historyIndex += direction;
            historyIndex = Mathf.Clamp(historyIndex, 0, commandHistory.Count - 1);

            if (inputField != null && historyIndex >= 0 && historyIndex < commandHistory.Count)
            {
                inputField.text = commandHistory[historyIndex];
                inputField.caretPosition = inputField.text.Length;
            }
        }

        /// <summary>
        /// 명령 실행
        /// </summary>
        private void ExecuteCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return;

            AddOutput($"> {command}");
            commandHistory.Add(command);
            historyIndex = commandHistory.Count;

            string[] parts = command.ToLower().Trim().Split(' ');
            string cmd = parts[0];

            switch (cmd)
            {
                case "help":
                    ShowHelp();
                    break;

                case "clear":
                    ClearOutput();
                    break;

                case "refresh":
                    RefreshReferences();
                    AddOutput("컴포넌트 참조 새로고침 완료");
                    break;

                // 플레이어 치트
                case "god":
                case "godmode":
                    ToggleGodMode();
                    break;

                case "heal":
                    HealPlayer(parts.Length > 1 ? ParseInt(parts[1], 999) : 999);
                    break;

                case "damage":
                    DamagePlayer(parts.Length > 1 ? ParseInt(parts[1], 10) : 10);
                    break;

                case "energy":
                    SetEnergy(parts.Length > 1 ? ParseInt(parts[1], 10) : 10);
                    break;

                case "block":
                    GiveBlock(parts.Length > 1 ? ParseInt(parts[1], 50) : 50);
                    break;

                // 적 치트
                case "killenemy":
                case "kill":
                    KillEnemy();
                    break;

                case "damageenemy":
                    DamageEnemy(parts.Length > 1 ? ParseInt(parts[1], 10) : 10);
                    break;

                // 카드 치트
                case "draw":
                    DrawCards(parts.Length > 1 ? ParseInt(parts[1], 1) : 1);
                    break;

                case "addcard":
                    AddCardToHand(parts.Length > 1 ? parts[1] : "일검");
                    break;

                case "clearhand":
                    ClearHand();
                    break;

                // 전투 치트
                case "win":
                case "victory":
                    InstantVictory();
                    break;

                case "endturn":
                    EndTurn();
                    break;

                // 정보 명령
                case "status":
                case "info":
                    ShowStatus();
                    break;

                default:
                    AddOutput($"알 수 없는 명령: {cmd}. 'help'를 입력하세요.");
                    break;
            }
        }

        #region Commands

        private void ShowHelp()
        {
            AddOutput("=== 사용 가능한 명령어 ===");
            AddOutput("");
            AddOutput("일반:");
            AddOutput("  help - 이 도움말 표시");
            AddOutput("  clear - 콘솔 출력 지우기");
            AddOutput("  refresh - 컴포넌트 참조 새로고침");
            AddOutput("  status - 현재 전투 상태 표시");
            AddOutput("");
            AddOutput("플레이어:");
            AddOutput("  godmode - 무적 모드 토글");
            AddOutput("  heal [amount] - 체력 회복 (기본 999)");
            AddOutput("  damage [amount] - 플레이어 피해 (기본 10)");
            AddOutput("  energy [amount] - 내공 설정 (기본 10)");
            AddOutput("  block [amount] - 방어도 추가 (기본 50)");
            AddOutput("");
            AddOutput("적:");
            AddOutput("  kill - 현재 적 즉시 처치");
            AddOutput("  damageenemy [amount] - 적에게 피해 (기본 10)");
            AddOutput("");
            AddOutput("카드:");
            AddOutput("  draw [count] - 카드 드로우 (기본 1)");
            AddOutput("  addcard [name] - 특정 카드 추가");
            AddOutput("  clearhand - 손패 비우기");
            AddOutput("");
            AddOutput("전투:");
            AddOutput("  win - 즉시 승리");
            AddOutput("  endturn - 턴 종료");
        }

        private void ShowStatus()
        {
            if (player != null)
            {
                AddOutput($"[플레이어] HP: {player.CurrentHealth}/{player.MaxHealth}, " +
                    $"에너지: {player.CurrentEnergy}/{player.MaxEnergy}, " +
                    $"방어도: {player.Block}");
            }
            else
            {
                AddOutput("[플레이어] 없음");
            }

            if (currentEnemy != null)
            {
                AddOutput($"[적] {currentEnemy.EnemyName} HP: {currentEnemy.CurrentHealth}/{currentEnemy.MaxHealth}, " +
                    $"방어도: {currentEnemy.Block}");
            }
            else
            {
                AddOutput("[적] 없음");
            }

            if (deckManager != null)
            {
                AddOutput($"[덱] 뽑기: {deckManager.GetDrawPileCount()}, " +
                    $"손패: {deckManager.GetHand().Count}, " +
                    $"버리기: {deckManager.GetDiscardPileCount()}, " +
                    $"소진: {deckManager.GetExhaustPileCount()}");
            }
        }

        private bool godModeEnabled = false;
        private void ToggleGodMode()
        {
            godModeEnabled = !godModeEnabled;
            AddOutput($"무적 모드: {(godModeEnabled ? "활성화" : "비활성화")}");

            if (player != null && godModeEnabled)
            {
                // 무적 모드 구현 (Player 클래스에 플래그 추가 필요)
                player.CurrentHealth = player.MaxHealth;
                AddOutput("체력이 최대치로 회복되었습니다.");
            }
        }

        private void HealPlayer(int amount)
        {
            if (player == null)
            {
                AddOutput("플레이어를 찾을 수 없습니다.");
                return;
            }

            player.Heal(amount);
            AddOutput($"플레이어 체력 {amount} 회복. 현재: {player.CurrentHealth}/{player.MaxHealth}");
        }

        private void DamagePlayer(int amount)
        {
            if (player == null)
            {
                AddOutput("플레이어를 찾을 수 없습니다.");
                return;
            }

            if (godModeEnabled)
            {
                AddOutput("무적 모드가 활성화되어 있습니다.");
                return;
            }

            player.TakeDamage(amount);
            AddOutput($"플레이어가 {amount} 피해를 입었습니다. 현재: {player.CurrentHealth}/{player.MaxHealth}");
        }

        private void SetEnergy(int amount)
        {
            if (player == null)
            {
                AddOutput("플레이어를 찾을 수 없습니다.");
                return;
            }

            player.CurrentEnergy = amount;
            AddOutput($"내공을 {amount}로 설정했습니다.");
        }

        private void GiveBlock(int amount)
        {
            if (player == null)
            {
                AddOutput("플레이어를 찾을 수 없습니다.");
                return;
            }

            player.GainBlock(amount);
            AddOutput($"방어도 {amount} 획득. 현재: {player.Block}");
        }

        private void KillEnemy()
        {
            if (currentEnemy == null)
            {
                AddOutput("적을 찾을 수 없습니다.");
                return;
            }

            currentEnemy.TakeDamage(9999);
            AddOutput($"{currentEnemy.EnemyName}를 처치했습니다!");
        }

        private void DamageEnemy(int amount)
        {
            if (currentEnemy == null)
            {
                AddOutput("적을 찾을 수 없습니다.");
                return;
            }

            currentEnemy.TakeDamage(amount);
            AddOutput($"{currentEnemy.EnemyName}에게 {amount} 피해. 현재: {currentEnemy.CurrentHealth}/{currentEnemy.MaxHealth}");
        }

        private void DrawCards(int count)
        {
            if (deckManager == null)
            {
                AddOutput("덱 매니저를 찾을 수 없습니다.");
                return;
            }

            deckManager.DrawCards(count);
            AddOutput($"{count}장의 카드를 뽑았습니다.");
        }

        private void AddCardToHand(string cardName)
        {
            AddOutput($"카드 추가 기능은 아직 구현되지 않았습니다: {cardName}");
            // TODO: DataManager에서 카드 데이터를 가져와 손패에 추가
        }

        private void ClearHand()
        {
            if (deckManager == null)
            {
                AddOutput("덱 매니저를 찾을 수 없습니다.");
                return;
            }

            var hand = new List<Card>(deckManager.GetHand());
            foreach (var card in hand)
            {
                deckManager.DiscardCard(card);
            }

            AddOutput("손패를 버렸습니다.");
        }

        private void InstantVictory()
        {
            var enemies = FindObjectsOfType<Enemy>();

            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(9999);
            }

            AddOutput($"{enemies.Length}명의 적을 모두 처치했습니다. 승리!");
        }

        private void EndTurn()
        {
            var combatUI = FindObjectOfType<UI.CombatUI>();

            if (combatUI != null)
            {
                combatUI.EndPlayerTurn();
                AddOutput("플레이어 턴을 종료했습니다.");
            }
            else
            {
                AddOutput("CombatUI를 찾을 수 없습니다.");
            }
        }

        #endregion

        #region Helper Methods

        private void RefreshReferences()
        {
            player = FindObjectOfType<Player>();
            currentEnemy = FindObjectOfType<Enemy>();
            deckManager = FindObjectOfType<DeckManager>();
        }

        private void AddOutput(string text)
        {
            outputHistory.Add(text);

            // 최대 라인 수 제한
            if (outputHistory.Count > maxOutputLines)
            {
                outputHistory.RemoveAt(0);
            }

            UpdateOutputDisplay();
        }

        private void ClearOutput()
        {
            outputHistory.Clear();
            UpdateOutputDisplay();
        }

        private void UpdateOutputDisplay()
        {
            if (outputText != null)
            {
                outputText.text = string.Join("\n", outputHistory);
            }
        }

        private int ParseInt(string value, int defaultValue)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        #endregion

        #region Public API

        /// <summary>
        /// 외부에서 콘솔 메시지 출력
        /// </summary>
        public static void Log(string message)
        {
            var console = FindObjectOfType<DebugConsole>();
            if (console != null)
            {
                console.AddOutput($"[LOG] {message}");
            }
        }

        #endregion
    }
}
