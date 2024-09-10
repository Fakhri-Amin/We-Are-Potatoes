using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Transform popup;

    private void Start()
    {
        popup.gameObject.SetActive(false);
    }

    public void DisplayWinUI()
    {
        popup.gameObject.SetActive(true);

        var coin = GameLevelManager.Instance.CoinCollected;
        coinCollectedText.text = "+" + coin;
    }
}
