using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitDetailInfoUI : MonoBehaviour
{
    [SerializeField] private Transform panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image unitIcon;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private Transform[] unitRangeAttackTypeLabels;
    [SerializeField] private TMP_Text seedCostText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private TMP_Text attackSpeedText;

    private void Start()
    {
        foreach (var item in unitRangeAttackTypeLabels)
        {
            item.gameObject.SetActive(false);
        }

        Hide();
    }

    public void Select(UnitData unitData)
    {
        panel.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);

        foreach (var item in unitRangeAttackTypeLabels)
        {
            item.gameObject.SetActive(false);
        }

        closeButton.onClick.AddListener(Hide);

        unitIcon.sprite = unitData.Sprite;
        unitName.text = unitData.Name;

        if (unitData.UnitRangeType == UnitRangeType.Melee)
        {
            unitRangeAttackTypeLabels[0].gameObject.SetActive(true);
        }
        else
        {
            unitRangeAttackTypeLabels[1].gameObject.SetActive(true);
        }

        if (unitData.UnitAttackType == UnitAttackType.Area)
        {
            unitRangeAttackTypeLabels[2].gameObject.SetActive(true);
        }

        seedCostText.text = unitData.SeedCost.ToString();
        healthText.text = unitData.Health.ToString();
        damageText.text = unitData.DamageAmount.ToString();

        if (unitData.MoveSpeedType == UnitMoveSpeedType.VerySlow)
        {
            moveSpeedText.text = "Very Slow";
        }
        else if (unitData.MoveSpeedType == UnitMoveSpeedType.VeryFast)
        {
            moveSpeedText.text = "Very Fast";
        }
        else
        {
            moveSpeedText.text = unitData.MoveSpeedType.ToString();
        }

        if (unitData.AttackSpeedType == UnitAttackSpeedType.VerySlow)
        {
            attackSpeedText.text = "Very Slow";
        }
        else if (unitData.AttackSpeedType == UnitAttackSpeedType.VeryFast)
        {
            attackSpeedText.text = "Very Fast";
        }
        else
        {
            attackSpeedText.text = unitData.AttackSpeedType.ToString();
        }
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
