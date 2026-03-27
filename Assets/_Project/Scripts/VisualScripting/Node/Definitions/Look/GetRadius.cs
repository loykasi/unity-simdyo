namespace Loykas.Scripting
{
    class GetRadiusNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetRadiusNode();
        }

        public override void Build()
        {
            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity)
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

        public ValueTransfer Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return ValueTransfer.CreateNumber(default);
            }

            if (entity is not CircleEntity circleEntity)
            {
                return ValueTransfer.CreateNumber(default);
            }

            return ValueTransfer.CreateNumber(circleEntity.Radius);
        }
    }
}