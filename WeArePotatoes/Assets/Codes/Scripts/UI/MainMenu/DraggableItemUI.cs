using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public UnitData UnitData;

    private Image image;
    private UnitSelectionSlotUI unitSelectionSlotUI;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(UnitSelectionSlotUI unitSelectionSlotUI, UnitData unitData)
    {
        this.unitSelectionSlotUI = unitSelectionSlotUI;
        UnitData = unitData;
    }

    public void Select()
    {
        unitSelectionSlotUI.SelectUnit();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag, false);
        image.raycastTarget = true;
    }
}
