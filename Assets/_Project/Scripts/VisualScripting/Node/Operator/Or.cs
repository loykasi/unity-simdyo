using UnityEngine;

namespace Loykas.Scripting
{
[CreateAssetMenu(fileName = "Or", menuName = "Scriptable Objects/Visual Scripting/Node/Or")]
public class Or : ScriptNodeData
{
    public override ScriptNode Create()
    {
        return new OrNode(Title);
    }
}

    class OrNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public OrNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<bool>(vs) || B.GetValue<bool>(vs);
                }
            );
        }
    }
}