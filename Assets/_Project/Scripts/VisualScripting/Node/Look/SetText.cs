using UnityEngine;

namespace Loykas.Scripting
{
    class SetTextNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Text;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetTextNode();
        }

        public SetTextNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Text = InputValue(nameof(Text), ScriptDataType.Single(DataType.String))
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

            if (entity is not BoxEntity boxEntity)
            {
                return Exit;
            }

            string text = (string)Text.GetValue();

            boxEntity.TextBox.Text = text;
            return Exit;
        }
    }
}