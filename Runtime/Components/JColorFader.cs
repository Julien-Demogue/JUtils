using UnityEngine;

/// <summary>
/// Fades the GameObject's material color between a set of predefined colors.
/// </summary>
public class JColorFader : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] Renderer objectRenderer;

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
        if (!shouldFade || colors.Length == 0 || objectRenderer == null)
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
        objectRenderer.material.color = Color.Lerp(startColor, endColor, t);
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
