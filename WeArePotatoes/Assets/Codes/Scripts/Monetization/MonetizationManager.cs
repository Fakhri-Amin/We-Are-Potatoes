using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using Gley.EasyIAP;
using UnityEngine;

public class MonetizationManager : Singleton<MonetizationManager>
{
    [SerializeField] private ShopUI shopUI;

    private bool isIAPInitialized = false; // Flag to check initialization

    public new void Awake()
    {
        base.Awake();

        Gley.MobileAds.API.Initialize();
        API.Initialize(InitializeComplete);
    }

    private void InitializeComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        if (status == IAPOperationStatus.Success)
        {
            isIAPInitialized = true;
            shopUI.RefreshPriceUI();
        }
        else
        {
            Debug.LogError("IAP Initialization failed: " + message);
        }
    }

    public void ShowRewardedVideo(Action onCompleted, Action onSkipped, Action onFailed = null)
    {
        if (Gley.MobileAds.API.IsRewardedVideoAvailable()) // Check if ad is ready
        {
            Gley.MobileAds.API.ShowRewardedVideo((bool completed) =>
            {
                if (completed)
                {
                    onCompleted?.Invoke();
                }
                else
                {
                    onSkipped?.Invoke();
                }
            });
        }
        else
        {
            onFailed?.Invoke(); // Handle failure case
        }
    }

    public bool IsIAPInitialized()
    {
        return isIAPInitialized;
    }


    public void WatchAdsAzure()
    {
        ShowRewardedVideo(() =>
        {
            CoinEffectManager.Instance.StartSpawnCoins(CurrencyType.AzureCoin, API.GetValue(ShopProductNames.AdsAzure));
        },
        () =>
        {
            FloatingTextObjectPool.Instance.DisplayAzureSkippedAds();
        });
    }

    public void BuySmallAzure()
    {
        Gley.EasyIAP.API.BuyProduct(ShopProductNames.SmallAzure, GetCompleteMethod);
    }

    public void BuyMediumAzure()
    {
        Gley.EasyIAP.API.BuyProduct(ShopProductNames.MediumAzure, GetCompleteMethod);
    }

    public void BuyBigAzure()
    {
        Gley.EasyIAP.API.BuyProduct(ShopProductNames.BigAzure, GetCompleteMethod);
    }

    private void GetCompleteMethod(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (status == IAPOperationStatus.Success)
        {
            CoinEffectManager.Instance.StartSpawnCoins(CurrencyType.AzureCoin, product.value);
        }
        else
        {
            Debug.Log(message);
        }
    }
}