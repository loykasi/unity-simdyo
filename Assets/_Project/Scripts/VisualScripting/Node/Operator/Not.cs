using UnityEngine;

namespace Loykas.Scripting
{
    class NotNode : ScriptNode
    {
        public InputValue A;

        public OutputValue Output;

        public NotNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return ! A.GetValue<bool>(vs);
                }
            );
        }
    }
}