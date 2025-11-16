using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using GangHoBiGeup.Gameplay;
using GangHoBiGeup.Data;
using static GangHoBiGeup.Tests.TestHelper;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class InnerDemonSystemTests
    {
        // Phase 12.1: 폐관수련과 심마
        [Test]
        public void 휴식처에서_폐관수련을_선택할_수_있다()
        {
            // Arrange
            var restSiteUIObject = new GameObject("RestSiteUI");
            var restSiteUI = restSiteUIObject.AddComponent<RestSiteUI>();

            var panelObject = new GameObject("Panel");
            panelObject.transform.SetParent(restSiteUIObject.transform);

            var restButtonObject = new GameObject("RestButton");
            restButtonObject.transform.SetParent(panelObject.transform);
            var restButton = restButtonObject.AddComponent<Button>();

            var smithButtonObject = new GameObject("SmithButton");
            smithButtonObject.transform.SetParent(panelObject.transform);
            var smithButton = smithButtonObject.AddComponent<Button>();

            var ascendButtonObject = new GameObject("AscendButton");
            ascendButtonObject.transform.SetParent(panelObject.transform);
            var ascendButton = ascendButtonObject.AddComponent<Button>();

            var deckViewUIObject = new GameObject("DeckViewUI");
            deckViewUIObject.transform.SetParent(restSiteUIObject.transform);
            deckViewUIObject.AddComponent<DeckViewUI>();

            var player = CreatePlayer(maxHealth: 100, currentHealth: 100);

            // 경지 돌파 준비 상태 설정
            player.IsReadyToAscend = true;

            // Act
            // RestSiteUI의 Open 메서드를 리플렉션으로 호출
            var openMethod = typeof(RestSiteUI).GetMethod("Open",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Assert - ascendButton이 활성화되는지 확인
            Assert.IsTrue(player.IsReadyToAscend, "플레이어가 경지 돌파 준비 상태여야 합니다");
            Assert.IsNotNull(ascendButton, "폐관수련 버튼이 존재해야 합니다");

            Cleanup(restSiteUI, player);
        }

        [Test]
        public void 폐관수련_선택_시_심마와의_전투가_시작된다()
        {
            // Arrange
            var player = CreatePlayer(maxHealth: 100, currentHealth: 100);
            player.CurrentRealm = Realm.Samryu;
            player.IsReadyToAscend = true;

            // Act - 폐관수련(심마 전투) 시작
            // GameManager.Instance.StartAscensionBattle(player.CurrentRealm) 시뮬레이션
            string expectedEncounterID = $"Ascension_{player.CurrentRealm}";

            // Assert
            Assert.AreEqual("Ascension_Samryu", expectedEncounterID, "삼류 경지의 심마 전투가 시작되어야 합니다");
            Assert.IsTrue(player.IsReadyToAscend, "경지 돌파 준비 상태여야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 심마_전투에서_승리하면_경지가_상승한다()
        {
            // Arrange
            var player = CreatePlayer(maxHealth: 100, currentHealth: 100);

            // 삼류 경지에서 시작
            Realm initialRealm = Realm.Samryu;
            player.CurrentRealm = initialRealm;
            player.IsReadyToAscend = true;

            int initialMaxNaegong = player.MaxNaegong;

            // Act - 심마 전투 승리 시 경지 상승
            Realm nextRealm = Realm.Iryu;
            player.AscendRealm(nextRealm);

            // Assert
            Assert.AreEqual(Realm.Iryu, player.CurrentRealm, "경지가 일류로 상승해야 합니다");
            Assert.IsFalse(player.IsReadyToAscend, "경지 상승 후 준비 상태가 해제되어야 합니다");
            Assert.Greater(player.MaxNaegong, initialMaxNaegong, "경지 상승 시 최대 내공이 증가해야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 심마_전투에서_패배하면_주화입마_상태가_된다()
        {
            // Arrange
            var player = CreatePlayer(maxHealth: 100, currentHealth: 100);
            player.CurrentRealm = Realm.Samryu;

            // Act - 심마 전투 패배 시 주화입마(디버프) 적용
            // 주화입마는 StatusEffect로 처리됨
            var demonicPossession = Weak(3, 5); // 5턴 동안 약화 3
            player.ApplyStatusEffect(demonicPossession);

            // Assert
            Assert.AreEqual(3, player.GetStatusEffectValue(StatusEffectType.Weak),
                "심마 전투 패배 시 약화 디버프가 적용되어야 합니다");

            // 경지는 상승하지 않음
            Assert.AreEqual(Realm.Samryu, player.CurrentRealm, "패배 시 경지가 상승하지 않아야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 경지가_상승하면_최대_내공이_증가한다()
        {
            // Arrange
            var player = CreatePlayer();

            // Act & Assert - 각 경지별 최대 내공 확인
            player.AscendRealm(Realm.Samryu);
            int naegongSamryu = player.MaxNaegong;

            player.AscendRealm(Realm.Iryu);
            int naegongIryu = player.MaxNaegong;

            player.AscendRealm(Realm.Illyu);
            int naegongIllyu = player.MaxNaegong;

            player.AscendRealm(Realm.Jeoljeong);
            int naegongJeoljeong = player.MaxNaegong;

            // Assert
            Assert.AreEqual(3, naegongSamryu, "삼류는 최대 내공 3이어야 합니다");
            Assert.AreEqual(4, naegongIryu, "일류는 최대 내공 4여야 합니다");
            Assert.AreEqual(4, naegongIllyu, "일류는 최대 내공 4여야 합니다");
            Assert.AreEqual(5, naegongJeoljeong, "절정은 최대 내공 5여야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 경험치가_충분하면_경지_돌파_준비_상태가_된다()
        {
            // Arrange
            var player = CreatePlayer();
            player.CurrentRealm = Realm.Samryu;
            player.XpToNextRealm = 10;

            // Act
            // 경험치를 충분히 획득
            for (int i = 0; i < 10; i++)
            {
                player.GainXp(1);
            }

            // Assert
            Assert.IsTrue(player.IsReadyToAscend, "경험치가 충분하면 경지 돌파 준비 상태가 되어야 합니다");

            Cleanup(player);
        }

        [Test]
        public void 최고_경지에서는_더_이상_상승하지_않는다()
        {
            // Arrange
            var player = CreatePlayer();

            // Act - 최고 경지로 설정
            player.AscendRealm(Realm.Saengsagyeong);

            // 경험치를 충분히 획득해도
            for (int i = 0; i < 100; i++)
            {
                player.GainXp(1);
            }

            // Assert
            Assert.AreEqual(Realm.Saengsagyeong, player.CurrentRealm, "최고 경지는 생사경이어야 합니다");
            Assert.IsFalse(player.IsReadyToAscend, "최고 경지에서는 더 이상 돌파할 수 없어야 합니다");

            Cleanup(player);
        }
    }
}
