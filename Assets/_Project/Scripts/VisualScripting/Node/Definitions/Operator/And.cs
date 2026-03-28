namespace Loykas.Scripting
{
    class AndNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new AndNode();
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

            Output = CreateOutputValue
            (
                nameof(Output),
                Get,
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get() => ValueTransfer.CreateBool(A.GetValue().BoolValue && B.GetValue().BoolValue);
    }
}