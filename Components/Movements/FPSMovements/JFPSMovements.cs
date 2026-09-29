using UnityEngine;

[RequireComponent(typeof(JPlayerInput))]
public class JFPSMovements : JMovements
{
    protected override void Move(Vector3 movement)
    {
        transform.Translate(movement, Space.Self);
    }
}