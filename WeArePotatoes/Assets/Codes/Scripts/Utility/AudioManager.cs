using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Farou.Utility;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private MMFeedbacks clickFeedbacks;
    [SerializeField] private MMFeedbacks levelStartFeedbacks;
    [SerializeField] private MMFeedbacks coinFeedbacks;
    [SerializeField] private MMFeedbacks unitHitFeedbacks;
    [SerializeField] private MMFeedbacks unitDeadFeedbacks;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayUnitHitFeedbacks()
    {
        unitHitFeedbacks.PlayFeedbacks();
    }

    public void PlayLevelStartFeedbacks()
    {
        levelStartFeedbacks.PlayFeedbacks();
    }

    public void PlayCoinFeedbacks()
    {
        coinFeedbacks.PlayFeedbacks();
    }

    public void PlayUnitDeadFeedbacks()
    {
        unitDeadFeedbacks.PlayFeedbacks();
    }

    public void PlayClickFeedbacks()
    {
        clickFeedbacks.PlayFeedbacks();
    }
}