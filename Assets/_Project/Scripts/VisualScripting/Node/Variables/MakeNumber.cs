using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class MakeNumberContent : ScriptNodeContent
    {
        public override Type Type => typeof(MakeNumberNode);
        public override ScriptNode Create() => new MakeNumberNode();
    }

    public class MakeNumberNode : MakeVariableNode
    {
        public MakeNumberNode() : base(DataType.Number) { }
    }
}