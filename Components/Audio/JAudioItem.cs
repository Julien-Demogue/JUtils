using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System.Linq;
using System;

/// <summary>
/// JAudioItem is a ScriptableObject that represents an audio item in Unity.    
/// </summary>
[CreateAssetMenu(fileName = "AudioItem", menuName = "Audio/Audio item")]
public class JAudioItem : ScriptableObject
{
    [Header("General")]
    public bool CreateOnAwake = true;
    public AudioClip[] Clips;
    public AudioMixerGroup MixerGroup;

    [Range(0f, 1f)]
    public float Volume = 1;
    [Range(0.1f, 3f)]
    public float Pitch = 1;
    public float PitchVariation;

    public bool Loop = false;
    public bool PlayOnAwake = false;

    [Header("Fade")]
    public float FadeIn = 0f;
    public float FadeOut = 0f;

    [HideInInspector]
    public AudioSource Source;

    [Header("Spatial settings")]
    [HideInInspector] public JSpatialSettingsPreset SpatialPreset;

    public void ResetValues()
    {
        Volume = 1;
        Pitch = 1;
        PitchVariation = 0;
        Loop = false;
        PlayOnAwake = false;
        FadeIn = 0;
        FadeOut = 0;
        SpatialPreset = null;
    }

#if UNITY_EDITOR
    [HideInInspector] public AudioSource PreviewSource;
    [HideInInspector] public float PreviewTime = 0f;
    [HideInInspector] public int SelectedClipIndex = 0; // Index of the selected clip

    public void PlayPreview()
    {
        if (PreviewSource == null)
        {
            CreatePreviewSource();
        }

        if (IsValidClipIndex(SelectedClipIndex))
        {
            ConfigurePreviewSource();
            PreviewSource.Play();
        }
    }

    private void CreatePreviewSource()
    {
        if (GameObject.Find("AudioPreview") != null)
        {
            DestroyImmediate(GameObject.Find("AudioPreview"));
        }

        GameObject previewObject = new GameObject("AudioPreview");
        PreviewSource = previewObject.AddComponent<AudioSource>();
        PreviewSource.hideFlags = HideFlags.HideAndDontSave;
    }

    private bool IsValidClipIndex(int index)
    {
        return Clips != null && Clips.Length > 0 && index >= 0 && index < Clips.Length && Clips[index] != null;
    }

    private void ConfigurePreviewSource()
    {
        PreviewSource.clip = Clips[SelectedClipIndex];
        PreviewSource.outputAudioMixerGroup = MixerGroup;
        PreviewSource.volume = Volume;
        PreviewSource.pitch = Pitch + UnityEngine.Random.Range(-PitchVariation, PitchVariation);
        PreviewSource.loop = Loop;

        PreviewSource.time = PreviewTime;
    }

    public void UpdatePreviewTime(float time)
    {
        if (PreviewSource != null && PreviewSource.isPlaying)
        {
            PreviewSource.time = time;
        }
        PreviewTime = time;
    }

    public float GetPreviewLength()
    {
        if (PreviewSource != null && PreviewSource.clip != null)
        {
            return PreviewSource.clip.length;
        }
        else if (Clips != null && Clips.Length > 0 && SelectedClipIndex >= 0 && SelectedClipIndex < Clips.Length && Clips[SelectedClipIndex] != null)
        {
            return Clips[SelectedClipIndex].length;
        }
        return 0f;
    }

    public void PausePreview()
    {
        if (PreviewSource != null)
        {
            PreviewTime = PreviewSource.time;
            PreviewSource.Pause();
        }
    }

    public void StopPreview()
    {
        if (PreviewSource != null)
        {
            PreviewSource.Stop();
            PreviewTime = 0f;
            DestroyPreviewSource();
        }
    }

    private void DestroyPreviewSource()
    {
        if (PreviewSource != null)
        {
            DestroyImmediate(PreviewSource.gameObject);
            PreviewSource = null;
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(JAudioItem))]
public class AudioItemEditor : Editor
{
    private int previousSelectedClipIndex = -1;
    private bool isPlayingPreview = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        JAudioItem audioItem = (JAudioItem)target;

        GUILayout.Space(10);

        GUILayout.Label("Spatial Preset", EditorStyles.boldLabel);

