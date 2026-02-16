using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class MakeBooleanNode : MakeVariableNode
    {
        public override ScriptNode Create()
        {
            return new MakeBooleanNode();
        }
        
        public MakeBooleanNode() : base(DataType.Boolean) { }
    }
}