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
            Input = InputValue(nameof(Input))
                            .UseGlobalVariableInput()
                            .DisableConnection()
                            .HideLabel();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get);

            Input.OnValueChanged += OnInputValueChanged;
        }

        private object Get(ScriptFlow vs)
        {
            string name = Input.GetValue(vs).ToString();
            return EngineManager.Instance.GlobalScript.GetVariable(name).Value;
        }

        private void OnInputValueChanged()
        {
            string name = Input.GetValue(Flow).ToString();
            Variable variable = EngineManager.Instance.GlobalScript.GetVariable(name);

            if (variable == null)
            {
                return;
            }

            Output.SetType(variable.Type);
            OnNodeUpdated?.Invoke();

            IEnumerable<NodeConnection> connections = Flow.GetConnections(Output);
            foreach (var item in connections)
            {
                item.Validate();
            }
        }
    }
}