using UnityEngine;

public class JCameraFollow : MonoBehaviour
{
    public bool ShouldFollow = true;

    public Transform Target;

    [Range(0.001f, 1f)]
    public float SmoothSpeed = 0.05f;
    public Vector3 Offset;

    private void LateUpdate()
    {
        if (Target != null && ShouldFollow)
        {
            Vector3 desiredPosition = Target.position + Offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}