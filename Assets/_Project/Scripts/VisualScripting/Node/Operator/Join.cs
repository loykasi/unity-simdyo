using UnityEngine;

namespace Loykas.Scripting
{
    class JoinNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new JoinNode();
        }

        public JoinNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Any)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Any)).UseInput();

            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.String), Get);
        }

        private object Get() => A.GetValue().ToString() + B.GetValue().ToString();
    }
}