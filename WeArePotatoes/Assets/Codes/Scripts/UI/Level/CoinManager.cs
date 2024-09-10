using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int CoinCollected { get; private set; }
    [SerializeField] private CoinCollectedUI coinCollectedUI;

    public void AddCoins(int amount)
    {
        CoinCollected += amount;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        coinCollectedUI.UpdateCoinCollectedUI(CoinCollected);
    }
}

