using UnityEngine;
using UnityEngine.UI;

public class SizeBar : MonoBehaviour
{
    [SerializeField] private RectTransform _content;

    public void RebuildUI()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
    }
}