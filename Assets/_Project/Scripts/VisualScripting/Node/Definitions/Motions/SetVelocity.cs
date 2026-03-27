using UnityEngine;

namespace Loykas.Scripting
{
    class SetVelocityNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue X;
        public InputValue Y;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetVelocityNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            X = CreateInputValue
            (
                nameof(X),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();

            Y = CreateInputValue
            (
                nameof(Y),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();

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

            float x = X.GetValue().NumberValue;
            float y = Y.GetValue().NumberValue;

            meshEntity.Velocity = new Vector2(x, y);
            return Exit;
        }
    }
}