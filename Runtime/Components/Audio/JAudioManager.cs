using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// JAudioManager is a singleton class that manages audio playback in Unity.
/// It allows playing, stopping, pausing, and resuming audio items,
/// as well as fading audio in and out.
/// </summary>
public class JAudioManager : MonoBehaviour
{
    [SerializeField] private bool isPersistent = true;

    public static bool SoundFading;

    private List<JAudioItem> pausedAudioItems = new();

    [SerializeField] private JAudioItems audioItems;
    [SerializeField] private AudioMixerGroup generalMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup soundEffectsMixer;

    private Dictionary<string, JAudioItem> audioItemDictionary;
    private Dictionary<AudioSource, Coroutine> fadeInCoroutines = new();
    private Dictionary<AudioSource, Coroutine> fadeOutCoroutines = new();

    public static JAudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioItemDictionary = new Dictionary<string, JAudioItem>();
            foreach (JAudioItem audioItem in audioItems.Items)
            {
                audioItemDictionary[audioItem.name] = audioItem;
                CreateAudioSource(audioItem);
            }
            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Initializes the values of the AudioSource component for a given audio item.
    /// </summary>
    /// <param name="audioItem">The audio item to initialize.</param>
    private void InitializeAudioSourceValues(JAudioItem audioItem)
    {
        AudioSource source = audioItem.Source;

        source.clip = audioItem.Clips[Random.Range(0, audioItem.Clips.Length)];
        source.volume = audioItem.Volume;
        source.pitch = Random.Range(audioItem.Pitch - audioItem.PitchVariation, audioItem.Pitch + audioItem.PitchVariation);
        source.loop = audioItem.Loop;
        source.outputAudioMixerGroup = audioItem.MixerGroup;

        if (audioItem.SpatialPreset)
        {
            source.spatialBlend = audioItem.SpatialPreset.SpatialBlend;
            source.maxDistance = audioItem.SpatialPreset.MaxDist;
            source.minDistance = audioItem.SpatialPreset.MinDist;
            source.dopplerLevel = audioItem.SpatialPreset.DopplerLevel;
            source.spread = audioItem.SpatialPreset.Spread;
            source.rolloffMode = audioItem.SpatialPreset.VolumeRollOff;
        }

        if (audioItem.PlayOnAwake) { audioItem.Source.Play(); }
    }

    /// <summary>
    /// Creates an AudioSource component for a given audio item.
    /// </summary>
    /// <param name="audioItem">The audio item to create an AudioSource for.</param>
    private void CreateAudioSource(JAudioItem audioItem)
    {
        if (audioItem.Clips == null || audioItem.Clips.Length == 0)
        {
            Debug.LogWarning("The audio item " + audioItem.name + " doesn't have any clips");
            return;
        }

        if (!audioItem.CreateOnAwake)
        {
            return;
        }

        audioItem.Source = gameObject.AddComponent<AudioSource>();
        InitializeAudioSourceValues(audioItem);
    }

    /// <summary>
    /// Creates an AudioSource component for a given audio item on a target GameObject.
    /// </summary>
    /// <param name="name">The name of the audio item.</param>
    /// <param name="targetObject">The target GameObject to attach the AudioSource to.</param>
    private AudioSource CreateAudioSourceOnTarget(string name, GameObject targetObject)
    {
        if (audioItemDictionary.TryGetValue(name, out JAudioItem audioItem))
        {
            audioItem.Source = targetObject.AddComponent<AudioSource>();
            InitializeAudioSourceValues(audioItem);
            return audioItem.Source;
        }
        Debug.LogWarning("The audio item " + name + " doesn't exist");
        return null;
    }

    /// <summary>
    /// Checks if a audio item is currently playing.
    /// </summary>
    /// <param name="name">The name of the audio item.</param>
    /// <returns>True if the audio item is playing, false otherwise.</returns>
    public bool IsPlaying(string name)
    {
        if (audioItemDictionary.TryGetValue(name, out JAudioItem audioItem))
        {
            return audioItem.Source.isPlaying;
        }
        Debug.LogWarning("The audio item " + name + " doesn't exist");
        return false;
    }

    /// <summary>
    /// Plays a audio item with an option to randomize the clip.
    /// </summary>
    /// <param name="name">The name of the audio item.</param>
    /// <param name="randomizeClip">Whether to randomize the clip.</param>
    public void Play(string name, bool randomizeClip = false)
    {
        if (audioItemDictionary.TryGetValue(name, out JAudioItem audioItem))
        {
            if (audioItem.Source == null)
            {
                Debug.LogWarning("The audio item " + name + " doesn't have an AudioSource component.");
                return;
            }

            if (randomizeClip)
            {
                audioItem.Source.clip = audioItem.Clips[Random.Range(0, audioItem.Clips.Length)];
            }
            else
            {
                audioItem.Source.clip = audioItem.Clips[0];
            }
            audioItem.Source.pitch = Random.Range(audioItem.Pitch - audioItem.PitchVariation, audioItem.Pitch + audioItem.PitchVariation);

            StartCoroutine(FadeIn(audioItem.Source, audioItem.FadeIn));
            return;
        }
        Debug.LogWarning("The audio item " + name + " doesn't exist");
    }

