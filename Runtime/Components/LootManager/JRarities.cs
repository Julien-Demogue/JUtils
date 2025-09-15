using UnityEngine;

[CreateAssetMenu(fileName = "Rarities", menuName = "JUtils/LootSystem/Rarities")]
/// <summary>
/// ScriptableObject that holds a list of all possible loot rarities and provides methods for random selection and lookup.
/// </summary>
public class JRarities : ScriptableObject
{
    public JRarity[] ListRarities;

    /// <summary>
    /// Calculates the total weight of all rarities.
    /// </summary>
    /// <returns>The sum of all rarity weights.</returns>
    public int GetTotalWeight()
    {
        int totalWeight = 0;
        foreach (JRarity rarity in ListRarities)
        {
            totalWeight += rarity.Weight;
        }
        return totalWeight;
    }

    /// <summary>
    /// Finds a rarity by its name.
    /// </summary>
    /// <param name="name">The name of the rarity.</param>
    /// <returns>The JRarity with the specified name, or null if not found.</returns>
    public JRarity GetRarityByName(string name)
    {
        foreach (JRarity rarity in ListRarities)
        {
            if (rarity.Name == name)
            {
                return rarity;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a random rarity from the list (uniform probability).
    /// </summary>
    /// <returns>A random JRarity, or null if the list is empty.</returns>
    public JRarity GetRandomRarity()
    {
        if (ListRarities.Length == 0) return null;
        int randomIndex = Random.Range(0, ListRarities.Length);
        return ListRarities[randomIndex];
    }

    /// <summary>
    /// Returns a random rarity, weighted by each rarity's weight.
    /// </summary>
    /// <returns>A random JRarity, or null if the list is empty.</returns>
    public JRarity GetRandomRarityByWeight()
    {
        int totalWeight = GetTotalWeight();
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (JRarity rarity in ListRarities)
        {
            cumulativeWeight += rarity.Weight;
            if (randomValue < cumulativeWeight)
            {
                return rarity;
            }
        }
        return null;
    }
}
