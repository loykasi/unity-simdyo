using UnityEngine;

namespace Loykas.Scripting
{
    class GetGravityNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputValue Entity;
        public OutputValue Value;

        public GetGravityNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean), Get)
                    .UseGlobalLocalized();
        }

        public override ScriptNode Create()
        {
            return new GetGravityNode();
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return default(float);
            }

            return meshEntity.IsGravityEnabled;
        }
    }
}