using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Farou.Utility;

public class UnitSelectedSlotUI : MonoBehaviour, IDropHandler
{
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

        if (draggableItem == null || unitSelectionUI.IsUnitAlreadyInUse(draggableItem.UnitData)) return;

        SelectUnit(draggableItem.UnitData);
        unitSelectionUI.SetSelectedUnit();
    }

    public void SelectUnit(UnitData unitData)
    {
        this.unitData = unitData;
        draggableItemUI.Initialize(unitSelectionUI, unitData);
        EventManager<UnitData>.Publish(Farou.Utility.EventType.OnUnitSelected, unitData);

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
        EventManager<UnitData>.Publish(Farou.Utility.EventType.OnUnitSelected, unitData);
    }
}

