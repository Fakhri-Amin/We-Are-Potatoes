using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseBuildingSO", menuName = "Farou/Base Building")]
public class BaseBuildingSO : ScriptableObject
{
    public float SeedProductionRate;
    public int BaseHealth;
    public int BaseUpgradeSeedPrice;
    public int BaseUpgradeBaseHealthPrice;
    public float SeedProductionRateUpgradeAmount;
    public int BaseHealthUpgradeAmount;
    public int[] UpgradeSeedProductionRatePriceList = new int[30];
    public int[] UpgradeBaseHealthPriceList = new int[30];
    public int BaseDestroyedReward;

    private void OnValidate()
    {
#if UNITY_EDITOR
        // Calculate the prices for upgrading Seed Production Rate
        CalculateUpgradePrices(BaseUpgradeSeedPrice, UpgradeSeedProductionRatePriceList);

        // Calculate the prices for upgrading Base Health
        CalculateUpgradePrices(BaseUpgradeBaseHealthPrice, UpgradeBaseHealthPriceList);

        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private void CalculateUpgradePrices(int basePrice, int[] priceList)
    {
        if (priceList == null || priceList.Length == 0) return;

        priceList[0] = basePrice; // Set the first element as the base price

        // Calculate subsequent upgrade prices based on the formula: Current Price * 2 + (Current Price * 0.2)
        for (int i = 1; i < priceList.Length; i++)
        {

            // priceList[i] = Mathf.CeilToInt(priceList[i - 1] * 1.8f); // Adjust 1.15f to tweak progression rate

            // priceList[i] = Mathf.CeilToInt(basePrice * Mathf.Pow(i + 1, 1.5f)); // This will give quadratic growth
            priceList[i] = Mathf.CeilToInt(basePrice * Mathf.Pow(i + 1, 4f)); // This will give quadratic growth

            // priceList[i] = Mathf.CeilToInt(basePrice + (i * 50) + Mathf.Pow(i, 4f)); // Adjust 1.5f for higher or lower exponential growth

            // priceList[i] = Mathf.CeilToInt(basePrice * Mathf.Log(i + 15f)); // Logarithmic growth slows down as the levels increase
        }
    }
}
