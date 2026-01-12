using UnityEngine;

namespace Loykas.Scripting
{
    class GetCameraSizeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Camera;

        public OutputValue Value;

        public override ScriptNode Create()
        {
            return new GetCameraSizeNode();
        }

        public GetCameraSizeNode()
        {
            Value = OutputValue(nameof(Value), ScriptDataType.Single(DataType.Number), Get);
        }

        public object Get()
        {
            Camera camera = SceneManager.Instance.SceneCamera;
            return camera.orthographicSize;
        }
    }
}