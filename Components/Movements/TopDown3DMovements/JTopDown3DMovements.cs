using UnityEngine;

public class JTopDown3DMovements : JMovements
{
    protected override void Move(Vector3 movement)
    {
        transform.Translate(movement, Space.World);
    }
}