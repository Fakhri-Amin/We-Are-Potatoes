using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text seedAmountText;

    public void Initialize(UnitData unitData)
    {
        unitImage.sprite = unitData.Sprite;
        seedAmountText.text = unitData.SeedCost.ToString();

        button.onClick.AddListener(() =>
        {
            PlayerUnitSpawner.Instance.OnUnitSpawn(unitData.UnitHero);
        });
    }
}
