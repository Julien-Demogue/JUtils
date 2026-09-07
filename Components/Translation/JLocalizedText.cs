using TMPro;
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