using UnityEngine;

namespace Loykas.Scripting
{
    class RandomNumberNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Min;
        public InputValue Max;
        public InputValue IntegersOnly;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new RandomNumberNode();
        }

        public override void Build()
        {
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

            IntegersOnly = CreateInputValue
            (
                nameof(IntegersOnly),
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    IsConnectionDisabled = true
                }
            )
            .UseInput();

            Value = CreateOutputValue
            (
                nameof(Value),
                GetRandom,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer GetRandom()
        {
            float a = Min.GetValue().NumberValue;
            float b = Max.GetValue().NumberValue;
            
            bool isInteger = IntegersOnly.GetValue().BoolValue;
            if (isInteger)
            {
                return ValueTransfer.CreateNumber(Random.Range((int)a, (int)b + 1));
            }
            return ValueTransfer.CreateNumber(Random.Range(a, b));
        }
    }
}