using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class GetGlobalVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;

        public InputValue Input;
        public OutputValue Value;

        private Variable _variable;

        public override ScriptNode Create()
        {
            return new GetGlobalVariableNode();
        }

        public GetGlobalVariableNode() : base()
        {
            Input = InputValue(nameof(Input), ScriptDataType.Single(DataType.String))
                    .UseGlobalVariableInput()
                    .DisableConnection()
                    .HideLabel();

            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Any), Get)
                    .UseGlobalLocalized();

            Input.OnValueChanged += OnInputValueChanged;
        }

        public override void FlowAssigned()
        {
            SceneManager.Instance.GlobalScript.OnVariableDeleted += OnVariableDeleted;
        }

        private object Get()
        {
            string name = Input.GetValue().ToString();
            return SceneManager.Instance.GlobalScript.GetVariable(name).Value;
        }

        private void OnVariableDeleted(Variable variable)
        {
            if (Flow == null) return;

            object value = Input.GetValue();

            if (value == null)
            {
                return;
            }

            string name = value.ToString();

            if (name.Equals(variable.Name))
            {
                Input.SetValue("");
            }
        }

        private void OnInputValueChanged()
        {
            if (Flow == null) return;
            
            if (_variable != null)
            {
                _variable.OnTypeUpdated -= UpdateOutputType;
            }

            string name = Input.GetValue().ToString();
            _variable = SceneManager.Instance.GlobalScript.GetVariable(name);

            UpdateOutputType();

            if (_variable != null)
            {
                _variable.OnTypeUpdated += UpdateOutputType;
            }
        }

        private void UpdateOutputType()
        {
            ScriptDataType type = _variable == null ? ScriptDataType.Single(DataType.Any) : _variable.Type;

            Value.SetType(type);

            IEnumerable<NodeConnection> connections = Flow.GetConnections(Value);
            foreach (var item in connections)
            {
                item.Validate();
            }

            OnNodeUpdated?.Invoke();
        }
    }
}