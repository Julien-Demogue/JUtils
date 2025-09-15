using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

public class JLocalizedText : TextMeshProUGUI
{
    [SerializeField] private string translationKey;

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
    /// Updates the text based on the current language and translation key.
    /// This method is called when the language changes or when the component is enabled.
    /// </summary>
    public void UpdateLocalizedText()
    {
        if (!string.IsNullOrEmpty(translationKey))
        {
            text = JTranslations.Get(translationKey);
        }
    }
}

[CustomEditor(typeof(JLocalizedText))]
public class JLocalizedTextEditor : TMP_EditorPanelUI
{
    SerializedProperty translationKey;
    SerializedProperty text;

    protected override void OnEnable()
    {
        base.OnEnable();
        translationKey = serializedObject.FindProperty("translationKey");
        text = serializedObject.FindProperty("m_text");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(translationKey);

        EditorGUILayout.BeginHorizontal();
        // Show text in FR
        if (GUILayout.Button("FR"))
        {
            JTranslations.SetLanguage(JTranslations.Language.FR);
            text.stringValue = JTranslations.Get(translationKey.stringValue);
        }
        // Show text in EN
        if (GUILayout.Button("EN"))
        {
            JTranslations.SetLanguage(JTranslations.Language.EN);
            text.stringValue = JTranslations.Get(translationKey.stringValue);
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}