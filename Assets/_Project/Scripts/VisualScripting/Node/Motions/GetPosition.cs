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
        public InputValue Input;
        public OutputValue X;
        public OutputValue Y;

        public GetPositionNode(string title) : base(title)
        {
            Input = InputValue(nameof(Input), ScriptDataType.Single(DataType.Entity))
                        .UseInput()
                        .DisableConnection();

            X = OutputValue(nameof(X), GetX);
            Y = OutputValue(nameof(Y), GetY);
        }

        private object GetX(ScriptFlow vs)
        {
            SceneEntity entity = (SceneEntity)Input.GetValue(vs);

            return entity.Position.x;
        }

        private object GetY(ScriptFlow vs)
        {
            SceneEntity entity = (SceneEntity)Input.GetValue(vs);

            return entity.Position.y;
        }
    }
}