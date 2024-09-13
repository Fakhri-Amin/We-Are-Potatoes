using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Transform panel;
    [SerializeField] private TMP_Text currentSeedRateText;
    [SerializeField] private TMP_Text currentBaseHealthText;
    [SerializeField] private TMP_Text upgradeSeedRatePriceText;
    [SerializeField] private TMP_Text upgradeBaseHealthPriceText;

    private void Start()
    {
        HandleUpdateSeedUI(GameDataManager.Instance.SeedProductionRate);
        HandleUpdateBaseHealth(GameDataManager.Instance.BaseHealth);

        Hide();
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnSeedProductionRateChanged += HandleUpdateSeedUI;
        GameDataManager.Instance.OnBaseHealthChanged += HandleUpdateBaseHealth;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnSeedProductionRateChanged -= HandleUpdateSeedUI;
        GameDataManager.Instance.OnBaseHealthChanged -= HandleUpdateBaseHealth;
    }

    public void HandleUpdateSeedUI(float rate)
    {
        currentSeedRateText.text = rate + "/s";
    }

    public void HandleUpdateBaseHealth(int health)
    {
        currentBaseHealthText.text = health.ToString();
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
