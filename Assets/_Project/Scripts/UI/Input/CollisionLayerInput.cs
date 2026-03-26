using System;
using System.Linq;
using TMPro;

namespace Loykas.Scripting
{
    public class CollisionLayerInput : BaseInput
    {
        public TMP_Dropdown Dropdown;
        private int _value;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            var options = Enum.GetNames(typeof(CollisionLayer)).ToList();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);

            Dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int value)
        {
            _value = value;
            OnSubmit?.Invoke((float)_value);
        }

        public override object GetValue()
        {
            return (float)_value;
        }

        public override void SetValue(object value)
        {
            _value = (int)(float)value;
            Dropdown.SetValueWithoutNotify(_value);
        }

        public override void SetDefaultValue(InputValue inputValue)
        {
            inputValue.Value = ValueTransfer.CreateNumber(_value);
        }
    }
}