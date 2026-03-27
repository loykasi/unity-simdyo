namespace Loykas.Scripting
{
    class SetSizeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Width;
        public InputValue Height;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetSizeNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            Width = CreateInputValue
            (
                nameof(Width),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Width)
                }
            ).UseInput();
                    
            Height = CreateInputValue
            (
                nameof(Height),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Height)
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

            if (entity is not BoxEntity boxEntity)
            {
                return Exit;
            }

            float width = Width.GetValue().NumberValue;
            float height = Height.GetValue().NumberValue;

            boxEntity.SetSize(width, height);
            return Exit;
        }
    }
}