using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private MMProgressBar healthBar;

    private void Start()
    {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }

    private void OnDisable()
    {
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.UpdateBar01(healthSystem.GetHealthNormalized());
        healthBar.gameObject.SetActive(healthSystem.GetHealthNormalized() < 1);
    }
}
