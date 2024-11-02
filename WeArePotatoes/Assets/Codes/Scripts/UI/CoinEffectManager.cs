using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using Farou.Utility;

public class CoinEffectManager : Singleton<CoinEffectManager>
{
    [SerializeField] private Transform coinStartPosition;
    [SerializeField] private Transform goldCoinEndPosition;
    [SerializeField] private Transform azureCoinEndPosition;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease scaleEase = Ease.InSine;
    [SerializeField] private Ease moveEase = Ease.InBack;
    [SerializeField] private int coinAmount;
    [SerializeField] private float coinSpawnTotalDelay;

    private float coinSpawnDelay;
    private float totalGoldCoinCollected;
    private float totalAzureCoinCollected;

    private void Start()
    {
        totalGoldCoinCollected = GameDataManager.Instance.GoldCoinCollected;
        totalAzureCoinCollected = GameDataManager.Instance.AzureCoinCollected;

        if (totalGoldCoinCollected > 0)
        {
            StartSpawnCoins(CurrencyType.GoldCoin, totalGoldCoinCollected);
        }

        if (totalAzureCoinCollected > 0)
        {
            StartSpawnCoins(CurrencyType.AzureCoin, totalAzureCoinCollected);
        }
    }

    public void StartSpawnCoins(CurrencyType currencyType, float amount)
    {
        if (currencyType == CurrencyType.GoldCoin)
        {
            StartCoroutine(SpawnCoins(CurrencyType.GoldCoin, amount));
        }
        else
        {
            StartCoroutine(SpawnCoins(CurrencyType.AzureCoin, amount));
        }
    }

    private IEnumerator SpawnCoins(CurrencyType currencyType, float totalCoins)
    {
        AudioManager.Instance.PlayCoinSpawnFeedbacks();

        coinSpawnDelay = coinSpawnTotalDelay / coinAmount;

        // Calculate the integer part of each coin's amount
        float singleCoinAmount = totalCoins / (float)coinAmount;

        // Calculate the remainder that needs to be distributed
        float remainder = totalCoins % coinAmount;

        for (int i = 0; i < coinAmount; i++)
        {
            // Add 1 to some coins to account for the remainder
            float amountToAdd = singleCoinAmount + (i < remainder ? 1 : 0);
            SpawnCoin(currencyType, amountToAdd, i * coinSpawnDelay);
        }

        yield return new WaitForSeconds(coinAmount * coinSpawnDelay);

        GameDataManager.Instance.ClearCoinCollected();
    }

    private void SpawnCoin(CurrencyType currencyType, float coinAmount, float delay)
    {
        Image spawnedCoinImage = UIEffectObjectPool.Instance.GetPooledObject(currencyType);

        // Generate a random offset to scatter the coins
        var offset = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0);
        var startPosition = coinStartPosition.transform.position + offset;
        spawnedCoinImage.transform.position = startPosition;

        Transform endPosition = currencyType == CurrencyType.GoldCoin ? goldCoinEndPosition : azureCoinEndPosition;

        // Set initial scale to 0 (shrunk) and then animate to full size
        spawnedCoinImage.transform.localScale = Vector3.zero;
        spawnedCoinImage.transform.DOScale(Vector3.one, delay)
            .SetDelay(0.5f)
            .SetEase(scaleEase)
            .OnComplete(() =>
            {
                // Animate the coin to move towards the end position
                spawnedCoinImage.transform.DOMove(endPosition.position, moveDuration)
                    .SetEase(moveEase)
                    .OnComplete(() =>
                    {
                        // Return the coin image to the pool and add the coin amount
                        UIEffectObjectPool.Instance.ReturnToPool(currencyType, spawnedCoinImage);
                        GameDataManager.Instance.ModifyCoin(currencyType, coinAmount);
                        AudioManager.Instance.PlayCoinAddedFeedbacks();
                    });
            });
    }
}
