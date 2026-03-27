namespace Loykas.Scripting
{
    class GetSizeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Width;
        public OutputValue Height;

        public override ScriptNode Create()
        {
            return new GetSizeNode();
        }

        public override void Build()
        {
            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    HideLabel = true,
                }
            )
            .UseInput()
            .NullMeanSelf();
                        
            Width = CreateOutputValue
            (
                nameof(Width),
                GetWidth,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Width)
                }
            );
            
            Height = CreateOutputValue
            (
                nameof(Height),
                GetHeight,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Height)
                }
            );
        }

        public ValueTransfer GetWidth()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateNumber(default);
            }

            if (entity is not BoxEntity boxEntity)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(boxEntity.Width);
        }

        public ValueTransfer GetHeight()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateNumber(default);
            }

            if (entity is not BoxEntity boxEntity)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(boxEntity.Height);
        }
    }
}