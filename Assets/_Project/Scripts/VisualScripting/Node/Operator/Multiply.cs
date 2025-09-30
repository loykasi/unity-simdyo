using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "Multiply", menuName = "Scriptable Objects/Visual Scripting/Node/Multiply")]
public class Multiply : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new MultiplyNode(Title);
    }
}

    class MultiplyNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public MultiplyNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return OperatorUtility.Multiply(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}