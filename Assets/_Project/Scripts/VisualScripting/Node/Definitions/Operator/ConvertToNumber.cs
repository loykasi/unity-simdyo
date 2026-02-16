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
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any)).UseInput().NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Number), Get).HideLabel();
        }

        private object Get()
        {
            if (Value.HasConnection)
            {
                return default(float);
            }
            
            object value = Value.GetValue();

            if (value is float floatValue)
            {
                return floatValue;
            }

            if (value is string stringValue)
            {
                if (float.TryParse(stringValue, out float result))
                {
                    return result;
                }
                return default(float);
            }

            if (value is bool boolValue)
            {
                return boolValue ? 1 : 0;
            }

            if (value is ColorHSV colorValue)
            {
                return colorValue.H + colorValue.S + colorValue.V + colorValue.A;
            }

            return default(float);
        }
    }
}