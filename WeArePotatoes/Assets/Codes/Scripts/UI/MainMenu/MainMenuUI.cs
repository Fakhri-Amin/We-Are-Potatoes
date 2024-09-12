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
        potatoSelectionButton.onClick.AddListener(() =>
        {
            isPotatoSelectionMenuOpen = !isPotatoSelectionMenuOpen;

            if (isPotatoSelectionMenuOpen)
            {
                unitSelectionUI.Show();
                unitSelectionUI.Initialize(GameDataManager.Instance.SelectedUnitHeroList);
            }
            else
            {
                unitSelectionUI.Hide();
            }

            potatoIcon.gameObject.SetActive(!isPotatoSelectionMenuOpen);
            potatoText.gameObject.SetActive(!isPotatoSelectionMenuOpen);
            potatoCloseIcon.gameObject.SetActive(isPotatoSelectionMenuOpen);
            potatoCloseText.gameObject.SetActive(isPotatoSelectionMenuOpen);
        });
    }
}
