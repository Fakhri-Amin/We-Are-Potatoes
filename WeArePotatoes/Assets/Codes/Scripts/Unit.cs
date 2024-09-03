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

    private bool canMove = true;
    private HealthSystem healthSystem;
    private Unit targetUnit;
    private UnitAnimation unitAnimation;
    private Vector3 moveDirection;
    private Coroutine attackRoutine;
    private Vector3 basePosition;
    private UnitStatData stat;

    public UnitType UnitType => unitType;
    public UnitStatData Stat => stat;
    public Unit TargetUnit => targetUnit;

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
                stat = item;
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
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, stat.DetectRadius, targetMask);

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
        transform.position += stat.MoveSpeed * Time.deltaTime * direction;
    }

    private void MoveStraight()
    {
        transform.position += stat.MoveSpeed * Time.deltaTime * moveDirection;
    }

    private void DetectEnemiesAndHandleAttack()
    {
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, stat.AttackRadius, targetMask);

        if (targetInRange.Length > 0)
        {
            float closestDistance = float.MaxValue;
            // targetUnit = null;

            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<Unit>(out Unit unit))
                {
                    // Unit unit = attackable as Unit;

                    if (unit != null && unit.UnitType != UnitType)
                    {
                        float distanceToBase = Mathf.Abs(Vector3.Distance(enemy.transform.position, basePosition));

                        if (distanceToBase < closestDistance)
                        {
                            closestDistance = distanceToBase;
                            targetUnit = unit;
                        }
                    }
                }
            }

            if (targetUnit != null)
            {
                if (attackRoutine != null) return;

                canMove = false;

                targetUnit.OnDead += ResetTargetEnemy;
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
        attackRoutine = null;
        targetUnit = null;
    }

    private IEnumerator AttackRoutine()
    {
        while (!canMove)
        {
            unitAnimation.PlayAttackAnimation();

            yield return new WaitForSeconds(stat.AttackSpeed);
        }

    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
}
