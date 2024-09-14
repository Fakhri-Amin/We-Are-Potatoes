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

    private GameDataManager gameDataManager;

    private void Start()
    {
        gameDataManager = GameDataManager.Instance;
        HandleUpdateSeedUI(gameDataManager.SeedProductionRate, gameDataManager.UpgradeSeedProductionRatePrice);
        HandleUpdateBaseHealth(gameDataManager.BaseHealth, gameDataManager.UpgradeBaseHealthPrice);

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

    public void HandleUpdateSeedUI(float rate, float price)
    {
        currentSeedRateText.text = rate + "/s";
        upgradeSeedRatePriceText.text = price.ToString();
    }

    public void HandleUpdateBaseHealth(float health, float price)
    {
        currentBaseHealthText.text = health.ToString();
        upgradeBaseHealthPriceText.text = price.ToString();
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
