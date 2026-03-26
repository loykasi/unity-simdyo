using UnityEngine;

namespace Loykas.Scripting
{
    public class OnReceiveSignalNode : EventNode
    {
        public InputValue Name;
        
        public OnReceiveSignalNode()
        {
            Name = CreateInputValue
            (
                nameof(Name),
                ScriptDataType.Single(DataType.String),
                new PortSettings
                {
                    HideLabel = true,
                    IsConnectionDisabled = true
                }
            ).UseInput();
        }

        public override ScriptNode Create()
        {
            return new OnReceiveSignalNode();
        }

        public override EventHook Hook => EventHook.Signal;
    }
}