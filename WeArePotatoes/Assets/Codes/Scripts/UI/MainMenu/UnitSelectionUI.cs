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
    [SerializeField] private List<UnitSelectionSlotReference> unitSelectionSlotReferences = new List<UnitSelectionSlotReference>();

    private UnitDetailInfoUI unitDetailInfoUI;

    private void Awake()
    {
        unitDetailInfoUI = GetComponent<UnitDetailInfoUI>();
    }

    private void Start()
    {
        foreach (var item in unitSelectionSlotReferences)
        {
            UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == item.UnitHero);
            item.Slot.Initialize(unitData, () => unitDetailInfoUI.Select(unitData));
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
