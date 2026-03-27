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
                ScriptDataType.Single(DataType.Any)
            );

            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Any)
            );

            Value = CreateOutputValue(nameof(Value), Get, ScriptDataType.Single(DataType.String));
        }

        private ValueTransfer Get() => ValueTransfer.CreateString(A.GetValue().ToString() + B.GetValue().ToString());
    }
}