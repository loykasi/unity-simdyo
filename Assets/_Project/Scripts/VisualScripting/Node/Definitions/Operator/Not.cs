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
            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Boolean)
            ).UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Boolean));
        }
        
        private ValueTransfer Get() => ValueTransfer.CreateBool(!Value.GetValue().BoolValue);
    }
}