using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JTextUnderlineSelectEffect : JSelectEffect
{
    [SerializeField] private TextMeshProUGUI buttonText;

    public override void OnSelect(BaseEventData eventData)
    {
        if (buttonText && !buttonText.text.Contains("<u>"))
        {
            buttonText.text = "<u>" + buttonText.text + "</u>";
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (buttonText)
        {
            buttonText.text = buttonText.text.Replace("<u>", "").Replace("</u>", "");
        }
    }
}
