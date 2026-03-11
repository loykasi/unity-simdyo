using UnityEngine;

namespace Loykas.Scripting
{
    class GetColorNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Color;

        public override ScriptNode Create()
        {
            return new GetColorNode();
        }

        public GetColorNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Color = OutputValue(nameof(Color), ScriptDataType.Single(DataType.Color), Get)
                    .UseGlobalLocalized();
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return default(float);
            }

            return meshEntity.CurrentColor;
        }
    }
}