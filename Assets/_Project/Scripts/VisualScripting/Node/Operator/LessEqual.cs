using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "LessEqual", menuName = "Scriptable Objects/Visual Scripting/Node/LessEqual")]
public class LessEqual : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new LessEqualNode(Title);
    }
}

    class LessEqualNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public LessEqualNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<float>(vs) <= B.GetValue<float>(vs);
                }
            );
        }
    }
}