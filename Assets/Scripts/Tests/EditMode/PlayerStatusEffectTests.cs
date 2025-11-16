using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Gameplay;
using static GangHoBiGeup.Tests.TestHelper;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class PlayerStatusEffectTests
    {
        [Test]
        public void Player에게_상태이상을_적용할_수_있다()
        {
            // Arrange
            var player = CreatePlayer();
            var statusEffect = Strength(2, 3);

            // Act
            player.ApplyStatusEffect(statusEffect);

            // Assert
            Assert.AreEqual(2, player.GetStatusEffectValue(StatusEffectType.Strength));

            Cleanup(player);
        }

        [Test]
        public void Player에게_힘_버프를_적용하면_공격력이_증가한다()
        {
            // Arrange
            var player = CreatePlayer();
            var strengthBuff = Strength(3, 2);

            // Act
            player.ApplyStatusEffect(strengthBuff);
            int damageBonus = player.GetStatusEffectValue(StatusEffectType.Strength);

            // Assert
            Assert.AreEqual(3, damageBonus);
            // 실제 데미지 계산은 전투 시스템에서 이 값을 사용함

            Cleanup(player);
        }

        [Test]
        public void Player에게_약화_디버프를_적용하면_공격력이_감소한다()
        {
            // Arrange
            var player = CreatePlayer();
            var weakDebuff = Weak(2, 2);

            // Act
            player.ApplyStatusEffect(weakDebuff);
            int damagePenalty = player.GetStatusEffectValue(StatusEffectType.Weak);

            // Assert
            Assert.AreEqual(2, damagePenalty);
            // 실제 데미지 감소는 전투 시스템에서 처리

            Cleanup(player);
        }

        [Test]
        public void Player에게_취약_디버프를_적용하면_받는_피해가_증가한다()
        {
            // Arrange
            var player = CreatePlayer(currentHealth: 100);
            player.Block = 0;
            var vulnerableDebuff = Vulnerable(1, 2);

            // Act
            player.ApplyStatusEffect(vulnerableDebuff);
            player.TakeDamage(10);

            // Assert
            // Vulnerable은 받는 피해를 1.5배로 증가 (10 * 1.5 = 15)
            Assert.AreEqual(85, player.CurrentHealth);

            Cleanup(player);
        }

        [Test]
        public void 턴_종료_시_일시적_상태이상의_지속_시간이_감소한다()
        {
            // Arrange
            var player = CreatePlayer();
            var strengthBuff = Strength(2, 3); // 3턴 지속

            // Act
            player.ApplyStatusEffect(strengthBuff);
            Assert.AreEqual(2, player.GetStatusEffectValue(StatusEffectType.Strength));

            // 턴 종료
            player.ProcessStatusEffectsOnTurnEnd();

            // Assert
            // 여전히 효과가 남아있어야 함 (3턴 중 1턴 소모)
            Assert.AreEqual(2, player.GetStatusEffectValue(StatusEffectType.Strength));

            // 2번 더 턴 종료
            player.ProcessStatusEffectsOnTurnEnd();
            player.ProcessStatusEffectsOnTurnEnd();

            // 3턴이 지나면 효과 사라짐
            Assert.AreEqual(0, player.GetStatusEffectValue(StatusEffectType.Strength));

            Cleanup(player);
        }

        [Test]
        public void 같은_상태이상을_중첩_적용하면_수치가_증가한다()
        {
            // Arrange
            var player = CreatePlayer();
            var strength1 = Strength(2, 2);
            var strength2 = Strength(3, 2);

            // Act
            player.ApplyStatusEffect(strength1);
            player.ApplyStatusEffect(strength2);

            // Assert
            Assert.AreEqual(5, player.GetStatusEffectValue(StatusEffectType.Strength)); // 2 + 3 = 5

            Cleanup(player);
        }

        [Test]
        public void 중독_상태이상은_턴_종료_시_데미지를_준다()
        {
            // Arrange
            var player = CreatePlayer(currentHealth: 100);
            var poison = Poison(5, 3); // 5 중독, 3턴

            // Act
            player.ApplyStatusEffect(poison);
            player.ProcessStatusEffectsOnTurnEnd();

            // Assert
            // 중독은 턴 종료 시 5 데미지
            Assert.AreEqual(95, player.CurrentHealth);

            Cleanup(player);
        }
    }
}
