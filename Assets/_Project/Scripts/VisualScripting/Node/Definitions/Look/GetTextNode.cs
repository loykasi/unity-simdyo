namespace Loykas.Scripting
{
    class GetTextNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Text;

        public override ScriptNode Create()
        {
            return new GetTextNode();
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
                        
            Text = CreateOutputValue
            (
                nameof(Text),
                Get,
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    LocalizationKey = nameof(Text)
                }
            );
        }

        public ValueTransfer Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateString(default);
            }

            if (entity is not BoxEntity boxEntity)
            {
                return ValueTransfer.CreateString(default);
            }

            return ValueTransfer.CreateString(boxEntity.TextBox.Text);
        }
    }
}