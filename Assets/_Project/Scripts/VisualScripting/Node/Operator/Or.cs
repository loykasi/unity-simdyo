using UnityEngine;

namespace Loykas.Scripting
{
    class OrNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new OrNode();
        }

        public OrNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();
            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(nameof(Output), Get);
        }

        private object Get() => A.GetValue<bool>() || B.GetValue<bool>();
    }
}