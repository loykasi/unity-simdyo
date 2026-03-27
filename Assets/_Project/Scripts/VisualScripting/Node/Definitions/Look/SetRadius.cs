namespace Loykas.Scripting
{
    class SetRadiusNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetRadiusNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            ).UseInput();

            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    HideLabel = true
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

            if (entity is not CircleEntity circleEntity)
            {
                return Exit;
            }

            float angle = Value.GetValue().NumberValue;
            circleEntity.SetRadius(angle);
            return Exit;
        }
    }
}