using System;
using UnityEngine;

public class JGraphicsSettings : JSettings
{
    [Serializable]
    public class GraphicsSettingsData
    {
        public int TargetFramerate;
        public bool EnableVSync;
        public WindowMode CurrentWindowMode;
        public Vector2Int Resolution;
    }

    public enum WindowMode { Windowed, Fullscreen, Borderless }

    [Range(-1, 400)]
    public int TargetFramerate = 400;
    private int defaultTargetFramerate;

    public bool EnableVSync = false;
    private bool defaultEnableVSync;

    public WindowMode CurrentWindowMode = WindowMode.Windowed;
    private WindowMode defaultWindowMode;

    public Vector2Int Resolution = new Vector2Int(1920, 1080);
    private Vector2Int defaultResolution;

    public override void SaveDefaultSettingsValues()
    {
        defaultTargetFramerate = TargetFramerate;
        defaultEnableVSync = EnableVSync;
        defaultWindowMode = CurrentWindowMode;
        defaultResolution = Resolution;
    }

    public override void Apply()
    {
        Application.targetFrameRate = TargetFramerate;
        QualitySettings.vSyncCount = EnableVSync ? 1 : 0;

        switch (CurrentWindowMode)
        {
            case WindowMode.Windowed:
                Screen.SetResolution(Resolution.x, Resolution.y, FullScreenMode.Windowed);
                break;
            case WindowMode.Fullscreen:
                Screen.SetResolution(Resolution.x, Resolution.y, FullScreenMode.ExclusiveFullScreen);
                break;
            case WindowMode.Borderless:
                Screen.SetResolution(Resolution.x, Resolution.y, FullScreenMode.FullScreenWindow);
                break;
        }
    }

    public override void Reset()
    {
        TargetFramerate = defaultTargetFramerate;
        EnableVSync = defaultEnableVSync;
        CurrentWindowMode = defaultWindowMode;
        Resolution = defaultResolution;
    }

    public override object ToData()
    {
        return new GraphicsSettingsData
        {
            TargetFramerate = TargetFramerate,
            EnableVSync = EnableVSync,
            CurrentWindowMode = CurrentWindowMode,
            Resolution = Resolution
        };
    }

    public override void FromData(object data)
    {
        if (data is GraphicsSettingsData graphicsData)
        {
            TargetFramerate = graphicsData.TargetFramerate;
            EnableVSync = graphicsData.EnableVSync;
            CurrentWindowMode = graphicsData.CurrentWindowMode;
            Resolution = graphicsData.Resolution;
        }
    }
}