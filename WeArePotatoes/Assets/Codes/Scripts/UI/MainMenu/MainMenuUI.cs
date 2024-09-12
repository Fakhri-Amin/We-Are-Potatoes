using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button battleButton;
    [SerializeField] private Button potatoSelectionButton;
    [SerializeField] private Button upgradeButton;

    [Header("Potatoes UI")]
    [SerializeField] private UnitSelectionUI unitSelectionUI;
    [SerializeField] private Image potatoIcon;
    [SerializeField] private TMP_Text potatoText;
    [SerializeField] private Image potatoCloseIcon;
    [SerializeField] private TMP_Text potatoCloseText;
    private bool isPotatoSelectionMenuOpen;

    private void Awake()
    {
        potatoSelectionButton.onClick.AddListener(TogglePotatoSelectionMenu);
    }

    private void TogglePotatoSelectionMenu()
    {
        isPotatoSelectionMenuOpen = !isPotatoSelectionMenuOpen;

        if (isPotatoSelectionMenuOpen)
            OpenPotatoSelectionMenu();
        else
            ClosePotatoSelectionMenu();
    }

    private void OpenPotatoSelectionMenu()
    {
        unitSelectionUI.Show();
        unitSelectionUI.Initialize(GameDataManager.Instance.SelectedUnitHeroList);
        SetPotatoUIState(false);
    }

    private void ClosePotatoSelectionMenu()
    {
        unitSelectionUI.Hide();
        SetPotatoUIState(true);
    }

    private void SetPotatoUIState(bool isSelectionClosed)
    {
        potatoIcon.gameObject.SetActive(isSelectionClosed);
        potatoText.gameObject.SetActive(isSelectionClosed);
        potatoCloseIcon.gameObject.SetActive(!isSelectionClosed);
        potatoCloseText.gameObject.SetActive(!isSelectionClosed);
    }
}

