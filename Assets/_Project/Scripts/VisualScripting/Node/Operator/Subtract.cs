using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "Subtract", menuName = "Scriptable Objects/Visual Scripting/Node/Subtract")]
public class Subtract : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new SubtractNode(Title);
    }
}

    class SubtractNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public SubtractNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return OperatorUtility.Subtract(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}