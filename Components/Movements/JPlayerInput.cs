using UnityEngine;
using UnityEngine.InputSystem;

public class JPlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference runAction;
    [SerializeField] private InputActionReference crouchAction;

    void OnEnable()
    {
        EnableAction(moveAction);
        EnableAction(lookAction);
        EnableAction(jumpAction);
        EnableAction(runAction);
        EnableAction(crouchAction);
    }

    private void EnableAction(InputActionReference actionReference)
    {
        if (actionReference != null && actionReference.action != null)
        {
            actionReference.action.Enable();
        }
    }

    public Vector2 Direction => moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 Look => lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;
    public bool LookFromPointer => lookAction != null
        && lookAction.action.activeControl != null
        && lookAction.action.activeControl.device is Pointer;
    public bool Jump => jumpAction != null && jumpAction.action.triggered;
    public bool Run => runAction != null && runAction.action.IsPressed();
    public bool Crouch => crouchAction != null && crouchAction.action.IsPressed();
}