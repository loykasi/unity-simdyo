namespace Loykas.Scripting
{
    class JoinNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new JoinNode();
        }

        public override void Build()
        {
            A = CreateInputValue
            (
                nameof(A),
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );

            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Any),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );

            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get() => ValueTransfer.CreateString(A.GetValue().ToString() + B.GetValue().ToString());
    }
}