using UnityEngine;

namespace Loykas.Scripting
{

    class JoinNode : ScriptNode
    {
        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public JoinNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Any)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Any)).UseInput();

            Value = OutputValue(nameof(Value), Get);
        }

        private object Get() => A.GetValue().ToString() + B.GetValue().ToString();
    }
}