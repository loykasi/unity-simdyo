using UnityEngine;

namespace Loykas.Scripting
{
    public class OnClickedNode : EventNode
    {
        public OnClickedNode()
        {
        }

        public override EventHook Hook => EventHook.Clicked;
    }
}