using UnityEngine;
using UnityEngine.EventSystems;

public class JScaleSelectEffect : JSelectEffect
{
    private Vector3 defaultScale = Vector3.one;

    [Range(0f, 1f)]
    [SerializeField] private float scaleMultiplier = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        defaultScale = transform.localScale;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        transform.localScale = defaultScale * (1 + scaleMultiplier);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = defaultScale;
    }
}
