using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Unit : MonoBehaviour, IAttackable
{
    public static event Action<Unit> OnAnyUnitDead;

    [SerializeField] private UnitStatSO unitStatSO;
    [SerializeField] private UnitType unitType;
    [SerializeField] private UnitHero unitHero;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Transform visual;
    [SerializeField] private MMFeedbacks deadFeedbacks;
    [SerializeField] private ParticleSystem deadVFX;

    protected float moveSpeed;
    protected float detectRadius = 3f;
    protected float attackRadius = 1f;
    private bool canMove = true;
    private bool isEnemyInRange = false;
    private HealthSystem healthSystem;
    private IAttackable attackableTarget;
    private UnitAnimation unitAnimation;
    private Vector3 moveDirection;

    public UnitType UnitType => unitType;

    public virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unitAnimation = GetComponent<UnitAnimation>();
    }

    private void Start()
    {
        InitializeUnit();
    }

    private void OnEnable()
    {
        healthSystem.OnDead += HandleOnDead;
    }

    private void OnDisable()
    {
        healthSystem.OnDead -= HandleOnDead;
    }

    private void HandleOnDead()
    {
        // deadFeedbacks?.PlayFeedbacks();
        var vfx = Instantiate(deadVFX, transform.position, Quaternion.identity);
        vfx.gameObject.SetActive(true);
        OnAnyUnitDead?.Invoke(this);
    }

    private void Update()
    {
        if (canMove && !isEnemyInRange)
        {
            Move();
        }

        DetectEnemiesAndHandleMoveToward();
        DetectEnemiesAndHandleAttack();
    }

    private void InitializeUnit()
    {
        // Set the stat
        foreach (var item in unitStatSO.UnitStatDataList)
        {
            moveSpeed = item.MoveSpeed;
            detectRadius = item.DetectRadius;
            attackRadius = item.AttackRadius;
        }

        // Set the move direction
        moveDirection = unitType == UnitType.Player ? Vector3.right : Vector3.left;

        // Set the scale
        visual.localScale = unitType == UnitType.Player
            ? new Vector3(Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z)
            : new Vector3(-Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z);
    }

    private void Move()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void MoveToward(Vector3 targetPosition)
    {
        // Calculate the direction towards the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the unit towards the target
        transform.position += moveSpeed * Time.deltaTime * direction;
    }

    private void DetectEnemiesAndHandleMoveToward()
    {
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, detectRadius, targetMask);
        if (targetInRange.Length > 0)
        {
            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable attackable) && canMove)
                {
                    isEnemyInRange = true;
                    MoveToward(enemy.transform.position);
                    return;
                }
            }
        }
        isEnemyInRange = false;
    }

    private void DetectEnemiesAndHandleAttack()
    {
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetMask);

        if (targetInRange.Length > 0)
        {
            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable attackable))
                {
                    Unit unit = attackable as Unit;

                    if (unit != null && unit.UnitType != UnitType)
                    {
                        canMove = false;
                        attackableTarget = attackable;
                        unitAnimation.PlayAttackAnimation();
                        return;
                    }
                }
            }
        }

        // Reset state if no valid targets are found
        canMove = true;
        attackableTarget = null;
        unitAnimation.PlayIdleAnimation();
    }

    public virtual void HandleAttack()
    {
        if (attackableTarget != null)
        {
            attackableTarget.Damage(10);
            Debug.Log($"{UnitType} attacked {attackableTarget}");
        }
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
}
