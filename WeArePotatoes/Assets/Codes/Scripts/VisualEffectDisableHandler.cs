using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectDisableHandler : MonoBehaviour
{
    [SerializeField] private VisualEffectType visualEffectType;

    private void OnParticleSystemStopped()
    {
        VisualEffectObjectPool.Instance.ReturnToPool(visualEffectType, GetComponent<ParticleSystem>());
    }
}
