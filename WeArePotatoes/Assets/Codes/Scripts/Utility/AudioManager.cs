using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Farou.Utility;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private MMFeedbacks clickFeedbacks;
    [SerializeField] private MMFeedbacks unitHitFeedbacks;
    [SerializeField] private MMFeedbacks unitDeadFeedbacks;
    [SerializeField] private MMFeedbacks particleImageFeedbacks;

    public void PlayClickFeedbacks()
    {
        clickFeedbacks.PlayFeedbacks();
    }

    public void PlayUnitHitFeedbacks()
    {
        unitHitFeedbacks.PlayFeedbacks();
    }

    public void PlayUnitDeadFeedbacks()
    {
        unitDeadFeedbacks.PlayFeedbacks();
    }

    public void PlayParticleImageFeedbacks()
    {
        particleImageFeedbacks.PlayFeedbacks();
    }
}