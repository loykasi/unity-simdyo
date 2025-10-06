using System;

namespace Loykas.Scripting
{
    public class MakeVariableNode : ScriptNode
    {
        public InputValue Input;
        public OutputValue Output;

        public MakeVariableNode(DataType type) : base()
        {
            Input = InputValue(nameof(Input), ScriptDataType.Single(type)).UseInput().DisableConnection().HideLabel();
            Output = OutputValue(nameof(Output), ScriptDataType.Single(type), (vs) => Input.GetValue(vs)).HideLabel();
        }
    }
}