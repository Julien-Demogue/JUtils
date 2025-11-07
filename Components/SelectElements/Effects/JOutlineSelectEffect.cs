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

        if (outline && EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            outline.enabled = false;
        }
    }

    public override void OnEffectSelected()
    {
        if (outline)
        {
            outline.enabled = true;
        }
    }

    public override void OnEffectDeselected()
    {
        if (outline)
        {
            outline.enabled = false;
        }
    }
}
