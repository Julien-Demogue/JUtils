using System;
using UnityEngine;

/// <summary>
/// JSpatialSettingsPreset is a ScriptableObject that holds spatial audio settings.
/// </summary>
[CreateAssetMenu(fileName = "SpatialSettingsPreset", menuName = "Audio/Spatial Settings Preset")]
public class JSpatialSettingsPreset : ScriptableObject
{
    [Range(0f, 1f)] public float SpatialBlend;
    public float MinDist = 1;
    public float MaxDist = 500;
    [Range(0f, 5f)] public float DopplerLevel = 1;
    [Range(0, 360)] public int Spread = 0;
    public AudioRolloffMode VolumeRollOff = AudioRolloffMode.Logarithmic;
}