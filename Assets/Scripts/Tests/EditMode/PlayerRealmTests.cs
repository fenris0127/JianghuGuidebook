using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Gameplay;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class PlayerRealmTests
    {
        [Test]
        public void Player의_수련치를_획득할_수_있다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            player.CurrentXp = 0;
            
            // Act
            player.GainXp(5);
            
            // Assert
            Assert.AreEqual(5, player.CurrentXp);
        }

        [Test]
        public void 수련치가_100퍼센트에_도달하면_경지_돌파_가능_상태가_된다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            player.CurrentXp = 0;
            player.XpToNextRealm = 10;
            player.IsReadyToAscend = false;
            
            // Act
            player.GainXp(10);
            
            // Assert
            Assert.AreEqual(10, player.CurrentXp);
            Assert.IsTrue(player.IsReadyToAscend);
        }

        [Test]
        public void Player의_내공_경지를_상승시킬_수_있다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            player.CurrentRealm = Realm.Samryu;
            
            // Act
            player.AscendRealm(Realm.Iryu);
            
            // Assert
            Assert.AreEqual(Realm.Iryu, player.CurrentRealm);
        }

        [Test]
        public void 내공_경지가_상승하면_최대_내공이_증가한다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            player.CurrentRealm = Realm.Samryu;
            player.MaxNaegong = 3;
            
            // Act
            player.AscendRealm(Realm.Iryu);
            
            // Assert
            Assert.AreEqual(Realm.Iryu, player.CurrentRealm);
            Assert.AreEqual(4, player.MaxNaegong); // 경지 상승 시 최대 내공 증가
        }

        [Test]
        public void Player의_무기술_경지를_기록할_수_있다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            
            // Act
            player.CurrentSwordRealm = SwordRealm.Geomgi;
            
            // Assert
            Assert.AreEqual(SwordRealm.Geomgi, player.CurrentSwordRealm);
        }

        [Test]
        public void 특정_업적을_달성하면_무기술_경지가_상승한다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            player.CurrentSwordRealm = SwordRealm.None;
            
            // Act - 공격 10회 달성 (검기 경지 조건)
            for (int i = 0; i < 10; i++)
            {
                player.RecordSwordEvent("Attack");
            }
            
            // Assert
            Assert.AreEqual(SwordRealm.Geomgi, player.CurrentSwordRealm);
        }

        [Test]
        public void 최고_경지에서는_더_이상_수련치를_얻지_않는다()
        {
            // Arrange
            var player = new GameObject().AddComponent<Player>();
            player.CurrentRealm = Realm.Saengsagyeong;
            player.CurrentXp = 0;
            
            // Act
            player.GainXp(10);
            
            // Assert
            Assert.AreEqual(0, player.CurrentXp); // 최고 경지에서는 XP 증가 안 함
        }
    }
}
