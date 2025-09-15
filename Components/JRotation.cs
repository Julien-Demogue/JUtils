using UnityEngine;

/// <summary>
/// JRotation provides a simple way to rotate a GameObject around a specified axis at a defined speed.
/// </summary>
public class JRotation : MonoBehaviour
{
    public float RotationSpeed = 10f;
    public Vector3 RotationAxis = Vector3.up;
    public bool ShouldRotate = true;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!ShouldRotate)
            return;

        transform.Rotate(RotationAxis, RotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Stops the rotation effect.
    /// </summary>
    public void Stop()
    {
        transform.rotation = initialRotation;
        ShouldRotate = false;
    }
}
