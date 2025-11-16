using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class RelicDataTests
    {
        [Test]
        public void RelicData_ScriptableObject를_생성할_수_있다()
        {
            // Arrange & Act
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            
            // Assert
            Assert.IsNotNull(relicData);
            Assert.IsInstanceOf<RelicData>(relicData);
        }

        [Test]
        public void RelicData에_기본_속성을_설정할_수_있다()
        {
            // Arrange
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            
            // Act
            relicData.id = "relic_001";
            relicData.relicName = "금강불괴";
            relicData.description = "전투 시작 시 방어도 3 획득";
            
            // Assert
            Assert.AreEqual("relic_001", relicData.id);
            Assert.AreEqual("금강불괴", relicData.relicName);
            Assert.AreEqual("전투 시작 시 방어도 3 획득", relicData.description);
        }

        [Test]
        public void RelicData의_Trigger_타입을_설정할_수_있다()
        {
            // Arrange
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            
            // Act
            relicData.triggerType = RelicTriggerType.OnBattleStart;
            
            // Assert
            Assert.AreEqual(RelicTriggerType.OnBattleStart, relicData.triggerType);
        }

        [Test]
        public void RelicData의_효과를_정의할_수_있다()
        {
            // Arrange
            var relicData = ScriptableObject.CreateInstance<RelicData>();
            
            // Act
            relicData.effectType = RelicEffectType.GainBlock;
            relicData.effectValue = 3;
            
            // Assert
            Assert.AreEqual(RelicEffectType.GainBlock, relicData.effectType);
            Assert.AreEqual(3, relicData.effectValue);
        }
    }
}
