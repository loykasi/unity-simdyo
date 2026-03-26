using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class NumberInput : BaseInput
    {
        public TMP_InputField InputField;
        public bool AutoRezise = true;
        public float MinWidth = 50f;
        public float MaxWidth = 200f;

        private float _value;

        private readonly float _horizontalPadding = 20f;

        private void Awake()
        {
            InputField.onValueChanged.AddListener(OnValueChanged);
            InputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnValueChanged(string value)
        {
            UpdateSize();
            OnValueUpdated?.Invoke();
        }

        private void OnEndEdit(string value)
        {
            if (float.TryParse(value, out float parsedValue))
            {
                _value = parsedValue;
            }

            if (ValueInstance != null)
            {
                ValueHandler.SetValue(ValueInstance, _value);
            }

            OnSubmit?.Invoke(_value);
        }

        public override object GetValue()
        {
            return _value;
        }

        public override void SetValueInstance(Variable value)
        {
            base.SetValueInstance(value);
            InputField.SetTextWithoutNotify(value.Value.ToString());
        }

        public override void SetValue(object value)
        {
            _value = (float)value;
            InputField.SetTextWithoutNotify(_value.ToString());
            UpdateSize();
        }

        public override void SetWidth(float width)
        {
            MinWidth = width;
            MaxWidth = width;
            Rect.sizeDelta = new Vector2(width, Rect.sizeDelta.y);
        }

        private void UpdateSize()
        {
            if (AutoRezise)
            {
                Vector2 size = InputField.textComponent.GetPreferredValues();
                float x = Mathf.Clamp(size.x + _horizontalPadding, MinWidth, MaxWidth);
                Rect.sizeDelta = new Vector2(x, Rect.sizeDelta.y);
            }
        }

        public override void SetDefaultValue(InputValue inputValue)
        {
            inputValue.Value = ValueTransfer.CreateNumber(_value);
        }
    }
}