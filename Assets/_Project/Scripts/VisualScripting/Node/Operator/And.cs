using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "And", menuName = "Scriptable Objects/Visual Scripting/Node/And")]
public class And : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new AndNode(Title);
    }
}

    class AndNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public AndNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<bool>(vs) && B.GetValue<bool>(vs);
                }
            );
        }
    }
}