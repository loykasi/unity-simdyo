using System.Collections.Generic;
using TMPro;

namespace Loykas.Scripting
{
    public class VariableNameInput : BaseInput
    {
        public TMP_Dropdown Dropdown;
        private int _selectedIndex;
        private Variable _variable;
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
            }

            if (_variable != null)
            {
                _variable.OnUpdated -= OnVariableUpdated;
            }
        }

        public void Init(ScriptFlow flow)
        {
            _flow = flow;
            _flow.OnVariableAdded += OnVariableAdded;
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
            _variable = _flow.GetVariable(name);
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
                int index = Dropdown.options.FindIndex(o => o.text.Equals(variableName));
                if (index != -1)
                {
                    if (_variable != null)
                    {
                        _variable.OnUpdated -= OnVariableUpdated;
                    }
                    _variable = _flow.GetVariable(variableName);
                    _variable.OnUpdated += OnVariableUpdated;
                }
                else
                {
                    Dropdown.value = 0;
                }
                
                Dropdown.SetValueWithoutNotify(_selectedIndex);
            }
            
        }
    }
}