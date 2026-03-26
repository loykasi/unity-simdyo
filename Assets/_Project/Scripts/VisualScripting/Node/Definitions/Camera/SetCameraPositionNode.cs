using UnityEngine;

namespace Loykas.Scripting
{
    class SetCameraPositionNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Camera;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue X;
        public InputValue Y;

        public override ScriptNode Create()
        {
            return new SetCameraPositionNode();
        }

        public SetCameraPositionNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit), PortSettings.Default);

            X = CreateInputValue
            (
                nameof(X),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            ).UseInput();
            
            Y = CreateInputValue
            (
                nameof(Y),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            ).UseInput();
        }

        public OutputTrigger Set(NodeTask task)
        {
            Camera camera = SceneManager.Instance.SceneCamera;

            float x = X.GetValue().NumberValue;
            float y = Y.GetValue().NumberValue;
            camera.transform.position = new Vector3(x, y, camera.transform.position.z);
            return Exit;
        }
    }
}