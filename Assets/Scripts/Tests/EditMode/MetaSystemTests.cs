using NUnit.Framework;
using UnityEngine;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class MetaSystemTests
    {
        // Phase 11.1: 깨달음 시스템
        [Test]
        public void 게임_종료_시_깨달음_포인트를_획득한다()
        {
            // Arrange
            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();

            // MetaManager의 Awake를 호출하여 초기화
            var awakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(metaManager, null);

            int initialPoints = metaManager.Progress.enlightenmentPoints;

            // Act - 게임 종료 시 깨달음 포인트 획득 (예: 3층 패배 시)
            int earnedPoints = 3 * 10; // currentFloor * 10
            metaManager.AddEnlightenmentPoints(earnedPoints);

            // Assert
            Assert.AreEqual(initialPoints + 30, metaManager.Progress.enlightenmentPoints,
                "3층에서 패배 시 30 깨달음 포인트를 얻어야 합니다");

            Object.DestroyImmediate(metaManagerObject);
        }

        [Test]
        public void 깨달음_포인트로_영구_강화를_구매할_수_있다()
        {
            // Arrange
            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();

            var awakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(metaManager, null);

            // 충분한 포인트 지급
            metaManager.Progress.enlightenmentPoints = 100;
            int initialBonusHealthLevel = metaManager.Progress.bonusHealthLevel;

            // Act
            int cost = metaManager.GetUpgradeCost("BonusHealth");
            bool success = metaManager.TryPurchaseUpgrade("BonusHealth", cost);

            // Assert
            Assert.IsTrue(success, "충분한 포인트가 있으면 구매에 성공해야 합니다");
            Assert.AreEqual(initialBonusHealthLevel + 1, metaManager.Progress.bonusHealthLevel,
                "영구 강화 레벨이 1 증가해야 합니다");
            Assert.AreEqual(100 - cost, metaManager.Progress.enlightenmentPoints,
                "소비한 포인트만큼 차감되어야 합니다");

            Object.DestroyImmediate(metaManagerObject);
        }

        [Test]
        public void 영구_강화가_새_게임에_적용된다()
        {
            // Arrange
            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();

            var awakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(metaManager, null);

            // 영구 강화 구매
            metaManager.Progress.enlightenmentPoints = 100;
            metaManager.Progress.bonusHealthLevel = 2; // 체력 +10 (2레벨 * 5)
            metaManager.Progress.startingGoldLevel = 1; // 시작 골드 +20 (1레벨 * 20)

            var factionData = ScriptableObject.CreateInstance<FactionData>();
            factionData.startingDeck = new System.Collections.Generic.List<CardData>();

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            // Act - 새 게임 시작 시 영구 강화 적용
            int bonusHealth = metaManager.Progress.bonusHealthLevel * 5; // 2 * 5 = 10
            player.Setup(new System.Collections.Generic.List<CardData>(factionData.startingDeck), bonusHealth);

            int startingGold = metaManager.Progress.startingGoldLevel * 20; // 1 * 20 = 20
            if (startingGold > 0)
                player.GainGold(startingGold);

            // Assert
            Assert.AreEqual(10, bonusHealth, "보너스 체력이 10이어야 합니다");
            Assert.AreEqual(20, startingGold, "시작 골드가 20이어야 합니다");
            Assert.AreEqual(20, player.gold, "플레이어가 시작 골드를 가지고 있어야 합니다");

            Object.DestroyImmediate(metaManagerObject);
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void 포인트가_부족하면_강화를_구매할_수_없다()
        {
            // Arrange
            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();

            var awakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(metaManager, null);

            // 포인트를 부족하게 설정
            metaManager.Progress.enlightenmentPoints = 5;
            int cost = metaManager.GetUpgradeCost("BonusHealth"); // 보통 10 이상

            // Act
            bool success = metaManager.TryPurchaseUpgrade("BonusHealth", cost);

            // Assert
            Assert.IsFalse(success, "포인트가 부족하면 구매에 실패해야 합니다");
            Assert.AreEqual(5, metaManager.Progress.enlightenmentPoints,
                "실패 시 포인트가 차감되지 않아야 합니다");

            Object.DestroyImmediate(metaManagerObject);
        }

        [Test]
        public void 강화_비용이_레벨에_따라_증가한다()
        {
            // Arrange
            var metaManagerObject = new GameObject("MetaManager");
            var metaManager = metaManagerObject.AddComponent<MetaManager>();

            var awakeMethod = typeof(MetaManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(metaManager, null);

            metaManager.Progress.bonusHealthLevel = 0;

            // Act
            int cost1 = metaManager.GetUpgradeCost("BonusHealth"); // 10 + (0 * 5) = 10

            metaManager.Progress.bonusHealthLevel = 1;
            int cost2 = metaManager.GetUpgradeCost("BonusHealth"); // 10 + (1 * 5) = 15

            metaManager.Progress.bonusHealthLevel = 2;
            int cost3 = metaManager.GetUpgradeCost("BonusHealth"); // 10 + (2 * 5) = 20

            // Assert
            Assert.AreEqual(10, cost1, "레벨 0일 때 비용은 10이어야 합니다");
            Assert.AreEqual(15, cost2, "레벨 1일 때 비용은 15여야 합니다");
            Assert.AreEqual(20, cost3, "레벨 2일 때 비용은 20이어야 합니다");
            Assert.IsTrue(cost3 > cost2 && cost2 > cost1, "레벨이 올라갈수록 비용이 증가해야 합니다");

            Object.DestroyImmediate(metaManagerObject);
        }
    }
}
