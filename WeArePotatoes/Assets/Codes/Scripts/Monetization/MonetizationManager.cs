using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using Gley.EasyIAP;
using UnityEngine;

public class MonetizationManager : Singleton<MonetizationManager>
{
    [SerializeField] private ShopUI shopUI;

    private void Start()
    {
        Gley.MobileAds.API.Initialize();
        Gley.EasyIAP.API.Initialize(InitializeComplete);
    }

    private void InitializeComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        if (status == IAPOperationStatus.Success)
        {
            shopUI?.RefreshUI();
        }
    }

    public void ShowRewardedVideo(Action onCompleted, Action onSkipped)
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