using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class GlobalVariableNameInput : BaseInput
    {
        public TMP_Dropdown Dropdown;
        private int _selectedIndex;
        private Variable _variable;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
            Init();
        }

        private void OnDisable()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.GlobalScript.OnVariableAdded -= OnVariableAdded;
                SceneManager.Instance.GlobalScript.OnVariableUpdated -= OnVariableUpdated;
                SceneManager.Instance.GlobalScript.OnVariableDeleted -= OnVariableDeleted;
            }
        }

        public void Init()
        {
            SceneManager.Instance.GlobalScript.OnVariableAdded += OnVariableAdded;
            SceneManager.Instance.GlobalScript.OnVariableUpdated += OnVariableUpdated;
            SceneManager.Instance.GlobalScript.OnVariableDeleted += OnVariableDeleted;

            List<string> options = SceneManager.Instance.GlobalScript.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        private void OnVariableAdded(Variable variable)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(variable.Name));
        }

        private void OnValueChanged(int index)
        {
            string name = Dropdown.options[index].text;
            if (name.Equals("Select..."))
            {
                name = string.Empty;
            }
            OnSubmit?.Invoke(name);

            if (name == string.Empty)
            {
                return;
            }
            _variable = SceneManager.Instance.GlobalScript.GetVariable(name);
        }

        private void OnVariableUpdated(Variable variable)
        {
            List<string> options = SceneManager.Instance.GlobalScript.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
            Dropdown.SetValueWithoutNotify(_selectedIndex);
        }

        private void OnVariableDeleted(Variable variable)
        {
            List<string> options = SceneManager.Instance.GlobalScript.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        public override object GetValue()
        {
            string name = Dropdown.options[_selectedIndex].text;
            if (name.Equals("Select..."))
            {
                name = string.Empty;
            }
            return name;
        }

        public override void SetValue(object value)
        {
            string variableName = (string)value;
            if (Dropdown.options.Count > 0)
            {
                _selectedIndex = Dropdown.options.FindIndex(o => o.text.Equals(variableName));
                if (_selectedIndex != -1)
                {
                    _variable = SceneManager.Instance.GlobalScript.GetVariable(name);
                }
                else
                {
                    _selectedIndex = 0;
                }

                Dropdown.SetValueWithoutNotify(_selectedIndex);
            }
        }
    }
}