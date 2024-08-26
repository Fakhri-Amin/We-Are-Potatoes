using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public static Action OnAnyPlayerUnitDead;

    [SerializeField] private float attackRange;

    private HealthSystem healthSystem;
    private PlayerAnimation playerAnimation;
    private EnemyUnit targetEnemy;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        playerAnimation = GetComponent<PlayerAnimation>();
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
        OnAnyPlayerUnitDead?.Invoke();

        Destroy(gameObject);
    }

    public void Damage(int damageAmount)
    {
        // healthSystem.Damage(damageAmount);
    }

    private void HandleUnitDetection()
    {
        var enemyUnits = EnemyUnitSpawner.Instance.GetEnemyUnits();

        if (enemyUnits.Count <= 0)
        {
            canMove = true;
            targetEnemy = null;
            playerAnimation.PlayIdleAnimation();
        }

        foreach (var item in enemyUnits)
        {
            if (Vector2.Distance(transform.position, item.transform.position) < attackRange)
            {
                canMove = false;
                targetEnemy = item;
                playerAnimation.PlayAttackAnimation();
            }
            else
            {
                canMove = true;
                targetEnemy = null;
                playerAnimation.PlayIdleAnimation();
            }
        }
    }

    public override void HandleAttack()
    {
        if (targetEnemy)
        {
            targetEnemy.Damage(10);
            Debug.Log("Attack");
        }
    }


}
