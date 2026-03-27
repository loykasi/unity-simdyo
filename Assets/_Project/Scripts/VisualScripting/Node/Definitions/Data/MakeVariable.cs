namespace Loykas.Scripting
{
    public abstract class MakeVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;
        
        public abstract DataType VariableType { get; }
        public InputValue Input;
        public OutputValue Output;

        public override void Build()
        {
            Input = CreateInputValue
            (
                nameof(Input),
                ScriptDataType.Single(VariableType),
                new PortSettings
                {
                    IsConnectionDisabled = true,
                    HideLabel = true
                }
            )
            .UseInput();
                        
            Output = CreateOutputValue
            (
                nameof(Output),
                Get,
                ScriptDataType.Single(VariableType),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get() => Input.GetValue();
    }
}