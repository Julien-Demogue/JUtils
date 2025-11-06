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
    }

    private void Start()
    {
        if (outlineImage && EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            outlineImage.enabled = false;
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (outlineImage)
        {
            outlineImage.enabled = true;
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (outlineImage)
        {
            outlineImage.enabled = false;
        }
    }
}
