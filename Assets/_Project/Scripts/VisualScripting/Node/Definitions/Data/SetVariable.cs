using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{    
    public class SetVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;
        public override bool CanUseGlobal => false;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Variable;
        public InputValue Value;

        private Variable _variable;

        public override ScriptNode Create()
        {
            return new SetVariableNode();
        }

        public SetVariableNode() : base()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            Variable = CreateInputValue
            (
                nameof(Variable),
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    IsConnectionDisabled = true,
                    HideLabel = true
                }
            )
            .UseInput(InputValueTypes.Variable);

            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Any(),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );

            Variable.OnValueChanged += OnInputValueChanged;
        }

        public override void FlowAssigned()
        {
            Flow.OnVariableDeleted += OnVariableDeleted;
        }

        public override void Init()
        {
            UpdateInputType();
            OnNodeUpdated?.Invoke();
        }

        private OutputTrigger Set(NodeTask task)
        {
            string name = Variable.GetValue().StringValue;
            ValueTransfer value = Value.GetValue();
            Flow.UpdateVariable(name, value.GetObjectValue());
            return Exit;
        }

        private void OnVariableDeleted(Variable variable)
        {
            if (Flow == null) return;

            object value = Variable.GetValue();

            if (value == null)
            {
                return;
            }

            string name = value.ToString();

            if (name.Equals(variable.Name))
            {
                Variable.SetValue(ValueTransfer.CreateString(string.Empty));
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
            UpdateInputType();
            ValidateConnections();
            OnNodeUpdated?.Invoke();
        }

        private void UpdateInputType()
        {
            string name = Variable.GetValue().StringValue;
            _variable = Flow.GetVariable(name);
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