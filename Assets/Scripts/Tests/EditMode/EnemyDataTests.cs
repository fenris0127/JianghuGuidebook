using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class EnemyDataTests
    {
        [Test]
        public void EnemyData_ScriptableObject를_생성할_수_있다()
        {
            // Arrange & Act
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            
            // Assert
            Assert.IsNotNull(enemyData);
            Assert.IsInstanceOf<EnemyData>(enemyData);
        }

        [Test]
        public void EnemyData에_기본_속성을_설정할_수_있다()
        {
            // Arrange
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            
            // Act
            enemyData.id = "goblin_001";
            enemyData.enemyName = "산적";
            enemyData.maxHealth = 50;
            
            // Assert
            Assert.AreEqual("goblin_001", enemyData.id);
            Assert.AreEqual("산적", enemyData.enemyName);
            Assert.AreEqual(50, enemyData.maxHealth);
        }

        [Test]
        public void EnemyData에_ActionPattern_리스트를_추가할_수_있다()
        {
            // Arrange
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            
            // Act
            enemyData.actionPatterns = new System.Collections.Generic.List<ActionPattern>();
            var pattern = new ActionPattern();
            enemyData.actionPatterns.Add(pattern);
            
            // Assert
            Assert.IsNotNull(enemyData.actionPatterns);
            Assert.AreEqual(1, enemyData.actionPatterns.Count);
            Assert.IsInstanceOf<ActionPattern>(enemyData.actionPatterns[0]);
        }

        [Test]
        public void ActionPattern에_Attack_타입과_데미지를_설정할_수_있다()
        {
            // Arrange
            var pattern = new ActionPattern();
            
            // Act
            pattern.actionType = ActionType.Attack;
            pattern.value = 8;
            
            // Assert
            Assert.AreEqual(ActionType.Attack, pattern.actionType);
            Assert.AreEqual(8, pattern.value);
        }

        [Test]
        public void ActionPattern에_Block_타입과_방어도를_설정할_수_있다()
        {
            // Arrange
            var pattern = new ActionPattern();
            
            // Act
            pattern.actionType = ActionType.Block;
            pattern.value = 6;
            
            // Assert
            Assert.AreEqual(ActionType.Block, pattern.actionType);
            Assert.AreEqual(6, pattern.value);
        }
    }
}
