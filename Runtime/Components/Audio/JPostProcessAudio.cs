using UnityEditor;
using UnityEngine;

/// <summary>
/// JPostProcessAudio is a Unity AssetPostprocessor that applies audio settings to audio clips after they are imported.
/// </summary>
public class JPostProcessAudio : AssetPostprocessor
{
    /// <summary>
    /// Post-processes audio clips to apply audio settings.
    /// </summary>
    void OnPostprocessAudio(AudioClip audioClip)
    {
        AudioImporter audioImporter = assetImporter as AudioImporter;
        if (audioImporter != null)
        {
            EditorApplication.delayCall += () => JAudioEditor.ApplyAudioSettings(audioImporter);
        }
    }
}
