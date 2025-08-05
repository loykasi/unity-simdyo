using UnityEngine;

[CreateAssetMenu(fileName = "MakeVector", menuName = "Scriptable Objects/Visual Scripting/Node/Make Vector")]
public class MakeVector : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new MakeVectorNode(Title);
    }
}

public class MakeVectorNode : ScriptNode
{
    public ValueInput inputX;
    public ValueInput inputY;
    public ValueOutput output;

    public MakeVectorNode(string title) : base(title)
    {
        inputX = ValueInput(DataType.Number, true);
        inputY = ValueInput(DataType.Number, true);
        output = ValueOutput(DataType.Vector, Get);
    }

    private object Get(VisualScripting vs)
    {
        float x = (float)inputX.GetValue(vs);
        float y = (float)inputY.GetValue(vs);
        return new Vector3(x, y);
    }
}