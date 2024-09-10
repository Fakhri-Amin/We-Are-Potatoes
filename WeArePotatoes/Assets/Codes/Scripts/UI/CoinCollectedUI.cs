using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using TMPro;
using UnityEngine;

public class CoinCollectedUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;

    public void UpdateCoinCollectedUI(int coinCollected)
    {
        coinCollectedText.text = coinCollected.ToString();
    }
}
