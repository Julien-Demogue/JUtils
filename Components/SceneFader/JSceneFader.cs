using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton component to manage scene fading using a specified fade effect.
/// </summary>
public class JSceneFader : MonoBehaviour
{
    [SerializeField] private bool isPersistent = true;

    [Space]
    [SerializeField] private JFadeEffect fadeEffect;

    public static JSceneFader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the fade effect to be used for scene transitions.
    /// </summary>
    /// <param name="newFadeEffect"></param>
    public void SetFadeEffect(JFadeEffect newFadeEffect)
    {
        fadeEffect = newFadeEffect;
    }

    /// <summary>
    /// Starts the fade-in effect.
    /// </summary>
    /// <param name="onFadeInComplete"></param>
    public void FadeIn(Action onFadeInComplete = null)
    {
        fadeEffect.FadeIn(onFadeInComplete);
    }

    /// <summary>
    /// Starts the fade-out effect.
    /// </summary>
    /// <param name="onFadeOutComplete"></param>
    public void FadeOut(Action onFadeOutComplete = null)
    {
        fadeEffect.FadeOut(onFadeOutComplete);
    }

    /// <summary>
    /// Fades to a new scene with fade-in and fade-out effects.
    /// </summary>
    /// <param name="sceneName"></param>
    public void FadeToScene(string sceneName)
    {
        fadeEffect.FadeIn(() =>
        {
            SceneManager.LoadScene(sceneName);
            fadeEffect.FadeOut();
        });
    }

    /// <summary>
    /// Stops any ongoing fade effect.
    /// </summary>
    /// <param name="onFadeStop"></param>
    public void StopFade(Action onFadeStop = null)
    {
        fadeEffect.StopFade(onFadeStop);
    }
}