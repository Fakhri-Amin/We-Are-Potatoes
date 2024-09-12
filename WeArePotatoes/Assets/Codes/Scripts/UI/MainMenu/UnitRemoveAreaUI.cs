using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitRemoveAreaUI : MonoBehaviour, IDropHandler
{
    private UnitSelectionUI unitSelectionUI;

    public void Initialize(UnitSelectionUI unitSelectionUI)
    {
        this.unitSelectionUI = unitSelectionUI;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItemUI draggableItem = dropped.GetComponent<DraggableItemUI>();

        draggableItem.Reset();

        unitSelectionUI.HideRemoveArea();
        unitSelectionUI.RemoveUnitFromSelectedUnitSlot(draggableItem.UnitData);

        EventManager.Publish(Farou.Utility.EventType.OnUnitSelected);
    }
}
