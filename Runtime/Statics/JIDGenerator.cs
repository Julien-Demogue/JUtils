using System;

/// <summary>
/// JIDGenerator is a utility class for generating unique IDs.
/// </summary> 
public class JIDGenerator
{
    private static int currentIdNumber = 0;

    /// <summary>
    /// Generates a unique integer ID number.
    /// Each call to this method will return a new ID number, starting from 0 and incrementing by 1.
    /// </summary>
    /// <returns>A unique integer ID number.</returns>
    /// remarks>
    /// This method is only suitable for scenarios where a simple incrementing ID is sufficient.
    /// </remarks>
    public static int GenerateIdNumber()
    {
        return currentIdNumber++;
    }

    /// <summary>
    /// Generates a unique string ID with an optional prefix.   
    /// Each call to this method will return a new ID string based on a GUID.
    /// </summary>
    /// <param name="prefix">An optional prefix to prepend to the generated ID string.</param>
    /// <returns>A unique string ID.</returns>  
    public static string GenerateIdString(string prefix = "")
    {
        string guid = Guid.NewGuid().ToString("N");
        return string.IsNullOrEmpty(prefix)
            ? guid
            : $"{prefix}_{guid}";
    }
}