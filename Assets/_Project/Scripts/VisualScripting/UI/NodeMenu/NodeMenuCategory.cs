using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class NodeMenuCategory : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransfrom;
        [SerializeField] private TMP_Text _titleField;
        [SerializeField] private RectTransform _content;
        [SerializeField] private NodeMenuItem _itemPrefab;

        private RectTransform _parentRect;
        private NodeMenu _nodeMenu;

        private int _itemCount = 0;
        private bool _isContentActive = false;

        public void Init(string title, RectTransform parentRect, NodeMenu nodeMenu)
        {
            _titleField.SetText(title);
            _parentRect = parentRect;
            _nodeMenu = nodeMenu;
        }

        public void ToggleContent()
        {
            SetOpen(!_isContentActive);
        }

        public void SetOpen(bool value)
        {
            _isContentActive = value;
            _content.gameObject.SetActive(value);

            if (value)
            {
                _rectTransfrom.sizeDelta = new Vector2(_rectTransfrom.sizeDelta.x, 40 + _content.sizeDelta.y);
            }
            else
            {
                _rectTransfrom.sizeDelta = new Vector2(_rectTransfrom.sizeDelta.x, 40);
            }
            LayoutRebuilder.MarkLayoutForRebuild(_parentRect);
        }

        public void AddItem(ScriptNodeData nodeData)
        {
            NodeMenuItem item = Instantiate(_itemPrefab, _content);
            item.NodeData = nodeData;
            item.NodeMenu = _nodeMenu;

            float height = item.RectTransform.sizeDelta.y;
            item.RectTransform.position = _content.position + _content.sizeDelta.y * _itemCount * Vector3.down;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, _content.sizeDelta.y + height);

            _itemCount++;
        }
    }
}