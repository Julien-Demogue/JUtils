using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JAudioItems is a ScriptableObject that holds a list of JAudioItem objects.
/// </summary>
[CreateAssetMenu(fileName = "AudioItems", menuName = "Audio/Audio items")]
public class JAudioItems : ScriptableObject
{
    public List<JAudioItem> Items = new();
}
