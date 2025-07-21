using System;
using UnityEngine;

// [CreateAssetMenu(fileName = "Break", menuName = "Scriptable Objects/Visual Scripting/Node/Break")]
public abstract class MakeVariable : ScriptNodeData
{
    public abstract Variable Type { get; }

    public override ScriptNode Create()
    {
        return new MakeVariableNode(Type, Title);
    }
}

public class MakeVariableNode : ScriptNode
{
    public ValueInput input;
    public ValueOutput output;

    public MakeVariableNode(Variable type, string title) : base(title)
    {
        input = ValueInput(type, true);
        output = ValueOutput(type, () => input.GetValue());
    }
}