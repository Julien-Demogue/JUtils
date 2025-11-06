using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Outline))]
public class JOutlineSelectEffect : JSelectEffect
{
    private UnityEngine.UI.Outline outline;

    protected override void Awake()
    {
        base.Awake();
        outline = GetComponent<UnityEngine.UI.Outline>();
    }

    private void Start()
    {
        if (outline && EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            outline.enabled = false;
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (outline)
        {
            outline.enabled = true;
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (outline)
        {
            outline.enabled = false;
        }
    }
}
