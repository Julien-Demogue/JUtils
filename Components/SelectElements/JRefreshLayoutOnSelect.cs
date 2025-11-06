using UnityEngine;
using UnityEngine.UI;

public class JRefreshLayoutOnSelect : MonoBehaviour
{
    [SerializeField] private RectTransform layoutGroupRect;

    public void OnSelect()
    {
        RefreshLayout();
    }

    public void RefreshLayout()
    {
        if (layoutGroupRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupRect);
        }
    }
}
