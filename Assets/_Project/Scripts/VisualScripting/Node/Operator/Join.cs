using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "Join", menuName = "Scriptable Objects/Visual Scripting/Node/Join")]
    public class Join : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new JoinNode(Title);
        }
    }

    class JoinNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public JoinNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Any)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Any)).UseInput();

            Value = OutputValue(
                nameof(Value),
                (vs) =>
                {
                    return A.GetValue(vs).ToString() + B.GetValue(vs).ToString();
                }
            );
        }
    }
}