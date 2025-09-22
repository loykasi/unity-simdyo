using System;
using UnityEngine;

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
        Input = InputValue(nameof(Input)).UseVariableInput();
        Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Any), Get);

        Input.OnValueChanged += OnInputValueChanged;
    }

    private void OnInputValueChanged()
    {
        string name = Input.GetValue(Flow).ToString();
        Variable variable = Flow.GetVariable(name);
        Output.SetType(variable.Type);

        OnNodeUpdated?.Invoke();

        for (int i = 0; i < Output.Destinations.Count; i++)
        {
            // Output.Destinations[i].
        }
    }

    private object Get(ScriptFlow vs)
    {
        string name = Input.GetValue(vs).ToString();
        return vs.GetVariable(name).Value;
    }
}