using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class ComboSystemTests
    {
        [Test]
        public void 연계_초식의_조건을_정의할_수_있다()
        {
            // Arrange
            var comboData = ScriptableObject.CreateInstance<ComboData>();

            // Act
            comboData.comboName = "매화검법 3연격";
            comboData.requiredCardIDs = new List<string> { "strike", "strike", "strike" };
            comboData.resultEffects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.Damage, value = 5 }
            };

            // Assert
            Assert.AreEqual("매화검법 3연격", comboData.comboName);
            Assert.AreEqual(3, comboData.requiredCardIDs.Count);
            Assert.AreEqual("strike", comboData.requiredCardIDs[0]);
            Assert.AreEqual(1, comboData.resultEffects.Count);
            Assert.AreEqual(GameEffectType.Damage, comboData.resultEffects[0].effectType);
        }

        [Test]
        public void 카드_사용_순서를_추적할_수_있다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var card1 = ScriptableObject.CreateInstance<CardData>();
            card1.id = "strike";
            var card2 = ScriptableObject.CreateInstance<CardData>();
            card2.id = "defend";
            var card3 = ScriptableObject.CreateInstance<CardData>();
            card3.id = "strike";

            // Act
            player.RecordCardPlay(card1);
            player.RecordCardPlay(card2);
            player.RecordCardPlay(card3);

            // Assert
            var history = player.GetCardPlayHistory();
            Assert.AreEqual(3, history.Count);
            Assert.AreEqual("strike", history[0].id);
            Assert.AreEqual("defend", history[1].id);
            Assert.AreEqual("strike", history[2].id);

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 특정_순서로_카드를_사용하면_연계가_발동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var comboData = ScriptableObject.CreateInstance<ComboData>();
            comboData.comboName = "검기 연타";
            comboData.requiredCardIDs = new List<string> { "strike", "strike", "strike" };
            comboData.resultEffects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.Damage, value = 10 }
            };

            var strike1 = ScriptableObject.CreateInstance<CardData>();
            strike1.id = "strike";
            var strike2 = ScriptableObject.CreateInstance<CardData>();
            strike2.id = "strike";
            var strike3 = ScriptableObject.CreateInstance<CardData>();
            strike3.id = "strike";

            // 콤보 데이터를 플레이어에게 등록
            player.RegisterCombo(comboData);

            // Act
            player.RecordCardPlay(strike1);
            player.RecordCardPlay(strike2);
            bool comboTriggered = player.RecordCardPlay(strike3); // 3번째 strike에서 콤보 발동

            // Assert
            Assert.IsTrue(comboTriggered, "연계가 발동되어야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 연계_발동_시_추가_효과가_적용된다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.CurrentHealth = 100;

            var comboData = ScriptableObject.CreateInstance<ComboData>();
            comboData.comboName = "검기 연타";
            comboData.requiredCardIDs = new List<string> { "strike", "strike" };
            comboData.resultEffects = new List<GameEffect>
            {
                new GameEffect { effectType = GameEffectType.Damage, value = 10 }
            };

            var strike1 = ScriptableObject.CreateInstance<CardData>();
            strike1.id = "strike";
            var strike2 = ScriptableObject.CreateInstance<CardData>();
            strike2.id = "strike";

            player.RegisterCombo(comboData);

            // Act
            player.RecordCardPlay(strike1);
            bool comboTriggered = player.RecordCardPlay(strike2);

            if (comboTriggered)
            {
                // 연계 추가 효과 적용
                foreach (var effect in comboData.resultEffects)
                {
                    if (effect.effectType == GameEffectType.Damage)
                    {
                        enemy.TakeDamage(effect.value);
                    }
                }
            }

            // Assert
            Assert.IsTrue(comboTriggered, "연계가 발동되어야 합니다");
            Assert.AreEqual(90, enemy.CurrentHealth, "연계 추가 데미지가 적용되어야 합니다");

            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 잘못된_순서로_카드를_사용하면_연계가_발동하지_않는다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var comboData = ScriptableObject.CreateInstance<ComboData>();
            comboData.requiredCardIDs = new List<string> { "strike", "strike", "strike" };

            var strike = ScriptableObject.CreateInstance<CardData>();
            strike.id = "strike";
            var defend = ScriptableObject.CreateInstance<CardData>();
            defend.id = "defend";

            player.RegisterCombo(comboData);

            // Act
            player.RecordCardPlay(strike);
            player.RecordCardPlay(defend); // 연계가 끊김
            bool comboTriggered = player.RecordCardPlay(strike);

            // Assert
            Assert.IsFalse(comboTriggered, "잘못된 순서로 카드를 사용하면 연계가 발동하지 않아야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 턴_종료_시_연계_카운터가_초기화된다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var strike = ScriptableObject.CreateInstance<CardData>();
            strike.id = "strike";

            // Act
            player.RecordCardPlay(strike);
            player.RecordCardPlay(strike);

            var historyBeforeEndTurn = player.GetCardPlayHistory();
            Assert.AreEqual(2, historyBeforeEndTurn.Count, "턴 종료 전 히스토리에 2장의 카드가 있어야 합니다");

            player.EndTurn();

            var historyAfterEndTurn = player.GetCardPlayHistory();

            // Assert
            Assert.AreEqual(0, historyAfterEndTurn.Count, "턴 종료 후 카드 사용 히스토리가 초기화되어야 합니다");

            Object.DestroyImmediate(playerObject);
        }
    }
}
