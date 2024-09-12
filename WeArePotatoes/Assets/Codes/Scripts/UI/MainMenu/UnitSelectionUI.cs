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

    [Header("Unit Selected UI")]
    [SerializeField] private List<UnitSelectedSlotUI> selectedSlotUIs = new List<UnitSelectedSlotUI>();

    [Header("Unit Selection UI")]
    [SerializeField] private UnitSelectionSlotUI unitSlotTemplate;
    [SerializeField] private Transform parent;
    [SerializeField] private UnitRemoveAreaUI unitRemoveAreaUI;

    private UnitDetailInfoUI unitDetailInfoUI;

    private void Awake()
    {
        unitDetailInfoUI = GetComponent<UnitDetailInfoUI>();
    }

    private void Start()
    {
        Hide();
    }

    public void Initialize(List<UnitHero> selectedUnitList)
    {
        unitSlotTemplate.gameObject.SetActive(false);
        HideRemoveArea();

        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            UnitData unitData = unitDataSO.UnitStatDataList.Find(x => x.UnitHero == selectedUnitList[i]);
            selectedSlotUIs[i].Initialize(this, unitData);
        }

        InitializeUnitSlots();
        unitRemoveAreaUI.Initialize(this);
    }

    private void InitializeUnitSlots()
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<UnitSelectionSlotUI>() == unitSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        var unitDataList = unitDataSO.UnitStatDataList;
        foreach (var item in unitDataList)
        {
            var slotUI = Instantiate(unitSlotTemplate, parent);
            slotUI.gameObject.SetActive(true);
            slotUI.Initialize(this, item, () => unitDetailInfoUI.Select(item));
        }
    }

    public bool IsUnitAlreadyInUse(UnitData unitData)
    {
        return selectedSlotUIs.Exists(i => i.UnitData == unitData);
    }

    public void RemoveUnitFromSelectedUnitSlot(UnitData unitData)
    {
        var selectedSlot = selectedSlotUIs.Find(i => i.UnitData == unitData);
        if (selectedSlot != null)
        {
            selectedSlot.RemoveUnit();
        }
    }

    public void SetSelectedUnit()
    {
        List<UnitHero> selectedUnitList = selectedSlotUIs.ConvertAll(item => item.UnitData.UnitHero);
        foreach (var item in selectedUnitList)
        {
            Debug.Log(item);
        }
        GameDataManager.Instance.SetSelectedUnit(selectedUnitList);
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    public void ShowRemoveArea()
    {
        unitRemoveAreaUI.gameObject.SetActive(true);
    }

    public void HideRemoveArea()
    {
        unitRemoveAreaUI.gameObject.SetActive(false);
    }
}

