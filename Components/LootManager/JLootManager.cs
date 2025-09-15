using UnityEngine;

/// <summary>
/// Manages loot generation and retrieval based on defined rarities and loot items.
/// </summary>
public class JLootManager : MonoBehaviour
{
    [SerializeField] private bool isPersistent = true;

    [Space]
    [SerializeField] private JRarities rarities;
    [SerializeField] private JLoots loots;

    public static JLootManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Retrieves a random loot item based on the defined rarities and weights.
    /// </summary>
    /// <returns>A random JLoot item or null if no rarities are defined.</returns>
    public JLoot GetRandomLoot()
    {
        JRarity rarity = rarities.GetRandomRarityByWeight();
        if (rarity == null) return null;

        return loots.GetRandomLootByRarityAndWeight(rarity);
    }

    /// <summary>
    /// Retrieves a loot item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the loot item.</param>
    /// <returns>The JLoot item with the specified ID or null if not found.</returns>
    public JLoot GetLootById(string id)
    {
        return loots.GetLootById(id);
    }

    /// <summary>
    /// Retrieves a rarity by its name.
    /// </summary>
    /// <param name="name">The name of the rarity.</param>
    /// <returns>The JRarity with the specified name or null if not found.</returns>
    public JRarity GetRarityByName(string name)
    {
        return rarities.GetRarityByName(name);
    }

    /// <summary>
    /// Retrieves a random loot item of a specific rarity by its name.
    /// </summary>
    /// <param name="name">The name of the rarity.</param>
    /// <returns>A random JLoot item of the specified rarity or null if the rarity is not found.</returns>
    public JLoot GetRandomLootByRarityName(string name)
    {
        JRarity rarity = rarities.GetRarityByName(name);
        if (rarity == null) return null;

        return loots.GetRandomLootByRarity(rarity);
    }
}
