using UnityEngine;

namespace Loykas.Scripting
{
    public class OnClickedNode : EventNode
    {
        public OnClickedNode() { }

        public override ScriptNode Create()
        {
            return new OnClickedNode();
        }

        public override EventHook Hook => EventHook.Clicked;
    }
}