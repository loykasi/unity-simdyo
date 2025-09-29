using System;
using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class ColorInput : BaseInput
    {
        public Button Button;
        public Image ButtonImage;
        private ColorHSV _value;

        private void Awake()
        {
            Button.onClick.AddListener(OnClick);
            SetValue(new ColorHSV(0f, 0f, 1f, 1f));
        }

        private void OnClick()
        {
            ColorPickerController.Instance.Open(_value, OnColorUpdated);
        }

        private void OnColorUpdated(ColorHSV color)
        {
            _value = color;

            if (ValueInstance != null)
            {
                ValueHandler.SetValue(ValueInstance, _value);
            }

            UpdateButton();
            OnSubmit?.Invoke(_value);
        }

        public override object GetValue()
        {
            return _value;
        }

        public override void SetValueInstance(Variable value)
        {
            base.SetValueInstance(value);
            _value = (ColorHSV)value.Value;
            UpdateButton();
        }

        public override void SetValue(object value)
        {
            _value = (ColorHSV)value;
            UpdateButton();
        }

        private void UpdateButton()
        {
            Color buttonColor = _value.ToUnityColor();
            ButtonImage.color = buttonColor;
        }
    }
}