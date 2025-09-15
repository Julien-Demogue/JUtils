#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JRandomSpawn))]
public class JRandomSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty spawnTypeProp = serializedObject.FindProperty("spawnType");
        EditorGUILayout.PropertyField(spawnTypeProp);

        JRandomSpawn.SpawnType spawnType = (JRandomSpawn.SpawnType)spawnTypeProp.enumValueIndex;
        if (spawnType == JRandomSpawn.SpawnType.AREA)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnAreaSize"));
        }
        else if (spawnType == JRandomSpawn.SpawnType.POINTS)
        {
            SerializedProperty pointsProp = serializedObject.FindProperty("spawnPoints");
            EditorGUILayout.PropertyField(pointsProp, true);

            if (GUILayout.Button("New point"))
            {
                JRandomSpawn randomSpawn = (JRandomSpawn)target;
                GameObject newPoint = new GameObject("SpawnPoint");
                newPoint.transform.parent = randomSpawn.transform;
                newPoint.transform.localPosition = Vector3.zero;

                var icon = EditorGUIUtility.IconContent("sv_label_3").image as Texture2D;
                EditorGUIUtility.SetIconForObject(newPoint, icon);

                pointsProp.arraySize++;
                pointsProp.GetArrayElementAtIndex(pointsProp.arraySize - 1).objectReferenceValue = newPoint.transform;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif