using UnityEngine;

namespace Loykas.Scripting
{
    class SetColliderNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetColliderNode();
        }

        public SetColliderNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean)).UseInput();
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
        }

        public OutputTrigger Set(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return Exit;
            }

            bool enable = (bool)Value.GetValue();
            meshEntity.IsColliderEnabled = enable;
            return Exit;
        }
    }
}