        string[] presetNames = new[] { "None" }
        .Concat(AssetDatabase.FindAssets("t:SpatialSettingsPreset")
            .Select(guid => AssetDatabase.LoadAssetAtPath<JSpatialSettingsPreset>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(preset => preset != null)
            .Select(preset => preset.name))
        .ToArray();

        JSpatialSettingsPreset[] presets = new JSpatialSettingsPreset[] { null }
            .Concat(AssetDatabase.FindAssets("t:SpatialSettingsPreset")
                .Select(guid => AssetDatabase.LoadAssetAtPath<JSpatialSettingsPreset>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(preset => preset != null))
            .ToArray();

        int currentIndex = Array.IndexOf(presets, audioItem.SpatialPreset);
        int selectedIndex = EditorGUILayout.Popup("Select Preset", currentIndex, presetNames);

        if (selectedIndex >= 0 && selectedIndex < presets.Length)
        {
            audioItem.SpatialPreset = presets[selectedIndex];
        }

        if (audioItem.SpatialPreset != null)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Spatial Blend", audioItem.SpatialPreset.SpatialBlend.ToString());
            EditorGUILayout.LabelField("Min Distance", audioItem.SpatialPreset.MinDist.ToString());
            EditorGUILayout.LabelField("Max Distance", audioItem.SpatialPreset.MaxDist.ToString());
            EditorGUILayout.LabelField("Doppler Level", audioItem.SpatialPreset.DopplerLevel.ToString());
            EditorGUILayout.LabelField("Spread", audioItem.SpatialPreset.Spread.ToString());
            EditorGUILayout.LabelField("Volume Roll Off", audioItem.SpatialPreset.VolumeRollOff.ToString());
        }

        GUILayout.Space(20);

        GUILayout.Label("Preview", EditorStyles.boldLabel);

        GUILayout.Space(10);

        if (audioItem.Clips != null && audioItem.Clips.Length > 0)
        {
            string[] clipNames = new string[audioItem.Clips.Length];
            for (int i = 0; i < audioItem.Clips.Length; i++)
            {
                clipNames[i] = audioItem.Clips[i] != null ? audioItem.Clips[i].name : "Unnamed Clip";
            }

            audioItem.SelectedClipIndex = EditorGUILayout.Popup("Select Clip", audioItem.SelectedClipIndex, clipNames);

            if (audioItem.SelectedClipIndex != previousSelectedClipIndex)
            {
                audioItem.PreviewTime = 0f;
                previousSelectedClipIndex = audioItem.SelectedClipIndex;
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Play Preview"))
        {
            audioItem.PlayPreview();
            isPlayingPreview = true;
        }

        if (GUILayout.Button("Pause Preview"))
        {
            audioItem.PausePreview();
            isPlayingPreview = false;
        }

        if (GUILayout.Button("Stop Preview"))
        {
            audioItem.StopPreview();
            isPlayingPreview = false;
        }

        if (GUILayout.Button("Reset Preview Time"))
        {
            audioItem.PreviewTime = 0f;
            if (audioItem.PreviewSource != null)
            {
                audioItem.PreviewSource.time = 0f;
            }
        }

        GUILayout.Space(10);

        float previewLength = audioItem.GetPreviewLength();
        float currentTime = audioItem.PreviewSource != null ? audioItem.PreviewSource.time : audioItem.PreviewTime;

        string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }

        GUILayout.Label($"Time: {FormatTime(currentTime)} / {FormatTime(previewLength)}");
        float newTime = GUILayout.HorizontalSlider(currentTime, 0, previewLength);

        if (Mathf.Abs(newTime - currentTime) > 0.01f)
        {
            audioItem.UpdatePreviewTime(newTime);
        }

        if (isPlayingPreview && audioItem.PreviewSource != null && audioItem.PreviewSource.isPlaying)
        {
            Repaint();
        }

        GUILayout.Space(50);

        if (GUILayout.Button("Reset Values"))
        {
            audioItem.ResetValues();
        }
    }

    private void OnDisable()
    {
        JAudioItem audioItem = (JAudioItem)target;
        audioItem.StopPreview();
        isPlayingPreview = false;
    }

    private void OnDestroy()
    {
        JAudioItem audioItem = (JAudioItem)target;
        audioItem.StopPreview();
        isPlayingPreview = false;
    }
}
#endif

