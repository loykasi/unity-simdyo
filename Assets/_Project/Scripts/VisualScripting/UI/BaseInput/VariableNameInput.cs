using System.Collections.Generic;
using TMPro;

namespace Loykas.Scripting
{
    public class VariableNameInput : BaseInput
    {
        public TMP_Dropdown Dropdown;
        private string _value;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        public void Init(List<string> options)
        {
            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        private void OnValueChanged(int index)
        {
            string name = Dropdown.options[index].text;
            if (name.Equals("Select..."))
            {
                name = string.Empty;
            }
            OnSubmit?.Invoke(name);
        }

        public override object GetValue()
        {
            return _value;
        }

        public override void SetValue(object value)
        {
            _value = (string)value;
            string variableName = (string)value;
            if (Dropdown.options.Count > 0)
            {
                int index = Dropdown.options.FindIndex(o => o.text.Equals(variableName));
                if (index != -1)
                {
                    Dropdown.SetValueWithoutNotify(index);
                }
                else
                {
                    Dropdown.value = 0;
                }
            }
        }
    }
}