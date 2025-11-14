using UnityEngine;

namespace Loykas.Scripting
{

    class GetGravityNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue Value;

        public GetGravityNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean), Get);
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