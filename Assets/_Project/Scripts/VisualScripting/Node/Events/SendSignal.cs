using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "SendSignal", menuName = "Scriptable Objects/Visual Scripting/Node/SendSignal")]
    public class SendSignal : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new SendSignalNode(Title);
        }
    }

    class SendSignalNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Name;

        public SendSignalNode(string title) : base(title)
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