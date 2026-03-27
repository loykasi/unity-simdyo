namespace Loykas.Scripting
{
    class GetVelocityNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputValue Entity;
        public OutputValue X;
        public OutputValue Y;

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

            X = CreateOutputValue
            (
                nameof(X),
                GetX,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );
            
            Y = CreateOutputValue
            (
                nameof(Y),
                GetY,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            );
        }

        public override ScriptNode Create()
        {
            return new GetVelocityNode();
        }

        private ValueTransfer GetX()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(meshEntity.Velocity.x);
        }

        private ValueTransfer GetY()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(meshEntity.Velocity.y);
        }
    }
}