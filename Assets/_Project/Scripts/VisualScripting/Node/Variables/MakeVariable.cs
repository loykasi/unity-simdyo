using System;
using Newtonsoft.Json;
using UnityEngine;

public abstract class MakeVariable : ScriptNodeData
{
    public abstract ScriptDataType Type { get; }

    public override ScriptNode Create()
    {
        return new MakeVariableNode(Type, Title);
    }
}

public class MakeVariableNode : ScriptNode
{
    [JsonIgnore]
    public InputValue Input;

    [JsonIgnore]
    public OutputValue Output;

    public MakeVariableNode(ScriptDataType type, string title) : base(title)
    {
        Input = InputValue(nameof(Input), type).UseInput().DisableConnection().HideLabel();
        Output = OutputValue(nameof(Output), type, (vs) => Input.GetValue(vs));
    }
}