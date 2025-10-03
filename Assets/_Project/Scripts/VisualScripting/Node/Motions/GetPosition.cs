using Unity.VisualScripting;
using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "GetPosition", menuName = "Scriptable Objects/Visual Scripting/Node/Get Position")]
    public class GetPosition : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new GetPositionNode(Title);
        }
    }

    class GetPositionNode : ScriptNode
    {
        public InputValue Entity;
        public OutputValue X;
        public OutputValue Y;

        public GetPositionNode(string title) : base(title)
        {
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .UseInput()
                        .DisableConnection();

            X = OutputValue(nameof(X), GetX);
            Y = OutputValue(nameof(Y), GetY);
        }

        private object GetX(ScriptFlow vs)
        {
            SceneEntity entity = (SceneEntity)Entity.GetValue(vs);

            if (entity == null)
            {
                return default(float);
            }

            return entity.Position.x;
        }

        private object GetY(ScriptFlow vs)
        {
            SceneEntity entity = (SceneEntity)Entity.GetValue(vs);

            if (entity == null)
            {
                return default(float);
            }

            return entity.Position.y;
        }
    }
}