using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public static Action<EnemyUnit> OnAnyEnemyUnitDead;

    [SerializeField] private float attackRange;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private new void Update()
    {
        base.Update();

        HandleUnitDetection();
    }

    private void HealthSystem_OnDead()
    {
        OnAnyEnemyUnitDead?.Invoke(this);
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HandleUnitDetection()
    {
        var playerUnits = PlayerUnitSpawner.Instance.GetPlayerUnits();

        foreach (var item in playerUnits)
        {
            if (Vector2.Distance(transform.position, item.transform.position) < attackRange)
            {
                canMove = false;
                HandleAttack();
            }
            else
            {
                canMove = true;
            }
        }
    }

    public override void HandleAttack()
    {

    }
}
