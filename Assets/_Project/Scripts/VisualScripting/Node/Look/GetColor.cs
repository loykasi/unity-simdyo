using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Look)]
    public class GetColorContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(GetColorNode);
        public override ScriptNode Create() => new GetColorNode();
    }

    class GetColorNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue Color;

        public GetColorNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Color = OutputValue(nameof(Color), ScriptDataType.Single(DataType.Color), Get);
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return Flow.Entity.CurrentColor;
        }
    }
}