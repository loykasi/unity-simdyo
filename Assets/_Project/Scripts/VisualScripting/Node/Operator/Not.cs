using UnityEngine;

namespace Loykas.Scripting
{
    class NotNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Operator;

        public InputValue A;

        public OutputValue Output;

        public override ScriptNode Create()
        {
            return new NotNode();
        }

        public NotNode()
        {
            A = InputValue(nameof(A), ScriptDataType.Single(DataType.Boolean)).UseInput();

            Output = OutputValue(nameof(Output), ScriptDataType.Single(DataType.Boolean), Get).HideLabel();
        }
        
        private object Get() => !A.GetValue<bool>();
    }
}