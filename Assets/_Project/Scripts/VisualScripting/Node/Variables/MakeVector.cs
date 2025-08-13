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
    public InputValue X;
    public InputValue Y;
    public OutputValue Output;

    public MakeVectorNode(string title) : base(title)
    {
        X = InputValue(nameof(X), DataType.Number, true);
        Y = InputValue(nameof(Y), DataType.Number, true);
        Output = OutputValue(nameof(Output), DataType.Vector, Get);
    }

    private object Get(VisualScripting vs)
    {
        float x = (float)X.GetValue(vs);
        float y = (float)Y.GetValue(vs);
        return new Vector3(x, y);
    }
}