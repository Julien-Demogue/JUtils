using UnityEngine;

/// <summary>
/// JFollow is a simple component that allows a GameObject to follow a target Transform
/// </summary>
public class JFollowSide : MonoBehaviour
{
    public enum Side
    {
        FRONT,
        BACK,
        LEFT,
        RIGHT,
        ABOVE,
        BELOW
    }

    public Transform Target;
    public Vector3 Offset = Vector3.zero;
    public float Speed = 5f;
    public bool ShouldFollow = true;
    public Side FollowSide = Side.BACK;

    // Use late update to ensure the camera follows the target after all other updates
    private void LateUpdate()
    {
        if (!ShouldFollow)
            return;

        if (Target != null)
        {
            Vector3 sideOffset = GetSideOffset();
            Vector3 desiredPosition = Target.position + sideOffset + Offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Speed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target is not assigned in JFollow component.");
        }
    }

    /// <summary>
    /// Gets the offset direction based on the specified side.
    /// </summary>
    /// <returns>A Vector3 representing the offset direction based on the side.</returns>
    private Vector3 GetSideOffset()
    {
        switch (FollowSide)
        {
            case Side.FRONT:
                return Target.forward;
            case Side.BACK:
                return -Target.forward;
            case Side.LEFT:
                return -Target.right;
            case Side.RIGHT:
                return Target.right;
            case Side.ABOVE:
                return Target.up;
            case Side.BELOW:
                return -Target.up;
            default:
                return Vector3.zero;
        }
    }
}
