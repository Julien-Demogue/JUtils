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
[CustomEditor(typeof(JAudioItems))]
public class JAudioItemsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JAudioItems audioItems = (JAudioItems)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Fill Audio Items"))
        {
            FillAudioItems(audioItems);
        }
    }

    private void FillAudioItems(JAudioItems audioItems)
    {
        const string audioItemsFolderPath = "Assets/Audio";

        string[] assetPaths = AssetDatabase.FindAssets("t:ScriptableObject", new[] { audioItemsFolderPath });

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

        EditorUtility.SetDirty(audioItems);
        AssetDatabase.SaveAssets();

        JDebug.LogGreen($"Audio items filled! {audioItems.Items.Count} items added.");
    }
}
#endif
