using UnityEngine;

namespace Loykas.Scripting
{
    class LessNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public LessNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(nameof(Output), Get);
        }

        private object Get() => A.GetValue<float>() < B.GetValue<float>();
    }
}