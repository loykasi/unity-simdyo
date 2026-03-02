using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{    
    public class SetGlobalVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Variable;
        public InputValue Value;

        private Variable _variable;

        public override ScriptNode Create()
        {
            return new SetGlobalVariableNode();
        }

        public SetGlobalVariableNode() : base()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Variable = InputValue(nameof(Variable), ScriptDataType.Single(DataType.String))
                        .UseGlobalVariableInput()
                        .DisableConnection()
                        .HideLabel();
                            
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any))
                    .UseGlobalLocalized();

            Variable.OnValueChanged += OnInputValueChanged;
        }

        public override void FlowAssigned()
        {
            SceneManager.Instance.GlobalScript.OnVariableDeleted += OnVariableDeleted;
        }

        public override void Init()
        {
            UpdateInputType();
            OnNodeUpdated?.Invoke();
        }

        private OutputTrigger Set(NodeTask task)
        {
            string name = Variable.GetValue().ToString();
            object value = Value.GetValue();
            SceneManager.Instance.GlobalScript.UpdateVariable(name, value);
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
                Variable.SetValue("");
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
            string name = Variable.GetValue().ToString();
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