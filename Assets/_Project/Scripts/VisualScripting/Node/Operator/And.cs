using UnityEngine;

namespace Loykas.Scripting
{
    class AndNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public AndNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(nameof(Output), Get);
        }

        private object Get() => A.GetValue<bool>() && B.GetValue<bool>();
    }
}