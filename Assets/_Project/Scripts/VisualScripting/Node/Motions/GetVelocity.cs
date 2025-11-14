using Unity.VisualScripting;
using UnityEngine;

namespace Loykas.Scripting
{
    class GetVelocityNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue X;
        public OutputValue Y;

        public GetVelocityNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();

            X = OutputValue(nameof(X), ScriptDataType.Single(DataType.Number), GetX);
            Y = OutputValue(nameof(Y), ScriptDataType.Single(DataType.Number), GetY);
        }

        private object GetX()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return entity.Velocity.x;
        }

        private object GetY()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return entity.Velocity.y;
        }
    }
}