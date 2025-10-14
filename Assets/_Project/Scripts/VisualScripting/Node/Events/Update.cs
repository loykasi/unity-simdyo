using System;
using UnityEngine;

namespace Loykas.Scripting
{

    [ScriptNode(ScriptNodeCategory.Event)]
    public class UpdateNodeContent : ScriptNodeContent
    {
        public override Type Type => typeof(UpdateNode);
        public override ScriptNode Create() => new UpdateNode();
    }


    public class UpdateNode : EventNode
    {
        public UpdateNode() { }

        public UpdateNode(string title) : base(title)
        {
        }

        public override EventHook Hook => EventHook.Update;
    }
}