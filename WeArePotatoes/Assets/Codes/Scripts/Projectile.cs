using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 2f;
    [SerializeField] private Rigidbody2D rb;
    private int damageAmount;
    private Transform targetTransform;
    private UnitType unitType;
    private ProjectileType projectileType;

    private void Awake()
    {
        // rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!targetTransform) return;

        transform.position = Vector3.Slerp(transform.position, targetTransform.position, Time.deltaTime);

        if (Vector2.Distance(transform.position, targetTransform.position) <= 0.05f)
        {
            ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
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

    public void Initialize(UnitType unitType, UnitStatData stat, ProjectileType projectileType, Transform targetTransform)
    {
        this.unitType = unitType;
        this.projectileType = projectileType;
        this.targetTransform = targetTransform;
        this.damageAmount = stat.DamageAmount;

        if (stat.UnitRangeType == UnitRangeType.RangeStraight)
        {
            // Set the direction
            Vector3 direction = targetTransform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * stat.MoveSpeed;

            // Set the rotation
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            return;
        }
        else if (stat.UnitRangeType == UnitRangeType.RangeCurve)
        {
            float calculatedAttackSpeed = 1 / (stat.AttackSpeed / 4);
            StartCoroutine(CurveMovementRoutine(transform.position, targetTransform.position, calculatedAttackSpeed));
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
