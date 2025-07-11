using UnityEngine;

[CreateAssetMenu(fileName = "AddNode", menuName = "Scriptable Objects/Visual Scripting/Node/Add")]
public class AddNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new AddNode(Title);
    }
}

class AddNode : ScriptNode
{
    public ValueInput ValueA;
    public ValueInput ValueB;

    public ValueOutput OutputPort;

    public AddNode(string title): base(title)
    {
        ValueA = ValueInput();
        ValueB = ValueInput();

        OutputPort = ValueOutput(() =>
        {
            return ValueA.GetValue<float>() + ValueB.GetValue<float>();
        });
    }
}