using UnityEngine;

namespace Loykas.Scripting
{
    class SetRadiusNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetRadiusNode();
        }

        public SetRadiusNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Number))
                    .UseInput()
                    .UseGlobalLocalized();

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

            if (entity is not CircleEntity circleEntity)
            {
                return Exit;
            }

            float angle = (float)Value.GetValue();
            circleEntity.SetRadius(angle);
            return Exit;
        }
    }
}