using UnityEngine;

[CreateAssetMenu(fileName = "Loots", menuName = "JUtils/LootSystem/Loots")]
/// <summary>
/// ScriptableObject representing a collection of loot entries and providing methods for weighted/random selection and lookup.
/// </summary>
public class JLoots : ScriptableObject
{
    public JLoot[] ListLoots;

    /// <summary>
    /// Calculates the total drop weight of all loot items.
    /// </summary>
    /// <returns>The total drop weight.</returns>
    public int GetTotalDropWeight()
    {
        int totalWeight = 0;
        foreach (JLoot loot in ListLoots)
        {
            totalWeight += loot.DropWeight;
        }
        return totalWeight;
    }

    /// <summary>
    /// Calculates the total drop weight for loot items of a specific rarity.
    /// </summary>
    /// <param name="rarity">The rarity to filter by.</param>
    /// <returns>The total drop weight for the specified rarity.</returns>
    public int GetTotalDropWeightByRarity(JRarity rarity)
    {
        int totalWeight = 0;
        foreach (JLoot loot in ListLoots)
        {
            if (loot.Rarity == rarity)
            {
                totalWeight += loot.DropWeight;
            }
        }
        return totalWeight;
    }

    /// <summary>
    /// Finds a loot entry by the item ID.
    /// </summary>
    /// <param name="id">The item ID to search for.</param>
    /// <returns>The JLoot with the specified item ID, or null if not found.</returns>
    public JLoot GetLootById(string id)
    {
        foreach (JLoot loot in ListLoots)
        {
            if (loot.Item.Id == id)
            {
                return loot;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a random loot entry from the list (uniform probability).
    /// </summary>
    /// <returns>A random JLoot, or null if the list is empty.</returns>
    public JLoot GetRandomLoot()
    {
        if (ListLoots.Length == 0) return null;
        int randomIndex = Random.Range(0, ListLoots.Length);
        return ListLoots[randomIndex];
    }

    /// <summary>
    /// Returns a random loot entry, weighted by each loot's drop weight.
    /// </summary>
    /// <returns>A random JLoot, or null if the list is empty.</returns>
    public JLoot GetRandomLootByWeight()
    {
        int totalWeight = GetTotalDropWeight();
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (JLoot loot in ListLoots)
        {
            cumulativeWeight += loot.DropWeight;
            if (randomValue < cumulativeWeight)
            {
                return loot;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a random loot entry of a specific rarity (uniform probability).
    /// </summary>
    /// <param name="rarity">The rarity to filter by.</param>
    /// <returns>A random JLoot of the specified rarity, or null if none found.</returns>
    public JLoot GetRandomLootByRarity(JRarity rarity)
    {
        JLoot[] filteredLoots = System.Array.FindAll(ListLoots, loot => loot.Rarity == rarity);
        if (filteredLoots.Length == 0) return null;

        int randomIndex = Random.Range(0, filteredLoots.Length);
        return filteredLoots[randomIndex];
    }

    /// <summary>
    /// Returns a random loot entry of a specific rarity, weighted by drop weight.
    /// </summary>
    /// <param name="rarity">The rarity to filter by.</param>
    /// <returns>A random JLoot of the specified rarity, or null if none found.</returns>
    public JLoot GetRandomLootByRarityAndWeight(JRarity rarity)
    {
        JLoot[] filteredLoots = System.Array.FindAll(ListLoots, loot => loot.Rarity == rarity);
        if (filteredLoots.Length == 0) return null;

        int totalWeight = GetTotalDropWeightByRarity(rarity);

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (JLoot loot in filteredLoots)
        {
            cumulativeWeight += loot.DropWeight;
            if (randomValue < cumulativeWeight)
            {
                return loot;
            }
        }
        return null;
    }
}