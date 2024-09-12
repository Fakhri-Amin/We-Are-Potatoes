using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitSelectionUI : MonoBehaviour
{
    [Serializable]
    public class UnitSelectionSlotReference
    {
        public UnitHero UnitHero;
        public UnitSelectionSlotUI Slot;
    }

    [SerializeField] private Transform panel;
    [SerializeField] private UnitDataSO unitDataSO;

    [Header("Unit Selection UI")]
    [SerializeField] private UnitSelectionSlotUI unitSlotTemplate;
    [SerializeField] private Transform parent;
    [SerializeField] private List<UnitSelectionSlotReference> unitSelectionSlotReferences = new List<UnitSelectionSlotReference>();

    private UnitDetailInfoUI unitDetailInfoUI;

    private void Awake()
    {
        unitDetailInfoUI = GetComponent<UnitDetailInfoUI>();
    }

    private void Start()
    {
        unitSlotTemplate.gameObject.SetActive(false);

        var unitDataList = unitDataSO.UnitStatDataList;
        foreach (var item in unitDataList)
        {
            // UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == item.UnitHero);
            UnitSelectionSlotUI slotUI = Instantiate(unitSlotTemplate, parent);
            slotUI.gameObject.SetActive(true);
            slotUI.Initialize(item, () => unitDetailInfoUI.Select(item));
        }

        Hide();
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
