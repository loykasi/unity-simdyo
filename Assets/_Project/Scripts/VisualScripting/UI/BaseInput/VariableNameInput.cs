using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class VariableNameInput : BaseInput
    {
        public TMP_Dropdown Dropdown;
        private int _selectedIndex;
        private string _variableName;
        private ScriptFlow _flow;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            if (_flow != null)
            {
                _flow.OnVariableAdded -= OnVariableAdded;   
                _flow.OnVariableUpdated -= OnVariableUpdated;
                _flow.OnVariableDeleted -= OnVariableDeleted;
            }
        }

        public void Init(ScriptFlow flow)
        {
            _flow = flow;
            _flow.OnVariableAdded += OnVariableAdded;
            _flow.OnVariableUpdated += OnVariableUpdated;
            _flow.OnVariableDeleted += OnVariableDeleted;

            List<string> options = _flow.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
        }

        private void OnVariableAdded(Variable variable)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(variable.Name));
        }

        private void OnValueChanged(int index)
        {
            Debug.Log(index);
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
            _variableName = _flow.GetVariable(name).Name;
        }

        private void OnVariableUpdated(Variable variable)
        {
            List<string> options = _flow.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
            Dropdown.SetValueWithoutNotify(_selectedIndex);
        }

        private void OnVariableDeleted(Variable variable)
        {
            List<string> options = _flow.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);

            int index = Dropdown.options.FindIndex(o => o.text == _variableName);
            _selectedIndex = index == -1 ? 0 : index;
            Dropdown.SetValueWithoutNotify(_selectedIndex);
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
                    _variableName = _flow.GetVariable(variableName).Name;
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