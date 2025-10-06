using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "Update", menuName = "Scriptable Objects/Visual Scripting/Node/Update")]
    public class Update : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new UpdateNode(Title);
        }
    }

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