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
    [SerializeField] private MMFeedbacks resetFeedbacks;
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private SpriteRenderer weaponSprite;

    private bool canMove = true;
    private IAttackable targetUnit;
    private HealthSystem healthSystem;
    private UnitAnimation unitAnimation;
    private UnitParticle unitParticle;
    private UnitAudio unitAudio;
    private Vector3 moveDirection;
    private Vector3 basePosition;
    private UnitStatData stat;

    public IAttackable TargetUnit => targetUnit;
    public UnitStatData Stat => stat;
    public UnitType UnitType => unitType;
    public UnitHero UnitHero => unitHero;
    public LayerMask TargetMask => targetMask;

    public GameObject GameObject => gameObject;

    private float attackSpeed = 0;
    private bool canAttack = true;

    public virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unitAnimation = GetComponent<UnitAnimation>();
        unitParticle = GetComponent<UnitParticle>();
        unitAudio = GetComponent<UnitAudio>();
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
        unitParticle.PlayDeadParticle();
        unitAudio.PlayDeadSound();

        OnDead?.Invoke();
        OnAnyUnitDead?.Invoke(this);
    }

    private void Update()
    {
        // Handle attack cooldown
        if (!canAttack)
        {
            attackSpeed -= Time.deltaTime;
            canAttack = attackSpeed <= 0;
        }

        // Handle movement and attack
        if (canMove)
        {
            Move();
        }
        else if (!canMove && canAttack)
        {
            unitAnimation.PlayAttackAnimation();
            attackSpeed = stat.AttackSpeed;
            canAttack = false;
        }

        DetectEnemiesAndHandleAttack(); // Ensure enemy detection and handling is checked every frame
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

        // Reset state
        healthSystem.ResetHealth(stat.Health);
        // ResetState();

        // Set the move direction
        moveDirection = unitType == UnitType.Player ? Vector3.right : Vector3.left;

        // Set the scale
        visual.localScale = unitType == UnitType.Player
            ? new Vector3(Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z)
            : new Vector3(-Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z);

        // Set the initial canMove variable
        canMove = true;

        // Set the animation to idle 
        unitAnimation.PlayIdleAnimation();
    }

    public void ResetState()
    {
        bodySprite.material.SetFloat("_HitEffectBlend", 0);
        weaponSprite.material.SetFloat("_HitEffectBlend", 0);
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

            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable unit))
                {
                    float distanceToBase = Mathf.Abs(Vector3.Distance(enemy.transform.position, basePosition));

                    if (distanceToBase < closestDistance)
                    {
                        closestDistance = distanceToBase;
                        targetUnit = unit;
                    }
                }
            }

            if (targetUnit != null)
            {
                canMove = false;
                return;
            }
        }

        // Reset state if no valid targets are found
        targetUnit = null;
        canMove = true;
    }

    public void Damage(int damageAmount)
    {
        unitParticle.PlayHitParticle();
        unitAudio.PlayHitSound();

        healthSystem.Damage(damageAmount);
    }
}
