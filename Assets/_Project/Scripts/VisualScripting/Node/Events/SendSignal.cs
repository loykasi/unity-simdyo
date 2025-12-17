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
            Enter = CreateInputTrigger(nameof(Enter), SendSignal);
            Exit = OutputTrigger(nameof(Exit));
            Name = InputValue(nameof(Name), ScriptDataType.Single(DataType.String)).UseInput().HideLabel().DisableConnection();
        }

        public override ScriptNode Create()
        {
            return new SendSignalNode();
        }

        private OutputTrigger SendSignal()
        {
            string signalName = (string)Name.GetValue();
            SignalSystem.Instance.SendSignal(signalName);
            return Exit;
        }
    }
}