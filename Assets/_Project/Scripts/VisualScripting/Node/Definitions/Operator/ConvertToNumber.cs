using UnityEngine;

namespace Loykas.Scripting
{
    class ConvertToNumberNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Value;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ConvertToNumberNode();
        }

        public ConvertToNumberNode()
        {
            Value = CreateInputValue(nameof(Value), ScriptDataType.Single(DataType.Any)).UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Number));
        }

        private ValueTransfer Get()
        {
            ValueTransfer value = Value.GetValue();

            float result = default;
            switch (value.Type.Type)
            {
                case DataType.String:
                    float.TryParse(value.StringValue, out result);
                    break;
                case DataType.Number:
                    result = value.NumberValue;
                    break;
                case DataType.Boolean:
                    result = value.BoolValue ? 1 : 0;
                    break;
                case DataType.Color:
                    result = value.ColorValue.H + value.ColorValue.S + value.ColorValue.V + value.ColorValue.A;
                    break;
            }

            return ValueTransfer.CreateNumber(result);
        }
    }
}