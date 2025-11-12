using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class GetGlobalVariableNodeContent : ScriptNodeContent
    {
        public override Type Type => typeof(GetGlobalVariableNode);
        public override ScriptNode Create() => new GetGlobalVariableNode();
    }

    public class GetGlobalVariableNode : ScriptNode
    {
        public InputValue Input;
        public OutputValue Output;

        public GetGlobalVariableNode() : base()
        {
            Input = InputValue(nameof(Input), ScriptDataType.Single(DataType.String))
                            .UseGlobalVariableInput()
                            .DisableConnection()
                            .HideLabel();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get);

            Input.OnValueChanged += OnInputValueChanged;
        }

        public override void FlowAssigned()
        {
            SceneManager.Instance.GlobalScript.OnVariableDeleted += OnVariableDeleted;
        }

        private object Get()
        {
            string name = Input.GetValue().ToString();
            return SceneManager.Instance.GlobalScript.GetVariable(name);
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
            Variable variable = SceneManager.Instance.GlobalScript.GetVariable(name);

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