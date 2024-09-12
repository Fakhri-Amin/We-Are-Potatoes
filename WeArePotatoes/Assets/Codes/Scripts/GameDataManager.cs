using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

[DefaultExecutionOrder(-99999999)]
public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public UnitDataSO unitDataSO;
    public event Action<int> OnCoinUpdated;
    public List<UnitHero> SelectedUnitHeroList = new List<UnitHero>(3);

    public new void Awake()
    {
        base.Awake();

        int coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(coin);

        SelectedUnitHeroList = Data.Get<GameData>().SelectedUnitList;
    }

    public void Save()
    {
        Data.Save();
    }

    public void SelectLevel(int levelNumber)
    {
        Data.Get<GameData>().SelectedLevel = levelNumber;
        Save();
    }

    public void ModifyMoney(int amount)
    {
        Data.Get<GameData>().Coin += amount;
        OnCoinUpdated?.Invoke(Data.Get<GameData>().Coin);
        Save();
    }

    public void AddUnlockedUnit(UnitHero unitHero)
    {
        string unlockedUnit = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitHero).UnitHero.ToString();
        Data.Get<GameData>().UnlockedUnitList.Add(unlockedUnit);
        Save();
    }

    public void SetSelectedUnit(List<UnitHero> selectedUnitList)
    {
        SelectedUnitHeroList = selectedUnitList;
        Data.Get<GameData>().SelectedUnitList = selectedUnitList;
        Save();
    }
}
