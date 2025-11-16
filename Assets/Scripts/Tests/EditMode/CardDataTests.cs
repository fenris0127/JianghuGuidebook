using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class CardDataTests
    {
        [Test]
        public void CardData_ScriptableObject를_생성할_수_있다()
        {
            // Arrange & Act
            var cardData = ScriptableObject.CreateInstance<CardData>();
            
            // Assert
            Assert.IsNotNull(cardData);
            Assert.IsInstanceOf<CardData>(cardData);
        }

        [Test]
        public void CardData에_기본_속성을_설정할_수_있다()
        {
            // Arrange
            var cardData = ScriptableObject.CreateInstance<CardData>();
            
            // Act
            cardData.id = "strike_001";
            cardData.cardName = "일반 베기";
            cardData.cost = 1;
            
            // Assert
            Assert.AreEqual("strike_001", cardData.id);
            Assert.AreEqual("일반 베기", cardData.cardName);
            Assert.AreEqual(1, cardData.cost);
        }

        [Test]
        public void CardData에_효과_리스트를_추가할_수_있다()
        {
            // Arrange
            var cardData = ScriptableObject.CreateInstance<CardData>();
            
            // Act
            cardData.effects = new System.Collections.Generic.List<CardEffect>();
            var effect = new CardEffect();
            cardData.effects.Add(effect);
            
            // Assert
            Assert.IsNotNull(cardData.effects);
            Assert.AreEqual(1, cardData.effects.Count);
            Assert.IsInstanceOf<CardEffect>(cardData.effects[0]);
        }

        [Test]
        public void CardEffect에_데미지_타입과_값을_설정할_수_있다()
        {
            // Arrange
            var effect = new CardEffect();
            
            // Act
            effect.effectType = EffectType.Damage;
            effect.value = 6;
            
            // Assert
            Assert.AreEqual(EffectType.Damage, effect.effectType);
            Assert.AreEqual(6, effect.value);
        }

        [Test]
        public void CardEffect에_방어도_타입과_값을_설정할_수_있다()
        {
            // Arrange
            var effect = new CardEffect();
            
            // Act
            effect.effectType = EffectType.Block;
            effect.value = 5;
            
            // Assert
            Assert.AreEqual(EffectType.Block, effect.effectType);
            Assert.AreEqual(5, effect.value);
        }

        [Test]
        public void CardData의_Rarity를_설정할_수_있다()
        {
            // Arrange
            var cardData = ScriptableObject.CreateInstance<CardData>();
            
            // Act
            cardData.rarity = CardRarity.Rare;
            
            // Assert
            Assert.AreEqual(CardRarity.Rare, cardData.rarity);
        }

        [Test]
        public void CardData의_문파를_설정할_수_있다()
        {
            // Arrange
            var cardData = ScriptableObject.CreateInstance<CardData>();
            
            // Act
            cardData.faction = Faction.HwaSan;
            
            // Assert
            Assert.AreEqual(Faction.HwaSan, cardData.faction);
        }
    }
}
