using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameUnitCardUI : MonoBehaviour
{
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private UnitCardUI normalUnitCardTemplate;
    [SerializeField] private UnitCardUI longUnitCardTemplate;

    [SerializeField] private Transform buttonParent;
    [SerializeField] private UnitHero[] normalUnitCardSize;
    [SerializeField] private UnitHero[] longUnitCardSize;

    [SerializeField] private List<UnitHero> selectedUnits = new List<UnitHero>();

    private void Start()
    {
        normalUnitCardTemplate.gameObject.SetActive(false);
        longUnitCardTemplate.gameObject.SetActive(false);

        foreach (var item in selectedUnits)
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


}
