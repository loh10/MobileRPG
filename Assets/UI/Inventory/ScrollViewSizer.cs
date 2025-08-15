using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSizer : MonoBehaviour
{
    public int itemHeight = 100;
    public int itemSpacing = 10;
    private RectTransform _rectTransform;

    private void OnEnable()
    {
        _rectTransform = GetComponent<ScrollRect>().content;
        RecalculateBounds();
    }

    private void RecalculateBounds()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, (itemHeight + itemSpacing)*_rectTransform.childCount);
    }
}
