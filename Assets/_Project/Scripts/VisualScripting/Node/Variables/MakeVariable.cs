using System;
using UnityEngine;

public abstract class MakeVariable : ScriptNodeData
{
    public abstract DataType Type { get; }

    public override ScriptNode Create()
    {
        return new MakeVariableNode(Type, Title);
    }
}

public class MakeVariableNode : ScriptNode
{
    public ValueInput input;
    public ValueOutput output;

    public MakeVariableNode(DataType type, string title) : base(title)
    {
        input = ValueInput(type, true);
        output = ValueOutput(type, (vs) => input.GetValue(vs));
    }
}