using UnityEngine;

namespace Loykas.Scripting
{
    class ModuloNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ModuloNode();
        }

        public ModuloNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            B = InputValue(nameof(B), ScriptDataType.Single(DataType.Number))
                .UseInput()
                .NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Number), Get).HideLabel();
        }

        private object Get() => OperatorUtility.Modulo(A.GetValue(), B.GetValue());
    }
}