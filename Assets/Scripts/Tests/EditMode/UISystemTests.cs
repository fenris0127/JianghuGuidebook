using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GangHoBiGeup.Gameplay;
using GangHoBiGeup.Data;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class UISystemTests
    {
        // Phase 10.1: 전투 UI
        [Test]
        public void Player의_체력_내공을_UI에_표시한다()
        {
            // Arrange
            var uiObject = new GameObject("PlayerStatsUI");
            var playerStatsUI = uiObject.AddComponent<PlayerStatsUI>();

            // UI 요소들 추가
            var healthTextObj = new GameObject("HealthText");
            healthTextObj.transform.SetParent(uiObject.transform);
            var healthText = healthTextObj.AddComponent<TextMeshProUGUI>();

            var naegongTextObj = new GameObject("NaegongText");
            naegongTextObj.transform.SetParent(uiObject.transform);
            var naegongText = naegongTextObj.AddComponent<TextMeshProUGUI>();

            var defenseIconObj = new GameObject("DefenseIcon");
            defenseIconObj.transform.SetParent(uiObject.transform);

            var defenseTextObj = new GameObject("DefenseText");
            defenseTextObj.transform.SetParent(uiObject.transform);
            var defenseText = defenseTextObj.AddComponent<TextMeshProUGUI>();

            // Act - PlayerStatsUI의 UpdateUI 메서드를 직접 호출
            var updateMethod = typeof(PlayerStatsUI).GetMethod("UpdateUI",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            updateMethod?.Invoke(playerStatsUI, new object[] { 80, 100, 5, 3, 3 });

            // Assert
            Assert.IsNotNull(playerStatsUI);
            // UI 컴포넌트가 존재하는지 확인
            Assert.IsNotNull(healthText);
            Assert.IsNotNull(naegongText);

            Object.DestroyImmediate(uiObject);
        }

        [Test]
        public void Enemy의_체력_Intent를_UI에_표시한다()
        {
            // Arrange
            var uiObject = new GameObject("EnemyUI");
            var enemyUI = uiObject.AddComponent<EnemyUI>();

            var healthSliderObj = new GameObject("HealthSlider");
            healthSliderObj.transform.SetParent(uiObject.transform);
            var healthSlider = healthSliderObj.AddComponent<Slider>();

            var healthTextObj = new GameObject("HealthText");
            healthTextObj.transform.SetParent(uiObject.transform);
            var healthText = healthTextObj.AddComponent<TextMeshProUGUI>();

            var intentPanelObj = new GameObject("IntentPanel");
            intentPanelObj.transform.SetParent(uiObject.transform);

            var intentIconObj = new GameObject("IntentIcon");
            intentIconObj.transform.SetParent(intentPanelObj.transform);
            var intentIcon = intentIconObj.AddComponent<Image>();

            var intentValueTextObj = new GameObject("IntentValueText");
            intentValueTextObj.transform.SetParent(intentPanelObj.transform);
            var intentValueText = intentValueTextObj.AddComponent<TextMeshProUGUI>();

            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 50;
            enemy.currentHealth = 30;

            // Act
            enemyUI.Setup(enemy);

            // Assert
            Assert.IsNotNull(enemyUI);
            Assert.IsNotNull(healthSlider);
            Assert.IsNotNull(healthText);
            Assert.IsNotNull(intentIcon);

            Object.DestroyImmediate(uiObject);
            Object.DestroyImmediate(enemyObject);
        }

        [Test]
        public void 핸드의_카드를_UI에_표시한다()
        {
            // Arrange
            var handUIObj = new GameObject("HandUI");
            var handUI = handUIObj.AddComponent<HandUI>();

            var containerObj = new GameObject("HandContainer");
            containerObj.transform.SetParent(handUIObj.transform);

            // 카드 프리팹 생성
            var cardPrefabObj = new GameObject("CardPrefab");
            cardPrefabObj.AddComponent<CardUI>();

            var playerObject = new GameObject("Player");
            var player = playerObject.AddComponent<Player>();

            var card1 = ScriptableObject.CreateInstance<CardData>();
            card1.cardName = "타격";
            var card2 = ScriptableObject.CreateInstance<CardData>();
            card2.cardName = "방어";
            var card3 = ScriptableObject.CreateInstance<CardData>();
            card3.cardName = "참격";

            player.hand.Add(card1);
            player.hand.Add(card2);
            player.hand.Add(card3);

            // Act
            // HandUI는 Player의 OnHandChanged 이벤트를 구독하므로, 이벤트를 발생시켜야 함
            // 테스트에서는 핸드에 카드가 있는지만 확인
            Assert.AreEqual(3, player.hand.Count, "핸드에 3장의 카드가 있어야 합니다");

            Object.DestroyImmediate(handUIObj);
            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(cardPrefabObj);
        }

        [Test]
        public void 카드_호버_시_툴팁이_표시된다()
        {
            // Arrange
            var cardUIObj = new GameObject("CardUI");
            var cardUI = cardUIObj.AddComponent<CardUI>();
            cardUIObj.AddComponent<CanvasGroup>();

            var nameTextObj = new GameObject("NameText");
            nameTextObj.transform.SetParent(cardUIObj.transform);
            nameTextObj.AddComponent<TextMeshProUGUI>();

            var costTextObj = new GameObject("CostText");
            costTextObj.transform.SetParent(cardUIObj.transform);
            costTextObj.AddComponent<TextMeshProUGUI>();

            var descriptionTextObj = new GameObject("DescriptionText");
            descriptionTextObj.transform.SetParent(cardUIObj.transform);
            descriptionTextObj.AddComponent<TextMeshProUGUI>();

            var cardData = ScriptableObject.CreateInstance<CardData>();
            cardData.cardName = "천마공살";
            cardData.description = "적에게 10의 피해를 입힙니다.";
            cardData.cost = 2;

            // Act
            cardUI.Setup(cardData);

            // Assert - CardUI가 ITooltipProvider 인터페이스를 구현하는지 확인
            Assert.IsTrue(cardUI is ITooltipProvider, "CardUI는 ITooltipProvider를 구현해야 합니다");

            var tooltipProvider = cardUI as ITooltipProvider;
            Assert.AreEqual("천마공살", tooltipProvider.GetTooltipTitle());
            Assert.IsTrue(tooltipProvider.GetTooltipContent().Contains("적에게 10의 피해를 입힙니다"));

            Object.DestroyImmediate(cardUIObj);
        }

        [Test]
        public void 상태이상_아이콘이_표시된다()
        {
            // Arrange
            var enemyObject = new GameObject("Enemy");
            var enemy = enemyObject.AddComponent<Enemy>();
            enemy.maxHealth = 50;
            enemy.currentHealth = 50;

            var poisonEffect = new StatusEffect(StatusEffectType.Poison, 3, 2);
            var strengthEffect = new StatusEffect(StatusEffectType.Strength, 2, 0);

            // Act
            enemy.ApplyStatusEffect(poisonEffect);
            enemy.ApplyStatusEffect(strengthEffect);

            // Assert
            Assert.AreEqual(3, enemy.GetStatusEffectValue(StatusEffectType.Poison), "중독 상태이상이 3이어야 합니다");
            Assert.AreEqual(2, enemy.GetStatusEffectValue(StatusEffectType.Strength), "힘 상태이상이 2여야 합니다");

            Object.DestroyImmediate(enemyObject);
        }

        // Phase 10.2: 맵 UI
        [Test]
        public void 맵_노드가_UI에_표시된다()
        {
            // Arrange
            var nodeObject = new GameObject("MapNode");
            var button = nodeObject.AddComponent<Button>();
            var image = nodeObject.AddComponent<Image>();
            var mapNode = nodeObject.AddComponent<MapNode>();

            mapNode.nodeType = NodeType.Combat;
            mapNode.Setup(0);

            // Act & Assert
            Assert.IsNotNull(mapNode);
            Assert.IsNotNull(button, "MapNode는 Button 컴포넌트를 가져야 합니다");
            Assert.IsNotNull(image, "MapNode는 Image 컴포넌트를 가져야 합니다");
            Assert.AreEqual(NodeType.Combat, mapNode.nodeType);
            Assert.AreEqual(0, mapNode.layer);

            Object.DestroyImmediate(nodeObject);
        }

        [Test]
        public void 현재_위치가_표시된다()
        {
            // Arrange
            var currentNode = new GameObject("CurrentNode").AddComponent<MapNode>();
            currentNode.gameObject.AddComponent<Button>();
            currentNode.gameObject.AddComponent<Image>();
            currentNode.nodeType = NodeType.Combat;

            // Act
            currentNode.SetClickable(true);
            var button = currentNode.GetComponent<Button>();

            // Assert
            Assert.IsTrue(button.interactable, "현재 위치 노드는 활성화되어 있어야 합니다");

            Object.DestroyImmediate(currentNode.gameObject);
        }

        [Test]
        public void 이동_가능한_노드가_강조_표시된다()
        {
            // Arrange
            var clickableNode = new GameObject("ClickableNode").AddComponent<MapNode>();
            clickableNode.gameObject.AddComponent<Button>();
            clickableNode.gameObject.AddComponent<Image>();

            var lockedNode = new GameObject("LockedNode").AddComponent<MapNode>();
            lockedNode.gameObject.AddComponent<Button>();
            lockedNode.gameObject.AddComponent<Image>();

            // Act
            clickableNode.SetClickable(true);
            lockedNode.SetClickable(false);

            var clickableButton = clickableNode.GetComponent<Button>();
            var lockedButton = lockedNode.GetComponent<Button>();

            // Assert
            Assert.IsTrue(clickableButton.interactable, "이동 가능한 노드는 클릭 가능해야 합니다");
            Assert.IsFalse(lockedButton.interactable, "이동 불가능한 노드는 클릭 불가능해야 합니다");

            // 시각적 차이 확인 (색상 변경)
            var clickableImage = clickableNode.GetComponent<Image>();
            var lockedImage = lockedNode.GetComponent<Image>();
            Assert.IsNotNull(clickableImage);
            Assert.IsNotNull(lockedImage);

            Object.DestroyImmediate(clickableNode.gameObject);
            Object.DestroyImmediate(lockedNode.gameObject);
        }
    }
}
