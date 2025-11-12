using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Look)]
    public class GetSizeContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(GetSizeNode);
        public override ScriptNode Create() => new GetSizeNode();
    }

    class GetSizeNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue Width;
        public OutputValue Height;

        public GetSizeNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Width = OutputValue(nameof(Width), GetWidth);
            Height = OutputValue(nameof(Height), GetHeight);
        }

        public object GetWidth()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            if (entity is not BoxEntity boxEntity)
            {
                return default(float);
            }

            return boxEntity.Width;
        }

        public object GetHeight()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            if (entity is not BoxEntity boxEntity)
            {
                return default(float);
            }

            return boxEntity.Height;
        }
    }
}