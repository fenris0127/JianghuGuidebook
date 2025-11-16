using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Gameplay;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class EnemyStatusEffectTests
    {
        [Test]
        public void Enemy에게_중독_디버프를_적용할_수_있다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            var poison = new StatusEffect(StatusEffectType.Poison, 3, 2);

            // Act
            enemy.ApplyStatusEffect(poison);

            // Assert
            Assert.AreEqual(3, enemy.GetStatusEffectValue(StatusEffectType.Poison));
        }

        [Test]
        public void 턴_종료_시_중독_데미지를_받는다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            enemy.CurrentHealth = 100;
            var poison = new StatusEffect(StatusEffectType.Poison, 5, 3); // 5 중독, 3턴

            // Act
            enemy.ApplyStatusEffect(poison);
            enemy.ProcessStatusEffectsOnTurnEnd();

            // Assert
            // 중독은 턴 종료 시 5 데미지
            Assert.AreEqual(95, enemy.CurrentHealth);
        }

        [Test]
        public void Enemy에게_여러_상태이상을_동시에_적용할_수_있다()
        {
            // Arrange
            var enemy = new GameObject().AddComponent<Enemy>();
            var poison = new StatusEffect(StatusEffectType.Poison, 3, 2);
            var weak = new StatusEffect(StatusEffectType.Weak, 2, 3);

            // Act
            enemy.ApplyStatusEffect(poison);
            enemy.ApplyStatusEffect(weak);

            // Assert
            Assert.AreEqual(3, enemy.GetStatusEffectValue(StatusEffectType.Poison));
            Assert.AreEqual(2, enemy.GetStatusEffectValue(StatusEffectType.Weak));
        }
    }
}
