using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "Wait", menuName = "Scriptable Objects/Visual Scripting/Node/Wait")]
    public class Wait : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new WaitNode(Title);
        }
    }

    class WaitNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue WaitTime;

        private bool _isStart;
        private float _time;

        public WaitNode(string title) : base(title)
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