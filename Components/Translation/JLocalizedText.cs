using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

public class JLocalizedText : TextMeshProUGUI
{
    [SerializeField] private string translationKey;
    [SerializeField] private string[] parameters;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif

        JTranslations.OnLanguageChanged += UpdateLocalizedText;
        UpdateLocalizedText();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif

        JTranslations.OnLanguageChanged -= UpdateLocalizedText;
    }

    /// <summary>
    /// Updates the text based on the current language, translation key, and parameters.
    /// </summary>
    public void UpdateLocalizedText()
    {
        if (!string.IsNullOrEmpty(translationKey))
        {
            if (parameters != null && parameters.Length > 0)
                text = JTranslations.Get(translationKey, parameters);
            else
                text = JTranslations.Get(translationKey);
        }
    }

    /// <summary>
    /// Sets the parameters to inject into the translation and updates the text.
    /// </summary>
    /// <param name="values">The values to inject into the translation string.</param>
    public void SetParameters(params string[] values)
    {
        parameters = values;
        UpdateLocalizedText();
    }
}

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