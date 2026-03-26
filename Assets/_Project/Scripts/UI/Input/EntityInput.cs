using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class EntityInput : BaseInput
    {
        public TMP_Dropdown Dropdown;
        
        private SceneEntity _enttiy;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int index)
        {
            _enttiy = ObjectManager.Instance.GetEntityByIndex(index);

            if (ValueInstance != null)
            {
                ValueHandler.SetValue(ValueInstance, _enttiy);
            }

            OnSubmit?.Invoke(_enttiy ? _enttiy.Id : null);
        }

        public void Init(List<string> options)
        {
            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        public override void SetValue(object value)
        {
            int index = 0;
            if (value != null)
            {
                index = ObjectManager.Instance.GetIndexByEntityID((int)value);   
            }
            Dropdown.SetValueWithoutNotify(index);
        }

        public override object GetValue()
        {
            var entity = ObjectManager.Instance.GetEntityByIndex(Dropdown.value);
            Debug.Log(entity);
            return entity == null ? null : entity.Id;
        }

        public override void SetWidth(float width)
        {
            Rect.sizeDelta = new Vector2(width, Rect.sizeDelta.y);
        }

        public override void SetDefaultValue(InputValue inputValue)
        {
            inputValue.Value = ValueTransfer.CreateEntity(_enttiy);
        }
    }
}