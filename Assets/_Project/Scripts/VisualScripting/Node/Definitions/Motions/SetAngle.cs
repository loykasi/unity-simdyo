namespace Loykas.Scripting
{
    class SetAngleNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetAngleNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            
            Exit = CreateOutputTrigger
            (
                nameof(Exit),
                new PortSettings
                {
                    HideLabel = true
                }
            );

            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            )
            .UseInput();
            
            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    LocalizationKey = nameof(Entity)
                }
            )
            .UseInput()
            .NullMeanSelf();
        }

        public OutputTrigger Set(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return Exit;
            }

            float angle = Value.GetValue().NumberValue;
            entity.Angle = angle;
            return Exit;
        }
    }
}