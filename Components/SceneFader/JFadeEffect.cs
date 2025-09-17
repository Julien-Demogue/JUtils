using System;
using UnityEngine;

/// <summary>
/// Abstract class for fade effects. Inherit from this class to implement specific fade effects.
/// </summary>
public abstract class JFadeEffect : MonoBehaviour
{
    protected enum FadeState
    {
        NONE,
        FADING_IN,
        FADING_OUT
    }

    [SerializeField] protected float fadeInDuration = 1f;
    [SerializeField] protected float fadeOutDuration = 1f;
    protected float fadeTimer = 0f;
    protected FadeState currentFadeState = FadeState.NONE;

    private Action onFadeInComplete;
    private Action onFadeOutComplete;

    private void Update()
    {
        if (currentFadeState == FadeState.NONE) return;

        fadeTimer += Time.deltaTime;
        FadeAnimation();
        switch (currentFadeState)
        {
            case FadeState.FADING_IN:
                if (fadeTimer >= fadeInDuration)
                {
                    currentFadeState = FadeState.NONE;
                    onFadeInComplete?.Invoke();
                }
                break;
            case FadeState.FADING_OUT:
                if (fadeTimer >= fadeOutDuration)
                {
                    currentFadeState = FadeState.NONE;
                    onFadeOutComplete?.Invoke();
                }
                break;
        }
    }

    /// <summary>
    /// Starts the fade-in effect.
    /// </summary>
    /// <param name="onFadeInComplete"></param>
    public void FadeIn(Action onFadeInComplete = null)
    {
        if (currentFadeState == FadeState.FADING_IN) return;

        currentFadeState = FadeState.FADING_IN;
        fadeTimer = 0f;
        this.onFadeInComplete = onFadeInComplete;
        OnFadeIn();
    }

    /// <summary>
    /// Starts the fade-out effect.
    /// </summary>
    /// <param name="onFadeOutComplete"></param>
    public void FadeOut(Action onFadeOutComplete = null)
    {
        if (currentFadeState == FadeState.FADING_OUT) return;

        currentFadeState = FadeState.FADING_OUT;
        fadeTimer = 0f;
        this.onFadeOutComplete = onFadeOutComplete;
        OnFadeOut();
    }

    /// <summary>
    /// Stops any ongoing fade effect.
    /// </summary>
    /// <param name="onFadeStop"></param>
    public void StopFade(Action onFadeStop = null)
    {
        currentFadeState = FadeState.NONE;
        fadeTimer = 0f;
        onFadeInComplete = null;
        onFadeOutComplete = null;
        onFadeStop?.Invoke();
    }

    /// <summary>
    /// Method to be called to perform the fade animation.
    /// </summary>
    abstract protected void FadeAnimation();

    /// <summary>
    /// Called when the fade-in effect starts.
    /// </summary>
    abstract protected void OnFadeIn();

    /// <summary>
    /// Called when the fade-out effect starts.
    /// </summary>
    abstract protected void OnFadeOut();
}