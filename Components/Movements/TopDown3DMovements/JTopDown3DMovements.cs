using UnityEngine;

public class JTopDown3DMovements : JMovements
{
    protected override Vector3 ToWorld(Vector3 movement) => movement;
}