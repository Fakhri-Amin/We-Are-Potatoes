using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text seedCost;

    private UnitData unitData;

    public void OnDrop(PointerEventData eventData)
    {
        if (unitData != null) return;

        GameObject dropped = eventData.pointerDrag;
        DraggableItemUI draggableItem = dropped.GetComponent<DraggableItemUI>();
        image.sprite = draggableItem.UnitData.Sprite;
        seedCost.text = draggableItem.UnitData.SeedCost.ToString();
        draggableItem.Select();

        unitData = draggableItem.UnitData;
    }
}
