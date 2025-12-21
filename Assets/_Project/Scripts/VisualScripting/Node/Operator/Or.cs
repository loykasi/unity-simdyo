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
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean))
                .UseInput()
                .NoLocalize();

            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Boolean))
                .UseInput()
                .NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get).HideLabel();
        }

        private object Get() => A.GetValue<bool>() || B.GetValue<bool>();
    }
}