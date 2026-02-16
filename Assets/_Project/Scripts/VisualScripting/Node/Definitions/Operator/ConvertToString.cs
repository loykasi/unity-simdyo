using UnityEngine;

namespace Loykas.Scripting
{
    class ConvertToStringNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Value;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new ConvertToStringNode();
        }

        public ConvertToStringNode()
        {
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Any)).UseInput().NoLocalize();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.String), Get).HideLabel();
        }

        private object Get() => Value.GetValue().ToString();
    }
}