using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static event Action<Projectile> OnProjectileAreaHit;

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 2f;
    [SerializeField] private Rigidbody2D rb;
    private Unit sourceUnit;
    private Unit targetUnit;
    private ProjectileType projectileType;
    private Vector2 targetPosition;

    private const float DistanceThreshold = .1f;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing.");
            }
        }
    }

    private void Update()
    {
        if (!targetUnit)
        {
            ReturnToPool();
            return;
        }
    }

    private bool IsTargetReached()
    {
        Vector2 targetPosition = sourceUnit.UnitType == UnitType.Enemy
            ? PlayerUnitSpawner.Instance.GetUnitPosition(targetUnit)
            : EnemyUnitSpawner.Instance.GetUnitPosition(targetUnit);

        return Vector2.Distance(transform.position, targetPosition) <= DistanceThreshold;
    }

    private void ReturnToPool()
    {
        ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
    }

    public IEnumerator CurveMovementRoutine(Vector2 start, Vector2 target, float projectileSpeed)
    {
        float timePassed = 0f;
        Vector2 end = target;

        while (timePassed < projectileSpeed)
        {
            timePassed += Time.deltaTime;

            float linearT = timePassed / projectileSpeed;
            float heightT = animationCurve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }

        ReturnToPool();
    }

    public void Initialize(Unit sourceUnit, Unit targetUnit, ProjectileType projectileType)
    {
        this.sourceUnit = sourceUnit;
        this.targetUnit = targetUnit;
        this.projectileType = projectileType;

        gameObject.layer = sourceUnit.gameObject.layer;

        if (sourceUnit.Stat.UnitRangeType == UnitRangeType.RangeStraight)
        {
            InitializeStraightProjectile();
        }
        else if (sourceUnit.Stat.UnitRangeType == UnitRangeType.RangeCurve)
        {
            StartCoroutine(CurveMovementRoutine(transform.position, targetUnit.gameObject.transform.position, 1 / sourceUnit.Stat.ProjectileSpeed));
        }
    }

    private void InitializeStraightProjectile()
    {
        Vector2 direction = (Vector2)(targetUnit.transform.position - transform.position);
        rb.velocity = direction.normalized * sourceUnit.Stat.ProjectileSpeed;

        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Unit singleUnit = other.GetComponent<Unit>();

        // Ensure the unit is valid and belongs to a different faction
        if (singleUnit == null || singleUnit.UnitType == sourceUnit.UnitType)
        {
            return;
        }

        // Handle single target attack
        if (sourceUnit.Stat.UnitAttackType == UnitAttackType.Single)
        {
            singleUnit.Damage(sourceUnit.Stat.DamageAmount);
        }
        // Handle area of effect attack
        else if (sourceUnit.Stat.UnitAttackType == UnitAttackType.Area)
        {
            ApplyAreaOfEffectDamage();
        }

        ReturnToPool();
    }

    private void ApplyAreaOfEffectDamage()
    {
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(transform.position, sourceUnit.Stat.AreaOfEffectRadius, sourceUnit.TargetMask);

        foreach (var item in targetsInRadius)
        {
            if (item.TryGetComponent<Unit>(out Unit unit) && unit.UnitType != sourceUnit.UnitType)
            {
                OnProjectileAreaHit?.Invoke(this);
                unit.Damage(sourceUnit.Stat.DamageAmount);
            }
        }
    }

}
