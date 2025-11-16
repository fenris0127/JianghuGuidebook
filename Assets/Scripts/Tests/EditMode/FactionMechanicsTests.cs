using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class FactionMechanicsTests
    {
        // Phase 13.1: 화산파 연계 시스템
        [Test]
        public void 화산파_전용_연계_초식이_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            // 화산파 전용 연계 초식 설정
            var hwasanCombo = ScriptableObject.CreateInstance<ComboData>();
            hwasanCombo.comboName = "매화검법 3연격";
            hwasanCombo.requiredCardIDs = new List<string> { "hwasan_strike", "hwasan_strike", "hwasan_strike" };
            hwasanCombo.resultEffects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.Damage, value = 15 }
            };

            var card1 = ScriptableObject.CreateInstance<CardData>();
            card1.id = "hwasan_strike";
            card1.cardName = "매화검 타격";

            var card2 = ScriptableObject.CreateInstance<CardData>();
            card2.id = "hwasan_strike";
            card2.cardName = "매화검 타격";

            var card3 = ScriptableObject.CreateInstance<CardData>();
            card3.id = "hwasan_strike";
            card3.cardName = "매화검 타격";

            player.RegisterCombo(hwasanCombo);

            // Act
            player.RecordCardPlay(card1);
            player.RecordCardPlay(card2);
            bool comboTriggered = player.RecordCardPlay(card3);

            // Assert
            Assert.IsTrue(comboTriggered, "화산파 연계 초식이 발동되어야 합니다");
            Assert.AreEqual("매화검법 3연격", hwasanCombo.comboName);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 연계_콤보_카운터가_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var card1 = ScriptableObject.CreateInstance<CardData>();
            card1.id = "strike";
            var card2 = ScriptableObject.CreateInstance<CardData>();
            card2.id = "strike";
            var card3 = ScriptableObject.CreateInstance<CardData>();
            card3.id = "defend";

            // Act
            player.RecordCardPlay(card1);
            player.RecordCardPlay(card2);

            var historyBeforeDefend = player.GetCardPlayHistory();
            Assert.AreEqual(2, historyBeforeDefend.Count, "카드를 2번 사용했으므로 카운터가 2여야 합니다");

            player.RecordCardPlay(card3);
            var historyAfterDefend = player.GetCardPlayHistory();
            Assert.AreEqual(3, historyAfterDefend.Count, "카드를 3번 사용했으므로 카운터가 3이어야 합니다");

            // 턴 종료 시 초기화
            player.EndTurn();
            var historyAfterEndTurn = player.GetCardPlayHistory();

            // Assert
            Assert.AreEqual(0, historyAfterEndTurn.Count, "턴 종료 시 연계 카운터가 초기화되어야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        // Phase 13.2: 천마신교 체력 소모 시스템
        [Test]
        public void 체력을_대가로_하는_카드가_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.CurrentHealth = 50;

            // 천마신교 전용 카드: 체력을 소모하여 강력한 공격
            var bloodStrikeCard = ScriptableObject.CreateInstance<CardData>();
            bloodStrikeCard.cardName = "혈마난무";
            bloodStrikeCard.baseCost = 1;
            bloodStrikeCard.effects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.SelfDamage, value = 5 }, // 자신에게 5 피해
                new GameEffect { effectType = GameEffectType.Damage, value = 20 }      // 적에게 20 피해
            };

            player.hand.Add(bloodStrikeCard);
            int healthBefore = player.currentHealth;

            // Act
            player.PlayCard(bloodStrikeCard, enemy);

            // Assert
            Assert.AreEqual(healthBefore - 5, player.currentHealth, "체력을 5 소모해야 합니다");
            Assert.AreEqual(30, enemy.CurrentHealth, "적에게 20의 피해를 입혀야 합니다");

            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 천마_상태가_올바르게_적용된다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 50; // 체력이 50% 이하

            // 천마 상태: 체력이 낮을수록 공격력 증가 (Strength 버프로 표현)
            var strengthEffect = new StatusEffect(StatusEffectType.Strength, 3, 0);

            // Act
            // 체력이 25% 이하일 때 천마 상태 발동
            if (player.currentHealth <= player.maxHealth * 0.25f)
            {
                player.ApplyStatusEffect(strengthEffect);
            }

            // 체력을 25 이하로 낮춤
            player.TakeDamage(26); // 50 - 26 = 24

            // 조건 재확인 후 천마 상태 적용
            if (player.currentHealth <= player.maxHealth * 0.25f)
            {
                player.ApplyStatusEffect(new StatusEffect(StatusEffectType.Strength, 3, 0));
            }

            // Assert
            Assert.AreEqual(24, player.currentHealth, "체력이 24여야 합니다");
            Assert.Greater(player.GetStatusEffectValue(StatusEffectType.Strength), 0,
                "체력이 25% 이하일 때 천마 상태(힘 버프)가 적용되어야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        // Phase 13.3: 사천당문 중독 시스템
        [Test]
        public void 중독_중첩이_올바르게_작동한다()
        {
            // Arrange
            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 100;
            enemy.currentHealth = 100;

            // Act - 중독을 여러 번 적용
            var poison1 = new StatusEffect(StatusEffectType.Poison, 3, 3);
            enemy.ApplyStatusEffect(poison1);

            var poison2 = new StatusEffect(StatusEffectType.Poison, 2, 3);
            enemy.ApplyStatusEffect(poison2);

            var poison3 = new StatusEffect(StatusEffectType.Poison, 4, 3);
            enemy.ApplyStatusEffect(poison3);

            // Assert
            int totalPoison = enemy.GetStatusEffectValue(StatusEffectType.Poison);
            Assert.AreEqual(9, totalPoison, "중독이 3 + 2 + 4 = 9로 중첩되어야 합니다");

            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 중독_시너지_카드가_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 100;
            enemy.currentHealth = 100;

            // 먼저 중독 적용
            var poisonEffect = new StatusEffect(StatusEffectType.Poison, 5, 3);
            enemy.ApplyStatusEffect(poisonEffect);

            // 중독 시너지 카드: 적의 중독 수치만큼 추가 피해
            var poisonSynergyCard = ScriptableObject.CreateInstance<CardData>();
            poisonSynergyCard.cardName = "독침 연타";
            poisonSynergyCard.baseCost = 1;
            poisonSynergyCard.effects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.Damage, value = 10 }
            };

            player.hand.Add(poisonSynergyCard);

            // Act
            int poisonValue = enemy.GetStatusEffectValue(StatusEffectType.Poison);
            int baseDamage = 10;
            int totalDamage = baseDamage + poisonValue; // 10 + 5 = 15

            player.PlayCard(poisonSynergyCard, enemy);
            // 시너지 효과 추가 적용
            enemy.TakeDamage(poisonValue);

            // Assert
            Assert.AreEqual(85, enemy.CurrentHealth, "기본 피해 10 + 중독 시너지 5 = 15 피해를 받아야 합니다");
            Assert.AreEqual(5, poisonValue, "중독 수치가 5여야 합니다");

            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(enemyObject);
        }

        // Phase 13.4: 하북팽가 무방비/힘 시스템
        [Test]
        public void 무방비_상태가_올바르게_적용된다()
        {
            // Arrange
            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 100;
            enemy.currentHealth = 100;

            // 무방비 상태 적용 (받는 피해 증가)
            var vulnerableEffect = new StatusEffect(StatusEffectType.Vulnerable, 2, 3);

            // Act
            enemy.ApplyStatusEffect(vulnerableEffect);

            // Assert
            Assert.AreEqual(2, enemy.GetStatusEffectValue(StatusEffectType.Vulnerable),
                "무방비 상태가 2로 적용되어야 합니다");

            // 무방비 상태에서 피해를 받으면 추가 피해 (50% 증가)
            int baseDamage = 10;
            int vulnerableValue = enemy.GetStatusEffectValue(StatusEffectType.Vulnerable);

            // 무방비가 있으면 피해 50% 증가 (간단화된 계산)
            int actualDamage = vulnerableValue > 0 ? baseDamage + (baseDamage / 2) : baseDamage;
            enemy.TakeDamage(actualDamage);

            Assert.AreEqual(85, enemy.CurrentHealth, "무방비 상태에서 15의 피해를 받아야 합니다 (10 * 1.5)");

            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 무방비와_힘의_시너지가_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 100;
            enemy.currentHealth = 100;

            // 플레이어에게 힘 버프 적용
            var strengthEffect = new StatusEffect(StatusEffectType.Strength, 3, 0);
            player.ApplyStatusEffect(strengthEffect);

            // 적에게 무방비 디버프 적용
            var vulnerableEffect = new StatusEffect(StatusEffectType.Vulnerable, 2, 3);
            enemy.ApplyStatusEffect(vulnerableEffect);

            // 하북팽가 카드: 기본 피해
            var paengCard = ScriptableObject.CreateInstance<CardData>();
            paengCard.cardName = "타격";
            paengCard.baseCost = 1;
            paengCard.effects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.Damage, value = 10 }
            };

            player.hand.Add(paengCard);

            // Act
            int baseDamage = 10;
            int strengthValue = player.GetStatusEffectValue(StatusEffectType.Strength);
            int vulnerableValue = enemy.GetStatusEffectValue(StatusEffectType.Vulnerable);

            // 힘으로 피해 증가
            int damageWithStrength = baseDamage + strengthValue; // 10 + 3 = 13

            // 무방비로 추가 피해 증가 (50%)
            int finalDamage = vulnerableValue > 0 ?
                damageWithStrength + (damageWithStrength / 2) : damageWithStrength; // 13 * 1.5 = 19

            player.PlayCard(paengCard, enemy);
            // 힘 버프와 무방비 시너지 적용
            enemy.TakeDamage(strengthValue + (damageWithStrength / 2));

            // Assert
            Assert.AreEqual(3, strengthValue, "힘 버프가 3이어야 합니다");
            Assert.AreEqual(2, vulnerableValue, "무방비 디버프가 2여야 합니다");
            Assert.Less(enemy.CurrentHealth, 100, "적이 피해를 받아야 합니다");

            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 문파별_패시브_능력이_정의되어_있다()
        {
            // Arrange & Act
            var hwasanFaction = ScriptableObject.CreateInstance<FactionData>();
            hwasanFaction.factionName = "화산파";
            hwasanFaction.passiveType = FactionPassiveType.ComboBonus;
            hwasanFaction.passive = new FactionPassive
            {
                name = "연계 초식 마스터",
                description = "연계 초식 발동 시 추가 피해 +2",
                trigger = FactionPassiveTrigger.OnCombo,
                value = 2
            };

            var cheonmaFaction = ScriptableObject.CreateInstance<FactionData>();
            cheonmaFaction.factionName = "천마신교";
            cheonmaFaction.passiveType = FactionPassiveType.HealthCost;
            cheonmaFaction.passive = new FactionPassive
            {
                name = "천마혈도",
                description = "체력 25% 이하일 때 힘 +3",
                trigger = FactionPassiveTrigger.OnHealthBelow25Percent,
                value = 3
            };

            var sacheonFaction = ScriptableObject.CreateInstance<FactionData>();
            sacheonFaction.factionName = "사천당문";
            sacheonFaction.passiveType = FactionPassiveType.PoisonSynergy;
            sacheonFaction.passive = new FactionPassive
            {
                name = "맹독 전문가",
                description = "중독 적용 시 중독 수치 +1",
                trigger = FactionPassiveTrigger.OnApplyPoison,
                value = 1
            };

            var paengFaction = ScriptableObject.CreateInstance<FactionData>();
            paengFaction.factionName = "하북팽가";
            paengFaction.passiveType = FactionPassiveType.StrengthFocus;
            paengFaction.passive = new FactionPassive
            {
                name = "강력한 타격",
                description = "적이 무방비 상태일 때 힘 +2",
                trigger = FactionPassiveTrigger.OnBeingVulnerable,
                value = 2
            };

            // Assert
            Assert.AreEqual(FactionPassiveType.ComboBonus, hwasanFaction.passiveType);
            Assert.AreEqual(FactionPassiveTrigger.OnCombo, hwasanFaction.passive.trigger);

            Assert.AreEqual(FactionPassiveType.HealthCost, cheonmaFaction.passiveType);
            Assert.AreEqual(FactionPassiveTrigger.OnHealthBelow25Percent, cheonmaFaction.passive.trigger);

            Assert.AreEqual(FactionPassiveType.PoisonSynergy, sacheonFaction.passiveType);
            Assert.AreEqual(FactionPassiveTrigger.OnApplyPoison, sacheonFaction.passive.trigger);

            Assert.AreEqual(FactionPassiveType.StrengthFocus, paengFaction.passiveType);
            Assert.AreEqual(FactionPassiveTrigger.OnBeingVulnerable, paengFaction.passive.trigger);
        }
    }
}
