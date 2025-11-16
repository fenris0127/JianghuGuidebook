using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Gameplay;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class EnemyTests
    {
        [Test]
        public void Enemy_객체를_생성할_수_있다()
        {
            // Arrange & Act
            var enemy = new GameObject().AddComponent<Enemy>();
            
            // Assert
            Assert.IsNotNull(enemy);
            Assert.IsInstanceOf<Enemy>(enemy);
        }

        [Test]
        public void Enemy를_EnemyData로_초기화할_수_있다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            enemyData.id = "goblin_001";
            enemyData.enemyName = "산적";
            enemyData.maxHealth = 50;
            
            // Act
            enemy.Initialize(enemyData);
            
            // Assert
            Assert.AreEqual(50, enemy.MaxHealth);
            Assert.AreEqual(50, enemy.CurrentHealth);
        }

        [Test]
        public void Enemy의_체력을_설정하고_읽을_수_있다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            
            // Act
            enemy.CurrentHealth = 30;
            enemy.MaxHealth = 50;
            
            // Assert
            Assert.AreEqual(30, enemy.CurrentHealth);
            Assert.AreEqual(50, enemy.MaxHealth);
        }

        [Test]
        public void Enemy가_데미지를_받으면_체력이_감소한다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            enemy.CurrentHealth = 50;
            
            // Act
            enemy.TakeDamage(15);
            
            // Assert
            Assert.AreEqual(35, enemy.CurrentHealth);
        }

        [Test]
        public void Enemy의_체력이_0_이하가_되면_사망_상태가_된다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            enemy.CurrentHealth = 10;
            
            // Act
            enemy.TakeDamage(15);
            
            // Assert
            Assert.AreEqual(0, enemy.CurrentHealth);
            Assert.IsTrue(enemy.IsDead);
        }

        [Test]
        public void Enemy의_방어도를_설정하고_읽을_수_있다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            
            // Act
            enemy.Block = 10;
            
            // Assert
            Assert.AreEqual(10, enemy.Block);
        }

        [Test]
        public void Enemy가_방어도를_가지고_있을_때_데미지를_받으면_방어도가_먼저_감소한다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            enemy.CurrentHealth = 50;
            enemy.Block = 15;
            
            // Act - 방어도보다 적은 데미지
            enemy.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(5, enemy.Block);
            Assert.AreEqual(50, enemy.CurrentHealth);
            
            // Act - 방어도를 초과하는 데미지
            enemy.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(0, enemy.Block);
            Assert.AreEqual(45, enemy.CurrentHealth);
        }
    }
}
