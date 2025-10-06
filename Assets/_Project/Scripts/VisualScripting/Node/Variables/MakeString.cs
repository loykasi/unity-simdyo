using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class MakeStringContent : ScriptNodeContent
    {
        public override Type Type => typeof(MakeStringNode);
        public override ScriptNode Create() => new MakeStringNode();
    }

    public class MakeStringNode : MakeVariableNode
    {
        public MakeStringNode() : base(DataType.String) { }
    }
}