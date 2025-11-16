using UnityEngine;
using GangHoBiGeup.Data;
using GangHoBiGeup.Gameplay;
using System.Collections.Generic;

namespace GangHoBiGeup.Tests
{
    /// <summary>
    /// 테스트 코드의 중복을 제거하기 위한 Helper 클래스
    /// </summary>
    public static class TestHelper
    {
        #region GameObject 생성 및 관리

        /// <summary>
        /// GameObject를 생성하고 컴포넌트를 추가합니다
        /// </summary>
        public static T CreateGameObject<T>(string name = null) where T : Component
        {
            var gameObject = new GameObject(name ?? typeof(T).Name);
            return gameObject.AddComponent<T>();
        }

        /// <summary>
        /// 여러 GameObject를 한 번에 정리합니다
        /// </summary>
        public static void Cleanup(params Object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    if (obj is Component component)
                        Object.DestroyImmediate(component.gameObject);
                    else
                        Object.DestroyImmediate(obj);
                }
            }
        }

        #endregion

        #region Player 생성

        /// <summary>
        /// 기본 설정으로 Player를 생성합니다
        /// </summary>
        public static Player CreatePlayer(int maxHealth = 100, int currentHealth = 100, int maxNaegong = 3)
        {
            var player = CreateGameObject<Player>("Player");
            player.maxHealth = maxHealth;
            player.currentHealth = currentHealth;
            player.MaxNaegong = maxNaegong;
            return player;
        }

        #endregion

        #region Enemy 생성

        /// <summary>
        /// 기본 설정으로 Enemy를 생성합니다
        /// </summary>
        public static Enemy CreateEnemy(int maxHealth = 50, int currentHealth = 50)
        {
            var enemy = CreateGameObject<Enemy>("Enemy");
            enemy.maxHealth = maxHealth;
            enemy.currentHealth = currentHealth;
            return enemy;
        }

        #endregion

        #region ScriptableObject 생성

        /// <summary>
        /// CardData를 생성하고 기본 속성을 설정합니다
        /// </summary>
        public static CardData CreateCard(string id, string name, int cost = 1, params GameEffect[] effects)
        {
            var card = ScriptableObject.CreateInstance<CardData>();
            card.id = id;
            card.cardName = name;
            card.baseCost = cost;

            if (effects != null && effects.Length > 0)
            {
                card.effects = new List<GameEffect>(effects);
            }

            return card;
        }

        /// <summary>
        /// EnemyData를 생성합니다
        /// </summary>
        public static EnemyData CreateEnemyData(string id, string name, int maxHealth)
        {
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            enemyData.id = id;
            enemyData.enemyName = name;
            enemyData.maxHealth = maxHealth;
            return enemyData;
        }

        /// <summary>
        /// RelicData를 생성합니다
        /// </summary>
        public static RelicData CreateRelicData(string name, RelicTrigger trigger, params GameEffect[] effects)
        {
            var relic = ScriptableObject.CreateInstance<RelicData>();
            relic.relicName = name;
            relic.trigger = trigger;
            relic.effects = new List<GameEffect>(effects);
            return relic;
        }

        /// <summary>
        /// FactionData를 생성합니다
        /// </summary>
        public static FactionData CreateFactionData(string name, params CardData[] startingDeck)
        {
            var faction = ScriptableObject.CreateInstance<FactionData>();
            faction.factionName = name;
            faction.startingDeck = new List<CardData>(startingDeck);
            return faction;
        }

        #endregion

        #region GameEffect 생성

        /// <summary>
        /// GameEffect를 간단하게 생성합니다
        /// </summary>
        public static GameEffect CreateEffect(GameEffectType type, int value)
        {
            return new GameEffect { type = type, value = value };
        }

        /// <summary>
        /// Damage 효과를 생성합니다
        /// </summary>
        public static GameEffect Damage(int value) => CreateEffect(GameEffectType.Damage, value);

        /// <summary>
        /// Block 효과를 생성합니다
        /// </summary>
        public static GameEffect Block(int value) => CreateEffect(GameEffectType.Block, value);

        /// <summary>
        /// Energy 효과를 생성합니다
        /// </summary>
        public static GameEffect Energy(int value) => CreateEffect(GameEffectType.Energy, value);

        #endregion

        #region Reflection Helpers

        /// <summary>
        /// Awake 메서드를 리플렉션으로 호출합니다
        /// </summary>
        public static void InvokeAwake(Component component)
        {
            var awakeMethod = component.GetType().GetMethod("Awake",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod?.Invoke(component, null);
        }

        /// <summary>
        /// Start 메서드를 리플렉션으로 호출합니다
        /// </summary>
        public static void InvokeStart(Component component)
        {
            var startMethod = component.GetType().GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            startMethod?.Invoke(component, null);
        }

        /// <summary>
        /// private 메서드를 리플렉션으로 호출합니다
        /// </summary>
        public static object InvokePrivateMethod(object obj, string methodName, params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return method?.Invoke(obj, parameters);
        }

        #endregion

        #region StatusEffect 생성

        /// <summary>
        /// StatusEffect를 간단하게 생성합니다
        /// </summary>
        public static StatusEffect CreateStatusEffect(StatusEffectType type, int value, int duration)
        {
            return new StatusEffect(type, value, duration);
        }

        /// <summary>
        /// Poison 효과를 생성합니다
        /// </summary>
        public static StatusEffect Poison(int value, int duration) =>
            CreateStatusEffect(StatusEffectType.Poison, value, duration);

        /// <summary>
        /// Strength 효과를 생성합니다
        /// </summary>
        public static StatusEffect Strength(int value, int duration) =>
            CreateStatusEffect(StatusEffectType.Strength, value, duration);

        /// <summary>
        /// Vulnerable 효과를 생성합니다
        /// </summary>
        public static StatusEffect Vulnerable(int value, int duration) =>
            CreateStatusEffect(StatusEffectType.Vulnerable, value, duration);

        /// <summary>
        /// Weak 효과를 생성합니다
        /// </summary>
        public static StatusEffect Weak(int value, int duration) =>
            CreateStatusEffect(StatusEffectType.Weak, value, duration);

        #endregion
    }
}
