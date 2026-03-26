using UnityEngine;

namespace Loykas.Scripting
{
    class SetZDepthNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetZDepthNode();
        }

        public SetZDepthNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            )
            .UseInput();

            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    HideLabel = true
                }
            )
            .UseInput()
            .NullMeanSelf();
        }

        public OutputTrigger Set(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return Exit;
            }

            float depth = Value.GetValue().NumberValue;
            entity.ZDepth = (int)depth;
            return Exit;
        }
    }
}