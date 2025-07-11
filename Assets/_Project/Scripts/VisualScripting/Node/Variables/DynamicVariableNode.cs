using System;

public class DynamicVariableNode : ScriptNode
{
    public ValueOutput output;
    public object Value;

    public DynamicVariableNode(string title) : base(title)
    {
        output = ValueOutput();
    }
}