using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JSelectEffect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public abstract void OnSelect(BaseEventData eventData);
    public abstract void OnDeselect(BaseEventData eventData);

    protected virtual void Awake() { }
}
