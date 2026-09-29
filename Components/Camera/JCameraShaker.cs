using System.Collections;
using UnityEngine;

/// <summary>
/// Shakes the camera to create a dynamic effect.
/// </summary>
[RequireComponent(typeof(Camera))]
public class JCameraShaker : MonoBehaviour
{
    private enum ShakeState
    {
        Idle,
        Shaking
    }

    private Camera cam;

    private float duration = 0f;
    private float magnitude = 0.1f;
    private ShakeState currentState = ShakeState.Idle;

    private Vector3 originalPosition;

    public static JCameraShaker Instance { get; private set; }

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originalPosition = cam.transform.localPosition;
    }

    private void Update()
    {
        if (currentState == ShakeState.Shaking)
        {
            if (duration > 0)
            {
                Vector3 randomPoint = originalPosition + Random.insideUnitSphere * magnitude;
                cam.transform.localPosition = randomPoint;
                duration -= Time.deltaTime;
            }
            else
            {
                Stop();
            }
        }
    }

    /// <summary>
    /// Starts the camera shake with specified duration and magnitude.
    /// </summary>
    /// <param name="duration">The duration of the shake.</param>
    /// <param name="magnitude">The magnitude of the shake.</param>
    public void Shake(float duration, float magnitude)
    {
        this.duration = duration;
        this.magnitude = magnitude;
        currentState = ShakeState.Shaking;
    }

    /// <summary>
    /// Stops the camera shake.
    /// </summary>
    public void Stop()
    {
        currentState = ShakeState.Idle;
        cam.transform.localPosition = originalPosition;
    }
}
