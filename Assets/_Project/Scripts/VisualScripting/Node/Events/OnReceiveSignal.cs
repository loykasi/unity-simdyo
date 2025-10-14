using UnityEngine;

namespace Loykas.Scripting
{
    public class OnReceiveSignalNode : EventNode
    {
        public InputValue Name;
        
        public OnReceiveSignalNode(string title) : base(title)
        {
            Name = InputValue(nameof(Name), ScriptDataType.Single(DataType.String)).UseInput().HideLabel().DisableConnection();
        }

        public override EventHook Hook => EventHook.Signal;
    }
}