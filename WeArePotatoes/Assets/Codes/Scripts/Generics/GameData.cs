using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public int SeedProductionLevel = 1;
    public int BaseHealthLevel = 1;
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
    public List<UnitHero> SelectedUnitList = new List<UnitHero>();
    public List<int> CompletedLevelList = new List<int>();
    public int Coin = 0;
    public int SelectedLevel;
}
