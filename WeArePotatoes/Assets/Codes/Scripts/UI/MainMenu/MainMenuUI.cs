using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Project References")]
    [SerializeField] private UnitDataSO unitDataSO;

    [SerializeField] private Button battleButton;
    [SerializeField] private Button potatoSelectionButton;
    [SerializeField] private Button upgradeButton;

    [Header("Selected Potato UI")]
    [SerializeField] private Image[] selectedPotatoImageArray;

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

    private void Start()
    {
        HandleSelectedUnitListChanged(GameDataManager.Instance.SelectedUnitHeroList);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnSelectedUnitListChanged += HandleSelectedUnitListChanged;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnSelectedUnitListChanged -= HandleSelectedUnitListChanged;
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

    private void HandleSelectedUnitListChanged(List<UnitHero> unitHeroes)
    {
        for (int i = 0; i < unitHeroes.Count; i++)
        {
            if (unitHeroes[i] == UnitHero.None)
            {
                selectedPotatoImageArray[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                selectedPotatoImageArray[i].gameObject.SetActive(true);
            }
            selectedPotatoImageArray[i].sprite = unitDataSO.UnitStatDataList.Find(x => x.UnitHero == unitHeroes[i]).Sprite;
        }
    }
}

