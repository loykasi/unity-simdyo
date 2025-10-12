using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class StringInput : BaseInput
    {
        public TMP_InputField InputField;
        public bool AutoRezise = true;
        public float MinWidth = 50f;
        public float MaxWidth = 200f;

        private readonly float _horizontalPadding = 20f;

        private void Awake()
        {
            InputField.onValueChanged.AddListener(OnValueChanged);
            InputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            if (ValueInstance != null)
            {
                ValueHandler.SetValue(ValueInstance, value);
            }

            OnSubmit?.Invoke(value);
        }

        private void OnValueChanged(string value)
        {
            if (AutoRezise)
            {
                Vector2 size = InputField.textComponent.GetPreferredValues(value);
                float x = Mathf.Clamp(size.x + _horizontalPadding, MinWidth, MaxWidth);
                Rect.sizeDelta = new Vector2
                (
                    x,
                    Rect.sizeDelta.y
                );   
            }
            OnValueUpdated?.Invoke();
        }

        public override object GetValue()
        {
            return InputField.text;
        }

        public override void SetValueInstance(Variable value)
        {
            base.SetValueInstance(value);
            InputField.SetTextWithoutNotify((string)value.Value);
        }

        public override void SetValue(object value)
        {
            InputField.SetTextWithoutNotify((string)value);
        }
    }
}