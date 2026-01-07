using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class NodeListCategoryItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransfrom;
        [SerializeField] private TMP_Text _titleField;
        [SerializeField] private RectTransform _content;
        [SerializeField] private NodeListItem _itemPrefab;

        private int _itemCount = 0;
        private List<NodeListItem> _menuItems = new();

        private readonly float _defaultHeight = 40f;

        public void Init(string title)
        {
            string localizedTitle = GlobalLocalization.Instance.GetValue("MainTable", title.ToLower());
            _titleField.SetText(localizedTitle);
        }

        public void AddItem(ScriptNode nodeData)
        {
            NodeListItem item = Instantiate(_itemPrefab, _content);
            item.NodeData = nodeData;

            float height = item.RectTransform.sizeDelta.y;
            item.RectTransform.anchoredPosition = _content.sizeDelta.y * Vector3.down;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, _content.sizeDelta.y + height);

            _rectTransfrom.sizeDelta = new Vector2(_rectTransfrom.sizeDelta.x, _defaultHeight + _content.sizeDelta.y);

            _itemCount++;
            _menuItems.Add(item);
        }
    }
}