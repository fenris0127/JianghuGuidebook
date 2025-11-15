using UnityEngine;

public enum StatusEffectType
{
    Poison, 
    Vulnerable, 
    Weak, 
    Strength, 
    Thorns
}

[CreateAssetMenu(fileName = "New StatusEffectData", menuName = "Game/Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    [Header("고유 식별자")]
    [Tooltip("ResourceManager에서 이 데이터를 찾기 위한 고유 ID입니다. (예: Strength, Poison)")]
    public string assetID;

    public StatusEffectType type;
    public string effectName;
    [TextArea] public string description;
    public Sprite icon;
    public bool isBuff; // 버프인지 디버프인지
}