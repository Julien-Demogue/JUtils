using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// JAudioEditor is a utility class for managing audio assets in Unity.
/// It provides methods to compile audio items into a ScriptableObject and refresh audio settings for audio clips.
/// </summary>
public class JAudioEditor : MonoBehaviour
{
    const string AUDIO_FOLDER_PATH = "Assets/Audio";
    const string AUDIO_ITEMS_FILE_NAME = "JAudioItems.asset";

    private static List<AudioImporter> processedAudios = new List<AudioImporter>();


    /// <summary>
    /// Compiles all audio items in the Audio folder into a ScriptableObject.
    /// </summary>
    public static void CompileAudioItems()
    {
        // Search or create the ScriptableObject audio items
        string audioItemsPath = $"{AUDIO_FOLDER_PATH}/{AUDIO_ITEMS_FILE_NAME}";
        if (!AssetDatabase.IsValidFolder(AUDIO_FOLDER_PATH))
        {
            AssetDatabase.CreateFolder("Assets", "Audio");
            JDebug.LogYellow($"Folder created at: {AUDIO_FOLDER_PATH}");
        }
        JAudioItems audioItems = AssetDatabase.LoadAssetAtPath<JAudioItems>(audioItemsPath);
        if (audioItems == null)
        {
            audioItems = ScriptableObject.CreateInstance<JAudioItems>();
            AssetDatabase.CreateAsset(audioItems, audioItemsPath);
            JDebug.LogYellow($"ScriptableObject JAudioItems created at: {audioItemsPath}");
        }

        // Compile ScriptableObjects into JAudioItems
        string[] assetPaths = AssetDatabase.FindAssets("t:ScriptableObject", new[] { AUDIO_FOLDER_PATH });

        audioItems.Items.Clear();
        foreach (string guid in assetPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            JAudioItem obj = AssetDatabase.LoadAssetAtPath<JAudioItem>(assetPath);
            if (obj != null)
            {
                audioItems.Items.Add(obj);
            }
        }

        // Save the ScriptableObject
        EditorUtility.SetDirty(audioItems);
        AssetDatabase.SaveAssets();

        JDebug.LogGreen($"Audio items compilation completed! {audioItems.Items.Count} items added.");
    }

    /// <summary>
    /// Refreshes all audio settings for audio clips in the project.
    /// This method is typically called from the Unity Editor to ensure all audio clips have the correct settings applied.
    /// </summary>
    public static void RefreshAudioSettings()
    {
        string[] audioGuids = AssetDatabase.FindAssets("t:AudioClip");
        processedAudios.Clear();
        foreach (string guid in audioGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
            if (audioImporter != null)
            {
                ApplyAudioSettings(audioImporter);
            }
        }
    }

    // <summary>
    /// Refreshes all audio settings for audio clips in the project.
    /// </summary>
    public static void ApplyAudioSettings(AudioImporter audioImporter)
    {
        if (processedAudios.Contains(audioImporter))
        {
            return;
        }

        AudioImporterSampleSettings settings = audioImporter.defaultSampleSettings;
        processedAudios.Add(audioImporter);
        AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioImporter.assetPath);
        if (audioClip != null)
        {
            float duration = audioClip.length;

            AudioClipLoadType loadType = duration > 10f ? AudioClipLoadType.Streaming : AudioClipLoadType.DecompressOnLoad;
            AudioCompressionFormat compressionFormat = duration > 10f ? AudioCompressionFormat.Vorbis : AudioCompressionFormat.PCM;
            float quality = duration > 10f ? 0.8f : 1.0f;
            bool loadInBackground = duration > 10f;

            if (
                settings.loadType == loadType &&
                settings.compressionFormat == compressionFormat &&
                settings.quality == quality &&
                audioImporter.loadInBackground == loadInBackground)
            {
                return;
            }

            settings.loadType = loadType;
            settings.compressionFormat = compressionFormat;
            settings.quality = quality;
            audioImporter.loadInBackground = loadInBackground;

            audioImporter.defaultSampleSettings = settings;
            audioImporter.SaveAndReimport();

            JDebug.LogGreen("Audio settings applied to: " + audioImporter.assetPath);
        }
    }
}
