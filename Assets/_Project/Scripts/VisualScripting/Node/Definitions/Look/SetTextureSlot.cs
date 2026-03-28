namespace Loykas.Scripting
{
    class SetTextureSlotNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetTextureSlotNode();
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
                ScriptDataType.Single(DataType.String),
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
                    LocalizationKey = nameof(Entity)
                }
            )
            .UseInput()
            .NullMeanSelf();
        }

        public OutputTrigger Set(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return Exit;
            }

            string key = Value.GetValue().StringValue;
            meshEntity.SetTexture(key);
            return Exit;
        }
    }
}