using UnityEngine;

namespace Loykas.Scripting
{

    class OrNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public OrNode()
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