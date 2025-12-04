using UnityEngine;

namespace Loykas.Scripting
{
    class SetTextureSlotNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetTextureSlotNode();
        }

        public SetTextureSlotNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Number)).UseInput();
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

            int slot = (int)Value.GetValue();
            entity.SetTexture(slot);
            return Exit;
        }
    }
}