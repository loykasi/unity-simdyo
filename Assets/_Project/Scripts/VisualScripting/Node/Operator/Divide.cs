using UnityEngine;

namespace Loykas.Scripting
{
    class DivideNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public DivideNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return OperatorUtility.Divide(A.GetValue(vs), B.GetValue(vs));
                }
            );
        }
    }
}