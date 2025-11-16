using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class FactionDataTests
    {
        [Test]
        public void FactionData_ScriptableObject를_생성할_수_있다()
        {
            // Arrange & Act
            var factionData = ScriptableObject.CreateInstance<FactionData>();
            
            // Assert
            Assert.IsNotNull(factionData);
            Assert.IsInstanceOf<FactionData>(factionData);
        }

        [Test]
        public void FactionData에_기본_속성을_설정할_수_있다()
        {
            // Arrange
            var factionData = ScriptableObject.CreateInstance<FactionData>();
            
            // Act
            factionData.id = "faction_hwasan";
            factionData.factionName = "화산파";
            
            // Assert
            Assert.AreEqual("faction_hwasan", factionData.id);
            Assert.AreEqual("화산파", factionData.factionName);
        }

        [Test]
        public void FactionData에_시작_덱_리스트를_설정할_수_있다()
        {
            // Arrange
            var factionData = ScriptableObject.CreateInstance<FactionData>();
            var card1 = ScriptableObject.CreateInstance<CardData>();
            var card2 = ScriptableObject.CreateInstance<CardData>();
            
            // Act
            factionData.startingDeck = new System.Collections.Generic.List<CardData>();
            factionData.startingDeck.Add(card1);
            factionData.startingDeck.Add(card2);
            
            // Assert
            Assert.IsNotNull(factionData.startingDeck);
            Assert.AreEqual(2, factionData.startingDeck.Count);
            Assert.IsInstanceOf<CardData>(factionData.startingDeck[0]);
        }

        [Test]
        public void FactionData에_패시브_능력을_설정할_수_있다()
        {
            // Arrange
            var factionData = ScriptableObject.CreateInstance<FactionData>();
            
            // Act
            factionData.passiveAbility = "연계 초식: 같은 타입의 카드를 연속으로 사용 시 추가 피해";
            factionData.passiveType = FactionPassiveType.ComboBonus;
            
            // Assert
            Assert.AreEqual("연계 초식: 같은 타입의 카드를 연속으로 사용 시 추가 피해", factionData.passiveAbility);
            Assert.AreEqual(FactionPassiveType.ComboBonus, factionData.passiveType);
        }
    }
}
