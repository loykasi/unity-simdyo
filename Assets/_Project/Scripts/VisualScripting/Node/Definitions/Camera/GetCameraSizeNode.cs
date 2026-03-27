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

        public override void Build()
        {
            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsConnectionDisabled = false,
                    IsLocalizationDisabled = true,
                    LocalizationKey = nameof(Value)
                }
            );
        }

        public ValueTransfer Get()
        {
            Camera camera = SceneManager.Instance.SceneCamera;
            return ValueTransfer.CreateNumber(camera.orthographicSize);
        }
    }
}