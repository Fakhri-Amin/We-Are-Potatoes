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
        UpdateHealthBar();
    }

    private void OnEnable()
    {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnHit += HealthSystem_OnHealthChanged;

    }

    private void OnDisable()
    {
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        healthSystem.OnHit -= HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.UpdateBar01(healthSystem.GetHealthNormalized());
        gameObject.SetActive(healthSystem.GetHealthNormalized() < 1);
    }
}
