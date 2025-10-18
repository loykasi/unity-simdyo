using System.Collections.Generic;
using TMPro;

namespace Loykas.Scripting
{
    public class GlobalVariableNameInput : BaseInput
    {
        public TMP_Dropdown Dropdown;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
            Init();
        }

        public void Init()
        {
            List<string> options = SceneManager.Instance.GlobalScript.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        private void OnValueChanged(int index)
        {
            string name = Dropdown.options[index].text;
            OnSubmit?.Invoke(name);
        }

        public override object GetValue()
        {
            if (Dropdown.options.Count == 0)
            {
                return string.Empty;
            }
            return Dropdown.options[Dropdown.value].text;
        }

        public override void SetValue(object value)
        {
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