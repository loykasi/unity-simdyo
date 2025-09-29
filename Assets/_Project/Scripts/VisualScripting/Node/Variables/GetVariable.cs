using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "GetVariable", menuName = "Scriptable Objects/Visual Scripting/Node/Get Variable")]
public class GetVariable : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new GetVariableNode(Title);
    }
}

    public class GetVariableNode : ScriptNode
    {
        public InputValue Input;
        public OutputValue Output;

        public GetVariableNode(string title) : base(title)
        {
            Input = InputValue(nameof(Input))
                            .UseVariableInput()
                            .DisableConnection()
                            .HideLabel();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get);

            Input.OnValueChanged += OnInputValueChanged;
        }

        private object Get(ScriptFlow vs)
        {
            string name = Input.GetValue(vs).ToString();
            return vs.GetVariable(name).Value;
        }

        private void OnInputValueChanged()
        {
            string name = Input.GetValue(Flow).ToString();
            Variable variable = Flow.GetVariable(name);

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