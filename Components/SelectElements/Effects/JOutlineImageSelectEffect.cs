using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class JOutlineImageSelectEffect : JSelectEffect
{
    private Image outlineImage;

    protected override void Awake()
    {
        base.Awake();
        outlineImage = GetComponent<Image>();

        if (outlineImage && EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            outlineImage.enabled = false;
        }
    }

    public override void OnEffectSelected()
    {
        if (outlineImage)
        {
            outlineImage.enabled = true;
        }
    }

    public override void OnEffectDeselected()
    {
        if (outlineImage)
        {
            outlineImage.enabled = false;
        }
    }
}
