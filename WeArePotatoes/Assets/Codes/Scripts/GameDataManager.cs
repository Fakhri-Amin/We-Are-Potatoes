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
    public event Action<List<UnitHero>> OnSelectedUnitListChanged;
    public List<UnitHero> SelectedUnitList = new List<UnitHero>(3);
    public List<UnitHero> UnlockedUnitlist = new List<UnitHero>();

    public new void Awake()
    {
        base.Awake();

        int coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(coin);

        SelectedUnitList = Data.Get<GameData>().SelectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);

        UnlockedUnitlist = Data.Get<GameData>().UnlockedUnitList;
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
        Data.Get<GameData>().UnlockedUnitList.Add(unitHero);
        Save();
    }

    public void SetSelectedUnit(List<UnitHero> selectedUnitList)
    {
        SelectedUnitList = selectedUnitList;
        Data.Get<GameData>().SelectedUnitList = selectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);
        Save();
    }
}
