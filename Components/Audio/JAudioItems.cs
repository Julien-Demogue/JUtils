using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// JAudioItems is a ScriptableObject that holds a list of JAudioItem objects.
/// </summary>
[CreateAssetMenu(fileName = "AudioItems", menuName = "Audio/Audio items")]
public class JAudioItems : ScriptableObject
{
    public List<JAudioItem> Items = new();
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioItems))]
public class AudioItemsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioItems audioItems = (AudioItems)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Fill Audio Items"))
        {
            FillAudioItems(audioItems);
        }
    }

    private void FillAudioItems(AudioItems audioItems)
    {
        const string audioItemsFolderPath = "Assets/Audio";

        string[] assetPaths = AssetDatabase.FindAssets("t:ScriptableObject", new[] { audioItemsFolderPath });

        audioItems.items.Clear();
        foreach (string guid in assetPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AudioItem obj = AssetDatabase.LoadAssetAtPath<AudioItem>(assetPath);
            if (obj != null)
            {
                audioItems.items.Add(obj);
            }
        }

        EditorUtility.SetDirty(audioItems);
        AssetDatabase.SaveAssets();

        JDebug.LogGreen($"Audio items filled! {audioItems.items.Count} items added.");
    }
}
#endif
