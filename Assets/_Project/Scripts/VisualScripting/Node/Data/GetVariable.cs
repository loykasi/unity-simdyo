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

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get);

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

            string name = Input.GetValue().ToString();
            
            if (name.Equals(variable.Name))
            {
                Input.SetValue("");
            }
        }

        private void OnInputValueChanged()
        {
            if (Flow == null) return;

            string name = Input.GetValue().ToString();
            Variable variable = Flow.GetVariable(name);

            ScriptDataType type = variable == null ? ScriptDataType.Single(DataType.Any) : variable.Type;

            Output.SetType(type);
            OnNodeUpdated?.Invoke();

            IEnumerable<NodeConnection> connections = Flow.GetConnections(Output);
            foreach (var item in connections)
            {
                item.Validate();
            }
        }
    }
}