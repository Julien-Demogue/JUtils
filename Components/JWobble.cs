using UnityEngine;

/// <summary>
/// A component that makes an object wobble.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class Wobble : MonoBehaviour
{
    [SerializeField]
    private float recovery = 2f;
    [SerializeField]
    private float wobbleSpeed = 2f;
    [SerializeField]
    private float maxWobble = 0.5f;

    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;

    private float wobbleAmountX;
    private float wobbleAmountZ;

    private float pulse;
    private Vector3 velocity;
    private Vector3 lastPos;
    private Vector3 angularVelocity;
    private Vector3 lastRot;

    private float time;
    private Renderer render;

    void Start()
    {
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        time += Time.deltaTime;

        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * recovery);
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * recovery);

        pulse = 2 * Mathf.PI * wobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        render.material.SetFloat("_WobbleX", wobbleAmountX);
        render.material.SetFloat("_WobbleZ", wobbleAmountZ);

        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;

        wobbleAmountToAddX += Mathf.Clamp((velocity.x + angularVelocity.z * 0.2f) * maxWobble, -maxWobble, maxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + angularVelocity.x * 0.2f) * maxWobble, -maxWobble, maxWobble);

        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }
}
