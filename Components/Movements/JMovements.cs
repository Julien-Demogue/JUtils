using UnityEngine;

[RequireComponent(typeof(JPlayerInput), typeof(CharacterController))]
public abstract class JMovements : MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 5f;
    public float Gravity = -9.81f;
    public float RunSpeedMultiplier = 1.5f;
    [Range(0f, 1f)] public float CrouchSpeedReduction = 0.5f;

    protected JPlayerInput playerInput;
    protected CharacterController characterController;
    private float verticalSpeed;

    protected virtual void Awake()
    {
        playerInput = GetComponent<JPlayerInput>();
        characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        Vector3 movement = ToWorld(GetHorizontalMovement());
        movement.y = GetVerticalSpeed() * Time.deltaTime;
        characterController.Move(movement);
    }

    private Vector3 GetHorizontalMovement()
    {
        Vector2 direction = playerInput.Direction;
        Vector3 movement = new Vector3(direction.x, 0f, direction.y) * Speed * Time.deltaTime;

        if (playerInput.Crouch)
        {
            movement *= CrouchSpeedReduction;
        }
        else if (playerInput.Run)
        {
            movement *= RunSpeedMultiplier;
        }

        return movement;
    }

    private float GetVerticalSpeed()
    {
        if (characterController.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -2f;

        if (characterController.isGrounded && playerInput.Jump)
            verticalSpeed = JumpForce;

        verticalSpeed += Gravity * Time.deltaTime;
        return verticalSpeed;
    }
    
    protected abstract Vector3 ToWorld(Vector3 movements);
}