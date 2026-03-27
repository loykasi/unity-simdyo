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

        public override void Build()
        {            
            X = CreateOutputValue
            (
                nameof(X),
                GetX,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true,
                }
            );
            
            Y = CreateOutputValue
            (
                nameof(Y),
                GetY,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true,
                }
            );
        }

        public ValueTransfer GetX()
        {
            Camera camera = SceneManager.Instance.SceneCamera;
            return ValueTransfer.CreateNumber(camera.transform.position.x);
        }

        public ValueTransfer GetY()
        {
            Camera camera = SceneManager.Instance.SceneCamera;
            return ValueTransfer.CreateNumber(camera.transform.position.y);
        }
    }
}