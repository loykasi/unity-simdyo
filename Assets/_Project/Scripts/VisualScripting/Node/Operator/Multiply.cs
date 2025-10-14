using UnityEngine;

namespace Loykas.Scripting
{
    class MultiplyNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public MultiplyNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return OperatorUtility.Multiply(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}