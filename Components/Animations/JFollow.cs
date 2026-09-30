using UnityEngine;

/// <summary>
/// JFollow is a simple component that allows a GameObject to follow a target Transform
/// </summary>
public class JFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = Vector3.zero;
    public float Speed = 5f;
    public bool ShouldFollow = true;

    // Use late update to ensure the camera follows the target after all other updates
    private void LateUpdate()
    {
        if (!ShouldFollow)
            return;

        if (Target != null)
        {
            Vector3 desiredPosition = Target.position + Offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Speed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target is not assigned in JFollow component.");
        }
    }
}
