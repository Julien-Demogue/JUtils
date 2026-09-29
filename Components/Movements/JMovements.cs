using UnityEngine;

[RequireComponent(typeof(JPlayerInput))]
public abstract class JMovements : MonoBehaviour
{
    public float Speed = 10f;
    protected JPlayerInput playerInput;

    protected virtual void Awake()
    {
        playerInput = GetComponent<JPlayerInput>();
    }

    protected virtual void Update()
    {
        Vector2 dir = playerInput.Direction;
        Vector3 movement = new Vector3(dir.x, 0f, dir.y) * Speed * Time.deltaTime;
        Move(movement);
    }

    protected abstract void Move(Vector3 movement);
}