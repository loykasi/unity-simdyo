using UnityEngine;

namespace Loykas.Scripting
{

    class GetAngleNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue Value;

        public GetAngleNode()
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

            return Flow.Entity.Angle;
        }
    }
}