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
            Input = CreateInputValue
            (
                nameof(Input),
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    IsConnectionDisabled = true,
                    LocalizationKey = nameof(Input)
                }
            )
            .UseInput(InputValueTypes.GlobalVariable);

            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    LocalizationKey = nameof(Input)
                }
            );

            Input.OnValueChanged += OnInputValueChanged;
        }

        public override void FlowAssigned()
        {
            SceneManager.Instance.GlobalScript.OnVariableDeleted += OnVariableDeleted;
        }

        public override void Init()
        {
            UpdateOutputType();
            OnNodeUpdated?.Invoke();
        }

        private ValueTransfer Get()
        {
            string name = Input.GetValue().StringValue;
            return SceneManager.Instance.GlobalScript.GetVariable(name).GetValueTransfer();
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
                Input.SetValue(ValueTransfer.CreateString(string.Empty));
            }
        }

        private void OnInputValueChanged()
        {
            if (Flow == null) return;
            
            if (_variable != null)
            {
                _variable.OnTypeUpdated -= OnTypeUpdated;
            }

            OnTypeUpdated();

            if (_variable != null)
            {
                _variable.OnTypeUpdated += OnTypeUpdated;
            }
        }

        private void OnTypeUpdated()
        {
            UpdateOutputType();
            ValidateConnections();
            OnNodeUpdated?.Invoke();
        }

        private void UpdateOutputType()
        {
            string name = Input.GetValue().StringValue;
            _variable = SceneManager.Instance.GlobalScript.GetVariable(name);
            ScriptDataType type = _variable == null ? ScriptDataType.Single(DataType.Any) : _variable.Type;
            Value.SetType(type);
        }

        private void ValidateConnections()
        {
            IEnumerable<NodeConnection> connections = Flow.GetConnections(Value);
            foreach (var item in connections)
            {
                item.Validate();
            }
        }
    }
}