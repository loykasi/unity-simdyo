using UnityEngine;

namespace Loykas.Scripting
{
    class ClampNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Value;
        public InputValue Min;
        public InputValue Max;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ClampNode();
        }

        public ClampNode()
        {
            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();

            Min = CreateInputValue
            (
                nameof(Min),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();
            
            Max = CreateInputValue
            (
                nameof(Max),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Number));
        }

        private ValueTransfer Get()
        {
            float value = Value.GetValue().NumberValue;
            float min = Min.GetValue().NumberValue;
            float max = Max.GetValue().NumberValue;
            return ValueTransfer.CreateNumber(Mathf.Clamp(value, min, max));
        }
    }
}