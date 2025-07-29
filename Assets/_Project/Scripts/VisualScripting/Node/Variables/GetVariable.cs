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
    public ValueInput input;
    public ValueOutput output;

    public GetVariableNode(string title) : base(title)
    {
        input = ValueInput(true);
        output = ValueOutput(Get);
    }

    private object Get(VisualScripting vs)
    {
        string name = input.GetValue(vs).ToString();
        return vs.GetVariable(name);
    }
}