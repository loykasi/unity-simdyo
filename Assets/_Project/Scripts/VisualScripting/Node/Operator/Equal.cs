using UnityEngine;

namespace Loykas.Scripting
{
    class EqualNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new EqualNode();
        }

        public EqualNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number)).UseInput();

            Output = OutputValue(nameof(Output), Get);
        }

        private object Get() => A.GetValue<float>() == B.GetValue<float>();
    }
}