using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUnitCardUI : MonoBehaviour
{
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private UnitCardUI normalUnitCardTemplate;
    [SerializeField] private UnitCardUI longUnitCardTemplate;
    [SerializeField] private TMP_Text seedCountText;

    [SerializeField] private UnitHero[] normalUnitCardSize;
    [SerializeField] private UnitHero[] longUnitCardSize;

    [Header("Reference To Other Gameobject")]
    [SerializeField] private PlayerUnitSpawner playerUnitSpawner;

    private void OnEnable()
    {
        playerUnitSpawner.OnSeedCountChanged += OnSeedProductionCountChanged;
    }

    private void OnDisable()
    {
        playerUnitSpawner.OnSeedCountChanged -= OnSeedProductionCountChanged;
    }

    private void Start()
    {
        normalUnitCardTemplate.gameObject.SetActive(false);
        longUnitCardTemplate.gameObject.SetActive(false);

        List<UnitHero> selectedUnitHeroList = GameDataManager.Instance.SelectedUnitList;

        foreach (var item in selectedUnitHeroList)
        {
            UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == item);
            if (normalUnitCardSize.Contains(unitData.UnitHero))
            {
                UnitCardUI unitCardUI = Instantiate(normalUnitCardTemplate, buttonParent);
                unitCardUI.Initialize(unitData);
                unitCardUI.gameObject.SetActive(true);
            }
            else if (longUnitCardSize.Contains(unitData.UnitHero))
            {
                UnitCardUI unitCardUI = Instantiate(longUnitCardTemplate, buttonParent);
                unitCardUI.Initialize(unitData);
                unitCardUI.gameObject.SetActive(true);
            }
        }
    }

    private void OnSeedProductionCountChanged(float currentSeedCount)
    {
        seedCountText.text = currentSeedCount.ToString();
    }


}
