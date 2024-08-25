using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Farou.Utility;

public class FeedbackManager : Singleton<FeedbackManager>
{
    [SerializeField] private MMFeedbacks clickFeedbacks;
    [SerializeField] private MMFeedbacks purchaseFeedbacks;
    [SerializeField] private MMFeedbacks buffFeedbacks;
    [SerializeField] private MMFeedbacks particleImageFeedbacks;

    public void PlayClickFeedbacks()
    {
        clickFeedbacks.PlayFeedbacks();
    }

    public void PlayPurchaseFeedbacks()
    {
        purchaseFeedbacks.PlayFeedbacks();
    }

    public void PlayBuffFeedbacks()
    {
        buffFeedbacks.PlayFeedbacks();
    }

    public void PlayParticleImageFeedbacks()
    {
        particleImageFeedbacks.PlayFeedbacks();
    }
}