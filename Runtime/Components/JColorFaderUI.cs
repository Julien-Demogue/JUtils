using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fades the UI element's color between a set of predefined colors.
/// </summary>
public class JColorFaderUI : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] Image image;

    int currentColorIndex = 0;
    float transitionTimer = 0f;

    private bool shouldFade = true;

    private void Start()
    {
        currentColorIndex = 0;
        transitionTimer = 0f;
    }

    private void Update()
    {
        if (!shouldFade || colors.Length == 0 || image == null)
            return;

        transitionTimer += Time.deltaTime;
        float t = transitionTimer / transitionDuration;

        if (t >= 1f)
        {
            transitionTimer = 0f;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            t = 0f;
        }

        Color startColor = colors[currentColorIndex];
        Color endColor = colors[(currentColorIndex + 1) % colors.Length];
        image.color = Color.Lerp(startColor, endColor, t);
    }

    /// <summary>
    /// Pauses the color fading.
    /// </summary>
    public void Pause()
    {
        shouldFade = false;
    }

    /// <summary>
    /// Resumes the color fading.
    /// </summary>
    public void Resume()
    {
        shouldFade = true;
    }
}
