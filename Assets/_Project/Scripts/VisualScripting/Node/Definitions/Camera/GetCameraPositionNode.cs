using UnityEngine;

namespace Loykas.Scripting
{
    class GetCameraPositionNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Camera;

        public OutputValue X;
        public OutputValue Y;

        public override ScriptNode Create()
        {
            return new GetCameraPositionNode();
        }

        public GetCameraPositionNode()
        {
            X = OutputValue(nameof(X), ScriptDataType.Single(DataType.Number), GetX).NoLocalize();
            Y = OutputValue(nameof(Y), ScriptDataType.Single(DataType.Number), GetY).NoLocalize();
        }

        public object GetX()
        {
            Camera camera = SceneManager.Instance.SceneCamera;
            return camera.transform.position.x;
        }

        public object GetY()
        {
            Camera camera = SceneManager.Instance.SceneCamera;
            return camera.transform.position.y;
        }
    }
}