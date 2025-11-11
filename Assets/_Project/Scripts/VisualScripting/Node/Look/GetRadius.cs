using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Look)]
    public class GetRadiusContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(GetRadiusNode);
        public override ScriptNode Create() => new GetRadiusNode();
    }

    class GetRadiusNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue Value;

        public GetRadiusNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Value = OutputValue(nameof(Value), Get);
        }

        public object Get(ScriptFlow vs)
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