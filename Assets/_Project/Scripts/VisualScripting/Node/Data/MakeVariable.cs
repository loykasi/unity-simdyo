using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class MakeVariableNode : ScriptNode
    {
        public InputValue Input;
        public OutputValue Output;

        public MakeVariableNode(DataType type) : base()
        {
            Input = InputValue(nameof(Input), ScriptDataType.Single(type))
                        .UseInput()
                        .DisableConnection()
                        .HideLabel();
                        
            Output = OutputValue(nameof(Output), ScriptDataType.Single(type), Get).HideLabel();
        }

        private object Get() => Input.GetValue();
    }
}