using UnityEngine;

[RequireComponent(typeof(JPlayerInput), typeof(CharacterController))]
public class JFPSMovements : JMovements
{
    public float MouseSensitivity = 10f;
    public float GamepadSensitivity = 10f;
    [Range(0f, 90f)] public float MaxLookAngle = 80f;
    [SerializeField] private Transform objectToRotate;
    private float verticalAngle;

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        base.Update();
        Look(playerInput.Look);
    }

    protected override Vector3 ToWorld(Vector3 movement) => transform.TransformDirection(movement);

    private void Look(Vector2 look)
    {
        Vector2 rotation = playerInput.LookFromPointer
        ? look * MouseSensitivity
        : look * GamepadSensitivity * Time.deltaTime;

        // Rotate body on horizontal axis
        transform.Rotate(Vector3.up, rotation.x);

        // Rotate camera on vertical axis
        verticalAngle = Mathf.Clamp(verticalAngle - rotation.y, -MaxLookAngle, MaxLookAngle);
        objectToRotate.localRotation = Quaternion.Euler(verticalAngle, 0f, 0f);
    }
}