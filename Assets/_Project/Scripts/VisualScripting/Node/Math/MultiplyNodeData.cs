using UnityEngine;

[CreateAssetMenu(fileName = "MultiplyNode", menuName = "Scriptable Objects/Visual Scripting/Node/Multiply")]
public class MultiplyNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new MultiplyNode(Title);
    }
}

class MultiplyNode : ScriptNode
{
    public ValueInput ValueA;
    public ValueInput ValueB;

    public ValueOutput OutputPort;

    public MultiplyNode(string title): base(title)
    {
        ValueA = ValueInput();
        ValueB = ValueInput();

        OutputPort = ValueOutput(() =>
        {
            return OperatorUtility.Multiply(ValueA.GetValue(), ValueB.GetValue());
        });
    }
}