namespace Loykas.Scripting
{
    class GetTextureSlotNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetTextureSlotNode();
        }

        public override void Build()
        {
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
                        
            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );
        }

        public ValueTransfer Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return ValueTransfer.CreateString(default);
            }

            return ValueTransfer.CreateString(meshEntity.TextureSlotKey);
        }
    }
}