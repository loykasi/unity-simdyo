using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "GreaterEqual", menuName = "Scriptable Objects/Visual Scripting/Node/GreaterEqual")]
public class GreaterEqual : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new GreaterEqualNode(Title);
    }
}

    class GreaterEqualNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public GreaterEqualNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<float>(vs) >= B.GetValue<float>(vs);
                }
            );
        }
    }
}