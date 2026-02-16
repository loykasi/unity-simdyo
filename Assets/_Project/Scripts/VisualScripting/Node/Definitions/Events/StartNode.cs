using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class StartNode : EventNode
    {
        public StartNode()
        {
        }

        public override ScriptNode Create()
        {
            return new StartNode();
        }

        public override EventHook Hook => EventHook.Start;
    }
}