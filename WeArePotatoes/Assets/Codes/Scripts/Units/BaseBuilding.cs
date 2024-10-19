using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;

public class BaseBuilding : MonoBehaviour, IAttackable
{
    public GameObject GameObject => spawnPosition;

    public UnitType UnitType => unitType;

    [SerializeField] private UnitType unitType;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private SpriteRenderer baseSprite;
    [SerializeField] private Sprite destroyedBaseSprite;
    [SerializeField] private MMFeedbacks destroyedFeedbacks;

    private HealthSystem healthSystem;
    private bool isDead;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnDead += OnBaseDestroyed;
    }

    private void OnDisable()
    {
        healthSystem.OnDead -= OnBaseDestroyed;
    }

    public void Initialize(int baseHealth)
    {
        healthSystem.ResetHealth(baseHealth);
    }

    private void OnBaseDestroyed()
    {
        if (isDead) return;

        destroyedFeedbacks.PlayFeedbacks();


        if (unitType == UnitType.Player)
        {
            EventManager.Publish(Farou.Utility.EventType.OnLevelLose);
        }
        else
        {
            EventManager.Publish(Farou.Utility.EventType.OnLevelWin);
            EventManager.Publish(Farou.Utility.EventType.OnEnemyBaseDestroyed);
        }

        isDead = true;
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

    public void Damage(float damageAmount)
    {
        PlayHitParticle();
        PlayHitSound();

        healthSystem.Damage(damageAmount);
    }

    public void ChangeToDestroyedSprite()
    {
        baseSprite.sprite = destroyedBaseSprite;
    }
}
