using UnityEngine;

namespace Loykas.Scripting
{
    class GetPositionNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputValue Entity;
        public OutputValue X;
        public OutputValue Y;

        public GetPositionNode()
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
            return new GetPositionNode();
        }

        private object GetX()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return entity.Position.x;
        }

        private object GetY()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return entity.Position.y;
        }
    }
}