using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Farou.Utility;
using Sirenix.OdinInspector;

public class MainHUD : Singleton<MainHUD>
{
    [SerializeField] private Button battleButton;
    [SerializeField] private Button potatoSelectionButton;
    [SerializeField] private Button upgradeButton;

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
    public bool isPotatoSelectionMenuOpen;

    [Header("Level Selection UI")]
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    [SerializeField] private Image battleIcon;
    [SerializeField] private TMP_Text battleText;
    [SerializeField] private Image baseIcon;
    [SerializeField] private TMP_Text baseText;
    public bool isLevelSelectionMenuOpen;

    public new void Awake()
    {
        base.Awake();

        upgradeButton.onClick.AddListener(ToggleUpgradeMenu);
        potatoSelectionButton.onClick.AddListener(TogglePotatoSelectionMenu);
        battleButton.onClick.AddListener(ToggleLevelSelectionMenu);
    }

    private void Start()
    {
        HandleCoinUpdate(GameDataManager.Instance.Coin);
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

        isUpgradeMenuOpen = !isUpgradeMenuOpen;

        if (isUpgradeMenuOpen)
            OpenUpgradeMenu();
        else
            CloseUpgradeMenu();
    }

    private void TogglePotatoSelectionMenu()
    {
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();

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
        unitSelectionUI.Initialize(GameDataManager.Instance.SelectedUnitList, GameDataManager.Instance.UnlockedUnitList);
        SetPotatoUIState(false);
    }

    private void ClosePotatoSelectionMenu()
    {
        unitSelectionUI.Hide();
        SetPotatoUIState(true);
    }

    private void OpenLevelSelectionMenu()
    {
        mainMenuUI.Hide();
        levelSelectionUI.Show();
        SetLevelUIState(false);
    }

    private void CloseLevelSelectionMenu()
    {
        levelSelectionUI.Hide();
        mainMenuUI.Show();
        SetLevelUIState(true);
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

    private void SetLevelUIState(bool isSelectionClosed)
    {
        battleIcon.gameObject.SetActive(isSelectionClosed);
        battleText.gameObject.SetActive(isSelectionClosed);
        baseIcon.gameObject.SetActive(!isSelectionClosed);
        baseText.gameObject.SetActive(!isSelectionClosed);
    }
}
