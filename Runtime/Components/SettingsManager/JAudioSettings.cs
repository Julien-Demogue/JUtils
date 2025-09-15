using System;
using UnityEngine;

public class JAudioSettings : JSettings
{
    [Serializable]
    public class AudioSettingsData
    {
        public float GeneralVolume;
        public float MusicVolume;
        public float SFXVolume;
    }

    [Range(0, 100)]
    public float GeneralVolume = 100f;
    private float defaultGeneralVolume;

    [Range(0, 100)]
    public float MusicVolume = 50f;
    private float defaultMusicVolume;

    [Range(0, 100)]
    public float SFXVolume = 50f;
    private float defaultSFXVolume;

    public override void SaveDefaultSettingsValues()
    {
        defaultGeneralVolume = GeneralVolume;
        defaultMusicVolume = MusicVolume;
        defaultSFXVolume = SFXVolume;
    }

    public override void Apply()
    {
        float generalVolumeLevel = GeneralVolume <= 0 ? -80f : GeneralVolume / 100f;
        float musicVolumeLevel = MusicVolume <= 0 ? -80f : MusicVolume / 100f;
        float soundEffectsVolumeLevel = SFXVolume <= 0 ? -80f : SFXVolume / 100f;

        JAudioManager.Instance.SetGeneralVolume(generalVolumeLevel);
        JAudioManager.Instance.SetMusicVolume(musicVolumeLevel);
        JAudioManager.Instance.SetSoundEffectsVolume(soundEffectsVolumeLevel);
    }

    public override void Reset()
    {
        GeneralVolume = defaultGeneralVolume;
        MusicVolume = defaultMusicVolume;
        SFXVolume = defaultSFXVolume;
    }

    public override object ToData()
    {
        return new AudioSettingsData
        {
            GeneralVolume = GeneralVolume,
            MusicVolume = MusicVolume,
            SFXVolume = SFXVolume
        };
    }

    public override void FromData(object data)
    {
        if (data is AudioSettingsData audioData)
        {
            GeneralVolume = audioData.GeneralVolume;
            MusicVolume = audioData.MusicVolume;
            SFXVolume = audioData.SFXVolume;
        }
    }

}