using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class MonetizationManager : Singleton<MonetizationManager>
{
    private void Start()
    {
        Gley.MobileAds.API.Initialize();
    }

    public void ShowRewardedVideo(Action action)
    {
        Gley.MobileAds.API.ShowRewardedVideo((bool completed) =>
        {
            action?.Invoke();
        });
    }
}
