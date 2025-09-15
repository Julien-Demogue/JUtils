using UnityEngine;

public abstract class JFadeEffect : MonoBehaviour
{
    private enum FadeState
    {
        NONE,
        FADING_IN,
        FADING_OUT
    }

    private FadeState currentFadeState = FadeState.NONE;

    public abstract void FadeIn(float duration);
    public abstract void FadeOut(float duration);
}