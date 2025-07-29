using UnityEngine;

[CreateAssetMenu(fileName = "GreaterNode", menuName = "Scriptable Objects/Visual Scripting/Node/Greater")]
public class GreaterNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new GreaterNode(Title);
    }
}

class GreaterNode : ScriptNode
{
    public ValueInput ValueA;
    public ValueInput ValueB;

    public ValueOutput OutputPort;

    public GreaterNode(string title): base(title)
    {
        ValueA = ValueInput();
        ValueB = ValueInput();

        OutputPort = ValueOutput((vs) =>
        {
            return ValueA.GetValue<float>(vs) > ValueB.GetValue<float>(vs);
        });
    }
}