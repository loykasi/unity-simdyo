using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Look)]
    public class SetColorContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(SetColorNode);
        public override ScriptNode Create() => new SetColorNode();
    }

    class SetColorNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;
        public InputValue Entity;

        public SetColorNode()
        {
            Enter = InputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Color)).UseInput();
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

            ColorHSV color = (ColorHSV)Value.GetValue();
            Flow.Entity.SetColor(color.ToUnityColor());
            return Exit;
        }
    }
}