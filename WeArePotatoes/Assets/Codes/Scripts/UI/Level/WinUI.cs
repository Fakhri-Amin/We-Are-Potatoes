using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Transform popup;

    public void Show(int coinCollectedAmount)
    {
        popup.gameObject.SetActive(true);

        coinCollectedText.text = "+" + coinCollectedAmount;
    }

    public void Hide()
    {
        popup.gameObject.SetActive(false);
    }
}
