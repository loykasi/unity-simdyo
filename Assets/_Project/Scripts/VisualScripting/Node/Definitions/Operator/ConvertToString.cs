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

        public override void Build()
        {
            Value = CreateInputValue(nameof(Value), ScriptDataType.Single(DataType.Any)).UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.String));
        }

        private ValueTransfer Get() => ValueTransfer.CreateString(Value.GetValue().ToString());
    }
}