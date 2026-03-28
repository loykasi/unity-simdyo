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

        public override void Build()
        {
            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            )
            .UseInput();

            Min = CreateInputValue
            (
                nameof(Min),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Min)
                }
            )
            .UseInput();
            
            Max = CreateInputValue
            (
                nameof(Max),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Max)
                }
            )
            .UseInput();

            Output = CreateOutputValue
            (
                nameof(Output),
                Get,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    HideLabel = true
                }
            );
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