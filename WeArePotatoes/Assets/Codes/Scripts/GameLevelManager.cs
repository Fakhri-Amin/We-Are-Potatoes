using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class GameLevelManager : Singleton<GameLevelManager>
{
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;
    public int CoinCollected = 0;

    private void Start()
    {
        winUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelLose);
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelLose);
    }

    public void HandleLevelWin()
    {
        winUI.gameObject.SetActive(true);
        winUI.DisplayWinUI();
        GameDataManager.Instance.ModifyMoney(CoinCollected);
        // Time.timeScale = 0;
    }

    public void HandleLevelLose()
    {
        loseUI.gameObject.SetActive(true);
        loseUI.DisplayLoseUI();
        GameDataManager.Instance.ModifyMoney(CoinCollected);
        // Time.timeScale = 0;
    }

    public void AddCoinCollected(int amount)
    {
        CoinCollected += amount;
    }
}
