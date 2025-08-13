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
    public InputValue A;
    public InputValue B;

    public OutputValue Output;

    public ModuloNode(string title): base(title)
    {
        A = InputValue(nameof(A));
        B = InputValue(nameof(B));

        Output = OutputValue(
            nameof(Output),
            (vs) =>
            {
                return OperatorUtility.Modulo(A.GetValue(vs), B.GetValue(vs));
            }
        );
    }
}