using UnityEngine;
using UnityEngine.InputSystem;

public class JPlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    public Vector2 Direction => moveAction.action.ReadValue<Vector2>();
}