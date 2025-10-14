using UnityEngine;

namespace Loykas.Scripting
{
    public class OnClickedNode : EventNode
    {
        public OnClickedNode(string title) : base(title)
        {
        }

        public override EventHook Hook => EventHook.Clicked;
    }
}