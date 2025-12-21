using UnityEngine;

namespace Loykas.Scripting
{
    class GetTextureSlotNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetTextureSlotNode();
        }

        public GetTextureSlotNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                    .HideLabel()
                    .UseInput()
                    .NullMeanSelf();
                        
            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Number), Get)
                    .UseGlobalLocalized();
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            return entity.TextureSlot;
        }
    }
}