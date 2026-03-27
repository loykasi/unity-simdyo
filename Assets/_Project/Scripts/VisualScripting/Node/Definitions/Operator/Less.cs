namespace Loykas.Scripting
{
    class LessNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;
        public InputValue B;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new LessNode();
        }

        public override void Build()
        {
            A = CreateInputValue
            (
                nameof(A),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();
            
            B = CreateInputValue
            (
                nameof(B),
                ScriptDataType.Single(DataType.Number)
            )
            .UseInput();

            Output = CreateOutputValue(nameof(Output), Get, ScriptDataType.Single(DataType.Boolean));
        }

        private ValueTransfer Get() => ValueTransfer.CreateBool(A.GetValue().NumberValue < B.GetValue().NumberValue);
    }
}