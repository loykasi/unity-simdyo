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

        public WaitNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Wait);
            Exit = OutputTrigger(nameof(Exit));

            WaitTime = InputValue(nameof(WaitTime), ScriptDataType.Single(DataType.Number)).UseInput();
        }

        private OutputTrigger Wait(NodeTask task)
        {
            if (_isFirstFrame)
            {
                _time = (float)WaitTime.GetValue();
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