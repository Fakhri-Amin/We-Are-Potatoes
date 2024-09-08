using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour, IAttackable
{
    public GameObject GameObject => spawnPosition;

    public UnitType UnitType => unitType;

    [SerializeField] private UnitStatSO unitStatSO;
    [SerializeField] private UnitType unitType;
    [SerializeField] private GameObject spawnPosition;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        if (unitType == UnitType.Player)
        {
            healthSystem.ResetHealth(unitStatSO.PlayerBaseMaxHealth);
        }
        else
        {
            healthSystem.ResetHealth(unitStatSO.EnemyBaseMaxHealth);
        }
    }

    public void PlayHitParticle()
    {
        PlayParticle(ParticleEffectType.Hit, spawnPosition.transform.position);
    }

    public void PlayDeadParticle()
    {
        PlayParticle(ParticleEffectType.Dead, spawnPosition.transform.position);
    }

    private void PlayParticle(ParticleEffectType vfxType, Vector3 position)
    {
        var vfx = ParticleEffectObjectPool.Instance.GetPooledObject(vfxType);
        if (vfx)
        {
            vfx.transform.position = position;
            vfx.Play();
        }
    }

    public void PlayHitSound()
    {
        AudioManager.Instance.PlayUnitHitFeedbacks();
    }

    public void PlayDeadSound()
    {
        AudioManager.Instance.PlayUnitDeadFeedbacks();
    }

    public void Damage(int damageAmount)
    {
        PlayHitParticle();
        PlayHitSound();

        healthSystem.Damage(damageAmount);
    }
}
