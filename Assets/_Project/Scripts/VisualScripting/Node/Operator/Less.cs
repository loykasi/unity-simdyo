using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "Less", menuName = "Scriptable Objects/Visual Scripting/Node/Less")]
public class Less : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new LessNode(Title);
    }
}

    class LessNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public LessNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<float>(vs) < B.GetValue<float>(vs);
                }
            );
        }
    }
}