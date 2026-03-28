namespace Loykas.Scripting
{
    class GetAngleNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputValue Entity;
        public OutputValue Value;

        public override void Build()
        {
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
                        
            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );
        }

        public override ScriptNode Create()
        {
            return new GetAngleNode();
        }

        public ValueTransfer Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(entity.Angle);
        }
    }
}