using UnityEngine;

namespace Loykas.Scripting
{
    class SubtractNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new SubtractNode();
        }

        public SubtractNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(nameof(Output), Get);
        }

        private object Get() => OperatorUtility.Subtract(A.GetValue(), B.GetValue());
    }
}