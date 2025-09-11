using UnityEngine;

[CreateAssetMenu(fileName = "AddNode", menuName = "Scriptable Objects/Visual Scripting/Node/Add")]
public class AddNodeData : ScriptNodeData
{
    public string InputA;
    public string InputB;

    public override ScriptNode Create()
    {
        return new AddNode(Title);
    }
}

class AddNode : ScriptNode
{
    public InputValue A;
    public InputValue B;

    public OutputValue Value;

    public AddNode(string title): base(title)
    {
        A = InputValue(nameof(A), DataType.Number).UseInput();
        B = InputValue(nameof(B), DataType.Number).UseInput();

        Value = OutputValue(
            nameof(Value),
            (vs) =>
            {
                return OperatorUtility.Add(A.GetValue(vs), B.GetValue(vs));;
            }
        );
    }
}