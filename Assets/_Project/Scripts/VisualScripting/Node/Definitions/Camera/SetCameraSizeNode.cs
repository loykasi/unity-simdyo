using UnityEngine;

namespace Loykas.Scripting
{
    class SetCameraSizeNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Camera;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Value;

        public override ScriptNode Create()
        {
            return new SetCameraSizeNode();
        }

        public SetCameraSizeNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = OutputTrigger(nameof(Exit));

            Value = InputValue(nameof(Value), ScriptDataType.Single(DataType.Number)).UseInput()
                .UseGlobalLocalized();;
        }

        public OutputTrigger Set(NodeTask task)
        {
            Camera camera = SceneManager.Instance.SceneCamera;

            float size = Value.GetValue<float>();
            camera.orthographicSize = size;
            return Exit;
        }
    }
}