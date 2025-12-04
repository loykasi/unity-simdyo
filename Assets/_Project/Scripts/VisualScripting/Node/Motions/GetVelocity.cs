using Unity.VisualScripting;
using UnityEngine;

namespace Loykas.Scripting
{
    class GetVelocityNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

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

        public override ScriptNode Create()
        {
            return new GetVelocityNode();
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