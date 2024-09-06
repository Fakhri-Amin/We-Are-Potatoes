using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnHealthChanged;
    public Action OnDead;

    private int healthAmount;
    private int maxHealth;

    public void ResetHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        healthAmount = maxHealth;
        OnHealthChanged?.Invoke();
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
