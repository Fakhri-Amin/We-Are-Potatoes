using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Farou.Utility;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using DG.Tweening;

public class MainHUD : Singleton<MainHUD>
{
    [Header("Buttons")]
    [SerializeField] private Button battleButton;
    [SerializeField] private Button baseButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button potatoSelectionButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cardButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    [Header("Main Menu")]
    [SerializeField] private MainMenuUI mainMenuUI;

    [Header("Currency")]
    [SerializeField] private TMP_Text goldCoinText;
    [SerializeField] private TMP_Text azureCoinText;

    [Header("Upgrade UI")]
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private Image upgradeCloseIcon;
    [SerializeField] private TMP_Text upgradeCloseText;
    public bool isUpgradeMenuOpen;

    [Header("Unit Selection")]
    [SerializeField] private UnitSelectionUI unitSelectionUI;
    [SerializeField] private Image potatoIcon;
    [SerializeField] private TMP_Text potatoText;
    [SerializeField] private Image potatoCloseIcon;
    [SerializeField] private TMP_Text potatoCloseText;
    [SerializeField] private Transform newPotatoLabel;
    public bool isPotatoSelectionMenuOpen;

    [Header("Level Selection UI")]
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    [SerializeField] private Image battleIcon;
    [SerializeField] private TMP_Text battleText;
    [SerializeField] private Image baseIcon;
    [SerializeField] private TMP_Text baseText;
    public bool isLevelSelectionMenuOpen;

    [Header("Card UI")]
    [SerializeField] private CardUI cardUI;
    [SerializeField] private Image cardIcon;
    [SerializeField] private TMP_Text cardText;
    [SerializeField] private Image cardCloseIcon;
    [SerializeField] private TMP_Text cardCloseText;
    public bool isCardMenuOpen;

    [Header("Shop UI")]
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private Image shopIcon;
    [SerializeField] private TMP_Text shopText;
    [SerializeField] private Image shopCloseIcon;
    [SerializeField] private TMP_Text shopCloseText;
    public bool isShopMenuOpen;


    [Header("Setting UI")]
    [SerializeField] private SettingUI settingUI;

    [Header("Quit UI")]
    [SerializeField] private QuitConfirmationUI quitConfirmationUI;

    [Header("Others")]
    [SerializeField] private CanvasGroup fader;

