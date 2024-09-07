using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnHealthReset;
    public Action OnHealthChanged;
    public Action OnDead;

    private int healthAmount;
    private int maxHealth;

    public void ResetHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        healthAmount = maxHealth;
        OnHealthReset?.Invoke();
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
