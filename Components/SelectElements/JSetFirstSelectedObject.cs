using UnityEngine;
using UnityEngine.EventSystems;

public class JSetFirstSelectedObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToSelect;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(objectToSelect);
    }
}
