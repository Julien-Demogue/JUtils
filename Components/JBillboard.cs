using UnityEngine;

public class JBillboard : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
