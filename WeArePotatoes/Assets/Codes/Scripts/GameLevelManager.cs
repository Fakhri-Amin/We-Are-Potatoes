using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelManager : Singleton<GameLevelManager>
{
    [Header("General UI")]
    public int CoinCollected = 0;
    [SerializeField] private CoinCollectedUI coinCollectedUI;

    [Header("Game End UI")]
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;

    [Header("Pause UI")]
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private Button pauseButton;

    public new void Awake()
    {
        base.Awake();

        pauseButton.onClick.AddListener(() =>
        {
            pauseUI.Show();
            Time.timeScale = 0;
        });
    }

    private void Start()
    {
        winUI.Hide();
        loseUI.Hide();

        pauseUI.InitializeButtonFunction(() =>
        {
            Time.timeScale = 1;
            pauseUI.Hide();
        });
        pauseUI.Hide();

        coinCollectedUI.UpdateCoinCollectedUI(CoinCollected);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelLose);
        EventManager.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelLose);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
    }

    private void HandleLevelWin()
    {
        winUI.gameObject.SetActive(true);
        coinCollectedUI.gameObject.SetActive(false);
        winUI.Show(CoinCollected);
        GameDataManager.Instance.ModifyMoney(CoinCollected);
        // Time.timeScale = 0;
    }

    private void HandleLevelLose()
    {
        loseUI.gameObject.SetActive(true);
        coinCollectedUI.gameObject.SetActive(false);
        loseUI.Show(CoinCollected);
        GameDataManager.Instance.ModifyMoney(CoinCollected);
        // Time.timeScale = 0;
    }

    private void HandleEnemyCoinDropped()
    {
        AddCoinCollected(100);
        coinCollectedUI.UpdateCoinCollectedUI(CoinCollected);
    }

    private void AddCoinCollected(int amount)
    {
        CoinCollected += amount;
    }
}
