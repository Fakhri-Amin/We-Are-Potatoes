using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CoinEffectManager : MonoBehaviour
{
    [SerializeField] private Transform coinStartPosition;
    [SerializeField] private Transform coinEndPosition;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease scaleEase = Ease.InSine;
    [SerializeField] private Ease moveEase = Ease.InBack;
    [SerializeField] private int coinAmount;
    [SerializeField] private float coinSpawnTotalDelay;

    private float coinSpawnDelay;
    private int totalCollectedCoins;

    private void Start()
    {
        totalCollectedCoins = GameDataManager.Instance.CoinCollected;

        if (totalCollectedCoins <= 0) return;
        StartCoroutine(OnCoinSpawn());
    }

    private IEnumerator OnCoinSpawn()
    {
        yield return new WaitForSeconds(0f);

        AudioManager.Instance.PlayCoinSpawnFeedbacks();

        coinSpawnDelay = coinSpawnTotalDelay / coinAmount;

        // Calculate the integer part of each coin's amount
        float singleCoinAmount = totalCollectedCoins / coinAmount;

        // Calculate the remainder that needs to be distributed
        float remainder = totalCollectedCoins % coinAmount;

        for (int i = 0; i < coinAmount; i++)
        {
            // Add 1 to some coins to account for the remainder
            float amountToAdd = singleCoinAmount + (i < remainder ? 1 : 0);
            SpawnCoin(amountToAdd, i * coinSpawnDelay);
        }

        GameDataManager.Instance.ClearCoinCollected();
    }


    private void SpawnCoin(float singleCoinAmount, float delay)
    {
        var spawnedCoinImage = UIEffectObjectPool.Instance.GetPooledObject(CurrencyType.GoldCoin);

        var offset = new Vector3(Random.Range(-80f, 80f), Random.Range(-80f, 80f), 0);
        var startPosition = coinStartPosition.transform.position + offset;
        spawnedCoinImage.transform.position = startPosition;

        spawnedCoinImage.transform.localScale = new Vector3(0, 0, 0);
        spawnedCoinImage.transform.DOScale(Vector3.one, delay).SetDelay(0.5f).SetEase(scaleEase).OnComplete(() =>
        {
            spawnedCoinImage.transform.DOMove(coinEndPosition.position, moveDuration).SetEase(moveEase).OnComplete(() =>
            {
                UIEffectObjectPool.Instance.ReturnToPool(CurrencyType.GoldCoin, spawnedCoinImage);
                GameDataManager.Instance.ModifyGoldCoin(singleCoinAmount);
                AudioManager.Instance.PlayCoinAddedFeedbacks();
            });
        });
    }
}
