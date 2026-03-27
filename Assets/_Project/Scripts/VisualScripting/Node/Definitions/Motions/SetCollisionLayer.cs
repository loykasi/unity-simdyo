namespace Loykas.Scripting
{
    class SetCollisionLayerNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetCollisionLayerNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            Value = CreateInputValue(nameof(Value), ScriptDataType.Single(DataType.Number))
                    .UseInput(InputValueTypes.CollisionLayer);

            Entity = CreateInputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
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

            int layer = (int)Value.GetValue().NumberValue;
            
            meshEntity.SetLayer(layer);
            return Exit;
        }
    }
}