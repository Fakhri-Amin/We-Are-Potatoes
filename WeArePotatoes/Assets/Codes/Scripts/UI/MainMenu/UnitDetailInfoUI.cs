using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitDetailInfoUI : MonoBehaviour
{
    [SerializeField] private Image unitIcon;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private Transform[] unitRangeAttackTypeLabels;
    [SerializeField] private TMP_Text seedCostText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private TMP_Text attackRangeText;
    [SerializeField] private TMP_Text attackSpeedText;

    private void Start()
    {
        foreach (var item in unitRangeAttackTypeLabels)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void Select(UnitData unitData)
    {
        foreach (var item in unitRangeAttackTypeLabels)
        {
            item.gameObject.SetActive(false);
        }

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
        moveSpeedText.text = unitData.MoveSpeedType.ToString();
        attackRangeText.text = unitData.AttackRadius.ToString();
        attackSpeedText.text = unitData.AttackSpeedType.ToString();
    }

}
