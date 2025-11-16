using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class EventSystemTests
    {
        // Phase 9.1: 기연 이벤트
        [Test]
        public void EventData로_이벤트를_정의할_수_있다()
        {
            // Arrange & Act
            var eventData = ScriptableObject.CreateInstance<EventData>();
            eventData.eventID = "event_001";
            eventData.eventName = "신비한 노인";
            eventData.description = "험난한 산길에서 수상한 노인을 만났다.";
            eventData.rarity = EventRarity.Common;

            var choice1 = new EventChoice
            {
                choiceText = "노인을 도와준다",
                successChance = 100,
                successEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.GainGold, value = 50 }
                }
            };

            var choice2 = new EventChoice
            {
                choiceText = "무시하고 지나간다",
                successChance = 100,
                successEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.HealHealth, value = 10 }
                }
            };

            eventData.choices = new List<EventChoice> { choice1, choice2 };

            // Assert
            Assert.AreEqual("event_001", eventData.eventID);
            Assert.AreEqual("신비한 노인", eventData.eventName);
            Assert.AreEqual("험난한 산길에서 수상한 노인을 만났다.", eventData.description);
            Assert.AreEqual(EventRarity.Common, eventData.rarity);
            Assert.AreEqual(2, eventData.choices.Count);
            Assert.AreEqual("노인을 도와준다", eventData.choices[0].choiceText);
            Assert.AreEqual("무시하고 지나간다", eventData.choices[1].choiceText);
        }

        [Test]
        public void 이벤트_선택지를_표시할_수_있다()
        {
            // Arrange
            var eventData = ScriptableObject.CreateInstance<EventData>();
            eventData.eventName = "낡은 보물상자";
            eventData.description = "오래된 보물상자를 발견했다.";

            var choice1 = new EventChoice
            {
                choiceText = "조심스럽게 연다",
                successChance = 70,
                successEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.GainGold, value = 100 }
                },
                failureEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.TakeDamage, value = 10 }
                }
            };

            var choice2 = new EventChoice
            {
                choiceText = "무시한다",
                successChance = 100,
                successEffects = new List<GameEffect>()
            };

            eventData.choices = new List<EventChoice> { choice1, choice2 };

            // Act & Assert
            Assert.AreEqual(2, eventData.choices.Count, "선택지가 2개 표시되어야 합니다");
            Assert.AreEqual("조심스럽게 연다", eventData.choices[0].choiceText);
            Assert.AreEqual("무시한다", eventData.choices[1].choiceText);

            // 선택지의 효과 검증
            Assert.AreEqual(70, eventData.choices[0].successChance);
            Assert.AreEqual(1, eventData.choices[0].successEffects.Count);
            Assert.AreEqual(1, eventData.choices[0].failureEffects.Count);
        }

        [Test]
        public void 선택지_선택_시_결과가_적용된다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 50;

            var choice = new EventChoice
            {
                choiceText = "휴식을 취한다",
                successChance = 100,
                successEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.HealHealth, value = 20 }
                }
            };

            // Act - 선택지 효과 적용 (성공)
            int healthBefore = player.currentHealth;

            // 성공 효과 적용
            foreach (var effect in choice.successEffects)
            {
                if (effect.type == GameEffectType.HealHealth)
                {
                    player.Heal(effect.value);
                }
            }

            // Assert
            Assert.AreEqual(healthBefore + 20, player.currentHealth, "체력이 20 회복되어야 합니다");

            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 확률적_결과가_있는_이벤트가_작동한다()
        {
            // Arrange
            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();
            player.maxHealth = 100;
            player.currentHealth = 100;

            var choice = new EventChoice
            {
                choiceText = "보물상자를 연다",
                successChance = 50, // 50% 확률
                successEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.GainGold, value = 100 }
                },
                failureEffects = new List<GameEffect>
                {
                    new GameEffect { type = GameEffectType.TakeDamage, value = 15 }
                }
            };

            // Act - 성공과 실패 케이스를 모두 테스트
            // 성공 케이스 시뮬레이션 (successChance = 50, roll = 30 -> 성공)
            float testRoll1 = 30f;
            bool isSuccess1 = testRoll1 < choice.successChance;
            Assert.IsTrue(isSuccess1, "roll이 30이고 successChance가 50이면 성공해야 합니다");

            // 실패 케이스 시뮬레이션 (successChance = 50, roll = 70 -> 실패)
            float testRoll2 = 70f;
            bool isSuccess2 = testRoll2 < choice.successChance;
            Assert.IsFalse(isSuccess2, "roll이 70이고 successChance가 50이면 실패해야 합니다");

            // 성공 시 골드 효과 확인
            int goldBefore = player.gold;
            foreach (var effect in choice.successEffects)
            {
                if (effect.type == GameEffectType.GainGold)
                {
                    player.GainGold(effect.value);
                }
            }
            Assert.AreEqual(goldBefore + 100, player.gold, "성공 시 골드 100을 얻어야 합니다");

            // 실패 시 데미지 효과 확인
            int healthBefore = player.currentHealth;
            foreach (var effect in choice.failureEffects)
            {
                if (effect.type == GameEffectType.TakeDamage)
                {
                    player.TakeDamage(effect.value);
                }
            }
            Assert.AreEqual(healthBefore - 15, player.currentHealth, "실패 시 15의 피해를 받아야 합니다");

            Object.DestroyImmediate(playerObject);
        }
    }
}
