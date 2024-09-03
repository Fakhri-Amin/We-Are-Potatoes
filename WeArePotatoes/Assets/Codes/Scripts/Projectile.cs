using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 2f;
    [SerializeField] private Rigidbody2D rb;
    private int damageAmount;
    private Unit targetUnit;
    private UnitType unitType;
    private UnitStatData stat;
    private ProjectileType projectileType;

    private void Awake()
    {
        // rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!targetUnit)
        {
            ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
            return;
        }

        if (unitType == UnitType.Enemy)
        {
            if (Vector2.Distance(transform.position, PlayerUnitSpawner.Instance.GetUnitPosition(targetUnit)) <= 0.05f)
            {
                ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
            }
        }
        else if (unitType == UnitType.Player)
        {
            if (Vector2.Distance(transform.position, EnemyUnitSpawner.Instance.GetUnitPosition(targetUnit)) <= 0.05f)
            {
                ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
            }
        }

    }

    public IEnumerator CurveMovementRoutine(Vector2 start, Vector2 target, float attackSpeed)
    {
        float timePassed = 0f;
        Vector2 end = target;

        while (timePassed < attackSpeed)
        {
            timePassed += Time.deltaTime;

            float linearT = timePassed / attackSpeed;
            float heightT = animationCurve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }

    }

    public void Initialize(UnitType unitType, UnitStatData stat, ProjectileType projectileType, Unit targetUnit)
    {
        this.unitType = unitType;
        this.stat = stat;
        this.projectileType = projectileType;
        this.targetUnit = targetUnit;
        this.damageAmount = stat.DamageAmount;

        if (stat.UnitRangeType == UnitRangeType.RangeStraight)
        {
            // Set the direction
            Vector3 direction = targetUnit.gameObject.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * stat.AttackSpeed * 3;

            // Set the rotation
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            return;
        }
        else if (stat.UnitRangeType == UnitRangeType.RangeCurve)
        {
            float calculatedAttackSpeed = 1 / (stat.AttackSpeed / 4);
            StartCoroutine(CurveMovementRoutine(transform.position, targetUnit.gameObject.transform.position, calculatedAttackSpeed));
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IAttackable>(out IAttackable attackable))
        {
            Unit target = attackable as Unit;
            if (target.UnitType != unitType)
            {
                attackable.Damage(damageAmount);
                ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
            }
        }
    }
}
