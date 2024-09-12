using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public List<string> UnlockedUnitList = new List<string>();
    public List<UnitHero> SelectedUnitList = new List<UnitHero>();
    public int Coin = 0;
    public int SelectedLevel;
}
