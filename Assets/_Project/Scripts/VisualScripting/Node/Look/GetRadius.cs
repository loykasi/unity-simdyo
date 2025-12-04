using UnityEngine;

namespace Loykas.Scripting
{
    class GetRadiusNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetRadiusNode();
        }

        public GetRadiusNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Value = OutputValue(nameof(Value), Get);
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            if (entity is not CircleEntity circleEntity)
            {
                return default(float);
            }

            return circleEntity.Radius;
        }
    }
}