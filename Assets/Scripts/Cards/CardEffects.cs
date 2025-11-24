using UnityEngine;
using JianghuGuidebook.Combat;
using JianghuGuidebook.Core;
using System.Collections.Generic;

namespace JianghuGuidebook.Cards
{
    /// <summary>
    /// 카드 효과를 처리하는 정적 클래스
    /// </summary>
    public static class CardEffects
    {
        /// <summary>
        /// 적에게 피해를 입힙니다
        /// </summary>
        public static void DealDamage(int amount, Enemy target)
        {
            if (target == null)
            {
                Debug.LogError("공격 대상이 없습니다");
                return;
            }

            if (amount <= 0)
            {
                Debug.LogWarning("피해량이 0 이하입니다");
                return;
            }

            Debug.Log($"카드 효과: {target.EnemyData.name}에게 {amount} 피해");
            target.TakeDamage(amount);
        }

        /// <summary>
        /// 모든 적에게 피해를 입힙니다
        /// </summary>
        public static void DealDamageToAll(int amount)
        {
            var enemies = CombatManager.Instance.Enemies;

            foreach (var enemy in enemies)
            {
                if (enemy != null && enemy.IsAlive())
                {
                    DealDamage(amount, enemy);
                }
            }
        }

        /// <summary>
        /// 플레이어가 방어도를 획득합니다
        /// </summary>
        public static void GainBlock(int amount, Player player)
        {
            if (player == null)
            {
                Debug.LogError("플레이어가 없습니다");
                return;
            }

            if (amount <= 0)
            {
                Debug.LogWarning("방어도가 0 이하입니다");
                return;
            }

            Debug.Log($"카드 효과: 방어도 {amount} 획득");
            player.GainBlock(amount);
        }

        /// <summary>
        /// 카드를 뽑습니다
        /// </summary>
        public static void DrawCards(int count)
        {
            if (count <= 0)
            {
                Debug.LogWarning("드로우 수가 0 이하입니다");
                return;
            }

            DeckManager deckManager = Object.FindObjectOfType<DeckManager>();
            if (deckManager == null)
            {
                Debug.LogError("DeckManager를 찾을 수 없습니다");
                return;
            }

            Debug.Log($"카드 효과: 카드 {count}장 드로우");
            deckManager.DrawCards(count);
        }

        /// <summary>
        /// 체력을 회복합니다
        /// </summary>
        public static void Heal(int amount, Player player)
        {
            if (player == null)
            {
                Debug.LogError("플레이어가 없습니다");
                return;
            }

            if (amount <= 0)
            {
                Debug.LogWarning("회복량이 0 이하입니다");
                return;
            }

            Debug.Log($"카드 효과: 체력 {amount} 회복");
            player.Heal(amount);
        }

        /// <summary>
        /// 내공을 획득합니다
        /// </summary>
        public static void GainEnergy(int amount, Player player)
        {
            if (player == null)
            {
                Debug.LogError("플레이어가 없습니다");
                return;
            }

            if (amount <= 0)
            {
                Debug.LogWarning("내공이 0 이하입니다");
                return;
            }

            Debug.Log($"카드 효과: 내공 {amount} 획득");
            player.GainEnergy(amount);
        }

        /// <summary>
        /// 카드 효과를 실행합니다
        /// </summary>
        public static void ExecuteCardEffects(Card card, Player player, Enemy target = null)
        {
            if (card == null)
            {
                Debug.LogError("카드가 null입니다");
                return;
            }

            Debug.Log($"=== 카드 효과 실행: {card.Name} ===");

            // 공격 카드
            if (card.Type == CardType.Attack && card.Damage > 0)
            {
                if (target != null)
                {
                    // 여러 번 공격
                    for (int i = 0; i < card.TimesToPlay; i++)
                    {
                        DealDamage(card.Damage, target);
                    }
                }
                else
                {
                    Debug.LogWarning("공격 대상이 지정되지 않았습니다");
                }
            }

            // 방어 카드
            if (card.Type == CardType.Defense && card.Block > 0)
            {
                GainBlock(card.Block, player);
            }

            // 드로우 효과
            if (card.DrawCards > 0)
            {
                DrawCards(card.DrawCards);
            }

            // 특수 효과 (카드 ID에 따라 처리)
            switch (card.Id)
            {
                case "card_sword_aura_release":
                    // 검기방출: 모든 적에게 피해
                    DealDamageToAll(card.Damage);
                    break;

                case "card_energy_flow":
                    // 기혈순환: 체력 회복
                    Heal(5, player);
                    break;

                case "card_sword_heart_unity":
                    // 심검합일: 다음 공격 +3 (TODO: 버프 시스템 구현 필요)
                    Debug.Log("심검합일 효과: 다음 공격 +3 (버프 시스템 미구현)");
                    break;

                default:
                    // 기본 카드는 위에서 처리됨
                    break;
            }

            Debug.Log($"=== 카드 효과 실행 완료 ===");
        }
    }
}
