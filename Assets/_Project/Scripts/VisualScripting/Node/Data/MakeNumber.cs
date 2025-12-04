using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class MakeNumberNode : MakeVariableNode
    {
        public override ScriptNode Create()
        {
            return new MakeNumberNode();
        }

        public MakeNumberNode() : base(DataType.Number) { }
    }
}