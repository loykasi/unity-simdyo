using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class MakeStringNode : MakeVariableNode
    {
        public override ScriptNode Create()
        {
            return new MakeStringNode();
        }

        public MakeStringNode() : base(DataType.String) { }
    }
}