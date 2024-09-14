using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    [Header("Seed Production Rate")]
    [SerializeField] private TMP_Text currentSeedRateText;
    [SerializeField] private Button upgradeSeedRateButton;
    [SerializeField] private TMP_Text upgradeSeedRatePriceText;

    [Header("Base Health")]
    [SerializeField] private TMP_Text currentBaseHealthText;
    [SerializeField] private Button upgradeBaseHealthButton;
    [SerializeField] private TMP_Text upgradeBaseHealthPriceText;

    private GameDataManager gameDataManager;

    private void Awake()
    {
        upgradeSeedRateButton.onClick.AddListener(UpgradeSeedProductionRate);
        upgradeBaseHealthButton.onClick.AddListener(UpgradeBaseHealth);
    }

    private void Start()
    {
        gameDataManager = GameDataManager.Instance;
        HandleUpdateSeedUI(gameDataManager.SeedProductionRate, gameDataManager.UpgradeSeedProductionRatePrice);
        HandleUpdateBaseHealth(gameDataManager.BaseHealth, gameDataManager.UpgradeBaseHealthPrice);

        // Hide();
        panel.gameObject.SetActive(false);
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

    private void UpgradeSeedProductionRate()
    {
        var gameDataManager = GameDataManager.Instance;
        if (gameDataManager.Coin >= gameDataManager.UpgradeSeedProductionRatePrice)
        {
            gameDataManager.ModifyMoney(-gameDataManager.UpgradeSeedProductionRatePrice);
            gameDataManager.UpgradeSeedProductionRate();
        }
    }

    private void UpgradeBaseHealth()
    {
        var gameDataManager = GameDataManager.Instance;
        if (gameDataManager.Coin >= gameDataManager.UpgradeBaseHealthPrice)
        {
            gameDataManager.ModifyMoney(-gameDataManager.UpgradeBaseHealthPrice);
            gameDataManager.UpgradeBaseHealth();
        }
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f);
    }

    public void Hide()
    {
        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
