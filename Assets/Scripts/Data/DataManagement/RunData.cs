using System;
using System.Collections.Generic;

// '이어하기' 기능을 위해, 현재 진행 중인 한 판(Run)의 모든 상태를 저장하는 클래스입니다.
[Serializable]
public class RunData
{
    public int currentFloor;

    public int playerMaxHealth;
    public int playerCurrentHealth;
    public int playerGold;
    public List<string> relicIDs;

    public List<string> drawPileIDs;
    public List<string> discardPileIDs;
    public List<string> handIDs;
    public List<string> exhaustPileIDs;

    public Realm currentRealm;
    public int currentXp;
    public int xpToNextRealm;
    public SwordRealm currentSwordRealm;
}