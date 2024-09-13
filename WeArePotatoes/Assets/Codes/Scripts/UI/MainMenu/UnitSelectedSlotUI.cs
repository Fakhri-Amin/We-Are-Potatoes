using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Farou.Utility;
using System;
using Sirenix.OdinInspector;

public class UnitSelectedSlotUI : MonoBehaviour, IDropHandler
{
    public static event Action<UnitSelectedSlotUI, int> OnAlreadyInUseUnitDropped;

    [SerializeField] private Image image;
    [SerializeField] private Image seedImage;
    [SerializeField] private TMP_Text seedCost;
    [SerializeField] private DraggableItemUI draggableItemUI;

    private UnitSelectionUI unitSelectionUI;
    private UnitData unitData = null;

    public UnitData UnitData => unitData;

    public void Initialize(UnitSelectionUI unitSelectionUI, UnitData unitData)
    {
        this.unitSelectionUI = unitSelectionUI;
        this.unitData = unitData;

        if (unitData != null)
        {
            SelectUnit(unitData);
        }
        else
        {
            RemoveUnit();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggableItem = eventData.pointerDrag.GetComponent<DraggableItemUI>();

        if (draggableItem == null) return;

        // If the unit is already in use, swap the units between slots
        if (unitSelectionUI.IsUnitAlreadyInUse(draggableItem.UnitData))
        {
            // Find the slot where the dropped unit is already used
            var slotWithDraggedUnit = unitSelectionUI.GetSelectedSlotWithUnit(draggableItem.UnitData);

            // Swap units between the current slot and the slot with the dropped unit
            SwapUnits(slotWithDraggedUnit);
        }
        else
        {
            SelectUnit(draggableItem.UnitData);
        }

        // Update the selected units in the GameDataManager after the swap
        unitSelectionUI.SetSelectedUnit();
    }

    private void SwapUnits(UnitSelectedSlotUI otherSlot)
    {
        // Store the current unit data temporarily
        var tempUnitData = this.unitData;

        // Set the other slot's unit data to this slot
        this.SelectUnit(otherSlot.unitData);

        // Set this slot's original unit data to the other slot
        otherSlot.SelectUnit(tempUnitData);
    }

    public void SelectUnit(UnitData unitData)
    {
        this.unitData = unitData;
        draggableItemUI.Initialize(unitSelectionUI, unitData);
        EventManager.Publish(Farou.Utility.EventType.OnUnitSelected);

        image.gameObject.SetActive(true);
        seedImage.gameObject.SetActive(true);
        seedCost.gameObject.SetActive(true);
        image.sprite = unitData.Sprite;
        seedCost.text = unitData.SeedCost.ToString();
    }

    public void RemoveUnit()
    {
        unitData = new UnitData();
        image.gameObject.SetActive(false);
        seedImage.gameObject.SetActive(false);
        seedCost.gameObject.SetActive(false);

        seedCost.text = "-";
        EventManager.Publish(Farou.Utility.EventType.OnUnitSelected);
    }
}


