using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class EntityInput : BaseInput
    {
        public TMP_Dropdown Dropdown;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int index)
        {
            var entity = ObjectManager.Instance.GetEntityByIndex(index);

            if (ValueInstance != null)
            {
                ValueHandler.SetValue(ValueInstance, entity);
            }

            OnSubmit?.Invoke(entity.Id);
        }

        public void Init(List<string> options)
        {
            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        public override void SetValue(object value)
        {
            int index = ObjectManager.Instance.GetIndexByEntityID((int)value);
            Dropdown.SetValueWithoutNotify(index);
        }

        public override object GetValue()
        {
            var entity = ObjectManager.Instance.GetEntityByIndex(Dropdown.value);
            return entity.Id;
        }

        public override void SetWidth(float width)
        {
            Rect.sizeDelta = new Vector2(width, Rect.sizeDelta.y);
        }
    }
}