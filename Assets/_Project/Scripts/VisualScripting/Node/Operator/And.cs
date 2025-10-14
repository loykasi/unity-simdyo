using UnityEngine;

namespace Loykas.Scripting
{
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