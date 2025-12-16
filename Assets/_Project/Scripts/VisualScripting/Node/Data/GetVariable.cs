using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class GetVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;
        public override bool CanUseGlobal => false;

        public InputValue Input;
        public OutputValue Output;

        private Variable _variable;

        public override ScriptNode Create()
        {
            return new GetVariableNode();
        }

        public GetVariableNode()
        {
            Input = InputValue(nameof(Input), ScriptDataType.Single(DataType.String))
                            .UseVariableInput()
                            .DisableConnection()
                            .HideLabel();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get).HideLabel();

            Input.OnValueChanged += OnInputValueChanged;
        }

        public override void FlowAssigned()
        {
            Flow.OnVariableDeleted += OnVariableDeleted;
        }

        private object Get()
        {
            string name = Input.GetValue().ToString();
            return Flow.GetVariable(name).Value;
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
            _variable = Flow.GetVariable(name);

            UpdateOutputType();

            if (_variable != null)
            {
                _variable.OnTypeUpdated += UpdateOutputType;
            }
        }

        private void UpdateOutputType()
        {
            ScriptDataType type = _variable == null ? ScriptDataType.Single(DataType.Any) : _variable.Type;

            Output.SetType(type);

            IEnumerable<NodeConnection> connections = Flow.GetConnections(Output);
            foreach (var item in connections)
            {
                item.Validate();
            }

            OnNodeUpdated?.Invoke();
        }
    }
}