using UnityEngine;

namespace Loykas.Scripting
{
    class GetColliderNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputValue Entity;
        public OutputValue Value;

        public GetColliderNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean), Get);
        }

        public override ScriptNode Create()
        {
            return new GetColliderNode();
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return Flow.Entity.IsColliderEnabled;
        }
    }
}