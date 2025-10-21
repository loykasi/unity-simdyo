using UnityEngine;

namespace Loykas.Scripting
{

    class SubtractNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public SubtractNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return OperatorUtility.Subtract(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}