using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public UnitDataSO unitDataSO;
    public event Action<int> OnCoinUpdated;

    private void Start()
    {
        int coin = Data.Get<GameData>().Coin;
        OnCoinUpdated?.Invoke(coin);
    }

    public void Save()
    {
        Data.Save();
    }

    public void ModifyMoney(int amount)
    {
        Data.Get<GameData>().Coin += amount;
        OnCoinUpdated?.Invoke(Data.Get<GameData>().Coin);
        Data.Save<GameData>();
    }

    public void AddUnlockedUnit(UnitHero unitHero)
    {
        string unlockedUnit = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitHero).UnitHero.ToString();
        Data.Get<GameData>().UnlockedUnitList.Add(unlockedUnit);
        Data.Save<GameData>();
    }
}
