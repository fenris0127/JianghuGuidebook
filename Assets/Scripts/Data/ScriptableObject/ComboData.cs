using UnityEngine;
using System.Collections.Generic;

// 연계 초식의 발동 조건과 결과 효과를 정의하는 ScriptableObject 애셋입니다.
[CreateAssetMenu(fileName = "New ComboData", menuName = "Game/ComboData")]
public class ComboData : ScriptableObject
{
    public string comboName;
    [Tooltip("이 연계 초식을 발동시키기 위해 필요한 카드들의 assetID 순서입니다.")]
    public List<string> requiredCardIDs;
    [Tooltip("연계 초식 성공 시 발동될 추가 효과 목록입니다.")]
    public List<GameEffect> resultEffects;
}