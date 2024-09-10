using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Transform popup;

    private void Start()
    {
        popup.gameObject.SetActive(false);
    }

    public void DisplayLoseUI()
    {
        popup.gameObject.SetActive(true);

        var coin = GameLevelManager.Instance.CoinCollected;
        coinCollectedText.text = "+" + coin;
    }
}
