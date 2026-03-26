namespace Loykas.Scripting
{
    class SendSignalNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Name;
        public InputValue Entity;

        public SendSignalNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), SendSignal);

            Exit = CreateOutputTrigger(nameof(Exit), PortSettings.Default);

            Name = CreateInputValue
            (
                nameof(Name),
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    HideLabel = true,
                    IsConnectionDisabled = true
                }
            ).UseInput();
                
            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    LocalizationKey = nameof(Entity)
                }
            )
            .UseInput(InputValueTypes.SignalEntity);
        }

        public override ScriptNode Create()
        {
            return new SendSignalNode();
        }

        private OutputTrigger SendSignal(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            string signalName = Name.GetValue().StringValue;
            SignalSystem.Instance.SendSignal(signalName, entity);
            return Exit;
        }
    }
}