using System;
using UnityEngine;

public class JGameplaySettings : JSettings
{
    [Serializable]
    public class GameplaySettingsData
    {
        public bool ShowFPS;
        public JTranslations.Language CurrentLanguage;
    }

    public bool ShowFPS = false;
    private bool defaultShowFPS;

    public JTranslations.Language CurrentLanguage = JTranslations.Language.EN;
    private JTranslations.Language defaultLanguage;

    public override void SaveDefaultSettingsValues()
    {
        defaultShowFPS = ShowFPS;
        defaultLanguage = CurrentLanguage;
    }

    public override void Apply()
    {
        JTranslations.SetLanguage(CurrentLanguage);
    }

    public override void Reset()
    {
        ShowFPS = defaultShowFPS;
        CurrentLanguage = defaultLanguage;
    }

    public override object ToData()
    {
        return new GameplaySettingsData
        {
            ShowFPS = ShowFPS,
            CurrentLanguage = CurrentLanguage
        };
    }

    public override void FromData(object data)
    {
        if (data is GameplaySettingsData gameplayData)
        {
            ShowFPS = gameplayData.ShowFPS;
            CurrentLanguage = gameplayData.CurrentLanguage;
        }
    }
}