using System;

// 게임 세션을 넘어 영구적으로 저장될 플레이어의 성장 데이터를 담는 클래스입니다.
[Serializable]
public class MetaProgress
{
    public int enlightenmentPoints;
    public int bonusHealthLevel;
    public int startingGoldLevel;
}