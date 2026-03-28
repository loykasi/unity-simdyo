namespace Loykas.Scripting
{
    class OrNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new OrNode();
        }

        public override void Build()
        {
            A = CreateInputValue
            (
                nameof(A),
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();
            
            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();

            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get() => ValueTransfer.CreateBool(A.GetValue().BoolValue || B.GetValue().BoolValue);
    }
}