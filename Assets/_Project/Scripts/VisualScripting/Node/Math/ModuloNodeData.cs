using UnityEngine;

[CreateAssetMenu(fileName = "ModuloNode", menuName = "Scriptable Objects/Visual Scripting/Node/Modulo")]
public class ModuloNodeData : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new ModuloNode(Title);
    }
}

class ModuloNode : ScriptNode
{
    public ValueInput ValueA;
    public ValueInput ValueB;

    public ValueOutput OutputPort;

    public ModuloNode(string title): base(title)
    {
        ValueA = ValueInput();
        ValueB = ValueInput();

        OutputPort = ValueOutput(() =>
        {
            return OperatorUtility.Modulo(ValueA.GetValue(), ValueB.GetValue());
        });
    }
}