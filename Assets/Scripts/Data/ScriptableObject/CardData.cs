using System.Collections.Generic;
using UnityEngine;

namespace GangHoBiGeup.Data
{
    /// <summary>
    /// 카드 데이터를 저장하는 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "New Card", menuName = "GangHoBiGeup/Card Data")]
    public class CardData : ScriptableObject
    {
        [Header("기본 속성")]
        public string id;
        public string cardName;
        public int cost;
        public CardRarity rarity;
        public Faction faction;
        
        [Header("효과")]
        public List<CardEffect> effects = new List<CardEffect>();
    }
}
