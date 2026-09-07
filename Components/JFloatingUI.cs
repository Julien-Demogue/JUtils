using UnityEngine;

/// <summary>
/// A component that makes a UI element float up and down.
/// </summary>
public class FloatingUI : MonoBehaviour
{
    [SerializeField] private float amplitude = 5f;
    [SerializeField] private float speed = 5f;

    private bool shouldFloat = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!shouldFloat)
            return;

        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;

        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public void Stop()
    {
        shouldFloat = false;
        transform.position = startPos;
    }
}
