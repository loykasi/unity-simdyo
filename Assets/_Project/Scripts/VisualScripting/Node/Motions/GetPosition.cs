using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Motion)]
    public class GetPositionContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(GetPositionNode);
        public override ScriptNode Create() => new GetPositionNode();
    }

    class GetPositionNode : ScriptNode
    {
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