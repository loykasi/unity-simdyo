using UnityEngine;

namespace Loykas.Scripting
{
    class SendSignalNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Name;

        public SendSignalNode()
        {
            Enter = InputTrigger(nameof(Enter), SendSignal);
            Exit = OutputTrigger(nameof(Exit)).HideLabel();
            Name = InputValue(nameof(Name), ScriptDataType.Single(DataType.String)).UseInput().HideLabel().DisableConnection();
        }

        private OutputTrigger SendSignal(ScriptFlow flow)
        {
            string signalName = (string)Name.GetValue(flow);
            SignalSystem.Instance.SendSignal(signalName);
            return Exit;
        }
    }
}