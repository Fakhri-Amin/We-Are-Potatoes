using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitSelectionSlotUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text seedCostText;
    [SerializeField] private Transform inUse;
    [SerializeField] private DraggableItemUI draggableItemUI;

    private UnitData unitData;

    public UnitData UnitData => unitData;

    private void Start()
    {
        inUse.gameObject.SetActive(false);
    }

    public void Initialize(UnitData unitData, Action onButtonClicked)
    {
        this.unitData = unitData;

        unitImage.sprite = unitData.Sprite;
        unitNameText.text = unitData.Name;
        seedCostText.text = unitData.SeedCost.ToString();

        button.onClick.AddListener(() =>
        {
            onButtonClicked();
        }
        );

        draggableItemUI.Initialize(this, unitData);
    }

    public void SelectUnit()
    {
        inUse.gameObject.SetActive(true);
        inUse.transform.SetAsLastSibling();
    }
}
