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
            A = CreateInputValue
            (
                nameof(A),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();
            
            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Number));
        }

        private ValueTransfer Get() => ValueTransfer.CreateNumber(A.GetValue().NumberValue % B.GetValue().NumberValue);
    }
}