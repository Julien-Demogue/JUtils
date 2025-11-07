using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JTextColorSelectEffect : JSelectEffect
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Color selectedTextColor = Color.white;
    private Color defaultTextColor;

    protected override void Awake()
    {
        base.Awake();
        if (buttonText)
        {
            defaultTextColor = buttonText.color;
        }
    }

    public override void OnEffectSelected()
    {
        if (buttonText)
        {
            buttonText.color = selectedTextColor;
        }
    }

    public override void OnEffectDeselected()
    {
        if (buttonText)
        {
            buttonText.color = defaultTextColor;
        }
    }
}
