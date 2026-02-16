using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class StartAsCloneNode : EventNode
    {
        public override bool CanUseGlobal => false;

        public StartAsCloneNode()
        {
        }

        public override ScriptNode Create()
        {
            return new StartAsCloneNode();
        }

        public override EventHook Hook => EventHook.StartAsClone;
    }
}