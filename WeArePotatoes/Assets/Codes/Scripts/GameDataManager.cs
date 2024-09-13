using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

[DefaultExecutionOrder(-99999999)]
public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public event Action<int> OnCoinUpdated;
    public event Action<List<UnitHero>> OnSelectedUnitListChanged;
    public List<UnitHero> SelectedUnitList = new List<UnitHero>(3);
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
    public List<int> CompletedLevelList = new List<int>();
    public int SelectedLevelIndex = 0;

    public new void Awake()
    {
        base.Awake();

        int coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(coin);

        SelectedUnitList = Data.Get<GameData>().SelectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);

        UnlockedUnitList = Data.Get<GameData>().UnlockedUnitList;
        CompletedLevelList = Data.Get<GameData>().CompletedLevelList;
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

    public void SetSelectedLevel(int levelIndex)
    {
        SelectedLevelIndex = levelIndex;
    }

    public void AddNewCompletedLevel(int levelIndex)
    {
        CompletedLevelList.Add(levelIndex);
        Data.Get<GameData>().CompletedLevelList = CompletedLevelList;
        Save();
    }
}
