namespace Loykas.Scripting
{
    class LogNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Debug;

        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Value;

        public override ScriptNode Create()
        {
            return new LogNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Log);
            Exit = CreateOutputTrigger(nameof(Exit));
            
            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Any(),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );
        }

        private OutputTrigger Log(NodeTask task)
        {
            var value = Value.GetValue().ToString();
            LogConsole.Instance.Log(value);

            return Exit;
        }
    }
}