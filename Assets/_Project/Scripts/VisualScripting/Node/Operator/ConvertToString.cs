using UnityEngine;

namespace Loykas.Scripting
{
    class ConvertToString : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Value;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ConvertToString();
        }

        public ConvertToString()
        {
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any)).UseInput();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Number), Get).HideLabel();
        }

        private object Get() => Value.GetValue().ToString();
    }
}