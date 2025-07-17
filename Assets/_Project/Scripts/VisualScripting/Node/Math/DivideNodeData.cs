using UnityEngine;

[CreateAssetMenu(fileName = "DivideNode", menuName = "Scriptable Objects/Visual Scripting/Node/Divide")]
public class DivideNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new DivideNode(Title);
    }
}

class DivideNode : ScriptNode
{
    public ValueInput ValueA;
    public ValueInput ValueB;

    public ValueOutput OutputPort;

    public DivideNode(string title): base(title)
    {
        ValueA = ValueInput();
        ValueB = ValueInput();

        OutputPort = ValueOutput(() =>
        {
            return OperatorUtility.Divide(ValueA.GetValue(), ValueB.GetValue());
        });
    }
}