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

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Boolean));
        }

        private ValueTransfer Get() => ValueTransfer.CreateBool(A.GetValue().BoolValue || B.GetValue().BoolValue);
    }
}