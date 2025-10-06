using System;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Data)]
    public class MakeBooleanContent : ScriptNodeContent
    {
        public override Type Type => typeof(MakeBooleanNode);
        public override ScriptNode Create() => new MakeBooleanNode();
    }

    public class MakeBooleanNode : MakeVariableNode
    {
        public MakeBooleanNode() : base(DataType.Boolean) { }
    }
}