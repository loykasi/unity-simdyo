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
            _variable = _flow.GetVariable(name);
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
            List<string> options = SceneManager.Instance.GlobalScript.GetVariableOptions();

            Dropdown.ClearOptions();
            Dropdown.AddOptions(options);
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
                    _variable = _flow.GetVariable(variableName);
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