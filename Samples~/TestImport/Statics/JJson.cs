using UnityEngine;

/// <summary>
/// JJson provides utility methods for serializing and deserializing objects to and from JSON using JsonUtility.
/// </summary>
public class JJson
{
    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    public static string Serialize(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    /// <summary>
    /// Deserializes a JSON string to an object of the specified type.
    /// </summary>
    public static T Deserialize<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// Deserializes a JSON string to an existing object.
    /// </summary>
    public static void Deserialize<T>(string json, T obj)
    {
        JsonUtility.FromJsonOverwrite(json, obj);
    }
}
