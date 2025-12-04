using UnityEngine;

namespace Loykas.Scripting
{
    class AddNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new AddNode();
        }

        public AddNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Value = OutputValue(nameof(Value), Get);
        }

        private object Get() => OperatorUtility.Add(A.GetValue(), B.GetValue());
    }
}