using UnityEngine;

namespace Loykas.Scripting
{
    class SetVelocityNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue X;
        public InputValue Y;
        public InputValue Entity;

        public SetVelocityNode()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            X = InputValue(nameof(X), ScriptDataType.Single(DataType.Number)).UseInput();
            Y = InputValue(nameof(Y), ScriptDataType.Single(DataType.Number)).UseInput();
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
        }

        public OutputTrigger Set(ScriptFlow vs)
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return Exit;
            }

            float x = (float)X.GetValue(vs);
            float y = (float)Y.GetValue(vs);

            vs.Entity.Velocity = new Vector2(x, y);
            return Exit;
        }
    }
}