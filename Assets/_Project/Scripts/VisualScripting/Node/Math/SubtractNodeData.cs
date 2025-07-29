using UnityEngine;

[CreateAssetMenu(fileName = "SubtractNode", menuName = "Scriptable Objects/Visual Scripting/Node/Subtract")]
public class SubtractNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new SubtractNode(Title);
    }
}

class SubtractNode : ScriptNode
{
    public ValueInput ValueA;
    public ValueInput ValueB;

    public ValueOutput OutputPort;

    public SubtractNode(string title): base(title)
    {
        ValueA = ValueInput();
        ValueB = ValueInput();

        OutputPort = ValueOutput((vs) =>
        {
            return OperatorUtility.Subtract(ValueA.GetValue(vs), ValueB.GetValue(vs));
        });
    }
}