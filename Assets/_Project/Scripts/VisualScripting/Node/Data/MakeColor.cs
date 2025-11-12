using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class MakeColorContent : ScriptNodeContent
    {
        public override Type Type => typeof(MakeColorNode);
        public override ScriptNode Create() => new MakeColorNode();
    }

    public class MakeColorNode : MakeVariableNode
    {
        public MakeColorNode() : base(DataType.Color) { }
    }
}