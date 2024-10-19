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
    [SerializeField] private Button battleButton;
    [SerializeField] private Button potatoSelectionButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cardButton;
    [SerializeField] private Button quitButton;

    [Header("Main Menu")]
    [SerializeField] private MainMenuUI mainMenuUI;

    [Header("Main Menu")]
    [SerializeField] private TMP_Text coinText;

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

    [SerializeField] private CanvasGroup fader;

    public new void Awake()
    {
        base.Awake();

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
            ToggleLevelSelectionMenu();
        });
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        HandleCoinUpdate(GameDataManager.Instance.Coin);

        if (GameDataManager.Instance.IsThereNewPotato)
        {
            newPotatoLabel.transform.gameObject.SetActive(true);
        }
        else
        {
            newPotatoLabel.transform.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnCoinUpdated += HandleCoinUpdate;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnCoinUpdated -= HandleCoinUpdate;
    }

    private void HandleCoinUpdate(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void ToggleUpgradeMenu()
    {
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
        isLevelSelectionMenuOpen = !isLevelSelectionMenuOpen;

        if (isLevelSelectionMenuOpen)
            OpenLevelSelectionMenu();
        else
            CloseLevelSelectionMenu();
    }

    private void ToggleCardMenu()
    {
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();

        isCardMenuOpen = !isCardMenuOpen;

        if (isCardMenuOpen)
            OpenCardMenu();
        else
            CloseCardMenu();
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

    private void OpenLevelSelectionMenu()
    {
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            mainMenuUI.Hide();
            levelSelectionUI.Show();
            SetLevelUIState(false);

            fader.DOFade(0, 0.1f);
        });
    }

    private void CloseLevelSelectionMenu()
    {
        if (isPotatoSelectionMenuOpen) TogglePotatoSelectionMenu();
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        if (isCardMenuOpen) ToggleCardMenu();

        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            cardUI.Hide();
            mainMenuUI.Show();
            SetLevelUIState(true);

            fader.DOFade(0, 0.1f);
        });
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
        Application.Quit();
    }
}
