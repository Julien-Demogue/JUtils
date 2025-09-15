using UnityEngine;

public abstract class JSettings : MonoBehaviour
{
    private void Awake()
    {
        SaveDefaultSettingsValues();
    }

    /// <summary>
    /// Saves the default settings values.
    /// </summary>
    public abstract void SaveDefaultSettingsValues();

    /// <summary>
    /// Applies the current settings to the respective settings components.
    /// </summary>
    public abstract void Apply();

    /// <summary>
    /// Resets the settings to their default values.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Converts the settings to a data format suitable for saving.
    /// </summary>
    /// <returns> The data representation of the settings. </returns>
    public abstract object ToData();

    /// <summary>
    /// Populates the settings from a data format.
    /// </summary>
    /// <param name="data"> The data representation of the settings. </param>
    public abstract void FromData(object data);
}