using UnityEngine;

// <summary>
// Component that allows a GameObject to pulse in size over time.
// </summary>
public class JPulse : MonoBehaviour
{
    public float PulseSpeed = 1f;
    public float PulseMagnitude = 0.1f;
    public bool ShouldPulse = true;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!ShouldPulse)
            return;

        float scaleModifier = Mathf.Sin(Time.time * PulseSpeed) * PulseMagnitude;
        transform.localScale = originalScale + new Vector3(scaleModifier, scaleModifier, scaleModifier);
    }

    /// <summary>
    /// Stops the pulsing effect and resets the scale to its original value.
    /// </summary>
    public void Stop()
    {
        ShouldPulse = false;
        transform.localScale = originalScale;
    }
}
