using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnHealthChanged;
    public Action OnDead;

    [SerializeField] private int maxHealth = 100;

    private int healthAmount;

    private void Awake()
    {
        healthAmount = maxHealth;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;

        if (healthAmount <= 0)
        {
            healthAmount = 0;
            OnDead?.Invoke();
        }

        OnHealthChanged?.Invoke();
    }

    public float GetHealthNormalized()
    {
        return (float)healthAmount / maxHealth;
    }
}
