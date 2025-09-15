using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the game settings.
/// </summary>
public class JSettingsManager : MonoBehaviour
{
    [Serializable]
    public class SettingsData
    {
        public JGraphicsSettings.GraphicsSettingsData GraphicsSettings;
        public JAudioSettings.AudioSettingsData AudioSettings;
        public JGameplaySettings.GameplaySettingsData GameplaySettings;
    }

    public JGraphicsSettings GraphicsSettings;
    public JAudioSettings AudioSettings;
    public JGameplaySettings GameplaySettings;

    private List<JSettings> allSettings = new();

    public static JSettingsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (GraphicsSettings) allSettings.Add(GraphicsSettings);
        if (AudioSettings) allSettings.Add(AudioSettings);
        if (GameplaySettings) allSettings.Add(GameplaySettings);
    }

    private void Start()
    {
        ResetSettings();
        LoadSettings();
    }

    /// <summary>
    /// Loads settings from a JSON file and applies them to the respective settings components.
    /// </summary>
    public void LoadSettings()
    {
        try
        {
            string settingsJson = JFileSystem.LoadGameSettings();
            var settingsData = JJson.Deserialize<Dictionary<string, object>>(settingsJson);

            foreach (JSettings setting in allSettings)
            {
                string key = setting.GetType().Name;
                if (settingsData.ContainsKey(key))
                {
                    setting.FromData(settingsData[key]);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load settings: {ex.Message}");
        }
    }

    /// <summary>
    /// Saves the current settings to a JSON file.
    /// </summary>
    public void SaveSettings()
    {
        try
        {
            var settingsData = new SettingsData
            {
                GraphicsSettings = GraphicsSettings != null ? (JGraphicsSettings.GraphicsSettingsData)GraphicsSettings.ToData() : null,
                AudioSettings = AudioSettings != null ? (JAudioSettings.AudioSettingsData)AudioSettings.ToData() : null,
                GameplaySettings = GameplaySettings != null ? (JGameplaySettings.GameplaySettingsData)GameplaySettings.ToData() : null
            };

            string settingsJson = JJson.Serialize(settingsData);
            Debug.Log(settingsJson);
            JFileSystem.SaveGameSettings(settingsJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save settings: {ex.Message}");
        }
    }

    /// <summary>
    /// Applies the current settings to the respective settings components.
    /// </summary>
    public void ApplySettings()
    {
        foreach (JSettings setting in allSettings)
        {
            setting.Apply();
        }
    }

    /// <summary>
    /// Resets the settings to their default values.
    /// </summary>
    public void ResetSettings()
    {
        foreach (JSettings setting in allSettings)
        {
            setting.Reset();
        }
    }
}