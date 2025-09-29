using System.Collections.Generic;
using TMPro;

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

            OnSubmit?.Invoke(entity);
        }

        public void Init(List<string> options)
        {
            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        public override void SetValue(object value)
        {

        }

        public override object GetValue()
        {
            var entity = ObjectManager.Instance.GetEntityByIndex(Dropdown.value);
            return entity;
        }
    }
}