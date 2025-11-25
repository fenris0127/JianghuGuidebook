using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 전투 UI의 메인 컨트롤러. 모든 UI 컴포넌트를 통합하고 관리합니다.
    /// </summary>
    public class CombatUI : MonoBehaviour
    {
        [Header("Core References")]
        [SerializeField] private Core.CombatManager combatManager;
        [SerializeField] private Combat.Player player;
        [SerializeField] private Canvas mainCanvas;

        [Header("UI Components")]
        [SerializeField] private PlayerStatsUI playerStatsUI;
        [SerializeField] private HandLayoutManager handLayoutManager;
        [SerializeField] private VictoryDefeatUI victoryDefeatUI;

        [Header("Enemy UI")]
        [SerializeField] private Transform enemyUIContainer;
        [SerializeField] private GameObject enemyUIPrefab;
        private List<EnemyIntentUI> enemyUIList = new List<EnemyIntentUI>();

        [Header("Boss UI")]
        [SerializeField] private BossUI bossUI;

        [Header("Buttons")]
        [SerializeField] private Button endTurnButton;
        [SerializeField] private TextMeshProUGUI endTurnButtonText;

        [Header("Pile Counters")]
        [SerializeField] private TextMeshProUGUI drawPileCountText;
        [SerializeField] private TextMeshProUGUI discardPileCountText;
        [SerializeField] private TextMeshProUGUI exhaustPileCountText;
        [SerializeField] private Button drawPileButton;
        [SerializeField] private Button discardPileButton;

        [Header("Turn Indicator")]
        [SerializeField] private GameObject playerTurnIndicator;
        [SerializeField] private GameObject enemyTurnIndicator;
        [SerializeField] private TextMeshProUGUI turnCountText;

        [Header("Damage Popup")]
        [SerializeField] private GameObject damagePopupPrefab;

        private Cards.DeckManager deckManager;
        private int currentTurn = 0;
        private bool isPlayerTurn = true;

        private void Awake()
        {
            // 자동으로 컴포넌트 찾기
            if (combatManager == null)
            {
                combatManager = FindObjectOfType<Core.CombatManager>();
            }

            if (player == null)
            {
                player = FindObjectOfType<Combat.Player>();
            }

            if (mainCanvas == null)
            {
                mainCanvas = GetComponentInParent<Canvas>();
            }

            if (playerStatsUI == null)
            {
                playerStatsUI = FindObjectOfType<PlayerStatsUI>();
            }

            if (handLayoutManager == null)
            {
                handLayoutManager = FindObjectOfType<HandLayoutManager>();
            }

            if (victoryDefeatUI == null)
            {
                victoryDefeatUI = FindObjectOfType<VictoryDefeatUI>();
            }
            
            if (bossUI == null)
            {
                bossUI = FindObjectOfType<BossUI>();
            }

            // 버튼 이벤트 설정
            SetupButtons();
        }

        private void Start()
        {
            // DeckManager 초기화
            deckManager = FindObjectOfType<Cards.DeckManager>();

            if (deckManager == null)
            {
                Debug.LogError("CombatUI: DeckManager not found!");
            }

            // 초기 UI 상태 설정
            UpdateAllUI();

            // 전투 시작
            StartCombat();
        }

        private void Update()
        {
            // 매 프레임 업데이트
            UpdatePileCounters();
            UpdateTurnIndicator();

            // 카드 사용 가능 여부 업데이트
            if (handLayoutManager != null)
            {
                handLayoutManager.UpdateAllCardPlayability();
            }
        }

        /// <summary>
        /// 버튼 이벤트 설정
        /// </summary>
        private void SetupButtons()
        {
            if (endTurnButton != null)
            {
                endTurnButton.onClick.AddListener(OnEndTurnClicked);
            }

            if (drawPileButton != null)
            {
                drawPileButton.onClick.AddListener(OnDrawPileClicked);
            }

            if (discardPileButton != null)
            {
                discardPileButton.onClick.AddListener(OnDiscardPileClicked);
            }
        }

        /// <summary>
        /// 전투 시작
        /// </summary>
        public void StartCombat()
        {
            currentTurn = 1;
            isPlayerTurn = true;

            // 적 UI 생성
            CreateEnemyUI();
            
            // 보스 UI 초기화
            var boss = FindObjectOfType<Combat.Boss>();
            if (boss != null && bossUI != null)
            {
                bossUI.Initialize(boss);
            }
            else if (bossUI != null)
            {
                bossUI.gameObject.SetActive(false);
            }

            // 플레이어 턴 시작
            StartPlayerTurn();
        }

        /// <summary>
        /// 플레이어 턴 시작
        /// </summary>
        public void StartPlayerTurn()
        {
            isPlayerTurn = true;
            currentTurn++;

            Debug.Log($"Player Turn {currentTurn} started");

            // CombatManager에 플레이어 턴 시작 알림
            if (combatManager != null)
            {
                // combatManager.StartPlayerTurn();
            }

            // 카드 드로우
            DrawCards(5); // Constants.STARTING_DRAW

            // 턴 종료 버튼 활성화
            SetEndTurnButtonEnabled(true);

            UpdateAllUI();
        }

        /// <summary>
        /// 플레이어 턴 종료
        /// </summary>
        public void EndPlayerTurn()
        {
            if (!isPlayerTurn) return;

            isPlayerTurn = false;

            Debug.Log("Player turn ended");

            // 손패 버리기
            DiscardHand();

            // 턴 종료 버튼 비활성화
            SetEndTurnButtonEnabled(false);

            // CombatManager에 플레이어 턴 종료 알림
            if (combatManager != null)
            {
                // combatManager.EndPlayerTurn();
            }

            // 적 턴 시작
            StartEnemyTurn();
        }

        /// <summary>
        /// 적 턴 시작
        /// </summary>
        private void StartEnemyTurn()
        {
            Debug.Log("Enemy turn started");

            // CombatManager에 적 턴 시작 알림
            if (combatManager != null)
            {
                // combatManager.StartEnemyTurn();
            }

            // 적 행동 실행 (코루틴으로 딜레이 추가)
            StartCoroutine(ExecuteEnemyTurnsCoroutine());
        }

        /// <summary>
        /// 적 턴 실행 (순차적으로)
        /// </summary>
        private System.Collections.IEnumerator ExecuteEnemyTurnsCoroutine()
        {
            yield return new WaitForSeconds(0.5f);

            var enemies = FindObjectsOfType<Combat.Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentHealth > 0)
                {
                    // 적 행동 실행
                    ExecuteEnemyIntent(enemy);

                    yield return new WaitForSeconds(1f);
                }
            }

            // 승리/패배 체크
            if (CheckVictory())
            {
                OnVictory();
                yield break;
            }

            if (CheckDefeat())
            {
                OnDefeat();
                yield break;
            }

            // 다음 플레이어 턴 시작
            StartPlayerTurn();
        }

        /// <summary>
        /// 적 의도 실행
        /// </summary>
        private void ExecuteEnemyIntent(Combat.Enemy enemy)
        {
            var intent = enemy.CurrentIntent;
            if (intent == null) return;

            switch (intent.Type)
            {
                case AI.IntentType.Attack:
                    // 플레이어에게 피해
                    player.TakeDamage(intent.Value);
                    ShowDamagePopup(player.transform.position, intent.Value, DamagePopup.PopupType.Damage);
                    break;

                case AI.IntentType.Defend:
                    // 방어도 획득
                    enemy.GainBlock(intent.Value);
                    ShowDamagePopup(enemy.transform.position, intent.Value, DamagePopup.PopupType.Block);
                    break;
            }

            // 다음 의도 결정
            enemy.DetermineIntent();

            // 적 UI 업데이트
            UpdateEnemyUI(enemy);
        }

        /// <summary>
        /// 카드 드로우
        /// </summary>
        private void DrawCards(int count)
        {
            if (deckManager == null || handLayoutManager == null) return;

            for (int i = 0; i < count; i++)
            {
                var card = deckManager.DrawCard();

                if (card != null)
                {
                    CardUI cardUI = handLayoutManager.AddCard(card);

                    if (cardUI != null)
                    {
                        handLayoutManager.PlayDrawAnimation(cardUI);
                    }
                }
            }
        }

        /// <summary>
        /// 손패 버리기
        /// </summary>
        private void DiscardHand()
        {
            if (deckManager == null || handLayoutManager == null) return;

            var cardsInHand = new List<Cards.Card>(deckManager.GetHand());

            foreach (var card in cardsInHand)
            {
                deckManager.DiscardCard(card);
            }

            handLayoutManager.ClearHand();
        }

        /// <summary>
        /// 카드 사용
        /// </summary>
        public void PlayCard(CardUI cardUI)
        {
            if (!isPlayerTurn || cardUI == null) return;

            var card = cardUI.CardData;

            // 내공 체크
            if (player.CurrentEnergy < card.Cost)
            {
                Debug.Log("Not enough energy to play this card!");
                handLayoutManager.ShakeHand();
                return;
            }

            // 내공 소모
            player.CurrentEnergy -= card.Cost;

            // 카드 효과 적용
            ApplyCardEffect(card);

            // 손패에서 제거
            handLayoutManager.RemoveCard(cardUI);

            // 덱 매니저에서 카드 사용 처리
            if (deckManager != null)
            {
                deckManager.PlayCard(card);
            }

            // 카드 사용 애니메이션
            cardUI.PlayAnimation();

            UpdateAllUI();
        }

        /// <summary>
        /// 카드 효과 적용
        /// </summary>
        private void ApplyCardEffect(Cards.Card card)
        {
            // 타겟 선택 (현재는 첫 번째 적)
            var enemy = FindObjectOfType<Combat.Enemy>();

            switch (card.Type)
            {
                case Data.CardType.Attack:
                    if (enemy != null)
                    {
                        int damage = card.BaseDamage;
                        enemy.TakeDamage(damage);
                        ShowDamagePopup(enemy.transform.position, damage, DamagePopup.PopupType.Damage);
                    }
                    break;

                case Data.CardType.Defense:
                    int block = card.BaseBlock;
                    player.GainBlock(block);
                    ShowDamagePopup(player.transform.position, block, DamagePopup.PopupType.Block);
                    break;

                case Data.CardType.Skill:
                    ApplySkillEffect(card, enemy);
                    break;
            }
        }

        /// <summary>
        /// 스킬 카드 효과 적용
        /// </summary>
        private void ApplySkillEffect(Cards.Card card, Combat.Enemy enemy)
        {
            // 카드 이름으로 효과 판별 (임시 구현)
            if (card.Name.Contains("드로우") || card.Name.Contains("Circulation"))
            {
                DrawCards(2);
            }
            else if (card.Name.Contains("회복") || card.Name.Contains("Flow"))
            {
                player.Heal(5);
                ShowDamagePopup(player.transform.position, 5, DamagePopup.PopupType.Heal);
            }
            else if (card.Name.Contains("전체") || card.Name.Contains("Release"))
            {
                var enemies = FindObjectsOfType<Combat.Enemy>();
                foreach (var e in enemies)
                {
                    e.TakeDamage(10);
                    ShowDamagePopup(e.transform.position, 10, DamagePopup.PopupType.Damage);
                }
            }
        }

        /// <summary>
        /// 피해 팝업 표시
        /// </summary>
        private void ShowDamagePopup(Vector3 worldPosition, int value, DamagePopup.PopupType type)
        {
            if (mainCanvas == null) return;

            // 월드 좌표를 스크린 좌표로 변환
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            DamagePopup.CreateDynamic(screenPosition, value, type, mainCanvas);
        }

        /// <summary>
        /// 적 UI 생성
        /// </summary>
        private void CreateEnemyUI()
        {
            var enemies = FindObjectsOfType<Combat.Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemyUIPrefab != null && enemyUIContainer != null)
                {
                    GameObject enemyUIObj = Instantiate(enemyUIPrefab, enemyUIContainer);
                    EnemyIntentUI enemyUI = enemyUIObj.GetComponent<EnemyIntentUI>();

                    if (enemyUI != null)
                    {
                        enemyUIList.Add(enemyUI);
                    }
                }
            }
        }

        /// <summary>
        /// 특정 적 UI 업데이트
        /// </summary>
        private void UpdateEnemyUI(Combat.Enemy enemy)
        {
            // 해당 적의 UI 찾아서 업데이트
            foreach (var enemyUI in enemyUIList)
            {
                enemyUI.UpdateIntentUI();
            }
        }

        /// <summary>
        /// 모든 UI 업데이트
        /// </summary>
        private void UpdateAllUI()
        {
            UpdatePileCounters();
            UpdateTurnIndicator();

            foreach (var enemyUI in enemyUIList)
            {
                enemyUI.UpdateIntentUI();
            }
        }

        /// <summary>
        /// 덱/버리기 더미 카운터 업데이트
        /// </summary>
        private void UpdatePileCounters()
        {
            if (deckManager == null) return;

            if (drawPileCountText != null)
            {
                drawPileCountText.text = deckManager.GetDrawPileCount().ToString();
            }

            if (discardPileCountText != null)
            {
                discardPileCountText.text = deckManager.GetDiscardPileCount().ToString();
            }

            if (exhaustPileCountText != null)
            {
                exhaustPileCountText.text = deckManager.GetExhaustPileCount().ToString();
            }
        }

        /// <summary>
        /// 턴 표시 업데이트
        /// </summary>
        private void UpdateTurnIndicator()
        {
            if (playerTurnIndicator != null)
            {
                playerTurnIndicator.SetActive(isPlayerTurn);
            }

            if (enemyTurnIndicator != null)
            {
                enemyTurnIndicator.SetActive(!isPlayerTurn);
            }

            if (turnCountText != null)
            {
                turnCountText.text = $"Turn {currentTurn}";
            }
        }

        /// <summary>
        /// 턴 종료 버튼 활성화/비활성화
        /// </summary>
        private void SetEndTurnButtonEnabled(bool enabled)
        {
            if (endTurnButton != null)
            {
                endTurnButton.interactable = enabled;

                if (endTurnButtonText != null)
                {
                    endTurnButtonText.color = enabled ? Color.white : Color.gray;
                }
            }
        }

        /// <summary>
        /// 승리 체크
        /// </summary>
        private bool CheckVictory()
        {
            var enemies = FindObjectsOfType<Combat.Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentHealth > 0)
                {
                    return false;
                }
            }

            return enemies.Length > 0;
        }

        /// <summary>
        /// 패배 체크
        /// </summary>
        private bool CheckDefeat()
        {
            return player != null && player.CurrentHealth <= 0;
        }

        /// <summary>
        /// 승리 처리
        /// </summary>
        private void OnVictory()
        {
            Debug.Log("Victory!");

            if (victoryDefeatUI != null)
            {
                // 보스인지 확인
                var boss = FindObjectOfType<Combat.Boss>();
                if (boss != null)
                {
                    // 보스 승리 연출
                    victoryDefeatUI.ShowBossVictory(boss.EnemyData.name, boss.DefeatDialogue, currentTurn, 0);
                }
                else
                {
                    // 일반 승리
                    victoryDefeatUI.ShowVictory(currentTurn, 0);
                }
            }
        }

        /// <summary>
        /// 패배 처리
        /// </summary>
        private void OnDefeat()
        {
            Debug.Log("Defeat!");

            if (victoryDefeatUI != null)
            {
                victoryDefeatUI.ShowDefeat(currentTurn);
            }
        }

        #region Button Callbacks

        private void OnEndTurnClicked()
        {
            EndPlayerTurn();
        }

        private void OnDrawPileClicked()
        {
            Debug.Log($"Draw Pile: {deckManager?.GetDrawPileCount() ?? 0} cards");
            // TODO: 덱 내용 표시 팝업
        }

        private void OnDiscardPileClicked()
        {
            Debug.Log($"Discard Pile: {deckManager?.GetDiscardPileCount() ?? 0} cards");
            // TODO: 버리기 더미 내용 표시 팝업
        }

        #endregion
    }
}
