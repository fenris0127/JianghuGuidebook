using System.Collections.Generic;
using UnityEngine;

namespace GangHoBiGeup.Data
{
    // 유물의 희귀도를 정의합니다.
    public enum RelicRarity 
    { 
        Common, 
        Uncommon, 
        Rare,  
        Epic,
        Legendary,
        Boss 
    }

    public enum RelicTrigger 
    { 
        OnPickup, 
        OnCombatStart, 
        OnTurnStart, 
        OnCardPlayed, 
        OnTakingDamage 
    }

    // 하나의 유물에 대한 모든 정보를 담고 있는 ScriptableObject 애셋입니다.
    [CreateAssetMenu(fileName = "New Relic", menuName = "Game/RelicData")]
    public class RelicData : ScriptableObject
    {
        // TDD 테스트용 속성
        public string id;
        
        // 기존 속성
        public string assetID;
        public string relicName;
        [TextArea] public string description;
        public Sprite icon;
        public RelicRarity rarity;
        
        [Header("효과 - TDD")]
        public RelicTriggerType triggerType;
        public RelicEffectType effectType;
        public int effectValue;
        
        [Header("효과 - 기존")]
        public RelicTrigger trigger;
        [Tooltip("해당 시점에 발동될 모든 효과의 목록입니다.")]
        public List<GameEffect> effects;
    }
}