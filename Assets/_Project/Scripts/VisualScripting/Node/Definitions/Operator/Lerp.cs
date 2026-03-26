using UnityEngine;

namespace Loykas.Scripting
{
    class LerpNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;
        public InputValue T;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new LerpNode();
        }

        public LerpNode()
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

            T = CreateInputValue
            (
                nameof(T),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Number));
        }

        private ValueTransfer Get()
        {
            float t = T.GetValue().NumberValue;
            float a = A.GetValue().NumberValue;
            float b = B.GetValue().NumberValue;
            return ValueTransfer.CreateNumber(Mathf.Lerp(a, b, t));
        }
    }
}