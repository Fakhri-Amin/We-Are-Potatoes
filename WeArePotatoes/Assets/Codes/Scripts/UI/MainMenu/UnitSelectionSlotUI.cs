using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitSelectionSlotUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Transform inUse;

    private UnitData unitData;

    public UnitData UnitData => unitData;

    private void Start()
    {
        inUse.gameObject.SetActive(false);
    }

    public void Initialize(UnitData unitData, Action onButtonClicked)
    {
        this.unitData = unitData;
        button.onClick.AddListener(() =>
        {
            onButtonClicked();
        }
        );
    }
}
