using UnityEngine;

public class JFloating : MonoBehaviour
{
    public float Height = 0.5f;
    public float Speed = 1f;
    public bool ShouldFloat = true;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (!ShouldFloat)
            return;

        float newY = startPosition.y + Mathf.Sin(Time.time * Speed) * Height;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.Rotate(Vector3.up, Time.deltaTime);
    }

    /// <summary>
    /// Stops the floating effect and resets the position to its original value.
    /// </summary>
    public void Stop()
    {
        transform.position = startPosition;
        ShouldFloat = false;
    }
}
