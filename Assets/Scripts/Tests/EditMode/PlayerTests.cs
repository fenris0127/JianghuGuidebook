using NUnit.Framework;
using GangHoBiGeup.Gameplay;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class PlayerTests
    {
        [Test]
        public void Player_객체를_생성할_수_있다()
        {
            // Arrange & Act
            var player = new Player();
            
            // Assert
            Assert.IsNotNull(player);
            Assert.IsInstanceOf<Player>(player);
        }

        [Test]
        public void Player의_현재_체력을_설정하고_읽을_수_있다()
        {
            // Arrange
            var player = new Player();
            
            // Act
            player.CurrentHealth = 50;
            
            // Assert
            Assert.AreEqual(50, player.CurrentHealth);
        }

        [Test]
        public void Player의_최대_체력을_설정하고_읽을_수_있다()
        {
            // Arrange
            var player = new Player();
            
            // Act
            player.MaxHealth = 100;
            
            // Assert
            Assert.AreEqual(100, player.MaxHealth);
        }

        [Test]
        public void Player가_데미지를_받으면_체력이_감소한다()
        {
            // Arrange
            var player = new Player();
            player.CurrentHealth = 50;
            
            // Act
            player.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(40, player.CurrentHealth);
        }

        [Test]
        public void Player의_체력이_0_이하가_되면_사망_상태가_된다()
        {
            // Arrange
            var player = new Player();
            player.CurrentHealth = 10;
            
            // Act
            player.TakeDamage(15);
            
            // Assert
            Assert.AreEqual(0, player.CurrentHealth);
            Assert.IsTrue(player.IsDead);
        }

        [Test]
        public void Player를_치유하면_체력이_회복되고_최대_체력을_넘지_않는다()
        {
            // Arrange
            var player = new Player();
            player.MaxHealth = 100;
            player.CurrentHealth = 50;
            
            // Act
            player.Heal(30);
            
            // Assert
            Assert.AreEqual(80, player.CurrentHealth);
            
            // Act - 최대 체력 초과 치유 시도
            player.Heal(30);
            
            // Assert
            Assert.AreEqual(100, player.CurrentHealth);
        }

        [Test]
        public void Player의_방어도를_설정하고_읽을_수_있다()
        {
            // Arrange
            var player = new Player();
            
            // Act
            player.Block = 10;
            
            // Assert
            Assert.AreEqual(10, player.Block);
        }

        [Test]
        public void Player가_방어도를_가지고_있을_때_데미지를_받으면_방어도가_먼저_감소한다()
        {
            // Arrange
            var player = new Player();
            player.CurrentHealth = 50;
            player.Block = 15;
            
            // Act - 방어도보다 적은 데미지
            player.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(5, player.Block);
            Assert.AreEqual(50, player.CurrentHealth);
            
            // Act - 방어도를 초과하는 데미지
            player.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(0, player.Block);
            Assert.AreEqual(45, player.CurrentHealth);
        }

        [Test]
        public void Player의_내공을_설정하고_읽을_수_있다()
        {
            // Arrange
            var player = new Player();
            
            // Act
            player.Energy = 3;
            
            // Assert
            Assert.AreEqual(3, player.Energy);
        }

        [Test]
        public void Player의_골드를_설정하고_읽을_수_있다()
        {
            // Arrange
            var player = new Player();
            
            // Act
            player.Gold = 100;
            
            // Assert
            Assert.AreEqual(100, player.Gold);
        }
    }
}
