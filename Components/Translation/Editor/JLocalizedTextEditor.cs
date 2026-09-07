using UnityEditor;
using UnityEngine;
using TMPro.EditorUtilities;

[CustomEditor(typeof(JLocalizedText))]
public class JLocalizedTextEditor : TMP_EditorPanelUI
{
    SerializedProperty translationKey;
    SerializedProperty parameters;
    SerializedProperty text;

    protected override void OnEnable()
    {
        base.OnEnable();
        translationKey = serializedObject.FindProperty("translationKey");
        parameters = serializedObject.FindProperty("parameters");
        text = serializedObject.FindProperty("m_text");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(translationKey);
        EditorGUILayout.PropertyField(parameters, true);

        EditorGUILayout.BeginHorizontal();
        // Show text in FR
        if (GUILayout.Button("FR"))
        {
            JTranslations.SetLanguage(JTranslations.Language.FR);
            text.stringValue = parameters != null && parameters.arraySize > 0
                ? JTranslations.Get(translationKey.stringValue, GetParametersArray())
                : JTranslations.Get(translationKey.stringValue);
        }
        // Show text in EN
        if (GUILayout.Button("EN"))
        {
            JTranslations.SetLanguage(JTranslations.Language.EN);
            text.stringValue = parameters != null && parameters.arraySize > 0
                ? JTranslations.Get(translationKey.stringValue, GetParametersArray())
                : JTranslations.Get(translationKey.stringValue);
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }

    private string[] GetParametersArray()
    {
        if (parameters == null || parameters.arraySize == 0)
        {
            return new string[0];
        }

        string[] arr = new string[parameters.arraySize];
        for (int i = 0; i < parameters.arraySize; i++)
        {
            arr[i] = parameters.GetArrayElementAtIndex(i).stringValue;
        }
        return arr;
    }
}