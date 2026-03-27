namespace Loykas.Scripting
{
    class GetPositionNode : ScriptNode
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
            return new GetPositionNode();
        }

        private ValueTransfer GetX()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(entity.Position.x);
        }

        private ValueTransfer GetY()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(entity.Position.y);
        }
    }
}