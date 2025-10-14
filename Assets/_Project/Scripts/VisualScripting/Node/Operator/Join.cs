using UnityEngine;

namespace Loykas.Scripting
{

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