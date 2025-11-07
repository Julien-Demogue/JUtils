using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JSelectEffect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private bool wasSelected = false;

    public abstract void OnEffectSelected();
    public abstract void OnEffectDeselected();

    public void OnSelect(BaseEventData eventData)
    {
        OnEffectSelected();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnEffectDeselected();
    }

    protected virtual void Awake() { }

    protected virtual void LateUpdate()
    {
        bool isCurrentlySelected = IsSelected();

        if (isCurrentlySelected && !wasSelected)
        {
            OnEffectSelected();
        }
        else if (!isCurrentlySelected && wasSelected)
        {
            OnEffectDeselected();
        }

        wasSelected = isCurrentlySelected;
    }

    private bool IsSelected()
    {
        return EventSystem.current != null &&
               EventSystem.current.currentSelectedGameObject == gameObject;
    }
}
