using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "StartNode", menuName = "Scriptable Objects/Visual Scripting/Node/Start")]
    public class StartNodeData : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new StartNode(Title);
        }
    }

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