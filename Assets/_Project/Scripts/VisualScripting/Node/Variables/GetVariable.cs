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
        Input = InputValue(nameof(Input), true);
        Output = OutputValue(nameof(Output), Get);
    }

    private object Get(VisualScripting vs)
    {
        string name = Input.GetValue(vs).ToString();
        return vs.GetVariable(name);
    }
}