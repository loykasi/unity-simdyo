using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "Equal", menuName = "Scriptable Objects/Visual Scripting/Node/Equal")]
public class Equal : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new EqualNode(Title);
    }
}

    class EqualNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public EqualNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<float>(vs) == B.GetValue<float>(vs);
                }
            );
        }
    }
}