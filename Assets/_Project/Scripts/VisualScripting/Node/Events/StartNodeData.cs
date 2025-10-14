using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Event)]
    public class StartNodeContent : ScriptNodeContent
    {
        public override Type Type => typeof(StartNode);
        public override ScriptNode Create() => new StartNode();
    }

    public class StartNode : EventNode
    {
        public StartNode()
        {
        }

        public StartNode(string title) : base(title)
        {
        }


        public override EventHook Hook => EventHook.Start;
    }
}