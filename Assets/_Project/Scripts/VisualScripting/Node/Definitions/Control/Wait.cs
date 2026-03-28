using UnityEngine;

namespace Loykas.Scripting
{
    class WaitNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue WaitTime;

        private bool _isFirstFrame = true;
        private float _time;

        public override ScriptNode Create()
        {
            return new WaitNode();
        }

        public override void Reset()
        {
            _isFirstFrame = true;
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Wait);
            Exit = CreateOutputTrigger
            (
                nameof(Exit),
                new PortSettings
                {
                    HideLabel = true
                }
            );

            WaitTime = CreateInputValue
            (
                nameof(WaitTime),
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            ).UseInput();
        }

        private OutputTrigger Wait(NodeTask task)
        {
            if (_isFirstFrame)
            {
                _time = WaitTime.GetValue().NumberValue;
                _isFirstFrame = false;
            }

            if (_time > 0)
            {
                _time -= Time.deltaTime;
                return null;
            }
            _isFirstFrame = true;

            return Exit;
        }
    }
}