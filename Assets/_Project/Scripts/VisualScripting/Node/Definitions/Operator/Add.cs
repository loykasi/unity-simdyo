namespace Loykas.Scripting
{
    class AddNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;
        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new AddNode();
        }

        public override void Build()
        {
            A = CreateInputValue
            (
                nameof(A),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();
            
            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Number),
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
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get()
        {
            ValueTransfer a = A.GetValue();
            ValueTransfer b = B.GetValue();

            return ValueTransfer.CreateNumber(a.NumberValue + b.NumberValue);
        }
    }
}