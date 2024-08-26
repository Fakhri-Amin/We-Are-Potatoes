using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IAttackable
{
    public static Action<Unit> OnAnyUnitDead;
    public UnitType UnitType;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection;

    protected bool canMove = true;

    protected HealthSystem healthSystem;
    protected IAttackable attackableTarget;
    protected UnitAnimation unitAnimation;

    public virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unitAnimation = GetComponent<UnitAnimation>();
    }

    private void OnEnable()
    {
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead()
    {
        OnAnyUnitDead?.Invoke(this);
    }

    protected void Update()
    {
        if (canMove)
        {
            Move(moveDirection, moveSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IAttackable>(out IAttackable attackable))
        {
            canMove = false;
            attackableTarget = attackable;
            unitAnimation.PlayAttackAnimation();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<IAttackable>(out IAttackable attackable))
        {
            canMove = false;
            attackableTarget = attackable;
            unitAnimation.PlayAttackAnimation();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canMove = true;
        attackableTarget = null;
        unitAnimation.PlayIdleAnimation();
    }

    private void Move(Vector3 moveDirection, float moveSpeed)
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    public virtual void HandleAttack()
    {
        if (attackableTarget != null)
        {
            attackableTarget.Damage(10);
            Debug.Log("Player Attack");
        }
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
}
