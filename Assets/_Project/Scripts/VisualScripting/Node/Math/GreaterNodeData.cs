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
    public InputValue A;
    public InputValue B;

    public OutputValue Output;

    public GreaterNode(string title): base(title)
    {
        A = InputValue(nameof(A));
        B = InputValue(nameof(B));

        Output = OutputValue(
            nameof(Output),
            (vs) =>
            {
                return A.GetValue<float>(vs) > B.GetValue<float>(vs);
            }
        );
    }
}