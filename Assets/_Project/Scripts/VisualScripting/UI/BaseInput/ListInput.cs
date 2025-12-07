using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class ListInput : BaseInput
    {
        public DataType ListType { get; set; }

        [SerializeField] private UIInputData _inputData;
        [SerializeField] private RectTransform _addButtonRect;
        [SerializeField] private Button _addButton;

        [SerializeField] private RectTransform _container;
        [SerializeField] private ListInputItem _listInputItemPrefab;
        private List<ListInputItem> _inputItems = new();

        private void Awake()
        {
            _addButton.onClick.AddListener(OnAddElement);
        }

        public override void SetValueInstance(Variable value)
        {
            base.SetValueInstance(value);

            IList list = (IList)value.Value;
            for (int i = 0; i < list.Count; i++)
            {
                AddElement(list[i]);
            }
        }

        private void OnAddElement()
        {
            ListInputItem listInputItem = AddElement();
            if (ValueInstance != null)
            {
                ValueHandler.ListAdd(ValueInstance, listInputItem.Get());
            }
            OnValueUpdated?.Invoke();
        }

        private ListInputItem AddElement(object value = null)
        {
            ListInputItem listInputItem = Instantiate(_listInputItemPrefab, _container);

            BaseInput input = _inputData.GetInputInstance(ListType);

            listInputItem.Init(this, input);
            listInputItem.Rect.localPosition = new Vector3(0f, -listInputItem.Rect.sizeDelta.y * _inputItems.Count, 0f);

            if (value != null)
            {
                listInputItem.Set(value);
            }

            _inputItems.Add(listInputItem);

            Size = new Vector2
            (
                Size.x,
                Size.y + 30f
            );

            return listInputItem;
        }

        public void Remove(ListInputItem item)
        {
            Remove(IndexOfElement(item));
            OnValueUpdated?.Invoke();
        }

        private void Remove(int index)
        {
            Destroy(_inputItems[index].gameObject);

            Size = new Vector2
            (
                Size.x,
                Size.y - 30f
            );

            _inputItems.RemoveAt(index);

            for (int i = index; i < _inputItems.Count; i++)
            {
                ListInputItem item = _inputItems[i];
                item.Rect.anchoredPosition = new Vector2
                (
                    0f,
                    item.Rect.anchoredPosition.y + 30f
                );
            }

            if (ValueInstance != null)
            {
                ValueHandler.ListRemoveAt(ValueInstance, index);
            }
        }

        public void Edit(ListInputItem item, object value)
        {
            if (ValueInstance != null)
            {
                ValueHandler.ListEdit(ValueInstance, IndexOfElement(item), value);
            }
        }

        private int IndexOfElement(ListInputItem item)
        {
            return _inputItems.IndexOf(item);
        }

        public override object GetValue()
        {
            if (ValueInstance != null)
            {
                return ValueInstance.Value;
            }

            return null;
        }
    }
}