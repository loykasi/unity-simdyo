namespace Loykas.Scripting
{
    class SetColliderNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetColliderNode();
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

            Value = CreateInputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean)).UseInput();
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

            bool enable = Value.GetValue().BoolValue;
            meshEntity.IsColliderEnabled = enable;
            return Exit;
        }
    }
}