using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "JUtils/LootSystem/Loot")]
/// <summary>
/// ScriptableObject representing a loot entry, including the item, quantity range, rarity, and drop weight.
/// </summary>
public class JLoot : ScriptableObject
{
    public string Id;
    public JItem Item;
    public int QuantityMax;
    public int QuantityMin;
    public JRarity Rarity;
    public int DropWeight;
}
