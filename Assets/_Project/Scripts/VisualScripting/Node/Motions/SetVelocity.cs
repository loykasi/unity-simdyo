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

        public SetVelocityNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            X = InputValue(nameof(X), ScriptDataType.Single(DataType.Number)).UseInput();
            Y = InputValue(nameof(Y), ScriptDataType.Single(DataType.Number)).UseInput();
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
        }

        public OutputTrigger Set()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return Exit;
            }

            float x = (float)X.GetValue();
            float y = (float)Y.GetValue();

            entity.Velocity = new Vector2(x, y);
            return Exit;
        }
    }
}