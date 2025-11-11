using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Look)]
    public class SetRadiusContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(SetRadiusNode);
        public override ScriptNode Create() => new SetRadiusNode();
    }

    class SetRadiusNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public SetRadiusNode()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Number)).UseInput();
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
        }

        public OutputTrigger Set(ScriptFlow vs)
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

            float angle = (float)Value.GetValue(vs);
            circleEntity.SetRadius(angle);
            return Exit;
        }
    }
}