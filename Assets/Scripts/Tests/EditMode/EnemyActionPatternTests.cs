using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Gameplay;
using GangHoBiGeup.Data;
using System.Collections.Generic;
using static GangHoBiGeup.Tests.TestHelper;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class EnemyActionPatternTests
    {
        [Test]
        public void Enemy의_다음_행동을_예고할_수_있다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var action = new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = 8
            };

            // Act
            enemy.SetNextAction(action);
            var nextAction = enemy.GetNextAction();

            // Assert
            Assert.IsNotNull(nextAction);
            Assert.AreEqual(EnemyActionType.Attack, nextAction.actionType);
            Assert.AreEqual(8, nextAction.value);

            Cleanup(enemy);
        }

        [Test]
        public void Enemy가_ActionPattern에_따라_행동을_선택한다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var enemyData = CreateEnemyData("test_enemy", "테스트 적", 50);

            var pattern = new List<EnemyAction>
            {
                new EnemyAction { actionType = EnemyActionType.Attack, value = 5 },
                new EnemyAction { actionType = EnemyActionType.Defend, value = 8 },
                new EnemyAction { actionType = EnemyActionType.Attack, value = 10 }
            };

            // Act
            enemy.InitializeWithPattern(enemyData, pattern);
            var action1 = enemy.GetNextAction();

            enemy.AdvanceTurn();
            var action2 = enemy.GetNextAction();

            enemy.AdvanceTurn();
            var action3 = enemy.GetNextAction();

            enemy.AdvanceTurn(); // 패턴이 반복되어야 함
            var action4 = enemy.GetNextAction();

            // Assert
            Assert.AreEqual(EnemyActionType.Attack, action1.actionType);
            Assert.AreEqual(5, action1.value);

            Assert.AreEqual(EnemyActionType.Defend, action2.actionType);
            Assert.AreEqual(8, action2.value);

            Assert.AreEqual(EnemyActionType.Attack, action3.actionType);
            Assert.AreEqual(10, action3.value);

            // 패턴 반복 확인
            Assert.AreEqual(EnemyActionType.Attack, action4.actionType);
            Assert.AreEqual(5, action4.value);

            Cleanup(enemy);
        }

        [Test]
        public void Enemy가_공격_행동을_수행하면_Player가_데미지를_받는다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var player = CreatePlayer(currentHealth: 100);
            player.Block = 0;

            var attackAction = new EnemyAction
            {
                actionType = EnemyActionType.Attack,
                value = 15
            };
            enemy.SetNextAction(attackAction);

            // Act
            enemy.PerformAction(player);

            // Assert
            Assert.AreEqual(85, player.CurrentHealth);

            Cleanup(enemy, player);
        }

        [Test]
        public void Enemy가_방어_행동을_수행하면_방어도를_얻는다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var player = CreatePlayer();

            var defendAction = new EnemyAction
            {
                actionType = EnemyActionType.Defend,
                value = 12
            };
            enemy.SetNextAction(defendAction);

            // Act
            enemy.PerformAction(player);

            // Assert
            Assert.AreEqual(12, enemy.Block);

            Cleanup(enemy, player);
        }

        [Test]
        public void Enemy가_버프_행동을_수행할_수_있다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var player = CreatePlayer();

            var buffAction = new EnemyAction
            {
                actionType = EnemyActionType.Buff,
                value = 3
            };

            // StatusEffectData 생성
            var strengthData = ScriptableObject.CreateInstance<StatusEffectData>();
            strengthData.type = StatusEffectType.Strength;
            buffAction.statusEffectData = strengthData;

            enemy.SetNextAction(buffAction);

            // Act
            enemy.PerformAction(player);

            // Assert
            Assert.AreEqual(3, enemy.GetStatusEffectValue(StatusEffectType.Strength));

            Cleanup(enemy, player);
        }

        [Test]
        public void Enemy가_디버프_행동을_수행하면_Player에게_디버프를_건다()
        {
            // Arrange
            var enemy = CreateEnemy();
            var player = CreatePlayer();

            var debuffAction = new EnemyAction
            {
                actionType = EnemyActionType.Debuff,
                value = 2
            };

            // StatusEffectData 생성
            var weakData = ScriptableObject.CreateInstance<StatusEffectData>();
            weakData.type = StatusEffectType.Weak;
            debuffAction.statusEffectData = weakData;

            enemy.SetNextAction(debuffAction);

            // Act
            enemy.PerformAction(player);

            // Assert
            Assert.AreEqual(2, player.GetStatusEffectValue(StatusEffectType.Weak));

            Cleanup(enemy, player);
        }
    }
}
