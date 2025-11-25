using UnityEngine;
using System.Collections.Generic;
using System.Text;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Cards;
using JianghuGuidebook.Data;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 전투 밸런스를 테스트하고 통계를 수집하는 시뮬레이터
    /// </summary>
    public class BalanceTestSimulator : MonoBehaviour
    {
        [Header("Simulation Settings")]
        [SerializeField] private int simulationsPerEnemy = 100;
        [SerializeField] private int maxTurnsPerSimulation = 50;
        [SerializeField] private bool enableDetailedLogs = false;

        [Header("Player Configuration")]
        [SerializeField] private int playerStartingHealth = 70;
        [SerializeField] private int playerMaxEnergy = 3;
        [SerializeField] private int cardsDrawnPerTurn = 5;

        private DataManager dataManager;

        /// <summary>
        /// 시뮬레이션 결과 데이터
        /// </summary>
        public class SimulationResult
        {
            public string enemyId;
            public string enemyName;
            public int totalSimulations;
            public int playerWins;
            public int playerLosses;
            public float winRate;
            public int averageTurnsToWin;
            public int averageTurnsToLose;
            public int averagePlayerHealthRemaining;
            public int minPlayerHealthRemaining;
            public int maxPlayerHealthRemaining;
            public List<int> turnsToWinList = new List<int>();
            public List<int> turnsToLoseList = new List<int>();
            public List<int> healthRemainingList = new List<int>();
        }

        private void Start()
        {
            dataManager = FindObjectOfType<DataManager>();
            if (dataManager == null)
            {
                GameObject dataManagerObj = new GameObject("DataManager");
                dataManager = dataManagerObj.AddComponent<DataManager>();
            }
        }

        /// <summary>
        /// 모든 적에 대한 밸런스 테스트 실행
        /// </summary>
        [ContextMenu("Run Full Balance Test")]
        public void RunFullBalanceTest()
        {
            Debug.Log("=== Starting Full Balance Test ===");

            List<SimulationResult> results = new List<SimulationResult>();

            // 모든 적 타입 테스트
            string[] enemyIds = { "enemy_bandit", "enemy_dark_cultivator", "enemy_armored_guard" };

            foreach (string enemyId in enemyIds)
            {
                SimulationResult result = SimulateEnemyEncounter(enemyId, simulationsPerEnemy);
                results.Add(result);
            }

            // 결과 출력
            PrintBalanceTestResults(results);
        }

        /// <summary>
        /// 특정 적과의 전투 시뮬레이션
        /// </summary>
        public SimulationResult SimulateEnemyEncounter(string enemyId, int simulations)
        {
            EnemyData enemyData = dataManager.GetEnemyData(enemyId);

            if (enemyData == null)
            {
                Debug.LogError($"Enemy data not found: {enemyId}");
                return null;
            }

            Debug.Log($"Simulating {simulations} encounters with {enemyData.name}...");

            SimulationResult result = new SimulationResult
            {
                enemyId = enemyId,
                enemyName = enemyData.name,
                totalSimulations = simulations
            };

            for (int i = 0; i < simulations; i++)
            {
                bool playerWon = SimulateSingleCombat(enemyData, out int turns, out int playerHealthRemaining);

                if (playerWon)
                {
                    result.playerWins++;
                    result.turnsToWinList.Add(turns);
                    result.healthRemainingList.Add(playerHealthRemaining);
                }
                else
                {
                    result.playerLosses++;
                    result.turnsToLoseList.Add(turns);
                }
            }

            // 통계 계산
            CalculateStatistics(result);

            return result;
        }

        /// <summary>
        /// 단일 전투 시뮬레이션
        /// </summary>
        private bool SimulateSingleCombat(EnemyData enemyData, out int turns, out int playerHealthRemaining)
        {
            // 시뮬레이션 플레이어 생성
            SimulatedPlayer player = new SimulatedPlayer(playerStartingHealth, playerMaxEnergy);

            // 시뮬레이션 적 생성
            SimulatedEnemy enemy = new SimulatedEnemy(enemyData);

            // 시뮬레이션 덱 생성 (시작 덱: 공격 7장, 방어 7장, 스킬 6장 중 기본 카드들)
            List<SimulatedCard> deck = CreateStarterDeck();

            turns = 0;
            int maxTurns = maxTurnsPerSimulation;

            while (turns < maxTurns)
            {
                turns++;

                // 플레이어 턴
                player.StartTurn();

                // 카드 드로우 (간단한 시뮬레이션)
                List<SimulatedCard> hand = DrawCards(deck, cardsDrawnPerTurn);

                // AI로 최선의 플레이 선택 (간단한 로직)
                PlayOptimalCards(player, enemy, hand);

                // 플레이어 턴 종료
                player.EndTurn();

                // 적이 죽었는지 확인
                if (enemy.CurrentHealth <= 0)
                {
                    playerHealthRemaining = player.CurrentHealth;
                    return true; // 플레이어 승리
                }

                // 적 턴
                enemy.ExecuteTurn(player);

                // 플레이어가 죽었는지 확인
                if (player.CurrentHealth <= 0)
                {
                    playerHealthRemaining = 0;
                    return false; // 플레이어 패배
                }
            }

            // 최대 턴 도달 시 패배로 간주
            playerHealthRemaining = player.CurrentHealth;
            return false;
        }

        /// <summary>
        /// 시작 덱 생성
        /// </summary>
        private List<SimulatedCard> CreateStarterDeck()
        {
            List<SimulatedCard> deck = new List<SimulatedCard>();

            // 기본 공격 카드 (일검) x 5
            for (int i = 0; i < 5; i++)
            {
                deck.Add(new SimulatedCard("일검", 1, CardType.Attack, 6, 0));
            }

            // 기본 방어 카드 (철포삼) x 4
            for (int i = 0; i < 4; i++)
            {
                deck.Add(new SimulatedCard("철포삼", 1, CardType.Defense, 0, 5));
            }

            // 기본 스킬 카드 (내공운기) x 1
            deck.Add(new SimulatedCard("내공운기", 1, CardType.Skill, 0, 0));

            return deck;
        }

        /// <summary>
        /// 카드 드로우 (셔플 포함)
        /// </summary>
        private List<SimulatedCard> DrawCards(List<SimulatedCard> deck, int count)
        {
            // 간단한 랜덤 드로우
            List<SimulatedCard> hand = new List<SimulatedCard>();

            for (int i = 0; i < count && i < deck.Count; i++)
            {
                int randomIndex = Random.Range(0, deck.Count);
                hand.Add(deck[randomIndex]);
            }

            return hand;
        }

        /// <summary>
        /// 최적의 카드 플레이 (간단한 AI)
        /// </summary>
        private void PlayOptimalCards(SimulatedPlayer player, SimulatedEnemy enemy, List<SimulatedCard> hand)
        {
            // 우선순위: 공격 > 방어 > 스킬
            // 내공이 허락하는 한 카드 사용

            // 1. 공격 카드 먼저 사용
            foreach (var card in hand)
            {
                if (player.CurrentEnergy >= card.Cost && card.Type == CardType.Attack)
                {
                    player.CurrentEnergy -= card.Cost;
                    enemy.TakeDamage(card.Damage);
                }
            }

            // 2. 방어 카드 사용
            foreach (var card in hand)
            {
                if (player.CurrentEnergy >= card.Cost && card.Type == CardType.Defense)
                {
                    player.CurrentEnergy -= card.Cost;
                    player.GainBlock(card.Block);
                }
            }
        }

        /// <summary>
        /// 통계 계산
        /// </summary>
        private void CalculateStatistics(SimulationResult result)
        {
            result.winRate = (float)result.playerWins / result.totalSimulations * 100f;

            if (result.turnsToWinList.Count > 0)
            {
                int totalTurns = 0;
                foreach (int turns in result.turnsToWinList)
                {
                    totalTurns += turns;
                }
                result.averageTurnsToWin = totalTurns / result.turnsToWinList.Count;
            }

            if (result.turnsToLoseList.Count > 0)
            {
                int totalTurns = 0;
                foreach (int turns in result.turnsToLoseList)
                {
                    totalTurns += turns;
                }
                result.averageTurnsToLose = totalTurns / result.turnsToLoseList.Count;
            }

            if (result.healthRemainingList.Count > 0)
            {
                int totalHealth = 0;
                result.minPlayerHealthRemaining = int.MaxValue;
                result.maxPlayerHealthRemaining = int.MinValue;

                foreach (int health in result.healthRemainingList)
                {
                    totalHealth += health;
                    if (health < result.minPlayerHealthRemaining)
                        result.minPlayerHealthRemaining = health;
                    if (health > result.maxPlayerHealthRemaining)
                        result.maxPlayerHealthRemaining = health;
                }

                result.averagePlayerHealthRemaining = totalHealth / result.healthRemainingList.Count;
            }
        }

        /// <summary>
        /// 밸런스 테스트 결과 출력
        /// </summary>
        private void PrintBalanceTestResults(List<SimulationResult> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n=== BALANCE TEST RESULTS ===\n");

            foreach (var result in results)
            {
                sb.AppendLine($"--- {result.enemyName} ({result.enemyId}) ---");
                sb.AppendLine($"Total Simulations: {result.totalSimulations}");
                sb.AppendLine($"Player Wins: {result.playerWins} ({result.winRate:F1}%)");
                sb.AppendLine($"Player Losses: {result.playerLosses} ({100 - result.winRate:F1}%)");

                if (result.playerWins > 0)
                {
                    sb.AppendLine($"Average Turns to Win: {result.averageTurnsToWin}");
                    sb.AppendLine($"Average Health Remaining: {result.averagePlayerHealthRemaining} HP");
                    sb.AppendLine($"Health Range: {result.minPlayerHealthRemaining} - {result.maxPlayerHealthRemaining} HP");
                }

                if (result.playerLosses > 0)
                {
                    sb.AppendLine($"Average Turns to Lose: {result.averageTurnsToLose}");
                }

                // 밸런스 평가
                string balanceAssessment = AssessBalance(result);
                sb.AppendLine($"Balance Assessment: {balanceAssessment}");

                sb.AppendLine();
            }

            sb.AppendLine("=========================\n");

            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 밸런스 평가
        /// </summary>
        private string AssessBalance(SimulationResult result)
        {
            if (result.winRate >= 70f)
                return "TOO EASY - Enemy needs buffing or better cards needed";
            else if (result.winRate >= 50f && result.winRate < 70f)
                return "WELL BALANCED - Good difficulty";
            else if (result.winRate >= 30f && result.winRate < 50f)
                return "CHALLENGING - Slightly difficult but fair";
            else
                return "TOO HARD - Enemy needs nerfing or player needs stronger cards";
        }

        #region Simulated Classes

        private class SimulatedPlayer
        {
            public int CurrentHealth;
            public int MaxHealth;
            public int CurrentEnergy;
            public int MaxEnergy;
            public int Block;

            public SimulatedPlayer(int health, int energy)
            {
                MaxHealth = health;
                CurrentHealth = health;
                MaxEnergy = energy;
                CurrentEnergy = energy;
                Block = 0;
            }

            public void StartTurn()
            {
                CurrentEnergy = MaxEnergy;
                Block = 0;
            }

            public void EndTurn()
            {
                // 턴 종료 처리
            }

            public void TakeDamage(int damage)
            {
                int remainingDamage = damage - Block;
                Block = Mathf.Max(0, Block - damage);

                if (remainingDamage > 0)
                {
                    CurrentHealth -= remainingDamage;
                }
            }

            public void GainBlock(int amount)
            {
                Block += amount;
            }
        }

        private class SimulatedEnemy
        {
            public int CurrentHealth;
            public int MaxHealth;
            public int Block;
            private EnemyData data;

            public SimulatedEnemy(EnemyData enemyData)
            {
                data = enemyData;
                MaxHealth = enemyData.maxHealth;
                CurrentHealth = enemyData.maxHealth;
                Block = 0;
            }

            public void TakeDamage(int damage)
            {
                int remainingDamage = damage - Block;
                Block = Mathf.Max(0, Block - damage);

                if (remainingDamage > 0)
                {
                    CurrentHealth -= remainingDamage;
                }
            }

            public void ExecuteTurn(SimulatedPlayer player)
            {
                Block = 0;

                // 가중치 기반 행동 선택
                var action = SelectRandomAction();

                if (action.type == 0) // Attack
                {
                    player.TakeDamage(action.value);
                }
                else if (action.type == 1) // Defend
                {
                    Block += action.value;
                }
            }

            private EnemyData.EnemyAction SelectRandomAction()
            {
                int totalWeight = 0;
                foreach (var action in data.actions)
                {
                    totalWeight += action.weight;
                }

                int randomValue = Random.Range(0, totalWeight);
                int currentWeight = 0;

                foreach (var action in data.actions)
                {
                    currentWeight += action.weight;
                    if (randomValue < currentWeight)
                    {
                        return action;
                    }
                }

                return data.actions[0];
            }
        }

        private class SimulatedCard
        {
            public string Name;
            public int Cost;
            public CardType Type;
            public int Damage;
            public int Block;

            public SimulatedCard(string name, int cost, CardType type, int damage, int block)
            {
                Name = name;
                Cost = cost;
                Type = type;
                Damage = damage;
                Block = block;
            }
        }

        #endregion
    }
}
