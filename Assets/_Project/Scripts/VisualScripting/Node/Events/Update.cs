using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class UpdateNode : EventNode
    {
        public UpdateNode() { }

        public override ScriptNode Create()
        {
            return new UpdateNode();
        }

        public override EventHook Hook => EventHook.Update;
    }
}