    /// <summary>
    /// Plays an audio item on a target GameObject with an option to randomize the clip.
    /// </summary>
    /// <param name="name">The name of the audio item.</param>
    /// <param name="targetObject">The target GameObject to attach the AudioSource to.</param>
    /// <param name="randomizeClip">Whether to randomize the clip.</param>
    public void Play(string name, GameObject targetObject)
    {
        AudioSource audioSource = CreateAudioSourceOnTarget(name, targetObject);

        if (audioSource == null)
        {
            return;
        }

        Play(name, true);

        if (!audioSource.loop)
        {
            StartCoroutine(RemoveAudioSource(audioSource, audioSource.clip.length + 0.1f));
        }
    }

    /// <summary>
    /// Stops an audio item.
    /// </summary>
    /// /// <param name="name">The name of the audio item.</param>
    public void Stop(string name)
    {
        if (audioItemDictionary.TryGetValue(name, out JAudioItem audioItem))
        {
            StartCoroutine(FadeOut(audioItem.Source, audioItem.FadeOut));
            return;
        }
        Debug.LogWarning("The audio item " + name + " doesn't exist");
    }

    /// <summary>
    /// Stops all audio items.
    /// </summary>
    public void StopAll()
    {
        foreach (JAudioItem audioItem in audioItems.Items)
        {
            if (audioItem.Source != null && audioItem.Source.isPlaying)
            {
                StartCoroutine(FadeOut(audioItem.Source, audioItem.FadeOut));
            }
        }
    }

    /// <summary>
    /// Stops all sound effects.
    /// </summary> 
    public void StopAllSoundEffects()
    {
        foreach (JAudioItem audioItem in audioItems.Items)
        {
            if (audioItem.Source != null && audioItem.MixerGroup == soundEffectsMixer && audioItem.Source.isPlaying)
            {
                StartCoroutine(FadeOut(audioItem.Source, audioItem.FadeOut));
            }
        }
    }

    /// <summary>
    /// Stops all music.
    /// </summary> 
    public void StopAllMusic()
    {
        foreach (JAudioItem audioItem in audioItems.Items)
        {
            if (audioItem.Source != null && audioItem.MixerGroup == musicMixer && audioItem.Source.isPlaying)
            {
                StartCoroutine(FadeOut(audioItem.Source, audioItem.FadeOut));
            }
        }
    }

    /// <summary>
    /// Pauses an audio item.
    /// </summary>
    /// <param name="name">The name of the audio item.</param>
    public void Pause(string name)
    {
        if (audioItemDictionary.TryGetValue(name, out JAudioItem audioItem))
        {
            audioItem.Source.Pause();
            pausedAudioItems.Add(audioItem);
            return;
        }
        Debug.LogWarning("The audio item " + name + " doesn't exist");
    }

    /// <summary>
    /// Pauses all audio items.
    /// </summary>
    public void PauseAll()
    {
        foreach (JAudioItem audioItem in audioItems.Items)
        {
            if (audioItem.Source != null && audioItem.Source.isPlaying)
            {
                audioItem.Source.Pause();
                pausedAudioItems.Add(audioItem);
            }
        }
    }

    /// <summary>
    /// Pauses all sound effects.
    /// </summary>
    public void PauseAllSoundEffects()
    {
        foreach (JAudioItem audioItem in audioItems.Items)
        {
            if (audioItem.Source != null && audioItem.Source.isPlaying && audioItem.MixerGroup == soundEffectsMixer)
            {
                audioItem.Source.Pause();
                pausedAudioItems.Add(audioItem);
            }
        }
    }

    /// <summary>
    /// Pauses all music.
    /// </summary>
    public void PauseAllMusic()
    {
        foreach (JAudioItem audioItem in audioItems.Items)
        {
            if (audioItem.Source != null && audioItem.Source.isPlaying && audioItem.MixerGroup == musicMixer)
            {
                audioItem.Source.Pause();
                pausedAudioItems.Add(audioItem);
            }
        }
    }

