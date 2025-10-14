using UnityEngine;

namespace Loykas.Scripting
{
    class EqualNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public EqualNode(string title) : base(title)
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(
                nameof(Output),
                (vs) =>
                {
                    return A.GetValue<float>(vs) == B.GetValue<float>(vs);
                }
            );
        }
    }
}