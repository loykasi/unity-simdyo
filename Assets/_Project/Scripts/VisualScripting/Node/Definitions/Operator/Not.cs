using UnityEngine;

namespace Loykas.Scripting
{
    class NotNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue Value;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new NotNode();
        }

        public NotNode()
        {
            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean))
                    .UseInput()
                    .UseGlobalLocalized();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get).HideLabel();
        }
        
        private object Get() => !Value.GetValue<bool>();
    }
}