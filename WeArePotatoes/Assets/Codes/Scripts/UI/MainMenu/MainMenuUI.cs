using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponDecorationReference
    {
        public UnitHero UnitHero;
        public Transform Weapon;
    }

    [Header("Weapon Decoration")]
    [SerializeField] private List<WeaponDecorationReference> weaponDecorationReferences = new List<WeaponDecorationReference>();

    [SerializeField] private Transform panel;

    [Header("Selected Potato UI")]
    [SerializeField] private Image[] selectedPotatoImageArray;

    [Header("Project References")]
    [SerializeField] private UnitDataSO unitDataSO;

    private void Start()
    {
        HandleSelectedUnitListChanged(GameDataManager.Instance.SelectedUnitList);

        var unlockedUnitList = GameDataManager.Instance.UnlockedUnitList;
        foreach (var item in weaponDecorationReferences)
        {
            item.Weapon.gameObject.SetActive(false);
        }

        for (int i = 0; i < unlockedUnitList.Count; i++)
        {
            if (unlockedUnitList[i] == weaponDecorationReferences[i].UnitHero)
            {
                weaponDecorationReferences[i].Weapon.gameObject.SetActive(true);
            }
        }
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

