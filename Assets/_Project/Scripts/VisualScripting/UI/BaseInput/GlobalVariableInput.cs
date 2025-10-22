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
            }

            if (_variable != null)
            {
                _variable.OnUpdated -= OnVariableUpdated;
            }
        }

        public void Init()
        {
            SceneManager.Instance.GlobalScript.OnVariableAdded += OnVariableAdded;
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
            if (_variable != null)
            {
                _variable.OnUpdated -= OnVariableUpdated;
            }
            _variable = SceneManager.Instance.GlobalScript.GetVariable(name);
            _variable.OnUpdated += OnVariableUpdated;
        }

        private void OnVariableUpdated()
        {
            Dropdown.options[_selectedIndex].text = _variable.Name;
            Dropdown.captionText.text = _variable.Name;
        }

        public override object GetValue()
        {
            return _variable.Name;
        }

        public override void SetValue(object value)
        {
            string variableName = (string)value;
            if (Dropdown.options.Count > 0)
            {
                _selectedIndex = Dropdown.options.FindIndex(o => o.text.Equals(variableName));
                if (_selectedIndex != -1)
                {
                    if (_variable != null)
                    {
                        _variable.OnUpdated -= OnVariableUpdated;
                    }
                    _variable = SceneManager.Instance.GlobalScript.GetVariable(variableName);
                    _variable.OnUpdated += OnVariableUpdated;
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