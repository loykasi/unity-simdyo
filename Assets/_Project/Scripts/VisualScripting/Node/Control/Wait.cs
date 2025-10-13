using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Control)]
    public class WaitNodeContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(WaitNode);
        public override ScriptNode Create() => new WaitNode();
    }

    class WaitNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue WaitTime;

        private bool _isStart;
        private float _time;

        public WaitNode()
        {
            Enter = InputTrigger(nameof(Enter), Wait);
            Exit = OutputTrigger(nameof(Exit));

            WaitTime = InputValue(nameof(WaitTime), ScriptDataType.Single(DataType.Number)).UseInput();
        }

        private OutputTrigger Wait(ScriptFlow flow)
        {
            if (!_isStart)
            {
                _time = (float)WaitTime.GetValue(flow);
                _isStart = true;
            }

            if (_isStart && _time > 0)
            {
                _time -= Time.deltaTime;
                return null;
            }
            _isStart = false;

            return Exit;
        }
    }
}