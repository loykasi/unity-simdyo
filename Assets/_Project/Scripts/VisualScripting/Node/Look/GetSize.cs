using UnityEngine;

namespace Loykas.Scripting
{
    class GetSizeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Width;
        public OutputValue Height;

        public override ScriptNode Create()
        {
            return new GetSizeNode();
        }

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