using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class NodeListCategoryItem : MonoBehaviour
    {
        [SerializeField] private UINodeColorData _colorData;
        [SerializeField] private RectTransform _rectTransfrom;
        [SerializeField] private TMP_Text _titleField;
        [SerializeField] private RectTransform _content;
        [SerializeField] private NodeListItem _itemPrefab;

        private List<NodeListItem> _menuItems = new();
        private ScriptNodeCategory _category;

        private readonly float _defaultHeight = 40f;

        public void Init(ScriptNodeCategory category)
        {
            _category = category;
            string localizedTitle = GlobalLocalization.Instance.GetValue("MainTable", category.ToString().ToLower());
            _titleField.SetText(localizedTitle);
        }

        public void AddItem(ScriptNode nodeData)
        {
            NodeListItem item = Instantiate(_itemPrefab, _content);
            item.NodeData = nodeData;
            item.SetColor(_colorData.Get(_category));

            float height = item.RectTransform.sizeDelta.y;
            item.RectTransform.anchoredPosition = _content.sizeDelta.y * Vector3.down;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, _content.sizeDelta.y + height);

            _rectTransfrom.sizeDelta = new Vector2(_rectTransfrom.sizeDelta.x, _defaultHeight + _content.sizeDelta.y);
            _menuItems.Add(item);
        }
    }
}