    public new void Awake()
    {
        base.Awake();

        shopButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            ToggleShopMenu();
        });
        upgradeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            ToggleUpgradeMenu();
        });
        potatoSelectionButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            TogglePotatoSelectionMenu();

            // Disable new potato label
            if (GameDataManager.Instance.IsThereNewPotato)
            {
                newPotatoLabel.transform.gameObject.SetActive(false);
                GameDataManager.Instance.SetNewPotatoStatus(false);
            }
        });
        cardButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            ToggleCardMenu();
        });
        battleButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            OpenLevelSelectionMenu();
        });
        baseButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            CloseLevelSelectionMenu();
        });
        quitButton.onClick.AddListener(QuitGame);
        settingButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            OpenSettingMenu();
        });
    }

    private void Start()
    {
        HandleCoinUpdate(GameDataManager.Instance.GoldCoin);
        HandleAzureUpdate(GameDataManager.Instance.AzureCoin);

        if (GameDataManager.Instance.IsThereNewPotato)
        {
            newPotatoLabel.transform.gameObject.SetActive(true);
        }
        else
        {
            newPotatoLabel.transform.gameObject.SetActive(false);
        }

        baseButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated += HandleCoinUpdate;
        GameDataManager.Instance.OnAzureCoinUpdated += HandleAzureUpdate;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated -= HandleCoinUpdate;
        GameDataManager.Instance.OnAzureCoinUpdated -= HandleAzureUpdate;
    }

    private void HandleCoinUpdate(int coin)
    {
        if (coin >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            goldCoinText.text = (coin / 1000000f).ToString("0.#") + "M";
        }
        else if (coin >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            goldCoinText.text = (coin / 1000f).ToString("0") + "K";
        }
        else if (coin >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 12.3K for 12,300)
            goldCoinText.text = (coin / 1000f).ToString("0.#") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            goldCoinText.text = coin.ToString();
        }
    }

    private void HandleAzureUpdate(int coin)
    {
        if (coin >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            azureCoinText.text = (coin / 1000000f).ToString("0.#") + "M";
        }
        else if (coin >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            azureCoinText.text = (coin / 1000f).ToString("0") + "K";
        }
        else if (coin >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 12.3K for 12,300)
            azureCoinText.text = (coin / 1000f).ToString("0.#") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            azureCoinText.text = coin.ToString();
        }
    }

    private void ToggleShopMenu()
    {
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        isShopMenuOpen = !isShopMenuOpen;

        if (isShopMenuOpen)
            OpenShopMenu();
        else
            CloseShopMenu();
    }

    private void ToggleUpgradeMenu()
    {
        if (isShopMenuOpen) ToggleShopMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        isUpgradeMenuOpen = !isUpgradeMenuOpen;

        if (isUpgradeMenuOpen)
            OpenUpgradeMenu();
        else
            CloseUpgradeMenu();
    }

    private void TogglePotatoSelectionMenu()
    {
        if (isShopMenuOpen) ToggleShopMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        isPotatoSelectionMenuOpen = !isPotatoSelectionMenuOpen;

        if (isPotatoSelectionMenuOpen)
            OpenPotatoSelectionMenu();
        else
            ClosePotatoSelectionMenu();
    }

    private void ToggleLevelSelectionMenu()
    {
        if (isShopMenuOpen) ToggleShopMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        isLevelSelectionMenuOpen = !isLevelSelectionMenuOpen;

        if (isLevelSelectionMenuOpen)
            OpenLevelSelectionMenu();
        else
            CloseLevelSelectionMenu();
    }

    private void ToggleCardMenu()
    {
        if (isShopMenuOpen) ToggleShopMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();

        isCardMenuOpen = !isCardMenuOpen;

        if (isCardMenuOpen)
            OpenCardMenu();
        else
            CloseCardMenu();
    }

    private void OpenShopMenu()
    {
        shopUI.Show();
        SetShopUIState(false);
    }

    private void CloseShopMenu()
    {
        shopUI.Hide();
        SetShopUIState(true);
    }

    private void OpenUpgradeMenu()
    {
        upgradeUI.Show();
        SetUpgradeUIState(false);
    }

    private void CloseUpgradeMenu()
    {
        upgradeUI.Hide();
        SetUpgradeUIState(true);
    }

    private void OpenPotatoSelectionMenu()
    {
        unitSelectionUI.Show();
        unitSelectionUI.Initialize();
        SetPotatoUIState(false);
    }

    private void ClosePotatoSelectionMenu()
    {
        unitSelectionUI.Hide();
        SetPotatoUIState(true);
    }

    private void OpenCardMenu()
    {
        cardUI.Show();
        cardUI.Initialize();
        SetCardUIState(false);
    }

    private void CloseCardMenu()
    {
        cardUI.Hide();
        SetCardUIState(true);
    }

    private void OpenSettingMenu()
    {
        settingUI.Show();
    }

    private void OpenLevelSelectionMenu()
    {
        if (isShopMenuOpen) ToggleShopMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            mainMenuUI.Hide();
            levelSelectionUI.Show();

            battleButton.gameObject.SetActive(false);
            baseButton.gameObject.SetActive(true);
            settingButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            fader.DOFade(0, 0.1f);
        });

    }

    private void CloseLevelSelectionMenu()
    {
        if (isShopMenuOpen) ToggleShopMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            levelSelectionUI.Hide();
            mainMenuUI.Show();

            battleButton.gameObject.SetActive(true);
            baseButton.gameObject.SetActive(false);
            settingButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);

            fader.DOFade(0, 0.1f);
        });

    }

    private void SetShopUIState(bool isShopMenuClosed)
    {
        shopIcon.gameObject.SetActive(isShopMenuClosed);
        shopText.gameObject.SetActive(isShopMenuClosed);
        shopCloseIcon.gameObject.SetActive(!isShopMenuClosed);
        shopCloseText.gameObject.SetActive(!isShopMenuClosed);
    }

    private void SetUpgradeUIState(bool isSelectionClosed)
    {
        upgradeIcon.gameObject.SetActive(isSelectionClosed);
        upgradeText.gameObject.SetActive(isSelectionClosed);
        upgradeCloseIcon.gameObject.SetActive(!isSelectionClosed);
        upgradeCloseText.gameObject.SetActive(!isSelectionClosed);
    }

    private void SetPotatoUIState(bool isSelectionClosed)
    {
        potatoIcon.gameObject.SetActive(isSelectionClosed);
        potatoText.gameObject.SetActive(isSelectionClosed);
        potatoCloseIcon.gameObject.SetActive(!isSelectionClosed);
        potatoCloseText.gameObject.SetActive(!isSelectionClosed);
    }

    private void SetCardUIState(bool isCardClosed)
    {
        cardIcon.gameObject.SetActive(isCardClosed);
        cardText.gameObject.SetActive(isCardClosed);
        cardCloseIcon.gameObject.SetActive(!isCardClosed);
        cardCloseText.gameObject.SetActive(!isCardClosed);
    }

    private void SetLevelUIState(bool isSelectionClosed)
    {
        battleIcon.gameObject.SetActive(isSelectionClosed);
        battleText.gameObject.SetActive(isSelectionClosed);
        baseIcon.gameObject.SetActive(!isSelectionClosed);
        baseText.gameObject.SetActive(!isSelectionClosed);
    }

    private void QuitGame()
    {
        quitConfirmationUI.Show();
    }
}
