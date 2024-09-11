using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitSelectionSlotUI : MonoBehaviour
{
    private Button button;
    private Transform focus;
    private Transform inUse;

    private UnitData unitData;

    public UnitData UnitData => unitData;

    private void Awake()
    {
        button = GetComponent<Button>();
        focus = transform.GetChild(1);
        inUse = transform.GetChild(2);
    }

    private void Start()
    {
        focus.gameObject.SetActive(false);
        inUse.gameObject.SetActive(false);
    }

    public void Initialize(UnitData unitData, Action onButtonClicked)
    {
        this.unitData = unitData;
        button.onClick.AddListener(() =>
        {
            Select();
            onButtonClicked();
        }
        );
    }

    public void Select()
    {
        focus.gameObject.SetActive(true);
    }
}
