using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DungeonLevelUI : MonoBehaviour
{
    [System.Serializable]
    public class DungeonLevelUIReference
    {
        public int LevelIndex;
        public Transform EntryTagTransform;
        public Button Button;
        public TMP_Text EntryText;
    }

    public DungeonLevelSO dungeonLevelSO;
    public List<DungeonLevelUIReference> dungeonLevelUIReferences = new List<DungeonLevelUIReference>();
    private GameData gameData;

    public void HandleDungeonLevelState()
    {
        HandleDateChecking();
    }

    private void HandleDateChecking()
    {
        // Check for date
        gameData = Data.Get<GameData>();
        DateTime lastDate = String.IsNullOrEmpty(gameData.LastOpenedDate) ? DateTime.Now.AddDays(-1) : DateTime.Parse(gameData.LastOpenedDate);

        if (DateTime.Now.Day > lastDate.Day)
        {
            gameData.LastOpenedDate = DateTime.Now.ToString();
            gameData.Save();

            GameDataManager.Instance.ClearClearedDailyDungeonLevel();
        }

        UpdateAllDungeonLevelsUI();
    }

    private void UpdateAllDungeonLevelsUI()
    {
        var gameDataManager = GameDataManager.Instance;

        foreach (var item in dungeonLevelUIReferences)
        {
            DungeonLevelData dungeonLevelData = gameDataManager.GetDungeonLevelData(item.LevelIndex);

            int entryLimit = dungeonLevelSO.DungeonLevelReferences.Find(i => i.LevelWaveSO.LevelIndex == item.LevelIndex).EntryLimit;

            if (dungeonLevelData == null)
            {
                // item.EntryText.text = $"{entryLimit}/{entryLimit}";
                continue;
            };

            if (dungeonLevelData.EntryCount < entryLimit)
            {
                item.Button.interactable = true;
            }
            else
            {
                item.Button.interactable = false;
            }
            item.EntryText.text = $"{entryLimit - dungeonLevelData.EntryCount}/{entryLimit}";
        }
    }

    private DateTime ParseDateTime(string dateTime)
    {
        string date = dateTime.Split("datetime")[1].Replace(":", "").Replace("\"", "").Split(",")[0].Split("T")[0];
        return DateTime.Parse($"{date}");
    }
}
