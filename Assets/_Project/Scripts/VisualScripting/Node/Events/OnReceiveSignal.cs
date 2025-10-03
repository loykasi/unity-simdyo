using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "OnReceiveSignal", menuName = "Scriptable Objects/Visual Scripting/Node/Receive Signal")]
    public class OnReceiveSignal : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new OnReceiveSignalNode(Title);
        }
    }

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