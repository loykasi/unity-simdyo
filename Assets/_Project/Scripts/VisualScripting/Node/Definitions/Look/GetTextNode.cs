using UnityEngine;

namespace Loykas.Scripting
{
    class GetTextNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Text;

        public override ScriptNode Create()
        {
            return new GetTextNode();
        }

        public GetTextNode()
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();
                        
            Text = OutputValue(nameof(Text), ScriptDataType.Single(DataType.String), Get)
                    .UseGlobalLocalized();
        }

        public object Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return default(float);
            }

            if (entity is not BoxEntity boxEntity)
            {
                return default(float);
            }

            return boxEntity.TextBox.Text;
        }
    }
}