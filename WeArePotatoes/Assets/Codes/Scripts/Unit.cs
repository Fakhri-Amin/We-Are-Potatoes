using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Unit : MonoBehaviour, IAttackable
{
    public static event Action<Unit> OnAnyUnitDead;
    public event Action OnDead;

    [SerializeField] private UnitStatSO unitStatSO;
    [SerializeField] private UnitType unitType;
    [SerializeField] private UnitHero unitHero;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Transform visual;
    [SerializeField] private MMFeedbacks deadFeedbacks;
    [SerializeField] private ParticleSystem deadVFX;

    protected int damageAmount;
    protected float moveSpeed;
    protected float detectRadius = 3f;
    protected float attackRadius = 1f;
    protected float attackSpeed = 0;
    private bool canMove = true;
    private HealthSystem healthSystem;
    private IAttackable attackableTarget;
    private UnitAnimation unitAnimation;
    private Vector3 moveDirection;
    private Coroutine attackRoutine;
    private Vector3 basePosition;

    public UnitType UnitType => unitType;
    public IAttackable AttackableTarget => attackableTarget;
    public int DamageAmount => damageAmount;

    public virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unitAnimation = GetComponent<UnitAnimation>();
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
        deadFeedbacks?.PlayFeedbacks();
        OnDead?.Invoke();
        // var vfx = Instantiate(deadVFX, transform.position, Quaternion.identity);
        // vfx.gameObject.SetActive(true);
        OnAnyUnitDead?.Invoke(this);
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
        }

        DetectEnemiesAndHandleAttack();
    }

    public void InitializeUnit(UnitType unitType, Vector3 basePosition)
    {
        // Set the type
        this.unitType = unitType;

        // Set the base position
        this.basePosition = basePosition;

        // Set the layer mask and tag
        gameObject.layer = LayerMask.NameToLayer(unitType.ToString());
        gameObject.tag = unitType.ToString();
        targetMask = LayerMask.GetMask(unitType == UnitType.Player ? "Enemy" : "Player");

        // Set the stat
        foreach (var item in unitStatSO.UnitStatDataList)
        {
            if (item.UnitHero == unitHero)
            {
                damageAmount = item.DamageAmount;
                moveSpeed = item.MoveSpeed;
                detectRadius = item.DetectRadius;
                attackRadius = item.AttackRadius;
                attackSpeed = item.AttackSpeed;
            }
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
        // Detect for target in range
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, detectRadius, targetMask);

        // If there is target detected
        if (targetInRange.Length > 0)
        {
            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable attackable))
                {
                    MoveToward(enemy.transform.position);
                    return;
                }
            }
        }

        // If there is no target detected
        MoveStraight();
    }

    private void MoveToward(Vector3 targetPosition)
    {
        // Calculate the direction towards the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the unit towards the target
        transform.position += moveSpeed * Time.deltaTime * direction;
    }

    private void MoveStraight()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void DetectEnemiesAndHandleAttack()
    {
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetMask);

        if (targetInRange.Length > 0)
        {
            float closestDistance = float.MaxValue;
            IAttackable closestEnemy = null;
            Unit closestUnit = null;

            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable attackable))
                {
                    Unit unit = attackable as Unit;

                    if (unit != null && unit.UnitType != UnitType)
                    {
                        float distanceToBase = Mathf.Abs(Vector3.Distance(enemy.transform.position, basePosition));

                        if (distanceToBase < closestDistance)
                        {
                            closestDistance = distanceToBase;
                            closestEnemy = attackable;
                            closestUnit = unit;
                        }
                    }
                }
            }

            if (closestEnemy != null)
            {
                if (attackRoutine != null) return;

                canMove = false;
                attackableTarget = closestEnemy;

                closestUnit.OnDead += ResetTargetEnemy;
                attackRoutine = StartCoroutine(AttackRoutine());
                return;
            }
        }

        // Reset state if no valid targets are found
        canMove = true;
        ResetTargetEnemy();
        unitAnimation.PlayIdleAnimation();
    }

    private void ResetTargetEnemy()
    {
        attackableTarget = null;
        attackRoutine = null;
    }

    private IEnumerator AttackRoutine()
    {
        while (!canMove)
        {
            unitAnimation.PlayAttackAnimation();

            yield return new WaitForSeconds(attackSpeed);
        }

    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
}
