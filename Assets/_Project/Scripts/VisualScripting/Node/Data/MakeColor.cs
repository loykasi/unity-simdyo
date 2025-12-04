using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class MakeColorNode : MakeVariableNode
    {
        public override ScriptNode Create()
        {
            return new MakeColorNode();
        }

        public MakeColorNode() : base(DataType.Color) { }
    }
}