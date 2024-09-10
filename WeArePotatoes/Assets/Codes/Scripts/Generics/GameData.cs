using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public List<string> UnlockedUnitList = new List<string>();
    public int Coin = 0;
}
