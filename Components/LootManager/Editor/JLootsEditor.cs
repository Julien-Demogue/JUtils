using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JLoots))]
public class JLootsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh Loots"))
        {
            JLoots loots = (JLoots)target;
            string[] guids = AssetDatabase.FindAssets("t:JLoot");
            JLoot[] allLoots = new JLoot[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                allLoots[i] = AssetDatabase.LoadAssetAtPath<JLoot>(path);
            }
            loots.ListLoots = allLoots;
            EditorUtility.SetDirty(loots);
        }
    }
}
