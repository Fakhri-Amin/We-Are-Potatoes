using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private MMProgressBar healthBar;
    [SerializeField] private Image barImage;

    private void Start()
    {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }

    private void HealthSystem_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // barImage.fillAmount = healthSystem.GetHealthNormalized();
        healthBar.UpdateBar01(healthSystem.GetHealthNormalized());
        gameObject.SetActive(healthSystem.GetHealthNormalized() < 1);
    }
}
