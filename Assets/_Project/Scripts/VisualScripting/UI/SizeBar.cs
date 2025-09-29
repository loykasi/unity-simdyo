using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class SizeBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;

        public void RebuildUI()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }
    }
}