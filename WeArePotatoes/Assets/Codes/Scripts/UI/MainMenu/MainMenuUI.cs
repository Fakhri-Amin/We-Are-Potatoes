using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Transform panel;

    [Header("Selected Potato UI")]
    [SerializeField] private Image[] selectedPotatoImageArray;

    [Header("Project References")]
    [SerializeField] private UnitDataSO unitDataSO;

    private void Start()
    {
        HandleSelectedUnitListChanged(GameDataManager.Instance.SelectedUnitList);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnSelectedUnitListChanged += HandleSelectedUnitListChanged;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnSelectedUnitListChanged -= HandleSelectedUnitListChanged;
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

    public void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}

