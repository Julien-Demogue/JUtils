using UnityEngine;
using UnityEngine.Audio;
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

