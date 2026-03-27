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

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger
            (
                nameof(Exit),
                PortSettings.Default
            );

            Value = CreateInputValue
            (
                nameof(Value),
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            ).UseInput();
        }

        public OutputTrigger Set(NodeTask task)
        {
            Camera camera = SceneManager.Instance.SceneCamera;

            float size = Value.GetValue().NumberValue;
            camera.orthographicSize = size;
            return Exit;
        }
    }
}