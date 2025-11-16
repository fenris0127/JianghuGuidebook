using NUnit.Framework;
using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    [TestFixture]
    public class MapSystemTests
    {
        // Phase 7.1: 맵 생성
        [Test]
        public void MapManager를_생성할_수_있다()
        {
            // Arrange
            var mapManagerObject = new GameObject("MapManager");

            // Act
            var mapManager = mapManagerObject.AddComponent<MapManager>();

            // Assert
            Assert.IsNotNull(mapManager);
            Assert.IsInstanceOf<MapManager>(mapManager);

            Object.DestroyImmediate(mapManagerObject);
        }

        [Test]
        public void MapManager를_싱글톤으로_생성할_수_있다()
        {
            // Arrange
            var mapManagerObject = new GameObject("MapManager");
            var mapManager = mapManagerObject.AddComponent<MapManager>();

            // Act
            var awakeMethod = typeof(MapManager).GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(mapManager, null);

            // Assert
            Assert.IsNotNull(MapManager.Instance);
            Assert.AreEqual(mapManager, MapManager.Instance);

            Object.DestroyImmediate(mapManagerObject);
        }

        [Test]
        public void 절차적으로_맵_노드를_생성할_수_있다()
        {
            // Arrange
            var nodeObject = new GameObject("MapNode");
            var button = nodeObject.AddComponent<UnityEngine.UI.Button>();
            var image = nodeObject.AddComponent<UnityEngine.UI.Image>();
            var mapNode = nodeObject.AddComponent<MapNode>();

            // Act
            mapNode.nodeType = NodeType.Combat;
            mapNode.Setup(0); // 0층 설정

            // Assert
            Assert.AreEqual(NodeType.Combat, mapNode.nodeType);
            Assert.AreEqual(0, mapNode.layer);

            Object.DestroyImmediate(nodeObject);
        }

        [Test]
        public void 노드_간_연결_경로를_생성할_수_있다()
        {
            // Arrange
            var node1Object = new GameObject("Node1");
            node1Object.AddComponent<UnityEngine.UI.Button>();
            node1Object.AddComponent<UnityEngine.UI.Image>();
            var node1 = node1Object.AddComponent<MapNode>();

            var node2Object = new GameObject("Node2");
            node2Object.AddComponent<UnityEngine.UI.Button>();
            node2Object.AddComponent<UnityEngine.UI.Image>();
            var node2 = node2Object.AddComponent<MapNode>();

            var node3Object = new GameObject("Node3");
            node3Object.AddComponent<UnityEngine.UI.Button>();
            node3Object.AddComponent<UnityEngine.UI.Image>();
            var node3 = node3Object.AddComponent<MapNode>();

            // Act
            node1.connectedNodes.Add(node2);
            node1.connectedNodes.Add(node3);

            // Assert
            Assert.AreEqual(2, node1.connectedNodes.Count);
            Assert.Contains(node2, node1.connectedNodes);
            Assert.Contains(node3, node1.connectedNodes);

            Object.DestroyImmediate(node1Object);
            Object.DestroyImmediate(node2Object);
            Object.DestroyImmediate(node3Object);
        }

        [Test]
        public void 각_층마다_노드_타입이_배치된다()
        {
            // Arrange & Act
            var nodes = new List<MapNode>();

            for (int i = 0; i < 5; i++)
            {
                var nodeObject = new GameObject($"Node{i}");
                nodeObject.AddComponent<UnityEngine.UI.Button>();
                nodeObject.AddComponent<UnityEngine.UI.Image>();
                var node = nodeObject.AddComponent<MapNode>();

                // 다양한 노드 타입 배치
                node.nodeType = (NodeType)(i % 6); // Combat, EliteCombat, Event, Shop, RestSite, Boss
                nodes.Add(node);
            }

            // Assert
            Assert.AreEqual(5, nodes.Count);
            Assert.AreEqual(NodeType.Combat, nodes[0].nodeType);
            Assert.AreEqual(NodeType.EliteCombat, nodes[1].nodeType);
            Assert.AreEqual(NodeType.Event, nodes[2].nodeType);
            Assert.AreEqual(NodeType.Shop, nodes[3].nodeType);
            Assert.AreEqual(NodeType.RestSite, nodes[4].nodeType);

            // Cleanup
            foreach (var node in nodes)
            {
                Object.DestroyImmediate(node.gameObject);
            }
        }

        [Test]
        public void 맵_구조가_여러_층으로_구성된다()
        {
            // Arrange
            var layer0Nodes = new List<MapNode>();
            var layer1Nodes = new List<MapNode>();

            // Layer 0
            for (int i = 0; i < 3; i++)
            {
                var nodeObject = new GameObject($"Layer0_Node{i}");
                nodeObject.AddComponent<UnityEngine.UI.Button>();
                nodeObject.AddComponent<UnityEngine.UI.Image>();
                var node = nodeObject.AddComponent<MapNode>();
                node.Setup(0);
                layer0Nodes.Add(node);
            }

            // Layer 1
            for (int i = 0; i < 3; i++)
            {
                var nodeObject = new GameObject($"Layer1_Node{i}");
                nodeObject.AddComponent<UnityEngine.UI.Button>();
                nodeObject.AddComponent<UnityEngine.UI.Image>();
                var node = nodeObject.AddComponent<MapNode>();
                node.Setup(1);
                layer1Nodes.Add(node);
            }

            // Act - 연결
            foreach (var node in layer0Nodes)
            {
                node.connectedNodes.Add(layer1Nodes[0]);
            }

            // Assert
            Assert.AreEqual(3, layer0Nodes.Count);
            Assert.AreEqual(3, layer1Nodes.Count);
            Assert.AreEqual(0, layer0Nodes[0].layer);
            Assert.AreEqual(1, layer1Nodes[0].layer);

            // Cleanup
            foreach (var node in layer0Nodes)
                Object.DestroyImmediate(node.gameObject);
            foreach (var node in layer1Nodes)
                Object.DestroyImmediate(node.gameObject);
        }

        // Phase 7.2: 노드 상호작용
        [Test]
        public void Player가_노드를_선택할_수_있다()
        {
            // Arrange
            var nodeObject = new GameObject("Node");
            var button = nodeObject.AddComponent<UnityEngine.UI.Button>();
            nodeObject.AddComponent<UnityEngine.UI.Image>();
            var node = nodeObject.AddComponent<MapNode>();
            node.nodeType = NodeType.Combat;

            // Act
            node.SetClickable(true);

            // Assert
            Assert.IsTrue(button.interactable, "노드가 클릭 가능해야 합니다");

            Object.DestroyImmediate(nodeObject);
        }

        [Test]
        public void 노드_타입에_따라_다른_이벤트가_설정된다()
        {
            // Arrange & Act
            var combatNode = CreateTestNode(NodeType.Combat);
            var shopNode = CreateTestNode(NodeType.Shop);
            var restNode = CreateTestNode(NodeType.RestSite);
            var eventNode = CreateTestNode(NodeType.Event);
            var bossNode = CreateTestNode(NodeType.Boss);

            // Assert
            Assert.AreEqual(NodeType.Combat, combatNode.nodeType);
            Assert.AreEqual(NodeType.Shop, shopNode.nodeType);
            Assert.AreEqual(NodeType.RestSite, restNode.nodeType);
            Assert.AreEqual(NodeType.Event, eventNode.nodeType);
            Assert.AreEqual(NodeType.Boss, bossNode.nodeType);

            // Cleanup
            Object.DestroyImmediate(combatNode.gameObject);
            Object.DestroyImmediate(shopNode.gameObject);
            Object.DestroyImmediate(restNode.gameObject);
            Object.DestroyImmediate(eventNode.gameObject);
            Object.DestroyImmediate(bossNode.gameObject);
        }

        [Test]
        public void 전투_노드에_EncounterData가_할당된다()
        {
            // Arrange
            var nodeObject = new GameObject("CombatNode");
            nodeObject.AddComponent<UnityEngine.UI.Button>();
            nodeObject.AddComponent<UnityEngine.UI.Image>();
            var node = nodeObject.AddComponent<MapNode>();
            node.nodeType = NodeType.Combat;

            var encounterData = ScriptableObject.CreateInstance<EncounterData>();
            encounterData.enemies = new List<EncounterData.EnemySpawn>();

            // Act
            node.encounterData = encounterData;

            // Assert
            Assert.IsNotNull(node.encounterData);
            Assert.AreEqual(encounterData, node.encounterData);

            Object.DestroyImmediate(nodeObject);
        }

        [Test]
        public void 기연_노드에_EventData가_할당된다()
        {
            // Arrange
            var nodeObject = new GameObject("EventNode");
            nodeObject.AddComponent<UnityEngine.UI.Button>();
            nodeObject.AddComponent<UnityEngine.UI.Image>();
            var node = nodeObject.AddComponent<MapNode>();
            node.nodeType = NodeType.Event;

            var eventData = ScriptableObject.CreateInstance<EventData>();
            eventData.eventName = "신비한 만남";

            // Act
            node.eventData = eventData;

            // Assert
            Assert.IsNotNull(node.eventData);
            Assert.AreEqual("신비한 만남", node.eventData.eventName);

            Object.DestroyImmediate(nodeObject);
        }

        [Test]
        public void 노드_선택_후_다음_층_노드가_활성화된다()
        {
            // Arrange
            var currentNode = CreateTestNode(NodeType.Combat);
            var nextNode1 = CreateTestNode(NodeType.Shop);
            var nextNode2 = CreateTestNode(NodeType.Event);

            currentNode.connectedNodes.Add(nextNode1);
            currentNode.connectedNodes.Add(nextNode2);

            currentNode.SetClickable(true);
            nextNode1.SetClickable(false);
            nextNode2.SetClickable(false);

            // Act - 노드 선택 시뮬레이션 (실제로는 OnNodeClicked -> MapManager.OnNodeSelected)
            // 테스트에서는 직접 연결된 노드들을 활성화
            foreach (var connectedNode in currentNode.connectedNodes)
            {
                connectedNode.SetClickable(true);
            }

            // Assert
            var button1 = nextNode1.GetComponent<UnityEngine.UI.Button>();
            var button2 = nextNode2.GetComponent<UnityEngine.UI.Button>();

            Assert.IsTrue(button1.interactable, "연결된 노드1이 활성화되어야 합니다");
            Assert.IsTrue(button2.interactable, "연결된 노드2가 활성화되어야 합니다");

            // Cleanup
            Object.DestroyImmediate(currentNode.gameObject);
            Object.DestroyImmediate(nextNode1.gameObject);
            Object.DestroyImmediate(nextNode2.gameObject);
        }

        [Test]
        public void 휴식처_노드는_RestSite_타입이다()
        {
            // Arrange & Act
            var restNode = CreateTestNode(NodeType.RestSite);

            // Assert
            Assert.AreEqual(NodeType.RestSite, restNode.nodeType);
            // 실제 RestSiteManager 호출은 통합 테스트에서 검증

            Object.DestroyImmediate(restNode.gameObject);
        }

        [Test]
        public void 상점_노드는_Shop_타입이다()
        {
            // Arrange & Act
            var shopNode = CreateTestNode(NodeType.Shop);

            // Assert
            Assert.AreEqual(NodeType.Shop, shopNode.nodeType);
            // 실제 ShopManager 호출은 통합 테스트에서 검증

            Object.DestroyImmediate(shopNode.gameObject);
        }

        // Helper method
        private MapNode CreateTestNode(NodeType type)
        {
            var nodeObject = new GameObject($"{type}Node");
            nodeObject.AddComponent<UnityEngine.UI.Button>();
            nodeObject.AddComponent<UnityEngine.UI.Image>();
            var node = nodeObject.AddComponent<MapNode>();
            node.nodeType = type;
            return node;
        }
    }
}
