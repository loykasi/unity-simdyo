using System;
using UnityEngine;

namespace Loykas.Scripting
{
    public class MakeVariableNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Data;
        
        public InputValue Input;
        public OutputValue Output;

        public MakeVariableNode(DataType type) : base()
        {
            Input = CreateInputValue
            (
                nameof(Input),
                ScriptDataType.Single(type),
                new PortSettings
                {
                    IsConnectionDisabled = true,
                    HideLabel = true
                }
            )
            .UseInput();
                        
            Output = CreateOutputValue
            (
                nameof(Output),
                Get,
                ScriptDataType.Single(type),
                new PortSettings
                {
                    HideLabel = true
                }
            );
        }

        private ValueTransfer Get() => Input.GetValue();
    }
}