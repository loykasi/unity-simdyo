using UnityEngine;

namespace Loykas.Scripting
{
    class SetColliderNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public SetColliderNode()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Boolean)).UseInput();
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
        }

        public OutputTrigger Set()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return Exit;
            }

            bool enable = (bool)Value.GetValue();
            Flow.Entity.SetCollider(enable);
            return Exit;
        }
    }
}