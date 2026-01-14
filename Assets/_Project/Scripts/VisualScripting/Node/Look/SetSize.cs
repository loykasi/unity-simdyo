using UnityEngine;

namespace Loykas.Scripting
{
    class SetSizeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Width;
        public InputValue Height;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetSizeNode();
        }

        public SetSizeNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Width = InputValue(nameof(Width), ScriptDataType.Single(DataType.Number))
                    .UseInput()
                    .UseGlobalLocalized();
                    
            Height = InputValue(nameof(Height), ScriptDataType.Single(DataType.Number))
                    .UseInput()
                    .UseGlobalLocalized();
            
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                    .HideLabel()
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

            if (entity is not BoxEntity boxEntity)
            {
                return Exit;
            }

            float width = (float)Width.GetValue();
            float height = (float)Height.GetValue();

            boxEntity.SetSize(width, height);
            return Exit;
        }
    }
}