    /// <summary>
    /// Resumes all paused audio items.
    /// </summary>
    public void ResumeAll()
    {
        foreach (JAudioItem audioItem in pausedAudioItems)
        {
            if (audioItem.Source != null && audioItem.Source.isPlaying)
            {
                Play(audioItem.name, false);
            }
        }
        pausedAudioItems = new List<JAudioItem>();
    }

    /// <summary>
    /// Resumes all paused sound effects.
    /// </summary>
    public void ResumeAllSoundEffects()
    {
        foreach (JAudioItem audioItem in pausedAudioItems)
        {
            if (audioItem.Source != null && audioItem.MixerGroup == soundEffectsMixer)
            {
                Play(audioItem.name, false);
            }
        }
        pausedAudioItems = new List<JAudioItem>();
    }

    /// <summary>
    /// Resumes all paused music.
    /// </summary>
    public void ResumeAllMusic()
    {
        foreach (JAudioItem audioItem in pausedAudioItems)
        {
            if (audioItem.Source != null && audioItem.MixerGroup == musicMixer)
            {
                Play(audioItem.name, false);
            }
        }
        pausedAudioItems = new List<JAudioItem>();
    }

    /// <summary>
    /// Fades in an AudioSource over a specified duration.
    /// </summary>
    /// <param name="audioSource">The AudioSource to fade in.</param>
    /// <param name="fadeDuration">The duration of the fade.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator FadeIn(AudioSource audioSource, float fadeDuration)
    {
        if (fadeOutCoroutines.ContainsKey(audioSource))
        {
            StopCoroutine(fadeOutCoroutines[audioSource]);
            fadeOutCoroutines.Remove(audioSource);
        }

        if (fadeInCoroutines.ContainsKey(audioSource))
        {
            StopCoroutine(fadeInCoroutines[audioSource]);
        }

        float startVolume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.Play();

        SoundFading = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }

        SoundFading = false;

        audioSource.volume = startVolume;
        fadeInCoroutines.Remove(audioSource);
    }

    /// <summary>
    /// Fades out an AudioSource over a specified duration.
    /// </summary>
    /// <param name="audioSource">The AudioSource to fade out.</param>
    /// <param name="fadeDuration">The duration of the fade.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        if (fadeInCoroutines.ContainsKey(audioSource))
        {
            StopCoroutine(fadeInCoroutines[audioSource]);
            fadeInCoroutines.Remove(audioSource);
        }

        if (fadeOutCoroutines.ContainsKey(audioSource))
        {
            StopCoroutine(fadeOutCoroutines[audioSource]);
        }

        float startVolume = audioSource.volume;

        SoundFading = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        SoundFading = false;

        audioSource.Stop();
        audioSource.volume = startVolume;

        if (audioSource.transform.parent != this.transform)
        {
            Destroy(audioSource);
        }

        fadeOutCoroutines.Remove(audioSource);
    }

    /// <summary>
    /// Removes an AudioSource from a GameObject after a delay.
    /// </summary>
    /// <param name="audioSource">The AudioSource to remove.</param>
    /// <param name="delay">The delay before removing the AudioSource.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator RemoveAudioSource(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(audioSource);
    }

    /// <summary>
    /// Sets the general volume for all audio items of type General.
    /// </summary>
    /// <param name="volume">The volume to set.</param>
    public void SetGeneralVolume(float volume)
    {
        generalMixer.audioMixer.SetFloat(generalMixer.name, CalculateVolume(volume));
    }

    /// <summary>
    /// Sets the volume for all audio items of type Music.
    /// </summary>
    /// <param name="volume">The volume to set.</param>
    public void SetMusicVolume(float volume)
    {
        musicMixer.audioMixer.SetFloat(musicMixer.name, CalculateVolume(volume));
    }

    /// <summary>
    /// Sets the volume for all audio items of type SoundEffect.
    /// </summary>
    /// <param name="volume">The volume to set.</param>
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsMixer.audioMixer.SetFloat(soundEffectsMixer.name, CalculateVolume(volume));
    }

    /// <summary>
    /// Calculates the logarithmic volume value for the AudioMixer.
    /// </summary>
    /// <param name="volume">The linear volume value (0 to 1).</param>
    /// <returns>The logarithmic volume value for the AudioMixer.</returns>
    private float CalculateVolume(float volume)
    {
        if (volume <= 0)
        {
            return -80f;
        }
        else
        {
            return Mathf.Log10(Mathf.Pow(volume, 2f)) * 20;
        }
    }

    /// <summary>
    /// Gets a specific audio item by name.
    /// </summary>
    public JAudioItem GetAudioItem(string name)
    {
        if (audioItemDictionary.TryGetValue(name, out JAudioItem audioItem))
        {
            return audioItem;
        }
        Debug.LogWarning("The audio item " + name + " doesn't exist");
        return null;
    }
}
