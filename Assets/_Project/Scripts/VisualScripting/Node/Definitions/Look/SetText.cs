namespace Loykas.Scripting
{
    class SetTextNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Text;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetTextNode();
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

            Text = CreateInputValue
            (
                nameof(Text),
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    LocalizationKey = nameof(Text)
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

            if (entity == null)
            {
                return Exit;
            }

            if (entity is not BoxEntity boxEntity)
            {
                return Exit;
            }

            string text = Text.GetValue().StringValue;

            boxEntity.TextBox.Text = text;
            return Exit;
        }
    }
}